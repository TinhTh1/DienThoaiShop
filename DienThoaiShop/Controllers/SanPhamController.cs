using DTShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DienThoaiShop.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly DTShopDbContext _context;
        public SanPhamController(DTShopDbContext context)
        {
            _context = context;
        }

        // GET: Index 
     //   [Route("san-pham/{trang?}")]
        public IActionResult Index(int? trang)
        {
            var danhSach = LayDanhSachSanPham(trang ?? 1);
            if (danhSach.SanPham.Count == 0)
                return NotFound();
            else
                return View(danhSach);
        }
        private PhanTrangSanPham LayDanhSachSanPham(int trangHienTai)
        {
            int maxRows = 20;

            PhanTrangSanPham phanTrang = new PhanTrangSanPham();
            phanTrang.SanPham = _context.SanPham
                .Include(s => s.HangSanXuat)
                .Include(s => s.LoaiSanPham)
                .OrderBy(r => r.LoaiSanPhamID)
                .Skip((trangHienTai - 1) * maxRows)
                .Take(maxRows).ToList();

            decimal tongSoTrang = Convert.ToDecimal(_context.SanPham.Count()) / Convert.ToDecimal(maxRows);
            phanTrang.TongSoTrang = (int)Math.Ceiling(tongSoTrang);
            phanTrang.TrangHienTai = trangHienTai;

            return phanTrang;
        }

        // GET: PhanLoai 
       //[Route("san-pham/{tenLoai}/{trang?}")]
        public IActionResult PhanLoai(string tenLoai, int? trang)
        {
            var danhSachPhanLoai = LayDanhSachSanPhamTheoPhanLoai(tenLoai, trang ?? 1);
            if (danhSachPhanLoai.SanPham.Count == 0)
                return NotFound();
            else
                return View(danhSachPhanLoai);
        }

        private PhanTrangSanPham LayDanhSachSanPhamTheoPhanLoai(string tenLoai, int trangHienTai)
        {
            int maxRows = 20;
            var sanPhamPhanLoai = _context.SanPham
                .Include(s => s.HangSanXuat)
                .Include(s => s.LoaiSanPham)
                .Where(r => r.LoaiSanPham.TenLoaiKhongDau == tenLoai);

            PhanTrangSanPham phanTrang = new PhanTrangSanPham();
            phanTrang.SanPham = sanPhamPhanLoai.OrderBy(r => r.LoaiSanPhamID)
                .Skip((trangHienTai - 1) * maxRows)
                .Take(maxRows).ToList();

            decimal tongSoTrang = Convert.ToDecimal(sanPhamPhanLoai.Count()) / Convert.ToDecimal(maxRows);
            phanTrang.TongSoTrang = (int)Math.Ceiling(tongSoTrang);
            phanTrang.TrangHienTai = trangHienTai;

            return phanTrang;
        }
        // GET: ChiTiet 
        // [Route("san-pham/{tenLoai}/{tenSanPham}.html")]
        public IActionResult ChiTiet(string tenLoai, string tenSanPham)
        {
            var sanPham = _context.SanPham
                .Include(s => s.HangSanXuat)
                .Include(s => s.LoaiSanPham)
                .Where(r => r.TenSanPhamKhongDau == tenSanPham).SingleOrDefault();
            if (sanPham == null)
                return NotFound();
            else
                return View(sanPham);
        }

    }
}