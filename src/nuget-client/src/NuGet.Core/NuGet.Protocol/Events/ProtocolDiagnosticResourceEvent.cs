// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Events
{
    public sealed class ProtocolDiagnosticResourceEvent
    {
        public string Source { get; }
        public string ResourceType { get; }
        public string Type { get; }
        public string Method { get; }
        public TimeSpan Duration { get; }

        public ProtocolDiagnosticResourceEvent(
            string source,
            string resourceType,
            string type,
            string method,
            TimeSpan duration)
        {
            Source = source;
            ResourceType = resourceType;
            Type = type;
            Method = method;
            Duration = duration;
        }
    }
}
