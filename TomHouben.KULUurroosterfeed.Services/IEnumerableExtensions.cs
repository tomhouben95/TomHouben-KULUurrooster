using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.Services
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TimeTableEntry> SelectTimeTableEntries(this IEnumerable<TimeTableEntry> source, IEnumerable<string> selectedCourses)
        {
            var result = source.Where(x => selectedCourses.Contains(x.Title));
            return result;

        }
    }
}
