using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticStore.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnReasonToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LyDoHoanHang",
                table: "DonHangs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyDoHoanHang",
                table: "DonHangs");
        }
    }
}
