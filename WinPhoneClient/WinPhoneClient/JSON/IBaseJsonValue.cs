﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace WinPhoneClient.JSON
{
    public interface IBaseJsonValue
    {
        JsonObject Json { get; set; }
        JsonObject CreateEmptyJsonObject();
    }
}
