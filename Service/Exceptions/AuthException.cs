namespace Service.Exceptions
{
	using System;

	public class AuthException : Exception
	{
		public AuthException()
			: base()
		{ }

		public AuthException(string message)
			: base(message)
		{ }
	}
}
