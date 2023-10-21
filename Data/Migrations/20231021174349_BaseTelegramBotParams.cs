using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class BaseTelegramBotParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TelegramParams",
                columns: table => new
                {
                    TelegramBotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BotName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BotUserName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TokenApi = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WebHookUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TosUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AcceptPromotionsBySms = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AcceptElectronicReceipts = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HelloText = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Menu = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramParams", x => x.TelegramBotId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Вставьте данные при миграции
            migrationBuilder.InsertData(
                table: "TelegramParams",
                columns: new[]
                {
            "BotName", "BotUserName", "TokenApi", "WebHookUrl", "TosUrl",
            "AcceptPromotionsBySms", "AcceptElectronicReceipts", "HelloText", "Menu"
                },
                values: new object[]
                {
            "АстроСовместимость | Blazz - знакомства,новые друзья,любовь!",
            "blazzmatchbot",
            "6832421683:AAGVGRbvk2qJuMaqIMCwbtw2yq7GQ4f9gNA",
            "https://close-unduly-maggot.ngrok-free.app",
            "Обработка персональных данных. Согласны?",
            true, true,
            "Привет Мир!",
            "{ \"StartMenu\": { \"Items\": [ { \"CommandName\": \"/register-0\", \"Name\": \"Регистрация\", \"SubMenu\": null }, { \"CommandName\": \"/signin-0\", \"Name\": \"Вход\", \"SubMenu\": null } ] }, \"AuthMenu\": { \"Items\": [ { \"CommandName\": \"/card-0\", \"Name\": \"Карта\", \"SubMenu\": null }, { \"CommandName\": \"/transition-0\", \"Name\": \"Личный кабинет\", \"SubMenu\": { \"Items\": [ { \"CommandName\": \"/invitefriend-1\", \"Name\": \"Приведи друга\", \"SubMenu\": null }, { \"CommandName\": \"/balance-1\", \"Name\": \"Баланс\", \"SubMenu\": null }, { \"CommandName\": \"/purchases-1\", \"Name\": \"Покупки\", \"SubMenu\": null }, { \"CommandName\": \"/transition-1\", \"Name\": \"Информация\", \"SubMenu\": { \"Items\": [ { \"CommandName\": \"/feedback-2\", \"Name\": \"Обратная связь\", \"SubMenu\": null } ] } } ] } } ] } }"
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramParams");
        }
    }
}
