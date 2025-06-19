namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    /// <summary>
    /// Portable tests for <see cref="UserContext"/>.
    /// </summary>
    [TestClass]
    public class UserContextTest
    {
        [TestMethod]
        public void ClassIsPublicToAllowSpecifyingCustomUserContextPropertiesInUserCode()
        {
            Assert.IsTrue(typeof(UserContext).GetTypeInfo().IsPublic);
        }
        
        [TestMethod]
        public void IdCanBeChangedByUserToSpecifyACustomValue()
        {
            var context = new UserContext();
            context.Id = "test value";
            Assert.AreEqual("test value", context.Id);
        }

        [TestMethod]
        public void UserAgentIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var context = new UserContext();
            Assert.IsNull(context.UserAgent);
        }

        [TestMethod]
        public void UserAgentCanBeChangedByUserToSpecifyACustomValue()
        {
            var context = new UserContext();
            context.UserAgent = "test value";
            Assert.AreEqual("test value", context.UserAgent);
        }

        [TestMethod]
        public void AccountIdIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var context = new UserContext();
            Assert.IsNull(context.AccountId);
        }

        [TestMethod]
        public void AccountIdCanBeChangedByUserToSpecifyACustomValue()
        {
            var context = new UserContext();
            context.AccountId = "test value";
            Assert.AreEqual("test value", context.AccountId);
        }
    }
}
