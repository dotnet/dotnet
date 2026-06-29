// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

        private readonly Dictionary<object, object> _registeredTaskObjects = new Dictionary<object, object>();

        /// <summary>
        /// Number of times <see cref="RegisterTaskObject" /> was called on this engine. The reset guard registers its
        /// build-lifetime sentinel exactly when it performs a reset, so this counts how many resets actually ran.
        /// </summary>
        public int RegisterTaskObjectCount;

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

        public object GetRegisteredTaskObject(object key, RegisteredTaskObjectLifetime lifetime)
        {
            lock (_registeredTaskObjects)
            {
                return _registeredTaskObjects.TryGetValue(key, out object value) ? value : null;
            }
        }

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

        public void RegisterTaskObject(object key, object obj, RegisteredTaskObjectLifetime lifetime, bool allowEarlyCollection)
        {
            lock (_registeredTaskObjects)
            {
                Interlocked.Increment(ref RegisterTaskObjectCount);
                _registeredTaskObjects[key] = obj;
            }
        }

        public object UnregisterTaskObject(object key, RegisteredTaskObjectLifetime lifetime)
        {
            lock (_registeredTaskObjects)
            {
                if (_registeredTaskObjects.TryGetValue(key, out object value))
                {
                    _registeredTaskObjects.Remove(key);
                    return value;
                }

                return null;
            }
        }

        public void Yield() => throw new NotImplementedException();
    }
}
