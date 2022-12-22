#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Globalization;

    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Provides methods for listening to events from FrameworkEventSource for SQL.
    /// </summary>
    internal class FrameworkSqlEventListener : EventListener
    {
        /// <summary>
        /// The SQL processor.
        /// </summary>
        internal readonly FrameworkSqlProcessing SqlProcessingFramework;

        /// <summary>
        /// The Framework EventSource name for SQL. 
        /// </summary>
        private const string AdoNetEventSourceName = "Microsoft-AdoNet-SystemData";

        /// <summary>
        /// The Microsoft.Data.SqlClient EventSource name for SQL. 
        /// </summary>
        private const string MdsEventSourceName = "Microsoft.Data.SqlClient.EventSource";

        /// <summary>
        /// BeginExecute Event ID.
        /// </summary>
        private const int BeginExecuteEventId = 1;

        /// <summary>
        /// EndExecute Event ID.
        /// </summary>
        private const int EndExecuteEventId = 2;

        /// <summary>
        /// Indicates whether SQL command text should be collected or not.
        /// </summary>
        private readonly bool collectCommandText;

        internal FrameworkSqlEventListener(TelemetryConfiguration configuration, CacheBasedOperationHolder telemetryTupleHolder, bool collectCommandText)
        {
            this.SqlProcessingFramework = new FrameworkSqlProcessing(configuration, telemetryTupleHolder);
            this.collectCommandText = collectCommandText;
        }

        private enum CompositeState
        {
            Success = 1,
            IsSqlException = 2,
            Synchronous = 4,
        }

        /// <summary>
        /// Enables SQL event source when EventSource is created. Called for all existing 
        /// event sources when the event listener is created and when a new event source is attached to the listener.
        /// </summary>
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource != null && eventSource.Name == AdoNetEventSourceName)
            {
                this.EnableEvents(eventSource, EventLevel.Informational, (EventKeywords)1);
                DependencyCollectorEventSource.Log.RemoteDependencyModuleVerbose("SqlEventListener initialized for event source:" + AdoNetEventSourceName);
            }

            if (eventSource != null && eventSource.Name == MdsEventSourceName)
            {
                this.EnableEvents(eventSource, EventLevel.Informational, (EventKeywords)1);
                DependencyCollectorEventSource.Log.RemoteDependencyModuleVerbose("SqlEventListener initialized for event source:" + MdsEventSourceName);
            }

            base.OnEventSourceCreated(eventSource);
        }

        /// <summary>
        /// Called whenever an event has been written by an event source for which the event listener has enabled events.
        /// </summary>
        /// <param name="eventData">The event arguments that describe the event.</param>
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData == null || eventData.Payload == null)
            {
                return;
            }

            try
            {
                switch (eventData.EventId)
                {
                    case BeginExecuteEventId:
                        this.OnBeginExecute(eventData);
                        break;
                    case EndExecuteEventId:
                        this.OnEndExecute(eventData);
                        break;
                }
            }
            catch (Exception exc)
            {
                DependencyCollectorEventSource.Log.CallbackError(0, "FrameworkSqlEventListener.OnEventWritten", exc);
            }
        }

        /// <summary>
        /// Called when a postfix of a SQLCommand begin methods have been invoked.
        /// </summary>
        /// <param name="eventData">The event arguments that describe the event.</param>
        private void OnBeginExecute(EventWrittenEventArgs eventData)
        {
            if (eventData.Payload.Count >= 4)
            {
                var id = Convert.ToInt64(eventData.Payload[0], CultureInfo.InvariantCulture);
                var dataSource = Convert.ToString(eventData.Payload[1], CultureInfo.InvariantCulture);
                var database = Convert.ToString(eventData.Payload[2], CultureInfo.InvariantCulture);
                var commandText = this.collectCommandText ? Convert.ToString(eventData.Payload[3], CultureInfo.InvariantCulture) : string.Empty;

                if (this.SqlProcessingFramework != null)
                {
                    this.SqlProcessingFramework.OnBeginExecuteCallback(id, dataSource, database, commandText);
                }
            }
        }

        /// <summary>
        /// Called when a postfix of a postfix of a SQLCommand end methods have been invoked.
        /// </summary>
        /// <param name="eventData">The event arguments that describe the event.</param>
        private void OnEndExecute(EventWrittenEventArgs eventData)
        {
            if (eventData.Payload.Count >= 3)
            {
                var id = Convert.ToInt32(eventData.Payload[0], CultureInfo.InvariantCulture);

                int compositeState = Convert.ToInt32(eventData.Payload[1], CultureInfo.InvariantCulture);

                var success = (compositeState & (int)CompositeState.Success) == (int)CompositeState.Success;
                var synchronous = (compositeState & (int)CompositeState.Synchronous) == (int)CompositeState.Synchronous;
                var isSqlException = (compositeState & (int)CompositeState.IsSqlException) == (int)CompositeState.IsSqlException;

                var sqlExceptionNumber = Convert.ToInt32(eventData.Payload[2], CultureInfo.InvariantCulture);

                if (this.SqlProcessingFramework != null)
                {
                    this.SqlProcessingFramework.OnEndExecuteCallback(id, success, sqlExceptionNumber);
                }
            }
        }
    }
}
#endif