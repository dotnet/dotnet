// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;

using Microsoft.VisualStudio.TestPlatform.Client;
using Microsoft.VisualStudio.TestPlatform.CommandLine;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Publisher;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.CommandLineUtilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace vstest.ProgrammerTests.Fakes;

internal class Fixture : IDisposable
{
    private readonly List<IDisposable> _disposables = new();

    public FakeErrorAggregator ErrorAggregator { get; } = new();
    public FakeProcessHelper ProcessHelper { get; }
    public string LogName { get; }
    public FakeProcess CurrentProcess { get; }
    public FakeFileHelper FileHelper { get; }
    public FakeTestRuntimeProviderManager TestRuntimeProviderManager { get; }
    public FakeTestRunEventsRegistrar TestRunEventsRegistrar { get; }
    public FakeEnvironment Environment { get; }
    public FakeTestDiscoveryEventsRegistrar TestDiscoveryEventsRegistrar { get; }
    public TestEngine? TestEngine { get; private set; }
    public TestPlatform? TestPlatform { get; private set; }
    public TestRunResultAggregator? TestRunResultAggregator { get; private set; }
    public FakeTestPlatformEventSource? TestPlatformEventSource { get; private set; }
    public FakeAssemblyMetadataProvider? AssemblyMetadataProvider { get; private set; }
    public InferHelper? InferHelper { get; private set; }
    public FakeDataCollectorAttachmentsProcessorsFactory? DataCollectorAttachmentsProcessorsFactory { get; private set; }
    public TestRunAttachmentsProcessingManager? TestRunAttachmentsProcessingManager { get; private set; }
    public TestRequestManager? TestRequestManager { get; private set; }
    public ProtocolConfig ProtocolConfig { get; internal set; }

    public List<TestResult> ExecutedTests => TestRunEventsRegistrar.RunChangedEvents.SelectMany(er => er.Data.NewTestResults!).ToList();
    public List<TestCase> DiscoveredTests => TestDiscoveryEventsRegistrar.DiscoveredTestsEvents.SelectMany(er => er.Data.DiscoveredTestCases!).ToList();

    public List<string> LoggedWarnings => TestRunEventsRegistrar.LoggedWarnings.Concat(TestDiscoveryEventsRegistrar.LoggedWarnings).ToList();

    public FakeTestSessionEventsHandler TestSessionEventsHandler { get; }

