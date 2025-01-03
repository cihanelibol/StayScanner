using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayScanner.Api.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class ChangedColumnNameReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestedDetail",
                table: "Reports",
                newName: "RequestedBody");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestedBody",
                table: "Reports",
                newName: "RequestedDetail");
        }
    }
}
