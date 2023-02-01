using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Binned.Migrations
{
    /// <inheritdoc />
    public partial class addedAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Block",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StreetName",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UnitNumber",
                table: "Orders",
                newName: "Address");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Orders",
                newName: "UnitNumber");

            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
