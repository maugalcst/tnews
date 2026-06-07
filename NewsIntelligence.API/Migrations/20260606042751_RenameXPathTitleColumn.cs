using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsIntelligence.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameXPathTitleColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "XPathTItle",
                table: "Sources",
                newName: "XPathTitle");

            migrationBuilder.AlterColumn<long>(
                name: "DurationMs",
                table: "ScrapingLogs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "XPathTitle",
                table: "Sources",
                newName: "XPathTItle");

            migrationBuilder.AlterColumn<int>(
                name: "DurationMs",
                table: "ScrapingLogs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
