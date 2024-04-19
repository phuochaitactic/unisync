namespace BuildCongRenLuyen.Models.CustomModels
{
    public class ThongTinHoatDongNgoaiKhoaModelDatum
    {
        public long Idtthdnk { get; set; }

        public string? TenNHHK { get; set; }

        public string TenKhoa { get; set; }

        public string? TenBHNgChng { get; set; }

        public string TenGiangVien { get; set; }

        public string? TenPhong { get; set; }

        public string? TenDiaDiem { get; set; }

        public string TenHdnk { get; set; }

        public string? PhamVi { get; set; }

        public int? SoLuongThucTe { get; set; }

        public int SoLuongDuKien { get; set; }

        public string? GhiChu { get; set; }

        public string BuoiUuTien { get; set; } = null!;
        public int diemHdnk { get; set; }

        public TimeSpan? ThoiLuongToChuc { get; set; }

        public DateTime? NgayBd { get; set; }

        public DateTime? NgayKt { get; set; }

        public bool IsCanPhong { get; set; }

        public DateTime? LastUpdate { get; set; }

        public DateTime? CreatedTime { get; set; }

        public string CreatedBy { get; set; } = null!;
        public string? HinhAnh { get; set; } = null!;
        public bool? isCanMinhChung { get; set; }
        public bool? tinhTrangDuyet { get; set; }
    }

    public class ThongTinHoatDongNgoaiKhoaByMaModel
    {
        public long Idtthdnk { get; set; }

        public string TenNhhk { get; set; }

        public string TenKhoa { get; set; }

        public string? TenBhngChng { get; set; }

        public string TenGiangVien { get; set; }

        public string? TenPhong { get; set; }

        public string? TenDiaDiem { get; set; }

        public string TenHdnk { get; set; }

        public string? PhamVi { get; set; }

        public int? SoLuongThucTe { get; set; }

        public int SoLuongDuKien { get; set; }

        public string? GhiChu { get; set; }

        public string BuoiUuTien { get; set; } = null!;

        public TimeSpan? ThoiLuongToChuc { get; set; }

        public DateTime? NgayBd { get; set; }

        public DateTime? NgayKt { get; set; }

        public bool IsCanPhong { get; set; }

        public DateTime? LastUpdate { get; set; }

        public DateTime? CreatedTime { get; set; }

        public string CreatedBy { get; set; } = null!;
        public string? HinhAnh { get; set; } = null!;
        public bool? isCanMinhChung { get; set; }
        public bool? tinhTrangDuyet { get; set; }
    }
}

public class ThongTinHoatDongNgoaiKhoaModel
{
    public long Idtthdnk { get; set; }
    public string? TenNHHK { get; set; }
    public string? TenBHNgChng { get; set; }
    public string MaKhoa { get; set; }
    public string TenKhoa { get; set; }
    public string MaHdnk { get; set; }
    public string TenHdnk { get; set; }
    public int DiemHdnk { get; set; }
    public int? CoVu { get; set; }
    public int? BanToChuc { get; set; }
    public string KyNangHDNK { get; set; }
    public string MaDieu { get; set; }
    public string NoiDungMinhChung { get; set; }
    public long IDGiangVien { get; set; }
    public long? IDPhong { get; set; }
    public long? IDDiaDiem { get; set; }
    public long TenGiangVien { get; set; }
    public long? TenPhong { get; set; }
    public long? TenDiaDiem { get; set; }
    public string? PhamVi { get; set; }
    public int? SoLuongThucTe { get; set; }
    public int SoLuongDuKien { get; set; }
    public string? GhiChu { get; set; }
    public string BuoiUuTien { get; set; } = null!;
    public TimeSpan? ThoiLuongToChuc { get; set; }
    public DateTime? NgayBd { get; set; }
    public DateTime? NgayKt { get; set; }
    public bool IsCanPhong { get; set; }
    public DateTime? LastUpdate { get; set; }
    public DateTime? CreatedTime { get; set; }
    public string CreatedBy { get; set; } = null!;
    public bool? isCanMinhChung { get; set; }
    public bool? tinhTrangDuyet { get; set; }
    public string? HinhAnh { get; set; }
}

public class ThongTinHoatDongNgoaiKhoaModel2
{
    public long Idtthdnk { get; set; }
    public long IdHdnk { get; set; }
    public long IdDuLieu { get; set; }
    public string? TenNHHK { get; set; }
    public string? TenBHNgChng { get; set; }
    public string? MaKhoa { get; set; }
    public string? TenKhoa { get; set; }
    public string? MaHdnk { get; set; }
    public string? TenHdnk { get; set; }
    public int DiemHdnk { get; set; }
    public int? CoVu { get; set; }
    public int? BanToChuc { get; set; }
    public string? KyNangHDNK { get; set; }
    public string? NoiDungMinhChung { get; set; }
    public string? TenGiangVien { get; set; }
    public string? TenPhong { get; set; }
    public string? TenDiaDiem { get; set; }
    public string? PhamVi { get; set; }
    public int? SoLuongThucTe { get; set; }
    public int SoLuongDuKien { get; set; }
    public string? GhiChu { get; set; }
    public string BuoiUuTien { get; set; } = null!;
    public TimeSpan? ThoiLuongToChuc { get; set; }
    public DateTime? NgayBd { get; set; }
    public DateTime? NgayKt { get; set; }
    public bool IsCanPhong { get; set; }
    public DateTime? LastUpdate { get; set; }
    public DateTime? CreatedTime { get; set; }
    public string? CreatedBy { get; set; } = null;
    public bool? isCanMinhChung { get; set; }
    public bool? tinhTrangDuyet { get; set; }
    public string? HinhAnh { get; set; }
}