using GlowGirlsBackend.Models;

namespace GlowGirlsBackend.Interfaces;

public interface IEMailService
{
    public Task SendAsync(EMailModel eMailModel, CancellationToken cancellationToken);
}
