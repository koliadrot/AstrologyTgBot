namespace Service.Core.TelegramBot
{
    /// <summary>
    /// Доступные сообщения из словаря для телеграм бота
    /// </summary>
    public static class MessageKey
    {
        /// <summary>
        /// Cтарт регистрации после перехода по реферальной ссылке
        /// </summary>
        public const string FRIEND_REFERAL_LINK = "registerStartFromReferalLink";

        /// <summary>
        /// Бонусы за регистрацию по реферальной ссылке
        /// </summary>
        public const string REFERAL_BONUSES = "registerReferalBonusesGift";

        /// <summary>
        /// Реферальная система не работает
        /// </summary>
        public const string DISABLE_REFERAL_SYSTEM = "disableReferalSystem";

        /// <summary>
        /// Промо код реферальной программы не действительный
        /// </summary>
        public const string PROMO_CODE_REFERAL_DISABLE = "promoCodeReferalDisable";

        /// <summary>
        /// Неверный формат промо кода реферальной программы
        /// </summary>
        public const string WRONG_FORMAT_PROMO_CODE_REFERAL = "wrongPromoCodeReferal";

        /// <summary>
        /// Ввод промо кода реферальной программы
        /// </summary>
        public const string ENTER_PROMO_CODE_REFERAL = "enterPromoCodeReferal";

        /// <summary>
        /// Выбор способа регистрации
        /// </summary>
        public const string CHOOSE_TYPE_REGISTER = "chooseTypeRegister";

        /// <summary>
        /// Выбор способа входа
        /// </summary>
        public const string CHOOSE_TYPE_ENTER = "chooseTypeEnter";

        /// <summary>
        /// Уже авторизован пользователь
        /// </summary>
        public const string ALREADY_REGISTER = "alreadyRegister";

        /// <summary>
        /// Успешная регистрация
        /// </summary>
        public const string SUCCESS_REGISTRATION = "successRegistration";

        /// <summary>
        /// Успешный вход
        /// </summary>
        public const string SUCCESS_ENTER = "successEnter";

        /// <summary>
        /// Отправлены данные на регистрацию
        /// </summary>
        public const string SEND_DATA_REGISTRATION = "sendDataRegister";

        /// <summary>
        /// Успешное подтверждение кода
        /// </summary>
        public const string SUCCESS_VERIFY_CODE = "successVerifyCode";

        /// <summary>
        /// Создание пароля
        /// </summary>
        public const string CREATE_PASSWORD = "createPassword";

        /// <summary>
        /// Сохранение пароля
        /// </summary>
        public const string SAVE_PASSWORD = "savePassword";

        /// <summary>
        /// Не верный формат пароля
        /// </summary>
        public const string WRONG_FORMAT_PASSWORD = "wrongFormatPassword";

        /// <summary>
        /// Ввод пароля
        /// </summary>
        public const string ENTER_PASSWORD = "enterPassword";

        /// <summary>
        /// Успешно изменен пароль
        /// </summary>
        public const string SUCCESS_CHANGE_PASSWORD = "successChangedPassword";

        /// <summary>
        /// Не верный пароль
        /// </summary>
        public const string WRONG_PASSWORD = "wrongPassword";

        /// <summary>
        /// Ввести email
        /// </summary>
        public const string ENTER_EMAIL = "enterEmail";

        /// <summary>
        /// Не найден email или не подтвержден
        /// </summary>
        public const string NOT_FOUND_EMAIL = "notFoundEmail";

        /// <summary>
        /// Неверный формат email
        /// </summary>
        public const string WRONG_FORMAT_EMAIL = "wrongFormatEmail";

        /// <summary>
        /// Email уже зарегистрирован
        /// </summary>
        public const string EXIST_EMAIL = "existEmail";

        /// <summary>
        /// Ввести мобильный телефон
        /// </summary>
        public const string ENTER_MOBILE_PHONE = "enterMobilePhone";

        /// <summary>
        /// Не найден мобильный телефон или не подтвержден
        /// </summary>
        public const string NOT_FOUND_MOBILE_PHONE = "notFoundMobilePhone";

        /// <summary>
        /// Неверный формат мобильного телефона
        /// </summary>
        public const string WRONG_FORMAT_MOBILE_PHONE = "wrongFormatMobilePhone";

        /// <summary>
        /// Мобильный телефон уже зарегистрирован
        /// </summary>
        public const string EXIST_MOBILE_PHONE = "existMobilePhone";

        /// <summary>
        /// Ввести номер карты
        /// </summary>
        public const string ENTER_CARD_CODE = "enterCardCode";

        /// <summary>
        /// Не найдена карта или не зарегистрирована
        /// </summary>
        public const string NOT_FOUND_CARD_CODE = "notFoundCardCode";

        /// <summary>
        /// Карта уже зарегистрирована
        /// </summary>
        public const string EXIST_CARD_CODE = "existCardCode";

        /// <summary>
        /// Ввод даты рождения
        /// </summary>
        public const string ENTER_BIRTHDAY = "enterBirthday";

        /// <summary>
        /// Неверный формат даты рождения
        /// </summary>
        public const string WRONG_FORMAT_BIRTHDAY = "wrongFormatBirthday";

        /// <summary>
        /// Рассылка персональных акций
        /// </summary>
        public const string PERSONAL_PROMOTIONS = "personalPromotions";

        /// <summary>
        /// Ввод имени пользователя
        /// </summary>
        public const string ENTER_FIRST_NAME = "registerConditionFirstName";

        /// <summary>
        /// Неверное имя
        /// </summary>
        public const string WRONG_FIRST_NAME = "wrongFirstName";

        /// <summary>
        /// Ввод гендера
        /// </summary>
        public const string ENTER_GENDER = "enterGender";

        /// <summary>
        /// Ввод фамилии пользователя
        /// </summary>
        public const string ENTER_LAST_NAME = "enterLastName";

        /// <summary>
        /// Неверная фамилия
        /// </summary>
        public const string WRONG_LAST_NAME = "wrongLastName";

        /// <summary>
        /// Ознакомление с политикой конфи-ти
        /// </summary>
        public const string READ_TERMS_OF_USE = "readTermsOfUse";

        /// <summary>
        /// Баланс бонусов клиента
        /// </summary>
        public const string BALANCE = "balanceClient";

        /// <summary>
        /// Поделиться картой
        /// </summary>
        public const string SHARE_CARD = "shareCard";

        /// <summary>
        /// Выбор темы обратной связи
        /// </summary>
        public const string FEEDBACK_CHOOSE = "feedbackChoose";

        /// <summary>
        /// Написать сообщене обратной связи
        /// </summary>
        public const string FEEDBACK_WRITE = "feedbackWrite";

        /// <summary>
        /// Обработка сообщения обратной связи
        /// </summary>
        public const string FEEDBACK_PROCESSING = "feedbackProcessing";

        /// <summary>
        /// История покупок
        /// </summary>
        public const string PURCHASE_HISTORY = "purchaseHistory";

        /// <summary>
        /// Пустая история покупок
        /// </summary>
        public const string EMPTY_PURCHASE_HISTORY = "emptyPurchaseHistory";

        /// <summary>
        /// Подтверждение адреса электронной почты
        /// </summary>
        public const string NEED_VERIFY_EMAIL = "needVerifyEmail";

        /// <summary>
        /// Сбой при попытке отправить повторно код подтверждения на почту
        /// </summary>
        public const string FAULT_VERIFY_EMAIL = "faultVerifyEmail";

        /// <summary>
        /// Подтверждение номера телефона по СМС
        /// </summary>
        public const string NEED_VERIFY_MOBILE_PHONE = "needVerifyMobilePhone";

        /// <summary>
        /// Подтверждение номера телефона по звонку от робота, который сообщит код
        /// </summary>
        public const string NEED_VERIFY_MOBILE_PHONE_SMS_CCALL = "needVerifyMobilePhoneSmsCall";

        /// <summary>
        /// Подтверждение номера телефона по последним четырем цифрам номера телефона, с которого позвонит робот
        /// </summary>
        public const string NEED_VERIFY_MOBILE_PHONE_SMS_CCALL_LAST_NUM = "needVerifyMobilePhoneSmsCallLastNum";

        /// <summary>
        /// Сбой при попытке отправить повторно код подтверждения по моб.тел
        /// </summary>
        public const string FAULT_VERIFY_MOBILE_PHONE = "faultVerifyMobilePhone";

        /// <summary>
        /// Восстановление пароля
        /// </summary>
        public const string CHOOSE_RESTORE_ACCESS = "chooseRestoreAccess";

        /// <summary>
        /// Нажатие на собственную реферальную ссылку
        /// </summary>
        public const string REFERAL_FRIEND_LINK = "referalFriendLink";

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
        /// Регистрация нового пользователя по реферальной ссылке
        /// </summary>
        public const string REFERAL_REGISTRATION = "referalRegistration";

        /// <summary>
        /// Шаблон данных товара в чеке
        /// </summary>
        public const string ITEMS = "items";
    }

    /// <summary>
    /// Доступные кнопки под строкой вводе в чате телеграмма
    /// </summary>
    public static class ReplyButton
    {
        /// <summary>
        /// Мобильный телефон
        /// </summary>
        public const string MOBILE_PHONE = "mobilePhoneButton";

        /// <summary>
        /// Эл. адрес
        /// </summary>
        public const string EMAIL = "emailButton";

        /// <summary>
        /// Поделиться мобильным телефоном
        /// </summary>
        public const string SHARE_MOBILE_PHONE = "shareMobilePhoneButton";

        /// <summary>
        /// Номер карты при регистрации
        /// </summary>
        public const string CARD_CODE_REGISTER = "cardCodeRegisterButton";

        /// <summary>
        /// Номер карты при входе
        /// </summary>
        public const string CARD_CODE_ENTER = "cardCodeEnterButton";

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
        /// Реферальная ссылка
        /// </summary>
        public const string REFERAL_LINK = "referalLinkButton";

        /// <summary>
        /// Назад
        /// </summary>
        public const string BACK = "backButton";
    }

    /// <summary>
    /// Доступные вставные шаблоны значений
    /// </summary>
    public static class Promt
    {
        /// <summary>
        /// Промо-код
        /// </summary>
        public const string PROMO_CODE = "promocode";

        /// <summary>
        /// Бонусные баллы
        /// </summary>
        public const string BONUSES_GIFT = "bonusesgift";

        /// <summary>
        /// Эл.адрес
        /// </summary>
        public const string EMAIL = "email";

        /// <summary>
        /// Мобильный телефон
        /// </summary>
        public const string MOBILE_PHONE = "mobilephone";

        /// <summary>
        /// Номер карты
        /// </summary>
        public const string CARD_CODE = "cardcode";

        /// <summary>
        /// Длина пароля
        /// </summary>
        public const string PASSWORD_LENGTH = "passwordlength";

        /// <summary>
        /// Баланс бонусов
        /// </summary>
        public const string BALANCE_BONUS = "balancebonus";

        /// <summary>
        /// Код чека
        /// </summary>
        public const string CODE_BILL = "codebill";

        /// <summary>
        /// Дата
        /// </summary>
        public const string DATE = "date";

        /// <summary>
        /// Магазин
        /// </summary>
        public const string SHOP = "shop";

        /// <summary>
        /// Действие
        /// </summary>
        public const string ACTION = "action";

        /// <summary>
        /// Шаблон данных товара в чеке
        /// </summary>
        public const string ITEMS = "items";

        /// <summary>
        /// Имя
        /// </summary>
        public const string NAME = "name";

        /// <summary>
        /// Количество
        /// </summary>
        public const string AMOUNT = "amount";

        /// <summary>
        /// Цена
        /// </summary>
        public const string PRICE = "price";

        /// <summary>
        /// Цена со скидкой
        /// </summary>
        public const string PRICE_DISCOUNT = "pricediscount";

        /// <summary>
        /// Скидка
        /// </summary>
        public const string DISCOUNT = "discount";

        /// <summary>
        /// Списано бонусов
        /// </summary>
        public const string SPEND_BONUS = "spendbonus";

        /// <summary>
        /// Начислено бонусов
        /// </summary>
        public const string ACCRUE_BONUS = "accruebonus";

        /// <summary>
        /// Имя
        /// </summary>
        public const string FIRST_NAME = "firstname";

        /// <summary>
        /// Фамилия
        /// </summary>
        public const string LAST_NAME = "lastname";

        /// <summary>
        /// Сообщение
        /// </summary>
        public const string MESSAGE = "message";
    }
}
