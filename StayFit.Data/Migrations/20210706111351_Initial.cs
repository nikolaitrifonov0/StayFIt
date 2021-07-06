using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StayFit.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BodyParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyParts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Equipment = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BodyPartExercise",
                columns: table => new
                {
                    BodyPartsId = table.Column<int>(type: "int", nullable: false),
                    ExercisesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyPartExercise", x => new { x.BodyPartsId, x.ExercisesId });
                    table.ForeignKey(
                        name: "FK_BodyPartExercise_BodyParts_BodyPartsId",
                        column: x => x.BodyPartsId,
                        principalTable: "BodyParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BodyPartExercise_Exercises_ExercisesId",
                        column: x => x.ExercisesId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseWorkDay",
                columns: table => new
                {
                    ExercisesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkDaysId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseWorkDay", x => new { x.ExercisesId, x.WorkDaysId });
                    table.ForeignKey(
                        name: "FK_ExerciseWorkDay_Exercises_ExercisesId",
                        column: x => x.ExercisesId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExerciseLogs",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExerciseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkDayId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExerciseLogs", x => new { x.UserId, x.WorkDayId, x.ExerciseId });
                    table.ForeignKey(
                        name: "FK_UserExerciseLogs_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkoutCycleType = table.Column<int>(type: "int", nullable: false),
                    CycleDays = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CurrentWorkoutId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Workouts_CurrentWorkoutId",
                        column: x => x.CurrentWorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkDays",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NextWorkout = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkoutId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkDays_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BodyPartExercise_ExercisesId",
                table: "BodyPartExercise",
                column: "ExercisesId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseWorkDay_WorkDaysId",
                table: "ExerciseWorkDay",
                column: "WorkDaysId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseLogs_ExerciseId",
                table: "UserExerciseLogs",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseLogs_WorkDayId",
                table: "UserExerciseLogs",
                column: "WorkDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentWorkoutId",
                table: "Users",
                column: "CurrentWorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDays_WorkoutId",
                table: "WorkDays",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_CreatorId",
                table: "Workouts",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseWorkDay_WorkDays_WorkDaysId",
                table: "ExerciseWorkDay",
                column: "WorkDaysId",
                principalTable: "WorkDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExerciseLogs_Users_UserId",
                table: "UserExerciseLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExerciseLogs_WorkDays_WorkDayId",
                table: "UserExerciseLogs",
                column: "WorkDayId",
                principalTable: "WorkDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_Users_CreatorId",
                table: "Workouts",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_Users_CreatorId",
                table: "Workouts");

            migrationBuilder.DropTable(
                name: "BodyPartExercise");

            migrationBuilder.DropTable(
                name: "ExerciseWorkDay");

            migrationBuilder.DropTable(
                name: "UserExerciseLogs");

            migrationBuilder.DropTable(
                name: "BodyParts");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "WorkDays");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Workouts");
        }
    }
}
