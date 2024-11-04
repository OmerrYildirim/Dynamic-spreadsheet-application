using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Dtos.Account;
using Project.Mappers;
using Project.Repository.Account;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/RegisterController")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly RegisterRepository _registerRepository;
       
        private readonly EmailRepository _emailRepository;
        public RegisterController(ApplicationDBContext context, RegisterRepository registerRepository, EmailRepository emailRepository)
        {
            _context = context;
            _registerRepository = registerRepository;
            _emailRepository = emailRepository;
        }


        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email) != null)
            {
                ModelState.AddModelError("Email", "There is already an address registered to this email.");
                return BadRequest(ModelState);
            }

            if (await _context.Users.FirstOrDefaultAsync(u => u.Username == registerDto.Username) != null)
            {
                ModelState.AddModelError("Username", "Username is taken");
                return BadRequest(ModelState);
            }



            var user = registerDto.ToUserFromRegisterDto();
            await _registerRepository.RegisterAsync(user);
            await SendEmail(email: registerDto.Email);

            return Ok(user);


        }
        [HttpPut("SendEmail")]
        public async Task<IActionResult> SendEmail([FromQuery] string email)
        {

            var receiver = email;
            var subject = "Email Verification";
            var message = "Please verify your email address by clicking the link below:";

           
                await _emailRepository.SendEmailAsync(receiver, subject, message);

                return Ok(new { Message = "Registration successful! Please check your email to verify your account.", });
            
           
        }
        [HttpGet("Verify")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            
                await _emailRepository.VerifyEmailAsync(token);
                return Ok(new { Message = "Email verification successful!" });
            
           
        }
    }
}
