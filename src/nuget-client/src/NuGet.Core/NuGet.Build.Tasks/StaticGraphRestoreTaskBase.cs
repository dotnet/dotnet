// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#if !IS_CORECLR

using System.Reflection;

#endif

using System.Threading;
using Microsoft.Build.Framework;
using NuGet.Common;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents a base class for tasks that use the out-of-proc NuGet.Build.Tasks.Console to perform restore operations with MSBuild's static graph.
    /// </summary>
    public abstract class StaticGraphRestoreTaskBase : Microsoft.Build.Utilities.Task, ICancelableTask, IDisposable
    {
        /// <summary>
        /// Gets the full path to this assembly.
        /// </summary>
        protected static readonly Lazy<FileInfo> ThisAssemblyLazy = new Lazy<FileInfo>(() => new FileInfo(typeof(RestoreTaskEx).Assembly.Location));

        internal readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Gets a value indicating whether or not <see cref="SolutionPath" /> contains a value.
        /// </summary>
        public bool IsSolutionPathDefined => !string.IsNullOrWhiteSpace(SolutionPath) && !string.Equals(SolutionPath, "*Undefined*", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets the full path to the directory containing MSBuild.
        /// </summary>
        [Required]
        public string MSBuildBinPath { get; set; }

        /// <summary>
        /// MSBuildStartupDirectory - Used to calculate relative paths
        /// </summary>
        public string MSBuildStartupDirectory { get; set; }

        /// <summary>
        /// The path to the file to start the additional process with.
        /// </summary>
        public string ProcessFileName { get; set; }

        /// <summary>
        /// Gets or sets the full path to the current project file.
        /// </summary>
        [Required]
        public string ProjectFullPath { get; set; }

        /// <summary>
        /// Get or sets a value indicating whether or not the restore should restore all projects or just the entry project.
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// Gets or sets the full path to the solution file (if any) that is being built.
        /// </summary>
        public string SolutionPath { get; set; }

        protected abstract string DebugEnvironmentVariableName { get; }

        /// <inheritdoc cref="ICancelableTask.Cancel" />
        public void Cancel() => _cancellationTokenSource.Cancel();

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        public override bool Execute()
        {
            try
            {
#if DEBUG
                if (string.Equals(Environment.GetEnvironmentVariable(DebugEnvironmentVariableName), bool.TrueString, StringComparison.OrdinalIgnoreCase))
                {
                    Debugger.Launch();
                }
#endif
                MSBuildLogger logger = new MSBuildLogger(Log);

                using (var semaphore = new SemaphoreSlim(initialCount: 0, maxCount: 1))
                using (var loggingQueue = new TaskLoggingQueue(Log))
                using (var process = new Process())
                {
                    process.EnableRaisingEvents = true;
                    process.StartInfo = new ProcessStartInfo
                    {
                        Arguments = GetCommandLineArguments(MSBuildBinPath),
                        CreateNoWindow = true,
                        FileName = GetProcessFileName(ProcessFileName),
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        WorkingDirectory = Environment.CurrentDirectory,
                    };

                    // Place the output in the queue which handles logging messages coming through on StdOut
                    process.OutputDataReceived += (sender, args) => loggingQueue.Enqueue(args?.Data);

                    process.Exited += (sender, args) => semaphore.Release();

                    try
                    {
                        Log.LogMessage(MessageImportance.Low, "Running command: \"{0}\" {1}", process.StartInfo.FileName, process.StartInfo.Arguments);

                        process.Start();

                        WriteArguments(process.StandardInput);

                        process.BeginOutputReadLine();

                        semaphore.Wait(_cancellationTokenSource.Token);

                        if (!process.HasExited)
                        {
                            try
                            {
                                process.Kill();
                            }
                            catch (InvalidOperationException)
                            {
                                // The process may have exited, in this case ignore the exception
                            }
                        }
                    }
                    catch (Exception e) when (
                        e is OperationCanceledException
                        || (e is AggregateException aggregateException && aggregateException.InnerException is OperationCanceledException))
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
            }

            return !Log.HasLoggedErrors;
        }

        /// <summary>
        /// Gets the command-line arguments to use when launching the process that executes the restore.
        /// </summary>
        internal string GetCommandLineArguments(string msbuildBinPath)
        {
            return string.Concat(
#if IS_CORECLR
                "\"", Path.Combine(ThisAssemblyLazy.Value.DirectoryName, Path.ChangeExtension(ThisAssemblyLazy.Value.Name, ".Console.dll")), "\"",
                " \"", Path.Combine(msbuildBinPath, "MSBuild.dll"), "\"",
#else
                "\"", Path.Combine(msbuildBinPath, "MSBuild.exe"), "\"",
#endif
                " \"", IsSolutionPathDefined ? SolutionPath : ProjectFullPath, "\"");
        }

        /// <summary>
        /// Enumerates a list of global properties for the current MSBuild instance.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}" /> containing global properties.</returns>
        internal virtual Dictionary<string, string> GetGlobalProperties()
        {
            IReadOnlyDictionary<string, string> globalProperties = null;

#if IS_CORECLR
            // MSBuild 16.5 and above has a method to get the global properties, older versions do not
            if (BuildEngine is IBuildEngine6 buildEngine6)
            {
                globalProperties = buildEngine6.GetGlobalProperties();
            }
#else
            // MSBuild 16.5 added a new interface, IBuildEngine6, which has a GetGlobalProperties() method.  However, we compile against
            // Microsoft.Build.Framework version 4.0 when targeting .NET Framework, so reflection is required since type checking
            // can't be done at compile time
            Type buildEngine6Type = typeof(IBuildEngine).Assembly.GetType("Microsoft.Build.Framework.IBuildEngine6");

            if (buildEngine6Type != null)
            {
                MethodInfo getGlobalPropertiesMethod = buildEngine6Type.GetMethod("GetGlobalProperties", BindingFlags.Instance | BindingFlags.Public);

                if (getGlobalPropertiesMethod != null)
                {
                    try
                    {
                        globalProperties = getGlobalPropertiesMethod.Invoke(BuildEngine, parameters: null) as IReadOnlyDictionary<string, string>;
                    }
                    catch (Exception)
                    {
                        // Ignored
                    }
                }
            }
#endif
            Dictionary<string, string> newGlobalProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (globalProperties != null)
            {
                foreach (KeyValuePair<string, string> item in globalProperties)
                {
                    newGlobalProperties[item.Key] = item.Value;
                }
            }

            newGlobalProperties["ExcludeRestorePackageImports"] = bool.TrueString;
            newGlobalProperties["OriginalMSBuildStartupDirectory"] = MSBuildStartupDirectory;

            if (IsSolutionPathDefined)
            {
                newGlobalProperties["SolutionPath"] = SolutionPath;
            }

            return newGlobalProperties;
        }

        /// <summary>
        /// Gets the file name of the process.
        /// </summary>
        /// <param name="processFileName">An optional process filename to use as an override.</param>
        /// <returns>The full path to the file for the process.</returns>
        internal string GetProcessFileName(string processFileName)
        {
            if (!string.IsNullOrEmpty(processFileName))
            {
                return Path.GetFullPath(processFileName);
            }
#if IS_CORECLR
            // In .NET Core, the path to dotnet is the file to run
            return Path.GetFullPath(Path.Combine(MSBuildBinPath, "..", "..", "dotnet"));
#else
            return Path.Combine(ThisAssemblyLazy.Value.DirectoryName, Path.ChangeExtension(ThisAssemblyLazy.Value.Name, ".Console.exe"));
#endif
        }

        protected virtual void Dispose(bool disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        protected virtual Dictionary<string, string> GetOptions()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [nameof(Recursive)] = Recursive.ToString()
            };
        }

        internal void WriteArguments(StreamWriter streamWriter)
        {
            var arguments = new StaticGraphRestoreArguments
            {
                GlobalProperties = GetGlobalProperties(),
                Options = GetOptions(),
            };

            arguments.Write(streamWriter);
        }
    }
}
