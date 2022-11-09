// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.Common.Hosting;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;

/// <summary>
/// Cross platform test engine entry point for the client.
/// </summary>
public class TestEngine : ITestEngine
{
    private readonly ITestRuntimeProviderManager _testHostProviderManager;
    private readonly IProcessHelper _processHelper;
    private readonly IEnvironment _environment;

    private ITestExtensionManager? _testExtensionManager;

    public TestEngine()
        : this(TestRuntimeProviderManager.Instance, new ProcessHelper())
    {
    }

    protected internal TestEngine(
        TestRuntimeProviderManager testHostProviderManager,
        IProcessHelper processHelper)
        : this(testHostProviderManager, processHelper, new PlatformEnvironment())
    {
    }

    internal TestEngine(
        ITestRuntimeProviderManager testHostProviderManager,
        IProcessHelper processHelper,
        IEnvironment environment)
    {
        _testHostProviderManager = testHostProviderManager;
        _processHelper = processHelper;
        _environment = environment;
    }

    #region ITestEngine implementation

    /// <inheritdoc/>
    public IProxyDiscoveryManager GetDiscoveryManager(
        IRequestData requestData,
        DiscoveryCriteria discoveryCriteria,
        IDictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger)
    {
        // Parallel level determines how many processes at most we should start at the same time. We take the number from settings, and if user
        // has no preference or the preference is 0 then we use the number of logical processors. Or the number of sources, whatever is lower.
        // We don't know for sure if we will start that many processes as some of the sources can run in a single testhost. This is determined by
        // Shared on the test runtime provider. At this point we need to know only if the parallel level is more than 1, and so if we will do parallel
        // run or not.
        var parallelLevel = VerifyParallelSettingAndCalculateParallelLevel(
            discoveryCriteria.Sources.Count(),
            discoveryCriteria.RunSettings!);

        var isParallelRun = parallelLevel > 1;

        // Collecting IsParallel enabled.
        requestData.MetricsCollection.Add(TelemetryDataConstants.ParallelEnabledDuringDiscovery, isParallelRun ? "True" : "False");
        requestData.MetricsCollection.Add(TelemetryDataConstants.TestSessionId, discoveryCriteria.TestSessionInfo?.Id.ToString() ?? string.Empty);

        // Get testhost managers by configuration, and either use it for in-process run. or for single source run.
        List<TestRuntimeProviderInfo> testHostManagers = GetTestRuntimeProvidersForUniqueConfigurations(discoveryCriteria.RunSettings!, sourceToSourceDetailMap, warningLogger, out ITestRuntimeProvider? testHostManager);

        // This is a big if that figures out if we can run in process. In process run is very restricted, it is non-parallel run
        // that has the same target framework as the current process, and it also must not be running in DesignMode (server mode / under IDE)
        // and more conditions. In all other cases we run in a separate testhost process.
        if (ShouldRunInProcess(discoveryCriteria.RunSettings!, isParallelRun, isDataCollectorEnabled: false, testHostManagers))
        {
            // We are running in process, so whatever the architecture and framework that was figured out is, it must be compatible. If we have more
            // changes that we want to do to runsettings in the future, based on SourceDetail then it will depend on those details. But in general
            // we will have to check that all source details are the same. Otherwise we for sure cannot run in process.
            // E.g. if we get list of sources where one of them has different architecture we for sure cannot run in process, because the current
            // process can handle only single runsettings.
            if (testHostManagers.Count != 1)
            {
                throw new InvalidOperationException($"Exactly 1 testhost manager must be provided when running in process, but there {testHostManagers.Count} were provided.");
            }
            var testHostManagerInfo = testHostManagers[0];

            TPDebug.Assert(testHostManager is not null, "testHostManager is null");
            // Don't intialize, we are taking an instance that we created already initialized in GetTestRuntimeProvidersForUniqueConfigurations
            // testHostManager.Initialize(TestSessionMessageLogger.Instance, testHostManagerInfo.RunSettings);

            return new InProcessProxyDiscoveryManager(
                testHostManager,
                new TestHostManagerFactory(requestData.IsTelemetryOptedIn));
        }

        // Create one data aggregator per parallel discovery and share it with all the proxy discovery managers.
        // We need to share the aggregator because when cancelling discovery we don't want to await all managers,
        // and so the first manager replying with the discovery complete (aborted) event arg will cause the parallel
        // discovery manager to publish its current state. But doing so we are losing the collected state of all the
        // other managers.
        var discoveryDataAggregator = new DiscoveryDataAggregator();
        Func<TestRuntimeProviderInfo, IProxyDiscoveryManager> proxyDiscoveryManagerCreator = runtimeProviderInfo =>
        {
            var sources = runtimeProviderInfo.SourceDetails.Select(r => r.Source!).ToList();
            var hostManager = _testHostProviderManager.GetTestHostManagerByRunConfiguration(runtimeProviderInfo.RunSettings, sources);
            hostManager?.Initialize(TestSessionMessageLogger.Instance, runtimeProviderInfo.RunSettings!);

            ThrowExceptionIfTestHostManagerIsNull(hostManager, runtimeProviderInfo.RunSettings);
            TPDebug.Assert(hostManager is not null, "hostManager is null");

            // This function is used to either take a pre-existing proxy operation manager from
            // the test pool or to create a new proxy operation manager on the spot.
            Func<string, ProxyDiscoveryManager, ProxyOperationManager>
                proxyOperationManagerCreator = (
                    string source,
                    ProxyDiscoveryManager proxyDiscoveryManager) =>
                {
                    TPDebug.Assert(discoveryCriteria.TestSessionInfo is not null, "discoveryCriteria.TestSessionInfo is null");

                    // In case we have an active test session, we always prefer the already
                    // created proxies instead of the ones that need to be created on the spot.
                    var proxyOperationManager = TestSessionPool.Instance.TryTakeProxy(
                        discoveryCriteria.TestSessionInfo,
                        source,
                        runtimeProviderInfo.RunSettings,
                        requestData);

                    if (proxyOperationManager == null)
                    {
                        // If the proxy creation process based on test session info failed, then
                        // we'll proceed with the normal creation process as if no test session
                        // info was passed in in the first place.
                        //
                        // WARNING: This should not normally happen and it raises questions
                        // regarding the test session pool operation and consistency.
                        EqtTrace.Warning("ProxyDiscoveryManager creation with test session failed.");

                        proxyOperationManager = new ProxyOperationManager(
                            requestData,
                            new TestRequestSender(requestData.ProtocolConfig!, hostManager),
                            hostManager,
                            // There is always at least one, and all of them have the same framework and architecture.
                            runtimeProviderInfo.SourceDetails[0].Framework,
                            proxyDiscoveryManager);
                    }

                    return proxyOperationManager;
                };

            // In case we have an active test session, we always prefer the already
            // created proxies instead of the ones that need to be created on the spot.
            return (discoveryCriteria.TestSessionInfo != null)
                ? new ProxyDiscoveryManager(
                    discoveryCriteria.TestSessionInfo,
                    proxyOperationManagerCreator,
                    discoveryDataAggregator)
                : new ProxyDiscoveryManager(
                    requestData,
                    new TestRequestSender(requestData.ProtocolConfig!, hostManager),
                    hostManager,
                    // There is always at least one, and all of them have the same framework and architecture.
                    runtimeProviderInfo.SourceDetails[0].Framework,
                    discoveryDataAggregator);
        };

        return new ParallelProxyDiscoveryManager(requestData, proxyDiscoveryManagerCreator, discoveryDataAggregator, parallelLevel, testHostManagers);
    }

