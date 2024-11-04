using System.Text.Json.Serialization;

namespace Project.Models.Table
{
    public class TableColumn
    {
        public int Id { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public int UserTableId { get; set; }
        [JsonIgnore]
        public UserTable? UserTable { get; set; }
        
    }

}
