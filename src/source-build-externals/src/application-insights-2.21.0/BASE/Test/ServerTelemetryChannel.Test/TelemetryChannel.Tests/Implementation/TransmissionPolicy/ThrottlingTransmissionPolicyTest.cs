namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class ThrottlingTransmissionPolicyTest
    {
        [TestClass]
        [TestCategory("TransmissionPolicy")]
        public class HandleTransmissionSentEvent : ErrorHandlingTransmissionPolicyTest
        {
            private const int ResponseCodePaymentRequired = 402;
            private const int ResponseCodeUnsupported = 0;

            [TestMethod]
            public void AssertTooManyRequestsStopsSending()
            {
                this.EvaluateIfStatusCodeTriggersThrottling(ResponseStatusCodes.ResponseCodeTooManyRequests, 0, null, null, false);
            }

            [TestMethod]
            public void AssertTooManyRequestsStopsSendingWithFlushAsyncTask()
            {
                this.EvaluateIfStatusCodeTriggersThrottling(ResponseStatusCodes.ResponseCodeTooManyRequests, 0, 0, null, true);
            }

            [TestMethod]
            public void AssertTooManyRequestsOverExtendedTimeStopsSendingAndCleansCache()
            {
                this.EvaluateIfStatusCodeTriggersThrottling(ResponseStatusCodes.ResponseCodeTooManyRequestsOverExtendedTime, 0, 0, 0, false);
            }

            [TestMethod]
            public void AssertPaymentRequiredDoesntChangeCapacity()
            {
                this.EvaluateIfStatusCodeIgnored(ResponseCodePaymentRequired);
            }

            [TestMethod]
            public void AssertUnsupportedResponseCodeDoesnotChangeCapacity()
            {
                this.EvaluateIfStatusCodeIgnored(ResponseCodeUnsupported);
            }

            [TestMethod]
            public void CannotParseRetryAfterWritesToEventSource()
            {
                const string unparsableDate = "no one can parse me! :)";

                var transmitter = new StubTransmitter();
                var policy = new ThrottlingTransmissionPolicy();
                policy.Initialize(transmitter);

                using (var listener = new TestEventListener())
                {
                    const long AllKeyword = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeyword);

                    transmitter.OnTransmissionSent(
                        new TransmissionProcessedEventArgs(
                            new StubTransmission(),null, new HttpWebResponseWrapper()
                            {
                                StatusCode = ResponseStatusCodes.ResponseCodeTooManyRequests,
                                RetryAfterHeader = unparsableDate
                            })
                        );
                    EventWrittenEventArgs trace = listener.Messages.First(args => args.EventId == 24);
                    Assert.AreEqual(unparsableDate, (string)trace.Payload[0]);
                }
            }

            private void EvaluateIfStatusCodeTriggersThrottling(int responseCode, int? expectedSenderCapacity, int? expectedBufferCapacity, int? expectedStorageCapacity, bool hasFlushTask)
            {
                const int RetryAfterSeconds = 2;
                var waitForTheFirstApplyAsync = TimeSpan.FromMilliseconds(100);
                var waitForTheSecondApplyAsync = TimeSpan.FromMilliseconds(RetryAfterSeconds * 1000 + 500);

                // SETUP
                var transmitter = new StubTransmitterEvalOnApply();

                var policy = new ThrottlingTransmissionPolicy();
                policy.Initialize(transmitter);

                transmitter.InvokeTransmissionSentEvent(responseCode, TimeSpan.FromSeconds(RetryAfterSeconds), hasFlushTask);
                
                // ASSERT: First Handle will trigger Throttle and delay.
                Assert.IsTrue(transmitter.IsApplyInvoked(waitForTheFirstApplyAsync));
                
                Assert.AreEqual(expectedSenderCapacity, policy.MaxSenderCapacity);
                Assert.AreEqual(expectedBufferCapacity, policy.MaxBufferCapacity);
                Assert.AreEqual(expectedStorageCapacity, policy.MaxStorageCapacity);

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
                transmitter.InvokeTransmissionSentEvent(statusCode, default, false);

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

                public void InvokeTransmissionSentEvent(int responseStatusCode, TimeSpan retryAfter, bool isFlushAsyncInProgress)
                {
                    this.OnTransmissionSent(new TransmissionProcessedEventArgs(
                        transmission: new StubTransmission() { IsFlushAsyncInProgress = isFlushAsyncInProgress },
                        exception: null,
                        response: new HttpWebResponseWrapper()
                        {
                            StatusCode = responseStatusCode,
                            StatusDescription = null,
                            RetryAfterHeader = DateTime.Now.ToUniversalTime().Add(retryAfter).ToString("R", CultureInfo.InvariantCulture),
                        }
                    ));
                }

                public bool IsApplyInvoked(TimeSpan timeout) => this.autoResetEvent.WaitOne(timeout);
            }
        }
    }
}