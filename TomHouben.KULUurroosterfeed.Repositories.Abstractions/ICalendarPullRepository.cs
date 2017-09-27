using System;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.Repositories.Abstractions
{
    public interface ICalendarPullRepository
    {
        IMongoQueryable<CalendarPull> GetQueryable();

        Task<CalendarPull> UpsertAsync(CalendarPull calendarPull);
    }
}
