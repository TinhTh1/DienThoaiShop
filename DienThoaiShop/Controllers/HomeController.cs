﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using DTShop.Logic;
using DTShop.Models;
namespace DienThoaiShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DTShopDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailLogic _mailLogic;

        public HomeController(ILogger<HomeController> logger, DTShopDbContext context, IHttpContextAccessor httpContextAccessor, IMailLogic mailLogic)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mailLogic = mailLogic;
        }
        public async Task<IActionResult> Index()
        {
            var loaiSanPham = await _context.LoaiSanPham.Include(s => s.SanPham).ToListAsync();
            return View(loaiSanPham);
        }
        // GET: Register 
        [AllowAnonymous]
        public IActionResult Register(string? successMessage)
        {
            if (!string.IsNullOrEmpty(successMessage))
                TempData["ThongBao"] = successMessage;
            return View();
        }

        // POST: Register 
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([Bind("ID,HoVaTen,Email,DienThoai,DiaChi,TenDangNhap,MatKhau,XacNhanMatKhau")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                var kiemTra = _context.NguoiDung.Where(r => r.TenDangNhap == nguoiDung.TenDangNhap).SingleOrDefault();
                if (kiemTra == null)
                {
                    nguoiDung.MatKhau = BC.HashPassword(nguoiDung.MatKhau);
                    nguoiDung.XacNhanMatKhau = BC.HashPassword(nguoiDung.MatKhau);
                    nguoiDung.Quyen = false; // Khách hàng 
                    _context.Add(nguoiDung);
                    await _context.SaveChangesAsync();

                    // Gởi email 
                    try
                    {
                        MailInfo mailInfo = new MailInfo();
                        mailInfo.Subject = "Đăng ký thành công tại ShopDienThoai.Com.Vn";
                        var nguoiDungInfo = nguoiDung;
                        await _mailLogic.GoiEmailDangKyThanhCong(nguoiDungInfo, mailInfo);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message.ToString());
                    }
                    return RedirectToAction("Register", "Home", new { Area = "", successMessage = "Đăng ký tài khoản thành công." });
                }
                else
                {
                    TempData["ThongBaoLoi"] = "Tên đăng nhập này đã được sử dụng cho một tài khoản khác.";
                    return View(nguoiDung);
                }
            }
            return View(nguoiDung);
        }



        // GET: Login 
        [AllowAnonymous]
        public IActionResult Login(string? ReturnUrl)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                // Nếu đã đăng nhập thì chuyển đến trang chủ 
                return LocalRedirect(ReturnUrl ?? "/");
            }
            else
            {
                // Nếu chưa đăng nhập thì chuyển đến trang đăng nhập 
                ViewBag.LienKetChuyenTrang = ReturnUrl ?? "/";
                return View();
            }
        }

        // POST: Login 
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind] DangNhap dangNhap)
        {
            if (ModelState.IsValid)
            {
                var nguoiDung = _context.NguoiDung.Where(r => r.TenDangNhap == dangNhap.TenDangNhap).SingleOrDefault();

                if (nguoiDung == null || !BC.Verify(dangNhap.MatKhau, nguoiDung.MatKhau))
                {
                    TempData["ThongBaoLoi"] = "Tài khoản không tồn tại trong hệ thống.";
                    return View(dangNhap);
                }
                else
                {
                    var claims = new List<Claim>
                    {
                        new Claim("ID", nguoiDung.ID.ToString()),
                        new Claim(ClaimTypes.Name, nguoiDung.TenDangNhap),
                        new Claim("HoVaTen", nguoiDung.HoVaTen),


                        new Claim(ClaimTypes.Role, nguoiDung.Quyen ? "Admin" : "User")
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = dangNhap.DuyTriDangNhap
                    };

                    //  Đăng nhập hệ thống 
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return LocalRedirect(dangNhap.LienKetChuyenTrang ?? (nguoiDung.Quyen ? "/Admin" : "/"));
                }
            }

            return View(dangNhap);
        }

        // GET: DangXuat 
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        // GET: Forbidden 
        public IActionResult Forbidden()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
