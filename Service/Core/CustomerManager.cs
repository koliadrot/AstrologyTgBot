namespace Service.Core
{
    using AutoMapper;
    using Data.Core;
    using Data.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Service.Abstract;
    using Service.ViewModels;
    using System;
    using Telegram.Bot.Types;

    /// <summary>
    /// Менеджер клиентов
    /// </summary>
    public class CustomerManager : ICustomerManager, IDisposable
    {
        private readonly ApplicationDbContext _bonusDbContext;
        private IMapper _mapper;

        public CustomerManager()
        {
            _bonusDbContext = new ApplicationDbContext();
            _mapper = new MapperConfig().GetMapper();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _bonusDbContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

        public bool ExistTelegram(long telegramId) => _bonusDbContext.Clients.Any(client => client.TelegramId == telegramId.ToString());
        public bool ExistTelegram(string userName) => _bonusDbContext.Clients.Any(client => client.UserName == userName);

        public ClientViewModel CreateClient(ClientViewModel clientViewModel)
        {
            Client client = _bonusDbContext.Clients.FirstOrDefault(x => x.ClientId == clientViewModel.ClientId);
            if (client == null)
            {
                client = _mapper.Map<Client>(clientViewModel);
                _bonusDbContext.Clients.Add(client);
                _bonusDbContext.SaveChanges();
                clientViewModel.ClientId = client.ClientId;
            }
            return clientViewModel;
        }

        public ClientViewModel UpdateClient(ClientViewModel clientViewModel)
        {
            Client client = _bonusDbContext.Clients.FirstOrDefault(x => x.ClientId == clientViewModel.ClientId);
            if (client != null)
            {
                client = _mapper.Map<Client>(clientViewModel);
                _bonusDbContext.Clients.Update(client);
                _bonusDbContext.SaveChanges();
            }
            return clientViewModel;
        }

        public void DeleteClient(ClientViewModel clientViewModel)
        {
            Client client = _bonusDbContext.Clients.FirstOrDefault(x => x.ClientId == clientViewModel.ClientId);
            if (client != null)
            {
                _bonusDbContext.Clients.Remove(client);
                _bonusDbContext.SaveChanges();
            }
        }

        public ClientViewModel GetClientByTelegram(string userId)
        {
            Client client = _bonusDbContext.Clients.Include("ClientMediaInfo").FirstOrDefault(x => x.TelegramId == userId);
            ClientViewModel viewModel = new ClientViewModel();
            if (client != null)
            {
                client.ClientMediaInfo.ClientPhotoInfos = _bonusDbContext.ClientPhotoInfos.Where(x => x.ClientMediaInfoId == client.ClientMediaInfo.ClientMediaInfoId).ToList();
                client.ClientMediaInfo.ClientVideoInfos = _bonusDbContext.ClientVideoInfos.Where(x => x.ClientMediaInfoId == client.ClientMediaInfo.ClientMediaInfoId).ToList();
                viewModel = _mapper.Map<ClientViewModel>(client);
            }
            return viewModel;
        }

        public ClientViewModel GetClientByUserName(string userName)
        {
            Client client = _bonusDbContext.Clients.FirstOrDefault(x => x.UserName == userName);
            return client != null ? GetClientByTelegram(client.TelegramId) : default;
        }

        public List<long> GetIdTelegramsByUserName(string userName) => _bonusDbContext.Clients.Where(x => x.UserName == userName).Select(x => long.Parse(x.TelegramId)).ToList();

        public List<InputMedia> GetMediaFilesByUserId(long userId)
        {
            List<InputMedia> files = new List<InputMedia>();
            var user = GetClientByTelegram(userId.ToString());

            if (user.ClientMediaInfo.ClientPhotoInfos.Any())
            {
                files.AddRange(user.ClientMediaInfo.ClientPhotoInfos.Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaPhoto(file)));
            }

            if (user.ClientMediaInfo.ClientVideoInfos.Any())
            {
                files.AddRange(user.ClientMediaInfo.ClientVideoInfos.Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaVideo(file)));
            }
            return files;
        }
    }
}
