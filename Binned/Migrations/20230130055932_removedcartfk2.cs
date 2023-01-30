using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Binned.Migrations
{
    /// <inheritdoc />
    public partial class removedcartfk2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Carts_CartForeignKey",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CartForeignKey",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CartForeignKey",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartForeignKey",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CartForeignKey",
                table: "Orders",
                column: "CartForeignKey",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Carts_CartForeignKey",
                table: "Orders",
                column: "CartForeignKey",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
