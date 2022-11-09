// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Diagnostics;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace vstest.ProgrammerTests.Fakes;

internal abstract class FakeCommunicationChannel : ICommunicationChannel
{
    public FakeCommunicationChannel(int id)
    {
        Id = id;
    }

    public int Id { get; }
    public CancellationTokenSource CancellationTokenSource { get; } = new();
    public BlockingCollection<string> InQueue { get; } = new();
    public BlockingCollection<FakeMessage> OutQueue { get; } = new();

    /// <summary>
    /// True if we encountered unexpected message (e.g. unknown message, message out of order) or when we sent all our prepared responses, and there were still more requests coming.
    /// </summary>
    public bool Faulted { get; protected set; }

    #region ICommunicationChannel
    // The naming for ICommunicationChannel is a bit confusing when this is implemented in-process.
    // Normally one side in one process would have one end of the communication channel,
    // and would use Send to pass message to another process. The other side would get notified
    // about new data by NotifyDataAvailable, read the data there, and send them to other consumers
    // using MessageReceived event. These consumers would then call Send, the channel and post the data
    // to the other process. The other process would recieve the data and be notified by MessageReceived.
    //
    // But when we are in the same process, one side sends data using Send. We recieve them here by reading
    // them from the queue (NotifyDataAvailable is not used, because we monitor the queue directly here, instead of
    // in communication manager). And then we reply back to the sender, by invoking MessageReceived.

    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;

    public void Dispose()
    {
        CancellationTokenSource.Cancel();
        InQueue.CompleteAdding();
        OutQueue.CompleteAdding();
    }

    public Task NotifyDataAvailable()
    {
        // This is used only by communication manager. vstest.console does not use communication manager.
        throw new NotImplementedException();
    }

    public Task Send(string data)
    {
        InQueue.Add(data);
        return Task.CompletedTask;
    }
    #endregion

    public void OnMessageReceived(object sender, MessageReceivedEventArgs eventArgs)
    {
        // This is still a race condition. In real code we solve this via SafeInvoke that does null check
        // and catches the exception. In this code I prefer doing it this way, to see if it is fragile.
        MessageReceived?.Invoke(this, eventArgs);
    }
}

internal class FakeCommunicationChannel<TContext> : FakeCommunicationChannel, ICommunicationChannel
{

    /// <summary>
    /// Queue of MessageType of the incoming request, and the response that will be sent back.
    /// </summary>
    public Queue<RequestResponsePair<string, FakeMessage, TContext>> NextResponses { get; } = new();
    public FakeErrorAggregator FakeErrorAggregator { get; }
    public FakeMessage? PendingMessage { get; private set; }
    public TContext? Context { get; private set; }
    public List<RequestResponsePair<Message, FakeMessage, TContext>> ProcessedMessages { get; } = new();
    public Task? ProcessIncomingMessagesTask { get; private set; }
    public Task? ProcessOutgoingMessagesTask { get; private set; }

    public FakeCommunicationChannel(List<RequestResponsePair<string, FakeMessage, TContext>> responses, FakeErrorAggregator fakeErrorAggregator, int id) : base(id)
    {
        FakeErrorAggregator = fakeErrorAggregator;
        responses.ForEach(NextResponses.Enqueue);
    }

    public void Start(TContext context)
    {
        Context = context;
        ProcessIncomingMessagesTask = Task.Run(() => ProcessIncomingMessages(context), CancellationTokenSource.Token);
        ProcessOutgoingMessagesTask = Task.Run(ProcessOutgoingMessages, CancellationTokenSource.Token);
    }

    private void ProcessOutgoingMessages()
    {
        var token = CancellationTokenSource.Token;
        while (!token.IsCancellationRequested)
        {
            try
            {
                // TODO: better name for the property? This is message that we are currently trying to send.
                PendingMessage = OutQueue.Take(token);
                OnMessageReceived(this, new MessageReceivedEventArgs { Data = PendingMessage.SerializedMessage });
                PendingMessage = null;
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                FakeErrorAggregator.Add(ex);
            }
        }
    }

