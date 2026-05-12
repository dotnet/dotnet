// Copyright (c) .NET Foundation. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics.Tracing;

namespace NuGet.Configuration
{
    [EventSource(Name = "Microsoft-NuGet-Configuration")]
    internal sealed class ConfigurationEventSource : EventSource
    {
        public static readonly ConfigurationEventSource Instance = new();

        private ConfigurationEventSource() { }

        public static class Keywords
        {
            public const EventKeywords Configuration = (EventKeywords)2;
        }

        public static class Tasks
        {
            public const EventTask SettingsFile_FileRead = (EventTask)1;
            public const EventTask SettingsLoadingContext_FileRead = (EventTask)2;
        }

        [Event(1, Level = EventLevel.Informational, Keywords = Keywords.Configuration, Opcode = EventOpcode.Start, Task = Tasks.SettingsFile_FileRead, ActivityOptions = EventActivityOptions.Detachable)]
        public void SettingsFile_FileReadStart(string configFilePath, int isMachineWide, int isReadOnly)
        {
            WriteEvent(1, configFilePath ?? string.Empty, isMachineWide, isReadOnly);
        }

        [Event(2, Level = EventLevel.Informational, Keywords = Keywords.Configuration, Opcode = EventOpcode.Stop, Task = Tasks.SettingsFile_FileRead, ActivityOptions = EventActivityOptions.Detachable)]
        public void SettingsFile_FileReadStop(string configFilePath, int isMachineWide, int isReadOnly)
        {
            WriteEvent(2, configFilePath ?? string.Empty, isMachineWide, isReadOnly);
        }

        [Event(3, Level = EventLevel.Informational, Keywords = Keywords.Configuration, Task = Tasks.SettingsLoadingContext_FileRead)]
        public void SettingsLoadingContext_FileRead(string fullPath, int isMachineWide, int isReadOnly)
        {
            WriteEvent(3, fullPath ?? string.Empty, isMachineWide, isReadOnly);
        }
    }
}
