using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using static Project.Repository.Account.RegisterRepository;
using System.Security.Cryptography;
using Project.Models.Account;
namespace Project.Repository.Account
{
    public class RegisterRepository
    {

        private readonly ApplicationDBContext _context;


        public RegisterRepository(ApplicationDBContext context)
        {
            _context = context;


        }


        public async Task<User> RegisterAsync(User user)
        {

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            user.HashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));


            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;


        }
    }
}
