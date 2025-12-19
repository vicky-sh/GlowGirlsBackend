namespace GlowGirlsBackend.Models;

public class CreateCalendarEventDto
{
    public string? Title { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string? Location { get; set; }
    public required string Email { get; set; }
}