namespace GlowGirlsData.Models;

public class Appointment : BaseEntity
{
    public required string Attendee { get; set; }
    public required string AttendeeEmail { get; set; }
    public string? AttendeePhone { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}