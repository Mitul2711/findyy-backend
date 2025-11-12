using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace findyy.Migrations
{
    /// <inheritdoc />
    public partial class addfieldchattable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Unread",
                table: "ChatMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unread",
                table: "ChatMessages");
        }
    }
}
