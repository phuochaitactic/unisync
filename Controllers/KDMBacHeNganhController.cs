using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For CRUD operations on bac he nganh data

namespace BuildCongRenLuyen.Controllers;

/// <summary>
/// Handles CRUD operations for bac he nganh data
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class KDMBacHeNganhController : ControllerBase
{
    private readonly IBacHeNganhService _bacHeNganhService;
    private readonly IAuthService _auth;

    public KDMBacHeNganhController(IBacHeNganhService bacHeNganhService, IAuthService auth)
    {
        this._bacHeNganhService = bacHeNganhService;
        this._auth = auth;
    }

    /// <summary>
    /// Exports bac he nganh table metadata to Excel
    /// </summary>
    /// <returns>Excel file</returns>
    [HttpGet("ToExcel/")]
    public IActionResult GetToExcel()
    {
        try
        {
            // Get metadata
            var properties = typeof(BacHeNganhModel).GetProperties();

            var columnMetadata = new List<Dictionary<string, string>>();

            foreach (var property in properties)
            {
                // Add to metadata list
                if (property.Name != "IdbhngChng")
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
    /// Gets all bac he nganh records
    /// </summary>
    /// <returns>List of bac he nganh</returns>
    [HttpGet]
    public async Task<object> Get()
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
        {
            var bacHeNganh = await _bacHeNganhService.GetAll();
            var response = (bacHeNganh as ObjectResult)?.Value;
            return response;
        }
        return Unauthorized("Unauthorized");
    }

    /// <summary>
    /// Gets bac he nganh record by ID
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <returns>Bac he nganh details</returns>
    [HttpGet("{id}")]
    public async Task<object> Get(long id)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
        {
            var bacHeNganh = await _bacHeNganhService.GetById(id);
            var response = (bacHeNganh as ObjectResult)?.Value;
            return response;
        }
        return Unauthorized("Unauthorized");
    }

    /// <summary>
    /// Search bac he nganh records by code
    /// </summary>
    /// <param name="MaBhNgh">Bac he nganh code</param>
    /// <returns>Matching records</returns>
    [HttpGet("MaBacHeNganh/{MaBhNgh}")]
    public async Task<object> GetByMaBacHeNganh(string MaBhNgh)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
        {
            var bacHeNganh = await _bacHeNganhService.GetByMaBacHeNganh(MaBhNgh);
            var response = (bacHeNganh as ObjectResult)?.Value;
            return response;
        }

        return Unauthorized("Unauthorized");
    }

    /// <summary>
    /// Gets bac he nganh records by faculty and name
    /// </summary>
    /// <param name="idNhhk">Faculty ID</param>
    /// <param name="tenBhNgh">Bac he nganh name</param>
    /// <returns>Matching records</returns>
    [HttpGet("BacHeNganhTheoKhoa")]
    public async Task<object> GetBacHeNganhTheoKhoa(string TenKhoa)
    {
        if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
        {
            var bache = await _bacHeNganhService.GetBacHeNganhTheoKhoa(TenKhoa);
            var response = (bache as ObjectResult)?.Value;
            return response;
        }
        return Unauthorized("User is not authenticated.");
    }

    /// <summary>
    /// Creates new bac he nganh record
    /// </summary>
    /// <param name="inputData">Bac he nganh data</param>
    /// <param name="TenBacHe">Bac he name</param>
    /// <param name="TenNganh">Nganh name</param>
    /// <returns>New record</returns>
    [HttpPost]
    public async Task<object> Post([FromBody] BacHeNganhModel inputData)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
        {
            var bacHeNganh = await _bacHeNganhService.CreateBacHeNganh(inputData);
            var response = (bacHeNganh as ObjectResult)?.Value;
            return response;
        }
        return Unauthorized("Unauthorized");
    }

    /// <summary>
    /// Updates a bac he nganh record
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <param name="TenBacHe">Bac he name</param>
    /// <param name="TenNganh">Nganh name</param>
    /// <param name="inputData">Updated data</param>
    /// <returns>Updated record</returns>
    [HttpPut("{id}")]
    public async Task<object> Put(long id, [FromBody] BacHeNganhModel inputData)
    {
        if (_auth.ValidateAdmin(this))
        {
            var bacHeNganh = await _bacHeNganhService.ChangeData(id, inputData);
            var response = (bacHeNganh as ObjectResult)?.Value;
            return response;
        }
        return Unauthorized("Unauthorized");
    }

    /// <summary>
    /// Deletes a bac he nganh record
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id}")]
    public async Task<object> Delete(long id)
    {
        if (_auth.ValidateAdmin(this))
        {
            var bacHeNganh = await _bacHeNganhService.Delete(id);
            var response = (bacHeNganh as ObjectResult)?.Value;
            return response;
        }
        return Unauthorized("Unauthorized");
    }
}