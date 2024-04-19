using Microsoft.EntityFrameworkCore;

namespace BuildCongRenLuyen.Models;

public partial class MyDBContext : DbContext
{
    public MyDBContext()
    {
    }

    public MyDBContext(DbContextOptions<MyDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kdmadmin> Kdmadmins { get; set; }

    public virtual DbSet<Kdmbh> Kdmbhs { get; set; }

    public virtual DbSet<KdmbhngChng> KdmbhngChngs { get; set; }

    public virtual DbSet<Kdmdieu> Kdmdieus { get; set; }

    public virtual DbSet<Kdmdssvdk> Kdmdssvdks { get; set; }

    public virtual DbSet<KdmduLieuHdnk> KdmduLieuHdnks { get; set; }

    public virtual DbSet<Kdmhdnk> Kdmhdnks { get; set; }

    public virtual DbSet<KdmlichDangKy> KdmlichDangKies { get; set; }

    public virtual DbSet<KdmlichDuyetSv> KdmlichDuyetSvs { get; set; }

    public virtual DbSet<KdmlichTaoHdnk> KdmlichTaoHdnks { get; set; }

    public virtual DbSet<KdmloaiHdnk> KdmloaiHdnks { get; set; }

    public virtual DbSet<KdmndminhChung> KdmndminhChungs { get; set; }

    public virtual DbSet<Kdmngh> Kdmnghs { get; set; }

    public virtual DbSet<Kdmnhhk> Kdmnhhks { get; set; }

    public virtual DbSet<Kdmtthdnk> Kdmtthdnks { get; set; }

    public virtual DbSet<KdmvanBan> KdmvanBans { get; set; }

    public virtual DbSet<KdmxepLoai> KdmxepLoais { get; set; }

    public virtual DbSet<Kkqsvdkhdnk> Kkqsvdkhdnks { get; set; }

    public virtual DbSet<NdmgiangVien> NdmgiangViens { get; set; }

    public virtual DbSet<Ndmkhoa> Ndmkhoas { get; set; }

    public virtual DbSet<PdmdiaDiem> PdmdiaDiems { get; set; }

    public virtual DbSet<Pdmphong> Pdmphongs { get; set; }

    public virtual DbSet<Sdmlop> Sdmlops { get; set; }

    public virtual DbSet<Sdmsession> Sdmsessions { get; set; }

    public virtual DbSet<Sdmsv> Sdmsvs { get; set; }

    public virtual DbSet<ViewSlsinhVienThamGiaHdtheoHk> ViewSlsinhVienThamGiaHdtheoHks { get; set; }

    public virtual DbSet<ViewSlsinhVienTheoKhoa> ViewSlsinhVienTheoKhoas { get; set; }

    public virtual DbSet<ViewSvchuaThamGiaHd> ViewSvchuaThamGiaHds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=103.98.160.17;Initial Catalog=AQCongDiemRenLuyen;User ID=dev;Password=dev@edusoft;Connect Timeout=1800;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kdmadmin>(entity =>
        {
            entity.HasKey(e => e.Idadmin).HasName("PK__KDMAdmin__D704F3E85C1EA203");

            entity.ToTable("KDMAdmin");

            entity.Property(e => e.Idadmin)
                .ValueGeneratedNever()
                .HasColumnName("IDAdmin");
            entity.Property(e => e.AdminPassword).HasMaxLength(500);
            entity.Property(e => e.Username).HasMaxLength(500);
        });

        modelBuilder.Entity<Kdmbh>(entity =>
        {
            entity.HasKey(e => e.Idbh).HasName("PK__KDMBH__B87DA8DF5B86DA5D");

            entity.ToTable("KDMBH");

            entity.Property(e => e.Idbh)
                .ValueGeneratedNever()
                .HasColumnName("IDBH");
            entity.Property(e => e.MaBh)
                .HasMaxLength(500)
                .HasColumnName("MaBH");
            entity.Property(e => e.TenBh)
                .HasMaxLength(1000)
                .HasColumnName("TenBH");
        });

        modelBuilder.Entity<KdmbhngChng>(entity =>
        {
            entity.HasKey(e => e.IdbhngChng).HasName("PK__KDMBHNgC__9B0F5EE554EBB057");

            entity.ToTable("KDMBHNgCHNG", tb =>
                {
                    tb.HasTrigger("trg_insert");
                    tb.HasTrigger("trg_update");
                });

            entity.Property(e => e.IdbhngChng)
                .ValueGeneratedNever()
                .HasColumnName("IDBHNgChng");
            entity.Property(e => e.Idbh).HasColumnName("IDBH");
            entity.Property(e => e.Idngh).HasColumnName("IDNgh");
            entity.Property(e => e.MaBhngChng)
                .HasMaxLength(1000)
                .HasColumnName("MaBHNgCHNg");
            entity.Property(e => e.TenBhngChng).HasColumnName("TenBHNgChng");


        });

        modelBuilder.Entity<Kdmdieu>(entity =>
        {
            entity.HasKey(e => e.Iddieu).HasName("PK__KDMDieu__52C174719F2FE29C");

            entity.ToTable("KDMDieu");

            entity.Property(e => e.Iddieu)
                .ValueGeneratedNever()
                .HasColumnName("IDDieu");
            entity.Property(e => e.IdvanBan).HasColumnName("IDVanBan");
            entity.Property(e => e.MaDieu).HasMaxLength(1000);


        });

        modelBuilder.Entity<Kdmdssvdk>(entity =>
        {
            entity.HasKey(e => e.Idap).HasName("PK__KDMAPPRO__B87DB0F64C2404F1");

            entity.ToTable("KDMDSSVDK");

            entity.Property(e => e.Idap)
                .ValueGeneratedNever()
                .HasColumnName("IDAP");
            entity.Property(e => e.Idnhhk).HasColumnName("IDNHHK");
            entity.Property(e => e.IdsinhVien).HasColumnName("IDSinhVien");
            entity.Property(e => e.Loinhan).HasColumnName("LOINHAN");
            entity.Property(e => e.TongDiem).HasMaxLength(50);


        });

        modelBuilder.Entity<KdmduLieuHdnk>(entity =>
        {
            entity.HasKey(e => e.IdduLieuHdnk).HasName("PK__KDMDuLie__464A15622B66D00B");

            entity.ToTable("KDMDuLieuHDNK");

            entity.Property(e => e.IdduLieuHdnk)
                .ValueGeneratedNever()
                .HasColumnName("IDDuLieuHDNK");
            entity.Property(e => e.IdbhngChng).HasColumnName("IDBHNgChng");
            entity.Property(e => e.Idhdnk).HasColumnName("IDHDNK");
            entity.Property(e => e.Idnhhk).HasColumnName("IDNHHK");


        });

        modelBuilder.Entity<Kdmhdnk>(entity =>
        {
            entity.HasKey(e => e.Idhdnk).HasName("PK__KDMHDNK__F8B3F945D5CD1578");

            entity.ToTable("KDMHDNK");

            entity.Property(e => e.Idhdnk)
                .ValueGeneratedNever()
                .HasColumnName("IDHDNK");
            entity.Property(e => e.Diemhdnk).HasColumnName("DIEMHDNK");
            entity.Property(e => e.IdminhChung).HasColumnName("IDMinhChung");
            entity.Property(e => e.KyNangHdnk)
                .HasMaxLength(500)
                .HasColumnName("KyNangHDNK");
            entity.Property(e => e.MaHdnk)
                .HasMaxLength(1000)
                .HasColumnName("MaHDNK");
            entity.Property(e => e.TenHdnk)
                .HasMaxLength(1000)
                .HasColumnName("TenHDNK");


        });

        modelBuilder.Entity<KdmlichDangKy>(entity =>
        {
            entity.HasKey(e => e.IdlichDangKy).HasName("PK__KDMLichD__AB444BD5B5F935ED");

            entity.ToTable("KDMLichDangKy");

            entity.Property(e => e.IdlichDangKy)
                .ValueGeneratedNever()
                .HasColumnName("IDLichDangKy");
            entity.Property(e => e.Idlop).HasColumnName("IDLop");
            entity.Property(e => e.Idnhhk).HasColumnName("IDNHHK");
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");


        });

        modelBuilder.Entity<KdmlichDuyetSv>(entity =>
        {
            entity.HasKey(e => e.IdlichDuyet).HasName("PK__KDMLichD__40AD497D73AD06CF");

            entity.ToTable("KDMLichDuyetSV");

            entity.Property(e => e.IdlichDuyet)
                .ValueGeneratedNever()
                .HasColumnName("IDLichDuyet");
            entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");
            entity.Property(e => e.Idnhhk).HasColumnName("IDNHHK");
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");


        });

        modelBuilder.Entity<KdmlichTaoHdnk>(entity =>
        {
            entity.HasKey(e => e.IdlichTaoHdnk).HasName("PK__KDMLichT__24E7438B937C8A6A");

            entity.ToTable("KDMLichTaoHDNK");

            entity.Property(e => e.IdlichTaoHdnk)
                .ValueGeneratedNever()
                .HasColumnName("IDLichTaoHDNK");
            entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");
            entity.Property(e => e.Idnhhk).HasColumnName("IDNHHK");
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");


        });

        modelBuilder.Entity<KdmloaiHdnk>(entity =>
        {
            entity.HasKey(e => e.IdloaiHdnk).HasName("PK__KDMLoaiH__C42524D953AC063D");

            entity.ToTable("KDMLoaiHDNK", tb => tb.HasTrigger("TRIG_MINHCHUNG_LOAIHDNK_UPDATE"));

            entity.Property(e => e.IdloaiHdnk)
                .ValueGeneratedNever()
                .HasColumnName("IDLoaiHDNK");
            entity.Property(e => e.DonViThucHien).HasMaxLength(1000);
            entity.Property(e => e.MaLoaiHdnk)
                .HasMaxLength(1000)
                .HasColumnName("MaLoaiHDNK");
            entity.Property(e => e.NoiDungLoaiHdnk).HasColumnName("NoiDungLoaiHDNK");
        });

        modelBuilder.Entity<KdmndminhChung>(entity =>
        {
            entity.HasKey(e => e.IdminhChung).HasName("PK__KDMNDMin__3F796E8D26CE7FA1");

            entity.ToTable("KDMNDMinhChung", tb => tb.HasTrigger("TRIG_MINHCHUNG_LOAIHDNK_INSERT"));

            entity.Property(e => e.IdminhChung)
                .ValueGeneratedNever()
                .HasColumnName("IDMinhChung");
            entity.Property(e => e.Iddieu).HasColumnName("IDDieu");
            entity.Property(e => e.IdloaiHdnk).HasColumnName("IDLoaiHDNK");


        });

        modelBuilder.Entity<Kdmngh>(entity =>
        {
            entity.HasKey(e => e.Idngh).HasName("PK__KDMNgh__94587EB33F090B76");

            entity.ToTable("KDMNgh");

            entity.Property(e => e.Idngh)
                .ValueGeneratedNever()
                .HasColumnName("IDNgh");
            entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");
            entity.Property(e => e.MaNgh).HasMaxLength(500);
            entity.Property(e => e.TenNgh).HasMaxLength(1000);


        });

        modelBuilder.Entity<Kdmnhhk>(entity =>
        {
            entity.HasKey(e => e.Idnhhk).HasName("PK__KDMNHHK__3D13F8174B27C029");

            entity.ToTable("KDMNHHK", tb =>
                {
                    tb.HasTrigger("TRG_INS_NGAYKETTHUC");
                    tb.HasTrigger("TRG_UPD_NGAYKETTHUC");
                });

            entity.Property(e => e.Idnhhk)
                .ValueGeneratedNever()
                .HasColumnName("IDNHHK");
            entity.Property(e => e.MaNhhk).HasColumnName("MaNHHK");
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            entity.Property(e => e.SoTuanHk).HasColumnName("SoTuanHK");
            entity.Property(e => e.TenNhhk)
                .HasMaxLength(1000)
                .HasColumnName("TenNHHK");
        });

        modelBuilder.Entity<Kdmtthdnk>(entity =>
        {
            entity.HasKey(e => e.Idtthdnk).HasName("PK__KDMTTHDN__42367C9F8BAFDD7F");

            entity.ToTable("KDMTTHDNK");

            entity.Property(e => e.Idtthdnk)
                .ValueGeneratedNever()
                .HasColumnName("IDTTHDNK");
            entity.Property(e => e.BuoiUuTien).HasMaxLength(1000);
            entity.Property(e => e.CreatedBy).HasMaxLength(1000);
            entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            entity.Property(e => e.GhiChu).HasMaxLength(2000);
            entity.Property(e => e.IddiaDiem).HasColumnName("IDDiaDiem");
            entity.Property(e => e.IdgiangVien).HasColumnName("IDGiangVien");
            entity.Property(e => e.Idhdnk).HasColumnName("IDHDNK");
            entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");
            entity.Property(e => e.Idphong).HasColumnName("IDPhong");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.NgayBđ)
                .HasColumnType("datetime")
                .HasColumnName("NgayBĐ");
            entity.Property(e => e.NgayKt)
                .HasColumnType("datetime")
                .HasColumnName("NgayKT");
            entity.Property(e => e.PhamVi).HasMaxLength(1000);
            entity.Property(e => e.SoLuongThucTe).HasDefaultValueSql("((0))");


        });

