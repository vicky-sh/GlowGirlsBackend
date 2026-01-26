namespace GlowGirlsBackend.Models;

public class ContactDto(
    string senderName,
    string senderEmail,
    string message,
    string phoneNumber = ""
)
{
    public string SenderName { get; set; } = senderName;
    public string SenderEmail { get; set; } = senderEmail;
    public string Message { get; init; } = message;
    public string PhoneNumber { get; init; } = phoneNumber;
}
