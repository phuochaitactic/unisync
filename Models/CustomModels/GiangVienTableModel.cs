namespace BuildCongRenLuyen.Models.CustomModels
{
    public class GiangVienTableModel
    {
        public long IdgiangVien { get; set; }

        public string MaNv { get; set; } = null!;

        public string MatKhau { get; set; } = null!;

        public string HoTen { get; set; } = null!;

        public bool Phai { get; set; }

        public DateTime NgaySinh { get; set; }

        public string TenKhoa { get; set; }
        public long IdKhoa { get; set; }

        public string VaiTro { get; set; } = null!;

        public string ThongTinLienHe { get; set; } = null!;
    }
}