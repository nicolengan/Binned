using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Binned.Migrations
{
    /// <inheritdoc />
    public partial class EditedPromoCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpireDate",
                table: "PromoCodes",
                newName: "ExpiryDate");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "PromoCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "PromoCodes");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "PromoCodes",
                newName: "ExpireDate");
        }
    }
}
