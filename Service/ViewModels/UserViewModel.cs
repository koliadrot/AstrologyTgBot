namespace Service.ViewModels
{
	public class UserViewModel
	{
		public int UserId { get; set; }
		public string? Login { get; set; }
		public string? PasswordHash { get; set; }
		public string? Name { get; set; }
		public string? Token { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public bool IsAdmin { get; set; }
		public int UserGroupId { get; set; }
		public int UserStatusId { get; set; }
		public int SupportServiceId { get; set; }
	}
}
