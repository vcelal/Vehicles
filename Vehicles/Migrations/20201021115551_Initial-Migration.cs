using Microsoft.EntityFrameworkCore.Migrations;

namespace Vehicles.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeightCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    MinWeight = table.Column<double>(nullable: false),
                    MaxWeight = table.Column<double>(nullable: false),
                    IconId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerName = table.Column<string>(maxLength: 30, nullable: false),
                    ManufacturerId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    ManufactureYear = table.Column<int>(nullable: false),
                    VehicleWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleDetails_WeightCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "WeightCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleDetails_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Mazda" },
                    { 2, "Mercedes" },
                    { 3, "Honda" },
                    { 4, "Ferrari" },
                    { 5, "Toyota" }
                });

            migrationBuilder.InsertData(
                table: "WeightCategories",
                columns: new[] { "Id", "IconId", "MaxWeight", "MinWeight", "Name" },
                values: new object[,]
                {
                    { 1, 1, 500.0, 0.0, "Light" },
                    { 2, 2, 2500.0, 501.0, "Medium" },
                    { 3, 3, 100000.0, 2501.0, "Heavy" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDetails_CategoryId",
                table: "VehicleDetails",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDetails_ManufacturerId",
                table: "VehicleDetails",
                column: "ManufacturerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleDetails");

            migrationBuilder.DropTable(
                name: "WeightCategories");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
