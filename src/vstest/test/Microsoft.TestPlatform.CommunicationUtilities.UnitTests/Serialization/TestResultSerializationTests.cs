// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TestResult= Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

[TestClass]
public class TestResultSerializationTests
{
    private static readonly TestCase TestCase = new(
        "sampleTestClass.sampleTestCase",
        new Uri("executor://sampleTestExecutor"),
        "sampleTest.dll");

    private static readonly DateTimeOffset StartTime = new(new DateTime(2007, 3, 10, 0, 0, 0, DateTimeKind.Utc));
    private static readonly TestResult TestResult = new(TestCase)
    {
        // Attachments = ?
        // Messages = ?
        Outcome = TestOutcome.Passed,
        ErrorMessage = "sampleError",
        ErrorStackTrace = "sampleStackTrace",
        DisplayName = "sampleTestResult",
        ComputerName = "sampleComputerName",
        Duration = TimeSpan.MaxValue,
        StartTime = StartTime,
        EndTime = DateTimeOffset.MaxValue
    };

    #region v1 serializer Tests (used with protocol 1 and accidentally with 3)

    [TestMethod]
    [DataRow(1)]
    [DataRow(3)]
    public void TestResultJsonShouldContainAllPropertiesOnSerialization(int version)
    {
        var json = Serialize(TestResult, version);

        // Use raw deserialization to validate basic properties (disable date parsing to keep raw strings)
        var data = ParseJsonNoDates(json);
        var properties = (JArray)data["Properties"]!;
        Assert.AreEqual("TestResult.Outcome", properties[0]!["Key"]!["Id"]!.ToString());
        Assert.AreEqual(1, (int)properties[0]!["Value"]!);
        Assert.AreEqual("TestResult.ErrorMessage", properties[1]!["Key"]!["Id"]!.ToString());
        Assert.AreEqual("sampleError", properties[1]!["Value"]!.ToString());
        Assert.AreEqual("TestResult.ErrorStackTrace", properties[2]!["Key"]!["Id"]!.ToString());
        Assert.AreEqual("sampleStackTrace", properties[2]!["Value"]!.ToString());
        Assert.AreEqual("TestResult.DisplayName", properties[3]!["Key"]!["Id"]!.ToString());
        Assert.AreEqual("sampleTestResult", properties[3]!["Value"]!.ToString());
        Assert.AreEqual("TestResult.ComputerName", properties[4]!["Key"]!["Id"]!.ToString());
        Assert.AreEqual("sampleComputerName", properties[4]!["Value"]!.ToString());
        Assert.AreEqual("TestResult.Duration", properties[5]!["Key"]!["Id"]!.ToString());
        Assert.AreEqual("10675199.02:48:05.4775807", properties[5]!["Value"]!.ToString());

        // By default json.net converts DateTimes to current time zone
        Assert.AreEqual("TestResult.StartTime", properties[6]!["Key"]!["Id"]!.ToString());
        Assert.AreEqual(StartTime.Year, DateTimeOffset.Parse(properties[6]!["Value"]!.ToString(), System.Globalization.CultureInfo.InvariantCulture).Year);
        Assert.AreEqual("TestResult.EndTime", properties[7]!["Key"]!["Id"]!.ToString());
        Assert.AreEqual(DateTimeOffset.MaxValue.Year, DateTimeOffset.Parse(properties[7]!["Value"]!.ToString(), System.Globalization.CultureInfo.InvariantCulture).Year);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(3)]
    public void TestResultObjectShouldContainAllPropertiesOnDeserialization(int version)
    {
        var json = "{\"TestCase\":{\"Properties\":[{\"Key\":{\"Id\":\"TestCase.FullyQualifiedName\",\"Label\":\"FullyQualifiedName\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"},{\"Key\":{\"Id\":\"TestCase.ExecutorUri\",\"Label\":\"Executor Uri\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Uri\"},\"Value\":\"executor://sampleTestExecutor\"},{\"Key\":{\"Id\":\"TestCase.Source\",\"Label\":\"Source\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTest.dll\"}]},\"Attachments\":[],\"Messages\":[],\"Properties\":[{\"Key\":{\"Id\":\"TestResult.Outcome\",\"Label\":\"Outcome\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome\"},\"Value\":1},{\"Key\":{\"Id\":\"TestResult.ErrorMessage\",\"Label\":\"Error Message\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleError\"},{\"Key\":{\"Id\":\"TestResult.ErrorStackTrace\",\"Label\":\"Error Stack Trace\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleStackTrace\"},{\"Key\":{\"Id\":\"TestResult.DisplayName\",\"Label\":\"TestResult Display Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestResult\"},{\"Key\":{\"Id\":\"TestResult.ComputerName\",\"Label\":\"Computer Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleComputerName\"},{\"Key\":{\"Id\":\"TestResult.Duration\",\"Label\":\"Duration\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.TimeSpan\"},\"Value\":\"10675199.02:48:05.4775807\"},{\"Key\":{\"Id\":\"TestResult.StartTime\",\"Label\":\"Start Time\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"2007-03-10T00:00:00+00:00\"},{\"Key\":{\"Id\":\"TestResult.EndTime\",\"Label\":\"End Time\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"9999-12-31T23:59:59.9999999+00:00\"}]}";

        var test = Deserialize<TestResult>(json, version);

        Assert.AreEqual(TestResult.TestCase.Id, test.TestCase.Id);
        Assert.HasCount(TestResult.Attachments.Count, test.Attachments);
        Assert.HasCount(TestResult.Messages.Count, test.Messages);

        Assert.AreEqual(TestResult.ComputerName, test.ComputerName);
        Assert.AreEqual(TestResult.DisplayName, test.DisplayName);
        Assert.AreEqual(TestResult.Duration, test.Duration);
        Assert.AreEqual(TestResult.EndTime, test.EndTime);
        Assert.AreEqual(TestResult.ErrorMessage, test.ErrorMessage);
        Assert.AreEqual(TestResult.ErrorStackTrace, test.ErrorStackTrace);
        Assert.AreEqual(TestResult.Outcome, test.Outcome);
        Assert.AreEqual(TestResult.StartTime, test.StartTime);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(3)]
    public void TestResultObjectShouldSerializeAttachments(int version)
    {
        var result = new TestResult(TestCase);
        result.StartTime = default;
        result.EndTime = default;
        result.Attachments.Add(new AttachmentSet(new Uri("http://dummyUri"), "sampleAttachment"));
        var expectedJson = "{\"TestCase\":{\"Properties\":[{\"Key\":{\"Id\":\"TestCase.FullyQualifiedName\",\"Label\":\"FullyQualifiedName\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"},{\"Key\":{\"Id\":\"TestCase.ExecutorUri\",\"Label\":\"Executor Uri\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Uri\"},\"Value\":\"executor://sampleTestExecutor\"},{\"Key\":{\"Id\":\"TestCase.Source\",\"Label\":\"Source\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTest.dll\"},{\"Key\":{\"Id\":\"TestCase.CodeFilePath\",\"Label\":\"File Path\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestCase.DisplayName\",\"Label\":\"Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"},{\"Key\":{\"Id\":\"TestCase.Id\",\"Label\":\"Id\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Guid\"},\"Value\":\"28e7a7ed-8fb9-05b7-5e90-4a8c52f32b5b\"},{\"Key\":{\"Id\":\"TestCase.LineNumber\",\"Label\":\"Line Number\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Int32\"},\"Value\":-1}]},\"Attachments\":[{\"Uri\":\"http://dummyUri\",\"DisplayName\":\"sampleAttachment\",\"Attachments\":[]}],\"Messages\":[],\"Properties\":[{\"Key\":{\"Id\":\"TestResult.Outcome\",\"Label\":\"Outcome\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome, Microsoft.VisualStudio.TestPlatform.ObjectModel, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\"},\"Value\":0},{\"Key\":{\"Id\":\"TestResult.ErrorMessage\",\"Label\":\"Error Message\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.ErrorStackTrace\",\"Label\":\"Error Stack Trace\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.DisplayName\",\"Label\":\"TestResult Display Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.ComputerName\",\"Label\":\"Computer Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"\"},{\"Key\":{\"Id\":\"TestResult.Duration\",\"Label\":\"Duration\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.TimeSpan\"},\"Value\":\"00:00:00\"},{\"Key\":{\"Id\":\"TestResult.StartTime\",\"Label\":\"Start Time\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"0001-01-01T00:00:00+00:00\"},{\"Key\":{\"Id\":\"TestResult.EndTime\",\"Label\":\"End Time\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"0001-01-01T00:00:00+00:00\"}]}";

        var json = Serialize(result, version);

        Assert.AreEqual(expectedJson, json);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(3)]
    public void TestResultObjectShouldDeserializeAttachments(int version)
    {
        var json = "{\"TestCase\":{\"Properties\":[{\"Key\":{\"Id\":\"TestCase.FullyQualifiedName\",\"Label\":\"FullyQualifiedName\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"},{\"Key\":{\"Id\":\"TestCase.ExecutorUri\",\"Label\":\"Executor Uri\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Uri\"},\"Value\":\"executor://sampleTestExecutor\"},{\"Key\":{\"Id\":\"TestCase.Source\",\"Label\":\"Source\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTest.dll\"}]},\"Attachments\":[{\"Uri\":\"http://dummyUri\",\"DisplayName\":\"sampleAttachment\",\"Attachments\":[]}],\"Messages\":[],\"Properties\":[]}";

        var result = Deserialize<TestResult>(json, version);

        Assert.HasCount(1, result.Attachments);
        Assert.AreEqual(new Uri("http://dummyUri"), result.Attachments[0].Uri);
        Assert.AreEqual("sampleAttachment", result.Attachments[0].DisplayName);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(3)]
    public void TestResultObjectShouldSerializeDefaultValues(int version)
    {
        var result = new TestResult(TestCase);
        result.StartTime = default;
        result.EndTime = default;
        var expectedJson = "{\"TestCase\":{\"Properties\":[{\"Key\":{\"Id\":\"TestCase.FullyQualifiedName\",\"Label\":\"FullyQualifiedName\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"},{\"Key\":{\"Id\":\"TestCase.ExecutorUri\",\"Label\":\"Executor Uri\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Uri\"},\"Value\":\"executor://sampleTestExecutor\"},{\"Key\":{\"Id\":\"TestCase.Source\",\"Label\":\"Source\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTest.dll\"},{\"Key\":{\"Id\":\"TestCase.CodeFilePath\",\"Label\":\"File Path\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestCase.DisplayName\",\"Label\":\"Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"},{\"Key\":{\"Id\":\"TestCase.Id\",\"Label\":\"Id\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Guid\"},\"Value\":\"28e7a7ed-8fb9-05b7-5e90-4a8c52f32b5b\"},{\"Key\":{\"Id\":\"TestCase.LineNumber\",\"Label\":\"Line Number\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Int32\"},\"Value\":-1}]},\"Attachments\":[],\"Messages\":[],\"Properties\":[{\"Key\":{\"Id\":\"TestResult.Outcome\",\"Label\":\"Outcome\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome, Microsoft.VisualStudio.TestPlatform.ObjectModel, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\"},\"Value\":0},{\"Key\":{\"Id\":\"TestResult.ErrorMessage\",\"Label\":\"Error Message\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.ErrorStackTrace\",\"Label\":\"Error Stack Trace\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.DisplayName\",\"Label\":\"TestResult Display Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.ComputerName\",\"Label\":\"Computer Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"\"},{\"Key\":{\"Id\":\"TestResult.Duration\",\"Label\":\"Duration\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.TimeSpan\"},\"Value\":\"00:00:00\"},{\"Key\":{\"Id\":\"TestResult.StartTime\",\"Label\":\"Start Time\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"0001-01-01T00:00:00+00:00\"},{\"Key\":{\"Id\":\"TestResult.EndTime\",\"Label\":\"End Time\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"0001-01-01T00:00:00+00:00\"}]}";

        var json = Serialize(result, version);

        // Values that should be null: DisplayName, ErrorMessage, ErrorStackTrace
        // Values that should be empty: ComputerName
        Assert.AreEqual(expectedJson, json);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(3)]
    public void TestResultObjectShouldDeserializeDefaultValues(int version)
    {
        var json = "{\"TestCase\":{\"Properties\":[{\"Key\":{\"Id\":\"TestCase.FullyQualifiedName\",\"Label\":\"FullyQualifiedName\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"},{\"Key\":{\"Id\":\"TestCase.ExecutorUri\",\"Label\":\"Executor Uri\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Uri\"},\"Value\":\"executor://sampleTestExecutor\"},{\"Key\":{\"Id\":\"TestCase.Source\",\"Label\":\"Source\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTest.dll\"},{\"Key\":{\"Id\":\"TestCase.CodeFilePath\",\"Label\":\"File Path\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestCase.DisplayName\",\"Label\":\"Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"},{\"Key\":{\"Id\":\"TestCase.Id\",\"Label\":\"Id\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Guid\"},\"Value\":\"28e7a7ed-8fb9-05b7-5e90-4a8c52f32b5b\"},{\"Key\":{\"Id\":\"TestCase.LineNumber\",\"Label\":\"Line Number\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Int32\"},\"Value\":-1}]},\"Attachments\":[],\"Messages\":[],\"Properties\":[{\"Key\":{\"Id\":\"TestResult.Outcome\",\"Label\":\"Outcome\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome, Microsoft.VisualStudio.TestPlatform.ObjectModel, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\"},\"Value\":0},{\"Key\":{\"Id\":\"TestResult.ErrorMessage\",\"Label\":\"Error Message\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.ErrorStackTrace\",\"Label\":\"Error Stack Trace\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.DisplayName\",\"Label\":\"TestResult Display Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.ComputerName\",\"Label\":\"Computer Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"\"},{\"Key\":{\"Id\":\"TestResult.Duration\",\"Label\":\"Duration\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.TimeSpan\"},\"Value\":\"00:00:00\"},{\"Key\":{\"Id\":\"TestResult.StartTime\",\"Label\":\"Start Time\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"0001-01-01T00:00:00+00:00\"},{\"Key\":{\"Id\":\"TestResult.EndTime\",\"Label\":\"End Time\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"0001-01-01T00:00:00+00:00\"}]}";

        var result = Deserialize<TestResult>(json, version);

        Assert.IsEmpty(result.Attachments);
        Assert.IsEmpty(result.Messages);
        Assert.IsNull(result.DisplayName);
        Assert.IsNull(result.ErrorMessage);
        Assert.IsNull(result.ErrorStackTrace);
        Assert.AreEqual(string.Empty, result.ComputerName);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(3)]
    public void TestResultPropertiesShouldGetRegisteredAsPartOfDeserialization(int version)
    {
        TestProperty.TryUnregister("DummyProperty", out var _);
        var json = "{\"TestCase\":{\"Properties\":[{\"Key\":{\"Id\":\"TestCase.FullyQualifiedName\",\"Label\":\"FullyQualifiedName\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"}," +
                   "{\"Key\":{\"Id\":\"TestCase.ExecutorUri\",\"Label\":\"Executor Uri\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Uri\"},\"Value\":\"executor://sampleTestExecutor\"}," +
                   "{\"Key\":{\"Id\":\"TestCase.Source\",\"Label\":\"Source\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTest.dll\"}," +
                   "{\"Key\":{\"Id\":\"TestCase.CodeFilePath\",\"Label\":\"File Path\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null}," +
                   "{\"Key\":{\"Id\":\"TestCase.DisplayName\",\"Label\":\"Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"}," +
                   "{\"Key\":{\"Id\":\"TestCase.Id\",\"Label\":\"Id\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Guid\"},\"Value\":\"28e7a7ed-8fb9-05b7-5e90-4a8c52f32b5b\"}," +
                   "{\"Key\":{\"Id\":\"TestCase.LineNumber\",\"Label\":\"Line Number\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Int32\"},\"Value\":-1}]}," +
                   "\"Attachments\":[],\"Messages\":[]," +
                   "\"Properties\":[{\"Key\":{\"Id\":\"TestResult.Outcome\",\"Label\":\"Outcome\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome, Microsoft.VisualStudio.TestPlatform.ObjectModel, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\"},\"Value\":0}," +
                   "{\"Key\":{\"Id\":\"TestResult.ErrorMessage\",\"Label\":\"Error Message\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.ErrorStackTrace\",\"Label\":\"Error Stack Trace\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null}," +
                   "{\"Key\":{\"Id\":\"TestResult.DisplayName\",\"Label\":\"TestResult Display Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":null},{\"Key\":{\"Id\":\"TestResult.ComputerName\",\"Label\":\"Computer Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"\"}," +
                   "{\"Key\":{\"Id\":\"TestResult.Duration\",\"Label\":\"Duration\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.TimeSpan\"},\"Value\":\"00:00:00\"},{\"Key\":{\"Id\":\"TestResult.StartTime\",\"Label\":\"Start Time\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"0001-01-01T00:00:00+00:00\"}," +
                   "{\"Key\":{\"Id\":\"DummyProperty\",\"Label\":\"DummyPropertyLabel\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":5,\"ValueType\":\"System.String\"},\"Value\":\"dummyString\"}," +
                   "{\"Key\":{\"Id\":\"TestResult.EndTime\",\"Label\":\"End Time\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"0001-01-01T00:00:00+00:00\"}]}";
        _ = Deserialize<TestResult>(json, version);

        VerifyDummyPropertyIsRegistered();
    }

    #endregion

    #region v2 serializer Tests (used with protocol 2 and 4)

    [TestMethod]
    [DataRow(2)]
    [DataRow(4)]
    public void TestResultJsonShouldContainAllPropertiesOnSerializationV2(int version)
    {
        var json = Serialize(TestResult, version);

        // Use raw deserialization to validate basic properties (disable date parsing to keep raw strings)
        var data = ParseJsonNoDates(json);

        Assert.AreEqual(1, (int)data["Outcome"]!);
        Assert.AreEqual("sampleError", data["ErrorMessage"]!.ToString());
        Assert.AreEqual("sampleStackTrace", data["ErrorStackTrace"]!.ToString());
        Assert.AreEqual("sampleTestResult", data["DisplayName"]!.ToString());
        Assert.AreEqual("sampleComputerName", data["ComputerName"]!.ToString());
        Assert.AreEqual("10675199.02:48:05.4775807", data["Duration"]!.ToString());

        // By default json.net converts DateTimes to current time zone
        Assert.AreEqual(StartTime.Year, DateTimeOffset.Parse(data["StartTime"]!.ToString(), System.Globalization.CultureInfo.InvariantCulture).Year);
        Assert.AreEqual(DateTimeOffset.MaxValue.Year, DateTimeOffset.Parse(data["EndTime"]!.ToString(), System.Globalization.CultureInfo.InvariantCulture).Year);
    }

    [TestMethod]
    [DataRow(2)]
    [DataRow(4)]
    public void TestResultObjectShouldContainAllPropertiesOnDeserializationV2(int version)
    {
        var json = "{\"TestCase\":{\"Id\":\"28e7a7ed-8fb9-05b7-5e90-4a8c52f32b5b\",\"FullyQualifiedName\":\"sampleTestClass.sampleTestCase\",\"DisplayName\":\"sampleTestClass.sampleTestCase\",\"ExecutorUri\":\"executor://sampleTestExecutor\",\"Source\":\"sampleTest.dll\",\"CodeFilePath\":null,\"LineNumber\":-1,\"Properties\":[]},\"Attachments\":[],\"Outcome\":1,\"ErrorMessage\":\"sampleError\",\"ErrorStackTrace\":\"sampleStackTrace\",\"DisplayName\":\"sampleTestResult\",\"Messages\":[],\"ComputerName\":\"sampleComputerName\",\"Duration\":\"10675199.02:48:05.4775807\",\"StartTime\":\"2007-03-10T00:00:00+00:00\",\"EndTime\":\"9999-12-31T23:59:59.9999999+00:00\",\"Properties\":[]}";

        var test = Deserialize<TestResult>(json, version);

        Assert.AreEqual(TestResult.TestCase.Id, test.TestCase.Id);
        Assert.HasCount(TestResult.Attachments.Count, test.Attachments);
        Assert.HasCount(TestResult.Messages.Count, test.Messages);

        Assert.AreEqual(TestResult.ComputerName, test.ComputerName);
        Assert.AreEqual(TestResult.DisplayName, test.DisplayName);
        Assert.AreEqual(TestResult.Duration, test.Duration);
        Assert.AreEqual(TestResult.EndTime, test.EndTime);
        Assert.AreEqual(TestResult.ErrorMessage, test.ErrorMessage);
        Assert.AreEqual(TestResult.ErrorStackTrace, test.ErrorStackTrace);
        Assert.AreEqual(TestResult.Outcome, test.Outcome);
        Assert.AreEqual(TestResult.StartTime, test.StartTime);
    }

    [TestMethod]
    [DataRow(2)]
    [DataRow(4)]
    public void TestResultObjectShouldSerializeAttachmentsV2(int version)
    {
        var result = new TestResult(TestCase);
        result.StartTime = default;
        result.EndTime = default;
        result.Attachments.Add(new AttachmentSet(new Uri("http://dummyUri"), "sampleAttachment"));
        var expectedJson = "{\"TestCase\":{\"Id\":\"28e7a7ed-8fb9-05b7-5e90-4a8c52f32b5b\",\"FullyQualifiedName\":\"sampleTestClass.sampleTestCase\",\"DisplayName\":\"sampleTestClass.sampleTestCase\",\"ExecutorUri\":\"executor://sampleTestExecutor\",\"Source\":\"sampleTest.dll\",\"CodeFilePath\":null,\"LineNumber\":-1,\"Properties\":[]},\"Attachments\":[{\"Uri\":\"http://dummyUri\",\"DisplayName\":\"sampleAttachment\",\"Attachments\":[]}],\"Outcome\":0,\"ErrorMessage\":null,\"ErrorStackTrace\":null,\"DisplayName\":null,\"Messages\":[],\"ComputerName\":null,\"Duration\":\"00:00:00\",\"StartTime\":\"0001-01-01T00:00:00+00:00\",\"EndTime\":\"0001-01-01T00:00:00+00:00\",\"Properties\":[]}";

        var json = Serialize(result, version);

        Assert.AreEqual(expectedJson, json);
    }

    [TestMethod]
    [DataRow(2)]
    [DataRow(4)]
    public void TestResultObjectShouldDeserializeAttachmentsV2(int version)
    {
        var json = "{\"TestCase\":{\"Id\":\"28e7a7ed-8fb9-05b7-5e90-4a8c52f32b5b\",\"FullyQualifiedName\":\"sampleTestClass.sampleTestCase\",\"DisplayName\":\"sampleTestClass.sampleTestCase\",\"ExecutorUri\":\"executor://sampleTestExecutor\",\"Source\":\"sampleTest.dll\",\"CodeFilePath\":null,\"LineNumber\":-1,\"Properties\":[]},\"Attachments\":[{\"Uri\":\"http://dummyUri\",\"DisplayName\":\"sampleAttachment\",\"Attachments\":[]}],\"Outcome\":0,\"ErrorMessage\":null,\"ErrorStackTrace\":null,\"DisplayName\":null,\"Messages\":[],\"ComputerName\":null,\"Duration\":\"00:00:00\",\"StartTime\":\"0001-01-01T00:00:00+00:00\",\"EndTime\":\"0001-01-01T00:00:00+00:00\",\"Properties\":[]}";

        var result = Deserialize<TestResult>(json, version);

        Assert.HasCount(1, result.Attachments);
        Assert.AreEqual(new Uri("http://dummyUri"), result.Attachments[0].Uri);
        Assert.AreEqual("sampleAttachment", result.Attachments[0].DisplayName);
    }

    [TestMethod]
    [DataRow(2)]
    [DataRow(4)]
    public void TestResultPropertiesShouldGetRegisteredAsPartOfDeserializationV2(int version)
    {
        TestProperty.TryUnregister("DummyProperty", out var _);
        var json = "{\"TestCase\":{\"Id\":\"28e7a7ed-8fb9-05b7-5e90-4a8c52f32b5b\",\"FullyQualifiedName\":\"sampleTestClass.sampleTestCase\",\"DisplayName\":\"sampleTestClass.sampleTestCase\",\"ExecutorUri\":\"executor://sampleTestExecutor\",\"Source\":\"sampleTest.dll\",\"CodeFilePath\":null,\"LineNumber\":-1,\"Properties\":[]},\"Attachments\":[],\"Outcome\":1,\"ErrorMessage\":\"sampleError\",\"ErrorStackTrace\":\"sampleStackTrace\",\"DisplayName\":\"sampleTestResult\",\"Messages\":[],\"ComputerName\":\"sampleComputerName\",\"Duration\":\"10675199.02:48:05.4775807\",\"StartTime\":\"2007-03-10T00:00:00+00:00\",\"EndTime\":\"9999-12-31T23:59:59.9999999+00:00\"," +
                   "\"Properties\":[{\"Key\":{\"Id\":\"DummyProperty\",\"Label\":\"DummyPropertyLabel\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":5,\"ValueType\":\"System.String\"},\"Value\":\"dummyString\"},]}";
        _ = Deserialize<TestResult>(json, version);

        VerifyDummyPropertyIsRegistered();
    }

    #endregion

    #region future

    [TestMethod]
    public void TestResultSerializationShouldThrowWhenProvidedProtocolVersionDoesNotExist()
    {
        // this is to ensure that introducing a new version is a conscious choice and
        // and that we don't fallback to version 1 as it happened with version 3, because the serializer
        // only checked for version 2
        var version = int.MaxValue;

        Assert.ThrowsExactly<NotSupportedException>(() => Serialize(TestResult, version));
    }

    #endregion

    private static string Serialize<T>(T data, int version)
    {
        return JsonDataSerializer.Instance.Serialize(data, version);
    }

    private static T Deserialize<T>(string json, int version)
    {
        return JsonDataSerializer.Instance.Deserialize<T>(json, version)!;
    }

    /// <summary>
    /// Parse JSON without Newtonsoft date auto-conversion so datetime strings stay as raw strings.
    /// </summary>
    private static JObject ParseJsonNoDates(string json)
    {
        using var reader = new JsonTextReader(new System.IO.StringReader(json)) { DateParseHandling = DateParseHandling.None };
        return JObject.Load(reader);
    }

    private static void VerifyDummyPropertyIsRegistered()
    {
        var dummyProperty = TestProperty.Find("DummyProperty");
        Assert.IsNotNull(dummyProperty);
        Assert.AreEqual("DummyPropertyLabel", dummyProperty.Label);
        Assert.AreEqual("System.String", dummyProperty.ValueType);
        Assert.AreEqual(5, (int)dummyProperty.Attributes);
    }
}
