namespace BuildCongRenLuyen.Models;

public partial class Kdmnhhk
{
    public long Idnhhk { get; set; }

    public long? MaNhhk { get; set; }

    public string TenNhhk { get; set; } = null!;

    public DateTime NgayBatDau { get; set; }

    public int TuanBatDau { get; set; }

    public int SoTuanHk { get; set; }

    public DateTime? NgayKetThuc { get; set; }

}
