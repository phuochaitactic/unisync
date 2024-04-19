namespace BuildCongRenLuyen.Models.CustomModels
{
    public class VaiTroHdnkModel
    {
        public long IdvaiTro { get; set; }

        public string MaDieu { get; set; }

        public string MaVaiTro { get; set; } = null!;

        public string TenVaiTro { get; set; } = null!;

        public int Diem { get; set; }
    }
}