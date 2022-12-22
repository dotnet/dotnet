namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using CompareLogic = KellermanSoftware.CompareNetObjects.CompareLogic;

    [TestClass]
    public class ExceptionTelemetryTest
    {
        [TestMethod]
        public void VerifyExpectedDefaultValue()
        {
            var exceptionTelemetry = new ExceptionTelemetry();
            Assert.AreEqual(SamplingDecision.None, exceptionTelemetry.ProactiveSamplingDecision);
            Assert.AreEqual(SamplingTelemetryItemTypes.Exception, exceptionTelemetry.ItemTypeFlag);
        }

        [TestMethod]
        public void ClassIsPublicAndCanBeUsedByCustomersDirectly()
        {
            Assert.IsTrue(typeof(ExceptionTelemetry).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void ExceptionTelemetryImplementsITelemetryContract()
        {
            var test = new ITelemetryTest<ExceptionTelemetry, AI.ExceptionData>();
            test.Run();
        }

        [TestMethod]
        public void ExceptionTelemetryReturnsNonNullContext()
        {
            ExceptionTelemetry item = new ExceptionTelemetry();
            Assert.IsNotNull(item.Context);
        }

        [TestMethod]
        public void ExceptionTelemetryCreatedBasedOnCustomData()
        {
            // ARRANGE
            var topLevelexceptionDetails = new ExceptionDetailsInfo(1, -1, "TopLevelException", "Top level exception",
                true, "Top level exception stack", new[]
                {
                    new StackFrame("Some.Assembly", "SomeFile.dll", 3, 33, "TopLevelMethod"),
                    new StackFrame("Some.Assembly", "SomeOtherFile.dll", 2, 22, "LowerLevelMethod"),
                    new StackFrame("Some.Assembly", "YetAnotherFile.dll", 1, 11, "LowLevelMethod")
                });

            var innerExceptionDetails = new ExceptionDetailsInfo(2, 1, "InnerException", "Inner exception", false,
                "Inner exception stack", new[]
                {
                    new StackFrame("Some.Assembly", "ImportantFile.dll", 2, 22, "InnerMethod"),
                    new StackFrame("Some.Assembly", "LessImportantFile.dll", 1, 11, "DeeperInnerMethod")
                });

            // ACT
            ExceptionTelemetry item = new ExceptionTelemetry(new[] {topLevelexceptionDetails, innerExceptionDetails},
                SeverityLevel.Error, "ProblemId",
                new Dictionary<string, string>() {["property1"] = "value1", ["property2"] = "value2"},
                new Dictionary<string, double>() {["property1"] = 1, ["property2"] = 2});

            item.ExceptionDetailsInfoList[1].Message = "Inner exception modified";
            item.ProblemId = "ProblemId modified";

            // ASSERT
            // use internal fields to validate
            Assert.AreEqual("Top level exception <--- Inner exception modified", item.Message);

            Assert.AreEqual(item.Data.Data.ver, 2);
            Assert.AreEqual(item.Data.Data.problemId, "ProblemId modified");
            Assert.AreEqual(item.Data.Data.severityLevel, Extensibility.Implementation.External.SeverityLevel.Error);

            Assert.AreEqual(item.Data.Data.properties.Count, 2);
            Assert.IsTrue(item.Data.Data.properties.Keys.Contains("property1"));
            Assert.IsTrue(item.Data.Data.properties.Keys.Contains("property2"));
            Assert.IsTrue(item.Data.Data.properties.Values.Contains("value1"));
            Assert.IsTrue(item.Data.Data.properties.Values.Contains("value2"));

            Assert.AreEqual(item.Data.Data.measurements.Count, 2);
            Assert.IsTrue(item.Data.Data.measurements.Keys.Contains("property1"));
            Assert.IsTrue(item.Data.Data.measurements.Keys.Contains("property2"));
            Assert.IsTrue(item.Data.Data.measurements.Values.Contains(1));
            Assert.IsTrue(item.Data.Data.measurements.Values.Contains(2));

            Assert.AreEqual(item.Data.Data.exceptions.Count, 2);

            Assert.AreEqual(item.Data.Data.exceptions.First().id, 1);
            Assert.AreEqual(item.Data.Data.exceptions.First().outerId, -1);
            Assert.AreEqual(item.Data.Data.exceptions.First().typeName, "TopLevelException");
            Assert.AreEqual(item.Data.Data.exceptions.First().message, "Top level exception");
            Assert.AreEqual(item.Data.Data.exceptions.First().hasFullStack, true);
            Assert.AreEqual(item.Data.Data.exceptions.First().stack, "Top level exception stack");
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack.Count, 3);

            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[0].assembly, "Some.Assembly");
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[0].fileName, "SomeFile.dll");
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[0].level, 3);
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[0].line, 33);
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[0].method, "TopLevelMethod");

            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[1].assembly, "Some.Assembly");
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[1].fileName, "SomeOtherFile.dll");
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[1].level, 2);
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[1].line, 22);
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[1].method, "LowerLevelMethod");

            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[2].assembly, "Some.Assembly");
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[2].fileName, "YetAnotherFile.dll");
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[2].level, 1);
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[2].line, 11);
            Assert.AreEqual(item.Data.Data.exceptions.First().parsedStack[2].method, "LowLevelMethod");

            Assert.AreEqual(item.Data.Data.exceptions.Last().id, 2);
            Assert.AreEqual(item.Data.Data.exceptions.Last().outerId, 1);
            Assert.AreEqual(item.Data.Data.exceptions.Last().typeName, "InnerException");
            Assert.AreEqual(item.Data.Data.exceptions.Last().message, "Inner exception modified");
            Assert.AreEqual(item.Data.Data.exceptions.Last().hasFullStack, false);
            Assert.AreEqual(item.Data.Data.exceptions.Last().stack, "Inner exception stack");
            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack.Count, 2);

            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack[0].assembly, "Some.Assembly");
            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack[0].fileName, "ImportantFile.dll");
            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack[0].level, 2);
            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack[0].line, 22);
            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack[0].method, "InnerMethod");

            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack[1].assembly, "Some.Assembly");
            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack[1].fileName, "LessImportantFile.dll");
            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack[1].level, 1);
            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack[1].line, 11);
            Assert.AreEqual(item.Data.Data.exceptions.Last().parsedStack[1].method, "DeeperInnerMethod");
        }

        [TestMethod]
        public void ExceptionTelemetryCreatedBasedOnCustomDataConstructsFakeExceptionCorrectly()
        {
            // ARRANGE
            var topLevelexceptionDetails = new ExceptionDetailsInfo(1, -1, "TopLevelException", "Top level exception",
                true, "Top level exception stack", new[]
                {
                    new StackFrame("Some.Assembly", "SomeFile.dll", 3, 33, "TopLevelMethod"),
                    new StackFrame("Some.Assembly", "SomeOtherFile.dll", 2, 22, "LowerLevelMethod"),
                    new StackFrame("Some.Assembly", "YetAnotherFile.dll", 1, 11, "LowLevelMethod")
                });

            var innerExceptionDetails = new ExceptionDetailsInfo(2, 1, "InnerException", "Inner exception", false,
                "Inner exception stack", new[]
                {
                    new StackFrame("Some.Assembly", "ImportantFile.dll", 2, 22, "InnerMethod"),
                    new StackFrame("Some.Assembly", "LessImportantFile.dll", 1, 11, "DeeperInnerMethod")
                });

            var innerInnerExceptionDetails = new ExceptionDetailsInfo(3, 1, "InnerInnerException", "Inner inner exception", false,
                "Inner inner exception stack", new[]
                {
                    new StackFrame("Some.Assembly", "ImportantInnerFile.dll", 2, 22, "InnerInnerMethod"),
                    new StackFrame("Some.Assembly", "LessImportantInnerFile.dll", 1, 11, "DeeperInnerInnerMethod")
                });

            ExceptionTelemetry item1 = new ExceptionTelemetry(new[] { topLevelexceptionDetails, innerExceptionDetails, innerInnerExceptionDetails },
                SeverityLevel.Error, "ProblemId",
                new Dictionary<string, string>() { ["property1"] = "value1", ["property2"] = "value2" },
                new Dictionary<string, double>() { ["property1"] = 1, ["property2"] = 2 });

            ExceptionTelemetry item2 = new ExceptionTelemetry(new[] { topLevelexceptionDetails, innerExceptionDetails },
                SeverityLevel.Error, "ProblemId",
                new Dictionary<string, string>() { ["property1"] = "value1", ["property2"] = "value2" },
                new Dictionary<string, double>() { ["property1"] = 1, ["property2"] = 2 });

            ExceptionTelemetry item3 = new ExceptionTelemetry(new[] { topLevelexceptionDetails },
                SeverityLevel.Error, "ProblemId",
                new Dictionary<string, string>() { ["property1"] = "value1", ["property2"] = "value2" },
                new Dictionary<string, double>() { ["property1"] = 1, ["property2"] = 2 });

            ExceptionTelemetry item4 = new ExceptionTelemetry(new ExceptionDetailsInfo[] { },
                SeverityLevel.Error, "ProblemId",
                new Dictionary<string, string>() { ["property1"] = "value1", ["property2"] = "value2" },
                new Dictionary<string, double>() { ["property1"] = 1, ["property2"] = 2 });

            // ACT
            Exception exception1 = item1.Exception;
            Exception exception2 = item2.Exception;
            Exception exception3 = item3.Exception;
            Exception exception4 = item4.Exception;

            // ASSERT
            Assert.AreEqual("Top level exception", exception1.Message);
            Assert.AreEqual("Inner exception", exception1.InnerException.Message);
            Assert.AreEqual("Inner inner exception", exception1.InnerException.InnerException.Message);
            Assert.IsNull(exception1.InnerException.InnerException.InnerException);

            Assert.AreEqual("Top level exception", exception2.Message);
            Assert.AreEqual("Inner exception", exception2.InnerException.Message);
            Assert.IsNull(exception2.InnerException.InnerException);

            Assert.AreEqual("Top level exception", exception3.Message);
            Assert.IsNull(exception3.InnerException);

            Assert.AreEqual(string.Empty, exception4.Message);
            Assert.IsNull(exception4.InnerException);
        }

        [TestMethod]
        public void ExceptionTelemetryExceptionDetailsUpdate()
        {
            // ARRANGE
            var exception = new AggregateException("Test Exception", new Exception());
            ExceptionTelemetry item = new ExceptionTelemetry(exception);

            // ACT
            IReadOnlyList<ExceptionDetailsInfo> newExceptionDetails = item.ExceptionDetailsInfoList;

            string modifiedMessage = "Modified Message";
            string modifiedTypeName = "Modified TypeName";

            newExceptionDetails[0].Message = modifiedMessage;
            newExceptionDetails[0].TypeName = modifiedTypeName;

            // ASSERT
            Assert.AreEqual(modifiedMessage, item.Exceptions[0].message);
            Assert.AreEqual(modifiedTypeName, item.Exceptions[0].typeName);
        }

        [TestMethod]
        public void ExceptionsPropertyIsInternalUntilWeSortOutPublicInterface()
        {
            Assert.IsFalse(typeof(ExceptionTelemetry).GetTypeInfo().GetDeclaredProperty("Exceptions").GetGetMethod(true).IsPublic);
        }

        [TestMethod]
        public void ConstructorAddsExceptionToExceptionPropertyAndExceptionsCollectionProperty()
        {
            Exception constructorException = new Exception("ConstructorException");
            var testExceptionTelemetry = new ExceptionTelemetry(constructorException);

            Assert.AreSame(constructorException, testExceptionTelemetry.Exception);
            Assert.AreEqual(constructorException.Message, testExceptionTelemetry.Exceptions.First().message);
            Assert.AreEqual(constructorException.Message, testExceptionTelemetry.ExceptionDetailsInfoList.First().Message);
        }

        [TestMethod]
        public void ExceptionPropertySetterReplacesExceptionDetailsInExceptionsCollectionProperty()
        {
            Exception constructorException = new Exception("ConstructorException");
            var testExceptionTelemetry = new ExceptionTelemetry(constructorException);

            Exception nextException = new Exception("NextException");
            testExceptionTelemetry.Exception = nextException;

            Assert.AreSame(nextException, testExceptionTelemetry.Exception);
            Assert.AreEqual(nextException.Message, testExceptionTelemetry.Exceptions.First().message);
            Assert.AreEqual(nextException.Message, testExceptionTelemetry.ExceptionDetailsInfoList.First().Message);
        }

