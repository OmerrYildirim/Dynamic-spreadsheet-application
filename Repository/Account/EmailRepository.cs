

using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

public class EmailRepository 
{
    private readonly ApplicationDBContext _context;

    public EmailRepository(ApplicationDBContext context)
    {
        _context = context;
        
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var mail = "42yildir54@gmail.com";
        var pw = "tyms rqnd jqeb patr";

        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(mail, pw)
        };

        var token = Guid.NewGuid().ToString();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user != null)
        {
            user.VerificationToken = token;
            await _context.SaveChangesAsync();
        }

        var baseUrl = "https://localhost:7107/api/RegisterController/Verify";
        var verificationLink = $"{baseUrl}?token={Uri.EscapeDataString(token)}";

        var verificationMessage = $"{message}<br/><br/>Please verify your email by clicking the link below:<br/><a href=\"{verificationLink}\">Verify Email</a>";

        var mailMessage = new MailMessage
        {
            From = new MailAddress(mail),
            Subject = subject,
            Body = verificationMessage,
            IsBodyHtml = true
        };

        mailMessage.To.Add(email);
        await client.SendMailAsync(mailMessage);

    }

    public async Task VerifyEmailAsync (string token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
        if (user != null && !user.EmailConfirmed)
        {
            user.EmailConfirmed = true;
            await _context.SaveChangesAsync();
            Console.WriteLine($"Email confirmed for user {user.Email}");
        }
        else { 
            throw new Exception("Invalid email verification token");
        }

    }
}
