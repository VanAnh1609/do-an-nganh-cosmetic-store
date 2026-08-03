using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticStore.Migrations
{
    /// <inheritdoc />
    public partial class TaoCoSoDuLieuLanDau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMucs",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucs", x => x.MaDanhMuc);
                });

            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    MaKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatKhauHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VaiTro = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHangs", x => x.MaKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "MaGiamGias",
                columns: table => new
                {
                    MaGiamGiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PhanTramGiam = table.Column<int>(type: "int", nullable: false),
                    GiamToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DonHangToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaGiamGias", x => x.MaGiamGiaId);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieus",
                columns: table => new
                {
                    MaThuongHieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuongHieus", x => x.MaThuongHieu);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    MaGiamGiaId = table.Column<int>(type: "int", nullable: true),
                    TenNguoiNhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DiaChiGiaoHang = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TienGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhiVanChuyen = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    KhachHangMaKhachHang = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DonHangs_KhachHangs_KhachHangMaKhachHang",
                        column: x => x.KhachHangMaKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKhachHang");
                    table.ForeignKey(
                        name: "FK_DonHangs_MaGiamGias_MaGiamGiaId",
                        column: x => x.MaGiamGiaId,
                        principalTable: "MaGiamGias",
                        principalColumn: "MaGiamGiaId");
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    MaSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSanPham = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false),
                    MaThuongHieu = table.Column<int>(type: "int", nullable: false),
                    DanhMucMaDanhMuc = table.Column<int>(type: "int", nullable: true),
                    ThuongHieuMaThuongHieu = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK_SanPhams_DanhMucs_DanhMucMaDanhMuc",
                        column: x => x.DanhMucMaDanhMuc,
                        principalTable: "DanhMucs",
                        principalColumn: "MaDanhMuc");
                    table.ForeignKey(
                        name: "FK_SanPhams_ThuongHieus_ThuongHieuMaThuongHieu",
                        column: x => x.ThuongHieuMaThuongHieu,
                        principalTable: "ThuongHieus",
                        principalColumn: "MaThuongHieu");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    MaChiTietDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    DonHangMaDonHang = table.Column<int>(type: "int", nullable: true),
                    SanPhamMaSanPham = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => x.MaChiTietDonHang);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_DonHangMaDonHang",
                        column: x => x.DonHangMaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang");
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_SanPhams_SanPhamMaSanPham",
                        column: x => x.SanPhamMaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "DanhGias",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    SoSao = table.Column<int>(type: "int", nullable: false),
                    BinhLuan = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaDuyet = table.Column<bool>(type: "bit", nullable: false),
                    KhachHangMaKhachHang = table.Column<int>(type: "int", nullable: true),
                    SanPhamMaSanPham = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGias", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK_DanhGias_KhachHangs_KhachHangMaKhachHang",
                        column: x => x.KhachHangMaKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKhachHang");
                    table.ForeignKey(
                        name: "FK_DanhGias_SanPhams_SanPhamMaSanPham",
                        column: x => x.SanPhamMaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "HinhAnhSanPhams",
                columns: table => new
                {
                    MaHinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuongDan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LaAnhDaiDien = table.Column<bool>(type: "bit", nullable: false),
                    ThuTu = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    SanPhamMaSanPham = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhSanPhams", x => x.MaHinhAnh);
                    table.ForeignKey(
                        name: "FK_HinhAnhSanPhams_SanPhams_SanPhamMaSanPham",
                        column: x => x.SanPhamMaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "YeuThichs",
                columns: table => new
                {
                    MaYeuThich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    NgayThem = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KhachHangMaKhachHang = table.Column<int>(type: "int", nullable: true),
                    SanPhamMaSanPham = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuThichs", x => x.MaYeuThich);
                    table.ForeignKey(
                        name: "FK_YeuThichs_KhachHangs_KhachHangMaKhachHang",
                        column: x => x.KhachHangMaKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKhachHang");
                    table.ForeignKey(
                        name: "FK_YeuThichs_SanPhams_SanPhamMaSanPham",
                        column: x => x.SanPhamMaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_DonHangMaDonHang",
                table: "ChiTietDonHangs",
                column: "DonHangMaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_SanPhamMaSanPham",
                table: "ChiTietDonHangs",
                column: "SanPhamMaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_KhachHangMaKhachHang",
                table: "DanhGias",
                column: "KhachHangMaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_SanPhamMaSanPham",
                table: "DanhGias",
                column: "SanPhamMaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_KhachHangMaKhachHang",
                table: "DonHangs",
                column: "KhachHangMaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaGiamGiaId",
                table: "DonHangs",
                column: "MaGiamGiaId");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhSanPhams_SanPhamMaSanPham",
                table: "HinhAnhSanPhams",
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
                name: "IX_YeuThichs_KhachHangMaKhachHang",
                table: "YeuThichs",
                column: "KhachHangMaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_SanPhamMaSanPham",
                table: "YeuThichs",
                column: "SanPhamMaSanPham");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "DanhGias");

            migrationBuilder.DropTable(
                name: "HinhAnhSanPhams");

            migrationBuilder.DropTable(
                name: "YeuThichs");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "KhachHangs");

            migrationBuilder.DropTable(
                name: "MaGiamGias");

            migrationBuilder.DropTable(
                name: "DanhMucs");

            migrationBuilder.DropTable(
                name: "ThuongHieus");
        }
    }
}
