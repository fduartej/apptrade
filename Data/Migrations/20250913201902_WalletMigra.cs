using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apptrade.Data.Migrations
{
    /// <inheritdoc />
    public partial class WalletMigra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_wallet",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    customerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_wallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_wallet_t_customer_customerId",
                        column: x => x.customerId,
                        principalTable: "t_customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_wallet_customerId",
                table: "t_wallet",
                column: "customerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_wallet");
        }
    }
}
