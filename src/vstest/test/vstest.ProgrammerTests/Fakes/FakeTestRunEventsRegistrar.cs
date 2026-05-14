// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeTestRunEventsRegistrar : ITestRunEventsRegistrar
{
    public Guid Id { get; } = Guid.NewGuid();
    public FakeTestRunEventsRegistrar(FakeErrorAggregator fakeErrorAggregator)
    {
        FakeErrorAggregator = fakeErrorAggregator;
    }

    public List<object> AllEvents { get; } = new();
    public List<string> LoggedWarnings { get; } = new();
    public List<EventRecord<TestRunCompleteEventArgs>> RunCompleteEvents { get; } = new();
    public List<EventRecord<TestRunStartEventArgs>> RunStartEvents { get; } = new();
    public List<EventRecord<TestRunChangedEventArgs>> RunChangedEvents { get; } = new();
    public List<EventRecord<string>> RawMessageEvents { get; } = new();
    public List<EventRecord<TestRunMessageEventArgs>> RunMessageEvents { get; } = new();
    public FakeErrorAggregator FakeErrorAggregator { get; }

    public void LogWarning(string message)
    {
        AllEvents.Add(message);
        LoggedWarnings.Add(message);
    }

    public void RegisterTestRunEvents(ITestRunRequest testRunRequest)
    {
        testRunRequest.TestRunMessage += OnTestRunMessage;
        testRunRequest.OnRawMessageReceived += OnRawMessage;
        testRunRequest.OnRunStart += OnRunStart;
        testRunRequest.OnRunStatsChange += OnRunStatsChange;
        testRunRequest.OnRunCompletion += OnRunCompletion;
    }

    public void UnregisterTestRunEvents(ITestRunRequest testRunRequest)
    {
        testRunRequest.TestRunMessage -= OnTestRunMessage;
        testRunRequest.OnRawMessageReceived -= OnRawMessage;
        testRunRequest.OnRunStart -= OnRunStart;
        testRunRequest.OnRunStatsChange -= OnRunStatsChange;
        testRunRequest.OnRunCompletion -= OnRunCompletion;
    }

    private void OnRunCompletion(object? sender, TestRunCompleteEventArgs e)
    {
        var eventRecord = new EventRecord<TestRunCompleteEventArgs>(sender, e);
        AllEvents.Add(eventRecord);
        RunCompleteEvents.Add(eventRecord);
    }

    private void OnRunStart(object? sender, TestRunStartEventArgs e)
    {
        var eventRecord = new EventRecord<TestRunStartEventArgs>(sender, e);
        AllEvents.Add(eventRecord);
        RunStartEvents.Add(eventRecord);
    }

    private void OnRunStatsChange(object? sender, TestRunChangedEventArgs e)
    {
        var eventRecord = new EventRecord<TestRunChangedEventArgs>(sender, e);
        AllEvents.Add(eventRecord);
        RunChangedEvents.Add(eventRecord);
    }

    private void OnRawMessage(object? sender, string e)
    {
        var eventRecord = new EventRecord<string>(sender, e);
        AllEvents.Add(eventRecord);
        RawMessageEvents.Add(eventRecord);
    }

    private void OnTestRunMessage(object? sender, TestRunMessageEventArgs e)
    {
        var eventRecord = new EventRecord<TestRunMessageEventArgs>(sender, e);
        if (e.Level == TestMessageLevel.Error)
        {
            FakeErrorAggregator.Errors.Add(eventRecord);
        }
        AllEvents.Add(eventRecord);
        RunMessageEvents.Add(eventRecord);
    }
}
