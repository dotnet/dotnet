// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// An automatic progress reporter.
    /// </summary>
    public sealed class AutomaticProgressReporter : IDisposable
    {
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IConnection _connection;
        private bool _isDisposed;
        private readonly Message _request;
        private readonly SemaphoreSlim _semaphore;
        private readonly Timer _timer;

        private AutomaticProgressReporter(
            IConnection connection,
            Message request,
            TimeSpan interval,
            CancellationToken cancellationToken)
        {
            _connection = connection;
            _request = request;
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cancellationToken = _cancellationTokenSource.Token;
            _semaphore = new SemaphoreSlim(initialCount: 1, maxCount: 1);
            _timer = new Timer(OnTimer, state: null, dueTime: interval, period: interval);
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                try
                {
                    _semaphore.Wait(_cancellationToken);
                }
                catch (OperationCanceledException)
                {
                }
                catch (ObjectDisposedException)
                {
                }

                try
                {
                    using (_cancellationTokenSource)
                    {
                        _cancellationTokenSource.Cancel();
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    // The timer queues callbacks for execution by thread pool threads, so it is possible for a timer
                    // callback to be fired after Dispose() has been called.
                    // The Dispose(WaitHandle) overload, which should handle this race condition, is not available
                    // until .NET Core 2.0.  Until then synchronization is required to ensure that a timer callback
                    // does not fire after Dispose().  Otherwise, a progress notification might be sent after a
                    // response, which would be a fatal plugin protocol error.
                    _timer.Dispose();

                    // Do not dispose of _connection.  It is still in use by a plugin.

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
                finally
                {
                    try
                    {
                        _semaphore.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="AutomaticProgressReporter" /> class.
        /// </summary>
        /// <remarks>This class does not take ownership of and dispose of <paramref name="connection" />.</remarks>
        /// <param name="connection">A connection.</param>
        /// <param name="request">A request.</param>
        /// <param name="interval">A progress interval.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connection" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="interval" />
        /// is either less than <see cref="ProtocolConstants.MinTimeout" /> or greater than
        /// <see cref="ProtocolConstants.MaxTimeout" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public static AutomaticProgressReporter Create(
            IConnection connection,
            Message request,
            TimeSpan interval,
            CancellationToken cancellationToken)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!TimeoutUtilities.IsValid(interval))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(interval),
                    interval,
                    Strings.Plugin_TimeoutOutOfRange);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return new AutomaticProgressReporter(
                connection,
                request,
                interval,
                cancellationToken);
        }

        private void OnTimer(object state)
        {
            try
            {
                _semaphore.Wait(_cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (ArgumentNullException)
            {
                // The semaphore may have been disposed already.
                return;
            }

            if (_isDisposed)
            {
                return;
            }

            Task.Run(async () =>
                {
                    // Top-level exception handler for a worker pool thread.
                    try
                    {
                        var progress = MessageUtilities.Create(
                            _request.RequestId,
                            MessageType.Progress,
                            _request.Method,
                            new Progress());

                        await _connection.SendAsync(progress, _cancellationToken);
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                },
                _cancellationToken);
        }
    }
}
