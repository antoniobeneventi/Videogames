using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideogamesWebApp.Migrations
{
    /// <inheritdoc />
    public partial class tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Launchers",
                columns: table => new
                {
                    LauncherId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LauncherName = table.Column<string>(type: "TEXT", nullable: false),
                    LauncherDescription = table.Column<string>(type: "TEXT", nullable: false),
                    Link = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Launchers", x => x.LauncherId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Launchers");
        }
    }
}
