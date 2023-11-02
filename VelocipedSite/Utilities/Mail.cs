using MailKit.Net.Smtp;
using MimeKit;
using VelocipedSite.Models;

namespace VelocipedSite.Utilities;

public static class Mail
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
        await client.AuthenticateAsync("info@velociped.ru", "MyLcTW2VheK8SL3ydGwe");
        await client.SendAsync(emailMessage);
 
        await client.DisconnectAsync(true);
    }

    public static async Task SendActivationEmail(string email, string token)
    {
        var mailBody = $"""
                        <h3>Добро пожаловать в Велосипед!</h3>
                        <p>Вы успешно зарегистрировались на нашем сайте. Чтобы закончить регистрацию, необходимо подтвердить адрес электронной почты, перейдя по этой <a href="https://velociped.ru/api/v1/profile/activation/{token}">ссылке</a></p>
                        <p>Хорошего вам дня!</p>
                        """;

        await SendMessage(email, "Активация учетной записи", mailBody);
    }

    public static async Task SendForgotPasswordEmail(string email, string newPassword)
    {
        var mailBody = $"""
                        <h3>Сброс пароля</h3>
                        <p>Здравствуйте, вы запросили сброс пароля для аккаунта на сайте Велосипед</p>
                        <p>Ваш новый пароль: <b>{newPassword}</b></p>
                        <p>Теперь в ваш аккаунт будет происходить с этим паролем. При необходимости, его можно поменять в настройках профиля</p>
                        """;

        await SendMessage(email, "Сброс пароля", mailBody);
    }

    public static async Task SendOrderCreatedEmail(string email, Order order)
    {
        var mailBody = $"""
                        <h3>Ваш заказ №{order.Id} от {order.Date:f}</h3>
                        <p>Скидка: {order.SaleValue:F2} руб.</p>
                        <b>Итого с учетом скидки: {order.TotalPrice:F2} руб.</b>
                        <p>Ваши товары:</p>
                       """;

        mailBody = order.Products.Select(x => $"<p>{x.Name} - {x.Price:F2} руб.</p>").Aggregate(mailBody, (current, line) => current + (line + "\n"));

        mailBody += "<p>Спасибо за заказ!</p>";

        await SendMessage(email, "Заказ создан", mailBody);
    }
}