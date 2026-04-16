namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuickPulseCollectionStateManagerTests
    {
        private const string StartCollectionMessage = "StartCollection";

        private const string StopCollectionMessage = "StopCollection";

        private const string CollectMessage = "Collect";

        private const string UpdatedConfigurationMessage = "UpdatedConfiguration";

        private static readonly CollectionConfigurationInfo EmptyCollectionConfigurationInfo = new CollectionConfigurationInfo()
        {
            ETag = string.Empty,
            Metrics = new CalculatedMetricInfo[0]
        };

        [TestMethod]
        public void QuickPulseCollectionStateManagerDoesNothingWithoutInstrumentationKey()
        {
            // ARRANGE
            var timings = QuickPulseTimings.Default;
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var actions = new List<string>();
            var returnedSamples = new List<QuickPulseDataSample>();
            var timeProvider = new ClockMock();
            var manager = CreateManager(serviceClient, timeProvider, actions, returnedSamples, timings);

            // ACT
            manager.UpdateState(string.Empty, string.Empty);
            manager.UpdateState(null, string.Empty);

            // ASSERT
            Assert.AreEqual(0, serviceClient.PingCount);
            Assert.AreEqual(0, serviceClient.SnappedSamples.Count);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerInitiallyInIdleState()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock();

            var manager = new QuickPulseCollectionStateManager(
                TelemetryConfiguration.CreateDefault(),
                serviceClient,
                new Clock(),
                QuickPulseTimings.Default,
                () => { },
                () => { },
                () => null,
                _ => { },
                _ => null,
                _ => { });

            // ACT

            // ASSERT
            Assert.AreEqual(false, manager.IsCollectingData);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerStaysInIdleState()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = false, ReturnValueFromSubmitSample = false };

            var actions = new List<string>();
            var manager = CreateManager(serviceClient, new Clock(), actions);

            // ACT
            for (int i = 0; i < 10; i++)
            {
                manager.UpdateState("empty iKey", string.Empty);
            }

            // ASSERT
            Assert.AreEqual(false, manager.IsCollectingData);
            Assert.AreEqual(0, actions.Count);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerTransitionsFromIdleToCollect()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };

            var actions = new List<string>();
            var manager = CreateManager(serviceClient, new Clock(), actions);

            // ACT
            manager.UpdateState("empty iKey", string.Empty);

            // on the next call it actually requests samples collected since the last call
            manager.UpdateState("empty iKey", string.Empty);

            // ASSERT
            Assert.AreEqual(true, manager.IsCollectingData);
            Assert.AreEqual(2, actions.Count);
            Assert.AreEqual(StartCollectionMessage, actions[0]);
            Assert.AreEqual(CollectMessage, actions[1]);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerTransitionsFromIdleToCollectAndImmediatelyBack()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = false };

            var actions = new List<string>();
            var manager = CreateManager(serviceClient, new Clock(), actions);

            // ACT
            manager.UpdateState("empty iKey", string.Empty);

            // requests samples collected since last call - and puts itself back into the idle state
            manager.UpdateState("empty iKey", string.Empty);

            // ASSERT
            Assert.AreEqual(false, manager.IsCollectingData);
            Assert.AreEqual(3, actions.Count);
            Assert.AreEqual(StartCollectionMessage, actions[0]);
            Assert.AreEqual(CollectMessage, actions[1]);
            Assert.AreEqual(StopCollectionMessage, actions[2]);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerTransitionsFromIdleToCollectAndStaysStable()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };

            var actions = new List<string>();
            var manager = CreateManager(serviceClient, new Clock(), actions);

            // ACT
            manager.UpdateState("empty iKey", string.Empty);

            // now we start sending samples
            int collectionCount = 10;
            for (int i = 0; i < collectionCount; i++)
            {
                manager.UpdateState("empty iKey", string.Empty);
            }

            // ASSERT
            Assert.AreEqual(true, manager.IsCollectingData);
            Assert.AreEqual(1 + collectionCount, actions.Count);
            Assert.AreEqual(StartCollectionMessage, actions[0]);
            for (int i = 1; i < collectionCount; i++)
            {
                Assert.AreEqual(CollectMessage, actions[i]);
            }
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerTransitionsFromCollectToIdleAndStaysStable()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };

            var actions = new List<string>();
            var manager = CreateManager(serviceClient, new Clock(), actions);

            // enter collect state
            manager.UpdateState("empty iKey", string.Empty);

            serviceClient.ReturnValueFromPing = false;
            serviceClient.ReturnValueFromSubmitSample = false;

            actions.Clear();

            // ACT
            int collectionCount = 10;
            for (int i = 0; i < collectionCount; i++)
            {
                manager.UpdateState("empty iKey", string.Empty);
            }

            // ASSERT
            Assert.AreEqual(false, manager.IsCollectingData);
            Assert.AreEqual(2, actions.Count);
            Assert.AreEqual(CollectMessage, actions[0]);
            Assert.AreEqual(StopCollectionMessage, actions[1]);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerFlipFlopsBetweenIdleAndCollect()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = false };

            var actions = new List<string>();
            var manager = CreateManager(serviceClient, new Clock(), actions);

            // ACT
            int collectionCount = 10;
            for (int i = 0; i < collectionCount; i++)
            {
                manager.UpdateState("empty iKey", string.Empty);
            }

            // ASSERT
            Assert.AreEqual(false, manager.IsCollectingData);
            Assert.AreEqual(1.5 * collectionCount, actions.Count);
            for (int i = 0; i < collectionCount; i += 3)
            {
                Assert.AreEqual(StartCollectionMessage, actions[i + 0]);

                Assert.AreEqual(CollectMessage, actions[i + 1]);
                Assert.AreEqual(StopCollectionMessage, actions[i + 2]);
            }
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerFlipFlopsBetweenCollectAndIdle()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };

            var actions = new List<string>();
            var manager = CreateManager(serviceClient, new Clock(), actions);

            // enter collect state
            manager.UpdateState("empty iKey", string.Empty);

            actions.Clear();

            // ACT
            int collectionCount = 10;
            for (int i = 0; i < collectionCount; i++)
            {
                if (i % 2 == 0)
                {
                    // collect -> idle
                    serviceClient.ReturnValueFromPing = false;
                    serviceClient.ReturnValueFromSubmitSample = false;
                }
                else
                {
                    // idle -> collect
                    serviceClient.ReturnValueFromPing = true;
                    serviceClient.ReturnValueFromSubmitSample = true;
                }

                manager.UpdateState("empty iKey", string.Empty);
            }

            // ASSERT
            Assert.AreEqual(true, manager.IsCollectingData);

            Assert.AreEqual(1.5 * collectionCount, actions.Count);
            for (int i = 0; i < 1.5 * collectionCount; i += 3)
            {
                Assert.AreEqual(CollectMessage, actions[i + 0]);
                Assert.AreEqual(StopCollectionMessage, actions[i + 1]);
                Assert.AreEqual(StartCollectionMessage, actions[i + 2]);
            }
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerReturnsFailedSamplesBack()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var actions = new List<string>();
            var returnedSamples = new List<QuickPulseDataSample>();
            var manager = CreateManager(serviceClient, new Clock(), actions, returnedSamples);

            // turn on collection
            manager.UpdateState("empty iKey", string.Empty);

            // ACT
            // lost connection
            serviceClient.ReturnValueFromSubmitSample = null;

            manager.UpdateState("empty iKey", string.Empty);

            // ASSERT
            Assert.AreEqual(1, returnedSamples.Count);
            Assert.AreEqual(5, returnedSamples[0].AIRequestsSucceededPerSecond);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerPingBacksOff()
        {
            // ARRANGE
            var timings = QuickPulseTimings.Default;
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = false, ReturnValueFromSubmitSample = false };
            var actions = new List<string>();
            var returnedSamples = new List<QuickPulseDataSample>();
            var timeProvider = new ClockMock();
            var manager = CreateManager(serviceClient, timeProvider, actions, returnedSamples, timings);

            // ACT & ASSERT
            Assert.AreEqual(timings.ServicePollingInterval, manager.UpdateState("some ikey", string.Empty));

            serviceClient.ReturnValueFromPing = null;
            timeProvider.FastForward(timings.TimeToServicePollingBackOff.Add(TimeSpan.FromSeconds(-1)));
            Assert.AreEqual(timings.ServicePollingInterval, manager.UpdateState("some ikey", string.Empty));

            timeProvider.FastForward(TimeSpan.FromSeconds(2));
            Assert.AreEqual(timings.ServicePollingBackedOffInterval, manager.UpdateState("some ikey", string.Empty));
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerPingBacksOffWhenConnectionInitiallyDown()
        {
            // ARRANGE
            var timings = QuickPulseTimings.Default;
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = null, ReturnValueFromSubmitSample = null };
            var actions = new List<string>();
            var returnedSamples = new List<QuickPulseDataSample>();
            var timeProvider = new ClockMock();
            var manager = CreateManager(serviceClient, timeProvider, actions, returnedSamples, timings);

            // ACT & ASSERT
            manager.UpdateState("some ikey", string.Empty);

            timeProvider.FastForward(timings.TimeToServicePollingBackOff.Add(TimeSpan.FromSeconds(-1)));
            Assert.AreEqual(timings.ServicePollingInterval, manager.UpdateState("some ikey", string.Empty));

            timeProvider.FastForward(TimeSpan.FromSeconds(2));
            Assert.AreEqual(timings.ServicePollingBackedOffInterval, manager.UpdateState("some ikey", string.Empty));
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerPingDoesNotBackOffOnFirstPing()
        {
            // ARRANGE
            var timings = QuickPulseTimings.Default;
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = false, ReturnValueFromSubmitSample = false };
            var actions = new List<string>();
            var returnedSamples = new List<QuickPulseDataSample>();
            var timeProvider = new ClockMock();
            var manager = CreateManager(serviceClient, timeProvider, actions, returnedSamples, timings);

            // ACT
            serviceClient.ReturnValueFromPing = null;
            timeProvider.FastForward(timings.TimeToServicePollingBackOff.Add(TimeSpan.FromSeconds(1)));

            // ASSERT
            Assert.AreEqual(timings.ServicePollingInterval, manager.UpdateState(string.Empty, string.Empty));
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerPingRecovers()
        {
            // ARRANGE
            var timings = QuickPulseTimings.Default;
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = false, ReturnValueFromSubmitSample = false };
            var actions = new List<string>();
            var returnedSamples = new List<QuickPulseDataSample>();
            var timeProvider = new ClockMock();
            var manager = CreateManager(serviceClient, timeProvider, actions, returnedSamples, timings);

            Assert.AreEqual(timings.ServicePollingInterval, manager.UpdateState(string.Empty, string.Empty));

            // ACT
            serviceClient.ReturnValueFromPing = null;
            timeProvider.FastForward(timings.TimeToServicePollingBackOff.Add(TimeSpan.FromSeconds(1)));
            manager.UpdateState(string.Empty, string.Empty);

            timeProvider.FastForward(TimeSpan.FromMinutes(1));
            serviceClient.ReturnValueFromPing = false;

            // ASSERT
            Assert.AreEqual(timings.ServicePollingInterval, manager.UpdateState(string.Empty, string.Empty));
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerSubmitBacksOff()
        {
            // ARRANGE
            var timings = QuickPulseTimings.Default;
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };
            var actions = new List<string>();
            var returnedSamples = new List<QuickPulseDataSample>();
            var timeProvider = new ClockMock();
            var manager = CreateManager(serviceClient, timeProvider, actions, returnedSamples, timings);

            manager.UpdateState(string.Empty, string.Empty);

            // ACT & ASSERT
            Assert.AreEqual(timings.CollectionInterval, manager.UpdateState("some ikey", string.Empty));

            serviceClient.ReturnValueFromSubmitSample = null;
            timeProvider.FastForward(timings.TimeToCollectionBackOff.Add(TimeSpan.FromSeconds(-1)));
            Assert.AreEqual(timings.CollectionInterval, manager.UpdateState("some ikey", string.Empty));
            Assert.AreEqual(true, manager.IsCollectingData);

            timeProvider.FastForward(TimeSpan.FromSeconds(2));
            Assert.AreEqual(timings.ServicePollingBackedOffInterval, manager.UpdateState("some ikey", string.Empty));
            Assert.AreEqual(false, manager.IsCollectingData);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerSubmitBacksOffWhenConnectionInitiallyDown()
        {
            // ARRANGE
            var timings = QuickPulseTimings.Default;
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = null };
            var actions = new List<string>();
            var returnedSamples = new List<QuickPulseDataSample>();
            var timeProvider = new ClockMock();
            var manager = CreateManager(serviceClient, timeProvider, actions, returnedSamples, timings);

            // ACT & ASSERT
            Assert.AreEqual(timings.CollectionInterval, manager.UpdateState("some ikey", string.Empty));
            Assert.IsTrue(manager.IsCollectingData);

            timeProvider.FastForward(timings.TimeToCollectionBackOff.Add(TimeSpan.FromSeconds(-1)));
            Assert.AreEqual(timings.CollectionInterval, manager.UpdateState("some ikey", string.Empty));
            Assert.IsTrue(manager.IsCollectingData);

            timeProvider.FastForward(TimeSpan.FromSeconds(2));
            Assert.AreEqual(timings.ServicePollingBackedOffInterval, manager.UpdateState("some ikey", string.Empty));
            Assert.AreEqual(false, manager.IsCollectingData);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerUpdatesCollectionConfigurationWhenNoConfigurationPreviously()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };

            var actions = new List<string>();
            var collectionConfigurationInfos = new List<CollectionConfigurationInfo>();
            var manager = CreateManager(serviceClient, new Clock(), actions, collectionConfigurationInfos: collectionConfigurationInfos);

            var filters = new[]
                              {
                                  new FilterConjunctionGroupInfo()
                                      {
                                          Filters =
                                              new[]
                                                  {
                                                      new FilterInfo()
                                                          {
                                                              FieldName = "Name",
                                                              Predicate = Predicate.Equal,
                                                              Comparand = "Request1"
                                                          }
                                                  }
                                      }
                              };
            var metrics = new[]
                              {
                                  new CalculatedMetricInfo()
                                      {
                                          Id = "Metric0",
                                          TelemetryType = TelemetryType.Request,
                                          Projection = "Name",
                                          Aggregation = AggregationType.Avg,
                                          FilterGroups = filters
                                      },
                                  new CalculatedMetricInfo()
                                      {
                                          Id = "Metric1",
                                          TelemetryType = TelemetryType.Request,
                                          Projection = "Id",
                                          Aggregation = AggregationType.Sum,
                                          FilterGroups = filters
                                      }
                              };
            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "1", Metrics = metrics };

            // ACT
            manager.UpdateState("empty iKey", string.Empty);

            // ASSERT
            CollectionConfigurationInfo receivedCollectionConfigurationInfo = collectionConfigurationInfos.Single();
            Assert.AreEqual("1", receivedCollectionConfigurationInfo.ETag);
            Assert.AreEqual(2, receivedCollectionConfigurationInfo.Metrics.Length);

            Assert.AreEqual(metrics[0].ToString(), receivedCollectionConfigurationInfo.Metrics[0].ToString());
            Assert.AreEqual(metrics[0].Id, receivedCollectionConfigurationInfo.Metrics[0].Id);

            Assert.AreEqual(metrics[1].ToString(), receivedCollectionConfigurationInfo.Metrics[1].ToString());
            Assert.AreEqual(metrics[1].Id, receivedCollectionConfigurationInfo.Metrics[1].Id);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerUpdatesCollectionConfigurationWhenETagChanges()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };

            var actions = new List<string>();
            var collectionConfigurationInfos = new List<CollectionConfigurationInfo>();
            var manager = CreateManager(serviceClient, new Clock(), actions, collectionConfigurationInfos: collectionConfigurationInfos);

            var filters = new[]
            {
                new FilterConjunctionGroupInfo()
                {
                    Filters = new[] { new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Request1" } }
                }
            };
            var metrics = new[]
                              {
                                  new CalculatedMetricInfo()
                                      {
                                          Id = "Metric0",
                                          TelemetryType = TelemetryType.Request,
                                          Projection = "Name",
                                          Aggregation = AggregationType.Avg,
                                          FilterGroups = filters
                                      },
                                  new CalculatedMetricInfo()
                                      {
                                          Id = "Metric1",
                                          TelemetryType = TelemetryType.Request,
                                          Projection = "Id",
                                          Aggregation = AggregationType.Sum,
                                          FilterGroups = filters
                                      }
                              };
            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "1", Metrics = metrics };

            manager.UpdateState("empty iKey", string.Empty);
            collectionConfigurationInfos.Clear();

            // ACT
            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "2", Metrics = metrics };
            manager.UpdateState("empty iKey", string.Empty);

            // ASSERT
            CollectionConfigurationInfo receivedCollectionConfigurationInfo = collectionConfigurationInfos.Single();
            Assert.AreEqual("2", receivedCollectionConfigurationInfo.ETag);
            Assert.AreEqual(2, receivedCollectionConfigurationInfo.Metrics.Length);

            Assert.AreEqual(metrics[0].ToString(), receivedCollectionConfigurationInfo.Metrics[0].ToString());
            Assert.AreEqual(metrics[0].Id, receivedCollectionConfigurationInfo.Metrics[0].Id);

            Assert.AreEqual(metrics[1].ToString(), receivedCollectionConfigurationInfo.Metrics[1].ToString());
            Assert.AreEqual(metrics[1].Id, receivedCollectionConfigurationInfo.Metrics[1].Id);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerDoesNotUpdateCollectionConfigurationWhenETagIsTheSame()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };

            var actions = new List<string>();
            var collectionConfigurationInfos = new List<CollectionConfigurationInfo>();
            var manager = CreateManager(serviceClient, new Clock(), actions, collectionConfigurationInfos: collectionConfigurationInfos);

            var filters = new[] { new FilterConjunctionGroupInfo { Filters = new[] { new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Request1" } } } };
            var metrics = new[]
                              {
                                  new CalculatedMetricInfo()
                                      {
                                          Id = "Metric0",
                                          TelemetryType = TelemetryType.Request,
                                          Projection = "Name",
                                          Aggregation = AggregationType.Avg,
                                          FilterGroups = filters
                                      },
                                  new CalculatedMetricInfo()
                                      {
                                          Id = "Metric1",
                                          TelemetryType = TelemetryType.Request,
                                          Projection = "Id",
                                          Aggregation = AggregationType.Sum,
                                          FilterGroups = filters
                                      }
                              };
            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "1", Metrics = metrics };

            manager.UpdateState("empty iKey", string.Empty);
            collectionConfigurationInfos.Clear();

            // ACT
            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "1", Metrics = new CalculatedMetricInfo[0] };
            manager.UpdateState("empty iKey", string.Empty);

            // ASSERT
            Assert.AreEqual(0, collectionConfigurationInfos.Count);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerReportsErrorsInCollectionConfiguration()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = true, ReturnValueFromSubmitSample = true };

            var actions = new List<string>();
            var collectionConfigurationInfos = new List<CollectionConfigurationInfo>();
            var manager = CreateManager(serviceClient, new Clock(), actions, collectionConfigurationInfos: collectionConfigurationInfos);

            var filter = new FilterInfo() { FieldName = "NonExistentNameInFilter", Predicate = Predicate.Equal, Comparand = "Request1" };
            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "Metric0",
                    TelemetryType = TelemetryType.Request,
                    Projection = "NoneExistentNameInProjection",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filter, filter } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "Metric1",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Sum,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filter } } }
                }
            };

            // ACT
            serviceClient.CollectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "1", Metrics = metrics };

            // ping
            manager.UpdateState("empty iKey", string.Empty);

            // post
            manager.UpdateState("empty iKey", string.Empty);

            // ASSERT
            CollectionConfigurationError[] errors = serviceClient.CollectionConfigurationErrors;
            Assert.AreEqual(4, errors.Length);

            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors[0].ErrorType);
            Assert.AreEqual("Failed to create a filter NonExistentNameInFilter Equal Request1.", errors[0].Message);
            Assert.IsTrue(
                errors[0].FullException.Contains(
                    "Error finding property NonExistentNameInFilter in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(5, errors[0].Data.Count);
            Assert.AreEqual("Metric0", errors[0].Data["MetricId"]);
            Assert.AreEqual("1", errors[0].Data["ETag"]);
            Assert.AreEqual("NonExistentNameInFilter", errors[0].Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Equal.ToString(), errors[0].Data["FilterPredicate"]);
            Assert.AreEqual("Request1", errors[0].Data["FilterComparand"]);

            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors[1].ErrorType);
            Assert.AreEqual("Failed to create a filter NonExistentNameInFilter Equal Request1.", errors[1].Message);
            Assert.IsTrue(
                errors[1].FullException.Contains(
                    "Error finding property NonExistentNameInFilter in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(5, errors[1].Data.Count);
            Assert.AreEqual("Metric0", errors[1].Data["MetricId"]);
            Assert.AreEqual("1", errors[1].Data["ETag"]);
            Assert.AreEqual("NonExistentNameInFilter", errors[1].Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Equal.ToString(), errors[1].Data["FilterPredicate"]);
            Assert.AreEqual("Request1", errors[1].Data["FilterComparand"]);

            Assert.AreEqual(CollectionConfigurationErrorType.MetricFailureToCreate, errors[2].ErrorType);
            Assert.AreEqual(
                "Failed to create metric Id: 'Metric0', TelemetryType: 'Request', Projection: 'NoneExistentNameInProjection', Aggregation: 'Avg', FilterGroups: [NonExistentNameInFilter Equal Request1, NonExistentNameInFilter Equal Request1].",
                errors[2].Message);
            Assert.IsTrue(errors[2].FullException.Contains("Could not construct the projection"));
            Assert.AreEqual(2, errors[2].Data.Count);
            Assert.AreEqual("Metric0", errors[2].Data["MetricId"]);
            Assert.AreEqual("1", errors[2].Data["ETag"]);

            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors[3].ErrorType);
            Assert.AreEqual("Failed to create a filter NonExistentNameInFilter Equal Request1.", errors[3].Message);
            Assert.IsTrue(
                errors[3].FullException.Contains(
                    "Error finding property NonExistentNameInFilter in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(5, errors[3].Data.Count);
            Assert.AreEqual("Metric1", errors[3].Data["MetricId"]);
            Assert.AreEqual("1", errors[3].Data["ETag"]);
            Assert.AreEqual("NonExistentNameInFilter", errors[3].Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Equal.ToString(), errors[3].Data["FilterPredicate"]);
            Assert.AreEqual("Request1", errors[3].Data["FilterComparand"]);
        }

        [TestMethod]
        public void QuickPulseCollectionStateManagerRespectsServicePollingIntervalHint()
        {
            // ARRANGE
            var timings = QuickPulseTimings.Default;
            var serviceClient = new QuickPulseServiceClientMock { ReturnValueFromPing = false, ReturnValueFromSubmitSample = false };
            var actions = new List<string>();
            var returnedSamples = new List<QuickPulseDataSample>();
            var timeProvider = new ClockMock();
            var manager = CreateManager(serviceClient, timeProvider, actions, returnedSamples, timings);
            TimeSpan intervalHint1 = TimeSpan.FromSeconds(65);
            TimeSpan intervalHint2 = TimeSpan.FromSeconds(75);

            // ACT
            serviceClient.ReturnValueFromPing = false;

            TimeSpan oldServicePollingInterval = manager.UpdateState("ikey1", string.Empty);

            serviceClient.ServicePollingIntervalHint = intervalHint1;
            TimeSpan newServicePollingInterval1 = manager.UpdateState("ikey1", string.Empty);

            serviceClient.ServicePollingIntervalHint = intervalHint2;
            TimeSpan newServicePollingInterval2 = manager.UpdateState("ikey1", string.Empty);

            serviceClient.ServicePollingIntervalHint = null;
            TimeSpan newServicePollingInterval3 = manager.UpdateState("ikey1", string.Empty);

            // ASSERT
            Assert.AreEqual(timings.ServicePollingInterval, oldServicePollingInterval);
            Assert.AreEqual(intervalHint1, newServicePollingInterval1);
            Assert.AreEqual(intervalHint2, newServicePollingInterval2);
            Assert.AreEqual(intervalHint2, newServicePollingInterval3);
        }

        #region Helpers

        private static QuickPulseCollectionStateManager CreateManager(
            IQuickPulseServiceClient serviceClient,
            Clock timeProvider,
            List<string> actions,
            List<QuickPulseDataSample> returnedSamples = null,
            QuickPulseTimings timings = null,
            List<CollectionConfigurationInfo> collectionConfigurationInfos = null)
        {
            var manager = new QuickPulseCollectionStateManager(
                TelemetryConfiguration.CreateDefault(),
                serviceClient,
                timeProvider,
                timings ?? QuickPulseTimings.Default,
                () => actions.Add(StartCollectionMessage),
                () => actions.Add(StopCollectionMessage),
                () =>
                {
                    actions.Add(CollectMessage);

                    CollectionConfigurationError[] errors;
                    var now = DateTimeOffset.UtcNow;
                    return
                        new[]
                        {
                                new QuickPulseDataSample(
                                    new QuickPulseDataAccumulator(
                                        new CollectionConfiguration(EmptyCollectionConfigurationInfo, out errors, timeProvider))
                                    {
                                        AIRequestSuccessCount = 5,
                                        StartTimestamp = now,
                                        EndTimestamp = now.AddSeconds(1)
                                    },
                                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                                    Enumerable.Empty<Tuple<string, int>>(),
                                    false)
                        }.ToList();
                },
                samples =>
                {
                    returnedSamples?.AddRange(samples);
                },
                collectionConfigurationInfo =>
                {
                    actions.Add(UpdatedConfigurationMessage);
                    collectionConfigurationInfos?.Add(collectionConfigurationInfo);

                    CollectionConfigurationError[] errors;
                    new CollectionConfiguration(collectionConfigurationInfo, out errors, timeProvider);
                    return errors;
                },
                _ => { });

            return manager;
        }

        #endregion
    }
}
