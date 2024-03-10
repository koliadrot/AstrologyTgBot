using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FindClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInfo",
                table: "TelegramBotRegisterConditions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvatar",
                table: "ClientVideoInfos",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlock",
                table: "Clients",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Clients",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvatar",
                table: "ClientPhotoInfos",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientMatchInfos",
                columns: table => new
                {
                    ClientMatchInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    LetterLikes = table.Column<int>(type: "int", nullable: false),
                    Dislikes = table.Column<int>(type: "int", nullable: false),
                    LastShowMatches = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientMatchInfos", x => x.ClientMatchInfoId);
                    table.ForeignKey(
                        name: "FK_ClientMatchInfos_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientVideoNoteInfos",
                columns: table => new
                {
                    ClientVideoNoteInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientMediaInfoId = table.Column<int>(type: "int", nullable: false),
                    MediaGroupId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAvatar = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    FileId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileUniqueId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    VideoNote = table.Column<byte[]>(type: "longblob", nullable: true),
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
                    table.PrimaryKey("PK_ClientVideoNoteInfos", x => x.ClientVideoNoteInfoId);
                    table.ForeignKey(
                        name: "FK_ClientVideoNoteInfos_ClientMediaInfos_ClientMediaInfoId",
                        column: x => x.ClientMediaInfoId,
                        principalTable: "ClientMediaInfos",
                        principalColumn: "ClientMediaInfoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CheckedClientMatchs",
                columns: table => new
                {
                    ClientMatchCheckedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientMatchInfoId = table.Column<int>(type: "int", nullable: false),
                    DateMatch = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MatchType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnswearDateMatch = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AnswearMatchType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MatchTelegramId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoveLetterText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckedClientMatchs", x => x.ClientMatchCheckedId);
                    table.ForeignKey(
                        name: "FK_CheckedClientMatchs_ClientMatchInfos_ClientMatchInfoId",
                        column: x => x.ClientMatchInfoId,
                        principalTable: "ClientMatchInfos",
                        principalColumn: "ClientMatchInfoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UncheckedClientMatchs",
                columns: table => new
                {
                    ClientMatchUncheckedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsWatched = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ClientMatchInfoId = table.Column<int>(type: "int", nullable: false),
                    DateMatch = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MatchType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnswearDateMatch = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AnswearMatchType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MatchTelegramId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoveLetterText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UncheckedClientMatchs", x => x.ClientMatchUncheckedId);
                    table.ForeignKey(
                        name: "FK_UncheckedClientMatchs_ClientMatchInfos_ClientMatchInfoId",
                        column: x => x.ClientMatchInfoId,
                        principalTable: "ClientMatchInfos",
                        principalColumn: "ClientMatchInfoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientMatchCheckedVideoInfos",
                columns: table => new
                {
                    ClientMatchCheckedVideoInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientMatchCheckedId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ClientMatchCheckedVideoInfos", x => x.ClientMatchCheckedVideoInfoId);
                    table.ForeignKey(
                        name: "FK_ClientMatchCheckedVideoInfos_CheckedClientMatchs_ClientMatch~",
                        column: x => x.ClientMatchCheckedId,
                        principalTable: "CheckedClientMatchs",
                        principalColumn: "ClientMatchCheckedId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientMatchCheckedVideoNoteInfos",
                columns: table => new
                {
                    ClientMatchCheckedVideoNoteInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientMatchCheckedId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileUniqueId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    VideoNote = table.Column<byte[]>(type: "longblob", nullable: true),
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
                    table.PrimaryKey("PK_ClientMatchCheckedVideoNoteInfos", x => x.ClientMatchCheckedVideoNoteInfoId);
                    table.ForeignKey(
                        name: "FK_ClientMatchCheckedVideoNoteInfos_CheckedClientMatchs_ClientM~",
                        column: x => x.ClientMatchCheckedId,
                        principalTable: "CheckedClientMatchs",
                        principalColumn: "ClientMatchCheckedId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientMatchUncheckedVideoInfos",
                columns: table => new
                {
                    ClientMatchUncheckedVideoInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientMatchUncheckedId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ClientMatchUncheckedVideoInfos", x => x.ClientMatchUncheckedVideoInfoId);
                    table.ForeignKey(
                        name: "FK_ClientMatchUncheckedVideoInfos_UncheckedClientMatchs_ClientM~",
                        column: x => x.ClientMatchUncheckedId,
                        principalTable: "UncheckedClientMatchs",
                        principalColumn: "ClientMatchUncheckedId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientMatchUncheckedVideoNoteInfos",
                columns: table => new
                {
                    ClientMatchUncheckedVideoNoteInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientMatchUncheckedId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileUniqueId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    VideoNote = table.Column<byte[]>(type: "longblob", nullable: true),
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
                    table.PrimaryKey("PK_ClientMatchUncheckedVideoNoteInfos", x => x.ClientMatchUncheckedVideoNoteInfoId);
                    table.ForeignKey(
                        name: "FK_ClientMatchUncheckedVideoNoteInfos_UncheckedClientMatchs_Cli~",
                        column: x => x.ClientMatchUncheckedId,
                        principalTable: "UncheckedClientMatchs",
                        principalColumn: "ClientMatchUncheckedId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CheckedClientMatchs_ClientMatchInfoId",
                table: "CheckedClientMatchs",
                column: "ClientMatchInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMatchCheckedVideoInfos_ClientMatchCheckedId",
                table: "ClientMatchCheckedVideoInfos",
                column: "ClientMatchCheckedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientMatchCheckedVideoNoteInfos_ClientMatchCheckedId",
                table: "ClientMatchCheckedVideoNoteInfos",
                column: "ClientMatchCheckedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientMatchInfos_ClientId",
                table: "ClientMatchInfos",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientMatchUncheckedVideoInfos_ClientMatchUncheckedId",
                table: "ClientMatchUncheckedVideoInfos",
                column: "ClientMatchUncheckedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientMatchUncheckedVideoNoteInfos_ClientMatchUncheckedId",
                table: "ClientMatchUncheckedVideoNoteInfos",
                column: "ClientMatchUncheckedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientVideoNoteInfos_ClientMediaInfoId",
                table: "ClientVideoNoteInfos",
                column: "ClientMediaInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UncheckedClientMatchs_ClientMatchInfoId",
                table: "UncheckedClientMatchs",
                column: "ClientMatchInfoId");

            migrationBuilder.InsertData(
               table: "TelegramBotParamMessages",
               columns: new[] { "TelegramBotId", "MessageName", "MessageDescription", "MessageValue", "MessageValueDefault", "IsButton", "IsSystem" },
               values: new object[,]
               {
                    { 1, "noVariantChoice", "Отсуствие выбора", "Нет такого варианта выбора", "Нет такого варианта выбора", false, true },
                    { 1, "sendLikeMatch", "Сообщает о симпатии другого пользователя", "Этому пользователю вы пригянулись❤", "Этому пользователю вы пригянулись❤", false, true },
                    { 1, "sendLoveLetterMatch", "Отправляет сообщение симпатии с любовным сообщением", "Этому пользователю вы пригянулись❤\nВам💌: \"%Message%\"", "Этому пользователю вы пригянулись❤\nВам💌: \"%Message%\"", false, true },
                    { 1, "sendVideoMatch", "Отправляет сообщение симпатии с видео", "Этому пользователю вы пригянулись❤", "Этому пользователю вы пригянулись❤", false, true },
                    { 1, "defaultFindApplication", "Приветственное сообщение по умолчанию у команды просмотра анкет", "Удачи!", "Удачи!", false, true },
                    { 1, "defaultCheckMatch", "Приветственное сообщение по умолчанию у команды просмотра совпадений анкет", "Помните, что люди могут выдавать себя за других!\nУдачи!", "Помните, что люди могут выдавать себя за других!\nУдачи!", false, true },
                    { 1, "noClientMatchs", "Отсутствуют симпатии", "Новые совпадения отсутствуют!\nДавай пойдем искать первыми👇", "Новые совпадения отсутствуют!\nДавай пойдем искать первыми👇", false, true },
                    { 1, "writeLoveLetter", "Написать любовное письмо", "Напиши сообщение для этого пользователя или запиши короткое видео(до %Duration% секунд)", "Напиши сообщение для этого пользователя или запиши короткое видео(до %Duration% секунд)", false, true },
                    { 1, "sendLoveLetter", "Отправлено любовное письмо", "Сообщение отправлено🤗\nОжидаем ответа😤", "Сообщение отправлено🤗\nОжидаем ответа😤", false, true },
                    { 1, "moreLoadMedia", "Предложение загрузить больше медиа файлов", "Хотите загрузить больше медиа файлов?🧐\nДоступно еще (%Media%)", "Хотите загрузить больше медиа файлов?🧐\nДоступно еще (%Media%)", false, true },
                    { 1, "sendMatchMyClient", "Отправить сообщение о взаимное симпатии себе", "Вы понравились❤\nНе бойтесь писать первыми,дерзайте! Живите!\nУдачи пообщаться\U0001f970\n👇👇👇\n%UserName%", "Вы понравились❤\nНе бойтесь писать первыми,дерзайте! Живите!\nУдачи пообщаться\U0001f970\n👇👇👇\n%UserName%", false, true },
                    { 1, "sendMatchOtherClient", "Отправить сообщение о взаимной симпатии другому пользователю", "Не бойтесь писать первыми,дерзайте! Живите!\nУдачи пообщаться\U0001f970\n👇👇👇\n%UserName%", "Не бойтесь писать первыми,дерзайте! Живите!\nУдачи пообщаться\U0001f970\n👇👇👇\n%UserName%", false, true },
                    { 1, "sendNotifyNewLikes", "Отправляет уведомление о новых лайках", "У вас новые симпатии ❤ (%Likes%)\nВперед смотреть🙌\n👇👇👇", "У вас новые симпатии ❤ (%Likes%)\nВперед смотреть🙌\n👇👇👇", false, true },
                    { 1, "countLeftLikes", "Сколько осталось лайков у пользователя", "Осталось симпатий ❤ (%Likes%)", "Осталось симпатий ❤ (%Likes%)", false, true },
                    { 1, "enterAvatar", "Отправить аватар", "Отправьте аватар!\nФотографию или видео (до %Duration% секунд)", "Отправьте аватар!\nФотографию или видео (до %Duration% секунд)", false, true },
                    { 1, "like", "Нравится", "❤", "❤", true, true },
                    { 1, "loveletter", "Любовное письмо", "💌/📹", "💌/📹", true, true },
                    { 1, "dislike", "Не нравится", "👎", "👎", true, true },
                    { 1, "profile", "Профиль", "📋", "📋", true, true },
                    { 1, "moreMedia", "Медиа файлы профиля", "Еще фотки", "Еще фотки", true, true },
                    { 1, "findApplications", "Просмотр анкет", "Смотреть анкеты", "Смотреть анкеты", true, true },
                    { 1, "watchLikes", "Просмотр лайков", "Смотреть лайки", "Смотреть лайки", true, true },
                    { 1, "showApplication", "Просмотр анкеты", "Показать анкету", "Показать анкету", true, true },

               });

            migrationBuilder.InsertData(
                table: "TelegramBotCommands",
                columns: new[] { "TelegramBotId", "Name", "Description", "CommandName", "CommandType", "IsAuth", "IsDefault", "IsEnable", "IsPublic", "AdditionalData" },
                values: new object[,] {
                    { 1, "Поиск анкет", "Поиск анкет", "/findapplication", "Custom", true, false, true, false, null },
                    { 1, "Мои лайки", "Проверка совпадений", "/checkmatch", "Custom", true, false, true, false, null }
                });

            migrationBuilder.UpdateData(
                table: "TelegramBotRegisterConditions",
                keyColumn: "ConditionName",
                keyValue: "MediaCondition",
                column: "Order",
                value: 12);

            migrationBuilder.Sql(@"
                UPDATE TelegramBotRegisterConditions 
                SET IsInfo = CASE 
                    WHEN ConditionName = 'MediaCondition' THEN 1
                    WHEN ConditionName = 'AboutMeCondition' THEN 1
                    ELSE IsInfo
                END
                WHERE ConditionName IN ('MediaCondition', 'AboutMeCondition');
            ");


            migrationBuilder.InsertData(
                table: "TelegramBotRegisterConditions",
                columns: new[] { "TelegramBotId", "Name", "ConditionName", "IsCanPass", "IsNecessarily", "IsEnable", "Order", "IsInfo" },
                values: new object[] { 1, "Аватар профиля", "AvatarCondition", false, false, true, 11, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM TelegramBotParamMessages WHERE MessageName IN
                ('noVariantChoice',
                'defaultFindApplication',
                'defaultCheckMatch',
                'sendLoveLetter',
                'writeLoveLetter',
                'like',
                'loveletter',
                'dislike',
                'moreMedia',
                'moreLoadMedia',
                'enterAvatar',
                'findApplications',
                'sendMatchMyClient',
                'sendMatchOtherClient',
                'sendLikeMatch',
                'sendLoveLetterMatch',
                'sendVideoMatch',
                'noClientMatchs',
                'sendNotifyNewLikes',
                'countLeftLikes',
                'watchLikes',
                'showApplication',
                'profile')");

            migrationBuilder.DeleteData(
                table: "TelegramBotCommands",
                keyColumn: "CommandName",
                keyValue: "/findapplication");

            migrationBuilder.DeleteData(
                table: "TelegramBotRegisterConditions",
                keyColumn: "ConditionName",
                keyValue: "AvatarCondition");

            migrationBuilder.DropTable(
                name: "ClientMatchCheckedVideoInfos");

            migrationBuilder.DropTable(
                name: "ClientMatchCheckedVideoNoteInfos");

            migrationBuilder.DropTable(
                name: "ClientMatchUncheckedVideoInfos");

            migrationBuilder.DropTable(
                name: "ClientMatchUncheckedVideoNoteInfos");

            migrationBuilder.DropTable(
                name: "ClientVideoNoteInfos");

            migrationBuilder.DropTable(
                name: "CheckedClientMatchs");

            migrationBuilder.DropTable(
                name: "UncheckedClientMatchs");

            migrationBuilder.DropTable(
                name: "ClientMatchInfos");

            migrationBuilder.DropColumn(
                name: "IsInfo",
                table: "TelegramBotRegisterConditions");

            migrationBuilder.DropColumn(
                name: "IsAvatar",
                table: "ClientVideoInfos");

            migrationBuilder.DropColumn(
                name: "IsBlock",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsAvatar",
                table: "ClientPhotoInfos");
        }
    }
}
