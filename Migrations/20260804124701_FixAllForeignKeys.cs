using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticStore.Migrations
{
    /// <inheritdoc />
    public partial class FixAllForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHangs_DonHangs_DonHangMaDonHang",
                table: "ChiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHangs_SanPhams_SanPhamMaSanPham",
                table: "ChiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_KhachHangs_KhachHangMaKhachHang",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_SanPhams_SanPhamMaSanPham",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_KhachHangs_KhachHangMaKhachHang",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_HinhAnhSanPhams_SanPhams_SanPhamMaSanPham",
                table: "HinhAnhSanPhams");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_DanhMucs_DanhMucMaDanhMuc",
                table: "SanPhams");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_ThuongHieus_ThuongHieuMaThuongHieu",
                table: "SanPhams");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuThichs_KhachHangs_KhachHangMaKhachHang",
                table: "YeuThichs");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuThichs_SanPhams_SanPhamMaSanPham",
                table: "YeuThichs");

            migrationBuilder.DropIndex(
                name: "IX_YeuThichs_KhachHangMaKhachHang",
                table: "YeuThichs");

            migrationBuilder.DropIndex(
                name: "IX_YeuThichs_SanPhamMaSanPham",
                table: "YeuThichs");

            migrationBuilder.DropIndex(
                name: "IX_SanPhams_DanhMucMaDanhMuc",
                table: "SanPhams");

            migrationBuilder.DropIndex(
                name: "IX_SanPhams_ThuongHieuMaThuongHieu",
                table: "SanPhams");

            migrationBuilder.DropIndex(
                name: "IX_HinhAnhSanPhams_SanPhamMaSanPham",
                table: "HinhAnhSanPhams");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_KhachHangMaKhachHang",
                table: "DonHangs");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_KhachHangMaKhachHang",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_SanPhamMaSanPham",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietDonHangs_DonHangMaDonHang",
                table: "ChiTietDonHangs");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietDonHangs_SanPhamMaSanPham",
                table: "ChiTietDonHangs");

            migrationBuilder.DropColumn(
                name: "KhachHangMaKhachHang",
                table: "YeuThichs");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSanPham",
                table: "YeuThichs");

            migrationBuilder.DropColumn(
                name: "DanhMucMaDanhMuc",
                table: "SanPhams");

            migrationBuilder.DropColumn(
                name: "ThuongHieuMaThuongHieu",
                table: "SanPhams");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSanPham",
                table: "HinhAnhSanPhams");

            migrationBuilder.DropColumn(
                name: "KhachHangMaKhachHang",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "KhachHangMaKhachHang",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSanPham",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "DonHangMaDonHang",
                table: "ChiTietDonHangs");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSanPham",
                table: "ChiTietDonHangs");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_MaKhachHang",
                table: "YeuThichs",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_MaSanPham",
                table: "YeuThichs",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaDanhMuc",
                table: "SanPhams",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaThuongHieu",
                table: "SanPhams",
                column: "MaThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhSanPhams_MaSanPham",
                table: "HinhAnhSanPhams",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaKhachHang",
                table: "DonHangs",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MaKhachHang",
                table: "DanhGias",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MaSanPham",
                table: "DanhGias",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaDonHang",
                table: "ChiTietDonHangs",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaSanPham",
                table: "ChiTietDonHangs",
                column: "MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHangs_DonHangs_MaDonHang",
                table: "ChiTietDonHangs",
                column: "MaDonHang",
                principalTable: "DonHangs",
                principalColumn: "MaDonHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHangs_SanPhams_MaSanPham",
                table: "ChiTietDonHangs",
                column: "MaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_KhachHangs_MaKhachHang",
                table: "DanhGias",
                column: "MaKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_SanPhams_MaSanPham",
                table: "DanhGias",
                column: "MaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_KhachHangs_MaKhachHang",
                table: "DonHangs",
                column: "MaKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HinhAnhSanPhams_SanPhams_MaSanPham",
                table: "HinhAnhSanPhams",
                column: "MaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_DanhMucs_MaDanhMuc",
                table: "SanPhams",
                column: "MaDanhMuc",
                principalTable: "DanhMucs",
                principalColumn: "MaDanhMuc",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_ThuongHieus_MaThuongHieu",
                table: "SanPhams",
                column: "MaThuongHieu",
                principalTable: "ThuongHieus",
                principalColumn: "MaThuongHieu",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YeuThichs_KhachHangs_MaKhachHang",
                table: "YeuThichs",
                column: "MaKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YeuThichs_SanPhams_MaSanPham",
                table: "YeuThichs",
                column: "MaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHangs_DonHangs_MaDonHang",
                table: "ChiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHangs_SanPhams_MaSanPham",
                table: "ChiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_KhachHangs_MaKhachHang",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_SanPhams_MaSanPham",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_KhachHangs_MaKhachHang",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_HinhAnhSanPhams_SanPhams_MaSanPham",
                table: "HinhAnhSanPhams");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_DanhMucs_MaDanhMuc",
                table: "SanPhams");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_ThuongHieus_MaThuongHieu",
                table: "SanPhams");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuThichs_KhachHangs_MaKhachHang",
                table: "YeuThichs");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuThichs_SanPhams_MaSanPham",
                table: "YeuThichs");

            migrationBuilder.DropIndex(
                name: "IX_YeuThichs_MaKhachHang",
                table: "YeuThichs");

            migrationBuilder.DropIndex(
                name: "IX_YeuThichs_MaSanPham",
                table: "YeuThichs");

            migrationBuilder.DropIndex(
                name: "IX_SanPhams_MaDanhMuc",
                table: "SanPhams");

            migrationBuilder.DropIndex(
                name: "IX_SanPhams_MaThuongHieu",
                table: "SanPhams");

            migrationBuilder.DropIndex(
                name: "IX_HinhAnhSanPhams_MaSanPham",
                table: "HinhAnhSanPhams");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_MaKhachHang",
                table: "DonHangs");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_MaKhachHang",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_MaSanPham",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietDonHangs_MaDonHang",
                table: "ChiTietDonHangs");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietDonHangs_MaSanPham",
                table: "ChiTietDonHangs");

            migrationBuilder.AddColumn<int>(
                name: "KhachHangMaKhachHang",
                table: "YeuThichs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSanPham",
                table: "YeuThichs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DanhMucMaDanhMuc",
                table: "SanPhams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThuongHieuMaThuongHieu",
                table: "SanPhams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSanPham",
                table: "HinhAnhSanPhams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KhachHangMaKhachHang",
                table: "DonHangs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KhachHangMaKhachHang",
                table: "DanhGias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSanPham",
                table: "DanhGias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonHangMaDonHang",
                table: "ChiTietDonHangs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSanPham",
                table: "ChiTietDonHangs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_KhachHangMaKhachHang",
                table: "YeuThichs",
                column: "KhachHangMaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_SanPhamMaSanPham",
                table: "YeuThichs",
                column: "SanPhamMaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_DanhMucMaDanhMuc",
                table: "SanPhams",
                column: "DanhMucMaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_ThuongHieuMaThuongHieu",
                table: "SanPhams",
                column: "ThuongHieuMaThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhSanPhams_SanPhamMaSanPham",
                table: "HinhAnhSanPhams",
                column: "SanPhamMaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_KhachHangMaKhachHang",
                table: "DonHangs",
                column: "KhachHangMaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_KhachHangMaKhachHang",
                table: "DanhGias",
                column: "KhachHangMaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_SanPhamMaSanPham",
                table: "DanhGias",
                column: "SanPhamMaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_DonHangMaDonHang",
                table: "ChiTietDonHangs",
                column: "DonHangMaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_SanPhamMaSanPham",
                table: "ChiTietDonHangs",
                column: "SanPhamMaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHangs_DonHangs_DonHangMaDonHang",
                table: "ChiTietDonHangs",
                column: "DonHangMaDonHang",
                principalTable: "DonHangs",
                principalColumn: "MaDonHang");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHangs_SanPhams_SanPhamMaSanPham",
                table: "ChiTietDonHangs",
                column: "SanPhamMaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_KhachHangs_KhachHangMaKhachHang",
                table: "DanhGias",
                column: "KhachHangMaKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_SanPhams_SanPhamMaSanPham",
                table: "DanhGias",
                column: "SanPhamMaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_KhachHangs_KhachHangMaKhachHang",
                table: "DonHangs",
                column: "KhachHangMaKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_HinhAnhSanPhams_SanPhams_SanPhamMaSanPham",
                table: "HinhAnhSanPhams",
                column: "SanPhamMaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_DanhMucs_DanhMucMaDanhMuc",
                table: "SanPhams",
                column: "DanhMucMaDanhMuc",
                principalTable: "DanhMucs",
                principalColumn: "MaDanhMuc");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_ThuongHieus_ThuongHieuMaThuongHieu",
                table: "SanPhams",
                column: "ThuongHieuMaThuongHieu",
                principalTable: "ThuongHieus",
                principalColumn: "MaThuongHieu");

            migrationBuilder.AddForeignKey(
                name: "FK_YeuThichs_KhachHangs_KhachHangMaKhachHang",
                table: "YeuThichs",
                column: "KhachHangMaKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_YeuThichs_SanPhams_SanPhamMaSanPham",
                table: "YeuThichs",
                column: "SanPhamMaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham");
        }
    }
}
