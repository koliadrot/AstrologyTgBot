namespace Data.Core
{
    using Data.Core.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;

    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var hostContext = new HostBuilder().Build().Services.GetRequiredService<IHostEnvironment>();
                var configFile = "DataAppsettings.json";
                var builder = new ConfigurationBuilder()
                    .SetBasePath(hostContext.ContentRootPath)
                    .AddJsonFile(configFile, optional: false, reloadOnChange: true);

                var configuration = builder.Build();

                optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MariaDbServerVersion(new Version(10, 3, 39)));
            }
        }


        public DbSet<User> Users { get; set; }
        public DbSet<TelegramBotParams> TelegramParams { get; set; }
        public DbSet<TelegramBotCommand> TelegramBotCommands { get; set; }
        public DbSet<TelegramBotRegisterCondition> TelegramBotRegisterConditions { get; set; }
        public DbSet<TelegramBotParamMessage> TelegramBotParamMessages { get; set; }
        public DbSet<ApiAccessToken> ApiAccessTokens { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientMediaInfo> ClientMediaInfos { get; set; }
        public DbSet<ClientPhotoInfo> ClientPhotoInfos { get; set; }
        public DbSet<ClientVideoInfo> ClientVideoInfos { get; set; }
    }
}