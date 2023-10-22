namespace Service.Abstract
{
	using Service.ViewModels;
	using System.Linq;
	using System.Threading.Tasks;

	public interface IUserManager
	{

		#region Users

		IQueryable<UserViewModel> GetUsers();
		UserViewModel GetUserByLogin(string loginName);
		public bool UserExist(string? loginName);
		Task CreateUser(UserViewModel viewModel);
		Task UpdateUser(UserViewModel viewModel);
		Task RemoveUser(UserViewModel viewModel);
		#endregion

	}
}
