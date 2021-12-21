// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Yarp.Configuration.Model
{
    public interface ILogLevelDictionary
    {
        IDictionary<string, LogLevel>? LogLevel { get; set; }
    }
}