using System;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Resend;

namespace Infrastructure.Email;

public class EmailSender(IConfiguration config) : IEmailSender<User>
{
    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        var subject = "Confirm your email address";
        var body = $@"
            <p> HI {user.DisplayName} </p>
            <p> Please confirm your email by clicking the link below</p>
            <p> <a href='{confirmationLink}'>
            Click here to confirm email</a>
            </p>
            <p> thanks!</p>
        ";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        var subject = "Reset your password";
        var body = $@"
            <p> HI {user.DisplayName} </p>
            <p> Please click this link to reset your password</p>
            <p> <a href='{config["ClientAppUrl"]}/reset-password?email={email}&code={resetCode}'>
            Click here to reset your password</a>
            </p>
            <p> If you did not request this, please, ignore this email</p>
        ";

        await SendEmailAsync(email, subject, body);
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
        Console.WriteLine(message.HtmlBody);

        //await resend.EmailSendAsync(message);
        await Task.CompletedTask;
    }
}
