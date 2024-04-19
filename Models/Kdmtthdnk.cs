namespace BuildCongRenLuyen.Models;

public partial class Kdmtthdnk
{
    public long Idtthdnk { get; set; }

    public long Idkhoa { get; set; }

    public long IdgiangVien { get; set; }

    public long? Idphong { get; set; }

    public long? IddiaDiem { get; set; }

    public long Idhdnk { get; set; }

    public string? PhamVi { get; set; }

    public int? SoLuongThucTe { get; set; }

    public int SoLuongDuKien { get; set; }

    public string? GhiChu { get; set; }

    public string BuoiUuTien { get; set; } = null!;

    public TimeSpan? ThoiLuongToChuc { get; set; }

    public DateTime? NgayBđ { get; set; }

    public DateTime? NgayKt { get; set; }

    public bool IsCanPhong { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime? CreatedTime { get; set; }

    public string? CreatedBy { get; set; }

    public bool? TinhTragDuyet { get; set; }

    public bool? IsCanMinhChung { get; set; }

    public string? HinhAnh { get; set; }

}
