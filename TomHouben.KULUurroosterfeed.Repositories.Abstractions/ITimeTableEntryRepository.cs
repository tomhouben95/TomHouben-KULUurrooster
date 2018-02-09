using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.Repositories.Abstractions
{
    public interface ITimeTableEntryRepository
    {
        Task<List<string>> GetAllCoursesAsync();

        Task<DateTime?> GetCreatedDateAsync();
        
        Task<List<TimeTableEntry>> GetEntriesAsync();

        Task<List<TimeTableEntry>> GetEntriesAsync(IEnumerable<string> coursesList);

        Task UpdateEntriesAsync(IEnumerable<TimeTableEntry> updatedEntries);
    }
}