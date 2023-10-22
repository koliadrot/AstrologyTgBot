using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
	/// <inheritdoc />
	public partial class UserAccountDistribution : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					UserId = table.Column<int>(type: "int", nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					Login = table.Column<string>(type: "longtext", nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					PasswordHash = table.Column<string>(type: "longtext", nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					Name = table.Column<string>(type: "longtext", nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					Email = table.Column<string>(type: "longtext", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4"),
					PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4"),
					IsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.UserId);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.InsertData(
				table: "Users",
				columns: new[] { "Login", "PasswordHash", "Name", "Email", "PhoneNumber", "IsAdmin" },
				values: new object[] { "koliadrot", "kilkala289289", "Blazz", "koliadrott@gmail.com", null, true });
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
