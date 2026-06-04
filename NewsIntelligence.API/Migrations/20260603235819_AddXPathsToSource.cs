using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsIntelligence.API.Migrations
{
    /// <inheritdoc />
    public partial class AddXPathsToSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "XPathContent",
                table: "Sources",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "XPathTItle",
                table: "Sources",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XPathContent",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "XPathTItle",
                table: "Sources");
        }
    }
}
