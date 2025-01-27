using Microsoft.AspNetCore.Mvc;
using OZON_Delivery_checker.Data;
using OZON_Delivery_checker.DataBase;

namespace OZON_Delivery_checker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackingController : ControllerBase
    {
        private readonly TrackingAPIService _trackingApiService;
        private readonly TrackingDBService _trackingDbService;
        private readonly ILogger<TrackingController> _logger;

        public TrackingController(TrackingAPIService trackingApiService, TrackingDBService trackingDbService, ILogger<TrackingController> logger)
        {
            _trackingApiService = trackingApiService;
            _trackingDbService = trackingDbService;
            _logger = logger;
        }

        [HttpGet("{trackingNumber}")]
        public async Task<IActionResult> GetTrackingInfo(string trackingNumber)
        {
            try
            {
                var existingRequestRecord = await _trackingDbService.GetRequestRecordFromDbAsync(trackingNumber);

                if (existingRequestRecord.LastFetchedAt.HasValue &&
                    DateTime.UtcNow - existingRequestRecord.LastFetchedAt.Value < TimeSpan.FromMinutes(1))
                {
                    var a = DateTime.UtcNow - DateTime.Parse(existingRequestRecord.DeliveryDateBegin);
                    var b = TimeSpan.FromMinutes(1);

                    _logger.LogInformation($"Данные для {trackingNumber} взяты из локальной базы данных ");

                    var existingRequestEvents = await _trackingDbService.GetRequestEventsFromDbAsync(existingRequestRecord.Id);

                    return Ok(new
                    {
                        deliveryDateBegin = existingRequestRecord.DeliveryDateBegin,
                        deliveryDateEnd = existingRequestRecord.DeliveryDateEnd,
                        deliveryDatePeriodChangedMoment = existingRequestRecord.DeliveryDatePeriodChangedMoment,
                        items = existingRequestEvents.Select(e => new
                        {
                            eventName = e.EventName,
                            moment = e.Moment
                        })
                    });
                }

                _logger.LogInformation($"Данные для {trackingNumber} запрашиваются у стороннего API");

                var trackingInfo = await _trackingApiService.GetTrackingInfoFromApiAsync(trackingNumber);

                var requestRecord = new RequestRecord
                {
                    TrackingNumber = trackingNumber,
                    DeliveryDateBegin = trackingInfo.DeliveryDateBegin,
                    DeliveryDateEnd = trackingInfo.DeliveryDateEnd,
                    DeliveryDatePeriodChangedMoment = trackingInfo.DeliveryDatePeriodChangedMoment,
                    LastFetchedAt = DateTime.UtcNow
                };

                await _trackingDbService.SaveRequestRecordAsync(requestRecord);

                var requestEvents = trackingInfo.Items.Select(item => new RequestEvent
                {
                    EventName = item.Event,
                    Moment = item.Moment,
                    RequestRecordId = requestRecord.Id
                }).ToList();

                await _trackingDbService.SaveRequestEventsAsync(requestEvents);

                _logger.LogInformation($"Данные для {trackingNumber} успешно записаны в базу данных");

                return Ok(new
                {
                    deliveryDateBegin = trackingInfo.DeliveryDateBegin,
                    deliveryDateEnd = trackingInfo.DeliveryDateEnd,
                    deliveryDatePeriodChangedMoment = trackingInfo.DeliveryDatePeriodChangedMoment,
                    items = trackingInfo.Items.Select(i => new
                    {
                        eventName = i.Event,
                        moment = i.Moment
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                return StatusCode(500, "Произошла ошибка.");
            }
        }
    }
}
