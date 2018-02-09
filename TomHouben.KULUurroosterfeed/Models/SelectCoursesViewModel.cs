using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TomHouben.KULUurroosterfeed.Models
{
    public class SelectCoursesViewModel
    {

        [Required]
        public List<CourseSelectionViewModel> SelectedCourses { get; set; } = new List<CourseSelectionViewModel>();

    }
}
