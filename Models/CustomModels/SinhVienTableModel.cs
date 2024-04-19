namespace BuildCongRenLuyen.Models.CustomModels
{
    public class SinhVienTableModel
    {
        public long IdsinhVien { get; set; }

        public string MaSinhVien { get; set; } = null!;

        public string HoTenSinhVien { get; set; } = null!;

        public string MatKhau { get; set; } = null!;

        public bool Phai { get; set; }

        public DateTime NgaySinh { get; set; }

        public string TrangThaiSinhVien { get; set; } = null!;

        public bool IsBanCanSu { get; set; }

        public string TenGv { get; set; }

        public string TenLop { get; set; }

        public string TenNhhk { get; set; }
        public string? DiaChiLienHe { get; set; }

    }

    public class SinhVienTheoGiangVien
    {
        public long IdSinhVien { get; set; }
        public string MaSinhVien { get; set; }
        public string HoTenSinhVien { get; set; }
        public bool Phai { get; set; }
        public string MatKhau { get; set; }
        public DateTime NgaySinh { get; set; }
        public string TrangThaiSinhVien { get; set; } = null!;
        public bool IsBanCanSu { get; set; }
        public long IdGiangVien { get; set; }
        public string TenGv { get; set; }
        public long IdLop { get; set; }
        public string TenLop { get; set; }
        public string NienKhoa { get; set; }
        public string AcademicYear { get; set; }
        public string? DiaChiLienHe { get; set; }

    }

    public class SinhVienTheoHdnk
    {
        public long IdSinhVien { get; set; }
        public string MaSinhVien { get; set; }
        public string HoTenSinhVien { get; set; }
        public bool Phai { get; set; }
        public string MatKhau { get; set; }
        public DateTime NgaySinh { get; set; }
        public string TrangThaiSinhVien { get; set; } = null!;
        public bool IsBanCanSu { get; set; }
        public long IdLop { get; set; }
        public string TenLop { get; set; }
        public string NienKhoa { get; set; }
        public string AcademicYear { get; set; }
        public string? DiaChiLienHe { get; set; }

    }
}