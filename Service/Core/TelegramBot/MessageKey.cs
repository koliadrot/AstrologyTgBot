namespace Service.Core.TelegramBot
{
    /// <summary>
    /// Доступные сообщения из словаря для телеграм бота
    /// </summary>
    public static class MessageKey
    {
        /// <summary>
        /// Ознакомление с политикой конфи-ти
        /// </summary>
        public const string READ_TERMS_OF_USE = "readTermsOfUse";

        /// <summary>
        /// Переходы между пунктами меню
        /// </summary>
        public const string TRANSITION_HELP = "transitionHelp";

        /// <summary>
        /// Неизвестная команда
        /// </summary>
        public const string UNKNOW_COMMAND = "unknowCommand";

        /// <summary>
        /// Необходимость регистрации/входа в системе
        /// </summary>
        public const string NEED_AUTH = "needAuthtorization";

        /// <summary>
        /// Бот занят обработкой другой команды
        /// </summary>
        public const string WAIT = "wait";

        /// <summary>
        /// Непредвиденная ошибка
        /// </summary>
        public const string ERROR = "error";

        /// <summary>
        /// Уже авторизован пользователь
        /// </summary>
        public const string ALREADY_REGISTER = "alreadyRegister";

        /// <summary>
        /// Успешная регистрация
        /// </summary>
        public const string SUCCESS_REGISTRATION = "successRegistration";

        /// <summary>
        /// Отправлены данные на регистрацию
        /// </summary>
        public const string SEND_DATA_REGISTRATION = "sendDataRegister";

        /// <summary>
        /// Ввод даты рождения
        /// </summary>
        public const string ENTER_BIRTHDAY = "enterBirthday";

        /// <summary>
        /// Неверный формат даты рождения
        /// </summary>
        public const string WRONG_FORMAT_BIRTHDAY = "wrongFormatBirthday";

        /// <summary>
        /// Ввод имени пользователя
        /// </summary>
        public const string ENTER_FIRST_NAME = "registerConditionFirstName";

        /// <summary>
        /// Неверное имя
        /// </summary>
        public const string WRONG_FIRST_NAME = "wrongFirstName";

        /// <summary>
        /// Ввод пола при рождении
        /// </summary>
        public const string ENTER_BIRTH_GENDER = "enterBirthGender";

        /// <summary>
        /// Ввод пола при поиске
        /// </summary>
        public const string ENTER_SEARCH_GENDER = "enterSearchGender";

        /// <summary>
        /// Ввод места рождения
        /// </summary>
        public const string ENTER_BIRTH_PLACE = "enterBirthPlace";

        /// <summary>
        /// Неверный формат места рождения
        /// </summary>
        public const string WRONG_FORMAT_BIRTH_PLACE = "wrongFormatBirthPlace";

        /// <summary>
        /// Ввод места поиска
        /// </summary>
        public const string ENTER_SEARCH_PLACE = "enterSearchPlace";

        /// <summary>
        /// Неверный формат места поиска
        /// </summary>
        public const string WRONG_FORMAT_SEARCH_PLACE = "wrongFormatSearchPlace";

        /// <summary>
        /// Подтвердить город
        /// </summary>
        public const string ACCEPT_CITY = "acceptCity";

        /// <summary>
        /// Ввод возраст поиска
        /// </summary>
        public const string ENTER_SEARCH_AGE = "enterSearchAge";

        /// <summary>
        /// Неверный формат возраст поиска
        /// </summary>
        public const string WRONG_FORMAT_SEARCH_AGE = "wrongFormatSearchAge";

        /// <summary>
        /// Ввод описания о себе
        /// </summary>
        public const string ENTER_ABOUT_ME = "enterAboutMe";

        /// <summary>
        /// Не верное о себе (длина символов большая)
        /// </summary>
        public const string WHRONG_ABOUT_ME = "wrongAboutMe";

        /// <summary>
        /// Ввод медиа файлов пользователя
        /// </summary>
        public const string ENTER_MEDIA_INFO = "enterMediaInfo";

        /// <summary>
        /// Неверное медиа файлы
        /// </summary>
        public const string WRONG_MEDIA_INFO = "wrongMediaInfo";

        /// <summary>
        /// Пустая аватарка
        /// </summary>
        public const string EMPTY_AVATAR_INFO = "emptyAvatarInfo";

        /// <summary>
        /// Большое кол-во медиа файлов
        /// </summary>
        public const string VERY_MANY_MEDIA_INFO = "veryManyMediaInfo";

        /// <summary>
        /// Ввод цель поиска
        /// </summary>
        public const string ENTER_SEARCH_GOAL = "enterSearchGoal";

        /// <summary>
        /// Неверная цель поиска
        /// </summary>
        public const string WRONG_SEARCH_GOAL = "wrongSearchGoal";

        /// <summary>
        /// Обработка меди контента
        /// </summary>
        public const string PROCESSING_MEDIA = "processingMedia";

        /// <summary>
        /// Очень длинное видео
        /// </summary>
        public const string VERY_LONG_VIDEO = "veryLongVideo";

        /// <summary>
        /// Не загружены видео
        /// </summary>
        public const string WRONG_LOAD_VIDEO = "wrongLoadVideo";

        /// <summary>
        /// Хотите загрузить больше медиа файлов
        /// </summary>
        public const string MORE_LOAD_MEDIA = "moreLoadMedia";

        /// <summary>
        /// Нет такого варианта выбора
        /// </summary>
        public const string NO_VARITANT_CHOICE = "noVariantChoice";

        /// <summary>
        /// Приветственное сообщение по умолчанию у команды просмотра анкет
        /// </summary>
        public const string DEFAULT_FIND_APPLICATION = "defaultFindApplication";

        /// <summary>
        /// Написать любовное сообщение
        /// </summary>
        public const string WRITE_LOVE_LETTER = "writeLoveLetter";

        /// <summary>
        /// Любовное сообщение отправлено
        /// </summary>
        public const string SEND_LOVE_LETTER = "sendLoveLetter";

        /// <summary>
        /// Отправить фото или видео для аватара
        /// </summary>
        public const string ENTER_AVATAR = "enterAvatar";

        /// <summary>
        /// Приветственное сообщение по умолчанию у команды просмотра совпадений анкет
        /// </summary>
        public const string DEFAULT_CHECK_MATCH = "defaultCheckMatch";

        /// <summary>
        /// Отсуствуют симпатии
        /// </summary>
        public const string NO_CLIENT_MATCHS = "noClientMatchs";

        /// <summary>
        /// Отправить сообщение о взаимное симпатии себе
        /// </summary>
        public const string SEND_MATCH_MY_CLIENT = "sendMatchMyClient";

        /// <summary>
        /// Отправить сообщение о взаимной симпатии другому пользователю
        /// </summary>
        public const string SEND_MATCH_OTHER_CLIENT = "sendMatchOtherClient";

        /// <summary>
        /// Сообщает о симпатии другого пользователя
        /// </summary>
        public const string SEND_LIKE_MATCH = "sendLikeMatch";

        /// <summary>
        /// Отправляет сообщение симпатии с любовным сообщением
        /// </summary>
        public const string SEND_LOVE_LETTER_MATCH = "sendLoveLetterMatch";

        /// <summary>
        /// Отправляет сообщение симпатии с видео
        /// </summary>
        public const string SEND_VIDEO_MATCH = "sendVideoMatch";

        /// <summary>
        /// Отправляет уведомление о новых лайках
        /// </summary>
        public const string SEND_NOTIFY_NEW_LIKES = "sendNotifyNewLikes";

        /// <summary>
        /// Сколько осталось лайков у пользователя
        /// </summary>
        public const string COUNT_LEFT_LIKES = "countLeftLikes";

    }

    /// <summary>
    /// Доступные кнопки под строкой вводе в чате телеграмма
    /// </summary>
    public static class ReplyButton
    {
        /// <summary>
        /// Да
        /// </summary>
        public const string YES = "yesButton";

        /// <summary>
        /// Мужчина
        /// </summary>
        public const string MAN = "manButton";

        /// <summary>
        /// Женщина
        /// </summary>
        public const string WOMAN = "womanButton";

        /// <summary>
        /// Любой
        /// </summary>
        public const string ANYWAY = "anyWay";

        /// <summary>
        /// Нет
        /// </summary>
        public const string NO = "noButton";

        /// <summary>
        /// Пропустить
        /// </summary>
        public const string SKIP = "skipButton";

        /// <summary>
        /// Соглашение
        /// </summary>
        public const string I_AGREE = "agreeButton";

        /// <summary>
        /// Читать
        /// </summary>
        public const string READ = "readButton";

        /// <summary>
        /// Назад
        /// </summary>
        public const string BACK = "backButton";

        /// <summary>
        /// Поделиться геопозицией
        /// </summary>
        public const string SHARE_GEO = "shareGeoButton";

        /// <summary>
        /// Переход поссылке
        /// </summary>
        public const string GO_TO_LINK = "goToLink";

        /// <summary>
        /// Подробнее
        /// </summary>
        public const string MORE_DETAILS = "moreDetails";

        /// <summary>
        /// Кратко
        /// </summary>
        public const string LESS_DETAILS = "lessDetails";

        /// <summary>
        /// Отношения
        /// </summary>
        public const string RELATIONSHIP = "relationship";

        /// <summary>
        /// Общение
        /// </summary>
        public const string COMMUNICATION = "communication";

        /// <summary>
        /// Аватар
        /// </summary>
        public const string Avatar = "avatar";

        /// <summary>
        /// Нравится
        /// </summary>
        public const string Like = "like";

        /// <summary>
        /// Любовное Письмо
        /// </summary>
        public const string LoveLetter = "loveletter";

        /// <summary>
        /// Не нравится
        /// </summary>
        public const string Dislike = "dislike";

        /// <summary>
        /// Профиль
        /// </summary>
        public const string Profile = "profile";

        /// <summary>
        /// Больше медиа
        /// </summary>
        public const string MORE_MEDIA = "moreMedia";

        /// <summary>
        /// Поиск анкет
        /// </summary>
        public const string FIND_APPLICATIONS = "findApplications";

        /// <summary>
        /// Смотреть лайки
        /// </summary>
        public const string WATCH_LIKES = "watchLikes";

        /// <summary>
        /// Показать анкету
        /// </summary>
        public const string SHOW_APPLICATION = "showApplication";
    }

    /// <summary>
    /// Доступные вставные шаблоны значений
    /// </summary>
    public static class Promt
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public const string MESSAGE = "message";

        /// <summary>
        /// Город
        /// </summary>
        public const string CITY = "city";

        /// <summary>
        /// Видео
        /// </summary>
        public const string VIDEO = "video";

        /// <summary>
        /// Длительность
        /// </summary>
        public const string DURATION = "duration";

        /// <summary>
        /// Медиа
        /// </summary>
        public const string MEDIA = "media";

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public const string USER_NAME = "username";

        /// <summary>
        /// Лайки
        /// </summary>
        public const string LIKES = "likes";
    }
}
