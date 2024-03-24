namespace Service.Extensions
{
    using Service.Abstract.TelegramBot;
    using Service.Support;
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

        /// <summary>
        /// Защищенный способ получения элемента из массива строк
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string? GetStringAtIndex(this List<string> source, int index, bool isTrim = true) => index >= 0 && index < source.Count ? source[index] : null;


        /// <summary>
        /// Исправная ли ссылка
        /// </summary>
        /// <returns></returns>
        public static bool IsValidLink(this string url) => Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        /// <summary>
        /// Метод для расчета расстояния между двумя координатами по формуле Haversine
        /// </summary>
        /// <param name="from">От кого</param>
        /// <param name="to">К кому</param>
        /// <returns></returns>
        public static double DistanceTo(this Coordinate from, Coordinate to)
        {
            const double EarthRadiusKm = 6371;

            double dLat = DegreesToRadians(to.Latitude - from.Latitude);
            double dLon = DegreesToRadians(to.Longitude - from.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(from.Latitude)) * Math.Cos(DegreesToRadians(to.Latitude)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = EarthRadiusKm * c;
            return distance;

            double DegreesToRadians(double degrees) => degrees * Math.PI / 180;
        }
    }
}
