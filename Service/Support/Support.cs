namespace Service.Support
{
	using System;
	using System.Linq;
	using System.Text;

	public static class Support
	{
		public static string CreateToken(string login)
		{
			byte[] key = Guid.NewGuid().ToByteArray();
			byte[] key2 = Guid.NewGuid().ToByteArray();

			byte[] loginByte = Encoding.Default.GetBytes(login);
			string Token = Convert.ToBase64String(key2.Concat(key).Concat(loginByte).ToArray());
			return Token;
		}
		public static string GetMD5(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return "";
			}
			System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
			return System.Text.Encoding.Default.GetString(hash);
		}
	}
}
