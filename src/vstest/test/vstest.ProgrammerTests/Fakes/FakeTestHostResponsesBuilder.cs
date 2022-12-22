// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace vstest.ProgrammerTests.Fakes;

/// <summary>
/// Builds a list of RequestResponse pairs, with the provided values. Each method is a name of incoming message type.
/// The order in which the builder methods are called determines the order or responses.
/// </summary>
internal class FakeTestHostResponsesBuilder
{
    private readonly List<RequestResponsePair<string, FakeMessage, FakeTestHostFixture>> _responses = new();

    /// <summary>
    /// For VersionCheck message it responds with VersionCheck response that has the given version.
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    internal FakeTestHostResponsesBuilder VersionCheck(int version)
    {
        AddPairWithValue(MessageType.VersionCheck, version);
        return this;
    }

    /// <summary>
    /// For VersionCheck message it responds with the given FakeMessage.
    /// </summary>
    /// <param name="message">Message to respond with, or FakeMessage.NoResponse to not respond.</param>
    /// <returns></returns>
    internal FakeTestHostResponsesBuilder VersionCheck(FakeMessage message)
    {
        AddPairWithFakeMessage(MessageType.VersionCheck, message);
        return this;
    }

    /// <summary>
    /// For VersionCheck message it does the given before action and responds with the given FakeMessage and then does the given after action.
    /// Use FakeMessage.NoResponse to not respond.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    internal FakeTestHostResponsesBuilder VersionCheck(FakeMessage message, Action<FakeTestHostFixture>? beforeAction = null, Action<FakeTestHostFixture>? afterAction = null)
    {
        AddPairWithFakeMessage(MessageType.VersionCheck, message, beforeAction, afterAction);
        return this;
    }

    internal FakeTestHostResponsesBuilder ExecutionInitialize(FakeMessage message)
    {
        AddPairWithFakeMessage(MessageType.ExecutionInitialize, message);
        return this;
    }

    internal FakeTestHostResponsesBuilder StartTestExecutionWithSources(FakeMessage message, Action<FakeTestHostFixture>? beforeAction = null, Action<FakeTestHostFixture>? afterAction = null)
    {
        AddPairWithFakeMessage(MessageType.StartTestExecutionWithSources, message, beforeAction, afterAction);
        return this;
    }

    internal FakeTestHostResponsesBuilder StartTestExecutionWithSources(List<List<TestResult>> testResultBatches)
    {
        ValidateArg.NotNull(testResultBatches, nameof(testResultBatches));

        List<FakeMessage> messages;
        if (testResultBatches.Count != 0)
        {
            // this will create as many test stats changes messages, as there are batches -1
            // the last batch will be sent as test run complete event

            // TODO: make the stats agree with the tests below
            List<FakeMessage> changeMessages = testResultBatches.Take(testResultBatches.Count - 1).Select(batch =>
                new FakeMessage<TestRunChangedEventArgs>(MessageType.TestRunStatsChange,
                      new TestRunChangedEventArgs(new TestRunStatistics(new Dictionary<TestOutcome, long> { [TestOutcome.Passed] = batch.Count }), batch, new List<TestCase>())
                     )).ToList<FakeMessage>();

            // TODO: This is finicky because the statistics processor expects the dictionary to not be null
            FakeMessage completedMessage = new FakeMessage<TestRunCompletePayload>(MessageType.ExecutionComplete, new TestRunCompletePayload
            {
                // TODO: make the stats agree with the tests below
                TestRunCompleteArgs = new TestRunCompleteEventArgs(new TestRunStatistics(new Dictionary<TestOutcome, long> { [TestOutcome.Passed] = 1 }), false, false, null, new System.Collections.ObjectModel.Collection<AttachmentSet>(), TimeSpan.Zero),
                LastRunTests = new TestRunChangedEventArgs(new TestRunStatistics(new Dictionary<TestOutcome, long> { [TestOutcome.Passed] = 1 }), testResultBatches.Last(), new List<TestCase>()),
            });
            messages = changeMessages.Concat(new[] { completedMessage }).ToList();
        }
        else
        {
            var completedMessage = new FakeMessage<TestRunCompletePayload>(MessageType.ExecutionComplete, new TestRunCompletePayload
            {
                TestRunCompleteArgs = new TestRunCompleteEventArgs(new TestRunStatistics(new Dictionary<TestOutcome, long> { [TestOutcome.Passed] = 0 }), false, false, null, new System.Collections.ObjectModel.Collection<AttachmentSet>(), TimeSpan.Zero),
                LastRunTests = new TestRunChangedEventArgs(new TestRunStatistics(new Dictionary<TestOutcome, long> { [TestOutcome.Passed] = 0 }), new List<TestResult>(), new List<TestCase>()),
            });

            messages = completedMessage.AsList<FakeMessage>();
        }


        AddPairWithMultipleFakeMessages(MessageType.StartTestExecutionWithSources, messages);
        return this;
    }


