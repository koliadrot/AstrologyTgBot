namespace Service.Extensions
{
    using Service.Abstract.TelegramBot;
    using System.Text.RegularExpressions;

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

        /// <summary>
        /// Заменяем пробелы и некоторые специальные символы на пустую строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ValidateString(this string value) => new Regex(@"[\s\/.<,]+|[""']+").Replace(value, "");

        /// <summary>
        /// Проверяет команду на наличие у списка команд телеграм бота
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Contains(this List<ICommand> commands, string name)
        {
            name = name.ToLower();
            return commands.FirstOrDefault(x => x.IsValidCommand(name)) != null;
        }

        public static bool Contains(this IReadOnlyList<ICommand> commands, string name) => Contains(commands.ToList(), name);

        /// <summary>
        /// Валидация команды телеграм бота
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidCommand(this ICommand command, string name)
        {
            name = name.ToLower();
            return command.Name.ToLower() == name || command.ShortDescription.ToLower() == name || $"/{command.ShortDescription}".ToLower() == name;
        }


        /// <summary>
        /// Приводит значение к читаемому формату
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberAfterDot"></param>
        /// <returns></returns>
        public static string ToStringFormatNumber(this decimal number, int numberAfterDot = 2)
        {
            string format = $"N{numberAfterDot}";
            if (number % 1 == 0)
            {
                format = FORMAT_INTEGER_NUMBER;
            }
            string formattedNumber = number.ToString(format);
            while (formattedNumber.EndsWith("0") && formattedNumber.Contains("."))
            {
                formattedNumber = formattedNumber.Substring(0, formattedNumber.Length - 1);
            }
            return formattedNumber;
        }
        public static string ToStringFormatNumber(this float number, int numberAfterDot = 2)
        {
            string format = $"N{numberAfterDot}";
            if (number % 1 == 0)
            {
                format = FORMAT_INTEGER_NUMBER;
            }
            string formattedNumber = number.ToString(format);
            while (formattedNumber.EndsWith("0") && formattedNumber.Contains("."))
            {
                formattedNumber = formattedNumber.Substring(0, formattedNumber.Length - 1);
            }
            return formattedNumber;
        }

        public static string ToStringFormatNumber(this double number, int numberAfterDot = 2)
        {
            string format = $"N{numberAfterDot}";
            if (number % 1 == 0)
            {
                format = FORMAT_INTEGER_NUMBER;
            }
            string formattedNumber = number.ToString(format);
            while (formattedNumber.EndsWith("0") && formattedNumber.Contains(","))
            {
                formattedNumber = formattedNumber.Substring(0, formattedNumber.Length - 1);
            }
            return formattedNumber;
        }

        public static string ToStringFormatNumber(this int number) => number.ToString(FORMAT_INTEGER_NUMBER);
    }
}
