// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

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

        private readonly Dictionary<RegisteredTaskObjectLifetime, Dictionary<object, object>> _registeredTaskObjects =
            new Dictionary<RegisteredTaskObjectLifetime, Dictionary<object, object>>();

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
                return _registeredTaskObjects.TryGetValue(lifetime, out Dictionary<object, object> objects) && objects.TryGetValue(key, out object registered)
                    ? registered
                    : null;
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

        /// <summary>
        /// Registers a task object. As in MSBuild, an existing registration for the same key wins.
        /// </summary>
        public void RegisterTaskObject(object key, object obj, RegisteredTaskObjectLifetime lifetime, bool allowEarlyCollection)
        {
            lock (_registeredTaskObjects)
            {
                if (!_registeredTaskObjects.TryGetValue(lifetime, out Dictionary<object, object> objects))
                {
                    objects = new Dictionary<object, object>();
                    _registeredTaskObjects.Add(lifetime, objects);
                }

                if (!objects.ContainsKey(key))
                {
                    objects.Add(key, obj);
                }
            }
        }

        public object UnregisterTaskObject(object key, RegisteredTaskObjectLifetime lifetime)
        {
            lock (_registeredTaskObjects)
            {
                object registered = GetRegisteredTaskObject(key, lifetime);

                if (registered != null)
                {
                    _registeredTaskObjects[lifetime].Remove(key);
                }

                return registered;
            }
        }

        /// <summary>
        /// Disposes the objects registered with the given lifetime, as MSBuild's node does when a build ends.
        /// </summary>
        public void DisposeRegisteredTaskObjects(RegisteredTaskObjectLifetime lifetime)
        {
            List<object> objects;

            lock (_registeredTaskObjects)
            {
                if (!_registeredTaskObjects.TryGetValue(lifetime, out Dictionary<object, object> registered))
                {
                    return;
                }

                objects = new List<object>(registered.Values);
                registered.Clear();
            }

            foreach (object obj in objects)
            {
                (obj as IDisposable)?.Dispose();
            }
        }

        public void Yield() => throw new NotImplementedException();
    }
}
