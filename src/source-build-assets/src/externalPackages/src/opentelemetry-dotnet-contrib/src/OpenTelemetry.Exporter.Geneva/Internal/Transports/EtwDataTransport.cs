// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using System.Diagnostics.Tracing;

namespace OpenTelemetry.Exporter.Geneva.Transports;

internal sealed class EtwDataTransport : IDataTransport, IDisposable
{
    private readonly EtwEventSource eventSource;
    private bool disposed;

    public EtwDataTransport(string providerName)
    {
        this.eventSource = new EtwEventSource(providerName);
    }

    public void Send(byte[] data, int size)
    {
        this.eventSource.SendEvent((int)EtwEventSource.EtwEventId.TraceEvent, data, size);
    }

    public bool IsEnabled()
    {
        return this.eventSource.IsEnabled();
    }

    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        this.eventSource.Dispose();

        this.disposed = true;
    }

    private sealed class EtwEventSource : EventSource
    {
        public EtwEventSource(string providerName)
            : base(providerName, EventSourceSettings.EtwManifestEventFormat)
        {
        }

        public enum EtwEventId
        {
            TraceEvent = 100,
        }

#pragma warning disable CA1822 // Mark members as static

        [Event((int)EtwEventId.TraceEvent, Version = 1, Level = EventLevel.Informational)]
        public void InformationalEvent()
        {
        }

#pragma warning restore CA1822 // Mark members as static

        [NonEvent]
#if NET
        [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "WriteEventCore is safe when eventData object is a primitive type, which it is in this case.")]
#endif
        public unsafe void SendEvent(int eventId, byte[] data, int size)
        {
            var dataDesc = stackalloc EventData[1];
            fixed (byte* ptr = data)
            {
                dataDesc[0].DataPointer = (IntPtr)ptr;
                dataDesc[0].Size = size;
                this.WriteEventCore(eventId, 1, dataDesc);
            }
        }
    }
}
