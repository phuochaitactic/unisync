namespace BuildCongRenLuyen.Models.CustomModels
{
    public class HdnkTheoHocKyModel
    {
        public long Idhdnk { get; set; }

        public string? TenBHNgChng { get; set; }

        public string MaHdnk { get; set; } = null!;

        public string TenHdnk { get; set; } = null!;
        public string NoiDungMinhChung { get; set; } = null!;

        public int Diemhdnk { get; set; }
        public int? CoVu { get; set; }

        public int? BanToChuc { get; set; }

        public string? KyNangHdnk { get; set; }
        public string? MaDieu { get; set; }
    }

    public class HdnkTheoSinhVienModel
    {
        public long Idhdnk { get; set; }

        public string? TenBHNgChng { get; set; }

        public string MaHdnk { get; set; } = null!;

        public string TenHdnk { get; set; } = null!;
        public string NoiDungMinhChung { get; set; } = null!;

        public int Diemhdnk { get; set; }
        public int? CoVu { get; set; }

        public int? BanToChuc { get; set; }

        public string? KyNangHdnk { get; set; }
        public string? MaDieu { get; set; }
        public bool? TinhTrangDuyet { get; set; }
    }

    public class HdnkTheoGiangVienModel
    {
        public long Idhdnk { get; set; }

        public string? TenBHNgChng { get; set; }

        public string MaHdnk { get; set; } = null!;

        public string TenHdnk { get; set; } = null!;
        public string NoiDungMinhChung { get; set; } = null!;

        public int Diemhdnk { get; set; }
        public int? CoVu { get; set; }

        public int? BanToChuc { get; set; }

        public string? KyNangHdnk { get; set; }
        public string? MaDieu { get; set; }
        public bool? TinhTrangDuyet { get; set; }
    }
}