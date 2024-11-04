using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Dtos.Account;
using Project.Mappers;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;
using Project.Models.Account;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Project.Services;
using Project.Repository.Account;


namespace Project.Controllers
{
    [Route("api/LoginController")]
    [ApiController]

    public class LoginController : ControllerBase
    {
       
        private readonly LoginRepository _loginRepository;
        private readonly TokenService _tokenService;
        
        public LoginController( LoginRepository loginRepository, TokenService tokenService)
        {
            
            _loginRepository = loginRepository;
            _tokenService = tokenService;
           
        }


      
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _loginRepository.LoginAsync(loginDto);
            if (user == null)
            {
                return NotFound();
            }
            
            if(!user.EmailConfirmed)
            {
                return Unauthorized(new { Message = "Please verify your email address before logging in." });
            }

            

            return Ok(
             new LoginDto
             {
                 Username = loginDto.Username,
                 Password = loginDto.Password,
                 JwtToken = _tokenService.CreateToken(user)
             }
         );
        }


        
    

      



    }
}
