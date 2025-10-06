namespace Microsoft.ApplicationInsights.Tests
{
    using System.Linq;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DocumentStreamTests
    {
        [TestMethod]
        public void DocumentStreamHandlesNoFiltersCorrectly()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var documentStreamInfo = new DocumentStreamInfo() { DocumentFilterGroups = new DocumentFilterConjunctionGroupInfo[0] };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var request = new RequestTelemetry() { Id = "apple" };

            // ACT
            CollectionConfigurationError[] runtimeErrors;
            bool result = documentStream.CheckFilters(request, out runtimeErrors);
            
            // ASSERT
            Assert.AreEqual(0, errors.Length);
            Assert.AreEqual(0, runtimeErrors.Length);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DocumentStreamHandlesNullFiltersCorrectly()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var documentStreamInfo = new DocumentStreamInfo() { DocumentFilterGroups = null };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var request = new RequestTelemetry() { Id = "apple" };

            // ACT
            CollectionConfigurationError[] runtimeErrors;
            bool result = documentStream.CheckFilters(request, out runtimeErrors);

            // ASSERT
            Assert.AreEqual(0, errors.Length);
            Assert.AreEqual(0, runtimeErrors.Length);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DocumentStreamFiltersRequestsCorrectly()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            FilterInfo filterApple = new FilterInfo { FieldName = "Id", Predicate = Predicate.Contains, Comparand = "apple" };
            FilterInfo filterOrange = new FilterInfo { FieldName = "Id", Predicate = Predicate.Contains, Comparand = "orange" };
            FilterInfo filterMango = new FilterInfo { FieldName = "Id", Predicate = Predicate.Contains, Comparand = "mango" };

            // (apple AND orange) OR mango
            var documentStreamInfo = new DocumentStreamInfo()
            {
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterApple, filterOrange } }
                        },
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterMango } }
                        }
                    }
            };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var requests = new[]
            {
                new RequestTelemetry() { Id = "apple" }, new RequestTelemetry() { Id = "orange" }, new RequestTelemetry() { Id = "mango" },
                new RequestTelemetry() { Id = "apple orange" }, new RequestTelemetry() { Id = "apple mango" },
                new RequestTelemetry() { Id = "orange mango" }, new RequestTelemetry() { Id = "apple orange mango" },
                new RequestTelemetry() { Id = "none of the above" }
            };

            // ACT
            var results = new bool[requests.Length];
            bool errorsEncountered = false;
            for (int i = 0; i < requests.Length; i++)
            {
                CollectionConfigurationError[] runtimeErrors;
                results[i] = documentStream.CheckFilters(requests[i], out runtimeErrors);
                if (runtimeErrors.Any())
                {
                    errorsEncountered = true;
                }
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            Assert.IsFalse(errorsEncountered);

            Assert.IsFalse(results[0]);
            Assert.IsFalse(results[1]);
            Assert.IsTrue(results[2]);
            Assert.IsTrue(results[3]);
            Assert.IsTrue(results[4]);
            Assert.IsTrue(results[5]);
            Assert.IsTrue(results[6]);
            Assert.IsFalse(results[7]);
        }

        [TestMethod]
        public void DocumentStreamFiltersDependenciesCorrectly()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            FilterInfo filterApple = new FilterInfo { FieldName = "Id", Predicate = Predicate.Contains, Comparand = "apple" };
            FilterInfo filterOrange = new FilterInfo { FieldName = "Id", Predicate = Predicate.Contains, Comparand = "orange" };
            FilterInfo filterMango = new FilterInfo { FieldName = "Id", Predicate = Predicate.Contains, Comparand = "mango" };

            // (apple AND orange) OR mango
            var documentStreamInfo = new DocumentStreamInfo()
            {
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Dependency,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterApple, filterOrange } }
                        },
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Dependency,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterMango } }
                        }
                    }
            };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var dependencies = new[]
            {
                new DependencyTelemetry() { Id = "apple" }, new DependencyTelemetry() { Id = "orange" }, new DependencyTelemetry() { Id = "mango" },
                new DependencyTelemetry() { Id = "apple orange" }, new DependencyTelemetry() { Id = "apple mango" },
                new DependencyTelemetry() { Id = "orange mango" }, new DependencyTelemetry() { Id = "apple orange mango" },
                new DependencyTelemetry() { Id = "none of the above" }
            };

            // ACT
            var results = new bool[dependencies.Length];
            bool errorsEncountered = false;
            for (int i = 0; i < dependencies.Length; i++)
            {
                CollectionConfigurationError[] runtimeErrors;
                results[i] = documentStream.CheckFilters(dependencies[i], out runtimeErrors);
                if (runtimeErrors.Any())
                {
                    errorsEncountered = true;
                }
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            Assert.IsFalse(errorsEncountered);

            Assert.IsFalse(results[0]);
            Assert.IsFalse(results[1]);
            Assert.IsTrue(results[2]);
            Assert.IsTrue(results[3]);
            Assert.IsTrue(results[4]);
            Assert.IsTrue(results[5]);
            Assert.IsTrue(results[6]);
            Assert.IsFalse(results[7]);
        }

        [TestMethod]
        public void DocumentStreamFiltersExceptionsCorrectly()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            FilterInfo filterApple = new FilterInfo { FieldName = "Message", Predicate = Predicate.Contains, Comparand = "apple" };
            FilterInfo filterOrange = new FilterInfo { FieldName = "Message", Predicate = Predicate.Contains, Comparand = "orange" };
            FilterInfo filterMango = new FilterInfo { FieldName = "Message", Predicate = Predicate.Contains, Comparand = "mango" };

            // (apple AND orange) OR mango
            var documentStreamInfo = new DocumentStreamInfo()
            {
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterApple, filterOrange } }
                        },
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterMango } }
                        }
                    }
            };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var exceptions = new[]
            {
                new ExceptionTelemetry() { Message = "apple" }, new ExceptionTelemetry() { Message = "orange" },
                new ExceptionTelemetry() { Message = "mango" }, new ExceptionTelemetry() { Message = "apple orange" },
                new ExceptionTelemetry() { Message = "apple mango" }, new ExceptionTelemetry() { Message = "orange mango" },
                new ExceptionTelemetry() { Message = "apple orange mango" }, new ExceptionTelemetry() { Message = "none of the above" }
            };

            // ACT
            var results = new bool[exceptions.Length];
            bool errorsEncountered = false;
            for (int i = 0; i < exceptions.Length; i++)
            {
                CollectionConfigurationError[] runtimeErrors;
                results[i] = documentStream.CheckFilters(exceptions[i], out runtimeErrors);
                if (runtimeErrors.Any())
                {
                    errorsEncountered = true;
                }
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            Assert.IsFalse(errorsEncountered);

            Assert.IsFalse(results[0]);
            Assert.IsFalse(results[1]);
            Assert.IsTrue(results[2]);
            Assert.IsTrue(results[3]);
            Assert.IsTrue(results[4]);
            Assert.IsTrue(results[5]);
            Assert.IsTrue(results[6]);
            Assert.IsFalse(results[7]);
        }

        [TestMethod]
        public void DocumentStreamFiltersEventsCorrectly()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            FilterInfo filterApple = new FilterInfo { FieldName = "Name", Predicate = Predicate.Contains, Comparand = "apple" };
            FilterInfo filterOrange = new FilterInfo { FieldName = "Name", Predicate = Predicate.Contains, Comparand = "orange" };
            FilterInfo filterMango = new FilterInfo { FieldName = "Name", Predicate = Predicate.Contains, Comparand = "mango" };

            // (apple AND orange) OR mango
            var documentStreamInfo = new DocumentStreamInfo()
            {
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Event,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterApple, filterOrange } }
                        },
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Event,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterMango } }
                        }
                    }
            };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var events = new[]
            {
                new EventTelemetry() { Name = "apple" }, new EventTelemetry() { Name = "orange" }, new EventTelemetry() { Name = "mango" },
                new EventTelemetry() { Name = "apple orange" }, new EventTelemetry() { Name = "apple mango" },
                new EventTelemetry() { Name = "orange mango" }, new EventTelemetry() { Name = "apple orange mango" },
                new EventTelemetry() { Name = "none of the above" }
            };

            // ACT
            var results = new bool[events.Length];
            bool errorsEncountered = false;
            for (int i = 0; i < events.Length; i++)
            {
                CollectionConfigurationError[] runtimeErrors;
                results[i] = documentStream.CheckFilters(events[i], out runtimeErrors);
                if (runtimeErrors.Any())
                {
                    errorsEncountered = true;
                }
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            Assert.IsFalse(errorsEncountered);

            Assert.IsFalse(results[0]);
            Assert.IsFalse(results[1]);
            Assert.IsTrue(results[2]);
            Assert.IsTrue(results[3]);
            Assert.IsTrue(results[4]);
            Assert.IsTrue(results[5]);
            Assert.IsTrue(results[6]);
            Assert.IsFalse(results[7]);
        }

        [TestMethod]
        public void DocumentStreamReportsErrorsDuringCreation()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            FilterInfo filterApple = new FilterInfo { FieldName = "NonExistentField1", Predicate = Predicate.Contains, Comparand = "apple" };
            FilterInfo filterOrange = new FilterInfo { FieldName = "NonExistentField2", Predicate = Predicate.Contains, Comparand = "orange" };
            FilterInfo filterMango = new FilterInfo { FieldName = "NonExistentField3", Predicate = Predicate.Contains, Comparand = "mango" };

            var documentStreamInfo = new DocumentStreamInfo()
            {
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterApple, filterOrange } }
                        },
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterMango } }
                        }
                    }
            };

            // ACT
            new DocumentStream(documentStreamInfo, out errors, new ClockMock());

            // ASSERT
            Assert.AreEqual(3, errors.Length);

            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors[0].ErrorType);
            Assert.AreEqual(
                "Failed to create a filter NonExistentField1 Contains apple.",
                errors[0].Message);
            Assert.IsTrue(errors[0].FullException.Contains("Error finding property NonExistentField1 in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(3, errors[0].Data.Count);
            Assert.AreEqual("NonExistentField1", errors[0].Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Contains.ToString(), errors[0].Data["FilterPredicate"]);
            Assert.AreEqual("apple", errors[0].Data["FilterComparand"]);

            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors[1].ErrorType);
            Assert.AreEqual(
                "Failed to create a filter NonExistentField2 Contains orange.",
                errors[1].Message);
            Assert.IsTrue(errors[1].FullException.Contains("Error finding property NonExistentField2 in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(3, errors[1].Data.Count);
            Assert.AreEqual("NonExistentField2", errors[1].Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Contains.ToString(), errors[1].Data["FilterPredicate"]);
            Assert.AreEqual("orange", errors[1].Data["FilterComparand"]);

            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors[2].ErrorType);
            Assert.AreEqual(
                "Failed to create a filter NonExistentField3 Contains mango.",
                errors[2].Message);
            Assert.IsTrue(errors[2].FullException.Contains("Error finding property NonExistentField3 in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(3, errors[2].Data.Count);
            Assert.AreEqual("NonExistentField3", errors[2].Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Contains.ToString(), errors[2].Data["FilterPredicate"]);
            Assert.AreEqual("mango", errors[2].Data["FilterComparand"]);
        }
    }
}