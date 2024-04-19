namespace BuildCongRenLuyen.Models;

public partial class KdmxepLoai
{
    public long IdxepLoai { get; set; }

    public long? IdvanBan { get; set; }

    public string MaLoaiDrl { get; set; } = null!;

    public int Diem { get; set; }

    public string XepLoai { get; set; } = null!;

}
