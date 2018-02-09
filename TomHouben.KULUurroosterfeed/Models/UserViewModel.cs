using System;
using System.Collections.Generic;

namespace TomHouben.KULUurroosterfeed.Models
{
    public class UserViewModel
    {
        public UserViewModel(string feedUrl, List<string> selectedCourses)
        {
            FeedUrl = feedUrl;
            SelectedCourses = selectedCourses;
        }
        
        public string FeedUrl { get; }
 
        
        public List<string> SelectedCourses { get; }
    }
}