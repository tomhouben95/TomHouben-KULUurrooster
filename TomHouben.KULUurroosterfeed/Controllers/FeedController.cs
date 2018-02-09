using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomHouben.KULUurroosterfeed.HTMLParserServices.Abstractions;
using TomHouben.KULUurroosterfeed.ICalService.Abstractions;
using TomHouben.KULUurroosterfeed.Models;
using TomHouben.KULUurroosterfeed.Services.Abstractions;

namespace TomHouben.KULUurroosterfeed.Controllers
{
    [Route("feed")]
    public class FeedController: Controller
    {
        private readonly ITimeTableEntryService _timeTableEntryService;
        private readonly UserManager<TimeTableUser> _userManager;

        public FeedController(
            ITimeTableEntryService timeTableEntryService,
            UserManager<TimeTableUser> userManager)
        {
            _timeTableEntryService = timeTableEntryService;
            _userManager = userManager;
        }



        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeed(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Error();

            var selectedCourses = user.SelectedCourses;

            var ical = await _timeTableEntryService.GetICalAsync(selectedCourses);

            return File(ical, "text/calendar", "uurrooster.ics");
        }

        [HttpGet("error")]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
    }
}
