using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideogamesWebApp.Migrations
{
    /// <inheritdoc />
    public partial class newtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "GameTransactions",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.CreateTable(
                name: "DLCs",
                columns: table => new
                {
                    DlcId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DlcName = table.Column<string>(type: "TEXT", nullable: false),
                    DlcDescription = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    GameId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DLCs", x => x.DlcId);
                    table.ForeignKey(
                        name: "FK_DLCs_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DLCs_GameId",
                table: "DLCs",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DLCs");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "GameTransactions",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }
    }
}
