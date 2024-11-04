using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Project.Dtos.Account;
using Project.Dtos.Table;
using Project.Mappers;
using Project.Models.Account;
using Project.Models.Table;
using Project.Repository.TableRepository;
using Project.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.Controllers
{
    
    [ApiController]
    [Route("api/Tables")]
    public class TableController : ControllerBase
    {
        private readonly TableRepository _tableRepository;
        private readonly ApplicationDBContext _context;
        private readonly TokenService _tokenService;
 

        public TableController(TableRepository tableRepository, ApplicationDBContext context,TokenService tokenService )
        {
            _tableRepository = tableRepository;
            _context = context;
            _tokenService = tokenService;
        }


        private int GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"UserId from token: {userId}");
            return int.TryParse(userId, out var id) ? id : 0;
        }


        [Authorize]
        [HttpGet("GetUserTablesNames")]
        public async Task <IActionResult> GetUserTablesName()
        {
            var userId = GetUserId();
            Console.WriteLine($"UserId: {userId}");
            var userTablesName = await _tableRepository.GetUserTableNamesAsync(userId);
            return Ok(userTablesName);
        }

        [Authorize]
        [HttpGet("GetUserTables")]
        public async Task<IActionResult> GetTables(string tableName)
        {
            var userId = GetUserId();
            Console.WriteLine($"UserId: {userId}");
            var userTables = await _tableRepository.GetUserTablesAsync(userId,tableName);
            return Ok(userTables);
            
        }


        [Authorize]
        [HttpPost("CreateUserTable")]
        public async Task<IActionResult> CreateUserTable()
        {
            var userId = GetUserId();


            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                Console.WriteLine($"Request Body: {body}");

     
                var userTableDto = JsonConvert.DeserializeObject<UserTableDto>(body);
                Console.WriteLine($"Deserialized DTO: {JsonConvert.SerializeObject(userTableDto)}");

              
                userTableDto.UserId = userId;
                Console.WriteLine($"UserId assigned to DTO: {userTableDto.UserId}");

     
                await _tableRepository.AddUserTableAsync(userTableDto);

              
                return Ok(userTableDto);
            }
        }

        [Authorize]
        [HttpPost("AddRow")]
        public async Task<IActionResult> AddRow()
        {
            var userId = GetUserId();
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                Console.WriteLine($"Request Body: {body}");

                var rowDto = JsonConvert.DeserializeObject<TableRowDto>(body);
                Console.WriteLine($"Deserialized DTO: {JsonConvert.SerializeObject(rowDto)}");

                await _tableRepository.AddRowAsync(rowDto);

                return Ok(rowDto);
            }
        }

        [Authorize]
        [HttpDelete("DeleteRow")]
        public async Task<IActionResult> DeleteRow( TableRowDto tableRowDto)
        {
            var result = await _tableRepository.DeleteRowAsync(tableRowDto.UserTableId, tableRowDto.Data);
            if (result)
            {
                return Ok();
            }
            return BadRequest();

        }
        [Authorize]
        [HttpPut("UpdateRow")]
        public async Task<IActionResult> UpdateRow(TableRowDto tableRowDto)
        {
            if (tableRowDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _tableRepository.UpdateRowAsync(tableRowDto.UserTableId, tableRowDto.Data,tableRowDto.Id);

            if (!result)
            {
                return NotFound("Row or table not found.");
            }

            return Ok("Row updated successfully.");
        }

        [Authorize]
        [HttpGet("GetUserName")]
        public async Task<IActionResult> GetUserName()
        {
            var userId = GetUserId();
            var user = await _tableRepository.GetUserNameAsync(userId);
            return Ok(user.Username);
        }
  


    }
}
