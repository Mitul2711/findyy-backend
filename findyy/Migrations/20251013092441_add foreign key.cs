using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace findyy.Migrations
{
    /// <inheritdoc />
    public partial class addforeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Business_OwnerUserId",
                table: "Business",
                column: "OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Business_AppUsers_OwnerUserId",
                table: "Business",
                column: "OwnerUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Business_AppUsers_OwnerUserId",
                table: "Business");

            migrationBuilder.DropIndex(
                name: "IX_Business_OwnerUserId",
                table: "Business");
        }
    }
}
