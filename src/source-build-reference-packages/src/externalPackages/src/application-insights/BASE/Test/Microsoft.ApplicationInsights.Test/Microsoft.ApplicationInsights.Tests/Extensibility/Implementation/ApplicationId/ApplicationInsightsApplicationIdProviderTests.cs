namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    [TestClass]
    public class ApplicationInsightsApplicationIdProviderTests : ApplicationIdTestBase
    {
        /// <summary>
        /// Lookup is expected to fail on first call, this is how it invokes the Http request.
        /// </summary>
        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyFailsOnFirstRequest()
        {
            var mockProfileServiceWrapper = GenerateMockServiceWrapper(HttpStatusCode.OK, testApplicationId);
            var aiApplicationIdProvider = new ApplicationInsightsApplicationIdProvider(mockProfileServiceWrapper);

            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore));
        }

        /// <summary>
        /// Lookup is expected to succeed on the second call, after the Http request has completed.
        /// </summary>
        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifySucceedsOnSecondRequest()
        {
            var mockProfileServiceWrapper = GenerateMockServiceWrapper(HttpStatusCode.OK, testApplicationId);
            var aiApplicationIdProvider = new ApplicationInsightsApplicationIdProvider(mockProfileServiceWrapper);

            // first request expected to fail
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore));

            // wait for async tasks to complete
            while (aiApplicationIdProvider.IsFetchAppInProgress(testInstrumentationKey))
            {
                Console.WriteLine("wait");
                Thread.Sleep(taskWaitMilliseconds);
            }

            Assert.IsTrue(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string actual));
            Assert.AreEqual(testFormattedApplicationId, actual);
        }

        /// <summary>
        /// Protect against injection attacks. Test that if an malicious value is returned, that value will be truncated.
        /// </summary>
        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyMaliciousApplicationIdIsTruncated() 
        {
            // 50 character string.
            var testApplicationId = "a123456789b123546789c123456789d123456798e123456789";

            // An arbitrary string that is expected to be truncated.
            var malicious = "00000000000000000000000000000000000000000000000000000000000";

            var testFormattedApplicationId = ApplicationIdHelper.ApplyFormatting(testApplicationId);

            var mockProfileServiceWrapper = GenerateMockServiceWrapper(HttpStatusCode.OK, testApplicationId + malicious);
            var aiApplicationIdProvider = new ApplicationInsightsApplicationIdProvider(mockProfileServiceWrapper);

            // first request expected to fail
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore));

            // wait for async tasks to complete
            while (aiApplicationIdProvider.IsFetchAppInProgress(testInstrumentationKey)) 
            {
                Console.WriteLine("wait");
                Thread.Sleep(taskWaitMilliseconds);
            }

            Assert.IsTrue(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string actual));
            Assert.AreEqual(testFormattedApplicationId, actual);
        }

        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyWhenTaskInProgressNoNewTaskCreated()
        {
            var mockProfileServiceWrapper = GenerateMockServiceWrapper(() =>
            {
                Console.WriteLine($"sleep start: {DateTime.UtcNow}");
                Thread.Sleep(60000); // intentional long pause,  inspect tasks
                Console.WriteLine($"sleep stop: {DateTime.UtcNow}");
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(testApplicationId)
                });
            });
            var aiApplicationIdProvider = new ApplicationInsightsApplicationIdProvider(mockProfileServiceWrapper);

            Console.WriteLine($"first request: {DateTime.UtcNow}");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore1));
            Console.WriteLine($"second request: {DateTime.UtcNow}");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore2));

            Assert.AreEqual(1, aiApplicationIdProvider.FetchTasks.Count);
        }

        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyWhenRequestFailsWillWaitBeforeRetry() 
        {
            mockMethodFailOnceStateBool = false;
            var mockProfileServiceWrapper = GenerateMockServiceWrapper(this.MockMethodFailOnce);
            var aiApplicationIdProvider = new ApplicationInsightsApplicationIdProvider(mockProfileServiceWrapper);
            var stopWatch = new Stopwatch();

            Console.WriteLine($"first request: {DateTime.UtcNow.ToString("HH:mm:ss:fffff")}");
            stopWatch.Start();
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore1)); // first request will fail, will create internal failure-timeout 

            // wait for async tasks to complete
            while (aiApplicationIdProvider.IsFetchAppInProgress(testInstrumentationKey))
            {
                Console.WriteLine("wait for task");
                Thread.Sleep(taskWaitMilliseconds);
            }

            Console.WriteLine($"\nsecond request: {DateTime.UtcNow.ToString("HH:mm:ss:fffff")}");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore2)); // first retry should fail because timeout (too soon)

            while(!mockProfileServiceWrapper.FailedRequestsManager.CanRetry(testInstrumentationKey))
            {
                Console.WriteLine("wait for retry");
                Thread.Sleep(taskWaitMilliseconds);
            }
            stopWatch.Stop();
            Assert.IsTrue(stopWatch.Elapsed >= failedRequestRetryWaitTime, "too fast, did not wait timeout");

            Console.WriteLine($"\nthird request: {DateTime.UtcNow.ToString("HH:mm:ss:fffff")}");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore3)); // second retry should fail (because no matching value), but will create new request task

            // wait for async tasks to complete
            while (aiApplicationIdProvider.IsFetchAppInProgress(testInstrumentationKey))
            {
                Console.WriteLine("wait for task");
                Thread.Sleep(taskWaitMilliseconds);
            }

            Console.WriteLine($"\nfourth request: {DateTime.UtcNow.ToString("HH:mm:ss:fffff")}");
            Assert.IsTrue(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string actual)); // third retry should resolve

            Assert.AreEqual(testFormattedApplicationId, actual);
        }

        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyWhenRequestHardFailsWillNotRetry() 
        {
            var mockProfileServiceWrapper = GenerateMockServiceWrapper(HttpStatusCode.NotFound);
            var aiApplicationIdProvider = new ApplicationInsightsApplicationIdProvider(mockProfileServiceWrapper);

            Console.WriteLine("first request");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore1));

            // wait for async tasks to complete
            while (aiApplicationIdProvider.IsFetchAppInProgress(testInstrumentationKey))
            {
                Console.WriteLine("wait");
                Thread.Sleep(taskWaitMilliseconds);
            }

            Console.WriteLine("second request");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore2)); // retry should fail, fatal error
            Thread.Sleep(failedRequestRetryWaitTime + failedRequestRetryWaitTime); // wait for timeout to expire (2x timeout).

            Console.WriteLine("third request");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore3)); // retry should still fail, fatal error
        }
    }
}

