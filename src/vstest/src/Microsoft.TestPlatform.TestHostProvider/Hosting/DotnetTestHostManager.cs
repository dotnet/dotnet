// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#if NETCOREAPP
using Microsoft.Extensions.DependencyModel;
#endif
using Microsoft.TestPlatform.TestHostProvider;
using Microsoft.TestPlatform.TestHostProvider.Hosting;
using TestHostResources = Microsoft.TestPlatform.TestHostProvider.Resources.Resources;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers.Interfaces;
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

#if NETCOREAPP
using System.Text.Json;
#else
using Jsonite;
#endif

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;

/// <summary>
/// A host manager for <c>dotnet</c> core runtime.
/// </summary>
/// <remarks>
/// Note that some functionality of this entity overlaps with that of <see cref="DefaultTestHostManager"/>. That is
/// intentional since we want to move this to a separate assembly (with some runtime extensibility discovery).
/// </remarks>
[ExtensionUri(DotnetTestHostUri)]
[FriendlyName(DotnetTestHostFriendlyName)]
public class DotnetTestHostManager : ITestRuntimeProvider2
{
    private const string DotnetTestHostUri = "HostProvider://DotnetTestHost";
    // Should the friendly name ever change, please make sure to change the corresponding constant
    // inside ProxyOperationManager::IsTesthostCompatibleWithTestSessions().
    private const string DotnetTestHostFriendlyName = "DotnetTestHost";
    private const string TestAdapterRegexPattern = @"TestAdapter.dll";
    private const string PROCESSOR_ARCHITECTURE = nameof(PROCESSOR_ARCHITECTURE);

    private readonly IDotnetHostHelper _dotnetHostHelper;
    private readonly IEnvironment _platformEnvironment;
    private readonly IProcessHelper _processHelper;
    private readonly IFileHelper _fileHelper;
    private readonly IWindowsRegistryHelper _windowsRegistryHelper;
    private readonly IEnvironmentVariableHelper _environmentVariableHelper;

    private ITestHostLauncher? _customTestHostLauncher;
    private Process? _testHostProcess;
    private StringBuilder? _testHostProcessStdError;
    private StringBuilder? _testHostProcessStdOut;
    private bool _hostExitedEventRaised;
    private string _hostPackageVersion = "15.0.0";
    private Architecture _architecture;
    private Framework? _targetFramework;
    private bool _isVersionCheckRequired = true;
    private string? _dotnetHostPath;
    private bool _captureOutput;
    private bool _createNoNewWindow;

    // True when the target platform was inferred by the platform rather than pinned by the user. Read from
    // the run settings in Initialize (RunConfiguration.IsTargetPlatformInferred), so this per-request fact
    // travels with the run settings instead of the process-wide RunSettingsHelper singleton. Defaults to
    // true (inferred), matching the historical RunSettingsHelper default when no marker is present.
    private bool _isDefaultTargetArchitecture = true;
    private protected TestHostManagerCallbacks? _testHostManagerCallbacks;

