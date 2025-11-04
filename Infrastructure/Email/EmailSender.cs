using System;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Resend;

namespace Infrastructure.Email;

public class EmailSender(IResend resend) : IEmailSender<User>
{
    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        var subject = "Confirm your email address";
        var body = $@"
            <p> HI {user.DisplayName} </p>
            <p> Please confirm your email by clicking the link below</p>
            <p> <a href='{confirmationLink}'>Click here to confirm email</p>
            <p> thanks!</p>
        ";

        await SendEmailAsync(email, subject, body);
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }

    private async Task SendEmailAsync(string email, string subject, string body)
    {
        var message = new EmailMessage
        {
            From = "test@resend.dev",
            Subject = subject,
            HtmlBody = body
        };
        message.To.Add(email);
        //Console.WriteLine(message.HtmlBody);

        await resend.EmailSendAsync(message);
        //await Task.CompletedTask;
    }
}
