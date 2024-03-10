﻿// <auto-generated />
using System;
using Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Data.Core.Models.ApiAccessToken", b =>
                {
                    b.Property<int>("ApiAccessTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ApiAccessTokenId");

                    b.HasIndex("UserId");

                    b.ToTable("ApiAccessTokens");
                });

            modelBuilder.Entity("Data.Core.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ClientId")
                        .HasColumnOrder(1);

                    b.Property<string>("AboutMe")
                        .HasColumnType("longtext");

                    b.Property<string>("BirthCity")
                        .HasColumnType("longtext");

                    b.Property<string>("BirthCoord")
                        .HasColumnType("longtext");

                    b.Property<string>("BirthGender")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<bool?>("IsBlock")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SearchAge")
                        .HasColumnType("longtext");

                    b.Property<string>("SearchCity")
                        .HasColumnType("longtext");

                    b.Property<string>("SearchCoord")
                        .HasColumnType("longtext");

                    b.Property<string>("SearchGender")
                        .HasColumnType("longtext");

                    b.Property<string>("SearchGoal")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<string>("TelegramId")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchChecked", b =>
                {
                    b.Property<int>("ClientMatchCheckedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("AnswearDateMatch")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("AnswearMatchType")
                        .HasColumnType("longtext");

                    b.Property<int>("ClientMatchInfoId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateMatch")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LoveLetterText")
                        .HasColumnType("longtext");

                    b.Property<string>("MatchTelegramId")
                        .HasColumnType("longtext");

                    b.Property<string>("MatchType")
                        .HasColumnType("longtext");

                    b.HasKey("ClientMatchCheckedId");

                    b.HasIndex("ClientMatchInfoId");

                    b.ToTable("CheckedClientMatchs");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchCheckedVideoInfo", b =>
                {
                    b.Property<int>("ClientMatchCheckedVideoInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientMatchCheckedId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileUniqueId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("MimeType")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Thumbnail")
                        .HasColumnType("longblob");

                    b.Property<string>("ThumbnailFileId")
                        .HasColumnType("longtext");

                    b.Property<long?>("ThumbnailFileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("ThumbnailFileUniqueId")
                        .HasColumnType("longtext");

                    b.Property<int?>("ThumbnailHeight")
                        .HasColumnType("int");

                    b.Property<int?>("ThumbnailWidth")
                        .HasColumnType("int");

                    b.Property<byte[]>("Video")
                        .HasColumnType("longblob");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("ClientMatchCheckedVideoInfoId");

                    b.HasIndex("ClientMatchCheckedId")
                        .IsUnique();

                    b.ToTable("ClientMatchCheckedVideoInfos");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchCheckedVideoNoteInfo", b =>
                {
                    b.Property<int>("ClientMatchCheckedVideoNoteInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientMatchCheckedId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileUniqueId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<byte[]>("Thumbnail")
                        .HasColumnType("longblob");

                    b.Property<string>("ThumbnailFileId")
                        .HasColumnType("longtext");

                    b.Property<long?>("ThumbnailFileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("ThumbnailFileUniqueId")
                        .HasColumnType("longtext");

                    b.Property<int?>("ThumbnailHeight")
                        .HasColumnType("int");

                    b.Property<int?>("ThumbnailWidth")
                        .HasColumnType("int");

                    b.Property<byte[]>("VideoNote")
                        .HasColumnType("longblob");

                    b.HasKey("ClientMatchCheckedVideoNoteInfoId");

                    b.HasIndex("ClientMatchCheckedId")
                        .IsUnique();

                    b.ToTable("ClientMatchCheckedVideoNoteInfos");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchInfo", b =>
                {
                    b.Property<int>("ClientMatchInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("Dislikes")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastShowMatches")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LetterLikes")
                        .HasColumnType("int");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.HasKey("ClientMatchInfoId");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("ClientMatchInfos");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchUnchecked", b =>
                {
                    b.Property<int>("ClientMatchUncheckedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("AnswearDateMatch")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("AnswearMatchType")
                        .HasColumnType("longtext");

                    b.Property<int>("ClientMatchInfoId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateMatch")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsWatched")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LoveLetterText")
                        .HasColumnType("longtext");

                    b.Property<string>("MatchTelegramId")
                        .HasColumnType("longtext");

                    b.Property<string>("MatchType")
                        .HasColumnType("longtext");

                    b.HasKey("ClientMatchUncheckedId");

                    b.HasIndex("ClientMatchInfoId");

                    b.ToTable("UncheckedClientMatchs");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchUncheckedVideoInfo", b =>
                {
                    b.Property<int>("ClientMatchUncheckedVideoInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientMatchUncheckedId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileUniqueId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("MimeType")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Thumbnail")
                        .HasColumnType("longblob");

                    b.Property<string>("ThumbnailFileId")
                        .HasColumnType("longtext");

                    b.Property<long?>("ThumbnailFileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("ThumbnailFileUniqueId")
                        .HasColumnType("longtext");

                    b.Property<int?>("ThumbnailHeight")
                        .HasColumnType("int");

                    b.Property<int?>("ThumbnailWidth")
                        .HasColumnType("int");

                    b.Property<byte[]>("Video")
                        .HasColumnType("longblob");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("ClientMatchUncheckedVideoInfoId");

                    b.HasIndex("ClientMatchUncheckedId")
                        .IsUnique();

                    b.ToTable("ClientMatchUncheckedVideoInfos");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchUncheckedVideoNoteInfo", b =>
                {
                    b.Property<int>("ClientMatchUncheckedVideoNoteInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientMatchUncheckedId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileUniqueId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<byte[]>("Thumbnail")
                        .HasColumnType("longblob");

                    b.Property<string>("ThumbnailFileId")
                        .HasColumnType("longtext");

                    b.Property<long?>("ThumbnailFileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("ThumbnailFileUniqueId")
                        .HasColumnType("longtext");

                    b.Property<int?>("ThumbnailHeight")
                        .HasColumnType("int");

                    b.Property<int?>("ThumbnailWidth")
                        .HasColumnType("int");

                    b.Property<byte[]>("VideoNote")
                        .HasColumnType("longblob");

                    b.HasKey("ClientMatchUncheckedVideoNoteInfoId");

                    b.HasIndex("ClientMatchUncheckedId")
                        .IsUnique();

                    b.ToTable("ClientMatchUncheckedVideoNoteInfos");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMediaInfo", b =>
                {
                    b.Property<int>("ClientMediaInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.HasKey("ClientMediaInfoId");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("ClientMediaInfos");
                });

            modelBuilder.Entity("Data.Core.Models.ClientPhotoInfo", b =>
                {
                    b.Property<int>("ClientPhotoInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientMediaInfoId")
                        .HasColumnType("int");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileUniqueId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<bool?>("IsAvatar")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MediaGroupId")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("longblob");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("ClientPhotoInfoId");

                    b.HasIndex("ClientMediaInfoId");

                    b.ToTable("ClientPhotoInfos");
                });

            modelBuilder.Entity("Data.Core.Models.ClientVideoInfo", b =>
                {
                    b.Property<int>("ClientVideoInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientMediaInfoId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileUniqueId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<bool?>("IsAvatar")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MediaGroupId")
                        .HasColumnType("longtext");

                    b.Property<string>("MimeType")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Thumbnail")
                        .HasColumnType("longblob");

                    b.Property<string>("ThumbnailFileId")
                        .HasColumnType("longtext");

                    b.Property<long?>("ThumbnailFileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("ThumbnailFileUniqueId")
                        .HasColumnType("longtext");

                    b.Property<int?>("ThumbnailHeight")
                        .HasColumnType("int");

                    b.Property<int?>("ThumbnailWidth")
                        .HasColumnType("int");

                    b.Property<byte[]>("Video")
                        .HasColumnType("longblob");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("ClientVideoInfoId");

                    b.HasIndex("ClientMediaInfoId");

                    b.ToTable("ClientVideoInfos");
                });

            modelBuilder.Entity("Data.Core.Models.ClientVideoNoteInfo", b =>
                {
                    b.Property<int>("ClientVideoNoteInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientMediaInfoId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileUniqueId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool?>("IsAvatar")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("MediaGroupId")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Thumbnail")
                        .HasColumnType("longblob");

                    b.Property<string>("ThumbnailFileId")
                        .HasColumnType("longtext");

                    b.Property<long?>("ThumbnailFileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("ThumbnailFileUniqueId")
                        .HasColumnType("longtext");

                    b.Property<int?>("ThumbnailHeight")
                        .HasColumnType("int");

                    b.Property<int?>("ThumbnailWidth")
                        .HasColumnType("int");

                    b.Property<byte[]>("VideoNote")
                        .HasColumnType("longblob");

                    b.HasKey("ClientVideoNoteInfoId");

                    b.HasIndex("ClientMediaInfoId");

                    b.ToTable("ClientVideoNoteInfos");
                });

            modelBuilder.Entity("Data.Core.Models.TelegramBotCommand", b =>
                {
                    b.Property<int>("BotCommandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AdditionalData")
                        .HasColumnType("longtext");

                    b.Property<string>("CommandName")
                        .HasColumnType("longtext");

                    b.Property<string>("CommandType")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsAuth")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsEnable")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("TelegramBotId")
                        .HasColumnType("int");

                    b.Property<int?>("TelegramBotParamsTelegramBotId")
                        .HasColumnType("int");

                    b.HasKey("BotCommandId");

                    b.HasIndex("TelegramBotParamsTelegramBotId");

                    b.ToTable("TelegramBotCommands");
                });

            modelBuilder.Entity("Data.Core.Models.TelegramBotParamMessage", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsButton")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsSystem")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MessageDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MessageName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MessageValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MessageValueDefault")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TelegramBotId")
                        .HasColumnType("int");

                    b.Property<int?>("TelegramBotParamsTelegramBotId")
                        .HasColumnType("int");

                    b.HasKey("MessageId");

                    b.HasIndex("TelegramBotParamsTelegramBotId");

                    b.ToTable("TelegramBotParamMessages");
                });

            modelBuilder.Entity("Data.Core.Models.TelegramBotParams", b =>
                {
                    b.Property<int>("TelegramBotId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BotAbout")
                        .HasColumnType("longtext");

                    b.Property<string>("BotDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("BotName")
                        .HasColumnType("longtext");

                    b.Property<string>("BotUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("HelloText")
                        .HasColumnType("longtext");

                    b.Property<string>("LastStatus")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Menu")
                        .HasColumnType("longtext");

                    b.Property<string>("TokenApi")
                        .HasColumnType("longtext");

                    b.Property<string>("TosUrl")
                        .HasColumnType("longtext");

                    b.Property<string>("WebHookUrl")
                        .HasColumnType("longtext");

                    b.HasKey("TelegramBotId");

                    b.ToTable("TelegramParams");
                });

            modelBuilder.Entity("Data.Core.Models.TelegramBotRegisterCondition", b =>
                {
                    b.Property<int>("RegisterConditionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConditionName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsCanPass")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsEnable")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsInfo")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsNecessarily")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("TelegramBotId")
                        .HasColumnType("int");

                    b.Property<int?>("TelegramBotParamsTelegramBotId")
                        .HasColumnType("int");

                    b.HasKey("RegisterConditionId");

                    b.HasIndex("TelegramBotParamsTelegramBotId");

                    b.ToTable("TelegramBotRegisterConditions");
                });

            modelBuilder.Entity("Data.Core.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Data.Core.Models.ApiAccessToken", b =>
                {
                    b.HasOne("Data.Core.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchChecked", b =>
                {
                    b.HasOne("Data.Core.Models.ClientMatchInfo", "ClientMatchInfo")
                        .WithMany("CheckedClientMatchs")
                        .HasForeignKey("ClientMatchInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientMatchInfo");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchCheckedVideoInfo", b =>
                {
                    b.HasOne("Data.Core.Models.ClientMatchChecked", "ClientMatchChecked")
                        .WithOne("ClientMatchCheckedVideoInfo")
                        .HasForeignKey("Data.Core.Models.ClientMatchCheckedVideoInfo", "ClientMatchCheckedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientMatchChecked");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchCheckedVideoNoteInfo", b =>
                {
                    b.HasOne("Data.Core.Models.ClientMatchChecked", "ClientMatchChecked")
                        .WithOne("ClientMatchCheckedVideoNoteInfo")
                        .HasForeignKey("Data.Core.Models.ClientMatchCheckedVideoNoteInfo", "ClientMatchCheckedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientMatchChecked");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchInfo", b =>
                {
                    b.HasOne("Data.Core.Models.Client", "Client")
                        .WithOne("ClientMatchInfo")
                        .HasForeignKey("Data.Core.Models.ClientMatchInfo", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchUnchecked", b =>
                {
                    b.HasOne("Data.Core.Models.ClientMatchInfo", "ClientMatchInfo")
                        .WithMany("UncheckedClientMatchs")
                        .HasForeignKey("ClientMatchInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientMatchInfo");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchUncheckedVideoInfo", b =>
                {
                    b.HasOne("Data.Core.Models.ClientMatchUnchecked", "ClientMatchUnchecked")
                        .WithOne("ClientMatchUncheckedVideoInfo")
                        .HasForeignKey("Data.Core.Models.ClientMatchUncheckedVideoInfo", "ClientMatchUncheckedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientMatchUnchecked");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchUncheckedVideoNoteInfo", b =>
                {
                    b.HasOne("Data.Core.Models.ClientMatchUnchecked", "ClientMatchUnchecked")
                        .WithOne("ClientMatchUncheckedVideoNoteInfo")
                        .HasForeignKey("Data.Core.Models.ClientMatchUncheckedVideoNoteInfo", "ClientMatchUncheckedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientMatchUnchecked");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMediaInfo", b =>
                {
                    b.HasOne("Data.Core.Models.Client", "Client")
                        .WithOne("ClientMediaInfo")
                        .HasForeignKey("Data.Core.Models.ClientMediaInfo", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Data.Core.Models.ClientPhotoInfo", b =>
                {
                    b.HasOne("Data.Core.Models.ClientMediaInfo", "ClientMediaInfo")
                        .WithMany("ClientPhotoInfos")
                        .HasForeignKey("ClientMediaInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientMediaInfo");
                });

            modelBuilder.Entity("Data.Core.Models.ClientVideoInfo", b =>
                {
                    b.HasOne("Data.Core.Models.ClientMediaInfo", "ClientMediaInfo")
                        .WithMany("ClientVideoInfos")
                        .HasForeignKey("ClientMediaInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientMediaInfo");
                });

            modelBuilder.Entity("Data.Core.Models.ClientVideoNoteInfo", b =>
                {
                    b.HasOne("Data.Core.Models.ClientMediaInfo", "ClientMediaInfo")
                        .WithMany("ClientVideoNoteInfos")
                        .HasForeignKey("ClientMediaInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientMediaInfo");
                });

            modelBuilder.Entity("Data.Core.Models.TelegramBotCommand", b =>
                {
                    b.HasOne("Data.Core.Models.TelegramBotParams", null)
                        .WithMany("BotCommands")
                        .HasForeignKey("TelegramBotParamsTelegramBotId");
                });

            modelBuilder.Entity("Data.Core.Models.TelegramBotParamMessage", b =>
                {
                    b.HasOne("Data.Core.Models.TelegramBotParams", null)
                        .WithMany("Messages")
                        .HasForeignKey("TelegramBotParamsTelegramBotId");
                });

            modelBuilder.Entity("Data.Core.Models.TelegramBotRegisterCondition", b =>
                {
                    b.HasOne("Data.Core.Models.TelegramBotParams", null)
                        .WithMany("BotRegisterConditions")
                        .HasForeignKey("TelegramBotParamsTelegramBotId");
                });

            modelBuilder.Entity("Data.Core.Models.Client", b =>
                {
                    b.Navigation("ClientMatchInfo")
                        .IsRequired();

                    b.Navigation("ClientMediaInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchChecked", b =>
                {
                    b.Navigation("ClientMatchCheckedVideoInfo");

                    b.Navigation("ClientMatchCheckedVideoNoteInfo");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchInfo", b =>
                {
                    b.Navigation("CheckedClientMatchs");

                    b.Navigation("UncheckedClientMatchs");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMatchUnchecked", b =>
                {
                    b.Navigation("ClientMatchUncheckedVideoInfo");

                    b.Navigation("ClientMatchUncheckedVideoNoteInfo");
                });

            modelBuilder.Entity("Data.Core.Models.ClientMediaInfo", b =>
                {
                    b.Navigation("ClientPhotoInfos");

                    b.Navigation("ClientVideoInfos");

                    b.Navigation("ClientVideoNoteInfos");
                });

            modelBuilder.Entity("Data.Core.Models.TelegramBotParams", b =>
                {
                    b.Navigation("BotCommands");

                    b.Navigation("BotRegisterConditions");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
