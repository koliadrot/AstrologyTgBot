namespace Service.Core
{
    using Data.Core;
    using Data.Core.Models;
    using Service.Abstract;
    using Service.Exceptions;
    using Service.ViewModels;
    using System.Linq;
    using System.Threading.Tasks;

    public class MobileManager : IMobileManager, IDisposable
    {
        private readonly ApplicationDbContext _bonusDbContext;
        public MobileManager()
        {
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

        public async Task<UserViewModel?> Auth(LoginViewModel loginViewModel)
        {
            //NOTE: Не смог сохранить хэш правильный
            //var passwordHash = Support.Support.GetMD5(loginViewModel.Password);

            var user = _bonusDbContext.Users.FirstOrDefault(f => f.Login == loginViewModel.Login && f.PasswordHash == loginViewModel.Password);
            if (user == null)
            {
                return null;
            }
            var token = Support.Support.CreateToken(loginViewModel.Login);
            _bonusDbContext.ApiAccessTokens.Add(new ApiAccessToken
            {
                UserId = user.UserId,
                User = user,
                Token = token
            });
            await _bonusDbContext.SaveChangesAsync();
            return new UserViewModel
            {
                Name = user.Name,
                Token = token,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                UserId = user.UserId,
                PhoneNumber = user.PhoneNumber
            };
        }
        public async Task<UserViewModel?> Register(RegisterViewModel viewModel)
        {
            var user = _bonusDbContext.Users.FirstOrDefault(f => f.Login == viewModel.Login);
            if (user != null)
            {
                throw new AuthException("Логин занят");
            }
            user = _bonusDbContext.Users.FirstOrDefault(f => f.Email == viewModel.Email);
            if (user != null)
            {
                throw new AuthException("Данный Email уже зарегистрирован");
            }
            var passwordHash = Support.Support.GetMD5(viewModel.Password);

            var newUser = new User
            {
                Login = viewModel.Login,
                Email = viewModel.Email,
                Name = viewModel.Name,
                PasswordHash = passwordHash,
            };
            await _bonusDbContext.Users.AddAsync(newUser);
            await _bonusDbContext.SaveChangesAsync();
            var token = Support.Support.CreateToken(viewModel.Login);
            await _bonusDbContext.ApiAccessTokens.AddAsync(new ApiAccessToken
            {
                UserId = newUser.UserId,
                User = newUser,
                Token = token
            });
            await _bonusDbContext.SaveChangesAsync();
            return new UserViewModel
            {
                Name = newUser.Name,
                Token = token
            };
        }


        public int GetUserId(string token) => _bonusDbContext.ApiAccessTokens.FirstOrDefault(f => f.Token == token)?.UserId ?? -1;
    }
}
