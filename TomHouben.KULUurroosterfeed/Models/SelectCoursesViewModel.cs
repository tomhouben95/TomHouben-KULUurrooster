using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TomHouben.KULUurroosterfeed.Models
{
    public class SelectCoursesViewModel
    {
        public string[] Courses { get; set; }

        [Required]
        public bool[] SelectedCourses { get; set; }

        public string GeneratedLink { get; set; }

    }
}
