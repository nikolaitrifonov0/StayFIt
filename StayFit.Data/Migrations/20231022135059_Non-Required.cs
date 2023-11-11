using Microsoft.EntityFrameworkCore.Migrations;

namespace StayFit.Data.Migrations
{
    public partial class NonRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExerciseLogs_WorkDays_WorkDayId",
                table: "UserExerciseLogs");

            migrationBuilder.AlterColumn<string>(
                name: "WorkDayId",
                table: "UserExerciseLogs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExerciseLogs_WorkDays_WorkDayId",
                table: "UserExerciseLogs",
                column: "WorkDayId",
                principalTable: "WorkDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExerciseLogs_WorkDays_WorkDayId",
                table: "UserExerciseLogs");

            migrationBuilder.AlterColumn<string>(
                name: "WorkDayId",
                table: "UserExerciseLogs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExerciseLogs_WorkDays_WorkDayId",
                table: "UserExerciseLogs",
                column: "WorkDayId",
                principalTable: "WorkDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
