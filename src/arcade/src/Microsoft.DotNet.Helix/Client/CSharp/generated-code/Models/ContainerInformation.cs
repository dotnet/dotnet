using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class ContainerInformation
    {
        public ContainerInformation(DateTimeOffset created, DateTimeOffset expiration, string creator, string containerName, string storageAccountName, Guid subscriptionId, string region)
        {
            Created = created;
            Expiration = expiration;
            Creator = creator;
            ContainerName = containerName;
            StorageAccountName = storageAccountName;
            SubscriptionId = subscriptionId;
            Region = region;
        }

        [JsonProperty("Created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("Expiration")]
        public DateTimeOffset Expiration { get; set; }

        [JsonProperty("ReadToken")]
        public string ReadToken { get; set; }

        [JsonProperty("WriteToken")]
        public string WriteToken { get; set; }

        [JsonProperty("Creator")]
        public string Creator { get; set; }

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
                if (string.IsNullOrEmpty(Creator))
                {
                    return false;
                }
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