    internal FakeTestHostResponsesBuilder SessionEnd(FakeMessage fakeMessage)
    {
        AddPairWithFakeMessage(MessageType.SessionEnd, fakeMessage);
        return this;
    }

    internal FakeTestHostResponsesBuilder SessionEnd(FakeMessage message, Action<FakeTestHostFixture>? beforeAction = null, Action<FakeTestHostFixture>? afterAction = null)
    {
        AddPairWithFakeMessage(MessageType.SessionEnd, message, beforeAction, afterAction);
        return this;
    }

    private void AddPairWithValue<T>(string messageType, T value, Action<FakeTestHostFixture>? beforeAction = null, Action<FakeTestHostFixture>? afterAction = null)
    {
        AddPairWithFakeMessage(messageType, new FakeMessage<T>(messageType, value), beforeAction, afterAction);
    }

    private void AddPairWithFakeMessage(string messageType, FakeMessage message, Action<FakeTestHostFixture>? beforeAction = null, Action<FakeTestHostFixture>? afterAction = null)
    {
        AddPairWithMultipleFakeMessages(messageType, new[] { message }, beforeAction, afterAction);
    }

    private void AddPairWithMultipleFakeMessages(string messageType, IEnumerable<FakeMessage> messages, Action<FakeTestHostFixture>? beforeAction = null, Action<FakeTestHostFixture>? afterAction = null)
    {
        _responses.Add(new RequestResponsePair<string, FakeMessage, FakeTestHostFixture>(messageType, messages, beforeAction, afterAction));
    }

    internal List<RequestResponsePair<string, FakeMessage, FakeTestHostFixture>> Build()
    {
        return _responses;
    }

    internal FakeTestHostResponsesBuilder DiscoveryInitialize(FakeMessage fakeMessage)
    {
        AddPairWithFakeMessage(MessageType.DiscoveryInitialize, fakeMessage);
        return this;
    }

    internal FakeTestHostResponsesBuilder StartDiscovery(List<List<TestResult>> testResultBatches)
    {
        // Discovery returns back test cases, not test results, but it is easier to take test results, because
        // we have a builder that can be re-used for both test run and test discovery.

        List<FakeMessage> messages;
        if (testResultBatches.Count != 0)
        {
            // this will create as many test stats changes messages, as there are batches -1
            // the last batch will be sent as test run complete event

            // see TestRequestSender.OnDiscoveryMessageReceived to see how the vstest.console receives the data
            List<FakeMessage> changeMessages = testResultBatches.Take(testResultBatches.Count - 1)
                .Select(batch => new FakeMessage<IEnumerable<TestCase>>(MessageType.TestCasesFound, batch.Select(testResult => testResult.TestCase).ToList()))
                .ToList<FakeMessage>();

            // TODO: if we send this incorrectly the handler just continues, check logs if we can understand it from there. We should at least write a warning.
            // because otherwise it hangs.
            FakeMessage completedMessage = new FakeMessage<DiscoveryCompletePayload>(MessageType.DiscoveryComplete, new DiscoveryCompletePayload
            {
                LastDiscoveredTests = testResultBatches.Last().Select(testResult => testResult.TestCase).ToList(),
            });
            messages = changeMessages.Concat(new[] { completedMessage }).ToList();
        }
        else
        {
            FakeMessage completedMessage = new FakeMessage<DiscoveryCompletePayload>(MessageType.DiscoveryComplete, new DiscoveryCompletePayload
            {
                LastDiscoveredTests = new List<TestCase>(),
            });

            messages = completedMessage.AsList();
        }

        AddPairWithMultipleFakeMessages(MessageType.StartDiscovery, messages);

        return this;
    }
}
