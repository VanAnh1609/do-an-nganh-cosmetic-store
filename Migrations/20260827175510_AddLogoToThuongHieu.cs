using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticStore.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoToThuongHieu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "ThuongHieus",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "ThuongHieus");
        }
    }
}
