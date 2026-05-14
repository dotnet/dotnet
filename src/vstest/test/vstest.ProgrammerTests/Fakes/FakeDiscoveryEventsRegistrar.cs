// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeTestDiscoveryEventsRegistrar : ITestDiscoveryEventsRegistrar
{
    private readonly FakeErrorAggregator _fakeErrorAggregator;

    public List<object> AllEvents { get; } = new();
    public List<string> LoggedWarnings { get; } = new();
    public List<EventRecord<DiscoveryCompleteEventArgs>> DiscoveryCompleteEvents { get; } = new();
    public List<EventRecord<DiscoveredTestsEventArgs>> DiscoveredTestsEvents { get; } = new();
    public List<EventRecord<DiscoveryStartEventArgs>> DiscoveryStartEvents { get; } = new();
    public List<EventRecord<TestRunMessageEventArgs>> DiscoveryMessageEvents { get; } = new();

    public FakeTestDiscoveryEventsRegistrar(FakeErrorAggregator fakeErrorAggregator)
    {
        _fakeErrorAggregator = fakeErrorAggregator;
    }

    public void LogWarning(string message)
    {
        AllEvents.Add(message);
        LoggedWarnings.Add(message);
    }

    public void RegisterDiscoveryEvents(IDiscoveryRequest discoveryRequest)
    {
        discoveryRequest.OnDiscoveryMessage += OnDiscoveryMessage;
        discoveryRequest.OnDiscoveryStart += OnDiscoveryStart;
        discoveryRequest.OnDiscoveredTests += OnDiscoveredTests;
        discoveryRequest.OnDiscoveryComplete += OnDiscoveryComplete;
    }

    public void UnregisterDiscoveryEvents(IDiscoveryRequest discoveryRequest)
    {
        discoveryRequest.OnDiscoveryMessage -= OnDiscoveryMessage;
        discoveryRequest.OnDiscoveryStart -= OnDiscoveryStart;
        discoveryRequest.OnDiscoveredTests -= OnDiscoveredTests;
        discoveryRequest.OnDiscoveryComplete -= OnDiscoveryComplete;
    }

    private void OnDiscoveryComplete(object? sender, DiscoveryCompleteEventArgs e)
    {
        var eventRecord = new EventRecord<DiscoveryCompleteEventArgs>(sender, e);
        AllEvents.Add(eventRecord);
        DiscoveryCompleteEvents.Add(eventRecord);
    }

    private void OnDiscoveredTests(object? sender, DiscoveredTestsEventArgs e)
    {
        var eventRecord = new EventRecord<DiscoveredTestsEventArgs>(sender, e);
        AllEvents.Add(eventRecord);
        DiscoveredTestsEvents.Add(eventRecord);
    }

    private void OnDiscoveryStart(object? sender, DiscoveryStartEventArgs e)
    {
        var eventRecord = new EventRecord<DiscoveryStartEventArgs>(sender, e);
        AllEvents.Add(eventRecord);
        DiscoveryStartEvents.Add(eventRecord);
    }

    private void OnDiscoveryMessage(object? sender, TestRunMessageEventArgs e)
    {
        var eventRecord = new EventRecord<TestRunMessageEventArgs>(sender, e);
        AllEvents.Add(eventRecord);
        DiscoveryMessageEvents.Add(eventRecord);
    }
}
