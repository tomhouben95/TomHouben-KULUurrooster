using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.HTMLParserServices.Helpers
{
    public static class HtmlParserHelper
    {
        public static DateTime ParseDateRegion(string text)
        {
            var trimmedText = text.TrimEnd(':');
            var textParts = trimmedText.Split(' ');

            return DateTime.ParseExact(textParts[1], FormatConstants.DateFormat,
                CultureInfo.CreateSpecificCulture("nl-be"), DateTimeStyles.AdjustToUniversal).ToUniversalTime();
        }

        public static IEnumerable<TimeTableEntry> ParseTable(HtmlNode tableNode, DateTime date)
        {
            if (!tableNode.Name.Equals("table", StringComparison.OrdinalIgnoreCase)) throw new Exception();

            var entries = tableNode.ChildNodes.Where(x => x.Name == "tr");

            var result = new List<TimeTableEntry>();

            foreach (var entry in entries)
            {
                if (!entry.Name.Equals("tr")) continue;

                var timeTableEntry = new TimeTableEntry();

                var node = entry.ChildNodes.FirstOrDefault(x => x.Name == "td");
                var startEndTime = ParseHours(node.InnerText, date);
                timeTableEntry.Start = startEndTime.Item1;
                timeTableEntry.End = startEndTime.Item2;

                node = node.NextSibling;

                node = node.NextSibling;
                timeTableEntry.Room = ParseLocation(node.InnerText);

                node = node.NextSibling;
                node = node.NextSibling;

                node = node.NextSibling;
                timeTableEntry.Course = ParseTitle(node);

                result.Add(timeTableEntry);
            }

            return result;
        }

        private static Tuple<DateTime, DateTime> ParseHours(string text, DateTime date)
        {
            var splitText = text.Split(new string[] { "&#8212;" }, StringSplitOptions.RemoveEmptyEntries);
            var start = TimeSpan.Parse(splitText[0], CultureInfo.CurrentCulture);
            var end = TimeSpan.Parse(splitText[1], CultureInfo.CurrentCulture);

            return new Tuple<DateTime, DateTime>(date + start, date + end);
        }

        private static string ParseLocation(string text)
        {
            var splitText = text.Split(new string[] { " "}, StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" - ", splitText);
        }

        private static string ParseTitle(HtmlNode node)
        {
            var titleNode = node.ChildNodes
                                .FirstOrDefault(x => x.Name == "a")
                                .ChildNodes
                                .FirstOrDefault(x => x.Name == "font");

            return titleNode.InnerHtml;
        }
    }
}
