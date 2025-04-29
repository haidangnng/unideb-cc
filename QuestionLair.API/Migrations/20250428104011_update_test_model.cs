using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestionLair.API.Migrations
{
    /// <inheritdoc />
    public partial class update_test_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowMultipleAttempts",
                table: "Tests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Tests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ShuffleQuestions",
                table: "Tests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TimeLimitMinutes",
                table: "Tests",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowMultipleAttempts",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "ShuffleQuestions",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TimeLimitMinutes",
                table: "Tests");
        }
    }
}
