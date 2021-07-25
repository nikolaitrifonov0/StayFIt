using Microsoft.EntityFrameworkCore.Migrations;

namespace StayFit.Data.Migrations
{
    public partial class RemoveSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.AddColumn<int>(
                name: "Repetitions",
                table: "UserExerciseLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SetNumber",
                table: "UserExerciseLogs",
                type: "int",
                maxLength: 10,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "UserExerciseLogs",
                type: "int",
                maxLength: 3000,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Repetitions",
                table: "UserExerciseLogs");

            migrationBuilder.DropColumn(
                name: "SetNumber",
                table: "UserExerciseLogs");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "UserExerciseLogs");

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Repetitions = table.Column<int>(type: "int", nullable: false),
                    UserExerciseLogId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_Sets_UserExerciseLogId",
                table: "Sets",
                column: "UserExerciseLogId",
                unique: true);
        }
    }
}
