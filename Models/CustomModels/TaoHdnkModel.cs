namespace BuildCongRenLuyen.Models.CustomModels
{
    public class TaoHdnkModel
    {
        public HoatDongNgoaiKhoaModel hoatDongNgoaiKhoa { get; set; }
        public ThongTinHoatDongNgoaiKhoaModelDatum tthdnk { get; set; }
    }

    public class TaoHdnkModelResult
    {
        public Kdmhdnk hoatDongNgoaiKhoa { get; set; }
        public KdmduLieuHdnk duLieuHdnk { get; set; }
        public Kdmtthdnk tthdnk { get; set; }
    }
}