namespace Service.Extensions
{
	public static class VariableExtensions
	{
		private const string FORMAT_INTEGER_NUMBER = "N0";

		/// <summary>
		/// Пустая строка или null
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsNull(this string str)
		{
			return string.IsNullOrEmpty(str);
		}
		/// <summary>
		/// Стока не пустая и не mull
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool NotNull(this string str)
		{
			return !string.IsNullOrEmpty(str);
		}
		/// <summary>
		/// Если строка пустая - возращается null, иначе исходная строка
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string NullIfEmpty(this string str)
		{
			return str.IsNull() ? null : str;
		}


	}
}
