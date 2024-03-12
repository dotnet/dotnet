using System.Net.Http;

namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Channel.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using System.Text;
    using System.IO;

    public class ErrorHandlingTransmissionPolicyTest
    {
        [TestClass]
        [TestCategory("TransmissionPolicy")]
        [TestCategory("WindowsOnly")] // these tests are flaky on linux builds.
        public class HandleTransmissionSentEvent : ErrorHandlingTransmissionPolicyTest
        {
            [TestMethod]
            public void StopsTransmissionSendingWhenTransmissionTimesOut()
            {
                StopsTransmissionSendingForGivenResponseCode(ResponseStatusCodes.RequestTimeout);
            }

            [TestMethod]
            public void StopsTransmissionSendingWhenTransmissionGetServerError()
            {
                StopsTransmissionSendingForGivenResponseCode(ResponseStatusCodes.InternalServerError);
            }

            [TestMethod]
            public void StopsTransmissionSendingWhenTransmissionGetServerUnavailable()
            {
                StopsTransmissionSendingForGivenResponseCode(ResponseStatusCodes.ServiceUnavailable);
            }

            [TestMethod]
            public void StopsTransmissionSendingForBadGateway()
            {
                StopsTransmissionSendingForGivenResponseCode(ResponseStatusCodes.BadGateway);
            }

            [TestMethod]
            public void StopsTransmissionSendingForGatewayTimeout()
            {
                StopsTransmissionSendingForGivenResponseCode(ResponseStatusCodes.GatewayTimeout);
            }

            [TestMethod]
            public void ResumesTransmissionSenderAfterPauseDurationWhenTransmissionTimesOut()
            {
                ResumesTransmissionSenderAfterPauseDuration(ResponseStatusCodes.RequestTimeout);
            }

            [TestMethod]
            public void ResumesTransmissionSenderAfterPauseDurationWhenTransmissionGetServerError()
            {
                ResumesTransmissionSenderAfterPauseDuration(ResponseStatusCodes.InternalServerError);
            }

            [TestMethod]
            public void ResumesTransmissionSenderAfterPauseDurationWhenTransmissionGetServerUnavailable()
            {
                ResumesTransmissionSenderAfterPauseDuration(ResponseStatusCodes.ServiceUnavailable);
            }

            [TestMethod]
            public void ResumesTransmissionSenderAfterPauseDurationForUnknownNetworkError()
            {
                ResumesTransmissionSenderAfterPauseDuration(ResponseStatusCodes.UnknownNetworkError);
            }

            [TestMethod]
            public void ResumesTransmissionSenderAfterPauseDurationForBadGateway()
            {
                ResumesTransmissionSenderAfterPauseDuration(ResponseStatusCodes.BadGateway);
            }

            [TestMethod]
            public void ResumesTransmissionSenderAfterPauseDurationForGatewayTimeout()
            {
                ResumesTransmissionSenderAfterPauseDuration(ResponseStatusCodes.GatewayTimeout);
            }

            [TestMethod]
            public void KeepsTransmissionSenderPausedWhenAdditionalTransmissionsFail()
            {
                var transmitter = new StubTransmitter(new TestableBackoffLogicManager(TimeSpan.FromMinutes(1)));
                var policy = new ErrorHandlingTransmissionPolicy();
                policy.Initialize(transmitter);
                
                transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(new StubTransmission(), new Exception("Error"), new HttpWebResponseWrapper() {StatusCode = ResponseStatusCodes.InternalServerError}));
                transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(new StubTransmission(), new Exception("Error"), new HttpWebResponseWrapper() { StatusCode = ResponseStatusCodes.InternalServerError }));

                Thread.Sleep(TimeSpan.FromMilliseconds(30));

                Assert.AreEqual(0, policy.MaxSenderCapacity);
            }

            [TestMethod]
            public void RetriesFailedTransmissionIfItsNumberOfAttemptsDidNotReachMaximum()
            {
                Transmission enqueuedTransmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = transmission =>
                {
                    enqueuedTransmission = transmission;
                };

                var policy = new ErrorHandlingTransmissionPolicy();
                policy.Initialize(transmitter);

                var failedTransmission = new StubTransmission();
                transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(failedTransmission, new Exception("Error"), new HttpWebResponseWrapper() { StatusCode = ResponseStatusCodes.InternalServerError }));

                Assert.AreSame(failedTransmission, enqueuedTransmission);
            }

            [TestMethod]
            public void RetriesFailedTransmissionInfinitely()
            {
                Transmission enqueuedTransmission = null;

                var transmitter = new StubTransmitter(new TestableBackoffLogicManager(TimeSpan.FromMilliseconds(10)));
                transmitter.OnEnqueue = transmission =>
                {
                    enqueuedTransmission = transmission;
                };

                var policy = new ErrorHandlingTransmissionPolicy();
                policy.Initialize(transmitter);                
                var failedTransmission = new StubTransmission();
                transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(failedTransmission, new Exception("Error"), new HttpWebResponseWrapper() { StatusCode = ResponseStatusCodes.InternalServerError }));
                transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(failedTransmission, new Exception("Error"), new HttpWebResponseWrapper() { StatusCode = ResponseStatusCodes.InternalServerError }));
                transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(failedTransmission, new Exception("Error"), new HttpWebResponseWrapper() { StatusCode = ResponseStatusCodes.InternalServerError }));
                Assert.AreSame(failedTransmission, enqueuedTransmission);
            }

            [TestMethod]
            public void DoesNotRetrySuccessfulTransmission()
            {
                Transmission enqueuedTransmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = transmission =>
                {
                    enqueuedTransmission = transmission;
                };

                var policy = new ErrorHandlingTransmissionPolicy();
                policy.Initialize(transmitter);

                var successfulTransmission = new StubTransmission();
                transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(successfulTransmission,null, new HttpWebResponseWrapper(){StatusCode = 200}));

                Assert.IsNull(enqueuedTransmission);
                Assert.AreEqual(0, transmitter.BackoffLogicManager.ConsecutiveErrors);
            }

            [TestMethod]
            public void DoesNotRetryTransmissionForUnknownResponseCode()
            {
                Transmission enqueuedTransmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = transmission =>
                {
                    enqueuedTransmission = transmission;
                };

                var policy = new ErrorHandlingTransmissionPolicy();
                policy.Initialize(transmitter);

                var successfulTransmission = new StubTransmission();
                transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(successfulTransmission, null, null));

                Assert.IsNull(enqueuedTransmission);
                Assert.AreEqual(0, transmitter.BackoffLogicManager.ConsecutiveErrors);
            }
            
            [TestMethod]
            public void LogsDataLossEventsWhenExceptionisNotNull()
            {
                using (var listener = new TestEventListener())
                {
                    // Arrange:
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeywords);

                    Transmission enqueuedTransmission = null;
                    var transmitter = new StubTransmitter(new BackoffLogicManager(TimeSpan.FromMilliseconds(10)))
                    {
                        OnEnqueue = transmission => { enqueuedTransmission = transmission; }
                    };

                    var policy = new ErrorHandlingTransmissionPolicy();
                    policy.Initialize(transmitter);

                    var failedTransmission = new StubTransmission();                    

                    // Act:
                    transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(failedTransmission, new Exception("Data loss"), null));

                    // Assert:
                    var traces = listener.Messages.Where(item => item.Level == EventLevel.Error).ToList();
                    Assert.AreEqual(1, traces.Count);
                    Assert.AreEqual(69, traces[0].EventId); // failed to send
                    Assert.AreEqual("Data loss", traces[0].Payload[1]);
                    // Sets flush task to failure on not whitelisted status code
                    Assert.IsFalse(failedTransmission.IsFlushAsyncInProgress);
                }
            }

            [TestMethod]
            public void LogsDataLossEventsWhenExceptionisNull()
            {
                using (var listener = new TestEventListener())
                {
                    // Arrange:
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeywords);

                    Transmission enqueuedTransmission = null;
                    var transmitter = new StubTransmitter(new BackoffLogicManager(TimeSpan.FromMilliseconds(10)))
                    {
                        OnEnqueue = transmission => { enqueuedTransmission = transmission; }
                    };

                    var policy = new ErrorHandlingTransmissionPolicy();
                    policy.Initialize(transmitter);

                    var failedTransmission = new StubTransmission();

                    // Act:
                    transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(failedTransmission, null, null));

                    // Assert:
                    var traces = listener.Messages.Where(item => item.Level == EventLevel.Error).ToList();
                    Assert.AreEqual(1, traces.Count);
                    Assert.AreEqual(69, traces[0].EventId); // failed to send
                    Assert.AreEqual("Unknown Exception Message", traces[0].Payload[1]);
                    // Sets flush task to failure on not whitelisted status code
                    Assert.IsFalse(failedTransmission.IsFlushAsyncInProgress);
                }
            }

            [TestMethod]
            public void LogsWarningWhenDataLossIntentional()
            {
                // ErrorHandlingTransmissionPolicy does retry only for a whitelisted set of status codes. For 
                // others telemetry is dropped. This test is to validate that those are logged.
                using (var listener = new TestEventListener())
                {
                    // Arrange:
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeywords);

                    Transmission enqueuedTransmission = null;
                    var transmitter = new StubTransmitter(new BackoffLogicManager(TimeSpan.FromMilliseconds(10)))
                    {
                        OnEnqueue = transmission => { enqueuedTransmission = transmission; }
                    };

                    var policy = new ErrorHandlingTransmissionPolicy();
                    policy.Initialize(transmitter);

                    var failedTransmission = new StubTransmission();

                    // Act:
                    var res = new HttpWebResponseWrapper();
                    res.StatusCode = 8989;  // some status code not whitelisted for retry.
                    transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(failedTransmission, null, res));

                    // Assert:
                    var traces = listener.Messages.Where(item => item.Level == EventLevel.Warning).ToList();
                    Assert.AreEqual(1, traces.Count);
                    Assert.AreEqual(71, traces[0].EventId); // failed to send
                    Assert.AreEqual("8989", traces[0].Payload[1]);
                    // Sets flush task to failure on not whitelisted status code
                    Assert.IsFalse(failedTransmission.IsFlushAsyncInProgress);
                }
            }

            [TestMethod]
            public void NoWarningLogsWhenResponseIsSucess()
            {
                // ErrorHandlingTransmissionPolicy does retry only for a whitelisted set of status codes. For 
                // success status, there should be no warnings. This test is to validate that no warning is logged.
                using (var listener = new TestEventListener())
                {
                    // Arrange:
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeywords);

                    Transmission enqueuedTransmission = null;
                    var transmitter = new StubTransmitter(new BackoffLogicManager(TimeSpan.FromMilliseconds(10)))
                    {
                        OnEnqueue = transmission => { enqueuedTransmission = transmission; }
                    };

                    var policy = new ErrorHandlingTransmissionPolicy();
                    policy.Initialize(transmitter);

                    var failedTransmission = new StubTransmission();

                    // Act:
                    var res = new HttpWebResponseWrapper();
                    res.StatusCode = 200;  // Sucess
                    transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(failedTransmission, null, res));

                    // Assert:
                    var traces = listener.Messages.Where(item => item.Level == EventLevel.Warning).ToList();
                    Assert.AreEqual(0, traces.Count);
                }
            }

            [TestMethod]
            public void NoWarningLogsWhenResponseIsPartiallSuccess()
            {
                // ErrorHandlingTransmissionPolicy does retry only for a whitelisted set of status codes. For 
                // partial success (206) status, there should be no warnings as this is handled by separate
                // Retry policy. This test is to validate that no warning is logged.
                using (var listener = new TestEventListener())
                {
                    // Arrange:
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeywords);

                    Transmission enqueuedTransmission = null;
                    var transmitter = new StubTransmitter(new BackoffLogicManager(TimeSpan.FromMilliseconds(10)))
                    {
                        OnEnqueue = transmission => { enqueuedTransmission = transmission; }
                    };

                    var policy = new ErrorHandlingTransmissionPolicy();
                    policy.Initialize(transmitter);

                    var failedTransmission = new StubTransmission();

                    // Act:
                    var res = new HttpWebResponseWrapper();
                    res.StatusCode = 206;  // Sucess
                    transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(failedTransmission, null, res));

                    // Assert:
                    var traces = listener.Messages.Where(item => item.Level == EventLevel.Warning).ToList();
                    Assert.AreEqual(0, traces.Count);
                }
            }

            [TestMethod]
            public void LogsAdditionalTracesIfResponseIsProvided()
            {
                using (var listener = new TestEventListener())
                {
                    // Arrange:
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeywords);

                    Transmission enqueuedTransmission = null;
                    var transmitter = new StubTransmitter (new BackoffLogicManager(TimeSpan.FromMilliseconds(10)))
                    {
                        OnEnqueue = transmission => { enqueuedTransmission = transmission; }
                    };

                    var policy = new ErrorHandlingTransmissionPolicy();
                    policy.Initialize(transmitter);

                    var failedTransmission = new StubTransmission();
                    var response = new HttpWebResponseWrapper {Content = BackendResponseHelper.CreateBackendResponse(2, 1, new[] { "123" })};
                    response.StatusCode = 502;

                    // Act:
                    transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(failedTransmission, null, response));

                    Thread.Sleep(1000);

                    // Assert:
                    var traces = listener.Messages.Where(item => item.Level == EventLevel.Warning).ToList();
                    Assert.AreEqual(1, traces.Count);
                    Assert.AreEqual(7, traces[0].EventId); // failed to send                    
                    Assert.AreEqual("Explanation", traces[0].Payload[0]);
                }
            }

            private static Task ThrowAsync(Exception e)
            {
                var tcs = new TaskCompletionSource<object>(null);
                tcs.SetException(e);
                return tcs.Task;
            }

            private void StopsTransmissionSendingForGivenResponseCode(int responseStatusCode)
            {
                var policyApplied = new AutoResetEvent(false);
                var transmitter = new StubTransmitter(new TestableBackoffLogicManager(TimeSpan.FromSeconds(10)));
                transmitter.OnApplyPolicies = () =>
                {
                    policyApplied.Set();
                };

                var policy = new ErrorHandlingTransmissionPolicy();
                policy.Initialize(transmitter);

                transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(new StubTransmission(), new Exception("Error"), new HttpWebResponseWrapper() { StatusCode = responseStatusCode }));


                Assert.IsTrue(policyApplied.WaitOne(100));
                Assert.AreEqual(0, policy.MaxSenderCapacity);
            }

            private void ResumesTransmissionSenderAfterPauseDuration(int responseStatusCode)
            {
                var policyApplied = new AutoResetEvent(false);
                var transmitter = new StubTransmitter(new TestableBackoffLogicManager(TimeSpan.FromMilliseconds(1)));
                transmitter.OnApplyPolicies = () => { policyApplied.Set(); };

                var policy = new ErrorHandlingTransmissionPolicy();
                policy.Initialize(transmitter);

                transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(new StubTransmission(),
                    new Exception("Error"),
                    new HttpWebResponseWrapper() {StatusCode = responseStatusCode }));

                Assert.IsTrue(policyApplied.WaitOne(100));
                Assert.IsTrue(policyApplied.WaitOne(100));
                Assert.IsNull(policy.MaxSenderCapacity);
            }
        }
    }
}