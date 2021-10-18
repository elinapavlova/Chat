using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class RenameFieldsDatetoDateCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Users",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Rooms",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Messages",
                newName: "DateCreated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Users",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Rooms",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Messages",
                newName: "Date");
        }
    }
}
