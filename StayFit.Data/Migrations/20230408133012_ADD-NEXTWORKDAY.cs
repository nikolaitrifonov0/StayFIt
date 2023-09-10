using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StayFit.Data.Migrations
{
    public partial class ADDNEXTWORKDAY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextWorkout",
                table: "WorkDays");           

            migrationBuilder.AddColumn<string>(
                name: "NextWorkDayId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NextWorkDayId",
                table: "AspNetUsers",
                column: "NextWorkDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WorkDays_NextWorkDayId",
                table: "AspNetUsers",
                column: "NextWorkDayId",
                principalTable: "WorkDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WorkDays_NextWorkDayId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NextWorkDayId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NextWorkDayId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "NextWorkout",
                table: "WorkDays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));            
        }
    }
}
