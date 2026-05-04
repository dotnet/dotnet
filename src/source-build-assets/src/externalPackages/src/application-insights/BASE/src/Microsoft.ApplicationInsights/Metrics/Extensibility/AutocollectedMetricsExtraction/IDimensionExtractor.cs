namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using Microsoft.ApplicationInsights.Channel;    

    internal interface IDimensionExtractor
    {
        int MaxValues { get; set; }

        string DefaultValue { get; set; }

        string Name { get; set; }

        string ExtractDimension(ITelemetry item);
    }
}
