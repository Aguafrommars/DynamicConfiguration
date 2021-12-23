// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Yarp.Configuration.Model
{
    public class ProviderLoggingConfig
    {
        public bool? IncludeScopes { get; set; }
        public IDictionary<string, LogLevel>? LogLevel { get; set; }

    }
}
