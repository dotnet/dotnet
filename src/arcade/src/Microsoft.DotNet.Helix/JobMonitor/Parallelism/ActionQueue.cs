// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Channels;

namespace Microsoft.DotNet.Helix.JobMonitor.Parallelism;

internal sealed class ActionQueue<T> : IAsyncDisposable
{
    private readonly Channel<T> _channel;
    private readonly Func<T, CancellationToken, ValueTask> _action;
    private readonly CancellationTokenSource _shutdown = new();
    private readonly Task[] _workers;
    private long _accepted;
    private long _started;
    private long _completed;
    private int _active;

    public ActionQueue(
        int capacity,
        int parallelism,
        Func<T, CancellationToken, ValueTask> action)
        : this(
            Channel.CreateBounded<T>(new BoundedChannelOptions(capacity)
            {
                AllowSynchronousContinuations = false,
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = parallelism == 1,
                SingleWriter = false,
            }),
            parallelism,
            action)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);
    }

    public ActionQueue(
        int parallelism,
        Func<T, CancellationToken, ValueTask> action)
        : this(
            Channel.CreateUnbounded<T>(new UnboundedChannelOptions
            {
                AllowSynchronousContinuations = false,
                SingleReader = parallelism == 1,
                SingleWriter = false,
            }),
            parallelism,
            action)
    {
    }

    private ActionQueue(
        Channel<T> channel,
        int parallelism,
        Func<T, CancellationToken, ValueTask> action)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(parallelism);
        ArgumentNullException.ThrowIfNull(action);

        _action = action;
        _channel = channel;
        _workers = Enumerable.Range(0, parallelism)
            .Select(_ => WorkerAsync())
            .ToArray();
    }

    public QueueSnapshot Snapshot => new(
        Accepted: Interlocked.Read(ref _accepted),
        Started: Interlocked.Read(ref _started),
        Completed: Interlocked.Read(ref _completed),
        Active: Volatile.Read(ref _active));

    public bool TryEnqueue(T item)
    {
        if (!_channel.Writer.TryWrite(item))
        {
            return false;
        }

        Interlocked.Increment(ref _accepted);
        return true;
    }

    public async ValueTask EnqueueAsync(T item, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(item, cancellationToken);
        Interlocked.Increment(ref _accepted);
    }

    public void Complete() => _channel.Writer.TryComplete();

    public void Cancel() => _shutdown.Cancel();

    public Task DrainAsync() => Task.WhenAll(_workers);

    public async ValueTask DisposeAsync()
    {
        _channel.Writer.TryComplete();
        _shutdown.Cancel();

        try
        {
            await Task.WhenAll(_workers);
        }
        catch (Exception) when (_shutdown.IsCancellationRequested)
        {
        }

        _shutdown.Dispose();
    }

    private async Task WorkerAsync()
    {
        await foreach (T item in _channel.Reader.ReadAllAsync(_shutdown.Token))
        {
            Interlocked.Increment(ref _started);
            Interlocked.Increment(ref _active);
            try
            {
                await _action(item, _shutdown.Token);
            }
            catch (Exception ex) when (!_shutdown.IsCancellationRequested)
            {
                _channel.Writer.TryComplete(ex);
                _shutdown.Cancel();
                throw;
            }
            finally
            {
                Interlocked.Decrement(ref _active);
                Interlocked.Increment(ref _completed);
            }
        }
    }
}

internal readonly record struct QueueSnapshot(
    long Accepted,
    long Started,
    long Completed,
    int Active)
{
    public long Queued => Accepted - Started;
}
