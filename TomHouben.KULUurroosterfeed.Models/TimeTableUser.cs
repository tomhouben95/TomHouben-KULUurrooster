using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TomHouben.KULUurroosterfeed.Models
{
    public class TimeTableUser: IdentityUser
    {
        public TimeTableUser(string email)
        {
            Email = email;
            UserName = email;
            SelectedCourses = new List<string>();
        }

        public List<string> SelectedCourses { get; set; }
    }
}