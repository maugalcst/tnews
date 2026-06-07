using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsIntelligence.API.Migrations
{
    /// <inheritdoc />
    public partial class AddXPathContainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "XPathContainer",
                table: "Sources",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XPathContainer",
                table: "Sources");
        }
    }
}
