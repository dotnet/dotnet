namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation.DataContracts;

    internal interface IAzureMetadataRequestor
    {
        Task<AzureInstanceComputeMetadata> GetAzureComputeMetadataAsync();
    }
}
