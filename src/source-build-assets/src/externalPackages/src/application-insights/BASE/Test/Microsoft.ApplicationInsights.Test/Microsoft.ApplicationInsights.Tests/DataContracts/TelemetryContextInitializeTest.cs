namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// If a user sets the context on a TelemetryClient, it is expected that those values will propagate to individual TelemetryItems.
    /// </summary>
    [TestClass]
    public class TelemetryContextInitializeTest
    {
        private TelemetryBuffer TelemetryBuffer;
        private ITelemetryChannel TelemetryChannel;
        private TelemetryClient TelemetryClient;

        #region Telemetry Context Properties
        private const string TestInstrumentationKey = "00000000-0000-0000-0000-000000000000";

        private const string TestComponentVersion = nameof(TestComponentVersion);

        private const string TestDeviceType = nameof(TestDeviceType);
        private const string TestDeviceId = nameof(TestDeviceId);
        private const string TestDeviceOperatingSystem = nameof(TestDeviceOperatingSystem);
        private const string TestDeviceOemName = nameof(TestDeviceOemName);
        private const string TestDeviceModel = nameof(TestDeviceModel);

        private const string TestCloudRoleName = nameof(TestCloudRoleName);
        private const string TestCloudRoleInstance = nameof(TestCloudRoleInstance);

        private const string TestSessionId = nameof(TestSessionId);
        private const bool TestSessionIsFirst = true;

        private const string TestUserId = nameof(TestUserId);
        private const string TestUserAccountId = nameof(TestUserAccountId);
        private const string TestUserUserAgent = nameof(TestUserUserAgent);
        private const string TestUserAuthenticatedUserId = nameof(TestUserAuthenticatedUserId);

        private const string TestOperationId = nameof(TestOperationId);
        private const string TestOperationParentId = nameof(TestOperationParentId);
        private const string TestOperationCorrelationVector = nameof(TestOperationCorrelationVector);
        private const string TestOperationSyntheticSource = nameof(TestOperationSyntheticSource);
        private const string TestOperationName = nameof(TestOperationName);

        private const string TestLocationIp = nameof(TestLocationIp);

        private const string TestInternalSdkVersion = nameof(TestInternalSdkVersion);
        private const string TestInternalAgentVersion = nameof(TestInternalAgentVersion);
        private const string TestInternalNodeName = nameof(TestInternalNodeName);
        #endregion

        #region Dependency Telemetry Properties
        private const string TestDependencyType = nameof(TestDependencyType);
        private const string TestDependencyName = nameof(TestDependencyName);
        private const string TestTarget = nameof(TestTarget);
        private const string TestData = nameof(TestData);
        #endregion

        /// <summary>
        /// Initialize InMemory TelemetryChannel, and set every possible property on the TelemetryContext.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.TelemetryBuffer = new TelemetryBuffer();
            this.TelemetryChannel = new InMemoryChannel(this.TelemetryBuffer, new InMemoryTransmitter(this.TelemetryBuffer));
            this.TelemetryClient = new TelemetryClient(new Extensibility.TelemetryConfiguration(instrumentationKey: TestInstrumentationKey, channel: this.TelemetryChannel));

            this.TelemetryClient.Context.Component.Version = TestComponentVersion;

            this.TelemetryClient.Context.Device.Type = TestDeviceType;
            this.TelemetryClient.Context.Device.Id = TestDeviceId;
            this.TelemetryClient.Context.Device.OperatingSystem = TestDeviceOperatingSystem;
            this.TelemetryClient.Context.Device.OemName = TestDeviceOemName;
            this.TelemetryClient.Context.Device.Model = TestDeviceModel;

            this.TelemetryClient.Context.Cloud.RoleName = TestCloudRoleName;
            this.TelemetryClient.Context.Cloud.RoleInstance = TestCloudRoleInstance;

            this.TelemetryClient.Context.Session.Id = TestSessionId;
            this.TelemetryClient.Context.Session.IsFirst = TestSessionIsFirst;

            this.TelemetryClient.Context.User.Id = TestUserId;
            this.TelemetryClient.Context.User.AccountId = TestUserAccountId;
            this.TelemetryClient.Context.User.UserAgent = TestUserUserAgent;
            this.TelemetryClient.Context.User.AuthenticatedUserId = TestUserAuthenticatedUserId;

            this.TelemetryClient.Context.Operation.Id = TestOperationId;
            this.TelemetryClient.Context.Operation.ParentId = TestOperationParentId;
            this.TelemetryClient.Context.Operation.CorrelationVector = TestOperationCorrelationVector;
            this.TelemetryClient.Context.Operation.SyntheticSource = TestOperationSyntheticSource;
            this.TelemetryClient.Context.Operation.Name = TestOperationName;

            this.TelemetryClient.Context.Location.Ip = TestLocationIp;

            this.TelemetryClient.Context.Internal.SdkVersion = TestInternalSdkVersion;
            this.TelemetryClient.Context.Internal.AgentVersion = TestInternalAgentVersion;
            this.TelemetryClient.Context.Internal.NodeName = TestInternalNodeName;
        }

        /// <summary>
        /// Verify that all TelemetryContext properties are set on a TelemetryItem.
        /// </summary>
        [TestMethod]
        public void VerifyContextPropertiesAreCopied()
        {
            var d = GetNewDependencyTelemetry();
            this.TelemetryClient.TrackDependency(d);

            IEnumerable<ITelemetry> telemetryItems = this.TelemetryBuffer.Dequeue();
            Assert.AreEqual(1, telemetryItems.Count());

            var dependencyTelemetryItem = telemetryItems.First() as DependencyTelemetry;
            Assert.AreEqual(TestDependencyName, dependencyTelemetryItem.Name);

            VerifyContext(dependencyTelemetryItem);
        }

        /// <summary>
        /// Verify that all TelemetryItem.Context properties are not overridden by the TelemetryContext.
        /// </summary>
        [TestMethod]
        public void VerifyOverridenContextPropertiesPersist()
        {
            var d = GetNewDependencyTelemetry(overrideContext: true);
            this.TelemetryClient.TrackDependency(d);

            IEnumerable<ITelemetry> telemetryItems = this.TelemetryBuffer.Dequeue();
            Assert.AreEqual(1, telemetryItems.Count());

            var dependencyTelemetryItem = telemetryItems.First() as DependencyTelemetry;
            Assert.AreEqual(TestDependencyName, dependencyTelemetryItem.Name);

            VerifyOverriddenContext(dependencyTelemetryItem);
        }

        /// <summary>
        /// Make a new DependencyTelemetry item, with or without overriding the Context properties.
        /// </summary>
        private DependencyTelemetry GetNewDependencyTelemetry(bool overrideContext = false)
        {
            var d = new DependencyTelemetry(dependencyTypeName: TestDependencyType, target: TestTarget, dependencyName: TestDependencyName, data: TestData);

            if (overrideContext)
            {
                d.Context.Component.Version = TestComponentVersion + "Overridden";

                d.Context.Device.Type = TestDeviceType + "Overridden";
                d.Context.Device.Id = TestDeviceId + "Overridden";
                d.Context.Device.OperatingSystem = TestDeviceOperatingSystem + "Overridden";
                d.Context.Device.OemName = TestDeviceOemName + "Overridden";
                d.Context.Device.Model = TestDeviceModel + "Overridden";

                d.Context.Cloud.RoleName = TestCloudRoleName + "Overridden";
                d.Context.Cloud.RoleInstance = TestCloudRoleInstance + "Overridden";

                d.Context.Session.Id = TestSessionId + "Overridden";
                d.Context.Session.IsFirst = !TestSessionIsFirst;

                d.Context.User.Id = TestUserId + "Overridden";
                d.Context.User.AccountId = TestUserAccountId + "Overridden";
                d.Context.User.UserAgent = TestUserUserAgent + "Overridden";
                d.Context.User.AuthenticatedUserId = TestUserAuthenticatedUserId + "Overridden";

                d.Context.Operation.Id = TestOperationId + "Overridden";
                d.Context.Operation.ParentId = TestOperationParentId + "Overridden";
                d.Context.Operation.CorrelationVector = TestOperationCorrelationVector + "Overridden";
                d.Context.Operation.SyntheticSource = TestOperationSyntheticSource + "Overridden";
                d.Context.Operation.Name = TestOperationName + "Overridden";

                d.Context.Location.Ip = TestLocationIp + "Overridden";

                d.Context.Internal.SdkVersion = TestInternalSdkVersion + "Overridden";
                d.Context.Internal.AgentVersion = TestInternalAgentVersion + "Overridden";
                d.Context.Internal.NodeName = TestInternalNodeName + "Overridden";
            }

            return d;
        }

        private void VerifyContext(ITelemetry telemetryItem)
        {
            Assert.AreEqual(TestInstrumentationKey, telemetryItem.Context.InstrumentationKey);

            Assert.AreEqual(TestComponentVersion, telemetryItem.Context.Component.Version);
            
            Assert.AreEqual(TestDeviceType, telemetryItem.Context.Device.Type);
            Assert.AreEqual(TestDeviceId, telemetryItem.Context.Device.Id);
            Assert.AreEqual(TestDeviceOperatingSystem, telemetryItem.Context.Device.OperatingSystem);
            Assert.AreEqual(TestDeviceOemName, telemetryItem.Context.Device.OemName);
            Assert.AreEqual(TestDeviceModel, telemetryItem.Context.Device.Model);
            
            Assert.AreEqual(TestCloudRoleName, telemetryItem.Context.Cloud.RoleName);
            Assert.AreEqual(TestCloudRoleInstance, telemetryItem.Context.Cloud.RoleInstance);
            
            Assert.AreEqual(TestSessionId, telemetryItem.Context.Session.Id);
            Assert.AreEqual(TestSessionIsFirst, telemetryItem.Context.Session.IsFirst);
            
            Assert.AreEqual(TestUserId, telemetryItem.Context.User.Id);
            Assert.AreEqual(TestUserAccountId, telemetryItem.Context.User.AccountId);
            Assert.AreEqual(TestUserUserAgent, telemetryItem.Context.User.UserAgent);
            Assert.AreEqual(TestUserAuthenticatedUserId, telemetryItem.Context.User.AuthenticatedUserId);
            
            Assert.AreEqual(TestOperationId, telemetryItem.Context.Operation.Id);
            Assert.AreEqual(TestOperationParentId, telemetryItem.Context.Operation.ParentId);
            Assert.AreEqual(TestOperationCorrelationVector, telemetryItem.Context.Operation.CorrelationVector);
            Assert.AreEqual(TestOperationSyntheticSource, telemetryItem.Context.Operation.SyntheticSource);
            Assert.AreEqual(TestOperationName, telemetryItem.Context.Operation.Name);
            
            Assert.AreEqual(TestLocationIp, telemetryItem.Context.Location.Ip);
            
            Assert.AreEqual(TestInternalSdkVersion, telemetryItem.Context.Internal.SdkVersion);
            Assert.AreEqual(TestInternalAgentVersion, telemetryItem.Context.Internal.AgentVersion);
            Assert.AreEqual(TestInternalNodeName, telemetryItem.Context.Internal.NodeName);
        }

        private void VerifyOverriddenContext(ITelemetry telemetryItem)
        {
            Assert.AreEqual(TestInstrumentationKey, telemetryItem.Context.InstrumentationKey);

            Assert.AreEqual(TestComponentVersion + "Overridden", telemetryItem.Context.Component.Version);

            Assert.AreEqual(TestDeviceType + "Overridden", telemetryItem.Context.Device.Type);
            Assert.AreEqual(TestDeviceId + "Overridden", telemetryItem.Context.Device.Id);
            Assert.AreEqual(TestDeviceOperatingSystem + "Overridden", telemetryItem.Context.Device.OperatingSystem);
            Assert.AreEqual(TestDeviceOemName + "Overridden", telemetryItem.Context.Device.OemName);
            Assert.AreEqual(TestDeviceModel + "Overridden", telemetryItem.Context.Device.Model);

            Assert.AreEqual(TestCloudRoleName + "Overridden", telemetryItem.Context.Cloud.RoleName);
            Assert.AreEqual(TestCloudRoleInstance + "Overridden", telemetryItem.Context.Cloud.RoleInstance);

            Assert.AreEqual(TestSessionId + "Overridden", telemetryItem.Context.Session.Id);
            Assert.AreEqual(!TestSessionIsFirst, telemetryItem.Context.Session.IsFirst);

            Assert.AreEqual(TestUserId + "Overridden", telemetryItem.Context.User.Id);
            Assert.AreEqual(TestUserAccountId + "Overridden", telemetryItem.Context.User.AccountId);
            Assert.AreEqual(TestUserUserAgent + "Overridden", telemetryItem.Context.User.UserAgent);
            Assert.AreEqual(TestUserAuthenticatedUserId + "Overridden", telemetryItem.Context.User.AuthenticatedUserId);

            Assert.AreEqual(TestOperationId + "Overridden", telemetryItem.Context.Operation.Id);
            Assert.AreEqual(TestOperationParentId + "Overridden", telemetryItem.Context.Operation.ParentId);
            Assert.AreEqual(TestOperationCorrelationVector + "Overridden", telemetryItem.Context.Operation.CorrelationVector);
            Assert.AreEqual(TestOperationSyntheticSource + "Overridden", telemetryItem.Context.Operation.SyntheticSource);
            Assert.AreEqual(TestOperationName + "Overridden", telemetryItem.Context.Operation.Name);

            Assert.AreEqual(TestLocationIp + "Overridden", telemetryItem.Context.Location.Ip);

            Assert.AreEqual(TestInternalSdkVersion + "Overridden", telemetryItem.Context.Internal.SdkVersion);
            Assert.AreEqual(TestInternalAgentVersion + "Overridden", telemetryItem.Context.Internal.AgentVersion);
            Assert.AreEqual(TestInternalNodeName + "Overridden", telemetryItem.Context.Internal.NodeName);
        }
    }
}
