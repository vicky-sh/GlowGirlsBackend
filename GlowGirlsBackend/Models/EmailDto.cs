namespace GlowGirlsBackend.Models;

public class EmailDto(string senderName, string senderEmail, string message)
{
    public string SenderName { get; set; } = senderName;
    public string SenderEmail { get; set; } = senderEmail;
    public string Message { get; init; } = message;
}
