using GlowGirlsBackend.Interfaces;
using GlowGirlsBackend.Models;
using GlowGirlsBackend.Models.Settings;
using GlowGirlsBackend.Utility;
using Hangfire;
using Mediator;
using Microsoft.Extensions.Options;

namespace GlowGirlsBackend.Features.ContactUs;

public class ContactUsCommandHandler(
    IBackgroundJobClient backgroundJobClient,
    IOptions<GmailSettings> gmailSettings
) : ICommandHandler<ContactUsCommand, Result>
{
    private const string ParlourName = "Glow Girls Parlour";

    public Task<Result> Handle(ContactUsCommand request, CancellationToken cancellationToken)
    {
        var firstJobId = backgroundJobClient.Enqueue<IEMailService>(x =>
            x.SendAsync(
                new EMailModel(
                    request.EmailDto.SenderName,
                    request.EmailDto.SenderEmail,
                    request.EmailDto.Message,
                    ParlourName,
                    gmailSettings.Value.Email,
                    $"Contact request from {request.EmailDto.SenderName}",
                    gmailSettings.Value.CcName,
                    gmailSettings.Value.CcEmail
                ),
                cancellationToken
            )
        );

        var message = GetMessage(request.EmailDto.SenderName);

        backgroundJobClient.ContinueJobWith<IEMailService>(
            firstJobId,
            x =>
                x.SendAsync(
                    new EMailModel(
                        ParlourName,
                        gmailSettings.Value.Email,
                        message,
                        request.EmailDto.SenderName,
                        request.EmailDto.SenderEmail,
                        $"Reply from {ParlourName}",
                        null,
                        null
                    ),
                    cancellationToken
                )
        );

        return Task.FromResult(Result.Success());
    }

    private string GetMessage(string customerName)
    {
        return string.Join(
            Environment.NewLine,
            new[]
            {
                $"Hello {customerName},",
                "",
                "Thank you for contacting Glow Girls Parlour!",
                "",
                "We’ve received your message and our team will get back to you as soon as possible.",
                "",
                "Warm regards,",
                "Glow Girls Parlour",
            }
        );
    }
}

public record ContactUsCommand(EmailDto EmailDto) : ICommand<Result>;