    /// <summary>
    /// Initializes a new instance of the <see cref="DotnetTestHostManager"/> class.
    /// </summary>
    public DotnetTestHostManager()
        : this(
            new ProcessHelper(),
            new FileHelper(),
            new DotnetHostHelper(),
            new PlatformEnvironment(),
            new WindowsRegistryHelper(),
            new EnvironmentVariableHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DotnetTestHostManager"/> class.
    /// </summary>
    /// <param name="processHelper">Process helper instance.</param>
    /// <param name="fileHelper">File helper instance.</param>
    /// <param name="dotnetHostHelper">DotnetHostHelper helper instance.</param>
    /// <param name="platformEnvironment">Platform Environment</param>
    /// <param name="windowsRegistryHelper">WindowsRegistryHelper instance</param>
    /// <param name="environmentVariableHelper">EnvironmentVariableHelper instance</param>
    internal DotnetTestHostManager(
        IProcessHelper processHelper,
        IFileHelper fileHelper,
        IDotnetHostHelper dotnetHostHelper,
        IEnvironment platformEnvironment,
        IWindowsRegistryHelper windowsRegistryHelper,
        IEnvironmentVariableHelper environmentVariableHelper)
    {
        _processHelper = processHelper;
        _fileHelper = fileHelper;
        _dotnetHostHelper = dotnetHostHelper;
        _platformEnvironment = platformEnvironment;
        _windowsRegistryHelper = windowsRegistryHelper;
        _environmentVariableHelper = environmentVariableHelper;
    }

    /// <inheritdoc />
    public event EventHandler<HostProviderEventArgs>? HostLaunched;

    /// <inheritdoc />
    public event EventHandler<HostProviderEventArgs>? HostExited;

    /// <summary>
    /// Gets a value indicating whether gets a value indicating if the test host can be shared for multiple sources.
    /// </summary>
    /// <remarks>
    /// Dependency resolution for .net core projects are pivoted by the test project. Hence each test
    /// project must be launched in a separate test host process.
    /// </remarks>
    public bool Shared => false;

    /// <summary>
    /// Gets a value indicating whether the test host supports protocol version check
    /// By default this is set to true. For host package version 15.0.0, this will be set to false;
    /// </summary>
    internal virtual bool IsVersionCheckRequired
    {
        get
        {
            return _isVersionCheckRequired;
        }

        private set
        {
            _isVersionCheckRequired = value;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the test host supports protocol version check
    /// </summary>
    internal bool MakeRunsettingsCompatible => _hostPackageVersion.StartsWith("15.0.0-preview");

    /// <summary>
    /// Gets callback on process exit
    /// </summary>
    private Action<object?> ExitCallBack => process =>
    {
        TPDebug.Assert(_testHostProcessStdError is not null, "_testHostProcessStdError is null");
        TestHostManagerCallbacks.ExitCallBack(_processHelper, process, _testHostProcessStdError, OnHostExited);
    };

    /// <summary>
    /// Gets callback to read from process error stream
    /// </summary>
    private Action<object?, string?> ErrorReceivedCallback => (process, data) =>
    {
        TPDebug.Assert(_testHostProcessStdError is not null, "_testHostProcessStdError is null");
        TPDebug.Assert(_testHostManagerCallbacks is not null, "Initialize must have been called before ExitCallBack");
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
    public void Initialize(IMessageLogger? logger, string runsettingsXml)
    {
        _hostExitedEventRaised = false;
        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(runsettingsXml);
        _captureOutput = runConfiguration.CaptureStandardOutput;
        _createNoNewWindow = runConfiguration.CreateNoNewWindow;
        var forwardOutput = runConfiguration.ForwardStandardOutput;
        _testHostManagerCallbacks = new TestHostManagerCallbacks(forwardOutput, logger);

        _architecture = runConfiguration.TargetPlatform;
        _targetFramework = runConfiguration.TargetFramework;
        _dotnetHostPath = runConfiguration.DotnetHostPath;
        _isDefaultTargetArchitecture = runConfiguration.IsTargetPlatformInferred;
    }

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
        TPDebug.Assert(_targetFramework is not null, "_targetFramework is null");
        EqtTrace.Verbose($"DotnetTestHostmanager.GetTestHostProcessStartInfo: Platform environment '{_platformEnvironment.Architecture}' target architecture '{_architecture}' framework '{_targetFramework}' OS '{_platformEnvironment.OperatingSystem}'");

        var startInfo = new TestProcessStartInfo();
        startInfo.EnvironmentVariables = environmentVariables ?? new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        string? dotnetRootPath = _environmentVariableHelper.GetEnvironmentVariable("VSTEST_DOTNET_ROOT_PATH");
        string? dotnetRootArchitecture = _environmentVariableHelper.GetEnvironmentVariable("VSTEST_DOTNET_ROOT_ARCHITECTURE");

        // The SDK (dotnet test) is the primary source: it computes the architecture specific dotnet root and passes it
        // via VSTEST_DOTNET_ROOT_PATH / VSTEST_DOTNET_ROOT_ARCHITECTURE. Only when the SDK did not provide it (direct
        // invocation of vstest.console, e.g. under Visual Studio or by hand) do we fall back to the architecture-less
        // DOTNET_ROOT from the surrounding environment or the caller (runsettings <EnvironmentVariables>).
        //
        // An apphost honors that architecture-less DOTNET_ROOT regardless of its own architecture, so if it points at a
        // different architecture (commonly an x64 install) an x86 apphost picks it up and tries to load the x64
        // hostfxr.dll into the 32-bit process, failing with 0x800700C1 (ERROR_BAD_EXE_FORMAT). See
        // https://github.com/microsoft/vstest/issues/16151. We derive the architecture the install actually is (from the
        // dotnet muxer's PE header) so the resolution below can set the architecture specific DOTNET_ROOT_<ARCH>, and we
        // clear the ambiguous DOTNET_ROOT so the apphost does not pick it up and fail.
        if (StringUtilities.IsNullOrWhiteSpace(dotnetRootPath))
        {
            var dotnetRoot = TryGetTestHostEnvironmentVariable(startInfo, "DOTNET_ROOT", out var callerProvidedDotnetRoot)
                ? callerProvidedDotnetRoot
                : _environmentVariableHelper.GetEnvironmentVariable("DOTNET_ROOT");

            if (!StringUtilities.IsNullOrWhiteSpace(dotnetRoot))
            {
                // The architecture specific apphosts affected by this ambiguity only exist on Windows, and the PE based
                // architecture probe is Windows only, so GetExecutableArchitecture returns null elsewhere and we skip.
                var dotnetExeArchitecture = GetExecutableArchitecture(Path.Combine(dotnetRoot, "dotnet.exe"));
                if (dotnetExeArchitecture is not null)
                {
                    dotnetRootPath = dotnetRoot;
                    dotnetRootArchitecture = dotnetExeArchitecture.ToString();

                    EqtTrace.Verbose($"DotnetTestHostmanager.GetTestHostProcessStartInfo: Derived dotnet root '{dotnetRootPath}' (architecture '{dotnetRootArchitecture}') from the architecture-less DOTNET_ROOT.");

                    startInfo.EnvironmentVariables["DOTNET_ROOT"] = string.Empty;
                    EqtTrace.Verbose($"DotnetTestHostmanager.GetTestHostProcessStartInfo: clearing the ambiguous DOTNET_ROOT to avoid an architecture mismatch.");
                }
            }
        }

        if (!StringUtilities.IsNullOrWhiteSpace(dotnetRootPath))
        {
            if (StringUtils.IsNullOrWhiteSpace(dotnetRootArchitecture))
            {
                throw new InvalidOperationException("Either 'VSTEST_DOTNET_ROOT_PATH' and 'VSTEST_DOTNET_ROOT_ARCHITECTURE', or 'DOTNET_ROOT' and autodetected architecture from dotnet.exe in that path must be both always set.");
            }

            EqtTrace.Verbose($"DotnetTestHostmanager.LaunchTestHostAsync: VSTEST_DOTNET_ROOT_PATH or DOTNET_ROOT={dotnetRootPath}");
            EqtTrace.Verbose($"DotnetTestHostmanager.LaunchTestHostAsync: VSTEST_DOTNET_ROOT_ARCHITECTURE or autodetected architecture from dotnet.exe={dotnetRootArchitecture}");

            if (!FeatureFlag.Instance.IsSet(FeatureFlag.VSTEST_DISABLE_DOTNET_ROOT_ON_NONWINDOWS))
            {
                // Set DOTNET_ROOT_<ARCH> for any run, so it gets propagated to testhost and its child processes, like dotnet run does it. This allows executables that start under testhost to find the path to dotnet
                // from which we called dotnet test. Before this change we only expected testhost.exe to be in this situation, but with xunit v3 running separate exe under testhost, the need for setting architecture
                // specific DOTNET_ROOT increases and makes this necessary for users to have good experience.
                SetDotnetRootForArchitecture(startInfo, dotnetRootPath!, dotnetRootArchitecture);
            }
        }

        // .NET core host manager is not a shared host. It will expect a single test source to be provided.
        // TODO: Throw an exception when we get 0 or more than 1 source, that explains what happened, instead of .Single throwing a generic exception?
        var args = string.Empty;
        var sourcePath = sources.Single();
        var sourceFile = Path.GetFileNameWithoutExtension(sourcePath);
        var sourceDirectory = Path.GetDirectoryName(sourcePath)!;

        // Probe for runtime config and deps file for the test source
        var runtimeConfigPath = Path.Combine(sourceDirectory, string.Concat(sourceFile, ".runtimeconfig.json"));
        var runtimeConfigFound = false;
        if (_fileHelper.Exists(runtimeConfigPath))
        {
            runtimeConfigFound = true;
            string argsToAdd = " --runtimeconfig " + runtimeConfigPath.AddDoubleQuote();
            args += argsToAdd;
            EqtTrace.Verbose("DotnetTestHostmanager: Adding {0} in args", argsToAdd);
        }
        else
        {
            EqtTrace.Verbose("DotnetTestHostmanager: File {0}, does not exist", runtimeConfigPath);
        }

        // Use the deps.json for test source
        var depsFilePath = Path.Combine(sourceDirectory, string.Concat(sourceFile, ".deps.json"));
        if (_fileHelper.Exists(depsFilePath))
        {
            string argsToAdd = " --depsfile " + depsFilePath.AddDoubleQuote();
            args += argsToAdd;
            EqtTrace.Verbose("DotnetTestHostmanager: Adding {0} in args", argsToAdd);
        }
        else
        {
            EqtTrace.Verbose("DotnetTestHostmanager: File {0}, does not exist", depsFilePath);
        }

        var runtimeConfigDevPath = Path.Combine(sourceDirectory, string.Concat(sourceFile, ".runtimeconfig.dev.json"));
        string testHostPath = string.Empty;
        bool useCustomDotnetHostpath = !_dotnetHostPath.IsNullOrEmpty();

        if (useCustomDotnetHostpath)
        {
            EqtTrace.Verbose("DotnetTestHostmanager: User specified custom path to dotnet host: '{0}'.", _dotnetHostPath);
        }

        // Try find testhost.exe (or the architecture specific version). We ship those ngened executables for Windows
        // because they have faster startup time. We ship them only for some platforms.
        // When user specified path to dotnet.exe don't try to find the executable, because we will always use the
        // testhost.dll together with their dotnet.exe.
        bool testHostExeFound = false;
        if (!useCustomDotnetHostpath
            && _platformEnvironment.OperatingSystem.Equals(PlatformOperatingSystem.Windows)
            // On arm we cannot rely on apphost and we'll use dotnet.exe muxer.
            && !IsWinOnArm())
        {
            // testhost.exe is 64-bit and has no suffix other versions have architecture suffix.
            var exeName = _architecture is Architecture.X64 or Architecture.Default or Architecture.AnyCPU
                ? "testhost.exe"
                : $"testhost.{_architecture.ToString().ToLowerInvariant()}.exe";

            var fullExePath = Path.Combine(sourceDirectory, exeName);

            // check for testhost.exe in sourceDirectory. If not found, check in nuget folder.
            if (_fileHelper.Exists(fullExePath))
            {
                EqtTrace.Verbose($"DotnetTestHostManager: {exeName} found at path: " + fullExePath);
                startInfo.FileName = fullExePath;
                testHostExeFound = true;
            }
            else
            {
                // Check if testhost.dll is found in nuget folder or next to the test.dll, and use that to locate testhost.exe that is in the build folder in the same Nuget package.
                testHostPath = GetTestHostPath(runtimeConfigDevPath, depsFilePath, sourceDirectory);
                if (!testHostPath.IsNullOrWhiteSpace() && testHostPath.IndexOf("microsoft.testplatform.testhost", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // testhost.dll is present in path {testHostNugetRoot}\lib\net8.0\testhost.dll
                    // testhost.(x86).exe is present in location {testHostNugetRoot}\build\net8.0\{x86/x64}\{testhost.x86.exe/testhost.exe}
                    var folderName = _architecture is Architecture.X64 or Architecture.Default or Architecture.AnyCPU
                        ? Architecture.X64.ToString().ToLowerInvariant()
                        : _architecture.ToString().ToLowerInvariant();

                    var testHostNugetRoot = new DirectoryInfo(testHostPath).Parent!.Parent!.Parent!;

#if DOTNET_BUILD_FROM_SOURCE
                    var testHostExeNugetPath = Path.Combine(testHostNugetRoot.FullName, "build", "net8.0", folderName, exeName);
                    if (!_fileHelper.Exists(testHostExeNugetPath))
                    {
                        testHostExeNugetPath = Path.Combine(testHostNugetRoot.FullName, "build", "net9.0", folderName, exeName);
                    }
#else
                    var testHostExeNugetPath = Path.Combine(testHostNugetRoot.FullName, "build", "net8.0", folderName, exeName);
#endif

                    if (_fileHelper.Exists(testHostExeNugetPath))
                    {
                        EqtTrace.Verbose("DotnetTestHostManager: Testhost.exe/testhost.x86.exe found at path: " + testHostExeNugetPath);
                        startInfo.FileName = testHostExeNugetPath;
                        testHostExeFound = true;
                    }
                }
            }
        }

        if (!testHostExeFound)
        {
            // We did not find testhost.exe, either it did not exist, or we are not on Windows, or the user forced a custom path to dotnet. So we will try
            // to find testhost.dll from the runtime config and deps.json.
            if (testHostPath.IsNullOrEmpty())
            {
                testHostPath = GetTestHostPath(runtimeConfigDevPath, depsFilePath, sourceDirectory);
            }

            if (testHostPath.IsNullOrEmpty())
            {
                // We still did not find testhost.dll. Try finding it next to vstest.console, (or in next to vstest.console ./TestHostNet for .NET Framework)
#if NETFRAMEWORK
                var testHostNextToRunner = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, "TestHostNet", "testhost.dll");
#else
                var testHostNextToRunner = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, "testhost.dll");
#endif
                if (_fileHelper.Exists(testHostNextToRunner))
                {
                    // The testhost shipped next to the runner (together with the package's testhost.deps.json and a
                    // synthesized runtimeconfig) is the fallback for native (C++) runners that bring no managed testhost.
                    // A managed (.NET) source reaching here did not resolve a testhost from its own deps.json, which means
                    // the test project is missing the Microsoft.NET.Test.Sdk reference. Don't silently run it on the
                    // built-in testhost (it would just discover no tests) - throw and point the user at Microsoft.NET.Test.Sdk.
                    if (!IsNativeModule(sourcePath))
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, TestHostResources.CouldNotFindTesthost, sourcePath, sourceDirectory);
                        throw new TestPlatformException(message);
                    }

                    EqtTrace.Verbose("DotnetTestHostManager: Found testhost.dll next to runner executable: {0}.", testHostNextToRunner);
                    testHostPath = testHostNextToRunner;

                    // Because we could not find the testhost based on the project reference, or next to the tested dll,
                    // it is most likely not referenced from it, or the .dll is unmanaged.
                    //
                    // Add additional deps, that describes the dependencies of testhost.dll, which will merge with deps.json
                    // if the process provided any, or will be used by itself if none was provided.
                    var testhostDeps = Path.Combine(Path.GetDirectoryName(testHostNextToRunner)!, "testhost.deps.json");
                    string argsToAdd = " --additional-deps " + testhostDeps.AddDoubleQuote();
                    args += argsToAdd;
                    EqtTrace.Verbose("DotnetTestHostmanager: Adding {0} in args", argsToAdd);

                    // Additional deps will contain relative paths, tell the process to search for the dlls also
                    // next to the testhost.dll. testhost.deps.json is the real deps.json produced when testhost.dll
                    // is built, shipped next to testhost.dll. Its dependency paths follow a NuGet package layout,
                    // so on its own dotnet would look for the dlls in a nested folder structure and fail to find them.
                    // We ship all the testhost dependencies flat next to testhost.dll and add that folder as an
                    // additional probing path below, which lets dotnet resolve every dependency from there.
                    //
                    // If they were in the base path (where the test dll is) it would work
                    // fine, because in base folder, dotnet searches directly in that folder, but not in probing paths.
                    var testHostProbingPath = Path.GetDirectoryName(testHostNextToRunner)!;
                    argsToAdd = " --additionalprobingpath " + testHostProbingPath.AddDoubleQuote();
                    args += argsToAdd;
                    EqtTrace.Verbose("DotnetTestHostmanager: Adding {0} in args", argsToAdd);

                    if (!runtimeConfigFound)
                    {
                        // Only native (e.g. C++) sources reach this point. A managed source that did not resolve its
                        // own runtime config threw above and pointed the user at Microsoft.NET.Test.Sdk. A native dll
                        // carries no target framework information, so we don't know - and don't care - which runtime
                        // version it runs on. We point it at testhost-latest.runtimeconfig.json, which rolls forward to
                        // the latest installed runtime, giving us the best chance of finding a runtime to launch on.
                        //
                        // We use --runtimeconfig rather than --fx-version because --fx-version pins an exact version and
                        // does not roll forward (even with --roll-forward), and we don't know which version is installed.
                        var testhostRuntimeConfig = Path.Combine(Path.GetDirectoryName(testHostNextToRunner)!, "testhost-latest.runtimeconfig.json");

                        argsToAdd = " --runtimeconfig " + testhostRuntimeConfig.AddDoubleQuote();
                        args += argsToAdd;
                        EqtTrace.Verbose("DotnetTestHostmanager: Adding {0} in args", argsToAdd);
                    }
                }
            }

            if (testHostPath.IsNullOrEmpty())
            {
                string message = string.Format(CultureInfo.CurrentCulture, TestHostResources.CouldNotFindTesthost, sourcePath, sourceDirectory);
                throw new TestPlatformException(message);
            }

            // We silently force x64 only if the target architecture is the default one and is not specified by user
            // through --arch or runsettings or -- RunConfiguration.TargetPlatform=arch
            bool forceToX64 = _isDefaultTargetArchitecture && SilentlyForceToX64(sourcePath);
            EqtTrace.Verbose($"DotnetTestHostmanager: Current process architetcure '{_processHelper.GetCurrentProcessArchitecture()}'");
            bool isSameArchitecture = IsSameArchitecture(_architecture, _processHelper.GetCurrentProcessArchitecture());
            var currentProcessPath = _processHelper.GetCurrentProcessFileName()!;
            bool isRunningWithDotnetMuxer = IsRunningWithDotnetMuxer(currentProcessPath);
            if (useCustomDotnetHostpath)
            {
                startInfo.FileName = _dotnetHostPath;
            }

            // If already running with the dotnet executable and the architecture is compatible, use it; otherwise search the correct muxer architecture on disk.
            else if (isRunningWithDotnetMuxer && isSameArchitecture && !forceToX64)
            {
                EqtTrace.Verbose("DotnetTestHostmanager.LaunchTestHostAsync: Compatible muxer architecture of running process '{0}' and target architecture '{1}'", _processHelper.GetCurrentProcessArchitecture(), _architecture);
                startInfo.FileName = currentProcessPath;
            }
            else
            {
                PlatformArchitecture targetArchitecture = TranslateToPlatformArchitecture(_architecture);
                EqtTrace.Verbose($"DotnetTestHostmanager: Searching muxer for the architecture '{targetArchitecture}', OS '{_platformEnvironment.OperatingSystem}' framework '{_targetFramework}' SDK platform architecture '{_platformEnvironment.Architecture}'");
                if (forceToX64)
                {
                    EqtTrace.Verbose($"DotnetTestHostmanager: Forcing the search to x64 architecure, IsDefaultTargetArchitecture '{_isDefaultTargetArchitecture}' OS '{_platformEnvironment.OperatingSystem}' framework '{_targetFramework}'");
                }

                // Check if DOTNET_ROOT resolution should be bypassed.
                var shouldIgnoreDotnetRoot = (_environmentVariableHelper.GetEnvironmentVariable("VSTEST_IGNORE_DOTNET_ROOT")?.Trim() ?? "0") != "0";
                var muxerResolutionStrategy = shouldIgnoreDotnetRoot
                        ? (DotnetMuxerResolutionStrategy.GlobalInstallationLocation | DotnetMuxerResolutionStrategy.DefaultInstallationLocation)
                        : DotnetMuxerResolutionStrategy.Default;

                PlatformArchitecture finalTargetArchitecture = forceToX64 ? PlatformArchitecture.X64 : targetArchitecture;
                if (!_dotnetHostHelper.TryGetDotnetPathByArchitecture(finalTargetArchitecture, muxerResolutionStrategy, out string? muxerPath))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, TestHostResources.NoDotnetMuxerFoundForArchitecture, $"dotnet{(_platformEnvironment.OperatingSystem == PlatformOperatingSystem.Windows ? ".exe" : string.Empty)}", finalTargetArchitecture.ToString());
                    EqtTrace.Error(message);
                    throw new TestPlatformException(message);
                }

                startInfo.FileName = muxerPath;
            }

