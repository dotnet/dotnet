
namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Collections.Generic;

    internal class MetricSample
    {
        public string Name { get; set; }

        public IDictionary<string, string> Dimensions { get; set; }

        public double Value { get; set; }
    }
}
