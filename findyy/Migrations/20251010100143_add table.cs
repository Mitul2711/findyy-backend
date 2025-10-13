using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace findyy.Migrations
{
    /// <inheritdoc />
    public partial class addtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Business");

            migrationBuilder.AddColumn<long>(
                name: "BusinessCategoryId",
                table: "Business",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BusinessCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Business_BusinessCategoryId",
                table: "Business",
                column: "BusinessCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Business_BusinessCategory_BusinessCategoryId",
                table: "Business",
                column: "BusinessCategoryId",
                principalTable: "BusinessCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Business_BusinessCategory_BusinessCategoryId",
                table: "Business");

            migrationBuilder.DropTable(
                name: "BusinessCategory");

            migrationBuilder.DropIndex(
                name: "IX_Business_BusinessCategoryId",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "BusinessCategoryId",
                table: "Business");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Business",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
