using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.Dtos.Table;
using Project.Models.Account;
using Project.Models.Table;
using System.Security.Claims;

namespace Project.Repository.TableRepository
{
    public class TableRepository
    {
        private readonly ApplicationDBContext _context;

        public TableRepository(ApplicationDBContext context)
        {
            _context = context;
        }




        public async Task<List<string>> GetUserTableNamesAsync(int userId)
        {
            return await _context.UserTables
                .Where(t => t.UserId == userId)
                .Select(t => t.TableName) 
                .ToListAsync();
        }
        public async Task<List<UserTable>> GetUserTablesAsync(int userId,string tableName)
        {
            return await _context.UserTables
                .Include(t => t.Columns)
                .Include(t => t.Rows)
                .Where(t => t.UserId == userId && t.TableName == tableName)
                .ToListAsync();
        }


        public async Task<UserTable> AddUserTableAsync(UserTableDto userTableDto)
        {
            var userTable = new UserTable
            {
             
                TableName = userTableDto.TableName,
                Columns = userTableDto.ColumnsDto?.Select(c => new TableColumn
                {
                    ColumnName = c.ColumnName,
                    DataType = c.DataType
                }).ToList() ?? new List<TableColumn>(),
                Rows = userTableDto.RowsDto?.Select(r => new TableRow
                {
                    Data = r.Data
                }).ToList() ?? new List<TableRow>(),
                UserId = userTableDto.UserId
            };

            await _context.UserTables.AddAsync(userTable);
            await _context.SaveChangesAsync();

            return userTable;
        }

        public async Task<UserTable> AddRowAsync(TableRowDto tableRowDto)
        {
            var userTable = await _context.UserTables
                .Include(t => t.Rows)
                 .FirstOrDefaultAsync(t => t.Id == tableRowDto.UserTableId);


            if (userTable == null)
            {
                return null;
            }

            userTable.Rows.Add(new TableRow
            {
                Data = tableRowDto.Data
            });

            await _context.SaveChangesAsync();

            return userTable;
        }
        public async Task<bool> DeleteRowAsync(int userTableId, List<string> rowData)
        {
         
            var table = await _context.UserTables
                .Include(t => t.Rows)
                .FirstOrDefaultAsync(t => t.Id == userTableId);

         

            if (table == null) return false;

          
            var rowToDelete = table.Rows
                .FirstOrDefault(row => row.Data.SequenceEqual(rowData));

            

            if (rowToDelete == null) return false;

           
            table.Rows.Remove(rowToDelete);

        
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateRowAsync(int userTableId, List<string> rowData,int id)
        {
            var userTable = await _context.UserTables
                .Include(ut => ut.Rows)
                .FirstOrDefaultAsync(ut => ut.Id == userTableId);

        

            if (userTable == null)
            {
                return false; 
            }

           var rowToUpdate = await _context.TableRows
                .FirstOrDefaultAsync(row => row.Id == id);

            Console.WriteLine(id.GetType);
            Console.WriteLine(rowToUpdate.Data);

            if (rowToUpdate == null)
            {
                return false; 
            }

            
            rowToUpdate.Data = rowData;
            _context.TableRows.Update(rowToUpdate);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User> GetUserNameAsync(int userId)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null)
            {
                return null;
            }
            Console.WriteLine($"User: {user.Username}");
            return user;
            
        }



    }
}
