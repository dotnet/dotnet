// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

using FluentAssertions;

using Intent;

using vstest.ProgrammerTests.Fakes;

namespace vstest.ProgrammerTests;

public class MultiTFM
{
    public class MultiTFMDiscovery
    {
        [Test(@"
            Given two test assemblies that have the same architecture
            but have different target frameworks.

            When we run test discovery.

            Then two testhosts should be started that target the same framework as each assembly.
        ")]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Specific test needs to be non-static")]
        public async Task A()
        {
            // -- arrange
            using var fixture = new Fixture();

            var mstest1Dll = new FakeTestDllBuilder()
                .WithPath(@"X:\fake\mstest1.dll")
                .WithFramework(KnownFrameworkNames.Net5) // <---
                .WithArchitecture(Architecture.X64)
                .WithTestCount(11, 5)
                .Build();

            var testhost1Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost1.exe");

            var runTests1 = new FakeTestHostResponsesBuilder()
                .VersionCheck(5)
                .DiscoveryInitialize(FakeMessage.NoResponse)
                .StartDiscovery(mstest1Dll.TestResultBatches)
                .SessionEnd(FakeMessage.NoResponse, afterAction: _ => testhost1Process.Exit())
                .Build();

            var testhost1 = new FakeTestHostFixtureBuilder(fixture)
                .WithTestDll(mstest1Dll)
                .WithProcess(testhost1Process)
                .WithResponses(runTests1)
                .Build();

            // --

            var mstest2Dll = new FakeTestDllBuilder()
                .WithPath(@"X:\fake\mstest2.dll")
                .WithFramework(KnownFrameworkNames.Net48) // <---
                .WithArchitecture(Architecture.X64)
                .WithTestCount(21, 5)
                .Build();

            var testhost2Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost2.exe");

            var runTests2 = new FakeTestHostResponsesBuilder()
                .VersionCheck(5)
                .DiscoveryInitialize(FakeMessage.NoResponse)
                .StartDiscovery(mstest2Dll.TestResultBatches)
                .SessionEnd(FakeMessage.NoResponse, _ => testhost2Process.Exit())
                // We actually do get asked to terminate multiple times. In the second host only.
                .SessionEnd(FakeMessage.NoResponse)
                .Build();

            var testhost2 = new FakeTestHostFixtureBuilder(fixture)
                .WithTestDll(mstest2Dll)
                .WithProcess(testhost2Process)
                .WithResponses(runTests2)
                .Build();

            fixture.AddTestHostFixtures(testhost1, testhost2);

            var testRequestManager = fixture.BuildTestRequestManager();

            mstest1Dll.FrameworkName.Should().NotBe(mstest2Dll.FrameworkName);

            // -- act
            var testDiscoveryPayload = new DiscoveryRequestPayload
            {
                Sources = new List<string> { mstest1Dll.Path, mstest2Dll.Path },
                RunSettings = $"<RunSettings></RunSettings>"
            };

            await testRequestManager.ExecuteWithAbort(tm => tm.DiscoverTests(testDiscoveryPayload, fixture.TestDiscoveryEventsRegistrar, fixture.ProtocolConfig));

            // -- assert
            fixture.AssertNoErrors();
            // We figure out the framework for each assembly so there should be no incompatibility warnings
            fixture.LoggedWarnings.Should().NotContainMatch("Test run detected DLL(s) which would use different framework*");

            fixture.ProcessHelper.Processes.Where(p => p.Started).Should().HaveCount(2);
            var startWithSources1 = testhost1.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartDiscovery);
            var startWithSources1Text = startWithSources1.Request.GetRawMessage();
            // We sent mstest1.dll.
            startWithSources1Text.Should().Contain("mstest1.dll");
            // And we sent net5 as the target framework, because that is the framework of mstest1.dll.
            startWithSources1Text.Should().Contain(KnownFrameworkStrings.Net5);

