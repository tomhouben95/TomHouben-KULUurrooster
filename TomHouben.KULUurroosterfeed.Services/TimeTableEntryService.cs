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
    public class TimeTableEntryService: ITimeTableEntryService
    {
        private readonly ITimeTableEntryRepository _timetableEntryRepository;
        private readonly IEffectiveTimeTableParser _effectiveTimeTableParser;
        private readonly IICalService _icalService;
        private readonly CalendarServiceOptions _options;

        public TimeTableEntryService(
            ITimeTableEntryRepository timetableEntryRepository,
            IICalService icalService,
            IEffectiveTimeTableParser effectiveTimeTableParser,
            IOptions<CalendarServiceOptions> options)
        {
            _timetableEntryRepository = timetableEntryRepository;
            _effectiveTimeTableParser = effectiveTimeTableParser;
            _icalService = icalService;
            _options = options.Value;
        }

        public async Task<IEnumerable<string>> GetCoursesAsync()
        {
            if (!IsStillValid(await _timetableEntryRepository.GetCreatedDateAsync()))
                await PullCalendarData();

            return await _timetableEntryRepository.GetAllCoursesAsync();
        }

        public async Task<byte[]> GetICalAsync(IEnumerable<string> courses)
        {
            if (!IsStillValid(await _timetableEntryRepository.GetCreatedDateAsync()))
                await PullCalendarData();

            var entries = await _timetableEntryRepository.GetEntriesAsync(courses);
            
            return _icalService.GenerateICal(entries);
        }

        private async Task PullCalendarData()
        {
            var sync = new object();
            var entries = new List<TimeTableEntry>();

            void AddRange(IEnumerable<TimeTableEntry> entriesToAdd) 
            {
                lock (sync)
                {
                    entries.AddRange(entriesToAdd);
                }
            }

            var t1 = Task.Run(() => AddRange(_effectiveTimeTableParser.Parse(_options.FirstSemesterUrl)));
            var t2 = Task.Run(() => AddRange(_effectiveTimeTableParser.Parse(_options.SecondSemesterUrl)));

            await Task.WhenAll(t1, t2);

            await _timetableEntryRepository.UpdateEntriesAsync(entries);
        }

        private bool IsStillValid(DateTime? lastPull)
        {
            if (!lastPull.HasValue)
                return false;
            
            var difference = DateTime.Now - lastPull.Value;

            return difference.TotalHours >= _options.RefreshInterval;
        }

    }
}