    /// <inheritdoc/>
    public IProxyExecutionManager GetExecutionManager(
        IRequestData requestData,
        TestRunCriteria testRunCriteria,
        IDictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger)
    {
        TPDebug.Assert(testRunCriteria.TestRunSettings is not null, "testRunCriteria.TestRunSettings is null");

        // We use mulitple "different" runsettings here. We have runsettings that come with the testRunCriteria,
        // and we use that to figure out the common stuff before we try to setup the run. Later we patch the settings
        // from the additional details that were passed. Those should not affect the common properties that are used for setup.
        // Right now the only two things that change there are the architecture and framework so we can mix them in a single run.
        var distinctSources = GetDistinctNumberOfSources(testRunCriteria);
        var parallelLevel = VerifyParallelSettingAndCalculateParallelLevel(distinctSources, testRunCriteria.TestRunSettings);

        // See comments in GetDiscoveryManager for more info about what is happening in this method.
        var isParallelRun = parallelLevel > 1;

        // Collecting IsParallel enabled.
        requestData.MetricsCollection.Add(TelemetryDataConstants.ParallelEnabledDuringExecution, isParallelRun ? "True" : "False");
        requestData.MetricsCollection.Add(TelemetryDataConstants.TestSessionId, testRunCriteria.TestSessionInfo?.Id.ToString() ?? string.Empty);

        var isDataCollectorEnabled = XmlRunSettingsUtilities.IsDataCollectionEnabled(testRunCriteria.TestRunSettings);
        var isInProcDataCollectorEnabled = XmlRunSettingsUtilities.IsInProcDataCollectionEnabled(testRunCriteria.TestRunSettings);

        var testHostProviders = GetTestRuntimeProvidersForUniqueConfigurations(testRunCriteria.TestRunSettings, sourceToSourceDetailMap, warningLogger, out ITestRuntimeProvider? testHostManager);

        if (ShouldRunInProcess(
                testRunCriteria.TestRunSettings,
                isParallelRun,
                isDataCollectorEnabled || isInProcDataCollectorEnabled,
                testHostProviders))
        {
            // Not updating runsettings from source detail on purpose here. We are running in process, so whatever the settings we figured out at the start. They must be compatible
            // with the current process, otherwise we would not be able to run inside of the current process.
            //
            // We know that we only have a single testHostManager here, because we figure that out in ShouldRunInProcess.
            ThrowExceptionIfTestHostManagerIsNull(testHostManager, testRunCriteria.TestRunSettings);
            TPDebug.Assert(testHostManager is not null, "testHostManager is null");

            // Don't intialize, we are taking an instance that we created already initialized in GetTestRuntimeProvidersForUniqueConfigurations
            // testHostManager.Initialize(TestSessionMessageLogger.Instance, testRunCriteria.TestRunSettings);

            // NOTE: The custom launcher should not be set when we have test session info available.
            if (testRunCriteria.TestHostLauncher != null)
            {
                testHostManager!.SetCustomLauncher(testRunCriteria.TestHostLauncher);
            }

            return new InProcessProxyExecutionManager(
                testHostManager,
                new TestHostManagerFactory(requestData.IsTelemetryOptedIn));
        }

        // This creates a single non-parallel execution manager, based requestData, isDataCollectorEnabled and the
        // overall testRunCriteria. The overall testRunCriteria are split to smaller pieces (e.g. each source from the overall
        // testRunCriteria) so we can run them in parallel, and those are then passed to those non-parallel execution managers.
        //
        // The function below grabs most of the parameter via closure from the local context,
        // but gets the runtime provider later, because that is specific info to the source (or sources) it will be running.
        // This creator does not get those smaller pieces of testRunCriteria, those come later when we call a method on
        // the non-parallel execution manager we create here. E.g. StartTests(<single piece of testRunCriteria>).
        Func<TestRuntimeProviderInfo, IProxyExecutionManager> proxyExecutionManagerCreator = runtimeProviderInfo =>
            CreateNonParallelExecutionManager(requestData, testRunCriteria, isDataCollectorEnabled, runtimeProviderInfo);

        var executionManager = new ParallelProxyExecutionManager(requestData, proxyExecutionManagerCreator, parallelLevel, testHostProviders);

        EqtTrace.Verbose($"TestEngine.GetExecutionManager: Chosen execution manager '{executionManager.GetType().AssemblyQualifiedName}' ParallelLevel '{parallelLevel}'.");

        return executionManager;
    }

