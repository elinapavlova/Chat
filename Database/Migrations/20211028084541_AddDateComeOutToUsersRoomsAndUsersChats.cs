using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class AddDateComeOutToUsersRoomsAndUsersChats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateComeOut",
                table: "UsersRooms",
                type: "timestamp without time zone",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateComeOut",
                table: "UsersChats",
                type: "timestamp without time zone",
                nullable: true,
                defaultValue: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateComeOut",
                table: "UsersRooms");

            migrationBuilder.DropColumn(
                name: "DateComeOut",
                table: "UsersChats");
        }
    }
}