    public Fixture(FixtureOptions? fixtureOptions = null)
    {
        // This type is compiled only in DEBUG, and won't exist otherwise.
#if DEBUG
        // We need to use static class to find the communication endpoint, this clears all the registrations of previous tests.
        TestServiceLocator.Clear();
#else
        // This fools compiler into not being able to tell that the the rest of the code is unreachable.
        var a = true;
        if (a)
        {
            throw new InvalidOperationException("Tests cannot run in Release mode, because TestServiceLocator is compiled only for Debug, and so the tests will fail to setup channel and will hang.");
        }
#endif

#pragma warning disable CS0618 // Type or member is obsolete (to prevent use outside of test context)
        FeatureFlag.Reset();
        fixtureOptions?.FeatureFlags?.ToList().ForEach(flag => ((FeatureFlag)FeatureFlag.Instance).SetFlag(flag.Key, flag.Value));
#pragma warning restore CS0618 // Type or member is obsolete

        // This makes the run a bit slower, but at least we get info in the output window. We probably should add a mode where we don't
        // use a file to write the output. Just trace listener. That would also be useful for UWP I think.
        LogName = Path.GetTempPath() + $"/log_{Guid.NewGuid()}.txt";
        //EqtTrace.InitializeVerboseTrace(LogName);

        CurrentProcess = new FakeProcess(ErrorAggregator, @"X:\fake\vstest.console.exe");
        ProcessHelper = new FakeProcessHelper(ErrorAggregator, CurrentProcess);
        FileHelper = new FakeFileHelper(ErrorAggregator);
        TestRuntimeProviderManager = new FakeTestRuntimeProviderManager(ErrorAggregator);
        TestRunEventsRegistrar = new FakeTestRunEventsRegistrar(ErrorAggregator);
        Environment = new FakeEnvironment();
        TestDiscoveryEventsRegistrar = new FakeTestDiscoveryEventsRegistrar(ErrorAggregator);
        TestSessionEventsHandler = new FakeTestSessionEventsHandler(ErrorAggregator);
        ProtocolConfig = new ProtocolConfig();
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            try { disposable.Dispose(); } catch (ObjectDisposedException) { }
        }
    }

    internal void AddTestHostFixtures(params FakeTestHostFixture[] testhosts)
    {
        _disposables.AddRange(testhosts);
        var providers = testhosts.Select(t => t.FakeTestRuntimeProvider).ToArray();
        TestRuntimeProviderManager.AddTestRuntimeProviders(providers);
    }

    internal TestRequestManagerTestHelper BuildTestRequestManager(
        int? timeout = DebugOptions.DefaultTimeout,
        int? debugTimeout = DebugOptions.DefaultDebugTimeout,
        bool? breakOnAbort = DebugOptions.DefaultBreakOnAbort)
    {
        if (!TestRuntimeProviderManager.TestRuntimeProviders.Any())
            throw new InvalidOperationException("There are runtime providers registered for FakeTestRuntimeProviderManager.");


        TestEngine = new TestEngine(TestRuntimeProviderManager, ProcessHelper, Environment);
        TestPlatform = new TestPlatform(TestEngine, FileHelper, TestRuntimeProviderManager);

        TestRunResultAggregator = new TestRunResultAggregator();
        TestPlatformEventSource = new FakeTestPlatformEventSource(ErrorAggregator);

        AssemblyMetadataProvider = new FakeAssemblyMetadataProvider(FileHelper, ErrorAggregator);
        InferHelper = new InferHelper(AssemblyMetadataProvider);

        // This is most likely not the correctl place where to cut this off, plugin cache is probably the better place,
        // but it is not injected, and I don't want to investigate this now.
        DataCollectorAttachmentsProcessorsFactory = new FakeDataCollectorAttachmentsProcessorsFactory(ErrorAggregator);
        TestRunAttachmentsProcessingManager = new TestRunAttachmentsProcessingManager(TestPlatformEventSource, DataCollectorAttachmentsProcessorsFactory);

        Task<IMetricsPublisher> fakeMetricsPublisherTask = Task.FromResult<IMetricsPublisher>(new FakeMetricsPublisher(ErrorAggregator));

        var commandLineOptions = new CommandLineOptions
        {
            // We are acting like we are running under IDE. This is done because some settings are trying to grab the
            // value from the pre-parsed settings in command line options. And some are looking at the actual run settings.
            // Ultimately we should have a single point of truth for both scenarios, but now it is easier to just provide
            // runsettings to the request.
            IsDesignMode = true,
        };
        TestRequestManager testRequestManager = new(
            commandLineOptions,
            TestPlatform,
            TestRunResultAggregator,
            TestPlatformEventSource,
            InferHelper,
            fakeMetricsPublisherTask,
            ProcessHelper,
            TestRunAttachmentsProcessingManager,
            Environment,
            new EnvironmentVariableHelper());

        TestRequestManager = testRequestManager;

        return new TestRequestManagerTestHelper(ErrorAggregator, testRequestManager, new DebugOptions
        {
            Timeout = timeout ?? DebugOptions.DefaultTimeout,
            DebugTimeout = debugTimeout ?? DebugOptions.DefaultDebugTimeout,
            BreakOnAbort = breakOnAbort ?? DebugOptions.DefaultBreakOnAbort,
        });
    }

    internal void AssertNoErrors()
    {
        ErrorAggregator.Errors.Should().BeEmpty();
    }
}
