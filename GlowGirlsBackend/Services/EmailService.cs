using GlowGirlsBackend.Interfaces;
using GlowGirlsBackend.Models;
using GlowGirlsBackend.Models.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GlowGirlsBackend.Services;

public class EmailService(IOptions<GmailSettings> gmailSettings) : IEMailService
{
    private readonly GmailSettings _gmailSettings = gmailSettings.Value;

    public async Task SendAsync(EMailModel eMailModel, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(
            _gmailSettings.SmtpHost,
            _gmailSettings.SmtpPort,
            SecureSocketOptions.StartTls,
            cancellationToken
        );

        await client.AuthenticateAsync(
            _gmailSettings.Email,
            _gmailSettings.AppPassword,
            cancellationToken
        );

        var mailMessage = new MimeMessage
        {
            From = { new MailboxAddress(eMailModel.SenderName, eMailModel.SenderEmail) },
            To = { new MailboxAddress(eMailModel.RecipientName, eMailModel.RecipientEmail) },
            Subject = eMailModel.Subject,
            Body = new TextPart("plain") { Text = eMailModel.Message },
        };

        if (eMailModel is { CcName: not null, CcEmail: not null })
        {
            mailMessage.Cc.Add(new MailboxAddress(eMailModel.CcName, eMailModel.CcEmail));
        }

        await client.SendAsync(mailMessage, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}
