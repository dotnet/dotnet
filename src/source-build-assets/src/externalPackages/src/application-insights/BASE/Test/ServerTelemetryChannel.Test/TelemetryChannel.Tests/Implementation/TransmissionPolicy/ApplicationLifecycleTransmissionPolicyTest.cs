namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using Channel.Helpers;

    public class ApplicationLifecycleTransmissionPolicyTest
    {
        [TestClass]
        [TestCategory("TransmissionPolicy")]
        public class HandleApplicationStoppingEvent : ApplicationLifecycleTransmissionPolicyTest
        {
            [TestMethod]
            public void StopsTransmissionSendingAndBuffering()
            {
                var transmitter = new StubTransmitter();
                
                var applicationLifecycle = new StubApplicationLifecycle();
                var policy = new ApplicationLifecycleTransmissionPolicy(applicationLifecycle);
                policy.Initialize(transmitter);

                applicationLifecycle.OnStopping(ApplicationStoppingEventArgs.Empty);

                Assert.AreEqual(0, policy.MaxSenderCapacity);
                Assert.AreEqual(0, policy.MaxBufferCapacity);
            }

            [TestMethod]
            public void EventHandlerIsNotAssignedInConstructorToPreventRaceConditionWithInitialize()
            {
                var applicationLifecycle = new StubApplicationLifecycle();
                var policy = new ApplicationLifecycleTransmissionPolicy(applicationLifecycle);

                bool asyncMethodInvoked = false;
                Func<Func<Task>, Task> asyncMethodRunner = asyncMethod =>
                {
                    asyncMethodInvoked = true;
                    return asyncMethod();
                };
                applicationLifecycle.OnStopping(new ApplicationStoppingEventArgs(asyncMethodRunner));

                Assert.IsFalse(asyncMethodInvoked);
            }
        }

        private class TestableApplicationLifecycleTransmissionPolicy : ApplicationLifecycleTransmissionPolicy
        {
            public TestableApplicationLifecycleTransmissionPolicy(IApplicationLifecycle applicationLifecycle) 
                : base(applicationLifecycle)
            {
            }

            public new int? MaxSenderCapacity
            {
                get { return base.MaxSenderCapacity; }
                set { base.MaxSenderCapacity = value; }
            }
        }
    }
}
