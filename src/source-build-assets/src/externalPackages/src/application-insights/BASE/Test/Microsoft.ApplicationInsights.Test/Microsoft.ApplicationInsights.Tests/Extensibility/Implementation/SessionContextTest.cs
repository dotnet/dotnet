namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class SessionContextTest
    {
        [TestMethod]
        public void ClassIsPublicToAllowInstantiationInSdkAndSessionCode()
        {
            Assert.IsTrue(typeof(SessionContext).GetTypeInfo().IsPublic);
        }
        
        [TestMethod]
        public void IdIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var session = new SessionContext();
            Assert.IsNull(session.Id);
        }

        [TestMethod]
        public void IdCanBeChangedByUserToSupplyApplicationDefinedValue()
        {
            var session = new SessionContext();
            session.Id = "42";
            Assert.AreEqual("42", session.Id);
        }
        
        [TestMethod]
        public void IsFirstIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var session = new SessionContext();
            Assert.IsNull(session.IsFirst);
        }

        [TestMethod]
        public void IsFirstCanBeSetByUserToSupplyCustomValue()
        {
            var session = new SessionContext();
            session.IsFirst = true;
            Assert.AreEqual(true, session.IsFirst);
        }

        [TestMethod]
        public void IsFirstCanBeSetToNullToRemoveItFromJsonPayload()
        {
            var session = new SessionContext() { IsFirst = true };
            session.IsFirst = null;
            Assert.IsNull(session.IsFirst);
        }
    }
}
