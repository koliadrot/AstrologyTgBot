namespace Service.Core
{
    using AutoMapper;
    using Data.Core;
    using Data.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Service.Abstract;
    using Service.Enums;
    using Service.Support;
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

        private const float DALAY_TO_NOTIFY_NEW_LIKES = 1800;

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

        public List<ClientViewModel> GetClients(params ClientViewModel?[] excludeClients)
        {
            var clientsId = GetTelegramClientsId().ToList();
            List<ClientViewModel> clients = new List<ClientViewModel>();
            clientsId.ForEach(id => clients.Add(GetClientByTelegram(id)));
            if (excludeClients != null)
            {
                clients = clients.Where(x => excludeClients.Any(client => client?.ClientId != x.ClientId)).ToList();
            }
            return clients;
        }

        public IQueryable<string> GetTelegramClientsId() => _bonusDbContext.Clients.Select(x => x.TelegramId);

        public ClientViewModel GetClientByTelegram(string userId)
        {
            Client client = _bonusDbContext.Clients.Include(x => x.ClientMediaInfo).Include(x => x.ClientMatchInfo).FirstOrDefault(x => x.TelegramId == userId);
            ClientViewModel viewModel = new ClientViewModel();
            if (client != null)
            {
                if (client.ClientMediaInfo != null)
                {
                    client.ClientMediaInfo.ClientPhotoInfos = _bonusDbContext.ClientPhotoInfos.Where(x => x.ClientMediaInfoId == client.ClientMediaInfo.ClientMediaInfoId).ToList();
                    client.ClientMediaInfo.ClientVideoInfos = _bonusDbContext.ClientVideoInfos.Where(x => x.ClientMediaInfoId == client.ClientMediaInfo.ClientMediaInfoId).ToList();
                    client.ClientMediaInfo.ClientVideoNoteInfos = _bonusDbContext.ClientVideoNoteInfos.Where(x => x.ClientMediaInfoId == client.ClientMediaInfo.ClientMediaInfoId).ToList();
                }
                else
                {
                    ClientMediaInfo clientMediaInfo = new ClientMediaInfo()
                    {
                        ClientId = client.ClientId
                    };
                    _bonusDbContext.ClientMediaInfos.Add(clientMediaInfo);
                    _bonusDbContext.SaveChanges();
                    client.ClientMediaInfo = clientMediaInfo;
                }
                if (client.ClientMatchInfo != null)
                {
                    client.ClientMatchInfo.UncheckedClientMatchs = _bonusDbContext.UncheckedClientMatchs.Include(x => x.ClientMatchUncheckedVideoNoteInfo).Include(x => x.ClientMatchUncheckedVideoInfo).Where(x => x.ClientMatchInfoId == client.ClientMatchInfo.ClientMatchInfoId).ToList();
                }
                else
                {
                    ClientMatchInfo clientMatchInfo = new ClientMatchInfo()
                    {
                        ClientId = client.ClientId
                    };
                    _bonusDbContext.ClientMatchInfos.Add(clientMatchInfo);
                    _bonusDbContext.SaveChanges();
                    client.ClientMatchInfo = clientMatchInfo;
                }
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

        public InputMediaCustom? GetAvatarFileByUserId(long userId)
        {
            var user = GetClientByTelegram(userId.ToString());
            InputMediaCustom file = default;

            if (user.ClientMediaInfo.ClientPhotoInfos.Any())
            {
                file = user.ClientMediaInfo.ClientPhotoInfos.Where(x => x.IsAvatar == true).Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.Photo)).FirstOrDefault();
            }

            if (file == null && user.ClientMediaInfo.ClientVideoInfos.Any())
            {
                file = user.ClientMediaInfo.ClientVideoInfos.Where(x => x.IsAvatar == true).Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.Video)).FirstOrDefault();
            }

            if (file == null && user.ClientMediaInfo.ClientVideoNoteInfos.Any())
            {
                file = user.ClientMediaInfo.ClientVideoNoteInfos.Where(x => x.IsAvatar == true).Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.VideoNote)).FirstOrDefault();
            }

            if (file == null && user.ClientMediaInfo.ClientPhotoInfos.Any())
            {
                file = user.ClientMediaInfo.ClientPhotoInfos.Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.Photo)).FirstOrDefault();
            }

            if (file == null && user.ClientMediaInfo.ClientVideoInfos.Any())
            {
                file = user.ClientMediaInfo.ClientVideoInfos.Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.Video)).FirstOrDefault();
            }

            if (file == null && user.ClientMediaInfo.ClientVideoNoteInfos.Any())
            {
                file = user.ClientMediaInfo.ClientVideoNoteInfos.Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.VideoNote)).FirstOrDefault();
            }


            return file;
        }

        public List<InputMediaCustom> GetMediaFilesByUserId(long userId, bool isIncludeAvatar = false)
        {
            var user = GetClientByTelegram(userId.ToString());
            return GetMediaFilesByUserId(user);
        }

        public List<InputMediaCustom> GetMediaFilesByUserId(ClientViewModel user, bool isIncludeAvatar = false)
        {
            List<InputMediaCustom> files = new List<InputMediaCustom>();

            if (isIncludeAvatar)
            {
                if (user.ClientMediaInfo.ClientPhotoInfos.Any())
                {
                    files.AddRange(user.ClientMediaInfo.ClientPhotoInfos.Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.Photo)));
                }

                if (user.ClientMediaInfo.ClientVideoInfos.Any())
                {
                    files.AddRange(user.ClientMediaInfo.ClientVideoInfos.Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.Video)));
                }

                if (user.ClientMediaInfo.ClientVideoNoteInfos.Any())
                {
                    files.AddRange(user.ClientMediaInfo.ClientVideoNoteInfos.Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.VideoNote)));
                }
            }
            else
            {
                if (user.ClientMediaInfo.ClientPhotoInfos.Where(x => x.IsAvatar == false || x.IsAvatar == null).Any())
                {
                    files.AddRange(user.ClientMediaInfo.ClientPhotoInfos.Where(x => x.IsAvatar == false || x.IsAvatar == null).Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.Photo)));
                }

                if (user.ClientMediaInfo.ClientVideoInfos.Where(x => x.IsAvatar == false || x.IsAvatar == null).Any())
                {
                    files.AddRange(user.ClientMediaInfo.ClientVideoInfos.Where(x => x.IsAvatar == false || x.IsAvatar == null).Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.Video)));
                }

                if (user.ClientMediaInfo.ClientVideoNoteInfos.Where(x => x.IsAvatar == false || x.IsAvatar == null).Any())
                {
                    files.AddRange(user.ClientMediaInfo.ClientVideoNoteInfos.Where(x => x.IsAvatar == false || x.IsAvatar == null).Select(x => new InputFileId(x.FileId)).Select(file => new InputMediaCustom(file, MediaType.VideoNote)));
                }
            }
            return files;
        }

        public void CreateClientMatch(ClientMatchUncheckedViewModel clientMatchUncheckedViewModel)
        {
            var clientMatchInfo = _bonusDbContext.ClientMatchInfos.FirstOrDefault(x => x.ClientMatchInfoId == clientMatchUncheckedViewModel.ClientMatchInfoId);
            if (clientMatchInfo != null)
            {
                if (clientMatchUncheckedViewModel.MatchType == MatchType.Dislike.ToString())
                {
                    clientMatchInfo.Dislikes += 1;
                }
                else
                {
                    if (clientMatchUncheckedViewModel.MatchType == MatchType.Like.ToString())
                    {
                        clientMatchInfo.Likes += 1;
                    }
                    else if (clientMatchUncheckedViewModel.MatchType == MatchType.LoveLetter.ToString())
                    {
                        clientMatchInfo.LetterLikes += 1;
                    }
                    //NOTE:Если копится определенное кол-во или время прошло больше чем надо с прошлого раза, то слать уведомление об этом.
                    //clientMatchInfo.NewLikes += 1;
                    //clientMatchInfo.LastShowMatches = clientMatchInfo.LastShowMatches != null ? clientMatchInfo.LastShowMatches : DateTime.Now;
                }


                if (clientMatchUncheckedViewModel.AnswearDateMatch == null)
                {
                    ClientMatchUnchecked clientMatch = _mapper.Map<ClientMatchUnchecked>(clientMatchUncheckedViewModel);
                    _bonusDbContext.UncheckedClientMatchs.Add(clientMatch);
                    if (clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo != null)
                    {
                        ClientMatchUncheckedVideoNoteInfo clientMatchUncheckedVideoNoteInfo = _mapper.Map<ClientMatchUncheckedVideoNoteInfo>(clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo);
                        clientMatch.ClientMatchUncheckedVideoNoteInfo = clientMatchUncheckedVideoNoteInfo;
                        _bonusDbContext.ClientMatchUncheckedVideoNoteInfos.Add(clientMatchUncheckedVideoNoteInfo);
                    }

                    if (clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo != null)
                    {
                        ClientMatchUncheckedVideoInfo clientMatchUncheckedVideoInfo = _mapper.Map<ClientMatchUncheckedVideoInfo>(clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo);
                        clientMatch.ClientMatchUncheckedVideoInfo = clientMatchUncheckedVideoInfo;
                        _bonusDbContext.ClientMatchUncheckedVideoInfos.Add(clientMatchUncheckedVideoInfo);
                    }
                }
                else
                {
                    ClientMatchChecked clientMatchChecked = new ClientMatchChecked()
                    {
                        ClientMatchInfoId = clientMatchUncheckedViewModel.ClientMatchInfoId,
                        DateMatch = clientMatchUncheckedViewModel.DateMatch,
                        MatchType = clientMatchUncheckedViewModel.MatchType,
                        MatchTelegramId = clientMatchUncheckedViewModel.MatchTelegramId,
                        LoveLetterText = clientMatchUncheckedViewModel.LoveLetterText,
                        AnswearDateMatch = clientMatchUncheckedViewModel.AnswearDateMatch,
                        AnswearMatchType = clientMatchUncheckedViewModel.AnswearMatchType
                    };
                    _bonusDbContext.CheckedClientMatchs.Add(clientMatchChecked);
                    _bonusDbContext.SaveChanges();

                    if (clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo != null)
                    {
                        ClientMatchCheckedVideoNoteInfo clientMatchCheckedVideoNoteInfo = new ClientMatchCheckedVideoNoteInfo()
                        {
                            ClientMatchCheckedId = clientMatchChecked.ClientMatchCheckedId,
                            FileId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.FileId,
                            FileUniqueId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.FileId,
                            FileSize = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.FileSize,
                            Length = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.Length,
                            Duration = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.Duration,
                            VideoNote = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.VideoNote,
                            ThumbnailFileId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.ThumbnailFileId,
                            ThumbnailFileUniqueId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.ThumbnailFileUniqueId,
                            ThumbnailFileSize = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.ThumbnailFileSize,
                            ThumbnailWidth = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.ThumbnailWidth,
                            ThumbnailHeight = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.ThumbnailHeight,
                            Thumbnail = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.Thumbnail,
                        };
                        clientMatchChecked.ClientMatchCheckedVideoNoteInfo = clientMatchCheckedVideoNoteInfo;
                        _bonusDbContext.ClientMatchCheckedVideoNoteInfos.Add(clientMatchCheckedVideoNoteInfo);
                    }

                    if (clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo != null)
                    {
                        ClientMatchCheckedVideoInfo clientMatchCheckedVideoInfo = new ClientMatchCheckedVideoInfo()
                        {
                            ClientMatchCheckedId = clientMatchChecked.ClientMatchCheckedId,
                            FileId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.FileId,
                            FileUniqueId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.FileId,
                            FileSize = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.FileSize,
                            Width = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.Width,
                            Height = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.Height,
                            MimeType = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.MimeType,
                            Duration = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.Duration,
                            Video = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.Video,
                            ThumbnailFileId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.ThumbnailFileId,
                            ThumbnailFileUniqueId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.ThumbnailFileUniqueId,
                            ThumbnailFileSize = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.ThumbnailFileSize,
                            ThumbnailWidth = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.ThumbnailWidth,
                            ThumbnailHeight = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.ThumbnailHeight,
                            Thumbnail = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.Thumbnail,
                        };
                        clientMatchChecked.ClientMatchCheckedVideoInfo = clientMatchCheckedVideoInfo;
                        _bonusDbContext.ClientMatchCheckedVideoInfos.Add(clientMatchCheckedVideoInfo);
                    }
                }
                _bonusDbContext.SaveChanges();
            }
        }

        public void UpdateClientMatch(ClientMatchUncheckedViewModel clientMatchUncheckedViewModel)
        {
            var clientMatchInfo = _bonusDbContext.ClientMatchInfos.FirstOrDefault(x => x.ClientMatchInfoId == clientMatchUncheckedViewModel.ClientMatchInfoId);
            if (clientMatchInfo != null)
            {
                ClientMatchUnchecked clientMatch = _bonusDbContext.UncheckedClientMatchs.Include(x => x.ClientMatchUncheckedVideoInfo).Include(x => x.ClientMatchUncheckedVideoNoteInfo).FirstOrDefault(x => x.ClientMatchUncheckedId == clientMatchUncheckedViewModel.ClientMatchUncheckedId);
                if (clientMatch != null)
                {
                    if (clientMatchUncheckedViewModel.AnswearMatchType == MatchType.Dislike.ToString())
                    {
                        clientMatchInfo.Dislikes += 1;
                    }
                    else if (clientMatchUncheckedViewModel.AnswearMatchType == MatchType.Like.ToString())
                    {
                        clientMatchInfo.Likes += 1;
                    }
                    //clientMatchInfo.NewLikes -= 1;
                    //clientMatchInfo.LastShowMatches = DateTime.Now;
                    //if (clientMatchInfo.NewLikes <= 0)
                    //{
                    //    clientMatchInfo.NewLikes = 0;
                    //    clientMatchInfo.LastShowMatches = null;
                    //}

                    ClientMatchChecked clientMatchChecked = new ClientMatchChecked()
                    {
                        ClientMatchInfoId = clientMatchUncheckedViewModel.ClientMatchInfoId,
                        DateMatch = clientMatchUncheckedViewModel.DateMatch,
                        MatchType = clientMatchUncheckedViewModel.MatchType,
                        MatchTelegramId = clientMatchUncheckedViewModel.MatchTelegramId,
                        LoveLetterText = clientMatchUncheckedViewModel.LoveLetterText,
                        AnswearDateMatch = clientMatchUncheckedViewModel.AnswearDateMatch,
                        AnswearMatchType = clientMatchUncheckedViewModel.AnswearMatchType
                    };
                    _bonusDbContext.CheckedClientMatchs.Add(clientMatchChecked);
                    _bonusDbContext.SaveChanges();

                    if (clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo != null)
                    {
                        ClientMatchCheckedVideoNoteInfo clientMatchCheckedVideoNoteInfo = new ClientMatchCheckedVideoNoteInfo()
                        {
                            ClientMatchCheckedId = clientMatchChecked.ClientMatchCheckedId,
                            FileId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.FileId,
                            FileUniqueId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.FileId,
                            FileSize = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.FileSize,
                            Length = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.Length,
                            Duration = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.Duration,
                            VideoNote = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.VideoNote,
                            ThumbnailFileId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.ThumbnailFileId,
                            ThumbnailFileUniqueId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.ThumbnailFileUniqueId,
                            ThumbnailFileSize = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.ThumbnailFileSize,
                            ThumbnailWidth = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.ThumbnailWidth,
                            ThumbnailHeight = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.ThumbnailHeight,
                            Thumbnail = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoNoteInfo.Thumbnail,
                        };
                        clientMatchChecked.ClientMatchCheckedVideoNoteInfo = clientMatchCheckedVideoNoteInfo;
                        _bonusDbContext.ClientMatchCheckedVideoNoteInfos.Add(clientMatchCheckedVideoNoteInfo);
                    }

                    if (clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo != null)
                    {
                        ClientMatchCheckedVideoInfo clientMatchCheckedVideoInfo = new ClientMatchCheckedVideoInfo()
                        {
                            ClientMatchCheckedId = clientMatchChecked.ClientMatchCheckedId,
                            FileId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.FileId,
                            FileUniqueId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.FileId,
                            FileSize = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.FileSize,
                            Width = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.Width,
                            Height = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.Height,
                            MimeType = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.MimeType,
                            Duration = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.Duration,
                            Video = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.Video,
                            ThumbnailFileId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.ThumbnailFileId,
                            ThumbnailFileUniqueId = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.ThumbnailFileUniqueId,
                            ThumbnailFileSize = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.ThumbnailFileSize,
                            ThumbnailWidth = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.ThumbnailWidth,
                            ThumbnailHeight = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.ThumbnailHeight,
                            Thumbnail = clientMatchUncheckedViewModel.ClientMatchUncheckedVideoInfo.Thumbnail,
                        };
                        clientMatchChecked.ClientMatchCheckedVideoInfo = clientMatchCheckedVideoInfo;
                        _bonusDbContext.ClientMatchCheckedVideoInfos.Add(clientMatchCheckedVideoInfo);
                    }

                    if (clientMatch.ClientMatchUncheckedVideoInfo != null)
                    {
                        _bonusDbContext.ClientMatchUncheckedVideoInfos.Remove(clientMatch.ClientMatchUncheckedVideoInfo);
                    }

                    if (clientMatch.ClientMatchUncheckedVideoNoteInfo != null)
                    {
                        _bonusDbContext.ClientMatchUncheckedVideoNoteInfos.Remove(clientMatch.ClientMatchUncheckedVideoNoteInfo);
                    }
                    _bonusDbContext.UncheckedClientMatchs.Remove(clientMatch);
                    _bonusDbContext.SaveChanges();
                }
            }
        }

        public bool HasClientNewLikes(ClientMatchInfoViewModel clientMatchInfoViewModel)
        {
            int likes = _bonusDbContext.UncheckedClientMatchs.Where(x => x.ClientMatchInfoId == clientMatchInfoViewModel.ClientMatchInfoId && !x.IsWatched).Count();
            return likes >= 1 && (clientMatchInfoViewModel.LastShowMatches == null || clientMatchInfoViewModel.LastShowMatches.Value.AddSeconds(DALAY_TO_NOTIFY_NEW_LIKES) <= DateTime.Now);
        }

        public int NewLikesCountByClientMatchInfo(ClientMatchInfoViewModel clientMatchInfoViewModel, bool isUnwatchedOnly = true)
        {
            if (isUnwatchedOnly)
            {
                return _bonusDbContext.UncheckedClientMatchs.Where(x => x.ClientMatchInfoId == clientMatchInfoViewModel.ClientMatchInfoId && !x.IsWatched).Count();
            }
            else
            {
                return _bonusDbContext.UncheckedClientMatchs.Where(x => x.ClientMatchInfoId == clientMatchInfoViewModel.ClientMatchInfoId).Count();
            }
        }

        public void UpdateTimeShowNewLikes(ClientMatchInfoViewModel clientMatchInfoViewModel)
        {
            var clientMatchInfo = _bonusDbContext.ClientMatchInfos.FirstOrDefault(x => x.ClientMatchInfoId == clientMatchInfoViewModel.ClientMatchInfoId);
            if (clientMatchInfo != null)
            {
                clientMatchInfo.LastShowMatches = DateTime.Now;
                _bonusDbContext.SaveChanges();
            }
        }

        public IQueryable<ClientMatchUncheckedViewModel> GetAllClientUncheckedMatchsByClientId(int clientId)
        {
            var clientMatches = _bonusDbContext.Clients.Include(x => x.ClientMatchInfo).Where(x => x.ClientId == clientId).Select(x => _mapper.Map<ClientMatchUncheckedViewModel>(x.ClientMatchInfo.UncheckedClientMatchs));
            return CollectClientMatchUncheckedVideoInfo(clientMatches.ToList()).AsQueryable();
        }

        public IQueryable<ClientMatchUncheckedViewModel> GetAllClientUncheckedMatchsByTelegramId(string telegramId)
        {
            var clientMatches = _bonusDbContext.Clients.Include(x => x.ClientMatchInfo).Where(x => x.TelegramId == telegramId).Select(x => _mapper.Map<ClientMatchUncheckedViewModel>(x.ClientMatchInfo.UncheckedClientMatchs));
            return CollectClientMatchUncheckedVideoInfo(clientMatches.ToList()).AsQueryable();
        }

        public IQueryable<ClientMatchUncheckedViewModel> GetAllClientUncheckedMatchs()
        {
            var clientMatches = _bonusDbContext.UncheckedClientMatchs.Select(x => _mapper.Map<ClientMatchUncheckedViewModel>(x));
            return CollectClientMatchUncheckedVideoInfo(clientMatches.ToList()).AsQueryable();
        }

        public ClientMatchUncheckedViewModel? GetTargetClientUncheckedMatch(int clientMatchInfoId, string telegramId)
        {
            var clientMatches = _bonusDbContext.UncheckedClientMatchs.Where(x => x.ClientMatchInfoId == clientMatchInfoId && x.MatchTelegramId == telegramId && x.AnswearMatchType == null).Select(x => _mapper.Map<ClientMatchUncheckedViewModel>(x));
            return CollectClientMatchUncheckedVideoInfo(clientMatches.ToList()).FirstOrDefault();
        }

        public void SetWatchClientMatch(ClientMatchUncheckedViewModel clientMatchUncheckedView)
        {
            var clientMatch = _bonusDbContext.UncheckedClientMatchs.FirstOrDefault(x => x.ClientMatchUncheckedId == clientMatchUncheckedView.ClientMatchUncheckedId && !x.IsWatched);
            if (clientMatch != null)
            {
                clientMatch.IsWatched = true;
                _bonusDbContext.SaveChanges();
            }
        }

        public bool AnyTargetClientUncheckedMatch(int clientMatchInfoId, string telegramId) => _bonusDbContext.UncheckedClientMatchs.Any(x => x.ClientMatchInfoId == clientMatchInfoId && x.MatchTelegramId == telegramId && x.AnswearMatchType == null);

        private List<ClientMatchUncheckedViewModel> CollectClientMatchUncheckedVideoInfo(List<ClientMatchUncheckedViewModel> clientMatches)
        {
            foreach (var clientMatch in clientMatches)
            {
                var videoNoteInfo = _bonusDbContext.ClientMatchUncheckedVideoNoteInfos.FirstOrDefault(x => x.ClientMatchUncheckedId == clientMatch.ClientMatchUncheckedId);
                if (videoNoteInfo != null)
                {
                    clientMatch.ClientMatchUncheckedVideoNoteInfo = _mapper.Map<ClientMatchUncheckedVideoNoteInfoViewModel>(videoNoteInfo);
                }

                var videoInfo = _bonusDbContext.ClientMatchUncheckedVideoInfos.FirstOrDefault(x => x.ClientMatchUncheckedId == clientMatch.ClientMatchUncheckedId);
                if (videoInfo != null)
                {
                    clientMatch.ClientMatchUncheckedVideoInfo = _mapper.Map<ClientMatchUncheckedVideoInfoViewModel>(videoInfo);
                }
            }
            return clientMatches;
        }

        public IQueryable<ClientMatchCheckedViewModel> GetAllClientCheckedMatchsByClientId(int clientId)
        {
            var clientMatches = _bonusDbContext.Clients.Include(x => x.ClientMatchInfo).Where(x => x.ClientId == clientId).Select(x => _mapper.Map<ClientMatchCheckedViewModel>(x.ClientMatchInfo.CheckedClientMatchs));
            return CollectClientMatchCheckedVideoInfo(clientMatches);
        }

        public IQueryable<ClientMatchCheckedViewModel> GetAllClientCheckedMatchsByTelegramId(string telegramId)
        {
            var clientMatches = _bonusDbContext.Clients.Include(x => x.ClientMatchInfo).Where(x => x.TelegramId == telegramId).Select(x => _mapper.Map<ClientMatchCheckedViewModel>(x.ClientMatchInfo.CheckedClientMatchs));
            return CollectClientMatchCheckedVideoInfo(clientMatches);
        }

        public IQueryable<ClientMatchCheckedViewModel> GetAllClientCheckedMatchs()
        {
            var clientMatches = _bonusDbContext.CheckedClientMatchs.Select(x => _mapper.Map<ClientMatchCheckedViewModel>(x));
            return CollectClientMatchCheckedVideoInfo(clientMatches);
        }

        public ClientMatchCheckedViewModel? GetTargetClientCheckedMatch(int clientMatchInfoId, string telegramId)
        {
            var clientMatches = _bonusDbContext.CheckedClientMatchs.Where(x => x.ClientMatchInfoId == clientMatchInfoId && x.MatchTelegramId == telegramId).Select(x => _mapper.Map<ClientMatchCheckedViewModel>(x));
            return CollectClientMatchCheckedVideoInfo(clientMatches).FirstOrDefault();
        }
        private IQueryable<ClientMatchCheckedViewModel> CollectClientMatchCheckedVideoInfo(IQueryable<ClientMatchCheckedViewModel> clientMatches)
        {
            foreach (var clientMatch in clientMatches)
            {
                var videoNoteInfo = _bonusDbContext.ClientMatchCheckedVideoNoteInfos.FirstOrDefault(x => x.ClientMatchCheckedId == clientMatch.ClientMatchCheckedId);
                if (videoNoteInfo != null)
                {
                    clientMatch.ClientMatchCheckedVideoNoteInfo = _mapper.Map<ClientMatchCheckedVideoNoteInfoViewModel>(videoNoteInfo);
                }

                var videoInfo = _bonusDbContext.ClientMatchCheckedVideoInfos.FirstOrDefault(x => x.ClientMatchCheckedId == clientMatch.ClientMatchCheckedId);
                if (videoInfo != null)
                {
                    clientMatch.ClientMatchCheckedVideoInfo = _mapper.Map<ClientMatchCheckedVideoInfoViewModel>(videoInfo);
                }
            }
            return clientMatches;
        }
    }
}