    // This is internal so tests can use it.
    internal IProxyExecutionManager CreateNonParallelExecutionManager(IRequestData requestData, TestRunCriteria testRunCriteria, bool isDataCollectorEnabled, TestRuntimeProviderInfo runtimeProviderInfo)
    {
        // SetupChannel ProxyExecutionManager with data collection if data collectors are
        // specified in run settings.
        // Create a new host manager, to be associated with individual
        // ProxyExecutionManager(&POM)
        var sources = runtimeProviderInfo.SourceDetails.Select(r => r.Source!).ToList();
        var hostManager = _testHostProviderManager.GetTestHostManagerByRunConfiguration(runtimeProviderInfo.RunSettings, sources);
        ThrowExceptionIfTestHostManagerIsNull(hostManager, runtimeProviderInfo.RunSettings);
        hostManager!.Initialize(TestSessionMessageLogger.Instance, runtimeProviderInfo.RunSettings!);

        if (testRunCriteria.TestHostLauncher != null)
        {
            hostManager.SetCustomLauncher(testRunCriteria.TestHostLauncher);
        }

        var requestSender = new TestRequestSender(requestData.ProtocolConfig!, hostManager);

        if (testRunCriteria.TestSessionInfo != null)
        {
            // This function is used to either take a pre-existing proxy operation manager from
            // the test pool or to create a new proxy operation manager on the spot.
            Func<string, ProxyExecutionManager, ProxyOperationManager>
                proxyOperationManagerCreator = (
                    string source,
                    ProxyExecutionManager proxyExecutionManager) =>
                {
                    var proxyOperationManager = TestSessionPool.Instance.TryTakeProxy(
                        testRunCriteria.TestSessionInfo,
                        source,
                        runtimeProviderInfo.RunSettings,
                        requestData);

                    if (proxyOperationManager == null)
                    {
                        // If the proxy creation process based on test session info failed, then
                        // we'll proceed with the normal creation process as if no test session
                        // info was passed in in the first place.
                        //
                        // WARNING: This should not normally happen and it raises questions
                        // regarding the test session pool operation and consistency.
                        EqtTrace.Warning("ProxyExecutionManager creation with test session failed.");

                        proxyOperationManager = new ProxyOperationManager(
                            requestData,
                            requestSender,
                            hostManager,
                            // There is always at least one, and all of them have the same framework and architecture.
                            runtimeProviderInfo.SourceDetails[0].Framework,
                            proxyExecutionManager);
                    }

                    return proxyOperationManager;
                };

            // In case we have an active test session, data collection needs were
            // already taken care of when first creating the session. As a consequence
            // we always return this proxy instead of choosing between the vanilla
            // execution proxy and the one with data collection enabled.
            return new ProxyExecutionManager(
                testRunCriteria.TestSessionInfo,
                proxyOperationManagerCreator,
                testRunCriteria.DebugEnabledForTestSession);
        }

        return isDataCollectorEnabled
            ? new ProxyExecutionManagerWithDataCollection(
                requestData,
                requestSender,
                hostManager,
                // There is always at least one, and all of them have the same framework and architecture.
                runtimeProviderInfo.SourceDetails[0].Framework!,
                new ProxyDataCollectionManager(
                    requestData,
                    runtimeProviderInfo.RunSettings,
                    sources))
            : new ProxyExecutionManager(
                requestData,
                requestSender,
                hostManager,
                // There is always at least one, and all of them have the same framework and architecture.
                runtimeProviderInfo.SourceDetails[0].Framework!);
    }

