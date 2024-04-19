namespace BuildCongRenLuyen.Models;

public partial class NdmgiangVien
{
    public long IdgiangVien { get; set; }

    public string MaNv { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public bool Phai { get; set; }

    public DateTime NgaySinh { get; set; }

    public long Idkhoa { get; set; }

    public string VaiTro { get; set; } = null!;

    public string ThongTinLienHe { get; set; } = null!;

    public virtual Ndmkhoa IdkhoaNavigation { get; set; } = null!;

}
