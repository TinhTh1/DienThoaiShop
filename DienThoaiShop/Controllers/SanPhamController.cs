using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DTShop.Models;

namespace DienThoaiShop.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly DTShopDbContext _context;

        public SanPhamController(DTShopDbContext context)
        {
            _context = context;
        }

        // GET: SanPham
        public async Task<IActionResult> Index()
        {
            var dTShopDbContext = _context.SanPham.Include(s => s.HangSanXuat).Include(s => s.LoaiSanPham);
            return View(await dTShopDbContext.ToListAsync());
        }

        // GET: SanPham/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .Include(s => s.HangSanXuat)
                .Include(s => s.LoaiSanPham)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPham/Create
        public IActionResult Create()
        {
            ViewData["HangSanXuatID"] = new SelectList(_context.HangSanXuat, "ID", "TenHangSanXuat");
            ViewData["LoaiSanPhamID"] = new SelectList(_context.LoaiSanPham, "ID", "TenLoai");
            return View();
        }

        // POST: SanPham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,HangSanXuatID,LoaiSanPhamID,TenSanPham,TenSanPhamKhongDau,DonGia,SoLuong,HinhAnh,MoTa")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HangSanXuatID"] = new SelectList(_context.HangSanXuat, "ID", "TenHangSanXuat", sanPham.HangSanXuatID);
            ViewData["LoaiSanPhamID"] = new SelectList(_context.LoaiSanPham, "ID", "TenLoai", sanPham.LoaiSanPhamID);
            return View(sanPham);
        }

        // GET: SanPham/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["HangSanXuatID"] = new SelectList(_context.HangSanXuat, "ID", "TenHangSanXuat", sanPham.HangSanXuatID);
            ViewData["LoaiSanPhamID"] = new SelectList(_context.LoaiSanPham, "ID", "TenLoai", sanPham.LoaiSanPhamID);
            return View(sanPham);
        }

        // POST: SanPham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,HangSanXuatID,LoaiSanPhamID,TenSanPham,TenSanPhamKhongDau,DonGia,SoLuong,HinhAnh,MoTa")] SanPham sanPham)
        {
            if (id != sanPham.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.ID))
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
            ViewData["HangSanXuatID"] = new SelectList(_context.HangSanXuat, "ID", "TenHangSanXuat", sanPham.HangSanXuatID);
            ViewData["LoaiSanPhamID"] = new SelectList(_context.LoaiSanPham, "ID", "TenLoai", sanPham.LoaiSanPhamID);
            return View(sanPham);
        }

        // GET: SanPham/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .Include(s => s.HangSanXuat)
                .Include(s => s.LoaiSanPham)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPham.Remove(sanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPham.Any(e => e.ID == id);
        }
    }
}
