using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace findyy.Migrations
{
    /// <inheritdoc />
    public partial class addfieldsinreviewtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BusinessReview",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "BusinessReview",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "BusinessReview",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "BusinessReview",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BusinessReview");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BusinessReview");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BusinessReview");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "BusinessReview");
        }
    }
}
