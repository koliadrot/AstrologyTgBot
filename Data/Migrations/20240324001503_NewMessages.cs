using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class NewMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "TelegramBotParamMessages",
            columns: new[] { "TelegramBotId", "MessageName", "MessageDescription", "MessageValue", "MessageValueDefault", "IsButton", "IsSystem" },
            values: new object[,]
            {
                { 1, "sendNotifyLikes", "Отправляет уведомление о лайках", "У вас есть симпатии ❤ (%Likes%)\nВперед смотреть🙌\n👇👇👇", "У вас есть симпатии ❤ (%Likes%)\nВперед смотреть🙌\n👇👇👇", false, true },
                { 1, "noActualFindClients", "Отстуствие анкет по потребностям", "К сожалению, пока нет анкет по твоим предпочтениям😥", "К сожалению, пока нет анкет по твоим предпочтениям😥", false, true },
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM TelegramBotParamMessages WHERE MessageName IN
                ('sendNotifyLikes',
                'noActualFindClients')");
        }
    }
}
