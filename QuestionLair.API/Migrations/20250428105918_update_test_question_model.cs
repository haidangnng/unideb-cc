using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestionLair.API.Migrations
{
    /// <inheritdoc />
    public partial class update_test_question_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMultipleAnswer",
                table: "TestQuestions");

            migrationBuilder.RenameColumn(
                name: "AnswerText",
                table: "TestQuestions",
                newName: "CorrectAnswer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorrectAnswer",
                table: "TestQuestions",
                newName: "AnswerText");

            migrationBuilder.AddColumn<bool>(
                name: "IsMultipleAnswer",
                table: "TestQuestions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
