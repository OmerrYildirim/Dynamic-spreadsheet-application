using System.Text.Json.Serialization;

namespace Project.Models.Table
{
    public class TableRow
    {
        public int Id { get; set; }
        public int UserTableId { get; set; }
        [JsonIgnore]
        public UserTable? UserTable { get; set; }
        public List<string> Data { get; set; } = new List<string>();
    }
}
