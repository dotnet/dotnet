namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
#if NET452
    // .NET 4.5.2 have a custom implementation of RichPayloadEventSource
#else
    /// <summary>
    /// Partial class to add the EventData attribute and any additional customizations to the generated type.
    /// </summary>
    [System.Diagnostics.Tracing.EventData(Name = "PartB_AjaxCallData")]
    internal partial class AjaxCallData
    {
    }
#endif
}
