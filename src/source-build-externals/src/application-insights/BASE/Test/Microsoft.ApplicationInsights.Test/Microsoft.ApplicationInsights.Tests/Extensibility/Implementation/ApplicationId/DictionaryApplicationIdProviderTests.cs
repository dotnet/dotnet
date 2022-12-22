namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DictionaryApplicationIdProviderTests
    {
        const string testInstrumentationKey1 = nameof(testInstrumentationKey1);
        const string testInstrumentationKey2 = nameof(testInstrumentationKey2);
        const string testApplicationId1 = nameof(testApplicationId1);
        const string testApplicationId2 = nameof(testApplicationId2);
        IApplicationIdProvider applicationIdProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            applicationIdProvider = new DictionaryApplicationIdProvider()
            {
                Defined = new Dictionary<string, string>
                {
                    {testInstrumentationKey1, testApplicationId1 },
                    {testInstrumentationKey2, testApplicationId2 }
                }
            };
        }

        [TestMethod]
        public void VerifyLookupsAsExcepected()
        {
            Assert.IsTrue(applicationIdProvider.TryGetApplicationId(testInstrumentationKey1, out string actual1));
            Assert.AreEqual(testApplicationId1, actual1);

            Assert.IsTrue(applicationIdProvider.TryGetApplicationId(testInstrumentationKey2, out string actual2));
            Assert.AreEqual(testApplicationId2, actual2);

            Assert.IsFalse(applicationIdProvider.TryGetApplicationId("abc", out string actual3));
            Assert.IsNull(actual3);
        }

        [TestMethod]
        public void VerifyEvaluatesNext()
        {
            var applicationIdProvider = new DictionaryApplicationIdProvider()
            {
                Next = new MockApplicationIdProvider()
            };

            Assert.IsTrue(applicationIdProvider.TryGetApplicationId(testInstrumentationKey1, out string actual));
            Assert.AreEqual(testApplicationId1, actual);
        }

        [TestMethod]
        public void VerifyReturnsNullIfNotInitialized()
        {
            var applicationIdProvider = new DictionaryApplicationIdProvider();

            Assert.IsFalse(applicationIdProvider.TryGetApplicationId(testInstrumentationKey1, out string actual));
            Assert.IsNull(actual);
        }

        private class MockApplicationIdProvider : IApplicationIdProvider
        {
            public bool TryGetApplicationId(string instrumentationKey, out string applicationId)
            {
                applicationId = testApplicationId1;
                return true;
            }
        }
    }
}
