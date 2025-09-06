using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apptrade.Data.Migrations
{
    /// <inheritdoc />
    public partial class WatchListMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_watchlist",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    AssestId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_watchlist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_watchlist_t_assest_AssestId",
                        column: x => x.AssestId,
                        principalTable: "t_assest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_watchlist_AssestId",
                table: "t_watchlist",
                column: "AssestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_watchlist");
        }
    }
}
