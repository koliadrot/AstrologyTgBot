using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class TelegramBotRegisterConditions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramBotCommand_TelegramParams_TelegramBotParamsTelegramB~",
                table: "TelegramBotCommand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TelegramBotCommand",
                table: "TelegramBotCommand");

            migrationBuilder.RenameTable(
                name: "TelegramBotCommand",
                newName: "TelegramBotCommands");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramBotCommand_TelegramBotParamsTelegramBotId",
                table: "TelegramBotCommands",
                newName: "IX_TelegramBotCommands_TelegramBotParamsTelegramBotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelegramBotCommands",
                table: "TelegramBotCommands",
                column: "BotCommandId");

            migrationBuilder.CreateTable(
                name: "TelegramBotRegisterConditions",
                columns: table => new
                {
                    RegisterConditionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TelegramBotId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConditionName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsCanPass = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsNecessarily = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsEnable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TelegramBotParamsTelegramBotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramBotRegisterConditions", x => x.RegisterConditionId);
                    table.ForeignKey(
                        name: "FK_TelegramBotRegisterConditions_TelegramParams_TelegramBotPara~",
                        column: x => x.TelegramBotParamsTelegramBotId,
                        principalTable: "TelegramParams",
                        principalColumn: "TelegramBotId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramBotRegisterConditions_TelegramBotParamsTelegramBotId",
                table: "TelegramBotRegisterConditions",
                column: "TelegramBotParamsTelegramBotId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramBotCommands_TelegramParams_TelegramBotParamsTelegram~",
                table: "TelegramBotCommands",
                column: "TelegramBotParamsTelegramBotId",
                principalTable: "TelegramParams",
                principalColumn: "TelegramBotId");

            migrationBuilder.InsertData(
                table: "TelegramBotRegisterConditions",
                columns: new[]
                {
                    "TelegramBotId", "Name", "ConditionName", "IsCanPass", "IsNecessarily",
                    "IsEnable", "Order"
                },
                values: new object[] { 1, "Имя", "FirstNameCondition", true, false, true, 2 });

            migrationBuilder.InsertData(
                table: "TelegramBotRegisterConditions",
                columns: new[]
                {
                    "TelegramBotId", "Name", "ConditionName", "IsCanPass", "IsNecessarily",
                    "IsEnable", "Order"
                },
                values: new object[] { 1, "Пол", "GenderCondition", true, false, true, 4 });

            migrationBuilder.InsertData(
                table: "TelegramBotRegisterConditions",
                columns: new[]
                {
                    "TelegramBotId", "Name", "ConditionName", "IsCanPass", "IsNecessarily",
                    "IsEnable", "Order"
                },
                values: new object[] { 1, "Политика программы", "TermOfUseCondition", false, true, true, 1001 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramBotCommands_TelegramParams_TelegramBotParamsTelegram~",
                table: "TelegramBotCommands");

            migrationBuilder.DropTable(
                name: "TelegramBotRegisterConditions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TelegramBotCommands",
                table: "TelegramBotCommands");

            migrationBuilder.RenameTable(
                name: "TelegramBotCommands",
                newName: "TelegramBotCommand");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramBotCommands_TelegramBotParamsTelegramBotId",
                table: "TelegramBotCommand",
                newName: "IX_TelegramBotCommand_TelegramBotParamsTelegramBotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelegramBotCommand",
                table: "TelegramBotCommand",
                column: "BotCommandId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramBotCommand_TelegramParams_TelegramBotParamsTelegramB~",
                table: "TelegramBotCommand",
                column: "TelegramBotParamsTelegramBotId",
                principalTable: "TelegramParams",
                principalColumn: "TelegramBotId");
        }
    }
}
