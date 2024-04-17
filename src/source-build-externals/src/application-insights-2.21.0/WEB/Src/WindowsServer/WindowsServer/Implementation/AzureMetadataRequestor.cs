namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation.DataContracts;

    internal class AzureMetadataRequestor : IAzureMetadataRequestor
    {
        /// <summary>
        /// Azure Instance Metadata Service exists on a single non-routable IP on machines configured
        /// by the Azure Resource Manager. https://go.microsoft.com/fwlink/?linkid=864683 .
        /// </summary>
        internal const string AzureImsApiVersion = "api-version=2017-12-01"; // this version has the format=text capability
        internal const string AzureImsJsonFormat = "format=json";
        internal const int AzureImsMaxResponseBufferSize = 512;

        /// <summary>
        /// Default timeout for the web requests made to obtain Azure IMS data. Internal to expose to tests.
        /// </summary>
        internal TimeSpan AzureImsRequestTimeout = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Gets or sets the base URI for the Azure Instance Metadata service. Internal to allow overriding in test.
        /// </summary>
        /// <remarks>
        /// At this time, this service does not support https. We should monitor their website for more information. https://docs.microsoft.com/azure/virtual-machines/windows/instance-metadata-service .
        /// </remarks>
        internal string BaseAimsUri { get; set; } = "http://169.254.169.254/metadata/instance/compute";

        public Task<AzureInstanceComputeMetadata> GetAzureComputeMetadataAsync()
        {
            string metadataRequestUrl = string.Format(
                CultureInfo.InvariantCulture,
                "{0}?{1}&{2}",
                this.BaseAimsUri,
                AzureMetadataRequestor.AzureImsJsonFormat,
                AzureMetadataRequestor.AzureImsApiVersion);

            return this.MakeAzureMetadataRequestAsync(metadataRequestUrl);
        }

        /// <summary>
        /// Provides a new HttpClient. This method can be mocked in unit tests.
        /// </summary>
        internal virtual HttpClient GetHttpClient() => new HttpClient();

        private async Task<AzureInstanceComputeMetadata> MakeAzureMetadataRequestAsync(string metadataRequestUrl)
        {
            AzureInstanceComputeMetadata requestResult = null;

            SdkInternalOperationsMonitor.Enter();
            try
            {
                requestResult = await this.MakeWebRequestAsync(metadataRequestUrl).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                WindowsServerEventSource.Log.AzureInstanceMetadataRequestFailure(
                    metadataRequestUrl, ex.Message, ex.InnerException != null ? ex.InnerException.Message : string.Empty);
            }
            finally
            {
                SdkInternalOperationsMonitor.Exit();
            }

            return requestResult;
        }

        private async Task<AzureInstanceComputeMetadata> MakeWebRequestAsync(string requestUrl)
        {
            AzureInstanceComputeMetadata azureIms = null;
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(AzureInstanceComputeMetadata));

            using (var azureImsClient = this.GetHttpClient())
            {
                azureImsClient.MaxResponseContentBufferSize = AzureMetadataRequestor.AzureImsMaxResponseBufferSize;
                azureImsClient.DefaultRequestHeaders.Add("Metadata", "True");
                azureImsClient.Timeout = this.AzureImsRequestTimeout;

                Stream content = await azureImsClient.GetStreamAsync(new Uri(requestUrl)).ConfigureAwait(false);
                azureIms = (AzureInstanceComputeMetadata)deserializer.ReadObject(content);
                content.Dispose();

                if (azureIms == null)
                {
                    WindowsServerEventSource.Log.CannotObtainAzureInstanceMetadata();
                }
            }

            return azureIms;
        }
    }
}
