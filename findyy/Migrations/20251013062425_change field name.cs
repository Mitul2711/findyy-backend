using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace findyy.Migrations
{
    /// <inheritdoc />
    public partial class changefieldname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessCategory",
                table: "AppUsers");

            migrationBuilder.AddColumn<long>(
                name: "BusinessCategoryId",
                table: "AppUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_BusinessCategoryId",
                table: "AppUsers",
                column: "BusinessCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_BusinessCategory_BusinessCategoryId",
                table: "AppUsers",
                column: "BusinessCategoryId",
                principalTable: "BusinessCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_BusinessCategory_BusinessCategoryId",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_BusinessCategoryId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "BusinessCategoryId",
                table: "AppUsers");

            migrationBuilder.AddColumn<string>(
                name: "BusinessCategory",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
