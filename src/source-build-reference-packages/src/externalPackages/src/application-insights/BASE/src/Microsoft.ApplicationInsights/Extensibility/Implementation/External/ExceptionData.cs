namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Partial class to add the EventData attribute and any additional customizations to the generated type.
    /// </summary>
#if !NET452
    // .NET 4.5.2 have a custom implementation of RichPayloadEventSource
    [System.Diagnostics.Tracing.EventData(Name = "PartB_ExceptionData")]
#endif
    internal partial class ExceptionData
    {
        public ExceptionData DeepClone()
        {
            var other = new ExceptionData();
            other.ver = this.ver;
            other.severityLevel = this.severityLevel;
            other.problemId = this.problemId;
            Debug.Assert(other.properties != null, "The constructor should have allocated properties dictionary");
            Debug.Assert(other.measurements != null, "The constructor should have allocated the measurements dictionary");
            Utils.CopyDictionary(this.properties, other.properties);
            Utils.CopyDictionary(this.measurements, other.measurements);

            Debug.Assert(other.exceptions != null, "The constructor should have allocated properties dictionary");
            foreach (var e in this.exceptions)
            {
                other.exceptions.Add(e);
            }

            return other;
        }
    }
}