            EqtTrace.Verbose("DotnetTestHostmanager: Full path of testhost.dll is {0}", testHostPath);
            args = "exec" + args;
            args += " " + testHostPath.AddDoubleQuote();
        }

        EqtTrace.Verbose("DotnetTestHostmanager: Full path of host exe is {0}", startInfo.FileName);

        // Attempt to upgrade netcoreapp2.1 and earlier versions of testhost to netcoreapp3.1 or a newer runtime,
        // assuming that the user does not have that old runtime installed.
        if (_targetFramework.Name.StartsWith(".NETCoreApp,", StringComparison.OrdinalIgnoreCase)
            && Version.TryParse(_targetFramework.Version, out var version)
            && version < new Version(3, 0))
        {
            args += " --roll-forward Major";
        }

        args += " " + connectionInfo.ToCommandLineOptions();

        // Create a additional probing path args with Nuget.Client
        // args += "--additionalprobingpath xxx"
        // TODO this may be required in ASP.net, requires validation

        // Sample command line for the spawned test host
        // "D:\dd\gh\Microsoft\vstest\tools\dotnet\dotnet.exe" exec
        // --runtimeconfig G:\tmp\netcore-test\bin\Debug\netcoreapp1.0\netcore-test.runtimeconfig.json
        // --depsfile G:\tmp\netcore-test\bin\Debug\netcoreapp1.0\netcore-test.deps.json
        // --additionalprobingpath C:\Users\username\.nuget\packages\
        // G:\nuget-package-path\microsoft.testplatform.testhost\version\**\testhost.dll
        // G:\tmp\netcore-test\bin\Debug\netcoreapp1.0\netcore-test.dll
        startInfo.Arguments = args;

        // If we're running using custom apphost we need to set DOTNET_ROOT/DOTNET_ROOT(x86)
        // We're setting it inside SDK to support private install scenario.
        // i.e. I've got only private install and no global installation, in this case apphost needs to use env var to locate runtime.
        if (testHostExeFound)
        {
            if (!StringUtilities.IsNullOrWhiteSpace(dotnetRootPath))
            {
                // The parent process is passing to us the path in which the dotnet.exe is and is passing the architecture of the dotnet.exe,
                // so if the child process (testhost) is the same architecture it can pick up that dotnet.exe location and run. This is to allow
                // local installations of dotnet/sdk to work with testhost.
                //
                // There are 2 complications in this process:
                // 1) There are differences between how .NET Apphosts are handling DOTNET_ROOT, versions pre-net6 are only looking at
                //    DOTNET_ROOT(x86) and then DOTNET_ROOT. This makes is really easy to set DOTNET_ROOT to point at x64 dotnet installation
                //    and have that picked up by x86 testhost and fail.
                //    Unfortunately vstest.console has to support both new (17.14+) testhosts that are built against net8, and old (pre 17.14)
                //    testhosts that are built using netcoreapp3.1 apphost, and so their approach to resolving DOTNET_ROOT differ.
                //
                //    /!\ The apphost version does not align with the targeted framework (tfm), an older testhost is built against netcoreapp3.1
                //        but can be used to run net8 tests. The only way to tell is the version of the testhost.
                //
                //    netcoreapp3.1 hosts only support DOTNET_ROOT and DOTNET_ROOT(x86) env variables.
                //    net8          hosts, support also DOTNET_ROOT_<ARCH> variables, which is what we should prefer to set the location of dotnet
                //                  in a more architecture specific way.
                //
                // 2) The surrounding environment might already have the environment variables set, most likely by setting DOTNET_ROOT, which is
                //    a universal way of setting where the dotnet is, that works across all different architectures of the .NET apphost.
                //    By setting our (hopefully more specific variable) we might overwrite what user specified, and in case of DOTNET_ROOT it is probably
                //    preferable when we can set the DOTNET_ROOT_<ARCH> variable.
                var testhostDllPath = Path.ChangeExtension(startInfo.FileName, ".dll");
                // This file check is for unit tests, we expect the file to always be there. Otherwise testhost.exe would not be able to run.
                var testhostVersionInfo = _fileHelper.Exists(testhostDllPath) ? FileVersionInfo.GetVersionInfo(testhostDllPath) : null;
                if (testhostVersionInfo != null && testhostVersionInfo.ProductMajorPart >= 17 && testhostVersionInfo.ProductMinorPart >= 14)
                {
                    // This is a new testhost that builds at least against net8 we should set the architecture specific DOTNET_ROOT_<ARCH>.
                    //
                    // We ship just testhost.exe and testhost.x86.exe if the architecture is different we won't find the testhost*.exe and
                    // won't reach this code, but let's write this in a generic way anyway, to avoid breaking if we add more variants of testhost*.exe.
                    //
                    // If the feature flag is set, revert to previous behavior of setting DOTNET_ROOT_<ARCH> only on Windows after we found testhost.exe.
                    if (FeatureFlag.Instance.IsSet(FeatureFlag.VSTEST_DISABLE_DOTNET_ROOT_ON_NONWINDOWS))
                    {
                        SetDotnetRootForArchitecture(startInfo, dotnetRootPath!, dotnetRootArchitecture!);
                    }
                }
                else
                {
                    // This is an old testhost that built against netcoreapp3.1, it does not understand architecture specific DOTNET_ROOT_<ARCH>, we have to set it more carefully
                    // to avoid setting DOTNET_ROOT that points to x64 but is picked up by x86 host.
                    //
                    // Also avoid setting it if we are already getting it from the surrounding environment.
                    var architectureFromEnv = (Architecture)Enum.Parse(typeof(Architecture), dotnetRootArchitecture!, ignoreCase: true);
                    if (architectureFromEnv == _architecture)
                    {
                        if (_architecture == Architecture.X86)
                        {
                            const string dotnetRootX86 = "DOTNET_ROOT(x86)";
                            if (StringUtils.IsNullOrWhiteSpace(_environmentVariableHelper.GetEnvironmentVariable(dotnetRootX86)))
                            {
                                startInfo.EnvironmentVariables.Add(dotnetRootX86, dotnetRootPath);
                            }
                        }
                        else
                        {
                            // If the architecture is not x86 and the testhost does not understand the architecture
                            // specific DOTNET_ROOT_<ARCH>, we have to re-insert the more generic DOTNET_ROOT. Use the
                            // indexer (not Add) because the architecture-less DOTNET_ROOT was cleared to empty above.
                            startInfo.EnvironmentVariables["DOTNET_ROOT"] = dotnetRootPath;
                        }
                    }
                }
            }
        }

        startInfo.WorkingDirectory = sourceDirectory;

        return startInfo;

        bool IsRunningWithDotnetMuxer(string currentProcessPath)
            => currentProcessPath.EndsWith("dotnet", StringComparison.OrdinalIgnoreCase) ||
               currentProcessPath.EndsWith("dotnet.exe", StringComparison.OrdinalIgnoreCase);

        bool IsWinOnArm()
        {
            bool isWinOnArm = false;
            if (_platformEnvironment.OperatingSystem == PlatformOperatingSystem.Windows)
            {
                string? processorArchitecture = Environment.GetEnvironmentVariable(PROCESSOR_ARCHITECTURE, EnvironmentVariableTarget.Machine);
                if (processorArchitecture is not null)
                {
                    EqtTrace.Verbose($"DotnetTestHostmanager.IsWinOnArm: Current PROCESSOR_ARCHITECTURE from environment variable '{processorArchitecture}'");
                    isWinOnArm = processorArchitecture.ToString().ToLowerInvariant().Contains("arm");
                }
            }

            EqtTrace.Verbose($"DotnetTestHostmanager.IsWinOnArm: Is Windows on ARM '{isWinOnArm}'");
            return isWinOnArm;
        }

        PlatformArchitecture TranslateToPlatformArchitecture(Architecture targetArchitecture)
        {
            switch (targetArchitecture)
            {
                case Architecture.X86:
                    return PlatformArchitecture.X86;
                case Architecture.X64:
                    return PlatformArchitecture.X64;
                case Architecture.ARM:
                    return PlatformArchitecture.ARM;
                case Architecture.ARM64:
                    return PlatformArchitecture.ARM64;
                case Architecture.S390x:
                    return PlatformArchitecture.S390x;
                case Architecture.Ppc64le:
                    return PlatformArchitecture.Ppc64le;
                case Architecture.RiscV64:
                    return PlatformArchitecture.RiscV64;
                case Architecture.LoongArch64:
                    return PlatformArchitecture.LoongArch64;
                case Architecture.AnyCPU:
                case Architecture.Default:
                default:
                    break;
            }

            throw new TestPlatformException($"Invalid target architecture '{targetArchitecture}'");
        }

        static bool IsSameArchitecture(Architecture targetArchitecture, PlatformArchitecture platformAchitecture)
            => targetArchitecture switch
            {
                Architecture.X86 => platformAchitecture == PlatformArchitecture.X86,
                Architecture.X64 => platformAchitecture == PlatformArchitecture.X64,
                Architecture.ARM => platformAchitecture == PlatformArchitecture.ARM,
                Architecture.ARM64 => platformAchitecture == PlatformArchitecture.ARM64,
                Architecture.S390x => platformAchitecture == PlatformArchitecture.S390x,
                Architecture.Ppc64le => platformAchitecture == PlatformArchitecture.Ppc64le,
                Architecture.RiscV64 => platformAchitecture == PlatformArchitecture.RiscV64,
                Architecture.LoongArch64 => platformAchitecture == PlatformArchitecture.LoongArch64,
                _ => throw new TestPlatformException($"Invalid target architecture '{targetArchitecture}'"),
            };

        bool SilentlyForceToX64(string sourcePath)
        {
            // We need to force x64 in some scenario
            // https://github.com/dotnet/sdk/blob/main/src/Tasks/Microsoft.NET.Build.Tasks/targets/Microsoft.NET.RuntimeIdentifierInference.targets#L140-L143

            // If we are running on an M1 with a native SDK and the TFM is < 6.0, we have to use a x64 apphost since there are no osx-arm64 apphosts previous to .NET 6.0.
            if (_platformEnvironment.OperatingSystem == PlatformOperatingSystem.OSX &&
                _platformEnvironment.Architecture == PlatformArchitecture.ARM64 &&
                new Version(_targetFramework.Version).Major < 6)
            {
                return true;
            }

            // If we are running on win-arm64 and the TFM is < 5.0, we have to use a x64 apphost since there are no win-arm64 apphosts previous to .NET 5.0.
            return _platformEnvironment.OperatingSystem == PlatformOperatingSystem.Windows &&
                   _platformEnvironment.Architecture == PlatformArchitecture.ARM64 &&
                   new Version(_targetFramework.Version).Major < 5 &&
                   !IsNativeModule(sourcePath);
        }
    }

    /// <summary>
    /// Determines whether the given module is a native (unmanaged) binary, such as a C++ CppUnitTestFramework test dll.
    /// </summary>
    /// <remarks>
    /// Native sources have no managed (CLI) metadata, or are not IL-only (mixed mode). They legitimately have no
    /// deps.json and rely on the testhost shipped next to the runner, so callers treat them leniently. If the module
    /// cannot be read we cannot prove it is native, so we assume managed.
    /// </remarks>
    internal virtual bool IsNativeModule(string modulePath)
    {
        // Scenario: dotnet test nativeArm64.dll for CppUnitTestFramework
        try
        {
            using var assemblyStream = _fileHelper.GetStream(modulePath, FileMode.Open, FileAccess.Read);
            using var peReader = new PEReader(assemblyStream);
            if (!peReader.HasMetadata || (peReader.PEHeaders.CorHeader?.Flags & CorFlags.ILOnly) == 0)
            {
                EqtTrace.Verbose($"DotnetTestHostManager.IsNativeModule: Source '{modulePath}' is native.");
                return true;
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Verbose($"DotnetTestHostManager.IsNativeModule: Failed to read '{modulePath}', assuming managed. {ex}");
        }

        return false;
    }

    /// <summary>
    /// Looks up an environment variable that the caller explicitly set for the testhost (i.e. one present in
    /// <paramref name="startInfo"/>'s <see cref="TestProcessStartInfo.EnvironmentVariables"/>, typically coming from
    /// runsettings <c>&lt;EnvironmentVariables&gt;</c>). The lookup is case-insensitive to match how environment
    /// variables are resolved on Windows, which is the only platform where the architecture specific apphosts handled
    /// here exist. Returns <see langword="true"/> when the variable was provided (even when its value is
    /// <see langword="null"/> or empty, i.e. explicitly cleared).
    /// </summary>
    private static bool TryGetTestHostEnvironmentVariable(TestProcessStartInfo startInfo, string name, out string? value)
    {
        value = null;
        if (startInfo.EnvironmentVariables is null)
        {
            return false;
        }

        foreach (var environmentVariable in startInfo.EnvironmentVariables)
        {
            if (string.Equals(environmentVariable.Key, name, StringComparison.OrdinalIgnoreCase))
            {
                value = environmentVariable.Value;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the architecture of a Windows PE executable (e.g. the dotnet muxer) by reading its COFF header, or
    /// <see langword="null"/> when the architecture cannot be determined (e.g. file missing or not a Windows PE).
    /// </summary>
    internal virtual PlatformArchitecture? GetExecutableArchitecture(string executablePath)
    {
        // The architecture specific apphosts we handle here only exist on Windows, so we only need to read the
        // Windows PE header.
        if (_platformEnvironment.OperatingSystem != PlatformOperatingSystem.Windows
            || !_fileHelper.Exists(executablePath))
        {
            return null;
        }

        try
        {
            // Open with FileShare.Read so the probe is resilient when the muxer (dotnet.exe) is currently running,
            // which is common on dev boxes and CI. The default FileShare.None overload would fail to open a file in
            // use and silently skip normalization, leaving the original architecture mismatch in place.
            using var stream = _fileHelper.GetStream(executablePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var peReader = new PEReader(stream);
            return peReader.PEHeaders.CoffHeader.Machine switch
            {
                Machine.Amd64 => PlatformArchitecture.X64,
                Machine.Arm64 => PlatformArchitecture.ARM64,
                Machine.Arm => PlatformArchitecture.ARM,
                Machine.I386 => PlatformArchitecture.X86,
                _ => (PlatformArchitecture?)null,
            };
        }
        catch (Exception ex)
        {
            EqtTrace.Verbose($"DotnetTestHostManager.GetExecutableArchitecture: Failed to read architecture from '{executablePath}': {ex.Message}");
            return null;
        }
    }

    private void SetDotnetRootForArchitecture(TestProcessStartInfo startInfo, string dotnetRootPath, string dotnetRootArchitecture)
    {
        var environmentVariableName = $"DOTNET_ROOT_{dotnetRootArchitecture.ToUpperInvariant()}";

        var existingDotnetRoot = _environmentVariableHelper.GetEnvironmentVariable(environmentVariableName);
        if (!StringUtilities.IsNullOrWhiteSpace(existingDotnetRoot))
        {
            EqtTrace.Verbose($"DotnetTestHostManager.SetDotnetRootForArchitecture: The variable {environmentVariableName} is already set in the surrounding environment, don't add it to testhost start info, because we want to keep what user provided externally.");
        }
        else
        {
            startInfo.EnvironmentVariables ??= new Dictionary<string, string?>();

            // Set the architecture specific variable to the environment of the process so it is picked up.
            startInfo.EnvironmentVariables.Add(environmentVariableName, dotnetRootPath);

            EqtTrace.Verbose($"DotnetTestHostManager.SetDotnetRootForArchitecture: Adding {environmentVariableName}={dotnetRootPath} to testhost start info.");
        }
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetTestPlatformExtensions(IEnumerable<string> sources, IEnumerable<string> extensions)
    {
        List<string> extensionPaths = new();
        var sourceDirectory = Path.GetDirectoryName(sources.Single());

        if (!sourceDirectory.IsNullOrEmpty() && _fileHelper.DirectoryExists(sourceDirectory))
        {
            extensionPaths.AddRange(_fileHelper.EnumerateFiles(sourceDirectory, SearchOption.TopDirectoryOnly, TestAdapterRegexPattern));
        }

        return extensionPaths;
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetTestSources(IEnumerable<string> sources)
    {
        // We do not have scenario where netcore tests are deployed to remote machine, so no need to update sources
        return sources;
    }

    /// <inheritdoc/>
    public bool CanExecuteCurrentRunConfiguration(string? runsettingsXml)
    {
        var config = XmlRunSettingsUtilities.GetRunConfigurationNode(runsettingsXml);
        var framework = config.TargetFramework;

        return framework!.Name.IndexOf("netcoreapp", StringComparison.OrdinalIgnoreCase) >= 0
               || framework.Name.IndexOf("net5", StringComparison.OrdinalIgnoreCase) >= 0;
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
            EqtTrace.Warning("DotnetTestHostManager: Unable to terminate test host process: " + ex);
        }

        _testHostProcess?.Dispose();

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public bool AttachDebuggerToTestHost()
    {
        TPDebug.Assert(_targetFramework is not null && _testHostProcess is not null, "DotnetTestHostManager: TargetFramework is null");
        return _customTestHostLauncher switch
        {
            ITestHostLauncher3 launcher3 => launcher3.AttachDebuggerToProcess(new AttachDebuggerInfo { ProcessId = _testHostProcess.Id, TargetFramework = _targetFramework.ToString() }, CancellationToken.None),
            ITestHostLauncher2 launcher2 => launcher2.AttachDebuggerToProcess(_testHostProcess.Id),
            _ => false,
        };
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
            EqtTrace.Verbose("DotnetTestHostManager.OnHostExited: invoking OnHostExited callback");
            HostExited?.SafeInvoke(this, e, "HostProviderEvents.OnHostExited");
        }
        else
        {
            EqtTrace.Verbose("DotnetTestHostManager.OnHostExited: exit event was already raised, skipping");
        }
    }

    [MemberNotNull(nameof(_testHostProcessStdError))]
    [MemberNotNullWhen(true, nameof(_testHostProcess))]
    private bool LaunchHost(TestProcessStartInfo testHostStartInfo, CancellationToken cancellationToken)
    {
        _testHostProcessStdError = new StringBuilder(0, CoreUtilities.Constants.StandardErrorMaxLength);
        _testHostProcessStdOut = new StringBuilder(0, CoreUtilities.Constants.StandardErrorMaxLength);

        // We launch the test host process here if we're on the normal test running workflow.
        // If we're debugging and we have access to the newest version of the testhost launcher
        // interface we launch it here as well, but we expect to attach later to the test host
        // process by using its PID.
        // For every other workflow (e.g.: profiling) we ask the IDE to launch the custom test
        // host for us. In the profiling case this is needed because then the IDE sets some
        // additional environmental variables for us to help with probing.
        if ((_customTestHostLauncher == null)
            || (_customTestHostLauncher.IsDebug
                && _customTestHostLauncher is ITestHostLauncher2))
        {
            if (EqtTrace.IsVerboseEnabled)
            {
                var dotnetEnvVars = testHostStartInfo.EnvironmentVariables?
                    .Where(kvp => kvp.Key.StartsWith("DOTNET_", StringComparison.OrdinalIgnoreCase))
                    .Select(kvp => $"{kvp.Key}={kvp.Value}")
                    .ToArray() ?? Array.Empty<string>();

                EqtTrace.Verbose($"DotnetTestHostManager: Starting process '{0}' with command line '{1}' and DOTNET environment: {string.Join(", ", dotnetEnvVars)} ", testHostStartInfo.FileName, testHostStartInfo.Arguments);
            }

            cancellationToken.ThrowIfCancellationRequested();
            EqtTrace.Verbose("DotnetTestHostManager: Launching testhost with CreateNoWindow={0}", _createNoNewWindow);

            var outputCallback = _captureOutput ? OutputReceivedCallback : null;
            _testHostProcess = _processHelper.LaunchProcess(
                testHostStartInfo.FileName!,
                testHostStartInfo.Arguments,
                testHostStartInfo.WorkingDirectory,
                testHostStartInfo.EnvironmentVariables,
                ErrorReceivedCallback,
                ExitCallBack,
                outputCallback,
                _createNoNewWindow) as Process;
        }
        else
        {
            var processId = _customTestHostLauncher.LaunchTestHost(testHostStartInfo, cancellationToken);
            _testHostProcess = Process.GetProcessById(processId);
            _processHelper.SetExitCallback(processId, ExitCallBack);
        }

        if (_testHostProcess is null)
        {
            return false;
        }

        DefaultTestHostManager.AdjustProcessPriorityBasedOnSettings(_testHostProcess, testHostStartInfo.EnvironmentVariables);
        OnHostLaunched(new HostProviderEventArgs("Test Runtime launched", 0, _testHostProcess.Id));
        return true;
    }

    private string GetTestHostPath(string runtimeConfigDevPath, string depsFilePath, string sourceDirectory)
    {
        string testHostPackageName = "microsoft.testplatform.testhost";
        // This must be empty string, otherwise the Path.Combine below
        // will fail if a very specific setup is used where you add our dlls
        // as assemblies directly, but you have no RuntimeAssemblyGroups in deps.json
        // because you don't add our nuget package. In such case we just want to move on
        // to the next fallback.
        string testHostPath = string.Empty;

        if (_fileHelper.Exists(depsFilePath))
        {
            if (_fileHelper.Exists(runtimeConfigDevPath))
            {
                EqtTrace.Verbose("DotnetTestHostmanager: Reading file {0} to get path of testhost.dll", depsFilePath);

                // Get testhost relative path
                using (var stream = _fileHelper.GetStream(depsFilePath, FileMode.Open, FileAccess.Read))
                {
#if NETCOREAPP
                    var context = new DependencyContextJsonReader().Read(stream);
                    var testhostPackage = context.RuntimeLibraries.FirstOrDefault(lib => lib.Name.Equals(testHostPackageName, StringComparison.OrdinalIgnoreCase));

                    if (testhostPackage != null)
                    {
                        foreach (var runtimeAssemblyGroup in testhostPackage.RuntimeAssemblyGroups)
                        {
                            foreach (var path in runtimeAssemblyGroup.AssetPaths)
                            {
                                if (path.EndsWith("testhost.dll", StringComparison.OrdinalIgnoreCase))
                                {
                                    testHostPath = path;
                                    break;
                                }
                            }
                        }

                        if (testhostPackage.Path is not null)
                        {
                            testHostPath = Path.Combine(testhostPackage.Path, testHostPath);
                        }
                        _hostPackageVersion = testhostPackage.Version;
                        IsVersionCheckRequired = !_hostPackageVersion.StartsWith("15.0.0");
                        EqtTrace.Verbose("DotnetTestHostmanager: Relative path of testhost.dll with respect to package folder is {0}", testHostPath);
                    }
#else
                    var library = DepsJsonParser.FindRuntimeLibrary(stream, testHostPackageName);
                    if (library != null)
                    {
                        testHostPath = library.RuntimeAssemblyPaths
                            .FirstOrDefault(p => p.EndsWith("testhost.dll", StringComparison.OrdinalIgnoreCase))
                            ?? testHostPath;

                        if (library.Path is not null)
                        {
                            testHostPath = Path.Combine(library.Path, testHostPath);
                        }

                        _hostPackageVersion = library.Version;
                        IsVersionCheckRequired = !_hostPackageVersion.StartsWith("15.0.0");
                        EqtTrace.Verbose("DotnetTestHostmanager: Relative path of testhost.dll with respect to package folder is {0}", testHostPath);
                    }
#endif
                }

                // Get probing path
                using (var stream = _fileHelper.GetStream(runtimeConfigDevPath, FileMode.Open, FileAccess.Read))
                {
#if NETCOREAPP
                    using var doc = JsonDocument.Parse(stream);

                    if (doc.RootElement.TryGetProperty("runtimeOptions", out var runtimeOptions) &&
                        runtimeOptions.TryGetProperty("additionalProbingPaths", out var additionalProbingPaths))
                    {
                        foreach (var x in additionalProbingPaths.EnumerateArray())
                        {
                            EqtTrace.Verbose("DotnetTestHostmanager: Looking for path {0} in folder {1}", testHostPath, x.GetString());
                            string testHostFullPath;
                            try
                            {
                                testHostFullPath = Path.Combine(x.GetString()!, testHostPath);
                            }
                            catch (ArgumentException)
                            {
                                // https://github.com/Microsoft/vstest/issues/847
                                // skip any invalid paths and continue checking the others
                                continue;
                            }

                            if (_fileHelper.Exists(testHostFullPath))
                            {
                                EqtTrace.Verbose("DotnetTestHostmanager: Found testhost.dll in {0}", testHostFullPath);
                                return testHostFullPath;
                            }
                        }
                    }
#else
                    using var reader = new StreamReader(stream);
                    var parsed = Json.Deserialize(reader) as IDictionary<string, object>;
                    var runtimeOpts = parsed is not null && parsed.TryGetValue("runtimeOptions", out var runtimeOptsObj)
                        ? runtimeOptsObj as IDictionary<string, object>
                        : null;

                    var probingPaths = runtimeOpts is not null && runtimeOpts.TryGetValue("additionalProbingPaths", out var probingPathsObj)
                        ? probingPathsObj as IList<object>
                        : null;

                    if (probingPaths is not null)
                    {
                        foreach (var x in probingPaths)
                        {
                            var pathStr = x?.ToString();
                            EqtTrace.Verbose("DotnetTestHostmanager: Looking for path {0} in folder {1}", testHostPath, pathStr);
                            string testHostFullPath;
                            try
                            {
                                testHostFullPath = Path.Combine(pathStr!, testHostPath);
                            }
                            catch (ArgumentException)
                            {
                                continue;
                            }

                            if (_fileHelper.Exists(testHostFullPath))
                            {
                                EqtTrace.Verbose("DotnetTestHostmanager: Found testhost.dll in {0}", testHostFullPath);
                                return testHostFullPath;
                            }
                        }
                    }
#endif
                }
            }
            else
            {
                EqtTrace.Verbose("DotnetTestHostmanager: Runtimeconfig.dev.json {0} does not exist.", runtimeConfigDevPath);
            }
        }
        else
        {
            EqtTrace.Verbose("DotnetTestHostmanager: Deps file {0} does not exist.", depsFilePath);
        }

        // If we are here it means it couldn't resolve testhost.dll from nuget cache.
        // Try resolving testhost from output directory of test project. This is required if user has published the test project
        // and is running tests in an isolated machine. A second scenario is self test: test platform unit tests take a project
        // dependency on testhost (instead of nuget dependency), this drops testhost to output path.
        var testHostNextToTestProject = Path.Combine(sourceDirectory, "testhost.dll");
        if (_fileHelper.Exists(testHostNextToTestProject))
        {
            EqtTrace.Verbose("DotnetTestHostManager: Found testhost.dll in source directory: {0}.", testHostNextToTestProject);
            return testHostNextToTestProject;
        }

        return testHostPath;
    }
}
