using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Controllers;

/// <summary>
/// Handles CRUD operations for hoat dong ngoai khoa data
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class KDMHoatDongNgoaiKhoaController : ControllerBase
{
    private readonly IHoatDongNgoaiKhoaService _hoatDongNgoaiKhoaService;
    private readonly IAuthService _auth;

    public KDMHoatDongNgoaiKhoaController(IHoatDongNgoaiKhoaService hoatDongNgoaiKhoaService, IAuthService auth)
    {
        this._hoatDongNgoaiKhoaService = hoatDongNgoaiKhoaService;
        this._auth = auth;
    }

    /// <summary>
    /// Exports hoat dong ngoai khoa table metadata to Excel
    /// </summary>
    /// <returns>Excel file</returns>
    [HttpGet("ToExcel/")]
    public IActionResult GetToExcel()
    {
        try
        {
            // Get metadata
            var properties = typeof(HoatDongNgoaiKhoaModel).GetProperties();

            var columnMetadata = new List<Dictionary<string, string>>();

            foreach (var property in properties)
            {
                // Add to metadata list
                if (property.Name != "Idhdnk")
                {
                    var column = new Dictionary<string, string>();
                    column.Add("ColumnName", property.Name);
                    column.Add("DataType", property.PropertyType.Name);
                    columnMetadata.Add(column);
                }
            }
            // Export Excel file
            return ExcelExporter.ExportToExcel(columnMetadata);
        }
        catch (Exception ex)
        {
            return BadRequest($"ERROR: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all hoat dong ngoai khoa records
    /// </summary>
    /// <returns>List of hoat dong ngoai khoa</returns>
    [HttpGet]
    public async Task<object> Get()
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
        {
            var hdNK = await _hoatDongNgoaiKhoaService.GetAll();
            var response = (hdNK as ObjectResult)?.Value;
            return response;
        }
        return Unauthorized("Unauthorized");
    }

    /// <summary>
    /// Search hoat dong ngoai khoa records by name
    /// </summary>
    /// <param name="tenHoatDongNgoaiKhoa">Name</param>
    /// <returns>Matching records</returns>
    [HttpGet("{tenHoatDongNgoaiKhoa}")]
    public async Task<object> GetByTenHDNK(string tenHoatDongNgoaiKhoa)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
        {
            var hdNK = await _hoatDongNgoaiKhoaService.GetByTenHoatDongNgoaiKhoa(tenHoatDongNgoaiKhoa);
            var response = (hdNK as ObjectResult)?.Value;
            return response;
        }

        return Unauthorized("Unauthorized");
    }

    [HttpGet("TheoNganh")]
    public async Task<object> GetByNganh(long idKhoa, string TenBhngChng)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
        {
            var hdNK = await _hoatDongNgoaiKhoaService.GetByNganh(idKhoa, TenBhngChng);
            var response = (hdNK as ObjectResult)?.Value;
            return response;
        }

        return Unauthorized("Unauthorized");
    }

    [HttpGet("TheoHocKy")]
    public async Task<object> GetByHocKy(long idKhoa, long IdNhhk)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
        {
            var hdNK = await _hoatDongNgoaiKhoaService.GetByHocKy(idKhoa, IdNhhk);
            var response = (hdNK as ObjectResult)?.Value;
            return response;
        }

        return Unauthorized("Unauthorized");
    }

    [HttpGet("LichSuHdnkSv")]
    public async Task<object> GetbySinhVienTheoHocKy(string MaSinhVien, long MaNhhk)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
        {
            var hdNK = await _hoatDongNgoaiKhoaService.GetbySinhVienTheoHocKy(MaSinhVien, MaNhhk);
            var response = (hdNK as ObjectResult)?.Value;
            return response;
        }

        return Unauthorized("Unauthorized");
    }

    [HttpGet("LichSuHdnkGv")]
    public async Task<object> GetbyGiangVienTheoHocKy(string MaGiangVien, long MaNhhk)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
        {
            var hdNK = await _hoatDongNgoaiKhoaService.GetbyGiangVienTheoHocKy(MaGiangVien, MaNhhk);
            var response = (hdNK as ObjectResult)?.Value;
            return response;
        }

        return Unauthorized("Unauthorized");
    }

    /// <summary>
    /// Creates new hoat dong ngoai khoa record
    /// </summary>
    /// <param name="noiDungMinhChung">Evidence content</param>
    /// <param name="inputData">Hoat dong data</param>
    /// <returns>New record</returns>
    [HttpPost]
    public async Task<object> Post([FromBody] HdnkTheoHocKyModel inputData)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
        {
            var hdNK = await _hoatDongNgoaiKhoaService.CreateHDNK(inputData);

            var response = (hdNK as ObjectResult)?.Value;
            return response;
        }

        return Unauthorized("Unauthorized");
    }

    /// <summary>
    /// Updates a hoat dong ngoai khoa record
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <param name="noiDungMinhChung">Evidence content</param>
    /// <param name="inputData">Updated data</param>
    /// <returns>Updated record</returns>
    [HttpPut("{id}")]
    public async Task<object> Put(long id, [FromBody] HdnkTheoHocKyModel inputData)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
        {
            var hdNK = await _hoatDongNgoaiKhoaService.ChangeData(id, inputData);

            var response = (hdNK as ObjectResult)?.Value;
            return response;
        }

        return Unauthorized("Unauthorized");
    }

    /// <summary>
    /// Deletes a hoat dong ngoai khoa record
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id}")]
    public async Task<object> Delete(long id)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
        {
            var hdNK = await _hoatDongNgoaiKhoaService.Delete(id);

            var response = (hdNK as ObjectResult)?.Value;
            return response;
        }

        return Unauthorized("Unauthorized");
    }
}