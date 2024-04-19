using System;
using System.Collections.Generic;

namespace BuildCongRenLuyen.Models;

public partial class ViewSvchuaThamGiaHd
{
    public long? Idnhhk { get; set; }

    public long? MaNhhk { get; set; }

    public long Idkhoa { get; set; }

    public string MaKhoa { get; set; } = null!;

    public string TenKhoa { get; set; } = null!;

    public int? SlsvtheoCtdt { get; set; }

    public long? Idhdnk { get; set; }

    public string? MaHdnk { get; set; }

    public string? TenHdnk { get; set; }

    public int? Diemhdnk { get; set; }

    public int? SlsvdaTungThamGia { get; set; }

    public int? SlsvchuaThamGia { get; set; }
}
