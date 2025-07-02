namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.FileDiagnosticsModule
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Runtime.InteropServices;

    /// <summary>
    /// TraceSource that will report Application Insights diagnostics messages.
    /// </summary>
    [ComVisible(false)]
    internal class TraceSourceForEventSource : TraceSource, IEventListener, IDisposable
    {
        private const long AllKeyword = -1;

#if REDFIELD
        private const string TraceSourceName = "Redfield.Microsoft.ApplicationInsights.Extensibility.TraceSource";
#else
        private const string TraceSourceName = "Microsoft.ApplicationInsights.Extensibility.TraceSource";
#endif

        private DiagnosticsEventListener listener;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceForEventSource" /> class.
        /// </summary>
        public TraceSourceForEventSource()
            : base(TraceSourceName)
        {
            this.listener = new DiagnosticsEventListener(EventLevel.Error, (EventKeywords)AllKeyword, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceForEventSource" /> class.
        /// </summary>
        public TraceSourceForEventSource(SourceLevels defaultLevel)
            : base(TraceSourceName, defaultLevel)
        {
            this.listener = new DiagnosticsEventListener(GetEventLevelFromSourceLevels(defaultLevel), (EventKeywords)AllKeyword, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceForEventSource" /> class.
        /// </summary>
        public TraceSourceForEventSource(EventLevel defaultLevel)
            : base(TraceSourceName, GetSourceLevelsForEventLevel(defaultLevel))
        {
            this.listener = new DiagnosticsEventListener(defaultLevel, (EventKeywords)AllKeyword, this);
        }

        /// <summary>
        /// Gets or sets event level to subscribe to.
        /// </summary>
        public EventLevel LogLevel
        {
            get
            {
                return this.listener.LogLevel;
            }

            set
            {
                if (value == this.listener.LogLevel)
                {
                    return;
                }

                var oldListener = this.listener;
                
                // we will see duplicated event for some time while old listener will not be disposed
                this.listener = new DiagnosticsEventListener(value, (EventKeywords)AllKeyword, this);
                oldListener.Dispose();

                this.Switch.Level = GetSourceLevelsForEventLevel(value);
            }
        }

        /// <summary>
        /// Convert EventSource event to tracing event.
        /// </summary>
        /// <param name="eventData">Event to trace.</param>
        void IEventListener.OnEventWritten(EventWrittenEventArgs eventData)
        {
            this.TraceEvent(eventData);
        }

        /// <summary>
        /// Disposes diagnostics listener.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes diagnostics listener.
        /// </summary>
        protected virtual void Dispose(bool disposeManaged = true)
        {
            if (disposeManaged)
            {
                this.listener.Dispose();
            }
        }

        /// <summary>
        /// Trace event.
        /// </summary>
        /// <param name="eventData">Event to trace.</param>
        protected void TraceEvent(EventWrittenEventArgs eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }

            try
            {
                if (eventData.Payload != null)
                {
                    List<object> args = new List<object>(eventData.Payload.Count);
                    foreach (var i in eventData.Payload)
                    {
                        args.Add(i);
                    }

                    this.TraceEvent(GetTraceEventTypeForEventLevel(eventData.Level), eventData.EventId, eventData.Message, args.ToArray());
                }
                else
                {
                    this.TraceEvent(GetTraceEventTypeForEventLevel(eventData.Level), eventData.EventId, eventData.Message);
                }
            }
            catch (Exception)
            { 
                /* swallow - we don't want to crash because of tracing */
            }
        }

        private static EventLevel GetEventLevelFromSourceLevels(SourceLevels defaultLevel)
        {
            switch (defaultLevel)
            {
                case SourceLevels.ActivityTracing:
                    return EventLevel.Informational;
                case SourceLevels.All:
                    return EventLevel.Verbose;
                case SourceLevels.Critical:
                    return EventLevel.Critical;
                case SourceLevels.Error:
                    return EventLevel.Error;
                case SourceLevels.Information:
                    return EventLevel.Informational;
                case SourceLevels.Off:
                    return EventLevel.LogAlways;
                case SourceLevels.Verbose:
                    return EventLevel.Verbose;
                case SourceLevels.Warning:
                    return EventLevel.Warning;
                default:
                    return EventLevel.Error;
            }
        }

        private static TraceEventType GetTraceEventTypeForEventLevel(EventLevel level)
        {
            switch (level)
            {
                case EventLevel.Critical:
                    return TraceEventType.Critical;
                case EventLevel.Error:
                    return TraceEventType.Error;
                case EventLevel.Informational:
                    return TraceEventType.Information;
                case EventLevel.LogAlways:
                    return TraceEventType.Verbose;
                case EventLevel.Verbose:
                    return TraceEventType.Verbose;
                case EventLevel.Warning:
                    return TraceEventType.Warning;
                default:
                    return TraceEventType.Verbose;
            }
        }

        private static SourceLevels GetSourceLevelsForEventLevel(EventLevel level)
        {
            switch (level)
            {
                case EventLevel.Critical:
                    return SourceLevels.Critical;
                case EventLevel.Error:
                    return SourceLevels.Error;
                case EventLevel.Informational:
                    return SourceLevels.Information;
                case EventLevel.LogAlways:
                    return SourceLevels.Verbose;
                case EventLevel.Verbose:
                    return SourceLevels.Verbose;
                case EventLevel.Warning:
                    return SourceLevels.Warning;
                default:
                    return SourceLevels.Verbose;
            }
        }
    }
}
