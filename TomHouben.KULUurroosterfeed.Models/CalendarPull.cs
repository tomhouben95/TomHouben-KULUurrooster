using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace TomHouben.KULUurroosterfeed.Models
{
    public class CalendarPull
    {
        public ObjectId Id { get; set; }

        public IEnumerable<string> Courses { get; set; }

        public DateTime PullDate { get; set; }

        public IEnumerable<TimeTableEntry> Entries { get; set; }
    }
}
