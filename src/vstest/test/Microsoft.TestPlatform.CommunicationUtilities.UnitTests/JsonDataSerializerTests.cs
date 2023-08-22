// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            PreserveReferencesHandling = PreserveReferencesHandling.All,
        };

        var classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(null);
        classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(classWithSelfReferencingLoop);
        classWithSelfReferencingLoop.InfiniteRefernce!.InfiniteRefernce = classWithSelfReferencingLoop;

        string serializedPayload = _jsonDataSerializer.SerializePayload("dummy", classWithSelfReferencingLoop, version);
        if (version <= 1)
        {
            Assert.AreEqual("{\"MessageType\":\"dummy\",\"Payload\":{\"InfiniteRefernce\":{}}}", serializedPayload);
        }
        else
        {
            Assert.AreEqual($"{{\"Version\":{version},\"MessageType\":\"dummy\",\"Payload\":{{\"InfiniteRefernce\":{{}}}}}}", serializedPayload);
        }

        JsonConvert.DefaultSettings = null;
    }

    [TestMethod]
    public void DeserializeMessageShouldNotPickDefaultSettings()
    {
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            PreserveReferencesHandling = PreserveReferencesHandling.All,
        };

        Message message = _jsonDataSerializer.DeserializeMessage("{\"MessageType\":\"dummy\",\"Payload\":{\"InfiniteRefernce\":{}}}");
        Assert.AreEqual("dummy", message?.MessageType);
        JsonConvert.DefaultSettings = null;
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

        Assert.IsNull(JsonConvert.DefaultSettings);
        //todo: how to check feature flag
        var completeArgs = new TestRunCompleteEventArgs(null, false, true, null, null, null, TimeSpan.Zero);
        var payload = new TestRunCompletePayload { TestRunCompleteArgs = completeArgs };

        JsonConvert.DefaultSettings = () =>
        {
            //restore the default settings to null
            JsonConvert.DefaultSettings = null;
            Assert.Fail("Should Not Access DefaultSettings");
            return new();
        };

        var withDefaultSettingUpdated = JsonDataSerializer.Instance.SerializePayload(MessageType.ExecutionComplete, payload, version);

        //restore the default settings to null
        JsonConvert.DefaultSettings = null;
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

        Assert.IsNull(JsonConvert.DefaultSettings, "If this is not null some other test didn't clean up its default setti0ngs");

        JsonConvert.DefaultSettings = () =>
        {
            //restore the default settings to null
            JsonConvert.DefaultSettings = null;
            Assert.Fail("Should Not Access DefaultSettings");
            return new();
        };

        // This line should deserialize properly
        Message message = _jsonDataSerializer.DeserializeMessage($"{{\"Version\":\"{version}\",\"MessageType\":\"dummy\",\"Payload\":{{\"InfiniteRefernce\":{{}}}}}}");


        //restore the default settings to null
        JsonConvert.DefaultSettings = null;
    }


    [TestMethod]
    public void SerializePayloadShouldSerializeAnObjectWithSelfReferencingLoop()
    {
        var classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(null);
        classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(classWithSelfReferencingLoop);
        classWithSelfReferencingLoop.InfiniteRefernce!.InfiniteRefernce = classWithSelfReferencingLoop;

        // This line should not throw exception
        _jsonDataSerializer.SerializePayload("dummy", classWithSelfReferencingLoop);
    }

    [TestMethod]
    public void DeserializeShouldDeserializeAnObjectWhichHadSelfReferencingLoopBeforeSerialization()
    {
        var classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(null);
        classWithSelfReferencingLoop = new ClassWithSelfReferencingLoop(classWithSelfReferencingLoop);
        classWithSelfReferencingLoop.InfiniteRefernce!.InfiniteRefernce = classWithSelfReferencingLoop;

        var json = _jsonDataSerializer.SerializePayload("dummy", classWithSelfReferencingLoop);

        // This line should deserialize properly
        var result = _jsonDataSerializer.Deserialize<ClassWithSelfReferencingLoop>(json, 1)!;

        Assert.AreEqual(typeof(ClassWithSelfReferencingLoop), result.GetType());
        Assert.IsNull(result.InfiniteRefernce);
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
        public ClassWithSelfReferencingLoop(ClassWithSelfReferencingLoop? ir)
        {
            InfiniteRefernce = ir;
        }

        public ClassWithSelfReferencingLoop? InfiniteRefernce { get; set; }
    }
}
