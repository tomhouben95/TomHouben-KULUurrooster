using System;
namespace TomHouben.KULUurroosterfeed.Models
{
    public class TimeTableEntry
    {
        public string Title { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Room { get; set; }
    }
}
