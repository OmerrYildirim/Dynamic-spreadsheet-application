using Project.Models.Account;
using System.Text.Json.Serialization;

namespace Project.Models.Table
{
    public class UserTable
    {
        public int Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public List<TableColumn> Columns { get; set;} = new List<TableColumn>();
        public List<TableRow> Rows { get; set; } = new List<TableRow>();
        [JsonIgnore]
        public User? User { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

    }
}
