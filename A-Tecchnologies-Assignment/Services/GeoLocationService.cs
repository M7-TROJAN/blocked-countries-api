using A_Tecchnologies_Assignment.Models;
using A_Tecchnologies_Assignment.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace A_Tecchnologies_Assignment.Services;

public class GeoLocationService : IGeoLocationService
{
    private readonly HttpClient _httpClient;
    private readonly GeoLocationSettings _settings;
    private readonly ILogger<GeoLocationService> _logger;

    public GeoLocationService(HttpClient httpClient, IOptions<GeoLocationSettings> options, ILogger<GeoLocationService> logger)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _logger = logger;
    }
    public async Task<GeoLocationResult?> GetGeoInfoAsync(string ipAddress)
    {
        if (string.IsNullOrEmpty(_settings.BaseUrl) || string.IsNullOrEmpty(_settings.ApiKey))
            throw new InvalidOperationException("GeoLocation API settings are not configured.");

        try
        {
            // Check if we are in a local environment (development)
            bool isLocal = string.IsNullOrWhiteSpace(ipAddress)
                           || ipAddress == "::1"
                           || ipAddress.StartsWith("127.");

            string endpoint = isLocal ? "check" : ipAddress;
            var url = $"{_settings.BaseUrl}/{endpoint}?access_key={_settings.ApiKey}&output=json";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"[GeoAPI Error] {response.StatusCode}: {content}");
                return null;
            }

            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            var countryName = root.TryGetProperty("country_name", out var cn) ? cn.GetString() : "Unknown";
            var countryCode = root.TryGetProperty("country_code", out var cc) ? cc.GetString() : "??";
            var ip = root.TryGetProperty("ip", out var ipVal) ? ipVal.GetString() : ipAddress;

            return new GeoLocationResult
            {
                Ip = ip ?? "N/A",
                CountryName = countryName ?? "Unknown",
                CountryCode = countryCode ?? "??"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"[GeoAPI Exception] {ex.Message}");
            return null;
        }
    }
}