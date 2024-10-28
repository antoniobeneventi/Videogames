using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideogamesWebApp.Migrations
{
    /// <inheritdoc />
    public partial class newtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DlcId",
                table: "GameTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LauncherId",
                table: "GameTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DlcId",
                table: "GameTransactions");

            migrationBuilder.DropColumn(
                name: "LauncherId",
                table: "GameTransactions");
        }
    }
}
