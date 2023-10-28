namespace Service.Abstract
{
    using Service.ViewModels;
    using System.Threading.Tasks;

    public interface IMobileManager
    {
        public Task<UserViewModel?> Auth(LoginViewModel loginViewModel);
        public Task<UserViewModel?> Register(RegisterViewModel viewModel);
        public int GetUserId(string token);
    }
}
