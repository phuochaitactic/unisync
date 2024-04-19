using BuildCongRenLuyen.Models.CustomModels;

namespace BuildCongRenLuyen.Services
{
    public class ColumnMetadataService
    {
        // ColumnMetadataService.cs

        public static List<Dictionary<string, string>> GetColumnMetadata()
        {
            var properties = typeof(SinhVienTableModel).GetProperties();
            var columnMetadata = new List<Dictionary<string, string>>();

            foreach (var property in properties)
            {
                //if (property.Name != "IdsinhVien")
                //{
                var column = new Dictionary<string, string>();
                column.Add("Name", property.Name);
                column.Add("DataType", property.PropertyType.Name);

                columnMetadata.Add(column);
                //}
            }

            return columnMetadata;
        }
    }
}