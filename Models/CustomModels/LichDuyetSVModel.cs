namespace BuildCongRenLuyen.Models.CustomModels
{
    public class LichDuyetSVModel
    {
        public long IdlichDuyet { get; set; }

        public string TenKhoa { get; set; }
        public string? MaKhoa { get; set; } = null;

        public string TenNhhk { get; set; }

        public DateTime? NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }
    }
}