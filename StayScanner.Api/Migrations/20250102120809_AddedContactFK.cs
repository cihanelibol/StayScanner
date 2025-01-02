using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayScanner.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedContactFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Hotels_HotelId",
                table: "Contacts");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelId",
                table: "Contacts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Contacts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Hotels_HotelId",
                table: "Contacts",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Hotels_HotelId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "Contacts");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelId",
                table: "Contacts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Hotels_HotelId",
                table: "Contacts",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");
        }
    }
}
