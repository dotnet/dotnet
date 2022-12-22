// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    internal sealed class PluginInstanceLogMessage : PluginLogMessage
    {
        private readonly string _pluginId;
        private readonly int? _processId;
        private readonly PluginState _state;

        internal PluginInstanceLogMessage(DateTimeOffset now, string pluginId, PluginState state)
            : this(now, pluginId, state, processId: null)
        {
        }

        internal PluginInstanceLogMessage(DateTimeOffset now, string pluginId, PluginState state, int? processId)
            : base(now)
        {
            _pluginId = pluginId;
            _processId = processId;
            _state = state;
        }

        public override string ToString()
        {
            var message = new JObject(
                new JProperty("plugin ID", _pluginId),
                new JProperty("state", _state));

            if (_processId.HasValue)
            {
                message.Add("process ID", _processId.Value);
            }

            return ToString("plugin instance", message);
        }
    }
}
