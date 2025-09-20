using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apptrade.Data.Migrations
{
    /// <inheritdoc />
    public partial class PortfolioMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_portfolio",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Risk = table.Column<string>(type: "TEXT", nullable: false),
                    Owner = table.Column<string>(type: "TEXT", nullable: false),
                    customerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_portfolio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_portfolio_t_customer_customerId",
                        column: x => x.customerId,
                        principalTable: "t_customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "t_portfolio_movement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    portfolio_id = table.Column<long>(type: "INTEGER", nullable: false),
                    movement_type = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    movement_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AssestId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_portfolio_movement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_portfolio_movement_t_assest_AssestId",
                        column: x => x.AssestId,
                        principalTable: "t_assest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_portfolio_movement_t_portfolio_portfolio_id",
                        column: x => x.portfolio_id,
                        principalTable: "t_portfolio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_portfolio_customerId",
                table: "t_portfolio",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_t_portfolio_movement_AssestId",
                table: "t_portfolio_movement",
                column: "AssestId");

            migrationBuilder.CreateIndex(
                name: "IX_t_portfolio_movement_portfolio_id",
                table: "t_portfolio_movement",
                column: "portfolio_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_portfolio_movement");

            migrationBuilder.DropTable(
                name: "t_portfolio");
        }
    }
}
