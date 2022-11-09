// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;

namespace NuGet.Test.TestExtensions.TestablePlugin
{
    internal sealed class Response
    {
        [JsonRequired]
        public MessageType Type { get; set; }

        [JsonRequired]
        public MessageMethod Method { get; set; }

        public JObject Payload { get; set; }
    }
}
