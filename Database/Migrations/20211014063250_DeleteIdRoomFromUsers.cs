using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class DeleteIdRoomFromUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdRoom",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdRoom",
                table: "Users",
                type: "integer",
                nullable: true);
        }
    }
}
