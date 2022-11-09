namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.WindowsServer.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation.DataContracts;
    using Microsoft.ApplicationInsights.WindowsServer.Mock;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;
    using Moq.Protected;

    using Assert = Xunit.Assert;

    /// <summary>
    /// Tests the heartbeat functionality through actual local-only Http calls to mimic
    /// end to end functionality as closely as possible.
    /// </summary>
    [TestClass]
    public class AzureInstanceMetadataEndToEndTests
    {
        [TestMethod]
        public async Task SpoofedResponseFromAzureIMSDoesntCrash()
        {
            // SETUP
            var testMetadata = GetTestMetadata();
            Mock<HttpMessageHandler> mockHttpMessageHandler = GetMockHttpMessageHandler(testMetadata);
            //var azureIms = new AzureMetadataRequestor(new HttpClient(mockHttpMessageHandler.Object));
            var azureIms = GetTestableAzureMetadataRequestor(mockHttpMessageHandler.Object);

            // ACT
            var azureImsProps = new AzureComputeMetadataHeartbeatPropertyProvider();
            var azureIMSData = await azureIms.GetAzureComputeMetadataAsync();

            // VERIFY
            foreach (string fieldName in azureImsProps.ExpectedAzureImsFields)
            {
                string fieldValue = azureIMSData.GetValueForField(fieldName);
                Assert.NotNull(fieldValue);
                Assert.Equal(fieldValue, testMetadata.GetValueForField(fieldName));
            }
        }

        [TestMethod]
        public async Task AzureImsResponseExcludesMalformedValues()
        {
            // SETUP
            var testMetadata = GetTestMetadata();
            // make it a malicious-ish response...
            testMetadata.Name = "Not allowed for VM names";
            testMetadata.ResourceGroupName = "Not allowed for resource group name";
            testMetadata.SubscriptionId = "Definitely-not-a GUID up here";

            Mock<HttpMessageHandler> mockHttpMessageHandler = GetMockHttpMessageHandler(testMetadata);
            //var azureIms = new AzureMetadataRequestor(new HttpClient(mockHttpMessageHandler.Object));
            var azureIms = GetTestableAzureMetadataRequestor(mockHttpMessageHandler.Object);

            // ACT
            var azureImsProps = new AzureComputeMetadataHeartbeatPropertyProvider(azureIms);
            var hbeatProvider = new HeartbeatProviderMock();
            var result = await azureImsProps.SetDefaultPayloadAsync(hbeatProvider);

            // VERIFY
            Assert.True(result);
            Assert.Empty(hbeatProvider.HbeatProps["azInst_name"]);
            Assert.Empty(hbeatProvider.HbeatProps["azInst_resourceGroupName"]);
            Assert.Empty(hbeatProvider.HbeatProps["azInst_subscriptionId"]);
        }

        [TestMethod]
        public async Task AzureImsResponseHandlesException()
        {
            // SETUP
            var testMetadata = GetTestMetadata();
            var mockHttpMessageHandler = GetMockHttpMessageHandler(testMetadata, throwException: true);
            //var azureIms = new AzureMetadataRequestor(new HttpClient(mockHttpMessageHandler.Object));
            var azureIms = GetTestableAzureMetadataRequestor(mockHttpMessageHandler.Object);

            // ACT
            var result = await azureIms.GetAzureComputeMetadataAsync();

            // VERIFY
            Assert.Null(result);
        }

        [TestMethod]
        public async Task AzureImsResponseUnsuccessful()
        {
            // SETUP
            var testMetadata = GetTestMetadata();
            var mockHttpMessageHandler = GetMockHttpMessageHandler(testMetadata, HttpStatusCode.Forbidden);
            //var azureIms = new AzureMetadataRequestor(new HttpClient(mockHttpMessageHandler.Object));
            var azureIms = GetTestableAzureMetadataRequestor(mockHttpMessageHandler.Object);

            // ACT
            var azureIMSData = await azureIms.GetAzureComputeMetadataAsync();

            // VERIFY
            Assert.Null(azureIMSData);
        }

        /// <summary>
        /// Creates test data for heartbeat e2e.
        /// </summary>
        /// <returns>An Azure Instance Metadata Compute object suitable for use in testing.</returns>
        private static AzureInstanceComputeMetadata GetTestMetadata() => new AzureInstanceComputeMetadata()
        {
            Location = "US-West",
            Name = "test-vm01",
            Offer = "D9_USWest",
            OsType = "Linux",
            PlacementGroupId = "placement-grp",
            PlatformFaultDomain = "0",
            PlatformUpdateDomain = "0",
            Publisher = "Microsoft",
            ResourceGroupName = "test.resource-group_01",
            Sku = "Windows_10",
            SubscriptionId = Guid.NewGuid().ToString(),
            Tags = "thisTag;thatTag",
            Version = "10.8a",
            VmId = Guid.NewGuid().ToString(),
            VmSize = "A8",
            VmScaleSetName = "ScaleName"
        };

        private static string SerializeAsJsonString(AzureInstanceComputeMetadata azureInstanceComputeMetadata)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AzureInstanceComputeMetadata));

            string returnData;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, azureInstanceComputeMetadata);
                memoryStream.Position = 0;
                StreamReader sr = new StreamReader(memoryStream);

                returnData = sr.ReadToEnd();

                sr.Close();
                memoryStream.Close();
            }

            return returnData;
        }

        private static Mock<HttpMessageHandler> GetMockHttpMessageHandler(AzureInstanceComputeMetadata metadata, HttpStatusCode httpStatusCode = HttpStatusCode.OK, bool throwException = false)
        {
            var json = SerializeAsJsonString(metadata);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
            };

            if (throwException)
            {
                mockHttpMessageHandler
                   .Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .Callback(() => throw new Exception("unit test forced exception"));
            }
            else
            {
                mockHttpMessageHandler
                   .Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(value: response);
            }

            return mockHttpMessageHandler;
        }

        private static AzureMetadataRequestor GetTestableAzureMetadataRequestor(HttpMessageHandler httpMessageHandler)
        {
            var mockAzureMetadataRequestor = new Mock<AzureMetadataRequestor>();

            mockAzureMetadataRequestor
                .Setup(x => x.GetHttpClient())
                .Returns(new HttpClient(httpMessageHandler));

            return mockAzureMetadataRequestor.Object;
        }
    }
}
