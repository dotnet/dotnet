#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = Xunit.Assert;

    [TestClass]
    public class RetryPolicyTest
    {
        [TestMethod]
        public void RetryPolicyThrowsArgumentOutOfRangeExceptionIfRetryCountIs0()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RetryPolicy.Retry<ArgumentException, object, object>(
                arg => arg,
                "test",
                TimeSpan.FromTicks(1),
                retryCount: 0));
        }

        [TestMethod]
        public void RetryPolicyReturnsFuncResult()
        {
            var result = RetryPolicy.Retry<ArgumentException, object, object>(
                arg => arg,
                param1: "test",
                retryInterval: TimeSpan.FromTicks(1));

            Assert.Equal("test", result);
        }

        [TestMethod]
        public void RetryPolicyRetriesOnExpectedExceptionType()
        {
            int attempt = 0;
            var result = RetryPolicy.Retry<ArgumentException, object, object>(
                arg =>
                {
                    ++attempt;
                    if (attempt == 1)
                    {
                        throw new ArgumentException();
                    }

                    return attempt;
                },
                param1: 1,
                retryInterval: TimeSpan.FromTicks(1),
                retryCount: 2);

            Assert.Equal(2, result);
        }

        [TestMethod]
        public void RetryPolicyThrowsExceptionIsRetryCountReached()
        {
            Assert.Throws<ArgumentException>(() => RetryPolicy.Retry<ArgumentException, object, object>(
                arg =>
                {
                    throw new ArgumentException();
                },
                param1: 1,
                retryInterval: TimeSpan.FromTicks(1)));
        }

        [TestMethod]
        public void RetryPolicyThrowsUnexpectedException()
        {
            Assert.Throws<ApplicationException>(() => RetryPolicy.Retry<ArgumentException, object, object>(
                arg =>
                {
                    throw new ApplicationException();
                },
                param1: 1,
                retryInterval: TimeSpan.FromTicks(1)));
        }
    }
}
#endif