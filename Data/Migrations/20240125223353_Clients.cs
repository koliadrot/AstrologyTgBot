using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Clients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptElectronicReceipts",
                table: "TelegramParams");

            migrationBuilder.DropColumn(
                name: "AcceptPromotionsBySms",
                table: "TelegramParams");

            migrationBuilder.AlterColumn<string>(
                name: "WebHookUrl",
                table: "TelegramParams",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TosUrl",
                table: "TelegramParams",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TokenApi",
                table: "TelegramParams",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Menu",
                table: "TelegramParams",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "HelloText",
                table: "TelegramParams",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BotUserName",
                table: "TelegramParams",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BotName",
                table: "TelegramParams",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BotAbout",
                table: "TelegramParams",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BotDescription",
                table: "TelegramParams",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LastStatus",
                table: "TelegramParams",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "TelegramBotParamMessages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TelegramId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BirthGender = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Birthday = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    BirthCoord = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BirthCity = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SearchGender = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SearchAge = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SearchCity = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SearchCoord = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SearchGoal = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AboutMe = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientMediaInfos",
                columns: table => new
                {
                    ClientMediaInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientMediaInfos", x => x.ClientMediaInfoId);
                    table.ForeignKey(
                        name: "FK_ClientMediaInfos_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientPhotoInfos",
                columns: table => new
                {
                    ClientPhotoInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientMediaInfoId = table.Column<int>(type: "int", nullable: false),
                    MediaGroupId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileUniqueId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<byte[]>(type: "longblob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPhotoInfos", x => x.ClientPhotoInfoId);
                    table.ForeignKey(
                        name: "FK_ClientPhotoInfos_ClientMediaInfos_ClientMediaInfoId",
                        column: x => x.ClientMediaInfoId,
                        principalTable: "ClientMediaInfos",
                        principalColumn: "ClientMediaInfoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientVideoInfos",
                columns: table => new
                {
                    ClientVideoInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientMediaInfoId = table.Column<int>(type: "int", nullable: false),
                    MediaGroupId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileUniqueId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    MimeType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Video = table.Column<byte[]>(type: "longblob", nullable: true),
                    ThumbnailFileId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThumbnailFileUniqueId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThumbnailFileSize = table.Column<long>(type: "bigint", nullable: true),
                    ThumbnailWidth = table.Column<int>(type: "int", nullable: true),
                    ThumbnailHeight = table.Column<int>(type: "int", nullable: true),
                    Thumbnail = table.Column<byte[]>(type: "longblob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientVideoInfos", x => x.ClientVideoInfoId);
                    table.ForeignKey(
                        name: "FK_ClientVideoInfos_ClientMediaInfos_ClientMediaInfoId",
                        column: x => x.ClientMediaInfoId,
                        principalTable: "ClientMediaInfos",
                        principalColumn: "ClientMediaInfoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMediaInfos_ClientId",
                table: "ClientMediaInfos",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientPhotoInfos_ClientMediaInfoId",
                table: "ClientPhotoInfos",
                column: "ClientMediaInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientVideoInfos_ClientMediaInfoId",
                table: "ClientVideoInfos",
                column: "ClientMediaInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientPhotoInfos");

            migrationBuilder.DropTable(
                name: "ClientVideoInfos");

            migrationBuilder.DropTable(
                name: "ClientMediaInfos");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropColumn(
                name: "BotAbout",
                table: "TelegramParams");

            migrationBuilder.DropColumn(
                name: "BotDescription",
                table: "TelegramParams");

            migrationBuilder.DropColumn(
                name: "LastStatus",
                table: "TelegramParams");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "TelegramBotParamMessages");

            migrationBuilder.UpdateData(
                table: "TelegramParams",
                keyColumn: "WebHookUrl",
                keyValue: null,
                column: "WebHookUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "WebHookUrl",
                table: "TelegramParams",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TelegramParams",
                keyColumn: "TosUrl",
                keyValue: null,
                column: "TosUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "TosUrl",
                table: "TelegramParams",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TelegramParams",
                keyColumn: "TokenApi",
                keyValue: null,
                column: "TokenApi",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "TokenApi",
                table: "TelegramParams",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TelegramParams",
                keyColumn: "Menu",
                keyValue: null,
                column: "Menu",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Menu",
                table: "TelegramParams",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TelegramParams",
                keyColumn: "HelloText",
                keyValue: null,
                column: "HelloText",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "HelloText",
                table: "TelegramParams",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TelegramParams",
                keyColumn: "BotUserName",
                keyValue: null,
                column: "BotUserName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "BotUserName",
                table: "TelegramParams",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TelegramParams",
                keyColumn: "BotName",
                keyValue: null,
                column: "BotName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "BotName",
                table: "TelegramParams",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "AcceptElectronicReceipts",
                table: "TelegramParams",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AcceptPromotionsBySms",
                table: "TelegramParams",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