    /// <inheritdoc/>
    public IProxyTestSessionManager? GetTestSessionManager(
        IRequestData requestData,
        StartTestSessionCriteria testSessionCriteria,
        IDictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger)
    {
        var parallelLevel = VerifyParallelSettingAndCalculateParallelLevel(
            testSessionCriteria.Sources!.Count,
            testSessionCriteria.RunSettings!);

        bool isParallelRun = parallelLevel > 1;
        requestData.MetricsCollection.Add(
            TelemetryDataConstants.ParallelEnabledDuringStartTestSession,
            isParallelRun ? "True" : "False");

        var isDataCollectorEnabled = XmlRunSettingsUtilities.IsDataCollectionEnabled(testSessionCriteria.RunSettings);
        var isInProcDataCollectorEnabled = XmlRunSettingsUtilities.IsInProcDataCollectionEnabled(testSessionCriteria.RunSettings);

        List<TestRuntimeProviderInfo> testRuntimeProviders = GetTestRuntimeProvidersForUniqueConfigurations(testSessionCriteria.RunSettings!, sourceToSourceDetailMap, warningLogger, out var _);

        if (ShouldRunInProcess(
                testSessionCriteria.RunSettings!,
                isParallelRun,
                isDataCollectorEnabled || isInProcDataCollectorEnabled,
                testRuntimeProviders))
        {
            // In this case all tests will be run in the current process (vstest.console), so there is no
            // testhost to pre-start. No session will be created, and the session info will be null.
            return null;
        }

        Func<TestRuntimeProviderInfo, ProxyOperationManager?> proxyCreator = testRuntimeProviderInfo =>
        {
            var sources = testRuntimeProviderInfo.SourceDetails.Select(x => x.Source!).ToList();
            var hostManager = _testHostProviderManager.GetTestHostManagerByRunConfiguration(testRuntimeProviderInfo.RunSettings, sources);
            ThrowExceptionIfTestHostManagerIsNull(hostManager, testRuntimeProviderInfo.RunSettings);

            hostManager!.Initialize(TestSessionMessageLogger.Instance, testRuntimeProviderInfo.RunSettings!);
            if (testSessionCriteria.TestHostLauncher != null)
            {
                hostManager.SetCustomLauncher(testSessionCriteria.TestHostLauncher);
            }

            var requestSender = new TestRequestSender(requestData.ProtocolConfig!, hostManager)
            {
                CloseConnectionOnOperationComplete = false
            };

            // TODO (copoiena): For now we don't support data collection alongside test
            // sessions.
            //
            // The reason for this is that, in the case of Code Coverage for example, the
            // data collector needs to pass some environment variables to the testhost process
            // before the testhost process is started. This means that the data collector must
            // be running when the testhost process is spawned, however the testhost process
            // should be spawned during build, and it's problematic to have the data collector
            // running during build because it must instrument the .dll files that don't exist
            // yet.
            return isDataCollectorEnabled
                ? null
                // ? new ProxyOperationManagerWithDataCollection(
                //     requestData,
                //     requestSender,
                //     hostManager,
                //     new ProxyDataCollectionManager(
                //         requestData,
                //         runsettingsXml,
                //         testSessionCriteria.Sources))
                //     {
                //         CloseRequestSenderChannelOnProxyClose = true
                //     }
                : new ProxyOperationManager(
                    requestData,
                    requestSender,
                    hostManager,
                    // There is always at least one, and all of them have the same framework and architecture.
                    testRuntimeProviderInfo.SourceDetails[0].Framework!);
        };

        // TODO: This condition should be returning the maxParallel level to avoid pre-starting way too many testhosts, because maxParallel level,
        // can be smaller than the number of sources to run.
        var maxTesthostCount = isParallelRun ? testSessionCriteria.Sources.Count : 1;

        return new ProxyTestSessionManager(testSessionCriteria, maxTesthostCount, proxyCreator, testRuntimeProviders);
    }

