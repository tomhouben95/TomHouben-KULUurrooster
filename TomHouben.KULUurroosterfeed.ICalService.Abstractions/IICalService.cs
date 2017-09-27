using System;
using System.Collections.Generic;
using TomHouben.KULUurroosterfeed.Models;

namespace TomHouben.KULUurroosterfeed.ICalService.Abstractions
{
    public interface IICalService
    {
        byte[] GenerateICal(IEnumerable<TimeTableEntry> entries);
    }
}
