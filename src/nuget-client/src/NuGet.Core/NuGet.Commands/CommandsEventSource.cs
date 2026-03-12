// Copyright (c) .NET Foundation. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics.Tracing;

namespace NuGet.Commands
{
    [EventSource(Name = "Microsoft-NuGet-Commands")]
    internal sealed class CommandsEventSource : EventSource
    {
        public static readonly CommandsEventSource Instance = new();

        private CommandsEventSource() { }

        public static class Keywords
        {
            public const EventKeywords Performance = (EventKeywords)8;
            public const EventKeywords Restore = (EventKeywords)32;
        }

        public static class Tasks
        {
            public const EventTask RestoreCommand_BuildAssetsFile = (EventTask)1;
            public const EventTask RestoreCommand_BuildRestoreGraph = (EventTask)2;
            public const EventTask RestoreCommand_CalcNoOpRestore = (EventTask)3;
            public const EventTask RestoreRunner_RestoreProject = (EventTask)4;
            public const EventTask RestoreRunner_CommitAsync = (EventTask)5;
            public const EventTask RestoreResult_WriteAssetsFile = (EventTask)6;
            public const EventTask RestoreResult_WriteCacheFile = (EventTask)7;
            public const EventTask RestoreResult_WritePackagesLockFile = (EventTask)8;
            public const EventTask RestoreResult_WriteDgSpecFile = (EventTask)9;
            public const EventTask DependencyGraphResolver_CreateRestoreTargetGraph = (EventTask)10;
            public const EventTask DependencyGraphResolver_ResolveDependencyGraphItems = (EventTask)11;
        }

        // RestoreCommand/BuildAssetsFile (IDs 1-2)

