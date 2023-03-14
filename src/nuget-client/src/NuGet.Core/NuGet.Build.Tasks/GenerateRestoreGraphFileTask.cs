// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Packaging;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents an MSBuild task that performs a command-line based restore.
    /// </summary>
    public sealed class GenerateRestoreGraphFileTask : Microsoft.Build.Utilities.Task, ICancelableTask, IDisposable
    {
        internal readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Gets the full path to this assembly.
        /// </summary>
        private static readonly Lazy<FileInfo> ThisAssemblyLazy = new Lazy<FileInfo>(() => new FileInfo(typeof(GenerateRestoreGraphFileTask).Assembly.Location));

        /// <summary>
        /// Gets a value indicating whether or not <see cref="SolutionPath" /> contains a value.
        /// </summary>
        private bool IsSolutionPathDefined => !string.IsNullOrWhiteSpace(SolutionPath) && !string.Equals(SolutionPath, "*Undefined*", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets the full path to the directory containing MSBuild.
        /// </summary>
        [Required]
        public string MSBuildBinPath { get; set; }

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

        /// <summary>
        /// The path to the file to start the additional process with.
        /// </summary>
        public string ProcessFileName { get; set; }

        /// <summary>
        /// MSBuildStartupDirectory - Used to calculate relative paths
        /// </summary>
        public string MSBuildStartupDirectory { get; set; }

        /// <summary>
        /// RestoreGraphOutputPath - The location to write the output to.
        /// </summary>
        [Required]
        public string RestoreGraphOutputPath { get; set; }

        /// <inheritdoc cref="ICancelableTask.Cancel" />
        public void Cancel() => _cancellationTokenSource.Cancel();

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        /// <inheritdoc cref="Task.Execute()" />
        public override bool Execute()
        {
            try
            {
#if DEBUG
                var debugTask = Environment.GetEnvironmentVariable("DEBUG_GENERATE_RESTORE_GRAPH");
                if (!string.IsNullOrEmpty(debugTask) && debugTask.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
                {
                    Debugger.Launch();
                }
#endif
                using (var semaphore = new SemaphoreSlim(initialCount: 0, maxCount: 1))
                using (var loggingQueue = new TaskLoggingQueue(Log))
                using (var process = new Process())
                {
                    process.EnableRaisingEvents = true;
                    process.StartInfo = new ProcessStartInfo
                    {
                        Arguments = $"\"{string.Join("\" \"", GetCommandLineArguments())}\"",
                        CreateNoWindow = true,
                        FileName = GetProcessFileName(ProcessFileName),
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        WorkingDirectory = Environment.CurrentDirectory,
                    };

                    // Place the output in the queue which handles logging messages coming through on StdOut
                    process.OutputDataReceived += (sender, args) => loggingQueue.Enqueue(args?.Data);

                    process.Exited += (sender, args) => semaphore.Release();

                    try
                    {
                        Log.LogMessage(MessageImportance.Low, "\"{0}\" {1}", process.StartInfo.FileName, process.StartInfo.Arguments);

                        process.Start();

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
        /// <returns>An <see cref="IEnumerable{String}" /> containing the command-line arguments that need to separated by spaces and surrounded by quotes.</returns>
        internal IEnumerable<string> GetCommandLineArguments()
        {
#if IS_CORECLR
            // The full path to the executable for dotnet core
            yield return Path.Combine(ThisAssemblyLazy.Value.DirectoryName, Path.ChangeExtension(ThisAssemblyLazy.Value.Name, ".Console.dll"));
#endif

            var options = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["GenerateRestoreGraphFile"] = true.ToString(CultureInfo.CurrentCulture),
                [nameof(Recursive)] = Recursive.ToString(CultureInfo.CurrentCulture),
                [nameof(RestoreGraphOutputPath)] = RestoreGraphOutputPath,
            };
            // Semicolon delimited list of options
            yield return string.Join(";", options.Select(i => $"{i.Key}={i.Value}"));

            // Full path to MSBuild.exe or MSBuild.dll
#if IS_CORECLR
            yield return Path.Combine(MSBuildBinPath, "MSBuild.dll");
#else
            yield return Path.Combine(MSBuildBinPath, "MSBuild.exe");

#endif
            // Full path to the entry project.  If its a solution file, it will be the full path to solution, otherwise SolutionPath is either empty
            // or is the value "*Undefined*" and ProjectFullPath is set instead.
            yield return IsSolutionPathDefined
                    ? SolutionPath
                    : ProjectFullPath;

            // Semicolon delimited list of MSBuild global properties
            var globalProperties = GetGlobalProperties().Select(i => $"{i.Key}={i.Value}").Concat(new string[] { $"OriginalMSBuildStartupDirectory={MSBuildStartupDirectory}" });

            yield return string.Join(";", globalProperties);
        }

        /// <summary>
        /// Gets the file name of the process.
        /// </summary>
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

        internal Dictionary<string, string> GetGlobalProperties()
        {
#if IS_CORECLR
            // MSBuild 16.5 and above has a method to get the global properties, older versions do not
            Dictionary<string, string> msBuildGlobalProperties = BuildEngine is IBuildEngine6 buildEngine6
                ? buildEngine6.GetGlobalProperties().ToDictionary(i => i.Key, i => i.Value, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
#else
            var msBuildGlobalProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // MSBuild 16.5 added a new interface, IBuildEngine6, which has a GetGlobalProperties() method.  However, we compile against
            // Microsoft.Build.Framework version 4.0 when targeting .NET Framework, so reflection is required since type checking
            // can't be done at compile time
            var buildEngine6Type = typeof(IBuildEngine).Assembly.GetType("Microsoft.Build.Framework.IBuildEngine6");

            if (buildEngine6Type != null)
            {
                var getGlobalPropertiesMethod = buildEngine6Type.GetMethod("GetGlobalProperties", BindingFlags.Instance | BindingFlags.Public);

                if (getGlobalPropertiesMethod != null)
                {
                    try
                    {
                        if (getGlobalPropertiesMethod.Invoke(BuildEngine, null) is IReadOnlyDictionary<string, string> globalProperties)
                        {
                            msBuildGlobalProperties.AddRange(globalProperties);
                        }
                    }
                    catch (Exception)
                    {
                        // Ignored
                    }
                }
            }
#endif
            msBuildGlobalProperties["ExcludeRestorePackageImports"] = "true";

            if (IsSolutionPathDefined)
            {
                msBuildGlobalProperties["SolutionPath"] = SolutionPath;
            }

            return msBuildGlobalProperties;
        }
    }
}
