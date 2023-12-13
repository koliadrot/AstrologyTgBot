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
                .HasAnnotation("ProductVersion", "7.0.2")
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

                    b.Property<bool>("AcceptElectronicReceipts")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("AcceptPromotionsBySms")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("BotName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("BotUserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("HelloText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Menu")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TokenApi")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TosUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("WebHookUrl")
                        .IsRequired()
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
