using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class TelegramBotParamMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelegramBotParamMessages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TelegramBotId = table.Column<int>(type: "int", nullable: false),
                    MessageName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageValue = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageValueDefault = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsButton = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TelegramBotParamsTelegramBotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramBotParamMessages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_TelegramBotParamMessages_TelegramParams_TelegramBotParamsTel~",
                        column: x => x.TelegramBotParamsTelegramBotId,
                        principalTable: "TelegramParams",
                        principalColumn: "TelegramBotId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBotParamMessages_TelegramBotParamsTelegramBotId",
                table: "TelegramBotParamMessages",
                column: "TelegramBotParamsTelegramBotId");

            migrationBuilder.InsertData(
        table: "TelegramBotParamMessages",
        columns: new[]
        {
            "TelegramBotId", "MessageName", "MessageDescription", "MessageValue", "MessageValueDefault", "IsButton"
        },
        values: new object[,]
        {
            { 1, "chooseTypeRegister", "Выбор способа регистрации", "Выберите способ регистрации", "Выберите способ регистрации", false },
            { 1, "alreadyRegister", "Уже авторизован пользователь", "Вы уже авторизированы", "Вы уже авторизированы", false },
            { 1, "successRegistration", "Успешная регистрация", "Поздравляем с регистрацией 🤝", "Поздравляем с регистрацией 🤝", false },
            { 1, "successEnter", "Успешный вход", "Доступ в систему успешно завершен 👌", "Доступ в систему успешно завершен 👌", false },
            { 1, "sendDataRegister", "Отправлены данные на регистрацию", "Данные для регистрации были отправлены. Секунду ⏳", "Данные для регистрации были отправлены. Секунду ⏳", false },
            { 1, "yesButton", "Положительный ответ", "Да", "Да", true },
            { 1, "noButton", "Отрицательный ответ", "Нет", "Нет", true },
            { 1, "skipButton", "Пропустить условие регистрации", "Пропустить", "Пропустить", true },
            { 1, "wrongFirstName", "Неверное имя", "Имя пустым быть не должно", "Имя пустым быть не должно", false },
            { 1, "manButton", "Мужской пол", "👨", "👨", true },
            { 1, "womanButton", "Женский пол", "👩", "👩", true },
            { 1, "enterGender", "Выбор гендера", "Выберите ваш пол", "Выберите ваш пол", false },
            { 1, "agreeButton", "Соглашение", "Я соглашаюсь", "Я соглашаюсь", true },
            { 1, "readButton", "Читать", "Читать", "Читать", true },
            { 1, "feedbackChoose", "Выбор темы обратной связи", "Оставьте своё обращение, выбрав тему внизу экрана 🤔 Ваш отзыв очень важен для нас 🤗", "Оставьте своё обращение, выбрав тему внизу экрана 🤔 Ваш отзыв очень важен для нас 🤗", false },
            { 1, "feedbackWrite", "Написать сообщение обратной связи", "Напишите Ваше обращение одним сообщением ✍️", "Напишите Ваше обращение одним сообщением ✍️", false },
            { 1, "feedbackProcessing", "Обработка сообщения обратной связи", "Мы оперативно обработаем полученную от Вас информацию. Спасибо большое за Вашу обратную связь 😇", "Мы оперативно обработаем полученную от Вас информацию. Спасибо большое за Вашу обратную связь 😇", false },
            { 1, "chooseTypeEnter", "Выбор способа входа", "Выберите, пожалуйста, способ входа", "Выберите, пожалуйста, способ входа", false },
            { 1, "transitionHelp", "Где искать пункты меню во время переходов в меню", "Пожалуйста, выберете, какую информацию вы хотите получить 👇", "Пожалуйста, выберете, какую информацию вы хотите получить 👇", false },
            { 1, "backButton", "Назад", "◀️ Назад", "◀️ Назад", true },
            { 1, "unknowCommand", "Неизвестная команда", "К сожалению, я не знаю что это %Message% 😢", "К сожалению, я не знаю что это %Message% 😢", false },
            { 1, "needAuthorization", "Необходимость регистрации/входа в системе", "Пройдите регистрацию или войдите в аккаунт, чтобы иметь доступ к полному функционалу 🙏", "Пройдите регистрацию или войдите в аккаунт, чтобы иметь доступ к полному функционалу 🙏", false },
            { 1, "wait", "Бот занят обработкой другой команды", "Я обрабатываю другую информацию, ожидайте ⏳", "Я обрабатываю другую информацию, ожидайте ⏳", false },
            { 1, "error", "Непредвиденная ошибка", "Ой, при обработке команды произошла ошибка 😩 Попробуйте эту команду позже, а я пока займусь её ремонтом 🛠", "Ой, при обработке команды произошла ошибка 😩 Попробуйте эту команду позже, а я пока займусь её ремонтом 🛠", false }
        });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramBotParamMessages");
        }
    }
}
