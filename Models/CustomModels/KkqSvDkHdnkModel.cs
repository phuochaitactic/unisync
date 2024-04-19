namespace BuildCongRenLuyen.Models.CustomModels
{
    public class KkqSvDkHdnkModel
    {
        public long IddangKy { get; set; }
        public string TensinhVien { get; set; }

        public string Tenhdnk { get; set; }

        public string TengiangVien { get; set; }

        public DateTime? NgayLap { get; set; }

        public DateTime? NgayDuyet { get; set; }

        public bool? TinhTrangDuyet { get; set; }

        public string? GhiChu { get; set; }

        public DateTime? NgayThamGia { get; set; }

        public bool? IsThamGia { get; set; }

        public int? SoDiem { get; set; }

        public string? MinhChungThamGia { get; set; }

        public string? VaiTroTg { get; set; }
        public DateTime? NgayBd { get; set; }

        public DateTime? NgayKt { get; set; }
    }


    public class KkqSvDkHdnkModel2
    {
        public long IddangKy { get; set; }
        public long IdSinhVien { get; set; }
        public string? TensinhVien { get; set; }
        public string? KyNangHDNK { get; set; }
        public string? NoiDungMinhChung { get; set; }
        public string? MaDieu { get; set; }


        public string Tenhdnk { get; set; }

        public string TengiangVien { get; set; }

        public DateTime? NgayLap { get; set; }

        public DateTime? NgayDuyet { get; set; }

        public bool? TinhTrangDuyet { get; set; }

        public string? GhiChu { get; set; }

        public DateTime? NgayThamGia { get; set; }

        public bool? IsThamGia { get; set; }

        public int? SoDiem { get; set; }

        public string? MinhChungThamGia { get; set; }

        public string? VaiTroTg { get; set; }
        public DateTime? NgayBd { get; set; }

        public DateTime? NgayKt { get; set; }
    }

    public class KkqBySvModel
    {
        public string TenNHHK { get; set; }

        public string HoTenSinhVien { get; set; }

        public string MaHDNK { get; set; }

        public string TenHDNK { get; set; }

        public DateTime? KyNangHDNK { get; set; }

        public DateTime? NgayDuyet { get; set; }

        public bool? TinhTrangDuyet { get; set; }

        public string? GhiChu { get; set; }

        public DateTime? NgayThamGia { get; set; }

        public bool? IsThamGia { get; set; }

        public int? SoDiem { get; set; }

        public string? MinhChungThamGia { get; set; }

        public string? VaiTroTg { get; set; }
        public DateTime? NgayBd { get; set; }

        public DateTime? NgayKt { get; set; }
    }

    public class KkqCuaSinhVienTheoNhhkModel
    {
        public string MaHDNK { get; set; }
        public string TenHDNK { get; set; }
        public string KyNangHDNK { get; set; }
        public string MaDieu { get; set; }
        public string NoiDungMinhChung { get; set; }
        public bool? IsThamGia { get; set; }
        public int? SoDiem { get; set; }
        public string VaiTroTG { get; set; }
    }

    public class ThongTinSinhVienThamGia
    {
        public long idSinhVien { get; set; }
        public string HoTenSV { get; set; }
        public string MSSV { get; set; }
        public string TenLop { get; set; }
        public string TenBacHeNganh { get; set; }
        public string NienKhoa { get; set; }
        public string VaiTroThamGia { get; set; }
        public string HoTenGV { get; set; }
    }

    public class KkqIsThamGiaModel
    {
        public long IdHdnk { get; set; }
        public long IdSinhVien { get; set; }
        public bool IsThamGia { get; set; }
    }
}