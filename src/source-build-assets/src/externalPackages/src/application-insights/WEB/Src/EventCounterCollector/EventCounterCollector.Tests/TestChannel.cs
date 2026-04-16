using Microsoft.ApplicationInsights.Channel;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EventCounterCollector.Tests
{
    internal class TestChannel : ITelemetryChannel
    {
        public bool? DeveloperMode { get; set; }
        public string EndpointAddress { get; set; }

        public ConcurrentQueue<ITelemetry> receivedItems;

        public TestChannel(ConcurrentQueue<ITelemetry> items)
        {
            this.receivedItems = items;
        }

        public void Flush()
        {
            
        }

        public void Send(ITelemetry item)
        {
            receivedItems.Enqueue(item);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}