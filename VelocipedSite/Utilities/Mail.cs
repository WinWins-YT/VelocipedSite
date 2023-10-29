using MailKit.Net.Smtp;
using MimeKit;

namespace VelocipedSite.Utilities;

public class Mail
{
    private static async Task SendMessage(string email, string subject, string text)
    {
        using var emailMessage = new MimeMessage();
 
        emailMessage.From.Add(new MailboxAddress("Велосипед", "info@velociped.ru"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = text
        };

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.mail.ru", 465, true);
        await client.AuthenticateAsync("info@velociped.ru", "-");
        await client.SendAsync(emailMessage);
 
        await client.DisconnectAsync(true);
    }

    public static async Task SendActivationEmail(string email, string token)
    {
        var mailBody = $"""
                        <h3>Добро пожаловать в Велосипед!</h3>
                        <p>Вы успешно зарегистрировались на нашем сайте. Чтобы закончить регистрацию, необходимо подтвердить адрес электронной почты, перейдя по этой <a href="https://localhost:44468/api/v1/profile/activation/{token}">ссылке</a></p>
                        <p>Хорошего вам дня!</p>
                        """;

        await SendMessage(email, "Активация учетной записи", mailBody);
    }
}