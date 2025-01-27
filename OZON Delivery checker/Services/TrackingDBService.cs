using Microsoft.EntityFrameworkCore;
using OZON_Delivery_checker.Data;
using OZON_Delivery_checker.DataBase;

public class TrackingDBService
{
    private readonly ApplicationDbContext _dbContext;

    public TrackingDBService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RequestRecord> GetRequestRecordFromDbAsync(string trackingNumber)
    {
        return await _dbContext.RequestRecords
            .Where(r => r.TrackingNumber == trackingNumber)
            .OrderByDescending(r => r.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<RequestEvent>> GetRequestEventsFromDbAsync(int requestRecordId)
    {
        return await _dbContext.RequestEvents
            .Where(e => e.RequestRecordId == requestRecordId)
            .ToListAsync();
    }

    public async Task SaveRequestRecordAsync(RequestRecord requestRecord)
    {
        await _dbContext.RequestRecords.AddAsync(requestRecord);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveRequestEventsAsync(List<RequestEvent> requestEvents)
    {
        await _dbContext.RequestEvents.AddRangeAsync(requestEvents);
        await _dbContext.SaveChangesAsync();
    }
}
