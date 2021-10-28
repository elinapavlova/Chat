using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class MakeUserIdInChatsTableNotNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Chats",
                oldNullable:true,
                nullable:false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Chats",
                oldNullable:false,
                nullable:true);
        }
    }
}
