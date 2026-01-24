// ReSharper disable InconsistentNaming

namespace GlowGirlsBackend.Models.Settings;

public class GoogleServiceAccountDetails
{
    public required string Type { get; init; }
    public required string Project_Id { get; init; }
    public required string Private_Key_Id { get; init; }
    public required string Private_Key { get; init; }
    public required string Client_Email { get; init; }
    public required string Client_Id { get; init; }
    public required string Auth_Uri { get; init; }
    public required string Token_Uri { get; init; }
    public required string Auth_Provider_X509_Cert_Url { get; init; }
    public required string Client_X509_Cert_Url { get; init; }
    public required string Universe_Domain { get; init; }
}
