using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchedulingAPI.Migrations
{
    /// <inheritdoc />
    public partial class updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookables_Shops_ShopId",
                table: "Bookables");

            migrationBuilder.DropIndex(
                name: "IX_Bookables_ShopId",
                table: "Bookables");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Bookables");

            migrationBuilder.AddColumn<long>(
                name: "OfferingShopId",
                table: "Bookables",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Bookables_OfferingShopId",
                table: "Bookables",
                column: "OfferingShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookables_Shops_OfferingShopId",
                table: "Bookables",
                column: "OfferingShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookables_Shops_OfferingShopId",
                table: "Bookables");

            migrationBuilder.DropIndex(
                name: "IX_Bookables_OfferingShopId",
                table: "Bookables");

            migrationBuilder.DropColumn(
                name: "OfferingShopId",
                table: "Bookables");

            migrationBuilder.AddColumn<long>(
                name: "ShopId",
                table: "Bookables",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookables_ShopId",
                table: "Bookables",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookables_Shops_ShopId",
                table: "Bookables",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");
        }
    }
}
