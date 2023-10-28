namespace Service.Core
{
    using Data.Core;
    using Data.Core.Models;
    using Service.Abstract;
    using Service.Exceptions;
    using Service.ViewModels;
    using System.Linq;
    using System.Threading.Tasks;

    public class MobileManager : IMobileManager
    {
        private readonly ApplicationDbContext _ctx;
        public MobileManager(ApplicationDbContext context)
        {
            _ctx = context;
        }
        public async Task<UserViewModel?> Auth(LoginViewModel loginViewModel)
        {
            //NOTE: Не смог сохранить хэш правильный
            //var passwordHash = Support.Support.GetMD5(loginViewModel.Password);

            var user = _ctx.Users.FirstOrDefault(f => f.Login == loginViewModel.Login && f.PasswordHash == loginViewModel.Password);
            if (user == null)
            {
                return null;
            }
            var token = Support.Support.CreateToken(loginViewModel.Login);
            _ctx.ApiAccessTokens.Add(new ApiAccessToken
            {
                UserId = user.UserId,
                User = user,
                Token = token
            });
            await _ctx.SaveChangesAsync();
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
            var user = _ctx.Users.FirstOrDefault(f => f.Login == viewModel.Login);
            if (user != null)
            {
                throw new AuthException("Логин занят");
            }
            user = _ctx.Users.FirstOrDefault(f => f.Email == viewModel.Email);
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
            await _ctx.Users.AddAsync(newUser);
            await _ctx.SaveChangesAsync();
            var token = Support.Support.CreateToken(viewModel.Login);
            await _ctx.ApiAccessTokens.AddAsync(new ApiAccessToken
            {
                UserId = newUser.UserId,
                User = newUser,
                Token = token
            });
            await _ctx.SaveChangesAsync();
            return new UserViewModel
            {
                Name = newUser.Name,
                Token = token
            };
        }


        public int GetUserId(string token) => _ctx.ApiAccessTokens.FirstOrDefault(f => f.Token == token)?.UserId ?? -1;
    }
}
