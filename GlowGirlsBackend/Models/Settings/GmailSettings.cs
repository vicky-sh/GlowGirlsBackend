namespace GlowGirlsBackend.Models.Settings;

public class GmailSettings
{
    public required string Email { get; init; }
    public required string AppPassword { get; init; }
    public required string SmtpHost { get; init; }
    public required int SmtpPort { get; init; }
    public required string CcName { get; init; }
    public required string CcEmail { get; init; }
}
