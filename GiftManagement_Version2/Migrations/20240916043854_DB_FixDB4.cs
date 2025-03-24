using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftManagement_Version2.Migrations
{
    public partial class DB_FixDB4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccumulatedPoint",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rankings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    TotalPoint = table.Column<int>(type: "int", nullable: false),
                    LastOrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    IsFinalized = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rankings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rankings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Elram9qc1jELQWCQKXiHPOBeYhSYqbGHwv1./IDnMpU/yopygu/5C");

            migrationBuilder.CreateIndex(
                name: "IX_Rankings_UserId",
                table: "Rankings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rankings");

            migrationBuilder.DropColumn(
                name: "AccumulatedPoint",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Yb6RrP6P/CCzpfHu96EkTunkG3Bg8z8i0/SWBlf6iOLfttcdxXAfW");
        }
    }
}
