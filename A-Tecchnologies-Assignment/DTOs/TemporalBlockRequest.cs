namespace A_Tecchnologies_Assignment.DTOs;

public class TemporalBlockRequest
{
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public int DurationMinutes { get; set; } // 1–1440
}