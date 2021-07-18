using Microsoft.EntityFrameworkCore.Migrations;

namespace StayFit.Data.Migrations
{
    public partial class Sets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserExerciseLogs",
                table: "UserExerciseLogs");

            migrationBuilder.DropColumn(
                name: "Repetitions",
                table: "UserExerciseLogs");

            migrationBuilder.DropColumn(
                name: "Sets",
                table: "UserExerciseLogs");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UserExerciseLogs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserExerciseLogs",
                table: "UserExerciseLogs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Repetitions = table.Column<int>(type: "int", nullable: false),
                    UserExerciseLogId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_UserExerciseLogs_UserExerciseLogId",
                        column: x => x.UserExerciseLogId,
                        principalTable: "UserExerciseLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseLogs_UserId",
                table: "UserExerciseLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_UserExerciseLogId",
                table: "Sets",
                column: "UserExerciseLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserExerciseLogs",
                table: "UserExerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_UserExerciseLogs_UserId",
                table: "UserExerciseLogs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserExerciseLogs");

            migrationBuilder.AddColumn<int>(
                name: "Repetitions",
                table: "UserExerciseLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sets",
                table: "UserExerciseLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserExerciseLogs",
                table: "UserExerciseLogs",
                columns: new[] { "UserId", "WorkDayId", "ExerciseId" });
        }
    }
}
