// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if NET
using System.Text.Json;
#endif

using TestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

[TestClass]
public class JsonDataSerializerTests
{
    private readonly JsonDataSerializer _jsonDataSerializer;

    public JsonDataSerializerTests()
    {
        _jsonDataSerializer = JsonDataSerializer.Instance;
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    public void SerializePayloadShouldNotPickDefaultSettings(int version)
    {
        var classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(null);
        classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(classWithSelfReferencingLoop);
        classWithSelfReferencingLoop.InfiniteReference!.InfiniteReference = classWithSelfReferencingLoop;

        string serializedPayload = _jsonDataSerializer.SerializePayload("dummy", classWithSelfReferencingLoop, version);

        bool useNewtonsoft = (Environment.GetEnvironmentVariable("VSTEST_USE_NEWTONSOFT_JSON_SERIALIZER")?.Trim() ?? "0") != "0";
        if (useNewtonsoft)
        {
            // Newtonsoft.Json with ReferenceLoopHandling.Ignore omits the circular property entirely,
            // producing only one nesting level before the empty object.
            if (version <= 1)
            {
                Assert.AreEqual("""{"MessageType":"dummy","Payload":{"InfiniteReference":{}}}""", serializedPayload);
            }
            else
            {
                var expected = "{\"Version\":" + version + ",\"MessageType\":\"dummy\",\"Payload\":{\"InfiniteReference\":{}}}";
                Assert.AreEqual(expected, serializedPayload);
            }
        }
        else
        {
            // System.Text.Json with ReferenceHandler.IgnoreCycles writes null for circular references
            if (version <= 1)
            {
                Assert.AreEqual("""{"MessageType":"dummy","Payload":{"InfiniteReference":{"InfiniteReference":null}}}""", serializedPayload);
            }
            else
            {
                var expected = $$$$"""{"Version":{{{{version}}}},"MessageType":"dummy","Payload":{"InfiniteReference":{"InfiniteReference":null}}}""";
                Assert.AreEqual(expected, serializedPayload);
            }
        }
    }

    [TestMethod]
    public void DeserializeMessageShouldNotPickDefaultSettings()
    {
        Message message = _jsonDataSerializer.DeserializeMessage("{\"MessageType\":\"dummy\",\"Payload\":{\"InfiniteReference\":{}}}");
        Assert.AreEqual("dummy", message?.MessageType);
    }


    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]

    public void SerializePayloadIsUnaffectedByJsonConverterDefaultSettings(int version)
    {
        var completeArgs = new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero);
        var payload = new TestRunCompletePayload { TestRunCompleteArgs = completeArgs };

        var withDefaultSettingUpdated = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, payload, version);

        Assert.IsNotNull(withDefaultSettingUpdated);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    public void DeserializePayloadIsUnaffectedByJsonConverterDefaultSettings(int version)
    {
        // This line should deserialize properly
        Message message = _jsonDataSerializer.DeserializeMessage($"{{\"Version\":\"{version}\",\"MessageType\":\"dummy\",\"Payload\":{{\"InfiniteReference\":{{}}}}}}");

        Assert.IsNotNull(message);
    }


    [TestMethod]
    public void SerializePayloadShouldSerializeAnObjectWithSelfReferencingLoop()
    {
        var classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(null);
        classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(classWithSelfReferencingLoop);
        classWithSelfReferencingLoop.InfiniteReference!.InfiniteReference = classWithSelfReferencingLoop;

        // This line should not throw exception
        _jsonDataSerializer.SerializePayload("dummy", classWithSelfReferencingLoop);
    }

    [TestMethod]
    public void DeserializeShouldDeserializeAnObjectWhichHadSelfReferencingLoopBeforeSerialization()
    {
        var classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(null);
        classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(classWithSelfReferencingLoop);
        classWithSelfReferencingLoop.InfiniteReference!.InfiniteReference = classWithSelfReferencingLoop;

        var json = _jsonDataSerializer.SerializePayload("dummy", classWithSelfReferencingLoop);

        // This line should deserialize properly
        // System.Text.Json with IgnoreCycles serializes the cyclic reference as null,
        // so the deserialized object has InfiniteReference.InfiniteReference = null
        var result = _jsonDataSerializer.Deserialize<ClassWithSelfReferencingLoop>(json, 1)!;
        Assert.AreEqual(typeof(ClassWithSelfReferencingLoop), result.GetType());
    }

    [TestMethod]
    public void CloneShouldReturnNullForNull()
    {
        var clonedTestCase = _jsonDataSerializer.Clone<TestCase>(null!);

        Assert.IsNull(clonedTestCase);
    }

    [TestMethod]
    public void CloneShouldWorkForValueType()
    {
        var i = 2;
        var clonedI = _jsonDataSerializer.Clone(i);

        Assert.AreEqual(clonedI, i);
    }

    [TestMethod]
    public void CloneShouldCloneTestCaseObject()
    {
        var testCase = GetSampleTestCase(out var expectedTrait);

        var clonedTestCase = _jsonDataSerializer.Clone(testCase)!;

        VerifyTestCaseClone(clonedTestCase, testCase, expectedTrait);
    }

    [TestMethod]
    public void CloneShouldCloneTestResultsObject()
    {
        var testCase = GetSampleTestCase(out var expectedTrait);

        var testResult = new TestResult(testCase);

        var startTime = DateTimeOffset.UtcNow;
        testResult.StartTime = startTime;

        var clonedTestResult = _jsonDataSerializer.Clone(testResult)!;

        Assert.IsFalse(ReferenceEquals(testResult, clonedTestResult));

        Assert.AreEqual(testResult.StartTime, clonedTestResult.StartTime);

        VerifyTestCaseClone(testResult.TestCase, clonedTestResult.TestCase, expectedTrait);
    }

    private static TestCase GetSampleTestCase(out Trait expectedTrait)
    {
        var testCase = new TestCase("x.y.z", new Uri("uri://dummy"), "x.dll");

        expectedTrait = new Trait("TraitName1", "TraitValue1");

        testCase.Traits.Add(expectedTrait);
        return testCase;
    }

    private static void VerifyTestCaseClone(TestCase clonedTestCase, TestCase testCase, Trait expectedTrait)
    {
        Assert.IsFalse(ReferenceEquals(clonedTestCase, testCase));

        Assert.AreEqual(testCase.FullyQualifiedName, clonedTestCase.FullyQualifiedName);
        Assert.IsFalse(ReferenceEquals(testCase.FullyQualifiedName, clonedTestCase.FullyQualifiedName));

        Assert.AreEqual(testCase.ExecutorUri, clonedTestCase.ExecutorUri);
        Assert.IsFalse(ReferenceEquals(testCase.ExecutorUri, clonedTestCase.ExecutorUri));

        Assert.AreEqual(testCase.Source, clonedTestCase.Source);
        Assert.IsFalse(ReferenceEquals(testCase.Source, clonedTestCase.Source));

        Assert.AreEqual(1, clonedTestCase.Traits.Count());

        foreach (var trait in clonedTestCase.Traits)
        {
            Assert.IsFalse(ReferenceEquals(expectedTrait, trait));
            Assert.AreEqual(expectedTrait.Name, trait.Name);
            Assert.AreEqual(expectedTrait.Value, trait.Value);
        }
    }

    public class ClassWithSelfReferencingLoop
    {
        public ClassWithSelfReferencingLoop()
        {
        }

        public ClassWithSelfReferencingLoop(ClassWithSelfReferencingLoop? ir)
        {
            InfiniteReference = ir;
        }

        public ClassWithSelfReferencingLoop? InfiniteReference { get; set; }
    }
}
