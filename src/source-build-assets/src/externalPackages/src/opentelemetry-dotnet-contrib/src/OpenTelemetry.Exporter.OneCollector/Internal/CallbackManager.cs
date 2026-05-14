// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Exporter.OneCollector;

internal sealed class CallbackManager<T> : IDisposable
    where T : Delegate
{
    private readonly Lock lockObject = new();
    private bool disposed;

    public T? Root { get; private set; }

    public IDisposable Add(T callback)
    {
        Debug.Assert(callback != null, "callback was null");

        lock (this.lockObject)
        {
#if NET
            ObjectDisposedException.ThrowIf(this.disposed, nameof(CallbackManager<>));
#else
            if (this.disposed)
            {
                throw new ObjectDisposedException(nameof(CallbackManager<>));
            }
#endif

            this.Root = (T)Delegate.Combine(this.Root, callback);
        }

        return new CallbackManagerRegistration(() =>
        {
            lock (this.lockObject)
            {
                this.Root = (T?)Delegate.Remove(this.Root, callback);
            }
        });
    }

    public void Dispose()
    {
        lock (this.lockObject)
        {
            this.Root = null;
            this.disposed = true;
        }
    }

    private sealed class CallbackManagerRegistration : IDisposable
    {
        private readonly Action disposeAction;

        public CallbackManagerRegistration(Action disposeAction)
        {
            this.disposeAction = disposeAction;
        }

        public void Dispose()
            => this.disposeAction();
    }
}
