namespace BuildCongRenLuyen.Models;

public partial class Pdmphong
{
    public long Idphong { get; set; }

    public string MaPhong { get; set; } = null!;

    public string TenPhong { get; set; } = null!;

    public long IddiaDiem { get; set; }

    public int SucChua { get; set; }

    public string DayPhong { get; set; } = null!;

    public string CoSo { get; set; } = null!;

    public string TinhChatPhong { get; set; } = null!;

    public int DienTichSuDung { get; set; }

}
