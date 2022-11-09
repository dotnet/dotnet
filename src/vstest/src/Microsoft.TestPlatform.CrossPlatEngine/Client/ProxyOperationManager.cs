// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommunicationUtilitiesResources = Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources.Resources;
using CoreUtilitiesConstants = Microsoft.VisualStudio.TestPlatform.CoreUtilities.Constants;
using CrossPlatEngineResources = Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

/// <summary>
/// Base class for any operations that the client needs to drive through the engine.
/// </summary>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Would cause a breaking change if users are inheriting this class and implement IDisposable")]
public class ProxyOperationManager
{
    private readonly string _versionCheckPropertyName = "IsVersionCheckRequired";
    private readonly string _makeRunsettingsCompatiblePropertyName = "MakeRunsettingsCompatible";
    private readonly ManualResetEventSlim _testHostExited = new(false);
    private readonly IProcessHelper _processHelper;
    private readonly IBaseProxy? _baseProxy;

    private bool _versionCheckRequired = true;
    private bool _makeRunsettingsCompatible;
    private bool _makeRunsettingsCompatibleSet;
    private bool _initialized;
    private bool _testHostLaunched;
    private int _testHostProcessId;
    private string? _testHostProcessStdError;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyOperationManager"/> class.
    /// </summary>
    ///
    /// <param name="requestData">Request data instance.</param>
    /// <param name="requestSender">Request sender instance.</param>
    /// <param name="testHostManager">Test host manager instance.</param>
    public ProxyOperationManager(
        IRequestData? requestData,
        ITestRequestSender requestSender,
        ITestRuntimeProvider testHostManager,
        Framework testhostManagerFramework)
        : this(
            requestData,
            requestSender,
            testHostManager,
            testhostManagerFramework,
            null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyOperationManager"/> class.
    /// </summary>
    ///
    /// <param name="requestData">Request data instance.</param>
    /// <param name="requestSender">Request sender instance.</param>
    /// <param name="testHostManager">Test host manager instance.</param>
    /// <param name="baseProxy">The base proxy.</param>
    public ProxyOperationManager(
        IRequestData? requestData,
        ITestRequestSender requestSender,
        ITestRuntimeProvider testHostManager,
        Framework? testhostManagerFramework,
        IBaseProxy? baseProxy)
    {
        RequestData = requestData;
        RequestSender = requestSender;
        TestHostManager = testHostManager;
        _baseProxy = baseProxy;

        _initialized = false;
        _testHostLaunched = false;
        _testHostProcessId = -1;
        _processHelper = new ProcessHelper();
        CancellationTokenSource = new CancellationTokenSource();
        TestHostManagerFramework = testhostManagerFramework;
    }

    /// <summary>
    /// Gets or sets the request data.
    /// </summary>
    public IRequestData? RequestData { get; set; }

    /// <summary>
    /// Gets or sets the server for communication.
    /// </summary>
    public ITestRequestSender RequestSender { get; set; }

    /// <summary>
    /// Gets or sets the test host manager.
    /// </summary>
    public ITestRuntimeProvider TestHostManager { get; set; }

    /// <summary>
    /// Gets the proxy operation manager id for proxy test session manager internal organization.
    /// </summary>
    public int Id { get; set; } = -1;

    /// <summary>
    /// Gets or sets the cancellation token source.
    /// </summary>
    public CancellationTokenSource CancellationTokenSource { get; set; }

    public Framework? TestHostManagerFramework { get; }

    #region IProxyOperationManager implementation.
    /// <summary>
    /// Initializes the proxy.
    /// </summary>
    ///
    /// <param name="skipDefaultAdapters">
    /// Flag indicating if we should skip the default adapters initialization.
    /// </param>
    public virtual void Initialize(bool skipDefaultAdapters)
    {
        // No-op.
    }

    /// <summary>
    /// Ensures that the engine is ready for test operations. Usually includes starting up the
    /// test host process.
    /// </summary>
    ///
    /// <param name="sources">List of test sources.</param>
    /// <param name="runSettings">Run settings to be used.</param>
    /// <param name="eventHandler">The events handler.</param>
    ///
    /// <returns>
    /// Returns true if the communication is established b/w runner and host, false otherwise.
    /// </returns>
    public virtual bool SetupChannel(
        IEnumerable<string> sources,
        string runSettings,
        ITestMessageEventHandler eventHandler)
    {
        // NOTE: Event handler is ignored here, but it is used in the overloaded method.
        return SetupChannel(sources, runSettings);
    }

    /// <summary>
    /// Ensures that the engine is ready for test operations. Usually includes starting up the
    /// test host process.
    /// </summary>
    ///
    /// <param name="sources">List of test sources.</param>
    /// <param name="runSettings">Run settings to be used.</param>
    ///
    /// <returns>
    /// Returns true if the communication is established b/w runner and host, false otherwise.
    /// </returns>
    public virtual bool SetupChannel(IEnumerable<string> sources, string? runSettings)
    {
        CancellationTokenSource.Token.ThrowTestPlatformExceptionIfCancellationRequested();

        if (_initialized)
        {
            return true;
        }

        var connTimeout = EnvironmentHelper.GetConnectionTimeout();

        _testHostProcessStdError = string.Empty;
        TestHostConnectionInfo testHostConnectionInfo = TestHostManager.GetTestHostConnectionInfo();

        var portNumber = 0;
        if (testHostConnectionInfo.Role == ConnectionRole.Client)
        {
            portNumber = RequestSender.InitializeCommunication();
            testHostConnectionInfo.Endpoint += portNumber;
        }

        var processId = _processHelper.GetCurrentProcessId();
        var connectionInfo = new TestRunnerConnectionInfo()
        {
            Port = portNumber,
            ConnectionInfo = testHostConnectionInfo,
            RunnerProcessId = processId,
            LogFile = GetTimestampedLogFile(EqtTrace.LogFile),
            TraceLevel = (int)EqtTrace.TraceLevel
        };

        // Subscribe to test host events.
        TestHostManager.HostLaunched += TestHostManagerHostLaunched;
        TestHostManager.HostExited += TestHostManagerHostExited;

        // Get environment variables from run settings.
        var envVars = InferRunSettingsHelper.GetEnvironmentVariables(runSettings);

        // Get the test process start info.
        var testHostStartInfo = UpdateTestProcessStartInfo(
            TestHostManager.GetTestHostProcessStartInfo(
                sources,
                envVars,
                connectionInfo));
        try
        {
            // Launch the test host.
            _testHostLaunched = TestHostManager.LaunchTestHostAsync(
                testHostStartInfo,
                CancellationTokenSource.Token).Result;

            if (_testHostLaunched && testHostConnectionInfo.Role == ConnectionRole.Host)
            {
                // If test runtime is service host, try to poll for connection as client.
                RequestSender.InitializeCommunication();
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("ProxyOperationManager: Failed to launch testhost :{0}", ex);

            CancellationTokenSource.Token.ThrowTestPlatformExceptionIfCancellationRequested();
            throw new TestPlatformException(string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.FailedToLaunchTestHost, ex.ToString()));
        }

        // Warn the user that execution will wait for debugger attach.
        var hostDebugEnabled = Environment.GetEnvironmentVariable("VSTEST_HOST_DEBUG");
        var nativeHostDebugEnabled = Environment.GetEnvironmentVariable("VSTEST_HOST_NATIVE_DEBUG");

        if ((!StringUtils.IsNullOrEmpty(hostDebugEnabled)
             && hostDebugEnabled.Equals("1", StringComparison.Ordinal))
            || (new PlatformEnvironment().OperatingSystem.Equals(PlatformOperatingSystem.Windows)
                && !StringUtils.IsNullOrEmpty(nativeHostDebugEnabled)
                && nativeHostDebugEnabled.Equals("1", StringComparison.Ordinal)))
        {
            ConsoleOutput.Instance.WriteLine(
                CrossPlatEngineResources.HostDebuggerWarning,
                OutputLevel.Warning);

            ConsoleOutput.Instance.WriteLine(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Process Id: {0}, Name: {1}",
                    _testHostProcessId,
                    _processHelper.GetProcessName(_testHostProcessId)),
                OutputLevel.Information);

            // Increase connection timeout when debugging is enabled.
            connTimeout *= 5;
        }

        // If test host does not launch then throw exception, otherwise wait for connection.
        if (!_testHostLaunched
            || !RequestSender.WaitForRequestHandlerConnection(
                connTimeout * 1000,
                CancellationTokenSource.Token))
        {
            EqtTrace.Verbose($"Test host failed to start Test host launched:{_testHostLaunched} test host exited: {_testHostExited.IsSet}");
            // Throw a test platform exception with the appropriate message if user requested cancellation.
            CancellationTokenSource.Token.ThrowTestPlatformExceptionIfCancellationRequested();

            // Throw a test platform exception along with the error messages from the test if the test host exited unexpectedly
            // before communication was established.
            ThrowOnTestHostExited(sources, _testHostExited.IsSet);

            // Throw a test platform exception stating the connection to test could not be established even after waiting
            // for the configure timeout period.
            ThrowExceptionOnConnectionFailure(sources, connTimeout);
        }

        // Handling special case for dotnet core projects with older test hosts.
        // Older test hosts are not aware of protocol version check, hence we should not be
        // sending VersionCheck message to these test hosts.
        CompatIssueWithVersionCheckAndRunsettings();
        if (_versionCheckRequired)
        {
            RequestSender.CheckVersionWithTestHost();
        }

        _initialized = true;

        return true;
    }

