using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DienThoaiShop.Migrations
{
    /// <inheritdoc />
    public partial class TrangTin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaiViet_ChuDe_ChuDeID",
                table: "BaiViet");

            migrationBuilder.DropForeignKey(
                name: "FK_BaiViet_NguoiDung_NguoiDungID",
                table: "BaiViet");

            migrationBuilder.DropForeignKey(
                name: "FK_BinhLuanBaiViet_BaiViet_BaiVietID",
                table: "BinhLuanBaiViet");

            migrationBuilder.DropForeignKey(
                name: "FK_BinhLuanBaiViet_NguoiDung_NguoiDungID",
                table: "BinhLuanBaiViet");

            migrationBuilder.DropForeignKey(
                name: "FK_DatHang_NguoiDung_NguoiDungID",
                table: "DatHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DatHang_TinhTrang_TinhTrangID",
                table: "DatHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DatHang_ChiTiet_DatHang_DatHangID",
                table: "DatHang_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_DatHang_ChiTiet_SanPham_SanPhamID",
                table: "DatHang_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_SanPham_SanPhamID",
                table: "GioHang");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_HangSanXuat_HangSanXuatID",
                table: "SanPham");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_LoaiSanPham_LoaiSanPhamID",
                table: "SanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_BaiViet_ChuDe_ChuDeID",
                table: "BaiViet",
                column: "ChuDeID",
                principalTable: "ChuDe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaiViet_NguoiDung_NguoiDungID",
                table: "BaiViet",
                column: "NguoiDungID",
                principalTable: "NguoiDung",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BinhLuanBaiViet_BaiViet_BaiVietID",
                table: "BinhLuanBaiViet",
                column: "BaiVietID",
                principalTable: "BaiViet",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BinhLuanBaiViet_NguoiDung_NguoiDungID",
                table: "BinhLuanBaiViet",
                column: "NguoiDungID",
                principalTable: "NguoiDung",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatHang_NguoiDung_NguoiDungID",
                table: "DatHang",
                column: "NguoiDungID",
                principalTable: "NguoiDung",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatHang_TinhTrang_TinhTrangID",
                table: "DatHang",
                column: "TinhTrangID",
                principalTable: "TinhTrang",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatHang_ChiTiet_DatHang_DatHangID",
                table: "DatHang_ChiTiet",
                column: "DatHangID",
                principalTable: "DatHang",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatHang_ChiTiet_SanPham_SanPhamID",
                table: "DatHang_ChiTiet",
                column: "SanPhamID",
                principalTable: "SanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_SanPham_SanPhamID",
                table: "GioHang",
                column: "SanPhamID",
                principalTable: "SanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_HangSanXuat_HangSanXuatID",
                table: "SanPham",
                column: "HangSanXuatID",
                principalTable: "HangSanXuat",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_LoaiSanPham_LoaiSanPhamID",
                table: "SanPham",
                column: "LoaiSanPhamID",
                principalTable: "LoaiSanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaiViet_ChuDe_ChuDeID",
                table: "BaiViet");

            migrationBuilder.DropForeignKey(
                name: "FK_BaiViet_NguoiDung_NguoiDungID",
                table: "BaiViet");

            migrationBuilder.DropForeignKey(
                name: "FK_BinhLuanBaiViet_BaiViet_BaiVietID",
                table: "BinhLuanBaiViet");

            migrationBuilder.DropForeignKey(
                name: "FK_BinhLuanBaiViet_NguoiDung_NguoiDungID",
                table: "BinhLuanBaiViet");

            migrationBuilder.DropForeignKey(
                name: "FK_DatHang_NguoiDung_NguoiDungID",
                table: "DatHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DatHang_TinhTrang_TinhTrangID",
                table: "DatHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DatHang_ChiTiet_DatHang_DatHangID",
                table: "DatHang_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_DatHang_ChiTiet_SanPham_SanPhamID",
                table: "DatHang_ChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_SanPham_SanPhamID",
                table: "GioHang");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_HangSanXuat_HangSanXuatID",
                table: "SanPham");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_LoaiSanPham_LoaiSanPhamID",
                table: "SanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_BaiViet_ChuDe_ChuDeID",
                table: "BaiViet",
                column: "ChuDeID",
                principalTable: "ChuDe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaiViet_NguoiDung_NguoiDungID",
                table: "BaiViet",
                column: "NguoiDungID",
                principalTable: "NguoiDung",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BinhLuanBaiViet_BaiViet_BaiVietID",
                table: "BinhLuanBaiViet",
                column: "BaiVietID",
                principalTable: "BaiViet",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BinhLuanBaiViet_NguoiDung_NguoiDungID",
                table: "BinhLuanBaiViet",
                column: "NguoiDungID",
                principalTable: "NguoiDung",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatHang_NguoiDung_NguoiDungID",
                table: "DatHang",
                column: "NguoiDungID",
                principalTable: "NguoiDung",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatHang_TinhTrang_TinhTrangID",
                table: "DatHang",
                column: "TinhTrangID",
                principalTable: "TinhTrang",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatHang_ChiTiet_DatHang_DatHangID",
                table: "DatHang_ChiTiet",
                column: "DatHangID",
                principalTable: "DatHang",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatHang_ChiTiet_SanPham_SanPhamID",
                table: "DatHang_ChiTiet",
                column: "SanPhamID",
                principalTable: "SanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_SanPham_SanPhamID",
                table: "GioHang",
                column: "SanPhamID",
                principalTable: "SanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_HangSanXuat_HangSanXuatID",
                table: "SanPham",
                column: "HangSanXuatID",
                principalTable: "HangSanXuat",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_LoaiSanPham_LoaiSanPhamID",
                table: "SanPham",
                column: "LoaiSanPhamID",
                principalTable: "LoaiSanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
