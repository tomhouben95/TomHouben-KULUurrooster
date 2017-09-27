using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace TomHouben.KULUurroosterfeed.Models
{
    public class CalendarPull
    {
        public ObjectId Id { get; set; }

        public DateTime PullDate { get; set; }

        public string PullUrl { get; set; }

        public List<TimeTableEntry> Entries { get; set; }
    }
}
