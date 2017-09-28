using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.Services
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TimeTableEntry> SelectTimeTableEntries(this IEnumerable<TimeTableEntry> source, BitArray bitArray, IEnumerable<string> courses)
        {
            var selectedCourses = SelectCourses(bitArray, courses);

            var result = source.Where(x => selectedCourses.Contains(x.Title));

            return result;

        }

        private static List<string> SelectCourses(BitArray bitArray, IEnumerable<string> courses)
        {
			var coursesArray = courses.ToArray();

			if (bitArray.Length < coursesArray.Length) throw new Exception();

			var selectedCourses = new List<string>();

            for (var i = 0; i < coursesArray.Length; i++)
            {
                if (bitArray[i]) selectedCourses.Add(coursesArray[i]);
            }

            return selectedCourses;
        }


    }
}
