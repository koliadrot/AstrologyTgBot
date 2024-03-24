namespace Service.ViewModels
{
    using Service.Support;
    using System;

    /// <summary>
    /// Клиент
    /// </summary>
    public class ClientViewModel
    {

        /// <summary>
        /// Id клиента
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Id телеграмм аккаунта
        /// </summary>
        public string? TelegramId { get; set; }

        /// <summary>
        /// Техническое имя аккаунта
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public string? BirthGender { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Координаты место рождения
        /// </summary>
        public string? BirthCoord { get; set; }

        /// <summary>
        /// Город рождения
        /// </summary>
        public string? BirthCity { get; set; }

        /// <summary>
        /// Город поиска
        /// </summary>
        public string? SearchCity { get; set; }

        /// <summary>
        /// Координаты поиска
        /// </summary>
        public string? SearchCoord { get; set; }

        /// <summary>
        /// Пол поиска
        /// </summary>
        public string? SearchGender { get; set; }

        /// <summary>
        /// Возраст поиска
        /// </summary>
        public string? SearchAge { get; set; }

        private int? _minSearchAge = null;

        /// <summary>
        /// Минимальная возрастная граница поиска
        /// </summary>
        public int MinSearchAge
        {
            get
            {
                if (_minSearchAge == null)
                {
                    _minSearchAge = int.Parse(SearchAge.Split('-')[0]);
                }
                return _minSearchAge.Value;
            }
        }

        public int? _maxSearchAge = null;

        /// <summary>
        /// Максимальная граница поиска возраста
        /// </summary>
        public int MaxSearchAge
        {
            get
            {
                if (_maxSearchAge == null)
                {
                    _maxSearchAge = int.Parse(SearchAge.Split('-')[1]);
                }
                return _maxSearchAge.Value;
            }
        }


        /// <summary>
        /// Цель поиска
        /// </summary>
        public string? SearchGoal { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string? AboutMe { get; set; }

        /// <summary>
        /// Статус анкеты
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Заблокирован ли пользователь
        /// </summary>
        public bool? IsBlock { get; set; }

        /// <summary>
        /// Дата создания анкеты
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Медиа информация
        /// </summary>
        public virtual ClientMediaInfoViewModel ClientMediaInfo { get; set; }

        /// <summary>
        /// Совпадения
        /// </summary>
        public virtual ClientMatchInfoViewModel ClientMatchInfo { get; set; }

        private Coordinate _coordinate = null;

        /// <summary>
        /// Координаты Поиска
        /// </summary>
        public Coordinate SearchCoordinate
        {
            get
            {
                if (_coordinate == null)
                {
                    _coordinate = new Coordinate(SearchCoord.Split(':')[0], SearchCoord.Split(':')[1]);
                }
                return _coordinate;
            }
        }


    }
}
