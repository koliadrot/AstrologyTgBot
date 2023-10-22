namespace Data.Core
{
	using Data.Core.Models;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

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

	}
}
