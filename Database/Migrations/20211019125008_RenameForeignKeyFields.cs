using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class RenameForeignKeyFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Rooms_IdRoom",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_IdUser",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Users_IdUser",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Rooms",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_IdUser",
                table: "Rooms",
                newName: "IX_Rooms_UserId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdRoom",
                table: "Messages",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_IdUser",
                table: "Messages",
                newName: "IX_Messages_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_IdRoom",
                table: "Messages",
                newName: "IX_Messages_RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Users_UserId",
                table: "Rooms",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Rooms_RoomId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Users_UserId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Rooms",
                newName: "IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_UserId",
                table: "Rooms",
                newName: "IX_Rooms_IdUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Messages",
                newName: "IdRoom");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                newName: "IX_Messages_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_RoomId",
                table: "Messages",
                newName: "IX_Messages_IdRoom");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_IdRoom",
                table: "Messages",
                column: "IdRoom",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_IdUser",
                table: "Messages",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Users_IdUser",
                table: "Rooms",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