    /// <summary>
    /// Closes the channel and terminates the test host process.
    /// </summary>
    public virtual void Close()
    {
        try
        {
            // Do not send message if the host did not launch.
            if (_testHostLaunched)
            {
                RequestSender.EndSession();

                // We want to give test host a chance to safely close.
                // The upper bound for wait should be 100ms.
                var timeout = int.TryParse(Environment.GetEnvironmentVariable("VSTEST_TESTHOST_SHUTDOWN_TIMEOUT"), out var time)
                    ? time
                    : 100;
                EqtTrace.Verbose("ProxyOperationManager.Close: waiting for test host to exit for {0} ms", timeout);
                if (!_testHostExited.Wait(timeout))
                {
                    EqtTrace.Warning("ProxyOperationManager: Timed out waiting for test host to exit. Will terminate process.");
                }

                // Closing the communication channel.
                RequestSender.Close();
            }
        }
        catch (Exception ex)
        {
            // Error in sending an end session is not necessarily a failure. Discovery and execution should be already
            // complete at this time.
            EqtTrace.Warning("ProxyOperationManager: Failed to end session: " + ex);
        }
        finally
        {
            _initialized = false;

            // This is calling external code, make sure we don't fail when it throws
            try
            {
                // Please clean up test host.
                TestHostManager.CleanTestHostAsync(CancellationToken.None).Wait();
            }
            catch (Exception ex)
            {
                EqtTrace.Error($"ProxyOperationManager: Cleaning testhost failed: {ex}");
            }

            TestHostManager.HostExited -= TestHostManagerHostExited;
            TestHostManager.HostLaunched -= TestHostManagerHostLaunched;
        }
    }

