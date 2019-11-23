using Microsoft.EntityFrameworkCore.Migrations;

namespace QuestionnaireApp.Data.Migrations
{
    public partial class AddQuestionAndAnswerNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Answers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Answers");
        }
    }
}
