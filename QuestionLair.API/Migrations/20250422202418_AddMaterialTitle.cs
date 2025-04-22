using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestionLair.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Materials",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Materials");
        }
    }
}
