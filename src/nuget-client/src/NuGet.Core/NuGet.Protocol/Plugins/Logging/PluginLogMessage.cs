// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Text.Json.Nodes;

namespace NuGet.Protocol.Plugins
{
    internal abstract class PluginLogMessage : IPluginLogMessage
    {
        private readonly DateTime _now;

        protected PluginLogMessage(DateTimeOffset now)
        {
            _now = now.UtcDateTime;
        }

        protected string ToString(string type, JsonObject message)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(type));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var outerMessage = new JsonObject
            {
                ["now"] = _now.ToString("O", CultureInfo.InvariantCulture), // round-trip format
                ["type"] = type,
                ["message"] = message,
            };

            return outerMessage.ToJsonString();
        }
    }
}
