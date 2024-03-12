namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Channel.Implementation;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using TaskEx = System.Threading.Tasks.Task;

    [TestClass]
    [TestCategory("TransmissionPolicy")]
    public class PartialSuccessTransmissionPolicyTest
    {
        [TestMethod]
        public void IfItemIsRejectedOnlyThisItemIsUploadedBack()
        {
            IList<Transmission> enqueuedTransmissions = new List<Transmission>();
            var transmitter = new StubTransmitter
            {
                OnEnqueue = t => { enqueuedTransmissions.Add(t); }
            };

            var policy = new PartialSuccessTransmissionPolicy();
            policy.Initialize(transmitter);

            var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };
            Transmission transmission = new Transmission(new Uri("http://uri"), items, "type", "encoding");

            string response = BackendResponseHelper.CreateBackendResponse(
                itemsReceived: 2, 
                itemsAccepted: 1, 
                errorCodes: new[] { "429" });

            var wrapper = new HttpWebResponseWrapper
            {
                StatusCode = 206,
                Content = response
            };

            transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));

            Assert.AreEqual(1, enqueuedTransmissions.Count);
        }

        [TestMethod]
        public void IfItemsAreRejectedTheyAreUploadedBackAsASingleTransmission()
        {
            IList<Transmission> enqueuedTransmissions = new List<Transmission>();
            var transmitter = new StubTransmitter
            {
                OnEnqueue = t => { enqueuedTransmissions.Add(t); }
            };

            var policy = new PartialSuccessTransmissionPolicy();
            policy.Initialize(transmitter);

            var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };
            Transmission transmission = new Transmission(new Uri("http://uri"), items, "type", "encoding");

            string response = BackendResponseHelper.CreateBackendResponse(
                itemsReceived: 3,
                itemsAccepted: 0,
                errorCodes: new[] { "408", "503", "500" });

            var wrapper = new HttpWebResponseWrapper
            {
                StatusCode = 206,
                Content = response
            };
            transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));

            Assert.AreEqual(1, enqueuedTransmissions.Count);
        }

        [TestMethod]
        public void IfNumberOfRecievedItemsEqualsToNumberOfAcceptedErrorsListIsIgnored()
        {
            var transmitter = new StubTransmitter();

            var policy = new PartialSuccessTransmissionPolicy();
            policy.Initialize(transmitter);

            var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };
            Transmission transmission = new Transmission(new Uri("http://uri"), items, "type", "encoding");

            string response = BackendResponseHelper.CreateBackendResponse(
                itemsReceived: 2,
                itemsAccepted: 2,
                errorCodes: new[] { "408", "408" });

            var wrapper = new HttpWebResponseWrapper
            {
                StatusCode = 206,
                Content = response
            };

            transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));

            Assert.AreEqual(0, transmitter.BackoffLogicManager.ConsecutiveErrors);
        }

        [TestMethod]
        public void NewTransmissionCreatedByIndexesFromResponse()
        {
            IList<Transmission> enqueuedTransmissions = new List<Transmission>();
            var transmitter = new StubTransmitter
            {
                OnEnqueue = t => { enqueuedTransmissions.Add(t); }
            };

            var policy = new PartialSuccessTransmissionPolicy();
            policy.Initialize(transmitter);

            var items = new List<ITelemetry>
                {
                    new EventTelemetry("1"),
                    new EventTelemetry("2"),
                    new EventTelemetry("3"),
                };
            Transmission transmission = new Transmission(new Uri("http://uri"), items, "type", "encoding");

            string response = BackendResponseHelper.CreateBackendResponse(
                itemsReceived: 3,
                itemsAccepted: 1,
                errorCodes: new[] { "439", "439" });

            var wrapper = new HttpWebResponseWrapper
            {
                StatusCode = 206,
                Content = response
            };

            transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));

            string[] newItems = JsonSerializer
                .Deserialize(enqueuedTransmissions[0].Content)
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Assert.AreEqual(2, newItems.Length);
            Assert.IsTrue(newItems[0].Contains("\"name\":\"1\""));
            Assert.IsTrue(newItems[1].Contains("\"name\":\"2\""));
        }

        [TestMethod]
        public void IfMultipleItemsAreRejectedNumberOfErrorsIsIncreasedByOne()
        {
            // Number of errors determine backoff timeout. 
            // When we get several bad items in one batch we want to increase errors by 1 only since it is one attempt to access Breeze

            var transmitter = new StubTransmitter();

            var policy = new PartialSuccessTransmissionPolicy();
            policy.Initialize(transmitter);

            var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };
            Transmission transmission = new Transmission(new Uri("http://uri"), items, "type", "encoding");

            string response = BackendResponseHelper.CreateBackendResponse(
                itemsReceived: 100,
                itemsAccepted: 95,
                errorCodes: new[] { "500", "500", "503", "503", "429" });

            var wrapper = new HttpWebResponseWrapper
            {
                StatusCode = 206,
                Content = response
            };

            transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));

            Assert.AreEqual(1, transmitter.BackoffLogicManager.ConsecutiveErrors);
        }

        [TestMethod]
        public void IfResponseIsBadJsonWeDoNotIncreaseErrorCount()
        {
            IList<Transmission> enqueuedTransmissions = new List<Transmission>();
            var transmitter = new StubTransmitter
            {
                OnEnqueue = t => { enqueuedTransmissions.Add(t); }
            };

            var policy = new PartialSuccessTransmissionPolicy();
            policy.Initialize(transmitter);

            var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };
            Transmission transmission = new Transmission(new Uri("http://uri"), items, "type", "encoding");

            string response = "[,]";

            var wrapper = new HttpWebResponseWrapper
            {
                StatusCode = 206,
                Content = response
            };

            transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));

            Assert.AreEqual(0, transmitter.BackoffLogicManager.ConsecutiveErrors);
            Assert.AreEqual(0, enqueuedTransmissions.Count);
        }

        [TestMethod]
        public void IfIndexMoreThanNumberOfItemsInTransmissionIgnoreError()
        {
            IList<Transmission> enqueuedTransmissions = new List<Transmission>();
            var transmitter = new StubTransmitter
            {
                OnEnqueue = t => { enqueuedTransmissions.Add(t); }
            };

            var policy = new PartialSuccessTransmissionPolicy();
            policy.Initialize(transmitter);

            var items = new List<ITelemetry> { new EventTelemetry() };
            Transmission transmission = new Transmission(new Uri("http://uri"), items, "type", "encoding");

            // Index is 0-based
            string response = BackendResponseHelper.CreateBackendResponse(
                itemsReceived: 2,
                itemsAccepted: 1,
                errorCodes: new[] { "408" },
                indexStartWith: 1);

            var wrapper = new HttpWebResponseWrapper
            {
                StatusCode = 206,
                Content = response
            };

            transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));

            Assert.AreEqual(0, transmitter.BackoffLogicManager.ConsecutiveErrors);
            Assert.AreEqual(0, enqueuedTransmissions.Count);
        }

        [TestMethod]
        public void IfIndexNegativeIgnoreError()
        {
            IList<Transmission> enqueuedTransmissions = new List<Transmission>();
            var transmitter = new StubTransmitter
            {
                OnEnqueue = t => { enqueuedTransmissions.Add(t); }
            };

            var policy = new PartialSuccessTransmissionPolicy();
            policy.Initialize(transmitter);

            var items = new List<ITelemetry> { new EventTelemetry() };
            Transmission transmission = new Transmission(new Uri("http://uri"), items, "type", "encoding");

            // Index is 0-based
            string response = BackendResponseHelper.CreateBackendResponse(
                itemsReceived: 2,
                itemsAccepted: 1,
                errorCodes: new[] { "408" },
                indexStartWith: -1);

            var wrapper = new HttpWebResponseWrapper
            {
                StatusCode = 206,
                Content = response
            };

            transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));

            Assert.AreEqual(0, transmitter.BackoffLogicManager.ConsecutiveErrors);
            Assert.AreEqual(0, enqueuedTransmissions.Count);
        }

        [TestMethod]
        public void DoesNotSendTransmissionForUnsupportedCodes()
        {
            IList<Transmission> enqueuedTransmissions = new List<Transmission>();
            var transmitter = new StubTransmitter
            {
                OnEnqueue = t => { enqueuedTransmissions.Add(t); }
            };

            var policy = new PartialSuccessTransmissionPolicy();
            policy.Initialize(transmitter);

            var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };
            Transmission transmission = new Transmission(new Uri("http://uri"), items, "type", "encoding");

            string response = BackendResponseHelper.CreateBackendResponse(
                itemsReceived: 50,
                itemsAccepted: 45,
                errorCodes: new[] { "400", "402", "502", "409", "417", "206" });

            var wrapper = new HttpWebResponseWrapper
            {
                StatusCode = 206,
                Content = response
            };

            transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));

            Assert.AreEqual(0, transmitter.BackoffLogicManager.ConsecutiveErrors);
            Assert.AreEqual(0, enqueuedTransmissions.Count);
        }

        [TestMethod]
        public void ItemsAreEnqueuedOnFlushAsync()
        {
            IList<Transmission> enqueuedTransmissions = new List<Transmission>();
            var transmitter = new StubTransmitter
            {
                OnEnqueue = t => { enqueuedTransmissions.Add(t); }
            };

            var policy = new PartialSuccessTransmissionPolicy();
            policy.Initialize(transmitter);

            var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };
            Transmission transmission = new Transmission(new Uri("http://uri"), items, "type", "encoding") { IsFlushAsyncInProgress = true };

            string response = BackendResponseHelper.CreateBackendResponse(
                itemsReceived: 2,
                itemsAccepted: 1,
                errorCodes: new[] { "429" });

            var wrapper = new HttpWebResponseWrapper
            {
                StatusCode = 206,
                Content = response
            };

            transmitter.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));

            Assert.AreEqual(1, enqueuedTransmissions.Count);
            Assert.IsTrue(transmission.IsFlushAsyncInProgress);
        }
    }
}
