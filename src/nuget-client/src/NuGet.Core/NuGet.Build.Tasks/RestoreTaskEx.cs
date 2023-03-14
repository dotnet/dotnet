// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
#if !IS_CORECLR
using System.Reflection;
#endif
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents an MSBuild task that performs a command-line based restore.
    /// </summary>
    public sealed class RestoreTaskEx : Microsoft.Build.Utilities.Task, ICancelableTask, IDisposable
    {
        internal readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Gets the full path to this assembly.
        /// </summary>
        private static readonly Lazy<FileInfo> ThisAssemblyLazy = new Lazy<FileInfo>(() => new FileInfo(typeof(RestoreTaskEx).Assembly.Location));

        /// <summary>
        /// Gets or sets a value indicating whether or not assets should be deleted for projects that don't support PackageReference.
        /// </summary>
        public bool CleanupAssetsForUnsupportedProjects { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not parallel restore should be enabled.
        /// Defaults to <code>false</code> if the current machine only has a single processor.
        /// </summary>
        public bool DisableParallel { get; set; } = Environment.ProcessorCount == 1;

        /// <summary>
        /// Gets or sets a value indicating whether or not, in PackageReference based projects, all dependencies should be resolved
        /// even if the last restore was successful. Specifying this flag is similar to deleting the project.assets.json file. This
        /// does not bypass the http-cache.
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to recompute the dependencies and update the lock file without any warning.
        /// </summary>
        public bool ForceEvaluate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not warnings and errors should be logged.
        /// </summary>
        public bool HideWarningsAndErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to ignore failing or missing package sources.
        /// </summary>
        public bool IgnoreFailedSources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the restore is allowed to interact with the user through a prompt or dialog.
        /// </summary>
        public bool Interactive { get; set; }

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
        /// Gets or sets a value indicating whether or not to avoid using cached packages.
        /// </summary>
        public bool NoCache { get; set; }

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
        /// Gets or sets a value indicating whether or not to restore projects using packages.config.
        /// </summary>
        public bool RestorePackagesConfig { get; set; }

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

        [Output]
        public ITaskItem[] EmbedInBinlog { get; set; }

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
                var debugRestoreTask = Environment.GetEnvironmentVariable("DEBUG_RESTORE_TASK_EX");
                if (!string.IsNullOrEmpty(debugRestoreTask) && debugRestoreTask.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
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

                        EmbedInBinlog = loggingQueue.FilesToEmbedInBinlog.Select(i => new TaskItem(i)).ToArray();
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

            var options = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
            {
                [nameof(CleanupAssetsForUnsupportedProjects)] = CleanupAssetsForUnsupportedProjects,
                [nameof(DisableParallel)] = DisableParallel,
                [nameof(Force)] = Force,
                [nameof(ForceEvaluate)] = ForceEvaluate,
                [nameof(HideWarningsAndErrors)] = HideWarningsAndErrors,
                [nameof(IgnoreFailedSources)] = IgnoreFailedSources,
                [nameof(Interactive)] = Interactive,
                [nameof(NoCache)] = NoCache,
                [nameof(Recursive)] = Recursive,
                [nameof(RestorePackagesConfig)] = RestorePackagesConfig,
            };

            // Semicolon delimited list of options
            yield return string.Join(";", options.Where(i => i.Value).Select(i => $"{i.Key}={i.Value}"));

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
            yield return string.Join(";", GetGlobalProperties().Select(i => $"{i.Key}={i.Value}"));
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

        /// <summary>
        /// Enumerates a list of global properties for the current MSBuild instance.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="KeyValuePair{TKey, TValue}" /> objects containing global properties.</returns>
        internal IEnumerable<KeyValuePair<string, string>> GetGlobalProperties()
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
            if (globalProperties != null)
            {
                foreach (KeyValuePair<string, string> item in globalProperties)
                {
                    yield return item;
                }
            }

            yield return new KeyValuePair<string, string>("ExcludeRestorePackageImports", bool.TrueString);

            yield return new KeyValuePair<string, string>("OriginalMSBuildStartupDirectory", MSBuildStartupDirectory);

            if (IsSolutionPathDefined)
            {
                yield return new KeyValuePair<string, string>("SolutionPath", SolutionPath);
            }
        }
    }
}
