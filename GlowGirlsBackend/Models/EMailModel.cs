namespace GlowGirlsBackend.Models;

public class EMailModel(
    string senderName,
    string senderEmail,
    string message,
    string recipientName,
    string recipientEmail,
    string subject,
    string? ccName,
    string? ccEmail
) : EmailDto(senderName, senderEmail, message)
{
    public string RecipientName { get; set; } = recipientName;
    public string RecipientEmail { get; set; } = recipientEmail;
    public string Subject { get; set; } = subject;
    public string? CcName { get; set; } = ccName;
    public string? CcEmail { get; set; } = ccEmail;
}
