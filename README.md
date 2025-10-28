# Blocked Counteries

This project is a **.NET 8 Web API** that manages **country blocking**, validates **IP addresses** using a **GeoLocation provider**,  
and logs **blocked access attempts** — all using **in-memory storage** (no database).

---

The main goal of this project is to build an API that allows:
- Blocking specific countries permanently or temporarily.
- Automatically unblocking temporary blocks after their duration expires.
- Checking any incoming IP (or caller IP) against the blocked list.
- Logging blocked attempts with details like IP, country, timestamp, and user agent.
- Providing all API documentation through Swagger.

Everything runs **in-memory**, so no database is required.  
Data is stored using a thread-safe `ConcurrentDictionary`, and a background service handles cleanup for expired temporary blocks.

---

## Technologies Used

- **.NET 8 Web API**
- **C# 12**
- **Swagger**
- **FluentValidation** (with AutoValidation)
- **HttpClient** for calling GeoLocation API
- **Hosted BackgroundService** for cleanup jobs
- **ILogger / Dependency Injection**
- **Options Pattern** for strongly-typed configuration

---

## API Endpoints Overview

|   Method   | Endpoint                                              | Description                                         |
| :--------: | :---------------------------------------------------- | :-------------------------------------------------- |
|  **POST**  | `/api/countries/block`                                | Block a country permanently                         |
|  **POST**  | `/api/countries/temporal-block`                       | Temporarily block a country for a specific duration |
| **DELETE** | `/api/countries/block/{countryCode}`                  | Unblock a previously blocked country                |
|   **GET**  | `/api/countries/blocked?page=1&pageSize=10&search=eg` | List all blocked countries with pagination & search |
|   **GET**  | `/api/ip/lookup?ipAddress=8.8.8.8`                    | Get GeoLocation info for any IP                     |
|   **GET**  | `/api/ip/check-block`                                 | Check if the caller’s IP is blocked                 |
|   **GET**  | `/api/logs/blocked-attempts?page=1&pageSize=10`       | Retrieve paginated logs of blocked attempts         |

---

## Architecture & Design

```
📦 A-Technologies-Assignment
 ┣ 📁 Controllers
 ┃ ┣ CountriesController.cs
 ┃ ┣ IpController.cs
 ┃ ┗ LogsController.cs
 ┣ 📁 Services
 ┃ ┣ BlockService.cs
 ┃ ┗ GeoLocationService.cs
 ┣ 📁 Repositories
 ┃ ┗ InMemoryBlockedRepository.cs
 ┣ 📁 BackgroundJobs
 ┃ ┗ TemporalBlockCleanupService.cs
 ┣ 📁 DTOs / Models / Validators / Settings
 ┗ Program.cs
```

* **Controllers** → Handle API endpoints and HTTP responses.
* **Services** → Contain business logic for blocking, checking, and Geo lookups.
* **Repositories** → Manage in-memory data storage.
* **Validators** → Apply FluentValidation rules for request validation.
* **BackgroundJobs** → Run scheduled cleanup of expired temporary blocks.
* **Settings** → Hold strongly-typed configuration for GeoLocation API.

---

## Key Features

* **Permanent & Temporary blocking** with expiration.
* **Automatic cleanup** for expired blocks every 5 minutes.
* **GeoLocation integration** using `ipapi.com` (async via HttpClient).
* **Thread-safe in-memory storage** using `ConcurrentDictionary`.
* **Pagination** and **search** support for blocked countries and logs.
* **Logging system** to record all blocked access attempts.
* **Options Pattern + Validation** for startup configuration safety.
* **Swagger Documentation** with XML comments and clear grouping by controllers.

---

## Configuration

All API settings are stored in `appsettings.json`:

```json
"GeoLocationSettings": {
  "BaseUrl": "https://api.ipapi.com/api",
  "ApiKey": "<API_KEY>"
}
```

If the API key is missing or invalid, the app will not start — thanks to:

```csharp
builder.Services.AddOptions<GeoLocationSettings>()
    .BindConfiguration(GeoLocationSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

---

## Example Log Entry

```json
{
  "IpAddress": "102.45.33.19",
  "CountryCode": "EG",
  "IsBlocked": true,
  "UserAgent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
  "Timestamp": "2025-10-27T20:10:45Z"
}
```

---

## Author

- **Developed by:** Mahmoud Mohamed
- **Email:** [mahmoud.abdalaziz@outlook.com](mailto:mahmoud.abdalaziz@outlook.com)
- **GitHub:** [M7-TROJAN](https://github.com/M7-TROJAN)
- **LinkedIn:** [Mahmoud Mohamed Abd](https://www.linkedin.com/in/mahmoud-mohamed-abd)