        [Event(1, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.RestoreCommand_BuildAssetsFile, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreCommand_BuildAssetsFileStart(string filePath)
        {
            WriteEvent(1, filePath ?? string.Empty);
        }

        [Event(2, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.RestoreCommand_BuildAssetsFile, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreCommand_BuildAssetsFileStop(string filePath)
        {
            WriteEvent(2, filePath ?? string.Empty);
        }

        // RestoreCommand/BuildRestoreGraph (IDs 3-4)

        [Event(3, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.RestoreCommand_BuildRestoreGraph, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreCommand_BuildRestoreGraphStart(string filePath)
        {
            WriteEvent(3, filePath ?? string.Empty);
        }

        [Event(4, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.RestoreCommand_BuildRestoreGraph, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreCommand_BuildRestoreGraphStop(string filePath)
        {
            WriteEvent(4, filePath ?? string.Empty);
        }

        // RestoreCommand/CalcNoOpRestore (IDs 5-6)

        [Event(5, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.RestoreCommand_CalcNoOpRestore, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreCommand_CalcNoOpRestoreStart(string filePath)
        {
            WriteEvent(5, filePath ?? string.Empty);
        }

        [Event(6, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.RestoreCommand_CalcNoOpRestore, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreCommand_CalcNoOpRestoreStop(string filePath)
        {
            WriteEvent(6, filePath ?? string.Empty);
        }

        // RestoreRunner/RestoreProject (IDs 7-8)

        [Event(7, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.RestoreRunner_RestoreProject, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreRunner_RestoreProjectStart(string filePath)
        {
            WriteEvent(7, filePath ?? string.Empty);
        }

        [Event(8, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.RestoreRunner_RestoreProject, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreRunner_RestoreProjectStop(string filePath)
        {
            WriteEvent(8, filePath ?? string.Empty);
        }

        // RestoreRunner/CommitAsync (IDs 9-10)

        [Event(9, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.RestoreRunner_CommitAsync, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreRunner_CommitAsyncStart(string filePath)
        {
            WriteEvent(9, filePath ?? string.Empty);
        }

        [Event(10, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.RestoreRunner_CommitAsync, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreRunner_CommitAsyncStop(string filePath)
        {
            WriteEvent(10, filePath ?? string.Empty);
        }

        // RestoreResult/WriteAssetsFile (IDs 11-12)

        [Event(11, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.RestoreResult_WriteAssetsFile, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreResult_WriteAssetsFileStart(string filePath)
        {
            WriteEvent(11, filePath ?? string.Empty);
        }

        [Event(12, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.RestoreResult_WriteAssetsFile, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreResult_WriteAssetsFileStop(string filePath)
        {
            WriteEvent(12, filePath ?? string.Empty);
        }

        // RestoreResult/WriteCacheFile (IDs 13-14)

        [Event(13, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.RestoreResult_WriteCacheFile, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreResult_WriteCacheFileStart(string filePath)
        {
            WriteEvent(13, filePath ?? string.Empty);
        }

        [Event(14, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.RestoreResult_WriteCacheFile, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreResult_WriteCacheFileStop(string filePath)
        {
            WriteEvent(14, filePath ?? string.Empty);
        }

        // RestoreResult/WritePackagesLockFile (IDs 15-16)

        [Event(15, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.RestoreResult_WritePackagesLockFile, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreResult_WritePackagesLockFileStart(string filePath)
        {
            WriteEvent(15, filePath ?? string.Empty);
        }

        [Event(16, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.RestoreResult_WritePackagesLockFile, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreResult_WritePackagesLockFileStop(string filePath)
        {
            WriteEvent(16, filePath ?? string.Empty);
        }

        // RestoreResult/WriteDgSpecFile (IDs 17-18)

        [Event(17, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.RestoreResult_WriteDgSpecFile, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreResult_WriteDgSpecFileStart(string filePath)
        {
            WriteEvent(17, filePath ?? string.Empty);
        }

        [Event(18, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.RestoreResult_WriteDgSpecFile, ActivityOptions = EventActivityOptions.Detachable)]
        public void RestoreResult_WriteDgSpecFileStop(string filePath)
        {
            WriteEvent(18, filePath ?? string.Empty);
        }

        // DependencyGraphResolver/CreateRestoreTargetGraph (IDs 19-20)

        [Event(19, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.DependencyGraphResolver_CreateRestoreTargetGraph, ActivityOptions = EventActivityOptions.Detachable)]
        public void DependencyGraphResolver_CreateRestoreTargetGraphStart(string filePath, string frameworkRuntimeDefinition)
        {
            WriteEvent(19, filePath ?? string.Empty, frameworkRuntimeDefinition ?? string.Empty);
        }

        [Event(20, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.DependencyGraphResolver_CreateRestoreTargetGraph, ActivityOptions = EventActivityOptions.Detachable)]
        public void DependencyGraphResolver_CreateRestoreTargetGraphStop(string filePath, string frameworkRuntimeDefinition, int success, int resolvedPackageCount, int unresolvedPackageCount)
        {
            WriteEvent(20, filePath ?? string.Empty, frameworkRuntimeDefinition ?? string.Empty, success, resolvedPackageCount, unresolvedPackageCount);
        }

        // DependencyGraphResolver/ResolveDependencyGraphItems (IDs 21-22)

        [Event(21, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Start, Task = Tasks.DependencyGraphResolver_ResolveDependencyGraphItems, ActivityOptions = EventActivityOptions.Detachable)]
        public void DependencyGraphResolver_ResolveDependencyGraphItemsStart(string filePath, string frameworkRuntimeDefinition)
        {
            WriteEvent(21, filePath ?? string.Empty, frameworkRuntimeDefinition ?? string.Empty);
        }

        [Event(22, Level = EventLevel.Informational, Keywords = Keywords.Performance | Keywords.Restore, Opcode = EventOpcode.Stop, Task = Tasks.DependencyGraphResolver_ResolveDependencyGraphItems, ActivityOptions = EventActivityOptions.Detachable)]
        public void DependencyGraphResolver_ResolveDependencyGraphItemsStop(string filePath, string frameworkRuntimeDefinition, int resolvedPackagesCount, int restartCount, int totalQueuedItemCount)
        {
            WriteEvent(22, filePath ?? string.Empty, frameworkRuntimeDefinition ?? string.Empty, resolvedPackagesCount, restartCount, totalQueuedItemCount);
        }
    }
}