#pragma warning disable 618
        [TestMethod]
        public void HandledAtReturnsUnhandledByDefault()
        {
            var telemetry = new ExceptionTelemetry();
            Assert.AreEqual(ExceptionHandledAt.Unhandled, telemetry.HandledAt);
        }
#pragma warning restore 618

        [TestMethod]
        public void ConstructorDoesNotSetSeverityLevel()
        {
            var telemetry = new ExceptionTelemetry();
            Assert.AreEqual(null, telemetry.SeverityLevel);
        }

        [TestMethod]
        public void MetricsReturnsEmptyDictionaryByDefaultToPreventNullReferenceExceptions()
        {
            var @exception = new ExceptionTelemetry(new Exception());
            var measurements = @exception.Metrics;
            Assert.IsNotNull(measurements);
        }

        [TestMethod]
        public void SerializeWritesNullValuesAsExpectedByEndpoint()
        {
            ExceptionTelemetry original = new ExceptionTelemetry();
            original.Exception = null;
            original.SeverityLevel = null;
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(original);

            Assert.AreEqual(2, item.data.baseData.ver);
        }

        [TestMethod]
        public void SerializeWritesItemVersionAsExpectedByEndpoint()
        {
            ExceptionTelemetry original = CreateExceptionTelemetry();
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(original);

            Assert.AreEqual(2, item.data.baseData.ver);
        }

        [TestMethod]
        public void SerializeUsesExceptionMessageIfTelemetryMessageNotProvided()
        {
            ExceptionTelemetry original = CreateExceptionTelemetry(new ArgumentException("Test"));
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(original);

            Assert.AreEqual("Test", item.data.baseData.exceptions[0].message);
        }

        [TestMethod]
        public void SerializeTelemetryMessageAsOuterExceptionMessage()
        {
            ExceptionTelemetry original = CreateExceptionTelemetry(new ArgumentException("Test"));
            original.Message = "Custom";
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(original);

            Assert.AreEqual("Custom", item.data.baseData.exceptions[0].message);
        }

        [TestMethod]
        public void SerializeUsesExceptionMessageForInnerExceptions()
        {
            Exception outerException = new ArgumentException("Outer", new Exception("Inner"));
            ExceptionTelemetry original = CreateExceptionTelemetry(outerException);

            original.Message = "Custom";
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(original);

            Assert.AreEqual("Custom", item.data.baseData.exceptions[0].message);
            Assert.AreEqual("Inner", item.data.baseData.exceptions[1].message);
        }

        [TestMethod]
        public void SerializeUsesExceptionMessageForInnerAggregateExceptions()
        {
            Exception innerException1 = new ArgumentException("Inner1");
            Exception innerException2 = new ArgumentException("Inner2");

            AggregateException aggregateException = new AggregateException("AggregateException", new [] {innerException1, innerException2});

            ExceptionTelemetry original = CreateExceptionTelemetry(aggregateException);

            original.Message = "Custom";
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(original);

            Assert.AreEqual("Custom", item.data.baseData.exceptions[0].message);
            Assert.AreEqual("Inner1", item.data.baseData.exceptions[1].message);
            Assert.AreEqual("Inner2", item.data.baseData.exceptions[2].message);
        }

        [TestMethod]
        public void SerializeWritesItemSeverityLevelAsExpectedByEndpoint()
        {
            ExceptionTelemetry original = CreateExceptionTelemetry();
            original.SeverityLevel = SeverityLevel.Information;
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(original);

            Assert.AreEqual(AI.SeverityLevel.Information, item.data.baseData.severityLevel.Value);
        }

        [TestMethod]
        public void SerializeWritesExceptionTypeNameAsExpectedByEndpoint()
        {
            var exception = new Exception();
            ExceptionTelemetry original = CreateExceptionTelemetry(exception);
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(original);

            Assert.AreEqual(exception.GetType().FullName, item.data.baseData.exceptions[0].typeName);
        }

        [TestMethod]
        public void SerializeWritesExceptionMessageAsExpectedByEndpoint()
        {
            var exception = new Exception("Test Message");
            ExceptionTelemetry original = CreateExceptionTelemetry(exception);
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(original);

            Assert.AreEqual(exception.Message, item.data.baseData.exceptions[0].message);
        }

        [TestMethod]
        public void SerializeWritesDataBaseTypeAsExpectedByEndpoint()
        {
            ExceptionTelemetry original = CreateExceptionTelemetry();
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(original);
            Assert.AreEqual(nameof(AI.ExceptionData), item.data.baseType);
        }

        [TestMethod]
        public void SerializeWritesRootExceptionWithoutOuterId()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var exception = new Exception();
                ExceptionTelemetry original = CreateExceptionTelemetry(exception);
                byte[] serializedTelemetryAsBytes = JsonSerializer.Serialize(original, compress: false);
                string serializedTelemetry = Encoding.UTF8.GetString(serializedTelemetryAsBytes, 0, serializedTelemetryAsBytes.Length);

                AssertEx.DoesNotContain("\"outerId\":", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void SerializeWritesInnerExceptionAsAdditionalItemInExceptionsArrayExpectedByEndpoint()
        {
            var innerException = new Exception("Inner Message");
            var exception = new Exception("Root Message", innerException);
            ExceptionTelemetry expected = CreateExceptionTelemetry(exception);

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(expected);

            Assert.AreEqual(innerException.Message, item.data.baseData.exceptions[1].message);
        }

        [TestMethod]
        public void SerializeWritesInnerExceptionWithOuterIdLinkingItToItsParentException()
        {
            var innerException = new Exception();
            var exception = new Exception("Test Exception", innerException);
            ExceptionTelemetry expected = CreateExceptionTelemetry(exception);

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(expected);

            Assert.AreEqual(exception.GetHashCode(), item.data.baseData.exceptions[1].outerId);
        }

        [TestMethod]
        public void SerializeWritesAggregateExceptionAsFirstItemInExceptionsArrayExpectedByEndpoint()
        {
            var exception = new AggregateException();
            ExceptionTelemetry expected = CreateExceptionTelemetry(exception);

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(expected);

            Assert.AreEqual(1, item.data.baseData.exceptions.Count);
        }

        [TestMethod]
        public void SerializeWritesInnerExceptionsOfAggregateExceptionAsAdditionalItemsInExceptionsArrayExpectedByEndpoint()
        {
            var exception = new AggregateException("Test Exception", new[] { new Exception(), new Exception() });
            ExceptionTelemetry expected = CreateExceptionTelemetry(exception);

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(expected);

            Assert.AreEqual(exception.GetHashCode(), item.data.baseData.exceptions[1].outerId);
            Assert.AreEqual(exception.GetHashCode(), item.data.baseData.exceptions[2].outerId);
        }

        [TestMethod]
        public void SerializeWritesHasFullStackPropertyAsItIsExpectedByEndpoint()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var exception = CreateExceptionWithStackTrace();
                ExceptionTelemetry expected = CreateExceptionTelemetry(exception);

                var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(expected);

                Assert.IsTrue(item.data.baseData.exceptions[0].hasFullStack);
            }
        }

        [TestMethod]
        public void SerializeWritesSingleInnerExceptionOfAggregateExceptionOnlyOnce()
        {
            var exception = new AggregateException("Test Exception", new Exception());

            ExceptionTelemetry expected = CreateExceptionTelemetry(exception);
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(expected);

            Assert.AreEqual(2, item.data.baseData.exceptions.Count);
        }

        [TestMethod]
        public void SerializeWritesPropertiesAsExpectedByEndpoint()
        {
            ExceptionTelemetry expected = CreateExceptionTelemetry();            

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(expected);

            // IExtension is currently flattened into the properties by serialization
            Utils.CopyDictionary(((MyTestExtension)expected.Extension).SerializeIntoDictionary(), expected.Properties);

            AssertEx.AreEqual(expected.Properties.ToArray(), item.data.baseData.properties.ToArray());
        }

        [TestMethod]
        public void SerializeWritesMetricsAsExpectedByEndpoint()
        {
            ExceptionTelemetry expected = CreateExceptionTelemetry();
            expected.Metrics.Add("TestMetric", 4.2);

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(expected);

            AssertEx.AreEqual(expected.Metrics.ToArray(), item.data.baseData.measurements.ToArray());
        }

        [TestMethod]
        public void SerializePopulatesRequiredFieldsOfExceptionTelemetry()
        {
            var exceptionTelemetry = new ExceptionTelemetry();
            exceptionTelemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(exceptionTelemetry);

            Assert.AreEqual(2, item.data.baseData.ver);
            Assert.IsNotNull(item.data.baseData.exceptions);
            Assert.AreEqual(0, item.data.baseData.exceptions.Count); // constructor without parameters does not initialize exception object
        }

        [TestMethod]
        public void ExceptionPropertySetterHandlesAggregateExceptionsWithMultipleNestedExceptionsCorrectly()
        {
            Exception exception1121 = new Exception("1.1.2.1");
            Exception exception111 = new Exception("1.1.1");
            Exception exception112 = new Exception("1.1.2", exception1121);
            AggregateException exception11 = new AggregateException("1.1", exception111, exception112);
            Exception exception121 = new Exception("1.2.1");
            Exception exception12 = new Exception("1.2", exception121);
            AggregateException rootLevelException = new AggregateException("1", exception11, exception12);

            ExceptionTelemetry telemetry = new ExceptionTelemetry { Exception = rootLevelException };

            string[] expectedSequence = new string[]
                                            {
                                                "1",
                                                "1.1",
                                                "1.1.1",
                                                "1.1.2",
                                                "1.1.2.1",
                                                "1.2",
                                                "1.2.1"
                                            };

            Assert.AreEqual(expectedSequence.Length, telemetry.Exceptions.Count);
            Assert.AreEqual(expectedSequence.Length, telemetry.ExceptionDetailsInfoList.Count);
            for(int counter = 0; counter < expectedSequence.Length; counter++)
            {
                ExceptionDetails details = telemetry.Exceptions[counter];
                ExceptionDetailsInfo newExceptionDetails = telemetry.ExceptionDetailsInfoList[counter];
                Assert.IsTrue(ReferenceEquals(details, newExceptionDetails.InternalExceptionDetails));
                if (details.typeName == "System.AggregateException")
                {
                    AssertEx.StartsWith(expectedSequence[counter], details.message);
                }
                else
                {
                    Assert.AreEqual(expectedSequence[counter], details.message);
                }
            }
        }

        [TestMethod]
        public void ExceptionPropertySetterHandlesAggregateExceptionsWithMultipleNestedExceptionsAndTrimsAfterReachingMaxCount()
        {
            const int Overage = 5;
            List<Exception> innerExceptions = new List<Exception>();
            for (int i = 0; i < Constants.MaxExceptionCountToSave + Overage; i++)
            {
                innerExceptions.Add(new Exception((i + 1).ToString(CultureInfo.InvariantCulture)));
            }

            AggregateException rootLevelException = new AggregateException("0", innerExceptions);

            ExceptionTelemetry telemetry = new ExceptionTelemetry { Exception = rootLevelException };

            Assert.AreEqual(Constants.MaxExceptionCountToSave + 1, telemetry.Exceptions.Count);
            Assert.AreEqual(Constants.MaxExceptionCountToSave + 1, telemetry.ExceptionDetailsInfoList.Count);
            for(int counter = 0; counter < Constants.MaxExceptionCountToSave; counter++)
            {
                ExceptionDetails details = telemetry.Exceptions[counter];
                ExceptionDetailsInfo newExceptionDetails = telemetry.ExceptionDetailsInfoList[counter];
                Assert.IsTrue(ReferenceEquals(details, newExceptionDetails.InternalExceptionDetails));
                if (details.typeName == "System.AggregateException")
                {
                    AssertEx.StartsWith(counter.ToString(CultureInfo.InvariantCulture), details.message);
                }
                else
                {
                    Assert.AreEqual(counter.ToString(CultureInfo.InvariantCulture), details.message);
                }
            }

            ExceptionDetails first = telemetry.Exceptions.First();
            ExceptionDetails last = telemetry.Exceptions.Last();
            Assert.AreEqual(first.id, last.outerId);
            Assert.AreEqual(typeof(InnerExceptionCountExceededException).FullName, last.typeName);
            Assert.AreEqual(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "The number of inner exceptions was {0} which is larger than {1}, the maximum number allowed during transmission. All but the first {1} have been dropped.",
                    1 + Constants.MaxExceptionCountToSave + Overage,
                    Constants.MaxExceptionCountToSave),
                last.message);
        }

        [TestMethod]
        public void SanitizeWillTrimMessageInExceptionTelemetry()
        {
            ExceptionTelemetry exceptionTelemetry = new ExceptionTelemetry();
            exceptionTelemetry.Message = new string('M', Property.MaxMessageLength) + 'M';

            ((ITelemetry)exceptionTelemetry).Sanitize();

            Assert.AreEqual(new string('M', Property.MaxMessageLength), exceptionTelemetry.Message);
        }

        [TestMethod]
        public void SanitizeWillTrimPropertiesKeyAndValueInExceptionTelemetry()
        {
            ExceptionTelemetry telemetry = new ExceptionTelemetry();
            telemetry.Properties.Add(new string('X', Property.MaxDictionaryNameLength) + 'X', new string('X', Property.MaxValueLength + 1));
            telemetry.Properties.Add(new string('X', Property.MaxDictionaryNameLength) + 'Y', new string('X', Property.MaxValueLength + 1));

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual(2, telemetry.Properties.Count);
            var t = new SortedList<string, string>(telemetry.Properties);

            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength), t.Keys.ToArray()[1]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), t.Values.ToArray()[1]);
            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength - 3) + "1", t.Keys.ToArray()[0]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), t.Values.ToArray()[0]);
        }

        [TestMethod]
        public void SanitizeWillTrimMetricsNameAndValueInExceptionTelemetry()
        {
            ExceptionTelemetry telemetry = new ExceptionTelemetry();
            telemetry.Metrics.Add(new string('Y', Property.MaxDictionaryNameLength) + 'X', 42.0);
            telemetry.Metrics.Add(new string('Y', Property.MaxDictionaryNameLength) + 'Y', 42.0);

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual(2, telemetry.Metrics.Count);
            string[] keys = telemetry.Metrics.Keys.OrderBy(s => s).ToArray();
            Assert.AreEqual(new string('Y', Property.MaxDictionaryNameLength), keys[1]);
            Assert.AreEqual(new string('Y', Property.MaxDictionaryNameLength - 3) + "1", keys[0]);
        }

        [TestMethod]
        public void ExceptionTelemetryImplementsISupportSamplingContract()
        {
            var telemetry = new ExceptionTelemetry();

            Assert.IsNotNull(telemetry as ISupportSampling);
        }

        [TestMethod]
        public void ExceptionTelemetryImplementsISupportAdvancedSamplingContract()
        {
            var telemetry = new ExceptionTelemetry();

            Assert.IsNotNull(telemetry as ISupportAdvancedSampling);
        }

        [TestMethod]
        public void ExceptionTelemetryHasCorrectValueOfSamplingPercentageAfterSerialization()
        {
            var telemetry = new ExceptionTelemetry();
            ((ISupportSampling)telemetry).SamplingPercentage = 10;

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(telemetry);

            Assert.AreEqual(10, item.sampleRate);
        }

        [TestMethod]
        public void ExceptionTelemetryDeepCloneCopiesAllProperties()
        {
            var telemetry = CreateExceptionTelemetry(CreateExceptionWithStackTrace());
            var other = telemetry.DeepClone();

            CompareLogic deepComparator = new CompareLogic();

            var result = deepComparator.Compare(telemetry, other);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod]
        public void ExceptionTelemetryDeepCloneWithNullExtensionDoesNotThrow()
        {
            var telemetry = new ExceptionTelemetry();
            // Extension is not set, means it'll be null.
            // Validate that cloning with null Extension does not throw.
            var other = telemetry.DeepClone();
        }

        [TestMethod]
        public void ExceptionTelemetryPropertiesFromContextAndItemSerializesToPropertiesInJson()
        {
            var expected = CreateExceptionTelemetry();
            ((ITelemetry)expected).Sanitize();

            Assert.AreEqual(1, expected.Properties.Count);
            Assert.AreEqual(1, expected.Context.GlobalProperties.Count);

            Assert.IsTrue(expected.Properties.ContainsKey("TestProperty"));
            Assert.IsTrue(expected.Context.GlobalProperties.ContainsKey("TestPropertyGlobal"));

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.ExceptionData>(expected);

            // Items added to both Exception.Properties, and Exception.Context.Properties are serialized to properties.
            // IExtension object in CreateExceptionTelemetry adds 2 more properties: myIntField and myStringField
            Assert.AreEqual(4, item.data.baseData.properties.Count);            
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestProperty"));
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestPropertyGlobal"));
        }

        private static Exception CreateExceptionWithStackTrace()
        {
            try
            {
                throw new Exception();
            }
            catch (Exception e)
            {
                return e;
            }
        }

        private static ExceptionTelemetry CreateExceptionTelemetry(Exception exception = null)
        {
            if (exception == null)
            {
                exception = new Exception();
            }

            ExceptionTelemetry output = new ExceptionTelemetry(exception) { Timestamp = DateTimeOffset.UtcNow };
            output.Context.GlobalProperties.Add("TestPropertyGlobal", "contextpropvalue");
            output.Context.InstrumentationKey = "required";
            output.Properties.Add("TestProperty", "TestPropertyValue");
            output.Extension = new MyTestExtension() { myIntField = 42, myStringField = "value" };
            return output;
        }
    }
}
