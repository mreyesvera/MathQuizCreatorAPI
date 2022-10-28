using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathQuizCreatorAPI.Migrations
{
    public partial class AddQuestionCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorUserId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatorUserId",
                table: "Questions",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_CreatorUserId",
                table: "Questions",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_CreatorUserId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CreatorUserId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Questions");
        }
    }
}