    private void ProcessIncomingMessages(TContext context)
    {
        var token = CancellationTokenSource.Token;
        while (!token.IsCancellationRequested)
        {
            try
            {
                var rawMessage = InQueue.Take(token);
                var requestMessage = JsonDataSerializer.Instance.DeserializeMessage(rawMessage);

                if (Faulted)
                {
                    // We already failed, when there are more requests coming, just save them and respond with error. We want to avoid
                    // a situation where server ignores our error message and responds with another request, for which we accidentally
                    // have the right answer in queue.
                    //
                    // E.g. We have VersionCheck, TestRunStart prepared, and server sends: VersionCheck, TestInitialize, TestRunStart.
                    // The first request has a valid response. The next TestInitialize does not have a valid response and errors out,
                    // but the server ignores it, and sends TestRunStart, which would normally have a prepared response, and lead to
                    // possibly overlooking the error response to TestInitialize.
                    //
                    // With this check in place we will not respond to TestRunStart with success, but with error.
                    // TODO: Better way to map MessageType and the payload type.
                    // TODO: simpler way to report error, and add it to the error aggregator
                    var errorMessage = $"FakeCommunicationChannel: FakeCommunicationChannel: Got message {requestMessage.MessageType}. But a message that was unexptected was received previously and the channel is now faulted. Review {nameof(ProcessedMessages)}, and {nameof(NextResponses)}.";
                    var exception = new Exception(errorMessage);
                    FakeErrorAggregator.Add(exception);
                    var errorResponse = new FakeMessage<TestMessagePayload>(MessageType.TestMessage, new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = errorMessage });
                    ProcessedMessages.Add(new RequestResponsePair<Message, FakeMessage, TContext>(requestMessage, errorResponse));
                    Faulted = true;
                    OutQueue.Add(errorResponse);
                }

                // Just peek at it so we can keep the message on the the queue in case of error.
                if (!NextResponses.TryPeek(out var nextResponsePair))
                {
                    // If there are no more prepared responses then return protocol error.
                    var errorMessage = $"FakeCommunicationChannel: Got message {requestMessage.MessageType}, but no more requests were expected, because there are no more responses in {nameof(NextResponses)}.";
                    var exception = new Exception(errorMessage);
                    FakeErrorAggregator.Add(exception);
                    var errorResponse = new FakeMessage<TestMessagePayload>(MessageType.ProtocolError, new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = errorMessage });
                    ProcessedMessages.Add(new RequestResponsePair<Message, FakeMessage, TContext>(requestMessage, errorResponse));
                    Faulted = true;
                    OutQueue.Add(errorResponse);
                }
                else if (nextResponsePair.Request != requestMessage.MessageType)
                {
                    // If the incoming message does not match what we expected return protocol error. The lsat message will remain in the
                    // NextResponses queue.
                    var errorMessage = $"FakeCommunicationChannel: Excpected message {nextResponsePair.Request} but got {requestMessage.MessageType}.";
                    var exception = new Exception(errorMessage);
                    FakeErrorAggregator.Add(exception);
                    var errorResponse = new FakeMessage<TestMessagePayload>(MessageType.ProtocolError, new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = errorMessage });
                    ProcessedMessages.Add(new RequestResponsePair<Message, FakeMessage, TContext>(requestMessage, errorResponse));
                    Faulted = true;
                    OutQueue.Add(errorResponse);
                }
                else
                {
                    var responsePair = NextResponses.Dequeue();
                    if (responsePair.Debug && Debugger.IsAttached)
                    {
                        // We are about to send an interesting message
                        Debugger.Break();
                    }

                    responsePair.BeforeAction?.Invoke(context);
                    var responses = responsePair.Responses;
                    ProcessedMessages.Add(new RequestResponsePair<Message, FakeMessage, TContext>(requestMessage, responses, false));

                    foreach (var response in responses)
                    {
                        // If we created a pair with NoResponse message, we won't send that back to the server.
                        if (response != FakeMessage.NoResponse)
                        {
                            OutQueue.Add(response);
                        }
                    }

                    responsePair.AfterAction?.Invoke(context);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                FakeErrorAggregator.Add(ex);
            }
        }
    }

    public override string? ToString()
    {
        return NextResponses.Peek()?.ToString();
    }
}

