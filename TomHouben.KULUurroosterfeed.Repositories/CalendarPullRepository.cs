using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TomHouben.AspNetCore.MongoDb.Abstractions;
using TomHouben.KULUurroosterfeed.Models;
using TomHouben.KULUurroosterfeed.Repositories.Abstractions;

namespace TomHouben.KULUurroosterfeed.Repositories
{
    public class CalendarPullRepository: ICalendarPullRepository
    {
        private IMongoCollection<CalendarPull> _collection;

        public CalendarPullRepository(IMongoConnection connection)
        {
            _collection = connection.Db.GetCollection<CalendarPull>(nameof(CalendarPull));
        }

        public IMongoQueryable<CalendarPull> GetQueryable()
        {
            return _collection.AsQueryable();
        }

        public async Task<CalendarPull> UpsertAsync(CalendarPull calendarPull)
        {
            var result = await _collection
                .ReplaceOneAsync(x => x.Id == calendarPull.Id, calendarPull, new UpdateOptions { IsUpsert = true });

            return calendarPull;
        }
    }
}
