namespace Microsoft.ApplicationInsights.TestFramework.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System.Diagnostics.Tracing;

#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Extensibility-Test")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-Test")]
#endif
    internal class TestEventSource : EventSource
    {
        [Event(1, Message = "Error: {0}", Level = EventLevel.Error)]
        public void TraceError(string message)
        {
            this.WriteEvent(1, message);
        }

        [Event(2, Message = "Verbose: {0}", Level = EventLevel.Verbose)]
        public void TraceVerbose(string message)
        {
            this.WriteEvent(2, message);
        }

        [Event(3,
            Keywords = Keywords.WebModule, 
            Message = "Verbose: {0}", 
            Level = EventLevel.Verbose)]
        public void TraceKeywords(string message)
        {
            this.WriteEvent(3, message);
        }
       
        public sealed class Keywords
        {           
            /// <summary>
            /// Key word for Web Request Module initialization failures.
            /// </summary>
            public const EventKeywords WebModule = (EventKeywords)0x10;
        }
    }
}
