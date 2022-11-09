// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using NuGet.Common;
using NuGet.Test.Utility;

namespace NuGet.Build.Tasks.Test
{
    /// <summary>
    /// MSBuild logger -> TestLogger
    /// </summary>
    public class TestBuildEngine : IBuildEngine6
    {
        /// <summary>
        /// Test logger
        /// </summary>
        public TestLogger TestLogger = new TestLogger();

        private readonly IReadOnlyDictionary<string, string> _globalProperties;

        public TestBuildEngine()
        {
            _globalProperties = new Dictionary<string, string>();
        }

        public TestBuildEngine(IReadOnlyDictionary<string, string> globalProperties)
        {
            _globalProperties = globalProperties;
        }
        public int ColumnNumberOfTaskNode => 0;

        public bool ContinueOnError => false;

        public bool IsRunningMultipleNodes { get; }

        public int LineNumberOfTaskNode => 0;

        public string ProjectFileOfTaskNode => string.Empty;

        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs) => true;

        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs, string toolsVersion) => throw new NotImplementedException();

        public bool BuildProjectFilesInParallel(string[] projectFileNames, string[] targetNames, IDictionary[] globalProperties, IDictionary[] targetOutputsPerProject, string[] toolsVersion, bool useResultsCache, bool unloadProjectsOnCompletion) => throw new NotImplementedException();

        public BuildEngineResult BuildProjectFilesInParallel(string[] projectFileNames, string[] targetNames, IDictionary[] globalProperties, IList<string>[] removeGlobalProperties, string[] toolsVersion, bool returnTargetOutputs) => throw new NotImplementedException();

        public IReadOnlyDictionary<string, string> GetGlobalProperties() => _globalProperties;

        public object GetRegisteredTaskObject(object key, RegisteredTaskObjectLifetime lifetime) => throw new NotImplementedException();

        public void LogCustomEvent(CustomBuildEventArgs e)
        {
            // ignored
        }

        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            var message = new RestoreLogMessage(LogLevel.Error, e.Message)
            {
                FilePath = e.File,
                ProjectPath = e.ProjectFile
            };

            if (!string.IsNullOrWhiteSpace(e.Code) && Enum.TryParse(e.Code, ignoreCase: true, out NuGetLogCode code))
            {
                message.Code = code;
            }

            TestLogger.Log(message);
        }

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            var level = LogLevel.Debug;

            if (e.Importance == MessageImportance.High)
            {
                level = LogLevel.Minimal;
            }

            if (e.Importance == MessageImportance.Normal)
            {
                level = LogLevel.Information;
            }

            var message = new RestoreLogMessage(level, e.Message)
            {
                FilePath = e.File,
                ProjectPath = e.ProjectFile
            };

            TestLogger.Log(message);
        }

        public void LogTelemetry(string eventName, IDictionary<string, string> properties) => throw new NotImplementedException();

        public void LogWarningEvent(BuildWarningEventArgs e)
        {
            var message = new RestoreLogMessage(LogLevel.Warning, e.Message)
            {
                FilePath = e.File,
                ProjectPath = e.ProjectFile
            };

            if (!string.IsNullOrWhiteSpace(e.Code) && Enum.TryParse(e.Code, ignoreCase: true, out NuGetLogCode code))
            {
                message.Code = code;
            }

            TestLogger.Log(message);
        }

        public void Reacquire() => throw new NotImplementedException();

        public void RegisterTaskObject(object key, object obj, RegisteredTaskObjectLifetime lifetime, bool allowEarlyCollection) => throw new NotImplementedException();

        public object UnregisterTaskObject(object key, RegisteredTaskObjectLifetime lifetime) => throw new NotImplementedException();

        public void Yield() => throw new NotImplementedException();
    }
}
