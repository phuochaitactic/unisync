using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Services
{
    public class ExcelExporter
    {
        public static IActionResult ExportToExcel(List<Dictionary<string, string>> columns)
        {
            var workbook = CreateWorkbook(columns);
            byte[] content = SaveWorkbookToMemory(workbook);

            return CreateExcelFileResult(content);
        }

        private static XLWorkbook CreateWorkbook(List<Dictionary<string, string>> columns)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Columns");

            AddDataTypesToWorksheet(columns, worksheet, workbook);
            AddColumnNamesToWorksheet(columns, worksheet);

            return workbook;
        }

        private static void AddDataTypesToWorksheet(List<Dictionary<string, string>> columns, IXLWorksheet worksheet, XLWorkbook workbook)
        {
            var dataTypeNames = new Dictionary<string, string>()
            {
                { "String", "Chuỗi" },
                { "Int64", "Số" },
                { "Boolean", "Boolean" },
                { "DateTime", "Ngày tháng" }
            };

            for (int i = 0; i < columns.Count; i++)
            {
                string dtName = dataTypeNames[columns[i]["DataType"]];
                worksheet.Cell(1, i + 1).Value = dtName;
            }
        }

        private static void AddColumnNamesToWorksheet(List<Dictionary<string, string>> columns, IXLWorksheet worksheet)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                worksheet.Cell(2, i + 1).Value = columns[i]["ColumnName"];
            }
        }

        private static byte[] SaveWorkbookToMemory(XLWorkbook workbook)
        {
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }

        private static IActionResult CreateExcelFileResult(byte[] content)
        {
            var fileResult = new FileContentResult(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fileResult.FileDownloadName = $"Columns_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            return fileResult;
        }
    }
}