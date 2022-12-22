using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class ContainerExtensionRequest
    {
        public ContainerExtensionRequest(double extensionInDays, string containerName, string storageAccountName, Guid subscriptionId, string region)
        {
            ExtensionInDays = extensionInDays;
            ContainerName = containerName;
            StorageAccountName = storageAccountName;
            SubscriptionId = subscriptionId;
            Region = region;
        }

        [JsonProperty("ExtensionInDays")]
        public double ExtensionInDays { get; set; }

        [JsonProperty("ContainerName")]
        public string ContainerName { get; set; }

        [JsonProperty("StorageAccountName")]
        public string StorageAccountName { get; set; }

        [JsonProperty("SubscriptionId")]
        public Guid SubscriptionId { get; set; }

        [JsonProperty("Region")]
        public string Region { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(ContainerName))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(StorageAccountName))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Region))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
