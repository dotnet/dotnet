// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.TestPlatform.TestHostProvider;
using Microsoft.TestPlatform.TestHostProvider.Hosting;
using Microsoft.TestPlatform.TestHostProvider.Resources;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers.Interfaces;
using Microsoft.VisualStudio.TestPlatform.DesktopTestHostRuntimeProvider;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;

/// <summary>
/// The default test host launcher for the engine.
/// This works for Desktop local scenarios
/// </summary>
[ExtensionUri(DefaultTestHostUri)]
[FriendlyName(DefaultTestHostFriendlyName)]
public class DefaultTestHostManager : ITestRuntimeProvider2
{
    private const string DefaultTestHostUri = "HostProvider://DefaultTestHost";
    // Should the friendly name ever change, please make sure to change the corresponding constant
    // inside ProxyOperationManager::IsTesthostCompatibleWithTestSessions().
    private const string DefaultTestHostFriendlyName = "DefaultTestHost";
    private const string TestAdapterEndsWithPattern = @"TestAdapter.dll";

    // Any version (older or newer) that is not in this list will use the default testhost.exe that is built using net462.
    // TODO: Add net481 when it is published, if it uses a new moniker.
    private static readonly ImmutableArray<string> SupportedTargetFrameworks = ImmutableArray.Create("net47", "net471", "net472", "net48");

