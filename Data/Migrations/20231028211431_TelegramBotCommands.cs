using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class TelegramBotCommands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelegramBotCommand",
                columns: table => new
                {
                    BotCommandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TelegramBotId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CommandName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CommandType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAuth = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsEnable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AdditionalData = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelegramBotParamsTelegramBotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramBotCommand", x => x.BotCommandId);
                    table.ForeignKey(
                        name: "FK_TelegramBotCommand_TelegramParams_TelegramBotParamsTelegramB~",
                        column: x => x.TelegramBotParamsTelegramBotId,
                        principalTable: "TelegramParams",
                        principalColumn: "TelegramBotId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBotCommand_TelegramBotParamsTelegramBotId",
                table: "TelegramBotCommand",
                column: "TelegramBotParamsTelegramBotId");

            migrationBuilder.InsertData(
                table: "TelegramBotCommand",
                columns: new[]
                {
                    "TelegramBotId", "Name", "Description", "CommandName", "CommandType",
                    "IsAuth", "IsDefault", "IsEnable", "IsPublic", "AdditionalData"
                },
                values: new object[]
                {
                    1, "Переход", "Переход в уровень меню", "/transition", "Custom", false, true, true, false, ""
                });

            migrationBuilder.InsertData(
                table: "TelegramBotCommand",
                columns: new[]
                {
                    "TelegramBotId", "Name", "Description", "CommandName", "CommandType",
                    "IsAuth", "IsDefault", "IsEnable", "IsPublic", "AdditionalData"
                },
                values: new object[]
                {
                    1, "Меню", "Главное меню", "/menu", "Custom", false, true, true, false, ""
                });

            migrationBuilder.InsertData(
                table: "TelegramBotCommand",
                columns: new[]
                {
                    "TelegramBotId", "Name", "Description", "CommandName", "CommandType",
                    "IsAuth", "IsDefault", "IsEnable", "IsPublic", "AdditionalData"
                },
                values: new object[]
                {
                    1, "Регистрация", "Регистрация в системе", "/register", "Custom", false, false, true, false, ""
                });

            migrationBuilder.InsertData(
                table: "TelegramBotCommand",
                columns: new[]
                {
                    "TelegramBotId", "Name", "Description", "CommandName", "CommandType",
                    "IsAuth", "IsDefault", "IsEnable", "IsPublic", "AdditionalData"
                },
                values: new object[]
                {
                    1, "Вход", "Вход в систему", "/signin", "Custom", false, false, true, false, ""
                });

            migrationBuilder.InsertData(
                table: "TelegramBotCommand",
                columns: new[]
                {
                    "TelegramBotId", "Name", "Description", "CommandName", "CommandType",
                    "IsAuth", "IsDefault", "IsEnable", "IsPublic", "AdditionalData"
                },
                values: new object[]
                {
                    1, "Инфо", "Информация о боте", "/start", "Custom", false, true, true, false, ""
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramBotCommand");
        }
    }
}
