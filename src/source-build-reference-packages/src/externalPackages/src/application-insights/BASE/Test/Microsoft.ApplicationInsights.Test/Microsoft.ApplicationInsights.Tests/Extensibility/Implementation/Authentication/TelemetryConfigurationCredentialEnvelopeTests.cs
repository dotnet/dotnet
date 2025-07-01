#if !NET452 && !NET46 && !REDFIELD
namespace Microsoft.ApplicationInsights.TestFramework.Extensibility.Implementation.Authentication
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// These tests verify that <see cref="TelemetryConfiguration"/> can receive and store an instance of <see cref="Azure.Core.TokenCredential"/>.
    /// </summary>
    /// <remarks>
    /// These tests do not run in NET452 OR NET46. 
    /// In these cases, the test runner is NET452 or NET46 and Azure.Core.TokenCredential is NOT SUPPORTED in these frameworks.
    /// This does not affect the end user because we REQUIRE the end user to create their own instance of TokenCredential.
    /// This ensures that the end user is consuming the AI SDK in one of the newer frameworks.
    /// </remarks>
    [TestClass]
    [TestCategory("AAD")]
    public class TelemetryConfigurationCredentialEnvelopeTests
    {
        /// <summary>
        /// This tests verifies that each supported language can create and set a Credential.
        /// </summary>
        [TestMethod]
        public void VerifyCanSetCredential()
        {
            var mockCredential = new MockCredential();

            var telemetryConfiguration = new TelemetryConfiguration();
            telemetryConfiguration.SetAzureTokenCredential(mockCredential);

            Assert.IsInstanceOfType(telemetryConfiguration.CredentialEnvelope, typeof(ReflectionCredentialEnvelope));
            Assert.AreEqual(mockCredential, telemetryConfiguration.CredentialEnvelope.Credential, "Credential should be the same instance that we pass in.");
        }

        /// <summary>
        /// TelemetryConfiguration accepts an <see cref="Object"/> parameter, and uses reflection to verify the type at runtime 
        /// This test is to verify that we cannot set invalid types.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VerifyCannotSetInvalidObjectOnTelemetryConfiguration()
        {
            var telemetryConfiguration = new TelemetryConfiguration();
            telemetryConfiguration.SetAzureTokenCredential(Guid.Empty);
        }

        [TestMethod]
        public void VerifySetCredential_CorrectlySetsTelemetryChannel_CredentialFirst()
        {
            // SETUP
            var tc = TelemetryConfiguration.CreateDefault();
            Assert.IsInstanceOfType(tc.TelemetryChannel, typeof(InMemoryChannel));
            Assert.IsTrue(tc.TelemetryChannel.EndpointAddress.Contains("v2")); // defaults to old api

            // ACT
            // set credential first
            tc.SetAzureTokenCredential(new MockCredential());
            Assert.IsTrue(tc.TelemetryChannel.EndpointAddress.Contains("v2.1")); // api switch

            // test new channel
            var channel = new InMemoryChannel();
            Assert.IsNull(channel.EndpointAddress); // new channel defaults null

            // change config channel
            tc.TelemetryChannel = channel;
            Assert.IsTrue(channel.EndpointAddress.Contains("v2.1")); // configuration sets new api
        }

        [TestMethod]
        public void VerifySetCredential_CorrectlySetsTelemetryChannel_TelemetryChannelFirst()
        {
            // SETUP
            var tc = TelemetryConfiguration.CreateDefault();
            Assert.IsInstanceOfType(tc.TelemetryChannel, typeof(InMemoryChannel));
            Assert.IsTrue(tc.TelemetryChannel.EndpointAddress.Contains("v2")); // defaults to old api

            // ACT
            // set new channel first
            var channel = new InMemoryChannel();
            Assert.IsNull(channel.EndpointAddress); // new channel defaults null

            // change config channel
            tc.TelemetryChannel = channel;
            Assert.IsTrue(channel.EndpointAddress.Contains("v2")); // configuration sets new api

            // set credential second
            tc.SetAzureTokenCredential(new MockCredential());
            Assert.IsTrue(tc.TelemetryChannel.EndpointAddress.Contains("v2.1")); // api switch
        }
    }
}
#endif