            var startWithSources2 = testhost2.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartDiscovery);
            var startWithSources2Text = startWithSources2.Request.GetRawMessage();
            // We sent mstest2.dll.
            startWithSources2Text.Should().Contain("mstest2.dll");
            // And we sent net48 as the target framework, because that is the framework of mstest2.dll.
            startWithSources2Text.Should().Contain(mstest2Dll.FrameworkName.ToString());

            fixture.DiscoveredTests.Should().HaveCount(mstest1Dll.TestCount + mstest2Dll.TestCount);
        }

        [Test(@"
            Given two test assemblies that have the same architecture
            but have different target frameworks.

            When we run test discovery
            and provide runsettings that define the desired target framework.

            Then two testhosts should be started that target the framework chosen by runsettings.
        ")]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Specific test needs to be non-static")]
        public async Task B()
        {
            // -- arrange
            using var fixture = new Fixture();

            var mstest1Dll = new FakeTestDllBuilder()
                .WithPath(@"X:\fake\mstest1.dll")
                .WithFramework(KnownFrameworkNames.Net5) // <---
                .WithArchitecture(Architecture.X64)
                .WithTestCount(11, 5)
                .Build();

            var testhost1Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost1.exe");

            var runTests1 = new FakeTestHostResponsesBuilder()
                .VersionCheck(5)
                .DiscoveryInitialize(FakeMessage.NoResponse)
                .StartDiscovery(mstest1Dll.TestResultBatches)
                .SessionEnd(FakeMessage.NoResponse, afterAction: _ => testhost1Process.Exit())
                .Build();

            var testhost1 = new FakeTestHostFixtureBuilder(fixture)
                .WithTestDll(mstest1Dll)
                .WithProcess(testhost1Process)
                .WithResponses(runTests1)
                .Build();

            // --

            var mstest2Dll = new FakeTestDllBuilder()
                .WithPath(@"X:\fake\mstest2.dll")
                .WithFramework(KnownFrameworkNames.Net6) // <---
                .WithArchitecture(Architecture.X64)
                .WithTestCount(21, 5)
                .Build();

            var testhost2Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost2.exe");

            var runTests2 = new FakeTestHostResponsesBuilder()
                .VersionCheck(5)
                .DiscoveryInitialize(FakeMessage.NoResponse)
                .StartDiscovery(mstest2Dll.TestResultBatches)
                .SessionEnd(FakeMessage.NoResponse, _ => testhost2Process.Exit())
                // We actually do get asked to terminate multiple times. In the second host only.
                .SessionEnd(FakeMessage.NoResponse)
                .Build();

            var testhost2 = new FakeTestHostFixtureBuilder(fixture)
                .WithTestDll(mstest2Dll)
                .WithProcess(testhost2Process)
                .WithResponses(runTests2)
                .Build();

            fixture.AddTestHostFixtures(testhost1, testhost2);

            var testRequestManager = fixture.BuildTestRequestManager();

            mstest1Dll.FrameworkName.Should().NotBe(mstest2Dll.FrameworkName);

            // -- act
            var testDiscoveryPayload = new DiscoveryRequestPayload
            {
                Sources = new List<string> { mstest1Dll.Path, mstest2Dll.Path },
                RunSettings = $"<RunSettings><RunConfiguration><TargetFrameworkVersion>{KnownFrameworkStrings.Net7}</TargetFrameworkVersion></RunConfiguration></RunSettings>"
            };

            await testRequestManager.ExecuteWithAbort(tm => tm.DiscoverTests(testDiscoveryPayload, fixture.TestDiscoveryEventsRegistrar, fixture.ProtocolConfig));

            // -- assert
            fixture.AssertNoErrors();
            // Runsettings will force NET7, so we should get a warning.
            fixture.LoggedWarnings.Should().ContainMatch("Test run detected DLL(s) which would use different framework*");

            fixture.ProcessHelper.Processes.Where(p => p.Started).Should().HaveCount(2);
            var startWithSources1 = testhost1.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartDiscovery);
            var startWithSources1Text = startWithSources1.Request.GetRawMessage();
            // We sent mstest1.dll and net7 because that is what we have in settings.
            startWithSources1Text.Should().Contain("mstest1.dll");
            startWithSources1Text.Should().Contain(KnownFrameworkStrings.Net7);

            var startWithSources2 = testhost2.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartDiscovery);
            var startWithSources2Text = startWithSources2.Request.GetRawMessage();
            // We sent mstest2.dll and net7 because that is what we have in settings.
            startWithSources2Text.Should().Contain("mstest2.dll");
            startWithSources2Text.Should().Contain(KnownFrameworkStrings.Net7);

            fixture.DiscoveredTests.Should().HaveCount(mstest1Dll.TestCount + mstest2Dll.TestCount);
        }
    }

    public class MultiTFMExecution
    {
        [Test(@"
            Given two test assemblies that have the same architecture
            but have different target frameworks.

            When we execute tests.

            Then two testhosts should be started that target the same framework as each assembly.
        ")]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Specific test needs to be non-static")]
        public async Task C()
        {
            // -- arrange
            using var fixture = new Fixture();

            var mstest1Dll = new FakeTestDllBuilder()
                .WithPath(@"X:\fake\mstest1.dll")
                .WithFramework(KnownFrameworkNames.Net5) // <---
                .WithArchitecture(Architecture.X64)
                .WithTestCount(11, 5)
                .Build();

            var testhost1Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost1.exe");

            var runTests1 = new FakeTestHostResponsesBuilder()
                .VersionCheck(5)
                .ExecutionInitialize(FakeMessage.NoResponse)
                .StartTestExecutionWithSources(mstest1Dll.TestResultBatches)
                .SessionEnd(FakeMessage.NoResponse, afterAction: _ => testhost1Process.Exit())
                .Build();

            var testhost1 = new FakeTestHostFixtureBuilder(fixture)
                .WithTestDll(mstest1Dll)
                .WithProcess(testhost1Process)
                .WithResponses(runTests1)
                .Build();

            // --

            var mstest2Dll = new FakeTestDllBuilder()
                .WithPath(@"X:\fake\mstest2.dll")
                .WithFramework(KnownFrameworkNames.Net6) // <---
                .WithArchitecture(Architecture.X64)
                .WithTestCount(21, 5)
                .Build();

            var testhost2Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost2.exe");

            var runTests2 = new FakeTestHostResponsesBuilder()
                .VersionCheck(5)
                .ExecutionInitialize(FakeMessage.NoResponse)
                .StartTestExecutionWithSources(mstest2Dll.TestResultBatches)
                .SessionEnd(FakeMessage.NoResponse, _ => testhost2Process.Exit())
                // We actually do get asked to terminate multiple times. In the second host only.
                .SessionEnd(FakeMessage.NoResponse)
                .Build();

            var testhost2 = new FakeTestHostFixtureBuilder(fixture)
                .WithTestDll(mstest2Dll)
                .WithProcess(testhost2Process)
                .WithResponses(runTests2)
                .Build();

            fixture.AddTestHostFixtures(testhost1, testhost2);

            var testRequestManager = fixture.BuildTestRequestManager();

            mstest1Dll.FrameworkName.Should().NotBe(mstest2Dll.FrameworkName);

            // -- act
            var testRunRequestPayload = new TestRunRequestPayload
            {
                Sources = new List<string> { mstest1Dll.Path, mstest2Dll.Path },
                RunSettings = $"<RunSettings></RunSettings>"
            };

            await testRequestManager.ExecuteWithAbort(tm => tm.RunTests(testRunRequestPayload, testHostLauncher: null, fixture.TestRunEventsRegistrar, fixture.ProtocolConfig));

            // -- assert
            fixture.AssertNoErrors();
            // We figure out the framework for each assembly so there should be no incompatibility warnings
            fixture.LoggedWarnings.Should().NotContainMatch("Test run detected DLL(s) which would use different framework*");

            fixture.ProcessHelper.Processes.Where(p => p.Started).Should().HaveCount(2);
            var startWithSources1 = testhost1.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartTestExecutionWithSources);
            var startWithSources1Text = startWithSources1.Request.GetRawMessage();
            // We sent mstest1.dll.
            startWithSources1Text.Should().Contain("mstest1.dll");
            // And we sent net5 as the target framework, because that is the framework of mstest1.dll.
            startWithSources1Text.Should().Contain(KnownFrameworkStrings.Net5);

            var startWithSources2 = testhost2.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartTestExecutionWithSources);
            var startWithSources2Text = startWithSources2.Request.GetRawMessage();
            // We sent mstest2.dll.
            startWithSources2Text.Should().Contain("mstest2.dll");
            // And we sent net48 as the target framework, because that is the framework of mstest2.dll.
            startWithSources2Text.Should().Contain(KnownFrameworkStrings.Net6);

            fixture.ExecutedTests.Should().HaveCount(mstest1Dll.TestCount + mstest2Dll.TestCount);
        }

        [Test(@"
            Given two test assemblies that have the same architecture
            but have different target frameworks.

            When we execute tests
            and provide runsettings that define the desired target framework.

            Then two testhosts should be started that target the framework chosen by runsettings.
        ")]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Specific test needs to be non-static")]
        public async Task D()
        {
            // -- arrange
            using var fixture = new Fixture();

            var mstest1Dll = new FakeTestDllBuilder()
                .WithPath(@"X:\fake\mstest1.dll")
                .WithFramework(KnownFrameworkNames.Net5) // <---
                .WithArchitecture(Architecture.X64)
                .WithTestCount(11, 5)
                .Build();

            var testhost1Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost1.exe");

            var runTests1 = new FakeTestHostResponsesBuilder()
                .VersionCheck(5)
                .ExecutionInitialize(FakeMessage.NoResponse)
                .StartTestExecutionWithSources(mstest1Dll.TestResultBatches)
                .SessionEnd(FakeMessage.NoResponse, afterAction: _ => testhost1Process.Exit())
                .Build();

            var testhost1 = new FakeTestHostFixtureBuilder(fixture)
                .WithTestDll(mstest1Dll)
                .WithProcess(testhost1Process)
                .WithResponses(runTests1)
                .Build();

            // --

            var mstest2Dll = new FakeTestDllBuilder()
                .WithPath(@"X:\fake\mstest2.dll")
                .WithFramework(KnownFrameworkNames.Net6) // <---
                .WithArchitecture(Architecture.X64)
                .WithTestCount(21, 5)
                .Build();

            var testhost2Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost2.exe");

            var runTests2 = new FakeTestHostResponsesBuilder()
                .VersionCheck(5)
                .ExecutionInitialize(FakeMessage.NoResponse)
                .StartTestExecutionWithSources(mstest2Dll.TestResultBatches)
                .SessionEnd(FakeMessage.NoResponse, _ => testhost2Process.Exit())
                // We actually do get asked to terminate multiple times. In the second host only.
                .SessionEnd(FakeMessage.NoResponse)
                .Build();

            var testhost2 = new FakeTestHostFixtureBuilder(fixture)
                .WithTestDll(mstest2Dll)
                .WithProcess(testhost2Process)
                .WithResponses(runTests2)
                .Build();

            fixture.AddTestHostFixtures(testhost1, testhost2);

            var testRequestManager = fixture.BuildTestRequestManager();

            mstest1Dll.FrameworkName.Should().NotBe(mstest2Dll.FrameworkName);

            // -- act
            var testRunRequestPayload = new TestRunRequestPayload
            {
                Sources = new List<string> { mstest1Dll.Path, mstest2Dll.Path },
                RunSettings = $"<RunSettings><RunConfiguration><TargetFrameworkVersion>{KnownFrameworkStrings.Net7}</TargetFrameworkVersion></RunConfiguration></RunSettings>"
            };

            await testRequestManager.ExecuteWithAbort(tm => tm.RunTests(testRunRequestPayload, testHostLauncher: null, fixture.TestRunEventsRegistrar, fixture.ProtocolConfig));

            // -- assert
            fixture.AssertNoErrors();
            // We specify net7 which is not compatible with either, so we should get warnings
            fixture.LoggedWarnings.Should().ContainMatch("Test run detected DLL(s) which would use different framework*");

            fixture.ProcessHelper.Processes.Where(p => p.Started).Should().HaveCount(2);
            var startWithSources1 = testhost1.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartTestExecutionWithSources);
            var startWithSources1Text = startWithSources1.Request.GetRawMessage();
            // We sent mstest1.dll.
            startWithSources1Text.Should().Contain("mstest1.dll");
            startWithSources1Text.Should().Contain(KnownFrameworkStrings.Net7);

            var startWithSources2 = testhost2.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartTestExecutionWithSources);
            var startWithSources2Text = startWithSources2.Request.GetRawMessage();
            // We sent mstest2.dll.
            startWithSources2Text.Should().Contain("mstest2.dll");
            startWithSources2Text.Should().Contain(KnownFrameworkStrings.Net7);

            fixture.ExecutedTests.Should().HaveCount(mstest1Dll.TestCount + mstest2Dll.TestCount);
        }
    }

    public class MultiTFMTestSessions
    {

        [Test(@"
            Given two test assemblies that have the same architecture
            but have different target frameworks.

            When we execute tests
            and provide runsettings that define the desired target framework.

            Then two testhosts should be started that target the framework chosen by runsettings.
        ")]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Specific test needs to be non-static")]
        public async Task E()
        {
            // -- arrange
            using var fixture = new Fixture();

            var mstest1Dll = new FakeTestDllBuilder()
                .WithPath(@"X:\fake\mstest1.dll")
                .WithFramework(KnownFrameworkNames.Net5) // <---
                .WithArchitecture(Architecture.X64)
                .WithTestCount(11, 5)
                .Build();

            var testhost1Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost1.exe");

            var runTests1 = new FakeTestHostResponsesBuilder()
                .VersionCheck(5)
                .ExecutionInitialize(FakeMessage.NoResponse)
                .StartTestExecutionWithSources(mstest1Dll.TestResultBatches)
                .SessionEnd(FakeMessage.NoResponse, afterAction: _ => testhost1Process.Exit())
                .Build();

            var testhost1 = new FakeTestHostFixtureBuilder(fixture)
                .WithTestDll(mstest1Dll)
                .WithProcess(testhost1Process)
                .WithResponses(runTests1)
                .Build();

            // --

            var mstest2Dll = new FakeTestDllBuilder()
                .WithPath(@"X:\fake\mstest2.dll")
                .WithFramework(KnownFrameworkNames.Net6) // <---
                .WithArchitecture(Architecture.X64)
                .WithTestCount(21, 5)
                .Build();

            var testhost2Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost2.exe");

            var runTests2 = new FakeTestHostResponsesBuilder()
                .VersionCheck(5)
                .ExecutionInitialize(FakeMessage.NoResponse)
                .StartTestExecutionWithSources(mstest2Dll.TestResultBatches)
                .SessionEnd(FakeMessage.NoResponse, _ => testhost2Process.Exit())
                // We actually do get asked to terminate multiple times. In the second host only.
                .SessionEnd(FakeMessage.NoResponse)
                .Build();

            var testhost2 = new FakeTestHostFixtureBuilder(fixture)
                .WithTestDll(mstest2Dll)
                .WithProcess(testhost2Process)
                .WithResponses(runTests2)
                .Build();

            fixture.AddTestHostFixtures(testhost1, testhost2);

            var testRequestManager = fixture.BuildTestRequestManager();

            mstest1Dll.FrameworkName.Should().NotBe(mstest2Dll.FrameworkName);

            // -- act

            var startTestSessionPayload = new StartTestSessionPayload
            {
                // We need to have a parallel run, otherwise we will create just a single proxy,
                // because 1 is the maximum number of proxies to start for non-parallel run.
                RunSettings = "<RunSettings><RunConfiguration><MaxCpuCount>0</MaxCpuCount></RunConfiguration></RunSettings>",
                Sources = new[] { mstest1Dll.Path, mstest2Dll.Path }
            };

            await testRequestManager.ExecuteWithAbort(tm => tm.StartTestSession(startTestSessionPayload, testHostLauncher: null, fixture.TestSessionEventsHandler, fixture.ProtocolConfig));

            // You need to pass this on, otherwise it will ignore the test session that you just started. This is a by product of being able to start multiple test sessions.
            var testSessionInfo = fixture.TestSessionEventsHandler.StartTestSessionCompleteEvents.Single()!.TestSessionInfo;

            var testRunRequestPayload = new TestRunRequestPayload
            {
                Sources = new List<string> { mstest1Dll.Path, mstest2Dll.Path },
                RunSettings = $"<RunSettings><RunConfiguration><MaxCpuCount>0</MaxCpuCount></RunConfiguration></RunSettings>",
                TestSessionInfo = testSessionInfo,
            };

            await testRequestManager.ExecuteWithAbort(tm => tm.RunTests(testRunRequestPayload, testHostLauncher: null, fixture.TestRunEventsRegistrar, fixture.ProtocolConfig));

            // -- assert
            fixture.AssertNoErrors();
            // We figure out the framework for each assembly so there should be no incompatibility warnings
            fixture.LoggedWarnings.Should().NotContainMatch("Test run detected DLL(s) which would use different framework*");

            fixture.ProcessHelper.Processes.Where(p => p.Started).Should().HaveCount(2);
            var startWithSources1 = testhost1.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartTestExecutionWithSources);
            var startWithSources1Text = startWithSources1.Request.GetRawMessage();
            // We sent mstest1.dll.
            startWithSources1Text.Should().Contain("mstest1.dll");
            startWithSources1Text.Should().Contain(mstest1Dll.FrameworkName.ToString());

            var startWithSources2 = testhost2.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartTestExecutionWithSources);
            var startWithSources2Text = startWithSources2.Request.GetRawMessage();
            // We sent mstest2.dll.
            startWithSources2Text.Should().Contain("mstest2.dll");
            startWithSources2Text.Should().Contain(mstest2Dll.FrameworkName.ToString());

            fixture.ExecutedTests.Should().HaveCount(mstest1Dll.TestCount + mstest2Dll.TestCount);
        }
    }
}

