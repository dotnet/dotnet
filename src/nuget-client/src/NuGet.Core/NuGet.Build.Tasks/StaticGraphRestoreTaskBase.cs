// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
#if !IS_CORECLR
using System.Reflection;
#endif
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents a base class for tasks that use the out-of-proc NuGet.Build.Tasks.Console to perform restore operations with MSBuild's static graph.
    /// </summary>
    public abstract class StaticGraphRestoreTaskBase : Microsoft.Build.Utilities.Task, ICancelableTask, IDisposable
    {
        internal readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        protected StaticGraphRestoreTaskBase()
            : base(Strings.ResourceManager)
        {
        }

        /// <summary>
        /// Gets the full path to this assembly.
        /// </summary>
        protected static readonly Lazy<FileInfo> ThisAssemblyLazy = new Lazy<FileInfo>(() => new FileInfo(typeof(RestoreTaskEx).Assembly.Location));

        [Output]
        public ITaskItem[] EmbedInBinlog { get; set; }

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
        /// Gets or sets a value indicating whether or not the global properties should be sent to NuGet.Build.Tasks.Console.exe over the standard input stream.
        /// </summary>
        public bool SerializeGlobalProperties { get; set; }

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

                Dictionary<string, string> globalProperties = GetGlobalProperties();

                Encoding utf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

                using (var semaphore = new SemaphoreSlim(initialCount: 0, maxCount: 1))
                using (var loggingQueue = new TaskLoggingQueue(Log))
                using (var process = new Process())
                {
                    bool errorLogged = false;

                    process.EnableRaisingEvents = true;
                    process.StartInfo = new ProcessStartInfo
                    {
                        Arguments = GetCommandLineArguments(globalProperties),
                        CreateNoWindow = true,
                        FileName = GetProcessFileName(ProcessFileName),
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
#if !NETFRAMEWORK
                        StandardInputEncoding = utf8Encoding,
#endif
                        UseShellExecute = false,
                        WorkingDirectory = Environment.CurrentDirectory,
                    };

                    // Place the output in the queue which handles logging messages coming through on StdOut
                    process.OutputDataReceived += (sender, args) => loggingQueue.Enqueue(args?.Data);

                    process.ErrorDataReceived += (_, args) =>
                    {
                        if (args.Data != null)
                        {
                            // Any message on the standard error stream should be logged as an error
                            Log.LogError(args.Data);

                            errorLogged = true;
                        }
                    };

                    process.Exited += (sender, args) => semaphore.Release();

                    Log.LogMessageFromResources(MessageImportance.Low, nameof(Strings.Log_RunningStaticGraphRestoreCommand), process.StartInfo.FileName, process.StartInfo.Arguments);

                    Encoding previousConsoleInputEncoding = Console.InputEncoding;

                    // Set the input encoding to UTF8 without a byte order mark, the spawned process will use this encoding on .NET Framework
                    Console.InputEncoding = utf8Encoding;

                    try
                    {
                        process.Start();
                    }
                    catch (Exception e)
                    {
                        Log.LogErrorFromResources(nameof(Strings.Error_StaticGraphRestoreFailedToStart), e.Message);

                        return false;
                    }
                    finally
                    {
                        Console.InputEncoding = previousConsoleInputEncoding;
                    }

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    if (SerializeGlobalProperties)
                    {
                        using var writer = new BinaryWriter(process.StandardInput.BaseStream, utf8Encoding, leaveOpen: true);

                        WriteGlobalProperties(writer, globalProperties);
                    }

                    process.StandardInput.Close();

                    try
                    {
                        semaphore.Wait(_cancellationTokenSource.Token);
                    }
                    catch (Exception e) when (
                        e is OperationCanceledException
                        || (e is AggregateException aggregateException && aggregateException.InnerException is OperationCanceledException))
                    {
                        // Wait() throws when the cancellation token is triggered, this is expected so ignore the exception
                    }

                    if (!process.HasExited)
                    {
                        try
                        {
                            // Kill the process in the case of cancellation
                            process.Kill();
                        }
                        catch (InvalidOperationException)
                        {
                            // The process may have exited, in this case ignore the exception
                        }
                    }

                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        // Return true so the task "succeeds" in the case of cancellation so the build doesn't appear to fail
                        return true;
                    }

                    if (process.ExitCode > 0 && !Log.HasLoggedErrors && !errorLogged)
                    {
                        // All non-zero exit codes should have logged an error, if not its unexpected so log an error asking the user to file an issue
                        Log.LogErrorFromResources(nameof(Strings.Error_StaticGraphNonZeroExitCode), process.ExitCode);
                    }

                    EmbedInBinlog = loggingQueue.FilesToEmbedInBinlog.Select(i => new TaskItem(i)).ToArray();
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
        /// <param name="globalProperties">Receives a <see cref="Dictionary{TKey, TValue}" /> containing the global properties.</param>
        internal string GetCommandLineArguments(Dictionary<string, string> globalProperties)
        {
            // First get the command-line arguments including the global properties
            return CreateArgumentString(EnumerateCommandLineArguments(SerializeGlobalProperties ? null : globalProperties));

            IEnumerable<string> EnumerateCommandLineArguments(Dictionary<string, string> globalProperties)
            {
#if IS_CORECLR
                // The full path to the executable for dotnet core
                yield return Path.Combine(ThisAssemblyLazy.Value.DirectoryName, Path.ChangeExtension(ThisAssemblyLazy.Value.Name, ".Console.dll"));
#endif
                var options = GetOptions();

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

                // globalProperties is null when they will be serialized over the standard input stream instead
                if (globalProperties != null)
                {
                    // Semicolon delimited list of MSBuild global properties
                    yield return string.Join(";", globalProperties.Select(i => $"{i.Key}={i.Value}"));
                }
            }
        }

        /// <summary>
        /// Creates an argument string of space delimited quoted arguments.
        /// </summary>
        /// <param name="arguments">An <see cref="IEnumerable{T}" /> containing individual argument values.</param>
        /// <returns>A <see cref="string" /> with space delimited quoted arguments.</returns>
        internal static string CreateArgumentString(IEnumerable<string> arguments)
        {
            return "\"" + string.Join("\" \"", arguments) + "\"";
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

        /// <summary>
        /// Writes global properties to the specified stream.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter" /> to write the global properties to.</param>
        /// <param name="globalProperties">A <see cref="Dictionary{TKey, TValue}" /> containing the global properties.</param>
        internal static void WriteGlobalProperties(BinaryWriter writer, Dictionary<string, string> globalProperties)
        {
            writer.Write(globalProperties.Count);

            foreach (KeyValuePair<string, string> option in globalProperties)
            {
                writer.Write(option.Key);
                writer.Write(option.Value);
            }
        }
    }
}