    #endregion

    /// <summary>
    /// This method is exposed to enable derived classes to modify
    /// <see cref="TestProcessStartInfo"/>. For example, data collectors need additional
    /// environment variables to be passed.
    /// </summary>
    ///
    /// <param name="testProcessStartInfo">The test process start info.</param>
    ///
    /// <returns>
    /// The <see cref="TestProcessStartInfo"/>.
    /// </returns>
    public virtual TestProcessStartInfo UpdateTestProcessStartInfo(TestProcessStartInfo testProcessStartInfo)
    {
        // TODO (copoiena): If called and testhost is already running, we should restart.
        if (_baseProxy == null)
        {
            // Update Telemetry Opt in status because by default in Test Host Telemetry is opted out
            var telemetryOptedIn = RequestData?.IsTelemetryOptedIn == true ? "true" : "false";
            testProcessStartInfo.Arguments += " --telemetryoptedin " + telemetryOptedIn;
            return testProcessStartInfo;
        }

        return _baseProxy.UpdateTestProcessStartInfo(testProcessStartInfo);
    }

    /// <summary>
    /// This function will remove the unknown run settings nodes from the run settings strings.
    /// This is necessary because older test hosts may throw exceptions when encountering
    /// unknown nodes.
    /// </summary>
    ///
    /// <param name="runsettingsXml">Run settings string.</param>
    ///
    /// <returns>The run settings after removing non-required nodes.</returns>
    public string? RemoveNodesFromRunsettingsIfRequired(string? runsettingsXml, Action<TestMessageLevel, string> logMessage)
    {
        var updatedRunSettingsXml = runsettingsXml;
        if (!_makeRunsettingsCompatibleSet)
        {
            CompatIssueWithVersionCheckAndRunsettings();
        }

        if (_makeRunsettingsCompatible)
        {
            logMessage.Invoke(TestMessageLevel.Warning, CrossPlatEngineResources.OldTestHostIsGettingUsed);
            updatedRunSettingsXml = InferRunSettingsHelper.MakeRunsettingsCompatible(runsettingsXml);
        }

        // We can remove "TargetPlatform" because is not needed, process is already in a "specific" target platform after test host process start,
        // so the default architecture is always the correct one.
        // This allow us to support new architecture enumeration without the need to update old test sdk.
        updatedRunSettingsXml = InferRunSettingsHelper.RemoveTargetPlatformElement(updatedRunSettingsXml);

        return updatedRunSettingsXml;
    }

