namespace BuildCongRenLuyen.Models.CustomModels
{
    public class HdnkTheoNganhModel
    {
        public long Idhdnk { get; set; }

        public string? TenBHNgChng { get; set; }

        public string MaHDNK { get; set; } = null!;

        public string TenHdnk { get; set; } = null!;

        public int Diemhdnk { get; set; }
        public string MaDieu { get; set; }
        public int? CoVu { get; set; }

        public int? BanToChuc { get; set; }

        public string? KyNangHdnk { get; set; }
    }
}