using Project.Dtos.Account;
using Project.Models.Account;
using Project.Services;
using System.Runtime.CompilerServices;

namespace Project.Mappers
{
    public static class AccountMapper
    {
  
        public static User ToUserFromRegisterDto(this RegisterDto registerDto)
        {
            return new User
            {
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                Email = registerDto.Email,
                Username = registerDto.Username,
                Password = registerDto.Password
            };
        }
        
     
    }
}
