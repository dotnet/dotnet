namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class OperationContextTest
    {
        [TestMethod]
        public void ClassIsPublicToAllowInstantiationInSdkAndUserCode()
        {
            Assert.IsTrue(typeof(OperationContext).GetTypeInfo().IsPublic);
        }
        
        [TestMethod]
        public void IdIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var operation = new OperationContext();
            Assert.IsNull(operation.Id);
        }

        [TestMethod]
        public void NameIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var operation = new OperationContext();
            Assert.IsNull(operation.Name);
        }

        [TestMethod]
        public void SyntheticSourceIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var operation = new OperationContext();
            Assert.IsNull(operation.SyntheticSource);
        }

        [TestMethod]
        public void ParentIdIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var operation = new OperationContext();
            Assert.IsNull(operation.ParentId);
        }

        [TestMethod]
        public void RootIdIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var operation = new OperationContext();
            Assert.IsNull(operation.Id);
        }

        [TestMethod]
        public void CorrelationVectorIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var operation = new OperationContext();
            Assert.IsNull(operation.CorrelationVector);
        }


        [TestMethod]
        public void IdCanBeChangedByUserToSupplyApplicationDefinedValue()
        {
            var operation = new OperationContext();
            operation.Id = "42";
            Assert.AreEqual("42", operation.Id);
        }

        [TestMethod]
        public void NameCanBeChangedByUserToSupplyApplicationDefinedValue()
        {
            var operation = new OperationContext();
            operation.Name = "SampleOperationName";
            Assert.AreEqual("SampleOperationName", operation.Name);
        }

        [TestMethod]
        public void SyntheticSourceCanBeChangedByUserToSupplyApplicationDefinedValue()
        {
            var operation = new OperationContext();
            operation.SyntheticSource = "Sample";
            Assert.AreEqual("Sample", operation.SyntheticSource);
        }

        [TestMethod]
        public void ParentIdCanBeChangedByUserToSupplyApplicationDefinedValue()
        {
            var operation = new OperationContext();
            operation.ParentId = "ParentId";
            Assert.AreEqual("ParentId", operation.ParentId);
        }

        [TestMethod]
        public void CorrelationVectorCanBeChangedByUserToSupplyApplicationDefinedValue()
        {
            var operation = new OperationContext();
            operation.CorrelationVector = "CorrelationVector";
            Assert.AreEqual("CorrelationVector", operation.CorrelationVector);
        }
    }
}
