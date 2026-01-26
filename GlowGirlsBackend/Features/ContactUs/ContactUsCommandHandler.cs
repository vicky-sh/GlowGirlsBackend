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
                new ContactModel(
                    request.ContactDto.SenderName,
                    request.ContactDto.SenderEmail,
                    request.ContactDto.Message
                        + Environment.NewLine
                        + Environment.NewLine
                        + $"Mobile Number: {request.ContactDto.PhoneNumber}",
                    ParlourName,
                    gmailSettings.Value.Email,
                    $"Contact request from {request.ContactDto.SenderName}",
                    gmailSettings.Value.CcName,
                    gmailSettings.Value.CcEmail
                ),
                cancellationToken
            )
        );

        var message = GetMessage(request.ContactDto.SenderName);

        backgroundJobClient.ContinueJobWith<IEMailService>(
            firstJobId,
            x =>
                x.SendAsync(
                    new ContactModel(
                        ParlourName,
                        gmailSettings.Value.Email,
                        message,
                        request.ContactDto.SenderName,
                        request.ContactDto.SenderEmail,
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

public record ContactUsCommand(ContactDto ContactDto) : ICommand<Result>;
