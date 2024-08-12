using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class UpdateForeignKeyConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductModels_Products_ProductId",
                table: "ProductModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Customers_CustomerId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Employees_EmployeeId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ProductModels_ModelId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Products_ProductId",
                table: "Services");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModels_Products_ProductId",
                table: "ProductModels",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Customers_CustomerId",
                table: "Services",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Employees_EmployeeId",
                table: "Services",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ProductModels_ModelId",
                table: "Services",
                column: "ModelId",
                principalTable: "ProductModels",
                principalColumn: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Products_ProductId",
                table: "Services",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductModels_Products_ProductId",
                table: "ProductModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Customers_CustomerId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Employees_EmployeeId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ProductModels_ModelId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Products_ProductId",
                table: "Services");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModels_Products_ProductId",
                table: "ProductModels",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Customers_CustomerId",
                table: "Services",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Employees_EmployeeId",
                table: "Services",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ProductModels_ModelId",
                table: "Services",
                column: "ModelId",
                principalTable: "ProductModels",
                principalColumn: "ModelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Products_ProductId",
                table: "Services",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
