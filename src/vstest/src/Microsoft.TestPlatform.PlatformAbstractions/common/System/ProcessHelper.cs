// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
#if !NET5_0_OR_GREATER
using System.Threading.Tasks;
#endif

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

/// <summary>
/// Helper class to deal with process related functionality.
/// </summary>
public partial class ProcessHelper : IProcessHelper
{
    private static readonly string Arm = "arm";
    private readonly Process _currentProcess = Process.GetCurrentProcess();

#if !NET5_0_OR_GREATER
    private readonly IEnvironment _environment;
#endif

    /// <summary>
    /// Default constructor.
    /// </summary>
    public ProcessHelper() : this(new PlatformEnvironment())
    {
    }

    internal ProcessHelper(IEnvironment environment)
    {
#if !NET5_0_OR_GREATER
        _environment = environment;
#endif
    }

    /// <summary>
    /// Gets or sets the set of environment variables to be used when spawning a new process.
    /// Should this set of environment variables be null, the environment variables inherited from
    /// the parent process will be used.
    /// </summary>
    internal static IDictionary<string, string?>? ExternalEnvironmentVariables { get; set; }

    /// <inheritdoc/>
    public object LaunchProcess(string processPath, string? arguments, string? workingDirectory, IDictionary<string, string?>? envVariables, Action<object?, string?>? errorCallback, Action<object?>? exitCallBack, Action<object?, string?>? outputCallBack)
    {
        if (!File.Exists(processPath))
        {
            throw new FileNotFoundException("Path does not exist: " + processPath, processPath);
        }

        var process = new Process();
        try
        {
            InitializeAndStart();
        }
        catch (Exception)
        {
            process.Dispose();

            //EqtTrace.Error("TestHost Object {0} failed to launch with the following exception: {1}", processPath, exception.Message);
            throw;
        }

        return process;

        // Local functions
        void InitializeAndStart()
        {
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = workingDirectory;

            process.StartInfo.FileName = processPath;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardError = true;

            process.EnableRaisingEvents = true;

            // When vstest.console is started in its own process in VisualStudio it is TestWindowStoreHost that starts it.
            // TestWindowStoreHost inherits environment variables from ServiceHost and DevEnv. Those env variables,
            // contain multiple "internal" environment variables, and they also contain DOTNET_ROOT pointing to the 
            // .NET that is shipped with VisualStudio. So to work around this, vstest.console is given a set of environment
            // variables that has only variables that DevEnv was started with. So it gets a "clean" set of env variables.
            //
            // When we run vstest.console in process, we cannot start ourselves with the same clean set of env variables,
            // and the best we can do is to start our child processes (testhost / datacollector) with this environment.
            // To do that we pass that set of "clean" env variables down to the ProcessHelper, and use those instead
            // of all the variables that are set in the current process.
            if (ExternalEnvironmentVariables is not null)
            {
                process.StartInfo.EnvironmentVariables.Clear();
                foreach (var kvp in ExternalEnvironmentVariables)
                {
                    if (kvp.Value is null)
                    {
                        continue;
                    }

                    process.StartInfo.AddEnvironmentVariable(kvp.Key, kvp.Value);
                }
            }

            // Set additional environment variables.
            if (envVariables != null)
            {
                foreach (var kvp in envVariables)
                {
                    process.StartInfo.AddEnvironmentVariable(kvp.Key, kvp.Value);
                }
            }

            if (outputCallBack != null)
            {
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sender, args) => outputCallBack(sender as Process, args.Data);
            }

            if (errorCallback != null)
            {
                process.ErrorDataReceived += (sender, args) => errorCallback(sender as Process, args.Data);
            }

