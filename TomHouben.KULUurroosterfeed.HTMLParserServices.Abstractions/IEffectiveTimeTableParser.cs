using System;
using System.Collections.Generic;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.HTMLParserServices.Abstractions
{
    public interface IEffectiveTimeTableParser
    {
        IEnumerable<TimeTableEntry> Parse(string url);
    }
}
