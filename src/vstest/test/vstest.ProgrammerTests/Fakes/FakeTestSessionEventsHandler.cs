// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeTestSessionEventsHandler : ITestSessionEventsHandler
{
    private readonly FakeErrorAggregator _fakeErrorAggregator;

    public FakeTestSessionEventsHandler(FakeErrorAggregator fakeErrorAggregator)
    {
        _fakeErrorAggregator = fakeErrorAggregator;
    }

    public List<object?> AllEvents { get; } = new();
    public List<TestMessage> LoggedMessages { get; } = new();
    public List<string> RawMessages { get; } = new();
    public List<StartTestSessionCompleteEventArgs?> StartTestSessionCompleteEvents { get; } = new();
    public List<StopTestSessionCompleteEventArgs?> StopTestSessionCompleteEvents { get; } = new();

    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        var msg = new TestMessage(level, message);
        AllEvents.Add(msg);
        LoggedMessages.Add(msg);
    }

    public void HandleRawMessage(string rawMessage)
    {
        AllEvents.Add(rawMessage);
        RawMessages.Add(rawMessage);
    }

    public void HandleStartTestSessionComplete(StartTestSessionCompleteEventArgs? eventArgs)
    {
        AllEvents.Add(eventArgs);
        StartTestSessionCompleteEvents.Add(eventArgs);
    }

    public void HandleStopTestSessionComplete(StopTestSessionCompleteEventArgs? eventArgs)
    {
        AllEvents.Add(eventArgs);
        StopTestSessionCompleteEvents.Add(eventArgs);
    }
}