            if (exitCallBack != null)
            {
                process.Exited += async (sender, args) =>
                {
                    const int timeout = 500;

                    if (sender is Process p)
                    {
                        try
                        {
                            // NOTE: When receiving an exit event, we want to give some time to the child process
                            // to close properly (i.e. flush output, error stream...). Despite this simple need,
                            // the actual implementation needs to be complex, especially for Unix systems.
                            // See ticket https://github.com/microsoft/vstest/issues/3375 to get the links to all
                            // issues, discussions and documentations.
                            //
                            // On .NET 5 and later, the solution is simple, we can simply use WaitForExitAsync which
                            // correctly ensure that some time is given to the child process (or any grandchild) to
                            // flush before exit happens.
                            //
                            // For older frameworks, the solution is more tricky but it seems we can get the expected
                            // behavior using the parameterless 'WaitForExit()' combined with an awaited Task.Run call.
                            var cts = new CancellationTokenSource(timeout);
#if NET5_0_OR_GREATER
                            await p.WaitForExitAsync(cts.Token);
#else
                            // NOTE: In case we run on Windows we must call 'WaitForExit(timeout)' instead of calling
                            // the parameterless overload. The reason for this requirement stems from the behavior of
                            // the Selenium WebDriver when debugging a test. If the debugger is detached, the default
                            // action is to kill the testhost process that it was attached to, but for some reason we
                            // end up with a zombie process that would make us wait indefinitely with a simple
                            // 'WaitForExit()' call. This in turn causes the vstest.console to block waiting for the
                            // test request to finish and this behavior will be visible to the user since TW will
                            // show the Selenium test as still running. Only killing the Edge Driver process would help
                            // unblock vstest.console, but this is not a reasonable ask to our users.
                            //
                            // TODO: This fix is not ideal, it's only a workaround to make Selenium tests usable again.
                            // Ideally, we should spend some more time here in order to better understand what causes
                            // the testhost to become a zombie process in the first place.
                            if (_environment.OperatingSystem is PlatformOperatingSystem.Windows)
                            {
                                p.WaitForExit(timeout);
                            }
                            else
                            {
                                cts.Token.Register(() =>
                                {
                                    try
                                    {
                                        if (!p.HasExited)
                                        {
                                            p.Kill();
                                        }
                                    }
                                    catch
                                    {
                                        // Ignore all exceptions thrown when trying to kill a process that may be
                                        // left hanging. This is a best effort to kill it, but should we fail for
                                        // any reason we'd probably block on 'WaitForExit()' anyway.
                                    }
                                });
                                await Task.Run(() => p.WaitForExit(), cts.Token).ConfigureAwait(false);
                            }
#endif
                        }
                        catch
                        {
                            // Ignore all exceptions thrown when asking for process to exit.
                            // We "expect" TaskCanceledException, COMException (if process was disposed before calling
                            // the exit) or InvalidOperationException.
                        }
                    }

                    // If exit callback has code that access Process object, ensure that the exceptions handling should be done properly.
                    exitCallBack(sender);
                };
            }

            // EqtTrace.Verbose("ProcessHelper: Starting process '{0}' with command line '{1}'", processPath, arguments);
            // TODO: Enable logging here, and consider wrapping Win32Exception into another that shows the path of the process.
            process.Start();

            if (errorCallback != null)
            {
                process.BeginErrorReadLine();
            }

            if (outputCallBack != null)
            {
                process.BeginOutputReadLine();
            }
        }
    }

    /// <inheritdoc/>
    public string? GetCurrentProcessFileName()
    {
        return _currentProcess.MainModule?.FileName;
    }

    /// <inheritdoc/>
    public string? GetTestEngineDirectory()
    {
        return Path.GetDirectoryName(typeof(ProcessHelper).Assembly.Location);
    }

    /// <inheritdoc/>
    public int GetCurrentProcessId()
    {
        return _currentProcess.Id;
    }

    /// <inheritdoc/>
    public string GetProcessName(int processId)
    {
        if (processId == _currentProcess.Id)
        {
            return _currentProcess.ProcessName;
        }

        return Process.GetProcessById(processId).ProcessName;
    }

    /// <inheritdoc/>
    public bool TryGetExitCode(object? process, out int exitCode)
    {
        try
        {
            if (process is Process proc && proc.HasExited)
            {
                exitCode = proc.ExitCode;
                return true;
            }
        }
        catch (InvalidOperationException)
        {
        }

        exitCode = 0;
        return false;
    }

    /// <inheritdoc/>
    public void SetExitCallback(int processId, Action<object?>? callbackAction)
    {
        try
        {
            var process = processId == _currentProcess.Id ? _currentProcess : Process.GetProcessById(processId);
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => callbackAction?.Invoke(sender);
        }
        catch (ArgumentException)
        {
            // Process.GetProcessById() throws ArgumentException if process is not running(identifier might be expired).
            // Invoke callback immediately.
            callbackAction?.Invoke(null);
        }
    }

    /// <inheritdoc/>
    public void TerminateProcess(object? process)
    {
        try
        {
            if (process is Process proc && !proc.HasExited)
            {
                proc.Kill();
            }
        }
        catch (InvalidOperationException)
        {
        }
    }

    /// <inheritdoc/>
    public int GetProcessId(object? process)
    {
        var proc = process as Process;
        return proc?.Id ?? -1;
    }

    /// <inheritdoc/>
    public string GetNativeDllDirectory()
    {
        var osArchitecture = new PlatformEnvironment().Architecture;
        return osArchitecture is PlatformArchitecture.ARM or PlatformArchitecture.ARM64
            ? Path.Combine(GetCurrentProcessLocation(), GetFormattedCurrentProcessArchitecture(), Arm)
            : Path.Combine(GetCurrentProcessLocation(), GetFormattedCurrentProcessArchitecture());
    }

    private string GetFormattedCurrentProcessArchitecture()
        => GetCurrentProcessArchitecture().ToString()
            .ToLower(
#if !NETCOREAPP1_0
        CultureInfo.InvariantCulture
#endif
            );

    /// <inheritdoc/>
    public void WaitForProcessExit(object? process)
    {
        if (process is Process proc && !proc.HasExited)
        {
            proc.WaitForExit();
        }
    }
}

#endif
