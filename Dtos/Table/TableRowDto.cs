using Project.Models.Table;
using System.Text.Json.Serialization;

namespace Project.Dtos.Table
{
    public class TableRowDto
    {
        public int Id { get; set; }
        public List<string> Data { get; set; }=new List<string>();
        public int UserTableId { get; set; }

    }
}
