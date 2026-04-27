// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

using FluentAssertions;

using Intent;

using vstest.ProgrammerTests.Fakes;

#pragma warning disable IDE1006 // Naming Styles
namespace vstest.ProgrammerTests;
#pragma warning restore IDE1006 // Naming Styles

// Tests are run by Intent library that is executed from our Program.Main. To debug press F5 in VS, and maybe mark just a single test with [Only].
// To just run, press Ctrl+F5 to run without debugging. It will use short timeout for abort in case something is wrong with your test.

public class BasicRunAndDiscovery
{
    [Test(@"
        Given a test assembly with 108 tests.

        When we run tests.

        Then all 108 tests are executed.
    ")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Specific test needs to be non-static")]
    public async Task A()
    {
        // -- arrange
        using var fixture = new Fixture();

        var mstest1Dll = new FakeTestDllBuilder()
            .WithPath(@"X:\fake\mstest1.dll")
            .WithFramework(KnownFrameworkNames.Net5)
            .WithArchitecture(Architecture.X64)
            .WithTestCount(108, 10)
            .Build();

        var testhost1Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost1.exe");

        var runTests1 = new FakeTestHostResponsesBuilder()
            .VersionCheck(5)
            .ExecutionInitialize(FakeMessage.NoResponse)
            .StartTestExecutionWithSources(mstest1Dll.TestResultBatches)
            .SessionEnd(FakeMessage.NoResponse, _ => testhost1Process.Exit())
            .SessionEnd(FakeMessage.NoResponse)
            .Build();

        var testhost1 = new FakeTestHostFixtureBuilder(fixture)
            .WithTestDll(mstest1Dll)
            .WithProcess(testhost1Process)
            .WithResponses(runTests1)
            .Build();

        fixture.AddTestHostFixtures(testhost1);

        var testRequestManager = fixture.BuildTestRequestManager();

        // -- act
        var testRunRequestPayload = new TestRunRequestPayload
        {
            Sources = new List<string> { mstest1Dll.Path },

            RunSettings = $"<RunSettings></RunSettings>"
        };

        await testRequestManager.ExecuteWithAbort(tm => tm.RunTests(testRunRequestPayload, testHostLauncher: null, fixture.TestRunEventsRegistrar, fixture.ProtocolConfig));

        // -- assert
        fixture.AssertNoErrors();
        fixture.ExecutedTests.Should().HaveCount(mstest1Dll.TestCount);
    }

    [Test(@"
        Given multple test assemblies that use the same target framework.

        When we run tests.

        Then all tests from all assemblies are run.
    ")]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Specific test needs to be non-static")]
    public async Task B()
    {
        // -- arrange
        using var fixture = new Fixture();

        var mstest1Dll = new FakeTestDllBuilder()
            .WithPath(@"X:\fake\mstest1.dll")
            .WithFramework(KnownFrameworkNames.Net5)
            .WithArchitecture(Architecture.X64)
            .WithTestCount(108, 10)
            .Build();

        var testhost1Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost1.exe");

        var runTests1 = new FakeTestHostResponsesBuilder()
            .VersionCheck(5)
            .ExecutionInitialize(FakeMessage.NoResponse)
            .StartTestExecutionWithSources(mstest1Dll.TestResultBatches)
            .SessionEnd(FakeMessage.NoResponse, _ => testhost1Process.Exit())
            .SessionEnd(FakeMessage.NoResponse)
            .Build();

        var testhost1 = new FakeTestHostFixtureBuilder(fixture)
            .WithTestDll(mstest1Dll)
            .WithProcess(testhost1Process)
            .WithResponses(runTests1)
            .Build();

        var mstest2Dll = new FakeTestDllBuilder()
            .WithPath(@"X:\fake\mstest2.dll")
            .WithFramework(KnownFrameworkNames.Net5)
            .WithArchitecture(Architecture.X64)
            .WithTestCount(50, 8)
            .Build();

        var testhost2Process = new FakeProcess(fixture.ErrorAggregator, @"X:\fake\testhost2.exe");

        var runTests2 = new FakeTestHostResponsesBuilder()
            .VersionCheck(5)
            .ExecutionInitialize(FakeMessage.NoResponse)
            .StartTestExecutionWithSources(mstest2Dll.TestResultBatches)
            .SessionEnd(FakeMessage.NoResponse, f => f.Process.Exit())
            .SessionEnd(FakeMessage.NoResponse)
            .Build();

        var testhost2 = new FakeTestHostFixtureBuilder(fixture)
            .WithTestDll(mstest2Dll)
            .WithProcess(testhost2Process)
            .WithResponses(runTests2)
            .Build();

        fixture.AddTestHostFixtures(testhost1, testhost2);

        var testRequestManager = fixture.BuildTestRequestManager();

        // -- act
        var testRunRequestPayload = new TestRunRequestPayload
        {
            Sources = new List<string> { mstest1Dll.Path, mstest2Dll.Path },

            RunSettings = $"<RunSettings></RunSettings>"
        };

        await testRequestManager.ExecuteWithAbort(tm => tm.RunTests(testRunRequestPayload, testHostLauncher: null, fixture.TestRunEventsRegistrar, fixture.ProtocolConfig));

        // -- assert
        fixture.AssertNoErrors();
        fixture.ExecutedTests.Should().HaveCount(mstest1Dll.TestCount + mstest2Dll.TestCount);
    }
}

// Test and improvmement ideas:
// TODO: passing null runsettings does not fail fast, instead it fails in Fakes settings code
// TODO: passing empty string fails in the xml parser code
// TODO: passing null sources and null testcases does not fail fast
// TODO: Just calling Exit, Close won't stop the run, we will keep waiting for test run to complete, I think in real life when we exit then Disconnected will be called on the vstest.console side, leading to abort flow.
//.StartTestExecutionWithSources(new FakeMessage<TestMessagePayload>(MessageType.TestMessage, new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = "Loading type failed." }), afterAction: f => { /*f.Process.Exit();*/ f.FakeCommunicationEndpoint.Disconnect(); })