    private readonly IProcessHelper _processHelper;
    private readonly IFileHelper _fileHelper;
    private readonly IEnvironment _environment;
    private readonly IDotnetHostHelper _dotnetHostHelper;
    private readonly IEnvironmentVariableHelper _environmentVariableHelper;
    private bool _disableAppDomain;
    private Architecture _architecture;
    private Framework? _targetFramework;
    private ITestHostLauncher? _customTestHostLauncher;
    private Process? _testHostProcess;
    private StringBuilder? _testHostProcessStdError;
    private StringBuilder? _testHostProcessStdOut;
    private IMessageLogger? _messageLogger;
    private bool _captureOutput;
    private bool _hostExitedEventRaised;
    private TestHostManagerCallbacks? _testHostManagerCallbacks;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultTestHostManager"/> class.
    /// </summary>
    public DefaultTestHostManager()
        : this(
            new ProcessHelper(),
            new FileHelper(),
            new DotnetHostHelper(),
            new PlatformEnvironment(),
            new EnvironmentVariableHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultTestHostManager"/> class.
    /// </summary>
    /// <param name="processHelper">Process helper instance.</param>
    /// <param name="fileHelper">File helper instance.</param>
    /// <param name="environment">Instance of platform environment.</param>
    /// <param name="environmentVariableHelper">The environment helper.</param>
    /// <param name="dotnetHostHelper">Instance of dotnet host helper.</param>
    internal DefaultTestHostManager(
        IProcessHelper processHelper,
        IFileHelper fileHelper,
        IDotnetHostHelper dotnetHostHelper,
        IEnvironment environment,
        IEnvironmentVariableHelper environmentVariableHelper)
    {
        _processHelper = processHelper;
        _fileHelper = fileHelper;
        _dotnetHostHelper = dotnetHostHelper;
        _environment = environment;
        _environmentVariableHelper = environmentVariableHelper;
    }

    /// <inheritdoc/>
    public event EventHandler<HostProviderEventArgs>? HostLaunched;

    /// <inheritdoc/>
    public event EventHandler<HostProviderEventArgs>? HostExited;

    /// <inheritdoc/>
    public bool Shared { get; private set; }

    /// <summary>
    /// Gets the properties of the test executor launcher. These could be the targetID for emulator/phone specific scenarios.
    /// </summary>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Part of the public API")]
    public IDictionary<string, string> Properties => new Dictionary<string, string>();

    /// <summary>
    /// Gets callback on process exit
    /// </summary>
    private Action<object?> ExitCallBack => process =>
    {
        TPDebug.Assert(_testHostProcessStdError is not null, "LaunchTestHostAsync must have been called before ExitCallBack");
        TPDebug.Assert(_testHostManagerCallbacks is not null, "Initialize must have been called before ExitCallBack");
        TestHostManagerCallbacks.ExitCallBack(_processHelper, process, _testHostProcessStdError, OnHostExited);
    };

    /// <summary>
    /// Gets callback to read from process error stream
    /// </summary>
    private Action<object?, string?> ErrorReceivedCallback => (process, data) =>
    {
        TPDebug.Assert(_testHostProcessStdError is not null, "LaunchTestHostAsync must have been called before ErrorReceivedCallback");
        TPDebug.Assert(_testHostManagerCallbacks is not null, "Initialize must have been called before ErrorReceivedCallback");
        _testHostManagerCallbacks.ErrorReceivedCallback(_testHostProcessStdError, data);
    };

    /// <summary>
    /// Gets callback to read from process standard stream
    /// </summary>
    private Action<object?, string?> OutputReceivedCallback => (process, data) =>
    {
        TPDebug.Assert(_testHostProcessStdOut is not null, "LaunchTestHostAsync must have been called before OutputReceivedCallback");
        TPDebug.Assert(_testHostManagerCallbacks is not null, "Initialize must have been called before OutputReceivedCallback");
        _testHostManagerCallbacks.StandardOutputReceivedCallback(_testHostProcessStdOut, data);
    };

    /// <inheritdoc/>
    public void SetCustomLauncher(ITestHostLauncher customLauncher)
    {
        _customTestHostLauncher = customLauncher;
    }

    /// <inheritdoc/>
    public TestHostConnectionInfo GetTestHostConnectionInfo()
    {
        return new TestHostConnectionInfo { Endpoint = "127.0.0.1:0", Role = ConnectionRole.Client, Transport = Transport.Sockets };
    }

    /// <inheritdoc/>
    public Task<bool> LaunchTestHostAsync(TestProcessStartInfo testHostStartInfo, CancellationToken cancellationToken)
    {
        // Do NOT offload this to thread pool using Task.Run, we already are on thread pool
        // and this would go into a queue after all the other startup tasks. Meaning we will start
        // testhost much later, and not immediately.
        return Task.FromResult(LaunchHost(testHostStartInfo, cancellationToken));
    }

    /// <inheritdoc/>
    public virtual TestProcessStartInfo GetTestHostProcessStartInfo(
        IEnumerable<string> sources,
        IDictionary<string, string?>? environmentVariables,
        TestRunnerConnectionInfo connectionInfo)
    {
        TPDebug.Assert(IsInitialized, "Initialize must have been called before GetTestHostProcessStartInfo");

        string testHostProcessName = GetTestHostName(_architecture, _targetFramework, _processHelper.GetCurrentProcessArchitecture());

        var currentWorkingDirectory = Path.GetDirectoryName(typeof(DefaultTestHostManager).Assembly.Location);
        var argumentsString = " " + connectionInfo.ToCommandLineOptions();

        TPDebug.Assert(currentWorkingDirectory is not null, "Current working directory must not be null.");

        // check in current location for testhost exe
        var testhostProcessPath = Path.Combine(currentWorkingDirectory, testHostProcessName);

        var originalTestHostProcessName = testHostProcessName;

        if (!_fileHelper.Exists(testhostProcessPath))
        {
            // We assume that we could not find testhost.exe in the root folder so we are going to lookup in the
            // TestHostNetFramework folder (assuming we are currently running under .NET) or in dotnet SDK
            // context.
            testHostProcessName = Path.Combine("TestHostNetFramework", originalTestHostProcessName);
            testhostProcessPath = Path.Combine(currentWorkingDirectory, "..", testHostProcessName);
        }

        if (_disableAppDomain)
        {
            // When host appdomains are disabled (in that case host is not shared) we need to pass the test assembly path as argument
            // so that the test host can create one appdomain on startup (Main method) and set appbase.
            argumentsString += " --testsourcepath " + sources.FirstOrDefault()?.AddDoubleQuote();
        }

        EqtTrace.Verbose("DefaultTestHostmanager.GetTestHostProcessStartInfo: Trying to use {0} from {1}", originalTestHostProcessName, testhostProcessPath);

        var launcherPath = testhostProcessPath;
        var processName = _processHelper.GetCurrentProcessFileName();
        if (processName is not null)
        {
            if (!_environment.OperatingSystem.Equals(PlatformOperatingSystem.Windows)
                && !processName.EndsWith(DotnetHostHelper.MONOEXENAME, StringComparison.OrdinalIgnoreCase))
            {
                launcherPath = _dotnetHostHelper.GetMonoPath();
                argumentsString = testhostProcessPath.AddDoubleQuote() + " " + argumentsString;
            }
            else
            {
                // Patching the relative path for IDE scenarios.
                if (_environment.OperatingSystem.Equals(PlatformOperatingSystem.Windows)
                    && !(processName.EndsWith("dotnet", StringComparison.OrdinalIgnoreCase)
                        || processName.EndsWith("dotnet.exe", StringComparison.OrdinalIgnoreCase))
                    && !File.Exists(testhostProcessPath))
                {
                    testhostProcessPath = Path.Combine(currentWorkingDirectory, "..", originalTestHostProcessName);
                    EqtTrace.Verbose("DefaultTestHostmanager.GetTestHostProcessStartInfo: Could not find {0} in previous location, now using {1}", originalTestHostProcessName, testhostProcessPath);
                    launcherPath = testhostProcessPath;
                }
            }
        }

        // For IDEs and other scenario, current directory should be the
        // working directory (not the vstest.console.exe location).
        // For VS - this becomes the solution directory for example
        // "TestResults" directory will be created at "current directory" of test host
        var processWorkingDirectory = Directory.GetCurrentDirectory();

        return new TestProcessStartInfo
        {
            FileName = launcherPath,
            Arguments = argumentsString,
            EnvironmentVariables = environmentVariables ?? new Dictionary<string, string?>(),
            WorkingDirectory = processWorkingDirectory
        };
    }

    private static string GetTestHostName(Architecture architecture, Framework targetFramework, PlatformArchitecture processArchitecture)
    {
        // We ship multiple executables for testhost that follow this naming schema:
        // testhost<.tfm><.architecture>.exe
        // e.g.: testhost.net472.x86.exe -> 32-bit testhost for .NET Framework 4.7.2
        //
        // The tfm is omitted for .NET Framework 4.5.1 testhost.
        // testhost.x86.exe -> 32-bit testhost for .NET Framework 4.5.1
        //
        // The architecture is omitted for 64-bit (x64) testhost.
        // testhost.net472.exe -> 64-bit testhost for .NET Framework 4.7.2
        // testhost.exe -> 64-bit testhost for .NET Framework 4.5.1
        //
        // These omissions are done for backwards compatibility because originally there were
        // only testhost.exe and testhost.x86.exe, both built against .NET Framework 4.5.1.

        StringBuilder testHostProcessName = new("testhost");

        if (targetFramework.Name.StartsWith(".NETFramework,Version=v"))
        {
            // Transform target framework name into moniker.
            // e.g. ".NETFramework,Version=v4.7.2" -> "net472".
            var targetFrameworkMoniker = "net" + targetFramework.Name.Replace(".NETFramework,Version=v", string.Empty).Replace(".", string.Empty);

            var isSupportedTargetFramework = SupportedTargetFrameworks.Contains(targetFrameworkMoniker);
            if (isSupportedTargetFramework)
            {
                testHostProcessName.Append('.').Append(targetFrameworkMoniker);
            }
            else
            {
                // The .NET Framework 4.5.1 testhost that does not have moniker in the name is used as fallback.
            }
        }

        var processArchitectureAsArchitecture = processArchitecture switch
        {
            PlatformArchitecture.X86 => Architecture.X86,
            PlatformArchitecture.X64 => Architecture.X64,
            PlatformArchitecture.ARM => Architecture.ARM,
            PlatformArchitecture.ARM64 => Architecture.ARM64,
            PlatformArchitecture.S390x => Architecture.S390x,
            PlatformArchitecture.Ppc64le => Architecture.Ppc64le,
            PlatformArchitecture.RiscV64 => Architecture.RiscV64,
            _ => throw new NotSupportedException(),
        };

        // Default architecture, or AnyCPU architecture will use the architecture of the current process,
        // so when you run from 32-bit vstest.console, or from 32-bit dotnet test, you will get 32-bit testhost
        // as the preferred testhost.
        var actualArchitecture = architecture is Architecture.Default or Architecture.AnyCPU
            ? processArchitectureAsArchitecture
            : architecture;

        if (actualArchitecture != Architecture.X64)
        {
            // Append .<architecture> to the name, such as .x86. It is possible that we are not shipping the
            // executable for the architecture with VS, and that will fail later with file not found exception,
            // which is okay.
            testHostProcessName.Append('.').Append(architecture.ToString().ToLowerInvariant());
        }
        else
        {
            // 64-bit (x64) executable, uses no architecture suffix in the name.
            // E.g.: testhost.exe or testhost.net472.exe
        }

        testHostProcessName.Append(".exe");
        return testHostProcessName.ToString();
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetTestPlatformExtensions(IEnumerable<string>? sources, IEnumerable<string> extensions)
    {
        if (sources != null && sources.Any())
        {
            extensions = extensions.Concat(sources.SelectMany(s => _fileHelper.EnumerateFiles(Path.GetDirectoryName(s)!, SearchOption.TopDirectoryOnly, TestAdapterEndsWithPattern)));
        }

        extensions = FilterExtensionsBasedOnVersion(extensions);

        return extensions;
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetTestSources(IEnumerable<string> sources)
    {
        // We are doing this specifically for UWP, should we extract it out to some other utility?
        // Why? Lets say if we have to do same for some other source extension, would we just add another if check?
        var uwpSources = sources.Where(source => source.EndsWith(".appxrecipe", StringComparison.OrdinalIgnoreCase));

        if (uwpSources.Any())
        {
            List<string> actualSources = new();
            foreach (var uwpSource in uwpSources)
            {
                actualSources.Add(Path.Combine(Path.GetDirectoryName(uwpSource)!, GetUwpSources(uwpSource)!));
            }

            return actualSources;
        }

        return sources;
    }

    /// <inheritdoc/>
    public bool CanExecuteCurrentRunConfiguration(string? runsettingsXml)
    {
        var config = XmlRunSettingsUtilities.GetRunConfigurationNode(runsettingsXml);
        var framework = config.TargetFramework;

        // This is expected to be called once every run so returning a new instance every time.
        return framework!.Name.IndexOf("NETFramework", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    [MemberNotNullWhen(true, nameof(_messageLogger), nameof(_targetFramework))]
    private bool IsInitialized { get; set; }

    /// <inheritdoc/>
    public void Initialize(IMessageLogger? logger, string runsettingsXml)
    {
        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(runsettingsXml);

        _messageLogger = logger;
        _captureOutput = runConfiguration.CaptureStandardOutput;
        var forwardOutput = runConfiguration.ForwardStandardOutput;
        _testHostManagerCallbacks = new TestHostManagerCallbacks(forwardOutput, logger);
        _architecture = runConfiguration.TargetPlatform;
        _targetFramework = runConfiguration.TargetFramework;
        _testHostProcess = null;

        _disableAppDomain = runConfiguration.DisableAppDomain;
        // If appdomains are disabled the host cannot be shared, because sharing means loading multiple assemblies
        // into the same process, and without appdomains we cannot safely do that.
        //
        // The OPPOSITE is not true though, disabling testhost sharing does not mean that we should not load the
        // dll into a separate appdomain in the host. It just means that we wish to run each dll in separate exe.
        Shared = !_disableAppDomain && !runConfiguration.DisableSharedTestHost;
        _hostExitedEventRaised = false;

        IsInitialized = true;
    }

    /// <inheritdoc/>
    public Task CleanTestHostAsync(CancellationToken cancellationToken)
    {
        try
        {
            _processHelper.TerminateProcess(_testHostProcess);
        }
        catch (Exception ex)
        {
            EqtTrace.Warning("DefaultTestHostManager: Unable to terminate test host process: " + ex);
        }

        _testHostProcess?.Dispose();

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public bool AttachDebuggerToTestHost()
    {
        TPDebug.Assert(_targetFramework is not null && _testHostProcess is not null, "Initialize and LaunchTestHostAsync must be called before AttachDebuggerToTestHost");

        return _customTestHostLauncher switch
        {
            ITestHostLauncher3 launcher3 => launcher3.AttachDebuggerToProcess(new AttachDebuggerInfo { ProcessId = _testHostProcess.Id, TargetFramework = _targetFramework.ToString() }, CancellationToken.None),
            ITestHostLauncher2 launcher2 => launcher2.AttachDebuggerToProcess(_testHostProcess.Id),
            _ => false,
        };
    }

    /// <summary>
    /// Filter duplicate extensions, include only the highest versioned extension
    /// </summary>
    /// <param name="extensions">Entire list of extensions</param>
    /// <returns>Filtered list of extensions</returns>
    private IEnumerable<string> FilterExtensionsBasedOnVersion(IEnumerable<string> extensions)
    {
        TPDebug.Assert(IsInitialized, "Initialize must be called before FilterExtensionsBasedOnVersion");

        Dictionary<string, string> selectedExtensions = new();
        Dictionary<string, Version?> highestFileVersions = new();
        Dictionary<string, Version> conflictingExtensions = new();

        foreach (var extensionFullPath in extensions)
        {
            // assemblyName is the key
            var extensionAssemblyName = Path.GetFileNameWithoutExtension(extensionFullPath);

            if (selectedExtensions.TryGetValue(extensionAssemblyName, out var oldExtensionPath))
            {
                // This extension is duplicate
                var currentVersion = GetAndLogFileVersion(extensionFullPath);

                var oldVersionFound = highestFileVersions.TryGetValue(extensionAssemblyName, out var oldVersion);
                if (!oldVersionFound)
                {
                    oldVersion = GetAndLogFileVersion(oldExtensionPath);
                }

                // If the version of current file is higher than the one in the map
                // replace the older with the current file
                if (currentVersion > oldVersion)
                {
                    highestFileVersions[extensionAssemblyName] = currentVersion;
                    conflictingExtensions[extensionAssemblyName] = currentVersion;
                    selectedExtensions[extensionAssemblyName] = extensionFullPath;
                }
                else
                {
                    if (currentVersion < oldVersion)
                    {
                        conflictingExtensions[extensionAssemblyName] = oldVersion;
                    }

                    if (!oldVersionFound)
                    {
                        highestFileVersions.Add(extensionAssemblyName, oldVersion!);
                    }
                }
            }
            else
            {
                selectedExtensions.Add(extensionAssemblyName, extensionFullPath);
            }
        }

        // Log warning if conflicting version extensions are found
        if (conflictingExtensions.Count != 0)
        {
            var extensionsString = string.Join("\n", conflictingExtensions.Select(kv => $"  {kv.Key} : {kv.Value}"));
            string message = string.Format(CultureInfo.CurrentCulture, Resources.MultipleFileVersions, extensionsString);
            _messageLogger.SendMessage(TestMessageLevel.Warning, message);
        }

        return selectedExtensions.Values;
    }

    private Version GetAndLogFileVersion(string path)
    {
        var fileVersion = _fileHelper.GetFileVersion(path);
        EqtTrace.Verbose("FileVersion for {0} : {1}", path, fileVersion);

        return fileVersion;
    }

    /// <summary>
    /// Raises HostLaunched event
    /// </summary>
    /// <param name="e">host provider event args</param>
    private void OnHostLaunched(HostProviderEventArgs e)
    {
        HostLaunched?.SafeInvoke(this, e, "HostProviderEvents.OnHostLaunched");
    }

    /// <summary>
    /// Raises HostExited event
    /// </summary>
    /// <param name="e">host provider event args</param>
    private void OnHostExited(HostProviderEventArgs e)
    {
        if (!_hostExitedEventRaised)
        {
            _hostExitedEventRaised = true;
            HostExited?.SafeInvoke(this, e, "HostProviderEvents.OnHostExited");
        }
    }

    [MemberNotNullWhen(true, nameof(_testHostProcess), nameof(_testHostProcessStdError))]
    private bool LaunchHost(TestProcessStartInfo testHostStartInfo, CancellationToken cancellationToken)
    {
        _testHostProcessStdError = new StringBuilder(0, CoreUtilities.Constants.StandardErrorMaxLength);
        _testHostProcessStdOut = new StringBuilder(0, CoreUtilities.Constants.StandardErrorMaxLength);
        EqtTrace.Verbose("Launching default test Host Process {0} with arguments {1}", testHostStartInfo.FileName, testHostStartInfo.Arguments);

        // We launch the test host process here if we're on the normal test running workflow.
        // If we're debugging and we have access to the newest version of the testhost launcher
        // interface we launch it here as well, but we expect to attach later to the test host
        // process by using its PID.
        // For every other workflow (e.g.: profiling) we ask the IDE to launch the custom test
        // host for us. In the profiling case this is needed because then the IDE sets some
        // additional environmental variables for us to help with probing.
        if (_customTestHostLauncher == null
            || (_customTestHostLauncher.IsDebug && _customTestHostLauncher is ITestHostLauncher2))
        {
            EqtTrace.Verbose("DefaultTestHostManager: Starting process '{0}' with command line '{1}'", testHostStartInfo.FileName, testHostStartInfo.Arguments);
            cancellationToken.ThrowIfCancellationRequested();
            var outputCallback = _captureOutput ? OutputReceivedCallback : null;
            _testHostProcess = _processHelper.LaunchProcess(
                testHostStartInfo.FileName!,
                testHostStartInfo.Arguments,
                testHostStartInfo.WorkingDirectory,
                testHostStartInfo.EnvironmentVariables,
                ErrorReceivedCallback,
                ExitCallBack,
                outputCallback) as Process;
        }
        else
        {
            int processId = _customTestHostLauncher.LaunchTestHost(testHostStartInfo, cancellationToken);
            _testHostProcess = Process.GetProcessById(processId);
            _processHelper.SetExitCallback(processId, ExitCallBack);
        }

        if (_testHostProcess is null)
        {
            return false;
        }

        AdjustProcessPriorityBasedOnSettings(_testHostProcess, testHostStartInfo.EnvironmentVariables);
        OnHostLaunched(new HostProviderEventArgs("Test Runtime launched", 0, _testHostProcess.Id));

        return true;
    }

    internal static void AdjustProcessPriorityBasedOnSettings(Process testHostProcess, IDictionary<string, string?>? testHostEnvironmentVariables)
    {
        ProcessPriorityClass testHostPriority = ProcessPriorityClass.BelowNormal;
        try
        {
            if (testHostEnvironmentVariables is not null
                && testHostEnvironmentVariables.TryGetValue("VSTEST_BACKGROUND_DISCOVERY", out var isBackgroundDiscoveryEnabled)
                && isBackgroundDiscoveryEnabled == "1")
            {
                testHostProcess.PriorityClass = testHostPriority;
                EqtTrace.Verbose("Setting test host process priority to {0}", testHostProcess.PriorityClass);
            }
        }
        // Setting the process Priority can fail with Win32Exception, NotSupportedException or InvalidOperationException.
        catch (Exception ex)
        {
            EqtTrace.Error("Failed to set test host process priority to {0}. Exception: {1}", testHostPriority, ex);
        }
    }

    private static string? GetUwpSources(string uwpSource)
    {
        var doc = XDocument.Load(uwpSource);
        var ns = doc.Root!.Name.Namespace;

        string appxManifestPath = doc.Element(ns + "Project")!.
            Element(ns + "ItemGroup")!.
            Element(ns + "AppXManifest")!.
            Attribute("Include")!.Value;

        if (!Path.IsPathRooted(appxManifestPath))
        {
            appxManifestPath = Path.Combine(Path.GetDirectoryName(uwpSource)!, appxManifestPath);
        }

        return AppxManifestFile.GetApplicationExecutableName(appxManifestPath);
    }
}
