// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json.Nodes;

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
            var message = new JsonObject
            {
                ["plugin ID"] = _pluginId,
                ["state"] = _state.ToString(),
            };

            if (_processId.HasValue)
            {
                message["process ID"] = _processId.Value;
            }

            return ToString("plugin instance", message);
        }
    }
}
