using Azure.Messaging;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using Project.Dtos.Account;
using Project.Models.Account;
using System.Security.Cryptography;


namespace Project.Repository.Account
{
    public class LoginRepository
    {
        private readonly ApplicationDBContext _context;


        public LoginRepository(ApplicationDBContext context)
        {
            _context = context;


        }



        public async Task<User?> LoginAsync(LoginDto loginDto)
        {
            
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Password == loginDto.Password);
        }
    }





}
