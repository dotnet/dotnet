namespace Microsoft.ApplicationInsights.WindowsServer.Implementation.DataContracts
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class representing the returned structure from an Azure Instance Metadata request
    /// for Compute information.
    /// </summary>
    [DataContract]
    internal class AzureInstanceComputeMetadata
    {
        private const int ResourceGroupNameLengthMax = 90;
        private const int ResourceGroupNameLengthMin = 1;
        private const string ResourceGroupNameValidChars = @"^[a-zA-Z0-9\.\-_]+$";
        private const int NameLenghtMax = 64; // 15 for windows, go with Linux for MAX
        private const int NameLengthMin = 1;
        private const string NameValidChars = @"^[a-zA-Z0-9()_\-]+$";
        private const int RegexTimeoutMs = 1000;

        [DataMember(Name = "location", IsRequired = true)]
        internal string Location { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        internal string Name { get; set; }

        [DataMember(Name = "offer", IsRequired = true)]
        internal string Offer { get; set; }

        [DataMember(Name = "osType", IsRequired = true)]
        internal string OsType { get; set; }

        [DataMember(Name = "placementGroupId", IsRequired = true)]
        internal string PlacementGroupId { get; set; }

        [DataMember(Name = "platformFaultDomain", IsRequired = true)]
        internal string PlatformFaultDomain { get; set; }

        [DataMember(Name = "platformUpdateDomain", IsRequired = true)]
        internal string PlatformUpdateDomain { get; set; }

        [DataMember(Name = "publisher", IsRequired = true)]
        internal string Publisher { get; set; }

        [DataMember(Name = "resourceGroupName", IsRequired = true)]
        internal string ResourceGroupName { get; set; }

        [DataMember(Name = "sku", IsRequired = true)]
        internal string Sku { get; set; }

        [DataMember(Name = "subscriptionId", IsRequired = true)]
        internal string SubscriptionId { get; set; }

        [DataMember(Name = "tags", IsRequired = false)]
        internal string Tags { get; set; }

        [DataMember(Name = "version", IsRequired = true)]
        internal string Version { get; set; }

        [DataMember(Name = "vmId", IsRequired = true)]
        internal string VmId { get; set; }

        [DataMember(Name = "vmSize", IsRequired = true)]
        internal string VmSize { get; set; }

        [DataMember(Name = "vmScaleSetName", IsRequired = true)]
        internal string VmScaleSetName { get; set; }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "This compares string to known values. Is safe to use lowercase.")]
        internal string GetValueForField(string fieldName)
        {
            string aimsValue = null;
            switch (fieldName.ToLowerInvariant())
            {
                case "ostype":
                    aimsValue = this.OsType;
                    break;
                case "location":
                    aimsValue = this.Location;
                    break;
                case "name":
                    aimsValue = this.Name;
                    break;
                case "offer":
                    aimsValue = this.Offer;
                    break;
                case "placementgroupid":
                    aimsValue = this.PlacementGroupId;
                    break;
                case "platformfaultdomain":
                    aimsValue = this.PlatformFaultDomain;
                    break;
                case "platformupdatedomain":
                    aimsValue = this.PlatformUpdateDomain;
                    break;
                case "publisher":
                    aimsValue = this.Publisher;
                    break;
                case "sku":
                    aimsValue = this.Sku;
                    break;
                case "version":
                    aimsValue = this.Version;
                    break;
                case "vmid":
                    aimsValue = this.VmId;
                    break;
                case "vmsize":
                    aimsValue = this.VmSize;
                    break;
                case "subscriptionid":
                    aimsValue = this.SubscriptionId;
                    break;
                case "resourcegroupname":
                    aimsValue = this.ResourceGroupName;
                    break;
                case "tags":
                    aimsValue = this.Tags;
                    break;
                case "vmscalesetname":
                    aimsValue = this.VmScaleSetName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format(CultureInfo.InvariantCulture, "No field named '{0}' in AzureInstanceComputeMetadata.", fieldName));
            }

            if (aimsValue == null)
            {
                aimsValue = string.Empty;
            }

            return aimsValue;
        }

        /// <summary>
        /// Because the Azure IMS is on a non-routable IP we need to do some due diligence in our accepting
        /// values returned from it. This method takes the fieldname and value received for that field, and
        /// if we can test that value against known limitations of that field we do so here. If the test fails
        /// we return the empty string, otherwise we return the string given.
        /// </summary>
        /// <param name="fieldName">Name of the field acquired from the call to Azure IMS.</param>
        /// <returns>The value of the field, verified, or the empty string.</returns>
        internal string VerifyExpectedValue(string fieldName)
        {
            string valueToVerify = this.GetValueForField(fieldName);

            string value = string.Empty;
            bool valueOk = true;
            TimeSpan regexTimeout = TimeSpan.FromMilliseconds(AzureInstanceComputeMetadata.RegexTimeoutMs);

            if (fieldName.Equals("resourceGroupName", StringComparison.OrdinalIgnoreCase))
            {
                var resGrpMatcher = new Regex(AzureInstanceComputeMetadata.ResourceGroupNameValidChars, RegexOptions.None, regexTimeout);
                valueOk = valueToVerify.Length <= AzureInstanceComputeMetadata.ResourceGroupNameLengthMax
                    && valueToVerify.Length >= AzureInstanceComputeMetadata.ResourceGroupNameLengthMin
                    && resGrpMatcher.IsMatch(valueToVerify)
                    && !valueToVerify.EndsWith(".", StringComparison.OrdinalIgnoreCase);

                if (valueOk)
                {
                    value = valueToVerify;
                }
            }
            else if (fieldName.Equals("subscriptionId", StringComparison.OrdinalIgnoreCase))
            {
                Guid g = new Guid();
                valueOk = Guid.TryParse(valueToVerify, out g);

                if (valueOk)
                {
                    value = valueToVerify;
                }
            }
            else if (fieldName.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                var nameMatcher = new Regex(AzureInstanceComputeMetadata.NameValidChars, RegexOptions.None, regexTimeout);
                valueOk = valueToVerify.Length <= AzureInstanceComputeMetadata.NameLenghtMax
                    && valueToVerify.Length >= AzureInstanceComputeMetadata.NameLengthMin
                    && nameMatcher.IsMatch(valueToVerify);

                if (valueOk)
                {
                    value = valueToVerify;
                }
            }
            else
            {
                // no sanitization method available for this value, just return the given value
                value = valueToVerify;
            }

            if (!valueOk)
            {
                WindowsServerEventSource.Log.AzureInstanceMetadataValueForFieldInvalid(fieldName);
            }

            return value;
        }
    }
}
