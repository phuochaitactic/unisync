namespace BuildCongRenLuyen.Models;

public partial class Kdmhdnk
{
    public long Idhdnk { get; set; }

    public long? IdminhChung { get; set; }

    public string MaHdnk { get; set; } = null!;

    public string TenHdnk { get; set; } = null!;

    public int Diemhdnk { get; set; }

    public int? CoVu { get; set; }

    public int? BanToChuc { get; set; }

    public string? KyNangHdnk { get; set; }

}
