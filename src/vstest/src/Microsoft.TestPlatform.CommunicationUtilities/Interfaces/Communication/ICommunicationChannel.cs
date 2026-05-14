// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

public interface ICommunicationChannel : IDisposable
{
    /// <summary>
    /// Event raised when data is received on the communication channel.
    /// </summary>
    TrackableEvent<MessageReceivedEventArgs> MessageReceived { get; }

    /// <summary>
    /// Frames and sends the provided data over communication channel.
    /// </summary>
    /// <param name="data">Data to send over the channel.</param>
    /// <returns>A <see cref="Task"/> implying async nature of the function.</returns>
    Task Send(string data);

    /// <summary>
    /// Notification from server/client that data is available.
    /// </summary>
    /// <returns>A <see cref="Task"/> implying async nature of the function.</returns>
    Task NotifyDataAvailable(CancellationToken cancellationToken);
}

#pragma warning disable CA1001 // Types that own disposable fields should be disposable
public class TrackableEvent<T>
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
{
    private readonly ManualResetEventSlim _slim;

    internal event EventHandler<T>? Event;

    public TrackableEvent()
    {
        _slim = new ManualResetEventSlim(Event != null);
    }

    public virtual void Notify(object sender, T eventArgs, string traceDisplayName)
    {
        var e = Event;
        if (e != null)
        {
            e.SafeInvoke(sender, eventArgs!, traceDisplayName);
        }
    }

    public virtual bool WaitForSubscriber(int timeoutMilliseconds, CancellationToken cancellationToken)
    {
        return _slim.Wait(timeoutMilliseconds, cancellationToken);
    }

    public virtual void Subscribe(EventHandler<T>? eventHandler)
    {
        Event += eventHandler;
        if (Event != null)
        {
            _slim.Set();
        }
    }

    public virtual void Unsubscribe(EventHandler<T>? eventHandler)
    {
        Event -= eventHandler;
        if (Event == null)
        {
            _slim.Reset();
        }
    }
}
