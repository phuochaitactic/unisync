using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Controllers;

/// <summary>
/// Handles CRUD operations for bac he data
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class KDMBacHeController : ControllerBase
{
    private readonly IBacHeService _bacHeService;
    private readonly IAuthService _auth;

    public KDMBacHeController(IBacHeService bacHeService, IAuthService auth)
    {
        this._bacHeService = bacHeService;
        this._auth = auth;
    }

    /// <summary>
    /// Exports bac he table column metadata to Excel
    /// </summary>
    /// <returns>Excel file</returns>
    [HttpGet("ToExcel/")]
    public IActionResult GetToExcel()
    {
        try
        {
            // Get column metadata
            var properties = typeof(Kdmbh).GetProperties();
            var columnMetadata = new List<Dictionary<string, string>>();
            foreach (var property in properties)
            {
                if (property.Name != "Idbh")
                {
                    // Add column metadata
                    var column = new Dictionary<string, string>();
                    column.Add("ColumnName", property.Name);
                    column.Add("DataType", property.PropertyType.Name);
                    columnMetadata.Add(column);
                }
            }

            // Export to Excel
            return ExcelExporter.ExportToExcel(columnMetadata);
        }
        catch (Exception ex)
        {
            return BadRequest($"ERROR: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all bac he records
    /// </summary>
    /// <returns>List of bac he</returns>
    [HttpGet]
    public async Task<object> Get()
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
        {
            var bache = await _bacHeService.GetAll();
            var response = (bache as ObjectResult)?.Value;

            return response;
        }

        return Unauthorized("User is not authenticated.");
    }

    /// <summary>
    /// Gets bac he record by ID
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <returns>Bac he details</returns>
    [HttpGet("{id}")]
    public async Task<object> GetById(long id)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
        {
            var bache = await _bacHeService.GetById(id);

            var response = (bache as ObjectResult)?.Value;

            return response;
        }

        return Unauthorized("User is not authenticated.");
    }

    /// <summary>
    /// Search bac he records by name
    /// </summary>
    /// <param name="TenBH">Bac he name</param>
    /// <returns>Matching records</returns>
    [HttpGet("TenBacHe/{TenBH}")]
    public async Task<object> GetByTenBacHe(string TenBH)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
        {
            var bache = await _bacHeService.GetByTenBacHe(TenBH);

            var response = (bache as ObjectResult)?.Value;

            return response;
        }

        return Unauthorized("User is not authenticated.");
    }

    /// <summary>
    /// Creates a new bac he record
    /// </summary>
    /// <param name="inputData">Bac he data</param>
    /// <returns>Newly created record</returns>
    [HttpPost]
    public async Task<object> Post([FromBody] Kdmbh inputData)
    {
        if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
        {
            var bache = await _bacHeService.CreateBacHe(inputData);

            var response = (bache as ObjectResult)?.Value;

            return response;
        }

        return Unauthorized("User is not authenticated.");
    }

    /// <summary>
    /// Updates a bac he record
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <param name="inputData">Updated data</param>
    /// <returns>Updated record</returns>
    [HttpPut("{id}")]
    public async Task<object> Put(long id, [FromBody] Kdmbh inputData)
    {
        if (_auth.ValidateAdmin(this))
        {
            var bache = await _bacHeService.ChangeData(id, inputData);

            var response = (bache as ObjectResult)?.Value;

            return response;
        }

        return Unauthorized("User is not authenticated.");
    }

    /// <summary>
    /// Deletes a bac he record
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <returns>Deletion response</returns>
    [HttpDelete("{id}")]
    public async Task<object> Delete(long id)
    {
        if (_auth.ValidateAdmin(this))
        {
            var bache = await _bacHeService.Delete(id);

            var response = (bache as ObjectResult)?.Value;
            return response;
        }

        return Unauthorized("User is not authenticated.");
    }
}