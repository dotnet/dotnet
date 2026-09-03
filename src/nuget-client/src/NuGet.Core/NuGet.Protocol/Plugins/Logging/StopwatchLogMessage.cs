// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json.Nodes;

namespace NuGet.Protocol.Plugins
{
    internal sealed class StopwatchLogMessage : PluginLogMessage
    {
        private readonly long _frequency;

        internal StopwatchLogMessage(DateTimeOffset now, long frequency)
            : base(now)
        {
            _frequency = frequency;
        }

        public override string ToString()
        {
            var message = new JsonObject
            {
                ["frequency"] = _frequency,
            };

            return ToString("stopwatch", message);
        }
    }
}
