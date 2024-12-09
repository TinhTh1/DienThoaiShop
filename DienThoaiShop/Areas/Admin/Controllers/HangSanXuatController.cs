using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DTShop.Models;
using SlugGenerator;
using Microsoft.AspNetCore.Authorization;

namespace DienThoaiShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HangSanXuatController : Controller
    {
        private readonly DTShopDbContext _context;

        public HangSanXuatController(DTShopDbContext context)
        {
            _context = context;
        }

        // GET: HangSanXuat
        public async Task<IActionResult> Index()
        {
            return View(await _context.HangSanXuat.ToListAsync());
        }

        // GET: HangSanXuat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangSanXuat = await _context.HangSanXuat
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hangSanXuat == null)
            {
                return NotFound();
            }

            return View(hangSanXuat);
        }

        // GET: HangSanXuat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HangSanXuat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TenHangSanXuat,TenHangSanXuatKhongDau")] HangSanXuat hangSanXuat)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(hangSanXuat.TenHangSanXuatKhongDau))
                {
                    hangSanXuat.TenHangSanXuatKhongDau = hangSanXuat.TenHangSanXuat.GenerateSlug();
                }

                _context.Add(hangSanXuat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hangSanXuat);
        }

        // GET: HangSanXuat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangSanXuat = await _context.HangSanXuat.FindAsync(id);
            if (hangSanXuat == null)
            {
                return NotFound();
            }
            return View(hangSanXuat);
        }

        // POST: HangSanXuat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TenHangSanXuat,TenHangSanXuatKhongDau")] HangSanXuat hangSanXuat)
        {
            if (id != hangSanXuat.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(hangSanXuat.TenHangSanXuatKhongDau))
                    {
                        hangSanXuat.TenHangSanXuatKhongDau = hangSanXuat.TenHangSanXuat.GenerateSlug();
                    }

                    _context.Update(hangSanXuat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HangSanXuatExists(hangSanXuat.ID))
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
            return View(hangSanXuat);
        }

        // GET: HangSanXuat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangSanXuat = await _context.HangSanXuat
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hangSanXuat == null)
            {
                return NotFound();
            }

            return View(hangSanXuat);
        }

        // POST: HangSanXuat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hangSanXuat = await _context.HangSanXuat.FindAsync(id);
            if (hangSanXuat != null)
            {
                _context.HangSanXuat.Remove(hangSanXuat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HangSanXuatExists(int id)
        {
            return _context.HangSanXuat.Any(e => e.ID == id);
        }
    }
}
