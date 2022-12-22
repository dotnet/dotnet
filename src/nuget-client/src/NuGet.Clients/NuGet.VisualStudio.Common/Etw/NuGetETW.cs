// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics.Tracing;

namespace NuGet.VisualStudio.Etw
{
    public static class NuGetETW
    {
        public const string ExtensibilityEventSourceName = "NuGet-VS-Extensibility";

        /// <summary>
        /// This EventSource should only be used to track usage for NuGet's VS extensibility APIs. It is listened to
        /// by ExtensibilityTelemetryCollector.
        /// </summary>
        public static EventSource ExtensibilityEventSource { get; } = new EventSource(ExtensibilityEventSourceName);

        public static EventSourceOptions AddEventOptions { get; } =
            new EventSourceOptions()
            {
                Opcode = CustomOpcodes.Add,
                Level = EventLevel.Informational,
                ActivityOptions = EventActivityOptions.Detachable
            };

        public static EventSourceOptions RemoveEventOptions { get; } =
            new EventSourceOptions()
            {
                Opcode = CustomOpcodes.Remove,
                Level = EventLevel.Informational,
                ActivityOptions = EventActivityOptions.Detachable
            };

        public static EventSourceOptions InfoEventOptions { get; } =
            new EventSourceOptions()
            {
                Opcode = EventOpcode.Info,
                Level = EventLevel.Informational,
                ActivityOptions = EventActivityOptions.None
            };

        public static class CustomOpcodes
        {
            public static readonly EventOpcode Add = (EventOpcode)11;
            public static readonly EventOpcode Remove = (EventOpcode)12;
        }
    }
}
