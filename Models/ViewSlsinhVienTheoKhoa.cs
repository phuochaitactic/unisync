using System;
using System.Collections.Generic;

namespace BuildCongRenLuyen.Models;

public partial class ViewSlsinhVienTheoKhoa
{
    public long Idkhoa { get; set; }

    public string MaKhoa { get; set; } = null!;

    public string TenKhoa { get; set; } = null!;

    public int? Slsv { get; set; }
}
