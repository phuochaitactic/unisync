namespace BuildCongRenLuyen.Models.CustomModels
{
    public class LopTableModel
    {
        public long Idlop { get; set; }

        public string MaLop { get; set; } = null!;

        public string TenLop { get; set; } = null!;

        public string NamVao { get; set; } = null!;

        public string TenKhoa { get; set; } = null!;

        public string TenBhngChng { get; set; } = null!;
        public string TenNhhk { get; set; } = null!;

        public string? NienKhoa { get; set; }
    }
}