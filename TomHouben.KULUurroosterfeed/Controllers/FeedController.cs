using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using TomHouben.KULUurroosterfeed.HTMLParserServices.Abstractions;
using TomHouben.KULUurroosterfeed.ICalService.Abstractions;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.Controllers
{
    public class FeedController: Controller
    {
        private readonly IEffectiveTimeTableParser _timeTableParser;
        private readonly IICalService _icalService;

        public FeedController(
            IEffectiveTimeTableParser timeTableParser,
            IICalService icalService)
        {
            _timeTableParser = timeTableParser;
            _icalService = icalService;
        }

        [HttpGet("calendar")]
        public IActionResult GetFeed()
        {
            var entries = _timeTableParser.Parse("https://people.cs.kuleuven.be/~btw/roosters1718/cws_semester_1.html");
            var ical = _icalService.GenerateICal(entries);

            return File(ical, "text/calendar");
        }
    }
}
