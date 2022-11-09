// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.Client;

internal class InProcessTestSessionEventsHandler : ITestSessionEventsHandler
{
    private readonly ITestSessionEventsHandler _testSessionEventsHandler;

    public EventHandler<StartTestSessionCompleteEventArgs?>? StartTestSessionCompleteEventHandler { get; set; }

    public EventHandler<StopTestSessionCompleteEventArgs?>? StopTestSessionCompleteEventHandler { get; set; }

    public InProcessTestSessionEventsHandler(ITestSessionEventsHandler testSessionEventsHandler)
    {
        _testSessionEventsHandler = testSessionEventsHandler;
    }

    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        _testSessionEventsHandler.HandleLogMessage(level, message);
    }

    public void HandleRawMessage(string rawMessage)
    {
        _testSessionEventsHandler.HandleRawMessage(rawMessage);
    }

    public void HandleStartTestSessionComplete(StartTestSessionCompleteEventArgs? eventArgs)
    {
        StartTestSessionCompleteEventHandler?.Invoke(this, eventArgs);
        _testSessionEventsHandler.HandleStartTestSessionComplete(eventArgs);
    }

    public void HandleStopTestSessionComplete(StopTestSessionCompleteEventArgs? eventArgs)
    {
        StopTestSessionCompleteEventHandler?.Invoke(this, eventArgs);
        _testSessionEventsHandler.HandleStopTestSessionComplete(eventArgs);
    }
}
