using Microsoft.EntityFrameworkCore.Migrations;

namespace StayFit.Data.Migrations
{
    public partial class ColumnFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExerciseLogs_AspNetUsers_UserId1",
                table: "UserExerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_UserExerciseLogs_UserId1",
                table: "UserExerciseLogs");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserExerciseLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UserExerciseLogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseLogs_UserId1",
                table: "UserExerciseLogs",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExerciseLogs_AspNetUsers_UserId1",
                table: "UserExerciseLogs",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
