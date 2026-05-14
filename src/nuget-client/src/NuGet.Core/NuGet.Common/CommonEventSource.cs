// Copyright (c) .NET Foundation. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics.Tracing;

namespace NuGet.Common
{
    [EventSource(Name = "Microsoft-NuGet-Common")]
    internal sealed class CommonEventSource : EventSource
    {
        public static readonly CommonEventSource Instance = new();

        private CommonEventSource() { }

        public static class Keywords
        {
            public const EventKeywords Common = (EventKeywords)1;
            public const EventKeywords Performance = (EventKeywords)8;
        }

        public static class Tasks
        {
            public const EventTask MigrationRunner_Run = (EventTask)1;
        }

        [Event(1, Level = EventLevel.Informational, Keywords = Keywords.Common | Keywords.Performance, Opcode = EventOpcode.Start, Task = Tasks.MigrationRunner_Run, ActivityOptions = EventActivityOptions.Detachable)]
        public void MigrationRunner_RunStart()
        {
            WriteEvent(1);
        }

        [Event(2, Level = EventLevel.Informational, Keywords = Keywords.Common | Keywords.Performance, Opcode = EventOpcode.Stop, Task = Tasks.MigrationRunner_Run, ActivityOptions = EventActivityOptions.Detachable)]
        public void MigrationRunner_RunStop(string migrationFileFullPath, int migrationPerformed)
        {
            WriteEvent(2, migrationFileFullPath ?? string.Empty, migrationPerformed);
        }
    }
}
