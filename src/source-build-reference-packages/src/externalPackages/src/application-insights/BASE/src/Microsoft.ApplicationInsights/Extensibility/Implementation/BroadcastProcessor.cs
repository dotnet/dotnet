namespace Microsoft.ApplicationInsights.Shared.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// An <see cref="ITelemetryProcessor"/> that asynchronously sends the data to multiple child telemetry sinks.
    /// </summary>
    internal class BroadcastProcessor : ITelemetryProcessor
    {
        private TelemetryDispatcher[] childrenDispatchers;

        public BroadcastProcessor(IEnumerable<TelemetrySink> children)
        {
            if (children == null)
            {
                throw new ArgumentNullException(nameof(children));
            }

            int childrenCount = children.Count();
            if (childrenCount < 2)
            {
                throw new ArgumentException("BroadcastProcessor requires two or more children", nameof(children));
            }

            this.childrenDispatchers = new TelemetryDispatcher[childrenCount];
            bool firstChild = true;
            int i = 0;
            foreach (TelemetrySink child in children)
            {
                this.childrenDispatchers[i++] = new TelemetryDispatcher(child, !firstChild);
                firstChild = false;
            }
        }

        public void Process(ITelemetry item)
        {
            // The assumptions we are making here are that:
            //   1. TelemetryProcessors are reliable and fast.
            //   2. Channels are reliable and process data asynchronously (ITelemetryChannel.Send() just queues up the telemetry and returns quickly).
            // As a result of these assumptions we can just let each sink process the data synchronously, with acceptable performance.
            // But it is also true that a misbehaving telemetry processor or channel in one of the sinks will affect other sinks.

            // Why the reverse traversal? As a perf optimization we want to avoid unecessary .DeepClone(). So we send the
            // original item to the very first TelemetrySink, however first telemetry sink can choose to modify this object.
            // In this case all the telemetry sinks will get the modified object. Hence as a protection against this, we are going to
            // send the object through the first telemetry sink at the very last. At this point the first telemetry sink is free to
            // modify the object as we have no further use of it.
            for (int i = this.childrenDispatchers.Length - 1; i >= 0; i--)
            {
                this.childrenDispatchers[i].SendItemToSink(item);
            }
        }

        /// <summary>
        /// Utility class that clones incoming telemetry as necessary and sends it to one of the telemetry sinks 
        /// handled by the parent BroadcastProcessor.
        /// </summary>
        private class TelemetryDispatcher
        {
            private bool cloneBeforeDispatch;
            private TelemetrySink sink;

            public TelemetryDispatcher(TelemetrySink sink, bool cloneBeforeDispatch)
            {
                Debug.Assert(sink != null, "Telemetry sink should not be null");
                this.sink = sink;
                this.cloneBeforeDispatch = cloneBeforeDispatch;
            }

            /// <summary>
            /// Sends the item to sink. If cloning of item is required, clones the item before sending it to <see cref="TelemetrySink"/>.
            /// </summary>
            /// <param name="telemetry">The telemetry item to send to sink.</param>
            public void SendItemToSink(ITelemetry telemetry)
            {
                ITelemetry itemToSendToSink = this.cloneBeforeDispatch ? telemetry.DeepClone() : telemetry;

                if (itemToSendToSink != null)
                {
                    this.sink.Process(itemToSendToSink);
                }
                else
                {
                    Debug.Fail("Telemetry item should not be null");
                }
            }
        }
    }
}
