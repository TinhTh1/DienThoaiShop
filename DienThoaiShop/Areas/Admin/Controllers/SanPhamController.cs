using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SlugGenerator;
using DTShop.Models;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
namespace DienThoaiShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SanPhamController : Controller
    {
        private readonly DTShopDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SanPhamController(DTShopDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: SanPham
        public async Task<IActionResult> Index()
        {
            var DTShopDbContext = _context.SanPham.Include(s => s.HangSanXuat).Include(s => s.LoaiSanPham);
            return View(await DTShopDbContext.ToListAsync());

            // return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index_LoadData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? int.Parse(length) : 0;
                int skip = start != null ? int.Parse(start) : 0;
                int totalRecords = 0;
                int filterRecords = 0;



                var sanPham = from r in _context.SanPham
                              select new
                              {
                                  r.ID,
                                  r.HinhAnh,
                                  r.LoaiSanPham.TenLoai,
                                  r.HangSanXuat.TenHangSanXuat,
                                  r.TenSanPham,
                                  r.SoLuong,
                                  r.DonGia
                              };

                totalRecords = sanPham.Count();

                // Sắp xếp 
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    sanPham = sanPham.OrderBy(sortColumn + " " + sortColumnDirection);
                }

                // Tìm kiếm 
                if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrWhiteSpace(searchValue))
                {
                    sanPham = sanPham.Where(r => r.TenLoai.Contains(searchValue) ||
                        r.TenHangSanXuat.Contains(searchValue) ||
                        r.TenSanPham.Contains(searchValue) ||
                        r.SoLuong.ToString().Contains(searchValue) ||
                        r.DonGia.ToString().Contains(searchValue));
                }

                filterRecords = sanPham.Count();
                var data = sanPham.Skip(skip).Take(pageSize).ToList();
                var jsonData = new
                {
                    draw,
                    recordsFiltered = filterRecords,
                    recordsTotal = totalRecords,
                    data
                };

                return Json(jsonData, new System.Text.Json.JsonSerializerOptions());
            }
            catch (Exception ex)
            {
                throw;
            }
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
        public async Task<IActionResult> Create([Bind("ID,HangSanXuatID,LoaiSanPhamID,TenSanPham,TenSanPhamKhongDau,DonGia,SoLuong,DuLieuHinhAnh,MoTa")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(sanPham.TenSanPhamKhongDau))
                {
                    sanPham.TenSanPhamKhongDau = sanPham.TenSanPham.GenerateSlug();
                }

                string path = "";

                // Nếu hình ảnh không bỏ trống thì upload
                if (sanPham.DuLieuHinhAnh != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string folder = "/uploads/";
                    string fileExtension = Path.GetExtension(sanPham.DuLieuHinhAnh.FileName).ToLower();//ToLower viết thường , ToUper viết hoa
                    string fileName = sanPham.TenSanPham;
                    string fileNameSluged = fileName.GenerateSlug();
                    path = fileNameSluged + fileExtension;
                    string physicalPath = Path.Combine(wwwRootPath + folder, fileNameSluged + fileExtension);
                    using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await sanPham.DuLieuHinhAnh.CopyToAsync(fileStream);
                    }
                }

                // Cập nhật đường dẫn vào CSDL
                sanPham.HinhAnh = path ?? null;
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,HangSanXuatID,LoaiSanPhamID,TenSanPham,TenSanPhamKhongDau,DonGia,SoLuong,DuLieuHinhAnh,MoTa")] SanPham sanPham)
        {
            if (id != sanPham.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string path = "";

                    // Nếu hình ảnh không bỏ trống thì upload ảnh mới
                    if (sanPham.DuLieuHinhAnh != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string folder = "/uploads/";
                        string fileExtension = Path.GetExtension(sanPham.DuLieuHinhAnh.FileName).ToLower();
                        string fileName = sanPham.TenSanPham;
                        string fileNameSluged = fileName.GenerateSlug();
                        path = fileNameSluged + fileExtension;
                        string physicalPath = Path.Combine(wwwRootPath + folder, fileNameSluged + fileExtension);
                        using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                        {
                            await sanPham.DuLieuHinhAnh.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(sanPham);

                    if (string.IsNullOrEmpty(path))
                        _context.Entry(sanPham).Property(x => x.HinhAnh).IsModified = false;
                    else
                        sanPham.HinhAnh = path;


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
                // Xóa hình ảnh (nếu có)
                if (!string.IsNullOrEmpty(sanPham.HinhAnh))
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", sanPham.HinhAnh);
                    if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                }

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
