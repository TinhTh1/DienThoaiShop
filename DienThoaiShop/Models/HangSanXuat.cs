using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTShop.Models
{
    public class HangSanXuat
    {
        [DisplayName("Mã Hãng Sản Xuất")]
        public int ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Tên hãng sản xuất không được bỏ trống.")]
        [DisplayName("Tên hãng sản xuất")]
        public  required string TenHangSanXuat { get; set; }

        [StringLength(255)]
        [DisplayName("Tên hãng sản xuất không dấu")]
        public string? TenHangSanXuatKhongDau { get; set; }

        public ICollection<SanPham>? SanPham { get; set; }
    }
}
