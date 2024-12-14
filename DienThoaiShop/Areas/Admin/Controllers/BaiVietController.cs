using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DTShop.Models;
using Microsoft.AspNetCore.Authorization;
using SlugGenerator;

namespace DienThoaiShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BaiVietController : Controller
    {
        private readonly DTShopDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaiVietController(DTShopDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: BaiViet
        public async Task<IActionResult> Index()
        {
            var dTShopDbContext = _context.BaiViet.Include(b => b.ChuDe).Include(b => b.NguoiDung).OrderByDescending(r => r.NgayDang);
            return View(await dTShopDbContext.ToListAsync());
        }

        // GET: BaiViet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViet
                .Include(b => b.ChuDe)
                .Include(b => b.NguoiDung)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (baiViet == null)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        // GET: BaiViet/Create
        public IActionResult Create()
        {
            ViewData["ChuDeID"] = new SelectList(_context.ChuDe, "ID", "TenChuDe");          
            return View();
        }

        // POST: BaiViet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ChuDeID,TieuDe,TieuDeKhongDau,TomTat,NoiDung,HienThi")] BaiViet baiViet)
        {
            if (ModelState.IsValid)
            {
                int maNguoiDung = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID")?.Value);

                // Gán các giá trị mặc định 
                baiViet.NguoiDungID = maNguoiDung;
                baiViet.NgayDang = DateTime.Now;
                baiViet.LuotXem = 0;
                baiViet.KiemDuyet = true;

                // Nếu tiêu đề không dấu bỏ trống thì tự động xử lý 
                if (string.IsNullOrWhiteSpace(baiViet.TieuDeKhongDau))
                {


                    baiViet.TieuDeKhongDau = baiViet.TieuDe.GenerateSlug();
                }

                _context.Add(baiViet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChuDeID"] = new SelectList(_context.ChuDe, "ID", "TenChuDe", baiViet.ChuDeID);          
            return View(baiViet);
        }

        // GET: BaiViet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViet.FindAsync(id);
            if (baiViet == null)
            {
                return NotFound();
            }
            ViewData["ChuDeID"] = new SelectList(_context.ChuDe, "ID", "TenChuDe", baiViet.ChuDeID);            
            return View(baiViet);
        }

        // POST: BaiViet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ChuDeID,TieuDe,TieuDeKhongDau,TomTat,NoiDung,KiemDuyet,HienThi")] BaiViet baiViet)
        {
            if (id != baiViet.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                { // Nếu tiêu đề không dấu bỏ trống thì tự động xử lý 
                    if (string.IsNullOrWhiteSpace(baiViet.TieuDeKhongDau))
                    {
                        baiViet.TieuDeKhongDau = baiViet.TieuDe.GenerateSlug();


                    }
                    _context.Update(baiViet);
                    // Bỏ qua không cập nhật 
                    _context.Entry(baiViet).Property(x => x.NguoiDungID).IsModified = false;
                    _context.Entry(baiViet).Property(x => x.NgayDang).IsModified = false;
                    _context.Entry(baiViet).Property(x => x.LuotXem).IsModified = false;
                    _context.Update(baiViet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaiVietExists(baiViet.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChuDeID"] = new SelectList(_context.ChuDe, "ID", "TenChuDe", baiViet.ChuDeID);           
            return View(baiViet);
        }

        // GET: BaiViet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViet
                .Include(b => b.ChuDe)
                .Include(b => b.NguoiDung)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (baiViet == null)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        // POST: BaiViet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baiViet = await _context.BaiViet.FindAsync(id);
            if (baiViet != null)
            {
                _context.BaiViet.Remove(baiViet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaiVietExists(int id)
        {
            return _context.BaiViet.Any(e => e.ID == id);
        }
    }
}
