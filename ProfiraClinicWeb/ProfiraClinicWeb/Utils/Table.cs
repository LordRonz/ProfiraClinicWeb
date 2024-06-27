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

    public class TableFilter
    {
        public string Column { get; set; }
        public string FieldName { get; set; }
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

    public class TableConfig(List<TableColumn> column, List<object> dataSource, string? addRoute = "", TableFilter? filter = null)
    {
        private readonly List<TableColumn> _column = column;
        private readonly List<object> _dataSource = dataSource;
        private readonly string _addRoute = addRoute;
        private readonly TableFilter? _filter = filter;

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

        public string GetAddRoute()
        {
            return _addRoute;
        }

        public TableFilter GetFilter()
        {
            return _filter;
        }

        private static Dictionary<string, string> ToDict(object dataSource)
        {
            return dataSource.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
             .ToDictionary(prop => prop.Name, prop => prop.GetValue(dataSource, null)?.ToString() ?? "");
        }
    }
}
