// Copyright (c) .NET Foundation. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics.Tracing;

namespace Microsoft.Build.NuGetSdkResolver
{
    [EventSource(Name = "Microsoft-NuGet-SdkResolver")]
    internal sealed class SdkResolverEventSource : EventSource
    {
        public static readonly SdkResolverEventSource Instance = new();

        private SdkResolverEventSource() { }

        public static class Keywords
        {
            public const EventKeywords Logging = (EventKeywords)4;
            public const EventKeywords Performance = (EventKeywords)8;
            public const EventKeywords SdkResolver = (EventKeywords)16;
        }

        public static class Tasks
        {
            public const EventTask GlobalJsonRead = (EventTask)1;
            public const EventTask Resolve = (EventTask)2;
            public const EventTask GetResult = (EventTask)3;
            public const EventTask LoadSettings = (EventTask)4;
            public const EventTask RestorePackage = (EventTask)5;
            public const EventTask LogMessage = (EventTask)6;
        }

        // GlobalJsonRead (IDs 1-2)

        [Event(1, Level = EventLevel.Informational, Keywords = Keywords.SdkResolver | Keywords.Performance, Opcode = EventOpcode.Start, Task = Tasks.GlobalJsonRead, ActivityOptions = EventActivityOptions.Detachable)]
        public void GlobalJsonReadStart(string path, string projectFullPath, string solutionFullPath)
        {
            WriteEvent(1, path ?? string.Empty, projectFullPath ?? string.Empty, solutionFullPath ?? string.Empty);
        }

        [Event(2, Level = EventLevel.Informational, Keywords = Keywords.SdkResolver | Keywords.Performance, Opcode = EventOpcode.Stop, Task = Tasks.GlobalJsonRead, ActivityOptions = EventActivityOptions.Detachable)]
        public void GlobalJsonReadStop(string path, string projectFullPath, string solutionFullPath)
        {
            WriteEvent(2, path ?? string.Empty, projectFullPath ?? string.Empty, solutionFullPath ?? string.Empty);
        }

        // Resolve (IDs 3-4)

        [Event(3, Level = EventLevel.Informational, Keywords = Keywords.SdkResolver | Keywords.Performance, Opcode = EventOpcode.Start, Task = Tasks.Resolve, ActivityOptions = EventActivityOptions.Detachable)]
        public void ResolveStart(string name, string version)
        {
            WriteEvent(3, name ?? string.Empty, version ?? string.Empty);
        }

        [Event(4, Level = EventLevel.Informational, Keywords = Keywords.SdkResolver | Keywords.Performance, Opcode = EventOpcode.Stop, Task = Tasks.Resolve, ActivityOptions = EventActivityOptions.Detachable)]
        public void ResolveStop(string name, string version)
        {
            WriteEvent(4, name ?? string.Empty, version ?? string.Empty);
        }

        // GetResult (IDs 5-6)

        [Event(5, Level = EventLevel.Informational, Keywords = Keywords.SdkResolver | Keywords.Performance, Opcode = EventOpcode.Start, Task = Tasks.GetResult, ActivityOptions = EventActivityOptions.Detachable)]
        public void GetResultStart(string id, string version)
        {
            WriteEvent(5, id ?? string.Empty, version ?? string.Empty);
        }

        [Event(6, Level = EventLevel.Informational, Keywords = Keywords.SdkResolver | Keywords.Performance, Opcode = EventOpcode.Stop, Task = Tasks.GetResult, ActivityOptions = EventActivityOptions.Detachable)]
        public void GetResultStop(string id, string version, string installPath, int success)
        {
            WriteEvent(6, id ?? string.Empty, version ?? string.Empty, installPath ?? string.Empty, success);
        }

        // LoadSettings (IDs 7-8)

        [Event(7, Level = EventLevel.Informational, Keywords = Keywords.SdkResolver | Keywords.Performance, Opcode = EventOpcode.Start, Task = Tasks.LoadSettings, ActivityOptions = EventActivityOptions.Detachable)]
        public void LoadSettingsStart()
        {
            WriteEvent(7);
        }

        [Event(8, Level = EventLevel.Informational, Keywords = Keywords.SdkResolver | Keywords.Performance, Opcode = EventOpcode.Stop, Task = Tasks.LoadSettings, ActivityOptions = EventActivityOptions.Detachable)]
        public void LoadSettingsStop()
        {
            WriteEvent(8);
        }

        // RestorePackage (IDs 9-10)

        [Event(9, Level = EventLevel.Informational, Keywords = Keywords.SdkResolver | Keywords.Performance, Opcode = EventOpcode.Start, Task = Tasks.RestorePackage, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestorePackageStart(string id, string version)
        {
            WriteEvent(9, id ?? string.Empty, version ?? string.Empty);
        }

        [Event(10, Level = EventLevel.Informational, Keywords = Keywords.SdkResolver | Keywords.Performance, Opcode = EventOpcode.Stop, Task = Tasks.RestorePackage, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestorePackageStop(string id, string version)
        {
            WriteEvent(10, id ?? string.Empty, version ?? string.Empty);
        }

        // LogMessage (IDs 11-15, ETW level matches NuGet LogLevel, original level passed as payload)

        [Event(11, Level = EventLevel.Verbose, Keywords = Keywords.Logging, Task = Tasks.LogMessage)]
        public void LogMessageVerbose(int logLevel, string message)
        {
            WriteEvent(11, logLevel, message ?? string.Empty);
        }

        [Event(12, Level = EventLevel.Informational, Keywords = Keywords.Logging, Task = Tasks.LogMessage)]
        public void LogMessageInformational(int logLevel, string message)
        {
            WriteEvent(12, logLevel, message ?? string.Empty);
        }

        [Event(13, Level = EventLevel.LogAlways, Keywords = Keywords.Logging, Task = Tasks.LogMessage)]
        public void LogMessageAlways(int logLevel, string message)
        {
            WriteEvent(13, logLevel, message ?? string.Empty);
        }

        [Event(14, Level = EventLevel.Warning, Keywords = Keywords.Logging, Task = Tasks.LogMessage)]
        public void LogMessageWarning(int logLevel, string message)
        {
            WriteEvent(14, logLevel, message ?? string.Empty);
        }

        [Event(15, Level = EventLevel.Error, Keywords = Keywords.Logging, Task = Tasks.LogMessage)]
        public void LogMessageError(int logLevel, string message)
        {
            WriteEvent(15, logLevel, message ?? string.Empty);
        }
    }
}
