namespace Data.Core.Models
{
	using System.ComponentModel.DataAnnotations.Schema;

	public class ApiAccessToken
	{
		public int ApiAccessTokenId { get; set; }
		public string Token { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }
		public int UserId { get; set; }
	}
}
