namespace BuildCongRenLuyen.Models.CustomModels
{
    public class XepLoaiModel
    {
        public long IdxepLoai { get; set; }

        public string? TenvanBan { get; set; }

        public string MaLoaiDrl { get; set; } = null!;

        public int Diem { get; set; }

        public string XepLoai { get; set; } = null!;
    }
}