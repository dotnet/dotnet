// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Represents a unidirectional communications channel from a target.
    /// </summary>
    /// <remarks>
    /// Any public static members of this type are thread safe.
    /// Any instance members are not guaranteed to be thread safe.
    /// </remarks>
    public sealed class StandardInputReceiver : Receiver
    {
        private readonly TextReader _reader;
        private readonly CancellationTokenSource _receiveCancellationTokenSource;
        private Task _receiveThread;

        /// <summary>
        /// Instantiates a new <see cref="StandardInputReceiver" /> class.
        /// </summary>
        /// <param name="reader">A text reader.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="reader" /> is <see langword="null" />.</exception>
        public StandardInputReceiver(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            _reader = reader;
            _receiveCancellationTokenSource = new CancellationTokenSource();
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                Close();

                try
                {
                    using (_receiveCancellationTokenSource)
                    {
                        _receiveCancellationTokenSource.Cancel();

                        // Do not attempt to wait on completion of the receive thread task.
                        // In scenarios where standard input is backed by a non-blocking stream
                        // (e.g.:  a MemoryStream in unit tests) waiting on the receive thread task
                        // is fine.  However, when standard input is backed by a blocking stream,
                        // reading from standard input is a blocking call, and while the receive
                        // thread is in a read call it cannot respond to cancellation requests.
                        // We would likely stop responding if we attempted to wait on completion of the
                        // receive thread task.
                    }
                }
                catch (Exception)
                {
                }

                _reader.Dispose();
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Connects.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if this object is closed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if this method has already been called.</exception>
        public override void Connect()
        {
            ThrowIfDisposed();
            ThrowIfClosed();

            if (_receiveThread != null)
            {
                throw new InvalidOperationException(Strings.Plugin_ConnectionAlreadyStarted);
            }

            _receiveThread = Task.Factory.StartNew(
                Receive,
                _receiveCancellationTokenSource.Token,
                _receiveCancellationTokenSource.Token,
                TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default);
        }

        private void Receive(object state)
        {
            Message message = null;

            try
            {
                var cancellationToken = (CancellationToken)state;

                string line;

                // Reading from the standard input stream is a blocking call; while we're
                // in a read call we can't respond to cancellation requests.
                while (!IsClosed && (line = _reader.ReadLine()) != null)
                {
                    message = null;

                    cancellationToken.ThrowIfCancellationRequested();

                    message = JsonSerializationUtilities.Deserialize<Message>(line);

                    if (message != null)
                    {
                        FireMessageReceivedEvent(message);
                    }
                }
            }
            catch (Exception ex)
            {
                FireFaultEvent(ex, message);
            }
        }
    }
}
