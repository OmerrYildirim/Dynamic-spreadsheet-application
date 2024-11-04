using Project.Models.Table;

namespace Project.Dtos.Table
{
    public class UserTableDto
    {
        public int Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public List<TableColumnDto> ColumnsDto { get; set; }= new List<TableColumnDto>();
        public List<TableRowDto> RowsDto { get; set; } = new List<TableRowDto>();
        public int UserId { get; set; }
   

    }
}
