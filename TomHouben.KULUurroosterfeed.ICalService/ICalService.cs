using System;
using System.Collections.Generic;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization.iCalendar.Serializers;
using Ical.Net.Serialization;

using TomHouben.KULUurroosterfeed.ICalService.Abstractions;
using TomHouben.KULUurroosterfeed.Models;
using System.Text;

namespace TomHouben.KULUurroosterfeed.ICalService
{
    public class ICalService: IICalService
    {
        public byte[] GenerateICal(IEnumerable<TimeTableEntry> entries)
        {
            var calendar = new Calendar();

            //const string timeZone = "Europe/Brussels";

            foreach(var entry in entries)
            {
                calendar.Events.Add(new CalendarEvent{
                    Summary = entry.Title,
                    Location = entry.Room,
                    Start = new CalDateTime(entry.Start),
                    End = new CalDateTime(entry.End)
                });
            }

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

            return Encoding.UTF8.GetBytes(serializedCalendar);
        }
    }
}
