using OZON_Delivery_checker.Models;
using System.Text.Json;

public class TrackingAPIService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TrackingAPIService> _logger;

    public TrackingAPIService(HttpClient httpClient, ILogger<TrackingAPIService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ResponseBody> GetTrackingInfoFromApiAsync(string trackingNumber)
    {
        _logger.LogInformation($"Запрос для {trackingNumber} отправлен");
        var requestUrl = $"https://tracking.ozon.ru/p-api/ozon-track-bff/tracking/{trackingNumber}?source=Global";
        var response = await _httpClient.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Ошибка ответа от API: {response.StatusCode}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var trackingInfo = JsonSerializer.Deserialize<ResponseBody>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (trackingInfo == null)
        {
            throw new Exception("Ошибка десериализации данных.");
        }
        _logger.LogInformation($"Данные для {trackingNumber} получены");
        return trackingInfo;
    }
}