public class MultiTFMRunAndDiscoveryCompatibilityMode
{
    [Test(@"
        Given two test assemblies that have the same architecture
        but have different target frameworks.

        When DISABLE_MULTI_TFM_RUN is set
        and we execute tests.

        Then two testhosts are both started for the same TFM.
    ")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Specific test needs to be non-static")]
    public async Task E()
    {
        // -- arrange
        using var fixture = new Fixture(
            new FixtureOptions
            {
                FeatureFlags = new Dictionary<string, bool>
                {
                    [FeatureFlag.VSTEST_DISABLE_MULTI_TFM_RUN] = true
                }
            }
        );

        var mstest1Dll = new FakeTestDllBuilder()
            .WithPath(@"X:\fake\mstest1.dll")
            .WithFramework(KnownFrameworkNames.Net5) // <---
            .WithArchitecture(Architecture.X64)
            .WithTestCount(2)
            .Build();

        var testhost1Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost1.exe");

        var runTests1 = new FakeTestHostResponsesBuilder()
            .VersionCheck(5)
            .ExecutionInitialize(FakeMessage.NoResponse)
            .StartTestExecutionWithSources(mstest1Dll.TestResultBatches)
            .SessionEnd(FakeMessage.NoResponse, afterAction: _ => testhost1Process.Exit())
            .SessionEnd(FakeMessage.NoResponse)
            .Build();

        var testhost1 = new FakeTestHostFixtureBuilder(fixture)
            .WithTestDll(mstest1Dll)
            .WithProcess(testhost1Process)
            .WithResponses(runTests1)
            .Build();

        // --

        var mstest2Dll = new FakeTestDllBuilder()
            .WithPath(@"X:\fake\mstest2.dll")
            .WithFramework(KnownFrameworkNames.Net48) // <---
            .WithArchitecture(Architecture.X64)
            // In reality, the dll would fail to load, and no tests would run from this dll,
            // we simulate that by making it have 0 tests.
            .WithTestCount(0)
            .Build();

        var testhost2Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost2.exe");

        var runTests2 = new FakeTestHostResponsesBuilder()
            .VersionCheck(5)
            .ExecutionInitialize(FakeMessage.NoResponse)
            .StartTestExecutionWithSources(mstest2Dll.TestResultBatches)
            .SessionEnd(FakeMessage.NoResponse, _ => testhost2Process.Exit())
            .SessionEnd(FakeMessage.NoResponse)
            .Build();

        var testhost2 = new FakeTestHostFixtureBuilder(fixture)
            .WithTestDll(mstest2Dll)
            .WithProcess(testhost2Process)
            .WithResponses(runTests2)
            .Build();

        fixture.AddTestHostFixtures(testhost1, testhost2);

        var testRequestManager = fixture.BuildTestRequestManager();

        mstest1Dll.FrameworkName.Should().NotBe(mstest2Dll.FrameworkName);

        // -- act
        var testRunRequestPayload = new TestRunRequestPayload
        {
            Sources = new List<string> { mstest1Dll.Path, mstest2Dll.Path },
            RunSettings = $"<RunSettings></RunSettings>",
        };

        await testRequestManager.ExecuteWithAbort(tm => tm.RunTests(testRunRequestPayload, testHostLauncher: null, fixture.TestRunEventsRegistrar, fixture.ProtocolConfig));

        // -- assert
        fixture.AssertNoErrors();
        // We unify the frameworks to netcoreapp1.0 (because the vstest.console dll we are loading is built for netcoreapp and prefers netcoreapp), and because the
        // behavior is to choose the common oldest framework. We then log warning about incompatible sources.
        fixture.LoggedWarnings.Should().ContainMatch($"Test run detected DLL(s) which would use different framework and platform versions*{KnownFrameworkNames.Netcoreapp1}*");

        // We started both testhosts, even thought we know one of them is incompatible.
        fixture.ProcessHelper.Processes.Where(p => p.Started).Should().HaveCount(2);
        var startWithSources1 = testhost1.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartTestExecutionWithSources);
        var startWithSources1Text = startWithSources1.Request.GetRawMessage();
        // We sent mstest1.dll
        startWithSources1Text.Should().Contain("mstest1.dll");
        // And we sent netcoreapp1.0 as the target framework, because that is the common fallback
        startWithSources1Text.Should().Contain(KnownFrameworkStrings.Netcoreapp1);