    [return: NotNullIfNotNull("logFile")]
    private static string? GetTimestampedLogFile(string? logFile)
    {
        return logFile.IsNullOrWhiteSpace()
            ? null
            : Path.ChangeExtension(
                logFile,
                string.Format(
                    CultureInfo.InvariantCulture,
                    "host.{0}_{1}{2}",
                    DateTime.Now.ToString("yy-MM-dd_HH-mm-ss_fffff", CultureInfo.InvariantCulture),
                    new PlatformEnvironment().GetCurrentManagedThreadId(),
                    Path.GetExtension(logFile))).AddDoubleQuote();
    }

    private void CompatIssueWithVersionCheckAndRunsettings()
    {
        var properties = TestHostManager.GetType().GetRuntimeProperties();

        // The field is actually defaulting to true, so this is just a complicated way to set or not set
        // this to true (modern testhosts should have it set to true). Bad thing about this is that we are checking
        // internal "undocumented" property. Good thing is that if you don't implement it you get the modern behavior.
        var versionCheckProperty = properties.FirstOrDefault(p => string.Equals(p.Name, _versionCheckPropertyName, StringComparison.OrdinalIgnoreCase));
        if (versionCheckProperty != null)
        {
            _versionCheckRequired = (bool)versionCheckProperty.GetValue(TestHostManager)!;
        }

        var makeRunsettingsCompatibleProperty = properties.FirstOrDefault(p => string.Equals(p.Name, _makeRunsettingsCompatiblePropertyName, StringComparison.OrdinalIgnoreCase));
        if (makeRunsettingsCompatibleProperty != null)
        {
            _makeRunsettingsCompatible = (bool)makeRunsettingsCompatibleProperty.GetValue(TestHostManager)!;
            _makeRunsettingsCompatibleSet = true;
        }
    }

    private void TestHostManagerHostLaunched(object? sender, HostProviderEventArgs? e)
    {
        EqtTrace.Verbose(e!.Data);
        _testHostProcessId = e.ProcessId;
    }

    private void TestHostManagerHostExited(object? sender, HostProviderEventArgs? e)
    {
        EqtTrace.Verbose("CrossPlatEngine.TestHostManagerHostExited: calling on client process exit callback.");
        _testHostProcessStdError = e!.Data;

        // This needs to be set before we call the OnClientProcess exit because the
        // OnClientProcess will short-circuit WaitForRequestHandlerConnection in SetupChannel
        // that then continues to throw an exception and checks if the test host process exited.
        // If not it reports timeout, if we don't set this before OnClientProcessExit we will
        // report timeout even though we exited the test host before even attempting the connect.
        _testHostExited.Set();
        RequestSender.OnClientProcessExit(_testHostProcessStdError);
    }

    private void ThrowOnTestHostExited(IEnumerable<string> sources, bool testHostExited)
    {
        if (testHostExited)
        {
            // We might consider passing standard output here in case standard error is not
            // available because some errors don't end up in the standard error output.
            throw new TestPlatformException(string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.TestHostExitedWithError, string.Join("', '", sources), _testHostProcessStdError));
        }
    }

    private void ThrowExceptionOnConnectionFailure(IEnumerable<string> sources, int connTimeout)
    {
        // Failed to launch testhost process.
        var errorMsg = CrossPlatEngineResources.InitializationFailed;

        // Testhost launched but timeout occurred due to machine slowness.
        if (_testHostLaunched)
        {
            errorMsg = string.Format(
                CultureInfo.CurrentCulture,
                CommunicationUtilitiesResources.ConnectionTimeoutErrorMessage,
                CoreUtilitiesConstants.VstestConsoleProcessName,
                CoreUtilitiesConstants.TesthostProcessName,
                connTimeout,
                EnvironmentHelper.VstestConnectionTimeout);
        }

        // After testhost process launched failed with error.
        if (!StringUtils.IsNullOrWhiteSpace(_testHostProcessStdError))
        {
            // Testhost failed with error.
            errorMsg = string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.TestHostExitedWithError, string.Join("', '", sources), _testHostProcessStdError);
        }

        throw new TestPlatformException(errorMsg);
    }
}
