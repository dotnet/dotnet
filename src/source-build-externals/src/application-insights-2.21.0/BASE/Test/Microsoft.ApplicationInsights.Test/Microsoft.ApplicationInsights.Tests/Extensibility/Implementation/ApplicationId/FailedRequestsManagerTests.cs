namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;

    [TestClass]
    public class FailedRequestsManagerTests : ApplicationIdTestBase
    {
        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyRetryTimeout()
        {
            var stopWatch = new Stopwatch();
            var failedRequestsManager = new FailedRequestsManager(failedRequestRetryWaitTime);
            
            stopWatch.Start();
            failedRequestsManager.RegisterFetchFailure(testInstrumentationKey, new Exception());
            Assert.IsFalse(failedRequestsManager.CanRetry(testInstrumentationKey));

            while (!failedRequestsManager.CanRetry(testInstrumentationKey))
            {
                Thread.Sleep(failedRequestRetryWaitTime);
            }

            stopWatch.Stop();
            Assert.IsTrue(stopWatch.Elapsed >= failedRequestRetryWaitTime);
        }

        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyCanRetryHttp500ErrorAfterTimeout()
        {
            var stopWatch = new Stopwatch();
            var failedRequestsManager = new FailedRequestsManager(failedRequestRetryWaitTime);

            stopWatch.Start();
            failedRequestsManager.RegisterFetchFailure(testInstrumentationKey, HttpStatusCode.InternalServerError);
            Assert.IsFalse(failedRequestsManager.CanRetry(testInstrumentationKey));

            while (!failedRequestsManager.CanRetry(testInstrumentationKey))
            {
                Thread.Sleep(failedRequestRetryWaitTime);
            }

            stopWatch.Stop();
            Assert.IsTrue(stopWatch.Elapsed >= failedRequestRetryWaitTime);

            Assert.IsTrue(failedRequestsManager.CanRetry(testInstrumentationKey));
        }

        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyCanNotRetryHttp400Error()
        {
            var failedRequestsManager = new FailedRequestsManager(failedRequestRetryWaitTime);

            failedRequestsManager.RegisterFetchFailure(testInstrumentationKey, HttpStatusCode.NotFound);

            Assert.IsFalse(failedRequestsManager.CanRetry(testInstrumentationKey));

            Thread.Sleep(failedRequestRetryWaitTime + failedRequestRetryWaitTime); // wait for timeout to expire (2x timeout).

            Assert.IsFalse(failedRequestsManager.CanRetry(testInstrumentationKey));
        }
    }
}