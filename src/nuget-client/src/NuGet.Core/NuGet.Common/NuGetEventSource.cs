// Copyright (c) .NET Foundation. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics.Tracing;

namespace NuGet.Common
{
    /// <summary>
    /// Represents a class used for logging trace events from NuGet.
    /// </summary>
    public static class NuGetEventSource
    {
        /// <summary>
        /// Gets a <see cref="NuGetEventSource" /> which can be used to trace events from NuGet.
        /// </summary>
        public static EventSource Instance { get; } = new EventSource("Microsoft-NuGet");

        /// <summary>
        /// Gets a value indicating whether tracing is enabled for the <see cref="NuGetEventSource" />.
        /// </summary>
        public static bool IsEnabled { get; } = Instance.IsEnabled();

        /// <summary>
        /// Represents a class for declaring event keywords. Each keyword must be a flag (2^N) for use in a bitwise operation.
        /// </summary>
        public static class Keywords
        {
            /// <summary>
            /// The event keyword for tracing events related to NuGet's common functionality from the NuGet.Common namespace.
            /// </summary>
            public const EventKeywords Common = (EventKeywords)1;

            /// <summary>
            /// The event keyword for tracing events related to NuGet's configuration and settings from the NuGet.Configuration namespace.
            /// </summary>
            public const EventKeywords Configuration = (EventKeywords)2;

            /// <summary>
            /// The event keyword for tracing events related to logging.
            /// </summary>
            public const EventKeywords Logging = (EventKeywords)4;

            /// <summary>
            /// The event keyword for tracing events related to performance.
            /// </summary>
            public const EventKeywords Performance = (EventKeywords)8;

            /// <summary>
            /// The event keyword for tracing events related to the NuGet-based MSBuild project SDK resolver.
            /// </summary>
            public const EventKeywords SdkResolver = (EventKeywords)16;
        }
    }
}


