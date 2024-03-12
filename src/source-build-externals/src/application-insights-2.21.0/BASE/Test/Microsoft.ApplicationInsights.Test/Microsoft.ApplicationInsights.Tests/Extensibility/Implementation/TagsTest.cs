namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class TagsTest
    {
        [TestMethod]
        public void GetTagValueOrNullReturnsCorrectStringValue()
        {
            var tags = new Dictionary<string, string>();

            string testKey = "testKey";
            string testValue = "testValue";

            tags[testKey] = testValue;

            string tagValue = tags.GetTagValueOrNull(testKey);

            Assert.AreEqual(testValue, tagValue);
        }

        [TestMethod]
        public void GetTagValueOrNullReturnsEmpty()
        {
            var tags = new Dictionary<string, string>();

            string tagValue = tags.GetTagValueOrNull("testKey");

            Assert.IsNull(tagValue);
        }

        [TestMethod]
        public void SetStringValueOrRemoveSetsCorrectStringValue()
        {
            var tags = new Dictionary<string, string>();

            string testKey = "testKey";
            string testValue = "testValue";

            tags.SetStringValueOrRemove(testKey, testValue);

            Assert.AreEqual(testValue, tags[testKey]);
        }

        [TestMethod]
        public void SetStringValueOrRemoveDoesNotThrowsWhenValueIsNull()
        {
            var tags = new Dictionary<string, string>();

            //Assert.DoesNotThrow
            tags.SetStringValueOrRemove("testKey", null);
        }

        [TestMethod]
        public void SetTagValueOrRemoveRemovesValue()
        {
            var tags = new Dictionary<string, string>();

            string testKey = "testKey";
            string testValue = "testValue";

            tags[testKey] = testValue;

            tags.SetTagValueOrRemove<bool?>(testKey, null);

            Assert.IsFalse(tags.ContainsKey(testKey));
        }

        [TestMethod]
        public void SetTagValueOrRemoveSetsCorrectBoolValue()
        {
            var tags = new Dictionary<string, string>();

            string testKey = "testKey";
            string testValue = "True";

            tags.SetTagValueOrRemove<bool?>(testKey, true);

            Assert.AreEqual(testValue, tags[testKey]);
        }

        [TestMethod]
        public void SetTagValueOrRemoveSetsCorrectIntValue()
        {
            var tags = new Dictionary<string, string>();

            string testKey = "testKey";
            string testValue = "5";

            tags.SetTagValueOrRemove<int?>(testKey, 5);

            Assert.AreEqual(testValue, tags[testKey]);
        }

        [TestMethod]
        public void SetTagValueOrRemoveSetsCorrectStringValue()
        {
            var tags = new Dictionary<string, string>();

            string testKey = "testKey";
            string testValue = "testValue";

            tags.SetTagValueOrRemove<string>(testKey, testValue);

            Assert.AreEqual(testValue, tags[testKey]);
        }        
    }
}
