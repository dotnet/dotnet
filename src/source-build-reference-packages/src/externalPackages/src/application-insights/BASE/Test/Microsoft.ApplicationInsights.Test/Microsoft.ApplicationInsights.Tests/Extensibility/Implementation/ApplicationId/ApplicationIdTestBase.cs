namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using Moq;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ApplicationIdTestBase
    {
        public const int testTimeoutMilliseconds = 20000; // 20 seconds
        public const int taskWaitMilliseconds = 50;
        private const int failedRequestRetryWaitTimeMilliseconds = 100;

        public const string testInstrumentationKey = nameof(testInstrumentationKey);
        public const string testApplicationId = nameof(testApplicationId);
        public readonly string testFormattedApplicationId = ApplicationIdHelper.ApplyFormatting(testApplicationId);

        public readonly TimeSpan failedRequestRetryWaitTime;

        public ApplicationIdTestBase()
        {
            this.failedRequestRetryWaitTime = TimeSpan.FromMilliseconds(failedRequestRetryWaitTimeMilliseconds);
        }

        internal ProfileServiceWrapper GenerateMockServiceWrapper(HttpStatusCode httpStatus, string testApplicationId = null)
        {
            var mock = new Mock<ProfileServiceWrapper>(failedRequestRetryWaitTime);
            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(() =>
                {
                    return Task.FromResult(new HttpResponseMessage(httpStatus)
                    {
                        Content = new StringContent(testApplicationId)
                    });
                });
            return mock.Object;
        }

        internal ProfileServiceWrapper GenerateMockServiceWrapper(Func<Task<HttpResponseMessage>> overrideGetAsync)
        {
            var mock = new Mock<ProfileServiceWrapper>(failedRequestRetryWaitTime);
            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(overrideGetAsync);
            return mock.Object;
        }

        /// <summary>
        /// This bool is external to the method so it will save state between runs.
        /// </summary>
        internal bool mockMethodFailOnceStateBool;
        internal Task<HttpResponseMessage> MockMethodFailOnce()
        {
            // Simulate a retry scenario: On first run fail, on second run pass. 
            Console.WriteLine($"will method succeed: {mockMethodFailOnceStateBool}");
            if (mockMethodFailOnceStateBool)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(testApplicationId)
                });
            }
            else
            {
                mockMethodFailOnceStateBool = true;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }
    }
}
