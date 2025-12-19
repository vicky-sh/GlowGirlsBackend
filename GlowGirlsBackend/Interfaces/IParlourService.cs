namespace GlowGirlsBackend.Interfaces;

public interface IParlourService
{
    public string ServiceName { get; set; }
    public TimeSpan TimeRequired { get; set; }
}