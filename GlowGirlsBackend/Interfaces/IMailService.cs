namespace GlowGirlsBackend.Interfaces;

public interface IMailService
{
    Task SendAppointmentConfirmationEmailAsync();
}