// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    internal sealed class MachineLogMessage : PluginLogMessage
    {
        private readonly int _logicalProcessorCount;

        internal MachineLogMessage(DateTimeOffset now)
            : base(now)
        {
            _logicalProcessorCount = Environment.ProcessorCount;
        }

        public override string ToString()
        {
            var message = new JObject(new JProperty("logical processor count", _logicalProcessorCount));

            return ToString("machine", message);
        }
    }
}
