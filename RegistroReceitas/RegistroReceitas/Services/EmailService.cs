using System.Net.Mail;

namespace RegistroReceitas.Services;

public class EmailService(IConfiguration config) : IEmailService
{
    private readonly IConfiguration _config = config;

    public async Task SendAsync(string to, string subject, string body)
    {
        var user = _config["Email:User"]!;

        var smtp = new SmtpClient(_config["Email:Smtp"], int.Parse(_config["Email:Port"]!))
        {
            Credentials = new System.Net.NetworkCredential(user, _config["Email:Password"]),
            EnableSsl = true,
        };

        var mail = new MailMessage(user, to, subject, body);

        await smtp.SendMailAsync(mail);
    }
}
