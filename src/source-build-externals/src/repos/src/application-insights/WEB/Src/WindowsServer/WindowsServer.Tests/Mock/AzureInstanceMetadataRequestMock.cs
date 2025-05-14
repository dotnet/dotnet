namespace Microsoft.ApplicationInsights.WindowsServer.Mock
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation.DataContracts;

    internal class AzureInstanceMetadataRequestMock : IAzureMetadataRequestor
    {
        public AzureInstanceComputeMetadata ComputeMetadata;

        private Func<AzureInstanceComputeMetadata> getComputeMetadata = null;

        public AzureInstanceMetadataRequestMock(Func<AzureInstanceComputeMetadata> getComputeMetadata = null)
        {
            this.getComputeMetadata = getComputeMetadata;
            if (getComputeMetadata == null)
            {
                this.getComputeMetadata = () => this.ComputeMetadata;
            }

            this.ComputeMetadata = new AzureInstanceComputeMetadata()
            {
                Location = "Here, now",
                Name = "vm-testRg-num1",
                Offer = "OneYouCannotPassUp",
                OsType = "Windows",
                PlacementGroupId = "plat-grp-id",
                PlatformFaultDomain = "0",
                PlatformUpdateDomain = "0",
                Publisher = "Microsoft-Vancouver",
                ResourceGroupName = "testRg",
                Sku = "OSVm01",
                SubscriptionId = Guid.NewGuid().ToString(),
                Tags = "thisTag;thatTag",
                Version = "0.0.0",
                VmId = Guid.NewGuid().ToString(),
                VmSize = "A01",
                VmScaleSetName = "ScaleName"
            };
        }
        
        public Task<AzureInstanceComputeMetadata> GetAzureComputeMetadataAsync()
        {
            return Task.FromResult(this.getComputeMetadata());
        }
    }
}
