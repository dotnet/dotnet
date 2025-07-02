namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    [TestClass]
    public class ProfileServiceWrapperTests : ApplicationIdTestBase
    {
        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public async Task VerifyHappyPath()
        {
            var profileServiceWrapper = GenerateMockServiceWrapper(HttpStatusCode.OK, testApplicationId);

            var actualApplicationId = await profileServiceWrapper.FetchApplicationIdAsync(testInstrumentationKey);
            Assert.AreEqual(testApplicationId, actualApplicationId);
        }

        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public async Task VerifyCanRetryHttp500ErrorAfterTimeout()
        {
            var stopWatch = new Stopwatch();
            var profileServiceWrapper = GenerateMockServiceWrapper(HttpStatusCode.InternalServerError, testApplicationId);

            stopWatch.Start();
            var fetch1 = await profileServiceWrapper.FetchApplicationIdAsync(testInstrumentationKey);
            Assert.IsNull(fetch1);
            Assert.IsFalse(profileServiceWrapper.FailedRequestsManager.CanRetry(testInstrumentationKey));

            while (!profileServiceWrapper.FailedRequestsManager.CanRetry(testInstrumentationKey))
            {
                Thread.Sleep(failedRequestRetryWaitTime);
            }

            stopWatch.Stop();
            Assert.IsTrue(stopWatch.Elapsed >= failedRequestRetryWaitTime);

            Assert.IsTrue(profileServiceWrapper.FailedRequestsManager.CanRetry(testInstrumentationKey));
        }

        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public async Task VerifyCanNotRetryHttp400Error()
        {
            var profileServiceWrapper = GenerateMockServiceWrapper(HttpStatusCode.NotFound, testApplicationId);

            var fetch1 = await profileServiceWrapper.FetchApplicationIdAsync(testInstrumentationKey);
            Assert.IsNull(fetch1);

            Assert.IsFalse(profileServiceWrapper.FailedRequestsManager.CanRetry(testInstrumentationKey));

            Thread.Sleep(failedRequestRetryWaitTime + failedRequestRetryWaitTime); // wait for timeout to expire (2x timeout).

            Assert.IsFalse(profileServiceWrapper.FailedRequestsManager.CanRetry(testInstrumentationKey));
        }
    }
}
