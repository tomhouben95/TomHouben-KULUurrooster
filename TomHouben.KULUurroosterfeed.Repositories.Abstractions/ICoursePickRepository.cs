using System;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.Repositories.Abstractions
{
    public interface ICoursePickRepository
    {
		IMongoQueryable<CoursePick> GetQueryable();

		Task<CoursePick> UpsertAsync(CoursePick calendarPull);
    }
}
