namespace Service.Core.TelegramBot
{

    /// <summary>
    /// Меню формат телеграм бота
    /// </summary>
    public class MenuList
    {
        /// <summary>
        /// Стартовое меню до авторизации
        /// </summary>
        public Menu StartMenu;

        /// <summary>
        /// Меню после авторизации
        /// </summary>
        public Menu AuthMenu;
    }

    /// <summary>
    /// Меню формат телеграм бота
    /// </summary>
    public class Menu
    {
        public List<MenuItem> Items { get; set; }
    }

    /// <summary>
    /// Пункт меню
    /// </summary>
    public class MenuItem
    {
        public string CommandName { get; set; }
        public string Name { get; set; }
        public Menu SubMenu { get; set; }
    }
}
