using System.ComponentModel.DataAnnotations;

namespace A_Tecchnologies_Assignment.Settings;

public class GeoLocationSettings
{
    public const string SectionName = "GeoLocationSettings";

    [Required(ErrorMessage = "The {0} field is required. Please make sure the base URL for the GeoLocation API is set in appsettings.json")]
    public string BaseUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "The {0} field is required. Please make sure the GeoLocation API key is set in appsettings.json")]
    public string ApiKey { get; set; } = string.Empty;
}
