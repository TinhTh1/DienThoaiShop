using DienThoaiShop.Logic;
using DTShop.Logic;
using DTShop.Models;
using DTShop.Logic;
using DTShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ITShop.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class KhachHangController : Controller
    {
        private readonly DTShopDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailLogic _mailLogic;

        public KhachHangController(DTShopDbContext context, IHttpContextAccessor httpContextAccessor, IMailLogic mailLogic)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mailLogic = mailLogic;
        }

        // GET: Index 
        public IActionResult Index()
        {

            return View();
        }

        // GET: DatHang 
        public IActionResult DatHang()
        {
            GioHangLogic gioHangLogic = new GioHangLogic(_context);
            var gioHang = gioHangLogic.LayGioHang();


            decimal tongTien = gioHangLogic.LayTongTienSanPham();
            TempData["TongTien"] = tongTien;
            return View(gioHang);
        }

        // POST: DatHang 
        [HttpPost]
        public async Task<IActionResult> DatHang(DatHang datHang)
        {
            GioHangLogic gioHangLogic = new GioHangLogic(_context);
            var gioHang = gioHangLogic.LayGioHang();

            if (string.IsNullOrWhiteSpace(datHang.DienThoaiGiaoHang) || string.IsNullOrWhiteSpace(datHang.DiaChiGiaoHang))
            {
                decimal tongTien = gioHangLogic.LayTongTienSanPham();
                TempData["TongTien"] = tongTien;
                TempData["ThongBaoLoi"] = "Thông tin giao hàng không được bỏ trống.";
                return View(gioHang);
            }
            else
            {
                DatHang dh = new DatHang();
                dh.NguoiDungID = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID")?.Value);
                dh.TinhTrangID = 1; // Đơn hàng mới 
                dh.DienThoaiGiaoHang = datHang.DienThoaiGiaoHang;
                dh.DiaChiGiaoHang = datHang.DiaChiGiaoHang;
                dh.NgayDatHang = DateTime.Now;
                _context.DatHang.Add(dh);
                await _context.SaveChangesAsync();

                foreach (var item in gioHang)
                {
                    DatHang_ChiTiet ct = new DatHang_ChiTiet();
                    ct.DatHangID = dh.ID;
                    ct.SanPhamID = item.SanPhamID;
                    ct.SoLuong = Convert.ToInt16(item.SoLuongTrongGio);
                    ct.DonGia = item.SanPham.DonGia;
                    _context.DatHang_ChiTiet.Add(ct);
                    await _context.SaveChangesAsync();
                }

                // Gởi email 
                try
                {
                    MailInfo mailInfo = new MailInfo();
                    mailInfo.Subject = "Đặt hàng thành công tại ITShop.Com.Vn";


                    var datHangInfo = _context.DatHang.Where(r => r.ID == dh.ID)
                        .Include(s => s.NguoiDung)
                        .Include(s => s.DatHang_ChiTiet).SingleOrDefault();
                    await _mailLogic.GoiEmailDatHangThanhCong(datHangInfo, mailInfo);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }

                return RedirectToAction("DatHangThanhCong", "KhachHang", new { Area = "" });
            }
        }

        // GET: DatHangThanhCong 
        public IActionResult DatHangThanhCong()
        {
            // Xóa giỏ hàng sau khi hoàn tất đặt hàng 
            GioHangLogic gioHangLogic = new GioHangLogic(_context);
            gioHangLogic.XoaTatCa();

            return View();
        }
    }
}