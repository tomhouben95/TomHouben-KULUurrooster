using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TomHouben.AspNetCore.MongoDb.Abstractions;
using TomHouben.KULUurroosterfeed.Models;
using TomHouben.KULUurroosterfeed.Repositories.Abstractions;

namespace TomHouben.KULUurroosterfeed.Repositories
{
    public class TimeTableEntryRepository: ITimeTableEntryRepository
    {
        private readonly IMongoCollection<TimeTableEntry> _collection;

        public TimeTableEntryRepository(IMongoConnection connection)
        {
            _collection = connection.Db.GetCollection<TimeTableEntry>(nameof(TimeTableEntry));
        }
        
        public Task<List<string>> GetAllCoursesAsync()
        {
            return _collection.AsQueryable().Select(x => x.Course).Distinct().OrderBy(x => x).ToListAsync();
        }

        public async Task<DateTime?> GetCreatedDateAsync()
        {
            var entry = await _collection.AsQueryable().FirstOrDefaultAsync();

            return entry?.CreatedAt;
        }

        public Task<List<TimeTableEntry>> GetEntriesAsync()
        {
            return _collection.AsQueryable().ToListAsync();
        }

        public async Task<List<TimeTableEntry>> GetEntriesAsync(IEnumerable<string> coursesList)
        {
            var filter = Builders<TimeTableEntry>.Filter.In(x => x.Course, coursesList);
            return await (await _collection.FindAsync(filter)).ToListAsync();
        }

        public async Task UpdateEntriesAsync(IEnumerable<TimeTableEntry> updatedEntries)
        {
            await _collection.DeleteManyAsync(x => true);
            await _collection.InsertManyAsync(updatedEntries);
        }
    }
}