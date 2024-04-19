namespace BuildCongRenLuyen.Models;

public partial class Sdmlop
{
    public long Idlop { get; set; }

    public string MaLop { get; set; } = null!;

    public string TenLop { get; set; } = null!;

    public string NamVao { get; set; } = null!;

    public long Idkhoa { get; set; }

    public long IdbhngChng { get; set; }

    public string? NienKhoa { get; set; }

}
