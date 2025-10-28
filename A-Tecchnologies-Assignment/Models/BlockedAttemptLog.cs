namespace A_Tecchnologies_Assignment.Models;

public class BlockedAttemptLog
{
    public string IpAddress { get; set; }
    public string CountryCode { get; set; }
    public bool IsBlocked { get; set; }
    public string UserAgent { get; set; }
    public DateTime Timestamp { get; set; }
}