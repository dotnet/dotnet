// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    internal abstract class PluginLogMessage : IPluginLogMessage
    {
        private static readonly StringEnumConverter _enumConverter = new StringEnumConverter();

        private readonly DateTime _now;

        protected PluginLogMessage(DateTimeOffset now)
        {
            _now = now.UtcDateTime;
        }

        protected string ToString(string type, JObject message)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(type));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var outerMessage = new JObject(
                new JProperty("now", _now.ToString("O", CultureInfo.InvariantCulture)), // round-trip format
                new JProperty("type", type),
                new JProperty("message", message));

            return outerMessage.ToString(Formatting.None, _enumConverter);
        }
    }
}
