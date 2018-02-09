using System;
using MongoDB.Bson;

namespace TomHouben.KULUurroosterfeed.Models
{
    public class TimeTableEntry
    {

        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
        
        public string Course { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Room { get; set; }
    }
}
