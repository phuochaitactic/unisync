using System;
using System.Collections.Generic;

namespace BuildCongRenLuyen.Models;

public partial class Sdmsession
{
    public long Idse { get; set; }

    public string? Usename { get; set; }

    public bool? Sestatus { get; set; }

    public string? Session { get; set; }
}
