// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Represents a unidirectional communications channel from a target.
    /// </summary>
    public abstract class Receiver : IReceiver
    {
        /// <summary>
        /// Gets a flag indicating whether or not this instance is closed.
        /// </summary>
        protected bool IsClosed { get; private set; }

        /// <summary>
        /// Gets or sets a flag indicating whether or not this instance is disposed.
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// Occurs when an unrecoverable fault has been caught.
        /// </summary>
        public event EventHandler<ProtocolErrorEventArgs> Faulted;

        /// <summary>
        /// Occurs when a message has been received.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <remarks>This does not call <see cref="IDisposable.Dispose" />.</remarks>
        public virtual void Close()
        {
            IsClosed = true;
        }

        /// <summary>
        /// Connects.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if this object is closed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if this method has already been called.</exception>
        public abstract void Connect();

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);

        protected void FireFaultEvent(Exception exception, Message message)
        {
            var ex = new ProtocolException(Strings.Plugin_ProtocolException, exception);
            var eventArgs = message == null
                ? new ProtocolErrorEventArgs(ex) : new ProtocolErrorEventArgs(ex, message);

            Faulted?.Invoke(this, eventArgs);
        }

        protected void FireMessageReceivedEvent(Message message)
        {
            MessageReceived?.Invoke(this, new MessageEventArgs(message));
        }

        protected void ThrowIfClosed()
        {
            if (IsClosed)
            {
                throw new InvalidOperationException(Strings.Plugin_ConnectionIsClosed);
            }
        }

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