    private List<TestRuntimeProviderInfo> GetTestRuntimeProvidersForUniqueConfigurations(
        string runSettings,
        IDictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger,
        out ITestRuntimeProvider? mostRecentlyCreatedInstance)
    {
        // Group source details to get unique frameworks and architectures for which we will run, so we can figure
        // out which runtime providers would run them, and if the runtime provider is shared or not.
        mostRecentlyCreatedInstance = null;
        var testRuntimeProviders = new List<TestRuntimeProviderInfo>();
        var uniqueRunConfigurations = sourceToSourceDetailMap.Values.GroupBy(k => $"{k.Framework}|{k.Architecture}");
        foreach (var runConfiguration in uniqueRunConfigurations)
        {
            // It is okay to take the first (or any) source detail in the group. We are grouping to get the same source detail, so all architectures and frameworks are the same.
            var sourceDetail = runConfiguration.First();
            var runsettingsXml = SourceDetailHelper.UpdateRunSettingsFromSourceDetail(runSettings, sourceDetail);
            var sources = runConfiguration.Select(c => c.Source!).ToList();
            var testRuntimeProvider = _testHostProviderManager.GetTestHostManagerByRunConfiguration(runsettingsXml, sources);

            if (testRuntimeProvider != null)
            {
                // Initialize here, because Shared is picked up from the instance, and it can be set during initalization.
                testRuntimeProvider.Initialize(TestSessionMessageLogger.Instance, runsettingsXml);

                // DO NOT move this above Initialize, the intialization can set Shared, to true, and we would not capture that.
                testRuntimeProviders.Add(new TestRuntimeProviderInfo(testRuntimeProvider.GetType(), testRuntimeProvider.Shared,
                   runsettingsXml, sourceDetails: runConfiguration.ToList()));

                // Outputting the instance, because the code for in-process run uses it, and we don't want to resolve it one more time.
                mostRecentlyCreatedInstance = testRuntimeProvider;
            }
            else
            {
                testRuntimeProviders.Add(new TestRuntimeProviderInfo(type: null, shared: false, runsettingsXml, sourceDetails: runConfiguration.ToList()));
            }
        }

        WarnAboutNotFoundRuntimeProvidersOrThrowWhenNoneAreFound(testRuntimeProviders, warningLogger);

        // Do NOT return just found providers here, instead return all of them. Later sources will be split to criteria
        // and we need to have all the sources available there, and filter them down to skip the ones that don't
        // have runtime providers there.
        // var foundRuntimeProviders = testRuntimeProviders.Where(runtimeProvider => runtimeProvider.Type != null).ToList();
        return testRuntimeProviders;
    }

