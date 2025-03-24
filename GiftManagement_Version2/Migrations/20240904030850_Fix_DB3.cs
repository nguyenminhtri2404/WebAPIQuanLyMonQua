using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftManagement_Version2.Migrations
{
    public partial class Fix_DB3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalPrice",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Yb6RrP6P/CCzpfHu96EkTunkG3Bg8z8i0/SWBlf6iOLfttcdxXAfW");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$lupPAD3H2SEimq1dmKfO6.X3F9cb/yO9cnNAldfgaqm7a2BYHSKze");
        }
    }
}
