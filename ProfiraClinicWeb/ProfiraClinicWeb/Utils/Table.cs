using System.Reflection;
using System.Runtime.InteropServices.Marshalling;

namespace ProfiraClinicWeb.Utils
{
    public enum ColumnType
    {
        Data,
        Edit,
        View,
    }

    public class TableColumn
    {
        public string Caption { get; set; }
        public string DataField { get; set; }
        public string Alignment { get; set; }
        public ColumnType? Type { get; set; } = ColumnType.Data;
        public string? CustomIcon { get; set; }
        public MudBlazor.Color? Color { get; set; }
    }

    public class TableConfig(List<TableColumn> column, List<object> dataSource)
    {
        private readonly List<TableColumn> _column = column;
        private readonly List<object> _dataSource = dataSource;

        public List<Dictionary<string, string>> GetData()
        {
            /*List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            foreach (object d in _dataSource) { 
                data.Add(ToDict(d));
            }
            return data;*/
            return _dataSource.ConvertAll(x => ToDict(x));
        }
 
        public List<TableColumn> GetColumn()
        {
            return _column;
        }

        private static Dictionary<string, string> ToDict(object dataSource)
        {
            return dataSource.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
             .ToDictionary(prop => prop.Name, prop => prop.GetValue(dataSource, null)?.ToString() ?? "");
        }
    }
}