    /// <inheritdoc/>
    public ITestExtensionManager GetExtensionManager() => _testExtensionManager ??= new TestExtensionManager();

    /// <inheritdoc/>
    public ITestLoggerManager GetLoggerManager(IRequestData requestData)
    {
        return new TestLoggerManager(
            requestData,
            TestSessionMessageLogger.Instance,
            new InternalTestLoggerEvents(TestSessionMessageLogger.Instance));
    }

    #endregion

    private static int GetDistinctNumberOfSources(TestRunCriteria testRunCriteria)
    {
        // No point in creating more processes if number of sources is less than what the user
        // configured for.
        int numberOfSources = testRunCriteria.HasSpecificTests
            ? new HashSet<string>(
                testRunCriteria.Tests.Select(testCase => testCase.Source)).Count
            : testRunCriteria.Sources.Count();
        return numberOfSources;
    }

    /// <summary>
    /// Verifies parallel setting and returns parallel level to use based on the run criteria.
    /// </summary>
    ///
    /// <param name="sourceCount">The source count.</param>
    /// <param name="runSettings">The run settings.</param>
    ///
    /// <returns>The parallel level to use.</returns>
    private int VerifyParallelSettingAndCalculateParallelLevel(
        int sourceCount,
        string? runSettings)
    {
        // Default is 1.
        int parallelLevelToUse;
        try
        {
            // Check the user parallel setting.
            int maxCpuCount = RunSettingsUtilities.GetMaxCpuCount(runSettings);
            parallelLevelToUse = maxCpuCount == 0
                ? _environment.ProcessorCount
                : maxCpuCount;

            EqtTrace.Verbose(
                "TestEngine: Initializing Parallel Execution as MaxCpuCount is set to: {0}",
                parallelLevelToUse);

            var enableParallel = parallelLevelToUse > 1;
            // Verify if the number of sources is less than user setting of parallel.
            // We should use number of sources as the parallel level, if sources count is less
            // than parallel level.
            if (enableParallel)
            {
                // In case of a background discovery we want to reduce the number of cores utilized
                // to leave enough power for other tasks.
                var runSettingsEnvVariables = InferRunSettingsHelper.GetEnvironmentVariables(runSettings);
                string? isBackgroundDiscoveryEnabled = null;
                if (runSettingsEnvVariables is not null
                    && runSettingsEnvVariables.TryGetValue("VSTEST_BACKGROUND_DISCOVERY", out isBackgroundDiscoveryEnabled)
                    && isBackgroundDiscoveryEnabled == "1"
                    && maxCpuCount == 0) // If user specifies a CPU count, respect it
                {
                    // Dummy logic based on some observations, might need to be tweaked/improved.
                    parallelLevelToUse = parallelLevelToUse switch
                    {
                        1 => 1,
                        < 8 => (int)Math.Round(parallelLevelToUse / 2.0, MidpointRounding.AwayFromZero),
                        _ => (int)Math.Round(0.75 * parallelLevelToUse, MidpointRounding.AwayFromZero),
                    };
                }

                EqtTrace.Verbose("TestEngine.VerifyParallelSettingAndCalculateParallelLevel: Parallel execution is enabled (cpu count: {0}, max cpu count is {1}, calculated cpu count is {2}, background mode is {3}, number of sources is {4})", _environment.ProcessorCount, maxCpuCount, parallelLevelToUse, isBackgroundDiscoveryEnabled == "1" ? "enabled" : "disabled", sourceCount);


                parallelLevelToUse = Math.Min(sourceCount, parallelLevelToUse);

                // If only one source, no need to use parallel service client.
                enableParallel = parallelLevelToUse > 1;

                EqtTrace.Verbose(
                    "TestEngine: ParallelExecution set to '{0}' as the parallel level is adjusted to '{1}' based on number of sources",
                    enableParallel,
                    parallelLevelToUse);
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error(
                "TestEngine: Error occurred while initializing ParallelExecution: {0}",
                ex);
            EqtTrace.Warning("TestEngine: Defaulting to Sequential Execution");

            parallelLevelToUse = 1;
        }

        return parallelLevelToUse;
    }

    private bool ShouldRunInProcess(
        string runsettings,
        bool isParallelEnabled,
        bool isDataCollectorEnabled,
        List<TestRuntimeProviderInfo> testHostProviders)
    {
        if (testHostProviders.Count > 1)
        {
            EqtTrace.Info("TestEngine.ShouldRunInNoIsolation: This run has multiple different architectures or frameworks, running in isolation (in a separate testhost proces).");
            return false;
        }

        var runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(runsettings);

        if (runConfiguration.InIsolation)
        {
            EqtTrace.Info("TestEngine.ShouldRunInNoIsolation: running test in isolation");
            return false;
        }

        // Run tests in isolation if run is authored using testsettings.
        if (InferRunSettingsHelper.IsTestSettingsEnabled(runsettings))
        {
            return false;
        }

        var currentProcessPath = _processHelper.GetCurrentProcessFileName();
        TPDebug.Assert(currentProcessPath != null, "currentProcessPath is null");

        // If running with the dotnet executable, then don't run in in process.
        if (currentProcessPath.EndsWith("dotnet", StringComparison.OrdinalIgnoreCase)
            || currentProcessPath.EndsWith("dotnet.exe", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Return true if
        // 1) Not running in parallel;
        // 2) Data collector is not enabled;
        // 3) Target platform is compatible or AnyCpu;
        // 4) DisableAppDomain is false;
        // 5) Not running in design mode;
        // 6) target framework is NETFramework (Desktop test);
        if (!isParallelEnabled &&
            !isDataCollectorEnabled &&
            (runConfiguration.TargetPlatform == ObjectModel.Constants.DefaultPlatform || runConfiguration.TargetPlatform == Architecture.AnyCPU) &&
            !runConfiguration.DisableAppDomain &&
            !runConfiguration.DesignMode &&
            runConfiguration.TargetFramework!.Name.IndexOf("netframework", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            EqtTrace.Info("TestEngine.ShouldRunInNoIsolation: running test in process(inside vstest.console.exe process)");
            return true;
        }

        return false;
    }

    private static void ThrowExceptionIfTestHostManagerIsNull(ITestRuntimeProvider? testHostManager, string? settingsXml)
    {
        if (testHostManager == null)
        {
            EqtTrace.Error($"{nameof(TestEngine)}.{nameof(ThrowExceptionIfTestHostManagerIsNull)}: No suitable testHostProvider found for runsettings: {settingsXml}");
            throw new TestPlatformException(Resources.Resources.NoTestHostProviderFound);
        }
    }

    private static void WarnAboutNotFoundRuntimeProvidersOrThrowWhenNoneAreFound(List<TestRuntimeProviderInfo> testRuntimeProviders, IWarningLogger warningLogger)
    {
        if (!testRuntimeProviders.Any())
            throw new ArgumentException(null, nameof(testRuntimeProviders));

        // Throw when we did not find any runtime provider for any of the provided sources.
        var shouldThrow = testRuntimeProviders.All(runtimeProvider => runtimeProvider == null);

        var missingRuntimeProviders = testRuntimeProviders.Where(p => p.Type == null);
        if (missingRuntimeProviders.Any())
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Resources.Resources.NoTestHostProviderFound);
            foreach (var missingRuntimeProvider in missingRuntimeProviders)
            {
                var text = $"{nameof(TestEngine)}.{nameof(WarnAboutNotFoundRuntimeProvidersOrThrowWhenNoneAreFound)}: No suitable testHostProvider found for sources {string.Join(", ", missingRuntimeProvider.SourceDetails.Select(s => s.Source))} and runsettings: {missingRuntimeProvider.RunSettings}";
                if (shouldThrow)
                {
                    EqtTrace.Error(text);
                }
                else
                {
                    EqtTrace.Warning(text);
                }
                missingRuntimeProvider.SourceDetails.ForEach(detail =>
                {
                    if (!shouldThrow)
                    {
                        stringBuilder.Append(Resources.Resources.SkippingSource).Append(' ');
                    }
                    stringBuilder.AppendLine(
#if NET6_0_OR_GREATER
                        System.Globalization.CultureInfo.InvariantCulture,
#endif
                        $"{detail.Source} ({detail.Framework}, {detail.Architecture})");
                });
            }

            if (shouldThrow)
            {
                stringBuilder.AppendLine().AppendLine(Resources.Resources.NoTestHostProviderFound);
                throw new TestPlatformException(stringBuilder.ToString());
            }
            else
            {
                warningLogger.LogWarning(stringBuilder.ToString());
            }
        }
    }
}
