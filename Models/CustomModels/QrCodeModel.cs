namespace BuildCongRenLuyen.Models.CustomModels
{
    public class QrCodeModelResult
    {
        public string MaSinhVien { get; set; }
        public string TenSinhVien { get; set; }
        public string MaHdnk { get; set; }
        public string TenHdnk { get; set; }
    }

    public class QrCodeModel
    {
        public long idHdnk { get; set; }
        public long IdSinhVien { get; set; }
        public DateTime NgayThamGia { get; set; }
    }
}