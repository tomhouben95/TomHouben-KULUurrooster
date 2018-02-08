using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TomHouben.KULUurroosterfeed.HTMLParserServices.Abstractions;
using TomHouben.KULUurroosterfeed.ICalService.Abstractions;
using TomHouben.KULUurroosterfeed.Models;
using TomHouben.KULUurroosterfeed.Repositories.Abstractions;
using TomHouben.KULUurroosterfeed.Services.Abstractions;

namespace TomHouben.KULUurroosterfeed.Services
{
    public class CalendarService: ICalendarService
    {
        private readonly ICalendarPullRepository _calendarPullRepository;
        private readonly IEffectiveTimeTableParser _effectiveTimeTableParser;
        private readonly IICalService _icalService;
        private readonly CalendarServiceOptions _options;

        public CalendarService(
            ICalendarPullRepository calendarPullRepository,
            IICalService icalService,
            IEffectiveTimeTableParser effectiveTimeTableParser,
            IOptions<CalendarServiceOptions> options)
        {
            _calendarPullRepository = calendarPullRepository;
            _effectiveTimeTableParser = effectiveTimeTableParser;
            _icalService = icalService;
            _options = options.Value;
        }

        public async Task<IEnumerable<string>> GetCoursesAsync()
        {
            var storedPull = await _calendarPullRepository.GetQueryable().FirstOrDefaultAsync() ?? await PullCalendarData();

            if (!IsStillValid(storedPull.PullDate)) storedPull = await PullCalendarData(storedPull);

            return storedPull.Courses;
        }

        public async Task<byte[]> GetICalAsync(IEnumerable<string> courses)
        {
            var storedPull = await _calendarPullRepository.GetQueryable().FirstOrDefaultAsync();

            if (storedPull == null) storedPull = await PullCalendarData();

            if (!IsStillValid(storedPull.PullDate)) storedPull = await PullCalendarData(storedPull);

            var selectedTimeEntries = storedPull.Entries.SelectTimeTableEntries(courses);

            return _icalService.GenerateICal(selectedTimeEntries);
        }

        private async Task<CalendarPull> PullCalendarData(CalendarPull previousPull = null)
        {
            var entries = new List<TimeTableEntry>();

            Parallel.Invoke(() =>
                            entries.AddRange(_effectiveTimeTableParser.Parse(_options.FirstSemesterUrl)),
                           () =>
                           entries.AddRange(_effectiveTimeTableParser.Parse(_options.SecondSemesterUrl)));


            var result = new CalendarPull
            {
                Entries = entries,
                PullDate = DateTime.Now,
                Courses = entries.Select(x => x.Title).Distinct().OrderBy(x => x)
            };

            if (previousPull != null)
                result.Id = previousPull.Id;


            await _calendarPullRepository.UpsertAsync(result);
            return result;
        }

        private bool IsStillValid(DateTime lastPull)
        {
            var difference = DateTime.Now - lastPull;

            return difference.TotalHours >= _options.RefreshInterval;
        }

    }
}
