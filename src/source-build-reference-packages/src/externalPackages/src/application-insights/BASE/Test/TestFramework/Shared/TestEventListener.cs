namespace Microsoft.ApplicationInsights.TestFramework
{
    using Microsoft.ApplicationInsights.Extensibility;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Threading;
#if NET452
    using System.Runtime.Remoting.Messaging;
#endif

    internal class TestEventListener : EventListener
    {
#if NET452

        public static class CurrentContextEvents
        {
            internal const string InternalOperationsMonitorSlotName = "Microsoft.ApplicationInsights.TestEventListener";

            private static Object syncObj = new object();

            public static bool IsEntered()
            {
                object data = null;
                try
                {
                    data = CallContext.LogicalGetData(InternalOperationsMonitorSlotName);
                }
                catch (Exception)
                {
                    // CallContext may fail in partially trusted environment
                }

                return data != null;
            }

            public static void Enter()
            {
                try
                {
                    CallContext.LogicalSetData(InternalOperationsMonitorSlotName, syncObj);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Please run this test in full trust environment");
                }
            }

            public static void Exit()
            {
                try
                {
                    CallContext.FreeNamedDataSlot(InternalOperationsMonitorSlotName);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Please run this test in full trust environment");
                }
            }
        }
#else
        public static class CurrentContextEvents
        {
            private static AsyncLocal<object> asyncLocalContext = new AsyncLocal<object>();

            private static object syncObj = new object();

            public static bool IsEntered()
            {
                return asyncLocalContext.Value != null;
            }

            public static void Enter()
            {
                asyncLocalContext.Value = syncObj;
            }

            public static void Exit()
            {
                asyncLocalContext.Value = null;
            }
        }
#endif

        private readonly ConcurrentQueue<EventWrittenEventArgs> events;
        private readonly AutoResetEvent eventWritten;

        private readonly bool waitForDelayedEvents;
        private readonly bool listenForCurrentContext;

        public TestEventListener(bool waitForDelayedEvents = true, bool listenForCurrentContext = true)
        {
            this.events = new ConcurrentQueue<EventWrittenEventArgs>();
            this.eventWritten = new AutoResetEvent(false);
            this.waitForDelayedEvents = waitForDelayedEvents;
            this.listenForCurrentContext = listenForCurrentContext;
            this.OnOnEventWritten = e =>
            {
                this.events.Enqueue(e);
                this.eventWritten.Set();
            };

            if (this.listenForCurrentContext)
            {
                CurrentContextEvents.Enter();
            }
        }

        public Action<EventSource> OnOnEventSourceCreated { get; set; }

        public Action<EventWrittenEventArgs> OnOnEventWritten { get; set; }

        public IEnumerable<EventWrittenEventArgs> Messages
        {
            get 
            {
                if (this.events.Count == 0 && this.waitForDelayedEvents)
                {
                    this.eventWritten.WaitOne(TimeSpan.FromSeconds(5));
                }

                while (this.events.Count != 0)
                {
                    EventWrittenEventArgs nextEvent;
                    if (this.events.TryDequeue(out nextEvent))
                    {
                        yield return nextEvent;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
        
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (listenForCurrentContext && CurrentContextEvents.IsEntered())
            {
                this.OnOnEventWritten(eventData);
            }
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // Check for null because this method is called by the base class constructor before we can initialize it
            Action<EventSource> callback = this.OnOnEventSourceCreated;
            if (callback != null)
            {
                callback(eventSource);
            }
        }

        public override void Dispose()
        {
            if (listenForCurrentContext)
            {
                CurrentContextEvents.Exit();
            }
            base.Dispose();
        }
    }
}
