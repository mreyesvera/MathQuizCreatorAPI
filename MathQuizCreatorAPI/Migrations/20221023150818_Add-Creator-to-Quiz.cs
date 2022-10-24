using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathQuizCreatorAPI.Migrations
{
    public partial class AddCreatortoQuiz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorUserId",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CreatorUserId",
                table: "Quizzes",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Users_CreatorUserId",
                table: "Quizzes",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Users_CreatorUserId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_CreatorUserId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Quizzes");
        }
    }
}