        modelBuilder.Entity<KdmvanBan>(entity =>
        {
            entity.HasKey(e => e.IdvanBan).HasName("PK__KDMVanBa__FDADF58B3B21FAEA");

            entity.ToTable("KDMVanBan");

            entity.Property(e => e.IdvanBan)
                .ValueGeneratedNever()
                .HasColumnName("IDVanBan");
            entity.Property(e => e.MaVanBan).HasMaxLength(1000);
            entity.Property(e => e.TenVanBan).HasMaxLength(1000);
        });

        modelBuilder.Entity<KdmxepLoai>(entity =>
        {
            entity.HasKey(e => e.IdxepLoai).HasName("PK__KDMXepLo__631030D5079CB46A");

            entity.ToTable("KDMXepLoai");

            entity.Property(e => e.IdxepLoai)
                .ValueGeneratedNever()
                .HasColumnName("IDXepLoai");
            entity.Property(e => e.IdvanBan).HasColumnName("IDVanBan");
            entity.Property(e => e.MaLoaiDrl)
                .HasMaxLength(2000)
                .HasColumnName("MaLoaiDRL");
            entity.Property(e => e.XepLoai).HasMaxLength(200);


        });

        modelBuilder.Entity<Kkqsvdkhdnk>(entity =>
        {
            entity.HasKey(e => e.IddangKy).HasName("PK__KKQSVDKH__735660833F9C0781");

            entity.ToTable("KKQSVDKHDNK", tb =>
                {
                    tb.HasTrigger("COUNT_DANGKY_THONGTINHDNK");
                    tb.HasTrigger("COUNT_DANGKY_THONGTINHDNK_delete");
                    tb.HasTrigger("TRG_DIEMVAITROTG");
                    tb.HasTrigger("TRG_UP_DIEMVAITROTG");
                    tb.HasTrigger("tr_HuyTongDiem");
                    tb.HasTrigger("tr_TongDiem");
                });

            entity.Property(e => e.IddangKy)
                .ValueGeneratedNever()
                .HasColumnName("IDDangKy");
            entity.Property(e => e.IdgiangVien).HasColumnName("IDGiangVien");
            entity.Property(e => e.Idhdnk).HasColumnName("IDHDNK");
            entity.Property(e => e.IdsinhVien).HasColumnName("IDSinhVien");
            entity.Property(e => e.NgayDuyet).HasColumnType("datetime");
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.NgayThamGia).HasColumnType("datetime");
            entity.Property(e => e.VaiTroTg)
                .HasMaxLength(500)
                .HasColumnName("VaiTroTG");


        });

        modelBuilder.Entity<NdmgiangVien>(entity =>
        {
            entity.HasKey(e => e.IdgiangVien).HasName("PK__NDMGiang__EA9BCA1CE8211194");

            entity.ToTable("NDMGiangVien");

            entity.Property(e => e.IdgiangVien)
                .ValueGeneratedNever()
                .HasColumnName("IDGiangVien");
            entity.Property(e => e.HoTen).HasMaxLength(1000);
            entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");
            entity.Property(e => e.MaNv)
                .HasMaxLength(1000)
                .HasColumnName("MaNV");
            entity.Property(e => e.MatKhau).HasMaxLength(1000);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.ThongTinLienHe).HasMaxLength(1000);
            entity.Property(e => e.VaiTro).HasMaxLength(500);


        });

        modelBuilder.Entity<Ndmkhoa>(entity =>
        {
            entity.HasKey(e => e.Idkhoa).HasName("PK__NDMKhoa__1AB42731444271B0");

            entity.ToTable("NDMKhoa");

            entity.Property(e => e.Idkhoa)
                .ValueGeneratedNever()
                .HasColumnName("IDKhoa");
            entity.Property(e => e.MaKhoa).HasMaxLength(1000);
            entity.Property(e => e.MatKhau).HasMaxLength(1000);
            entity.Property(e => e.TenKhoa).HasMaxLength(1000);
        });

        modelBuilder.Entity<PdmdiaDiem>(entity =>
        {
            entity.HasKey(e => e.IddiaDiem).HasName("PK__PDMDiaDi__3DD0D654F30FF54A");

            entity.ToTable("PDMDiaDiem");

            entity.Property(e => e.IddiaDiem)
                .ValueGeneratedNever()
                .HasColumnName("IDDiaDiem");
            entity.Property(e => e.DiaChi).HasMaxLength(1000);
            entity.Property(e => e.TenDiaDiem).HasMaxLength(1000);
        });

        modelBuilder.Entity<Pdmphong>(entity =>
        {
            entity.HasKey(e => e.Idphong).HasName("PK__PDMPhong__81CB11522F27D9D9");

            entity.ToTable("PDMPhong");

            entity.Property(e => e.Idphong)
                .ValueGeneratedNever()
                .HasColumnName("IDPhong");
            entity.Property(e => e.CoSo).HasMaxLength(500);
            entity.Property(e => e.DayPhong).HasMaxLength(500);
            entity.Property(e => e.IddiaDiem).HasColumnName("IDDiaDiem");
            entity.Property(e => e.MaPhong).HasMaxLength(1000);
            entity.Property(e => e.TenPhong).HasMaxLength(1000);
            entity.Property(e => e.TinhChatPhong).HasMaxLength(1000);


        });

        modelBuilder.Entity<Sdmlop>(entity =>
        {
            entity.HasKey(e => e.Idlop).HasName("PK__SDMLop__95D0020399EB1AB3");

            entity.ToTable("SDMLop");

            entity.Property(e => e.Idlop)
                .ValueGeneratedNever()
                .HasColumnName("IDLop");
            entity.Property(e => e.IdbhngChng).HasColumnName("IDBHNgCHNg");
            entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");
            entity.Property(e => e.MaLop).HasMaxLength(1000);
            entity.Property(e => e.NamVao).HasMaxLength(500);
            entity.Property(e => e.NienKhoa).HasMaxLength(500);
            entity.Property(e => e.TenLop).HasMaxLength(1000);


        });

        modelBuilder.Entity<Sdmsession>(entity =>
        {
            entity.HasKey(e => e.Idse).HasName("PK__SDMSESSI__B87C02A8B1B59679");

            entity.ToTable("SDMSESSION");

            entity.Property(e => e.Idse)
                .ValueGeneratedNever()
                .HasColumnName("IDSE");
            entity.Property(e => e.Session).HasColumnName("SESSION");
            entity.Property(e => e.Sestatus).HasColumnName("SESTATUS");
            entity.Property(e => e.Usename).HasColumnName("USENAME");
        });

        modelBuilder.Entity<Sdmsv>(entity =>
        {
            entity.HasKey(e => e.IdsinhVien).HasName("PK__SDMSV__9015D5B3961C512D");

            entity.ToTable("SDMSV");

            entity.Property(e => e.IdsinhVien)
                .ValueGeneratedNever()
                .HasColumnName("IDSinhVien");
            entity.Property(e => e.DiaChiLienHe).HasMaxLength(50);
            entity.Property(e => e.HoTenSinhVien).HasMaxLength(1000);
            entity.Property(e => e.IdgiangVien).HasColumnName("IDGiangVien");
            entity.Property(e => e.Idlop).HasColumnName("IDLop");
            entity.Property(e => e.Idnhhk).HasColumnName("IDNHHK");
            entity.Property(e => e.MaSinhVien).HasMaxLength(1000);
            entity.Property(e => e.MatKhau).HasMaxLength(1000);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.TrangThaiSinhVien).HasMaxLength(1000);


        });

        modelBuilder.Entity<ViewSlsinhVienThamGiaHdtheoHk>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_SLSinhVienThamGiaHDTheoHK");

            entity.Property(e => e.Diemhdnk).HasColumnName("DIEMHDNK");
            entity.Property(e => e.Idhdnk).HasColumnName("IDHDNK");
            entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");
            entity.Property(e => e.Idnhhk).HasColumnName("IDNHHK");
            entity.Property(e => e.MaHdnk)
                .HasMaxLength(1000)
                .HasColumnName("MaHDNK");
            entity.Property(e => e.MaNhhk).HasColumnName("MaNHHK");
            entity.Property(e => e.Slsv).HasColumnName("SLSV");
            entity.Property(e => e.TenHdnk)
                .HasMaxLength(1000)
                .HasColumnName("TenHDNK");
            entity.Property(e => e.TenKhoa).HasMaxLength(1000);
            entity.Property(e => e.TenNhhk)
                .HasMaxLength(1000)
                .HasColumnName("TenNHHK");
        });

        modelBuilder.Entity<ViewSlsinhVienTheoKhoa>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_SLSinhVienTheoKhoa");

            entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");
            entity.Property(e => e.MaKhoa).HasMaxLength(1000);
            entity.Property(e => e.Slsv).HasColumnName("SLSV");
            entity.Property(e => e.TenKhoa).HasMaxLength(1000);
        });

        modelBuilder.Entity<ViewSvchuaThamGiaHd>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_SVChuaThamGiaHD");

            entity.Property(e => e.Diemhdnk).HasColumnName("DIEMHDNK");
            entity.Property(e => e.Idhdnk).HasColumnName("IDHDNK");
            entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");
            entity.Property(e => e.Idnhhk).HasColumnName("IDNHHK");
            entity.Property(e => e.MaHdnk)
                .HasMaxLength(1000)
                .HasColumnName("MaHDNK");
            entity.Property(e => e.MaKhoa).HasMaxLength(1000);
            entity.Property(e => e.MaNhhk).HasColumnName("MaNHHK");
            entity.Property(e => e.SlsvchuaThamGia).HasColumnName("SLSVChuaThamGia");
            entity.Property(e => e.SlsvdaTungThamGia).HasColumnName("SLSVDaTungThamGia");
            entity.Property(e => e.SlsvtheoCtdt).HasColumnName("SLSVTheoCTDT");
            entity.Property(e => e.TenHdnk)
                .HasMaxLength(1000)
                .HasColumnName("TenHDNK");
            entity.Property(e => e.TenKhoa).HasMaxLength(1000);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
