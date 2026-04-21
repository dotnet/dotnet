namespace Microsoft.ApplicationInsights.TestFramework.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    public class InMemoryTraceListener : TraceListener
    {
        public InMemoryTraceListener()
        {
            this.Trace = string.Empty;
        }

        public string Trace
        {
            get;
            set;
        }

        public override void Write(string message)
        {
            this.Trace += message;
        }

        public override void WriteLine(string message)
        {
            this.Trace += message + Environment.NewLine;
        }
    }
}
