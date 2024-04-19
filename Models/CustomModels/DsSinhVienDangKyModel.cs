namespace BuildCongRenLuyen.Models.CustomModels
{
    public class DsSinhVienDangKyModel
    {
        public long Idhdnk { get; set; }

        public long? IdminhChung { get; set; }

        public string? PhamVi { get; set; }

        public DateTime? NgayBD { get; set; }
        public DateTime? NgayKT { get; set; }

        public TimeSpan? ThoiLuongToChuc { get; set; }

        public string? TenKhoa { get; set; }
        public string? TenHdnk { get; set; }
        public string? maDieu { get; set; }
        public string? HinhAnh { get; set; }
        public int DiemHDNK { get; set; }
    }

    public class DsSinhVienDangKyHdnkModel
    {
        public string? MaSinhVIen { get; set; }
        public string? HoTenSinhVien { get; set; }
        public string? TenNHHK { get; set; }
        public string? MaLop { get; set; }
        public bool? TinhTrangDuyet { get; set; }
        public string? LoiNhan { get; set; }

    }

    public class GiangVienComment
    {
        public long IdAP { get; set; }
        public string? TenSinhVien { get; set; }
        public string? TenNHHK { get; set; }
        public string? LoiNhan { get; set; }
    }

    public class GiangVienCommentResult
    {
        public long IdAP { get; set; }
        public long? IdSinhVien { get; set; }
        public long? IdNHHK { get; set; }
        public string? TenNHHK { get; set; }
        public string? LoiNhan { get; set; }
        public string? TongDiem { get; set; }
    }
}