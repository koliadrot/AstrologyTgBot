namespace Service.Core
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data.Core;
    using Data.Core.Models;
    using Service.Abstract;
    using Service.Extensions;
    using Service.ViewModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserManager : IUserManager, IDisposable
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _bonusDbContext;

        public UserManager(IMapper mapper)
        {
            _mapper = mapper;
            _bonusDbContext = new ApplicationDbContext();
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

        #region Users
        public IQueryable<UserViewModel> GetUsers() => _bonusDbContext.Users.ProjectTo<UserViewModel>(_mapper.ConfigurationProvider);

        public UserViewModel GetUserByLogin(string loginName)
        {
            var entity = _bonusDbContext.Users.FirstOrDefault(u => u.Email == loginName);

            return entity == null ? throw new ArgumentException("User not found") : _mapper.Map<UserViewModel>(entity);
        }
        public bool UserExist(string? loginName)
        {
            if (loginName == null)
            {
                return false;
            }

            var entity = _bonusDbContext.Users.FirstOrDefault(u => u.Email == loginName);
            return entity != null;
        }

        public async Task CreateUser(UserViewModel viewModel)
        {
            var entity = _mapper.Map<User>(viewModel);

            entity.Login = entity.Login.IsNull() ? "" : entity.Login;
            entity.PasswordHash = entity.PasswordHash.IsNull() ? "" : entity.PasswordHash;

            await _bonusDbContext.Users.AddAsync(entity);
            await _bonusDbContext.SaveChangesAsync();
            viewModel.UserId = entity.UserId;
        }


        public async Task UpdateUser(UserViewModel viewModel)
        {
            viewModel.Login = viewModel.Login.IsNull() ? "" : viewModel.Login;
            viewModel.PasswordHash = viewModel.PasswordHash.IsNull() ? "" : viewModel.PasswordHash;

            await _bonusDbContext.Users.UpdateEntity(viewModel.UserId, viewModel, _mapper);
            await _bonusDbContext.SaveChangesAsync();
        }

        public async Task RemoveUser(UserViewModel viewModel)
        {
            await _bonusDbContext.Users.RemoveById(viewModel.UserId);
            await _bonusDbContext.SaveChangesAsync();
        }
        #endregion
    }
}
