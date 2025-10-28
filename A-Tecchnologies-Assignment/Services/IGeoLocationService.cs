using A_Tecchnologies_Assignment.Models;

namespace A_Tecchnologies_Assignment.Services;

public interface IGeoLocationService
{
    Task<GeoLocationResult?> GetGeoInfoAsync(string ipAddress);
}