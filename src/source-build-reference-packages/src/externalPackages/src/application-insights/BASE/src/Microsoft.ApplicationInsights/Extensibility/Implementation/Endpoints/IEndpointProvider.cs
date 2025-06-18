namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    using System;

    internal interface IEndpointProvider
    {
        string ConnectionString { get; set; }

        Uri GetEndpoint(EndpointName endpointName);

        string GetInstrumentationKey();
    }
}
