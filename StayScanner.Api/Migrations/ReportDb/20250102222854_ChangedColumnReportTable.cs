using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayScanner.Api.Migrations.ReportDb
{
    /// <inheritdoc />
    public partial class ChangedColumnReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportObjectType",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "ReportType",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportType",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "ReportObjectType",
                table: "Reports",
                type: "text",
                nullable: true);
        }
    }
}
