namespace A_Tecchnologies_Assignment.Models;

public class BlockedCountry
{
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public DateTime? ExpirationTime { get; set; } // null = permanent block
}