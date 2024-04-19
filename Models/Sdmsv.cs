namespace BuildCongRenLuyen.Models;

public partial class Sdmsv
{
    public long IdsinhVien { get; set; }

    public string MaSinhVien { get; set; } = null!;

    public string HoTenSinhVien { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public bool Phai { get; set; }

    public DateTime NgaySinh { get; set; }

    public string TrangThaiSinhVien { get; set; } = null!;

    public bool IsBanCanSu { get; set; }

    public long IdgiangVien { get; set; }

    public long Idlop { get; set; }

    public long Idnhhk { get; set; }

    public string? DiaChiLienHe { get; set; }

}
