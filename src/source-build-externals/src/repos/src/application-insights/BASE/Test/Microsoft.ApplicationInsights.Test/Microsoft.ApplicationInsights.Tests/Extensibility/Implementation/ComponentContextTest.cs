namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    /// <summary>
    /// Portable tests for <see cref="ComponentContext"/>.
    /// </summary>
    [TestClass]
    public class ComponentContextTest
    {
        [TestMethod]
        public void ClassIsPublicToEnableInstantiatingItInSdkAndUserCode()
        {
            Assert.IsTrue(typeof(ComponentContext).GetTypeInfo().IsPublic);
        }
        
        [TestMethod]
        public void VersionIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var component = new ComponentContext();
            Assert.IsNull(component.Version);
        }

        [TestMethod]
        public void VersionCanBeChangedByUserToSpecifyVersionOfTheirApplication()
        {
            var component = new ComponentContext();
            component.Version = "4.2";
            Assert.AreEqual("4.2", component.Version);
        }
        
        [TestMethod]
        public void VersionSetsCorrectTagKeyAndValue()
        {
            IDictionary<string, string> tags = new Dictionary<string, string>();
            var component = new ComponentContext();

            string componentVersion = "fakeVersion";
            component.Version = componentVersion;

            component.UpdateTags(tags);
            Assert.IsTrue(tags.Contains(new KeyValuePair<string, string>(ContextTagKeys.Keys.ApplicationVersion, componentVersion)));
        }
    }
}