        var startWithSources2 = testhost2.FakeCommunicationChannel.ProcessedMessages.Single(m => m.Request.MessageType == MessageType.StartTestExecutionWithSources);
        var startWithSources2Text = startWithSources2.Request.GetRawMessage();
        // We sent mstest2.dll
        startWithSources2Text.Should().Contain("mstest2.dll");
        // And we sent netcoreapp1.0 as the target framework, because that is the common fallback, even though the source is not compatible with it
        startWithSources2Text.Should().Contain(KnownFrameworkStrings.Netcoreapp1);

        fixture.ExecutedTests.Should().HaveCount(mstest1Dll.TestCount);
    }
}

internal static class MessageExtensions
{
    private static MethodInfo? s_messageProperty;

    internal static string GetRawMessage(this Message request)
    {
        if (s_messageProperty == null)
        {
            s_messageProperty = request.GetType().GetProperty("RawMessage")!.GetGetMethod();
        }

        return (string)s_messageProperty!.Invoke(request, [])!;
    }
}

// Test and improvmement ideas:
// TODO: passing null runsettings does not fail fast, instead it fails in Fakes settings code
// TODO: passing empty string fails in the xml parser code
// TODO: passing null sources and null testcases does not fail fast
// TODO: Just calling Exit, Close won't stop the run, we will keep waiting for test run to complete, I think in real life when we exit then Disconnected will be called on the vstest.console side, leading to abort flow.
//.StartTestExecutionWithSources(new FakeMessage<TestMessagePayload>(MessageType.TestMessage, new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = "Loading type failed." }), afterAction: f => { /*f.Process.Exit();*/ f.FakeCommunicationEndpoint.Disconnect(); })
