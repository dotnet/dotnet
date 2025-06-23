namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Microsoft.ApplicationInsights.Channel.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory("TransmissionPolicy")]
    public class AuthenticationTransmissionPolicyTests
    {
        [TestMethod]
        public void Verify401TriggersThrottling() => this.EvaluateIfStatusCodeTriggersThrottling(ResponseStatusCodes.Unauthorized);

        [TestMethod]
        public void Verify403TriggersThrottling() => this.EvaluateIfStatusCodeTriggersThrottling(ResponseStatusCodes.Forbidden);

        [TestMethod]
        public void Verify400DoesNotTriggerThrottling() => this.EvaluateIfStatusCodeIgnored(ResponseStatusCodes.BadRequest);

        [TestMethod]
        public void Verify200DoesNotTriggerThrottling() => this.EvaluateIfStatusCodeIgnored(ResponseStatusCodes.Success);

        [TestMethod]
        public void VerifyOtherDoesNotTriggerThrottling() => this.EvaluateIfStatusCodeIgnored(000);

        private void EvaluateIfStatusCodeTriggersThrottling(int statusCode)
        {
            var retryDelay = TimeSpan.FromSeconds(5);
            var waitForTheFirstApplyAsync = TimeSpan.FromMilliseconds(100);
            var waitForTheSecondApplyAsync = retryDelay.Add(TimeSpan.FromMilliseconds(500)); // adding a few ms to give the unit test a buffer.

            // SETUP
            var transmitter = new StubTransmitterEvalOnApply();

            var policy = new AuthenticationTransmissionPolicy()
            {
                Enabled = true,
            };

            // we override the default timer here to speed up unit tests.
            policy.PauseTimer = new TaskTimerInternal { Delay = retryDelay };
            policy.Initialize(transmitter);

            // ACT
            transmitter.InvokeTransmissionSentEvent(statusCode);

            // ASSERT: First Handle will trigger Throttle and delay.
            Assert.IsTrue(transmitter.IsApplyInvoked(waitForTheFirstApplyAsync));

            Assert.AreEqual(0, policy.MaxSenderCapacity);
            Assert.AreEqual(0, policy.MaxBufferCapacity);
            Assert.IsNull(policy.MaxStorageCapacity);

            // ASSERT: Throttle expires and policy will be reset.
            Assert.IsTrue(transmitter.IsApplyInvoked(waitForTheSecondApplyAsync));

            Assert.IsNull(policy.MaxSenderCapacity);
            Assert.IsNull(policy.MaxBufferCapacity);
            Assert.IsNull(policy.MaxStorageCapacity);
        }

        private void EvaluateIfStatusCodeIgnored(int statusCode)
        {
            var waitForTheFirstApplyAsync = TimeSpan.FromMilliseconds(100);

            // SETUP
            var transmitter = new StubTransmitterEvalOnApply();

            var policy = new AuthenticationTransmissionPolicy()
            {
                Enabled = true,
            };
            policy.Initialize(transmitter);

            // ACT
            transmitter.InvokeTransmissionSentEvent(statusCode);

            // ASSERT: The Apply event handler should not be called.
            Assert.IsFalse(transmitter.IsApplyInvoked(waitForTheFirstApplyAsync));

            // ASSERT: Capacities should have default values.
            Assert.IsNull(policy.MaxSenderCapacity);
            Assert.IsNull(policy.MaxBufferCapacity);
            Assert.IsNull(policy.MaxStorageCapacity);
        }

        private class StubTransmitterEvalOnApply : StubTransmitter
        {
            private AutoResetEvent autoResetEvent;

            public StubTransmitterEvalOnApply()
            {
                this.autoResetEvent = new AutoResetEvent(false);
                this.OnApplyPolicies = () => this.autoResetEvent.Set();
            }

            public void InvokeTransmissionSentEvent(int responseStatusCode)
            {
                this.OnTransmissionSent(new TransmissionProcessedEventArgs(
                    transmission: new StubTransmission(),
                    exception: null,
                    response: new HttpWebResponseWrapper()
                    {
                        StatusCode = responseStatusCode,
                        StatusDescription = null,
                    }
                ));
            }

            public bool IsApplyInvoked(TimeSpan timeout) => this.autoResetEvent.WaitOne(timeout);
        }
    }
}
