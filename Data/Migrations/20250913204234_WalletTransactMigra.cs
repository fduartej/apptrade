using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apptrade.Data.Migrations
{
    /// <inheritdoc />
    public partial class WalletTransactMigra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_wallet_transaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WalletId = table.Column<long>(type: "INTEGER", nullable: true),
                    TransactionType = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_wallet_transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_wallet_transaction_t_wallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "t_wallet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_wallet_transaction_WalletId",
                table: "t_wallet_transaction",
                column: "WalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_wallet_transaction");
        }
    }
}
