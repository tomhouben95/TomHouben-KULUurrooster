using System;
namespace TomHouben.KULUurroosterfeed.Services.Abstractions
{
    public class CalendarServiceOptions
    {
        public string FirstSemesterUrl { get; set; }

        public string SecondSemesterUrl { get; set; }

        public double RefreshInterval { get; set; }
    }
}
