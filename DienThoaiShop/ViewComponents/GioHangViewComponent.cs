using DienThoaiShop.Logic;
using DTShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace DTShop.ViewComponents
{
    public class GioHangViewComponent : ViewComponent
    {
        private readonly DTShopDbContext _context;

        public GioHangViewComponent(DTShopDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            GioHangLogic gioHangLogic = new GioHangLogic(_context);
            decimal tongTien = gioHangLogic.LayTongTienSanPham();
            decimal tongSoLuong = gioHangLogic.LayTongSoLuong();
            TempData["TopMenu_TongTien"] = tongTien;
            TempData["TopMenu_TongSoLuong"] = tongSoLuong;
            return View("Default");
        }
    }
}

