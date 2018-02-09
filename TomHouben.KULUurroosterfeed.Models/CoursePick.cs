using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace TomHouben.KULUurroosterfeed.Models
{
    public class CoursePick
    {
        public ObjectId Id { get; set; }

        public DateTime LastUsed { get; set; }

        public List<string> SelectedCourses { get; set; }
    }
}
