using Microsoft.EntityFrameworkCore;

namespace DTShop.Models
{
    public class DTShopDbContext: DbContext
    {
        public DTShopDbContext(DbContextOptions<DTShopDbContext> options) : base(options) { }

        public DbSet<LoaiSanPham> LoaiSanPham { get; set; }
		public DbSet<HangSanXuat> HangSanXuat { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<TinhTrang> TinhTrang { get; set; }
        public DbSet<DatHang> DatHang { get; set; }    
        public DbSet<DatHang_ChiTiet> DatHang_ChiTiet { get; set; }
        public DbSet<GioHang> GioHang { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoaiSanPham>().ToTable("LoaiSanPham");
			modelBuilder.Entity<HangSanXuat>().ToTable("HangSanXuat");
            modelBuilder.Entity<SanPham>().ToTable("SanPham");
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
            modelBuilder.Entity<TinhTrang>().ToTable("TinhTrang");
            modelBuilder.Entity<DatHang>().ToTable("DatHang");
            modelBuilder.Entity<DatHang_ChiTiet>().ToTable("DatHang_ChiTiet");
            modelBuilder.Entity<GioHang>().ToTable("GioHang");

        }
    }
}
