// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Nodes;

namespace NuGet.Protocol.Plugins
{
    internal sealed class ProcessLogMessage : PluginLogMessage
    {
        private readonly int _processId;
        private readonly string _processName;
        private readonly DateTime _processStartTime;

        internal ProcessLogMessage(DateTimeOffset now)
            : base(now)
        {
            using (var process = Process.GetCurrentProcess())
            {
                _processId = process.Id;
                _processName = process.ProcessName;
                _processStartTime = process.StartTime.ToUniversalTime();
            }
        }

        public override string ToString()
        {
            var message = new JsonObject
            {
                ["process ID"] = _processId,
                ["process name"] = _processName,
                ["process start time"] = _processStartTime.ToString("O", CultureInfo.CurrentCulture),
            };

            return ToString("process", message);
        }
    }
}
