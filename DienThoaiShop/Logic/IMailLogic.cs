using DTShop.Models;
using DTShop.Models;

namespace DTShop.Logic
{
    public interface IMailLogic
    {
        Task GoiEmail(MailInfo mailInfo);
        Task GoiEmailDatHangThanhCong(DatHang datHang, MailInfo mailInfo);
    }
}