// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;

#pragma warning disable IDE1006 // Naming Styles
namespace vstest.ProgrammerTests.Fakes;
#pragma warning restore IDE1006 // Naming Styles

internal class TestRequestManagerTestHelper
{
    private readonly FakeErrorAggregator _errorAggregator;
    private readonly TestRequestManager _testRequestManager;
    private readonly DebugOptions _debugOptions;

    public TestRequestManagerTestHelper(FakeErrorAggregator errorAggregator, TestRequestManager testRequestManager, DebugOptions debugOptions)
    {
        _errorAggregator = errorAggregator;
        _testRequestManager = testRequestManager;
        _debugOptions = debugOptions;
    }

    public async Task ExecuteWithAbort(Action<TestRequestManager> testRequestManagerAction)
    {
        // We make sure the test is running for the timeout time at max and then we try to abort
        // if we aborted we write the error to aggregator

        // Start tasks that waits until it is the right time to call abort
        // and continue to starting the method. If that method finishes running on time we cancel this
        // wait and don't abort. Otherwise we call abort to start our abort flow.
        //
        // This abort does not guarantee that we won't hang. If our abort flow is broken then we will
        // remain hanging. To have that guarantee it needs to be handled by failfast, or something else that will hang dump us.
        // Or a simple timer that kill the process after a given timeout, like a simplified blame hang dumper.
        var cancelAbort = new CancellationTokenSource();
        var abortOnTimeout = Task.Run(async () =>
        {
            // Wait until timeout or until we are cancelled.
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(Debugger.IsAttached ? _debugOptions.DebugTimeout : _debugOptions.Timeout), cancelAbort.Token);

                if (Debugger.IsAttached && _debugOptions.BreakOnAbort)
                {
                    var errors = _errorAggregator.Errors;
                    // we will abort because we are hanging, look at errors and at concurrent stacks to see where we are hanging.
                    Debugger.Break();
                }
                _errorAggregator.Add(new Exception("errr we aborted"));
                _testRequestManager.AbortTestRun();
            }
            catch (TaskCanceledException)
            {
            }
        });

        testRequestManagerAction(_testRequestManager);

        cancelAbort.Cancel();
        if (!abortOnTimeout.IsCanceled)
        {
            await abortOnTimeout;
        }
    }
}
