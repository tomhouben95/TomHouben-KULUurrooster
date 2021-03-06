﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TomHouben.KULUurroosterfeed.Services.Abstractions
{
    public interface ITimeTableEntryService
    {
        Task<IEnumerable<string>> GetCoursesAsync();

        Task<byte[]> GetICalAsync(IEnumerable<string> courseSelection);
    }
}
