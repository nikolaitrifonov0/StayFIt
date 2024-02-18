using Microsoft.EntityFrameworkCore.Migrations;

namespace StayFit.Data.Migrations
{
    public partial class ChatLogRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatLog",
                table: "ChatLog");

            migrationBuilder.RenameTable(
                name: "ChatLog",
                newName: "ChatLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatLogs",
                table: "ChatLogs",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatLogs",
                table: "ChatLogs");

            migrationBuilder.RenameTable(
                name: "ChatLogs",
                newName: "ChatLog");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatLog",
                table: "ChatLog",
                column: "Id");
        }
    }
}
