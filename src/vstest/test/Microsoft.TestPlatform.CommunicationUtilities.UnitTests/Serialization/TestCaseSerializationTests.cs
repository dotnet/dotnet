// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

[TestClass]
public class TestCaseSerializationTests
{
    private static readonly TestCase TestCase = new(
        "sampleTestClass.sampleTestCase",
        new Uri("executor://sampleTestExecutor"),
        "sampleTest.dll")
    {
        CodeFilePath = "/user/src/testFile.cs",
        DisplayName = "sampleTestCase",
        Id = new Guid("be78d6fc-61b0-4882-9d07-40d796fd96ce"),
        LineNumber = 999,
        Traits = { new Trait("Priority", "0"), new Trait("Category", "unit") }
    };

    #region v1 Tests

    [TestMethod]
    public void TestCaseJsonShouldContainAllPropertiesOnSerialization()
    {
        var json = Serialize(TestCase);

        // Use raw deserialization to validate basic properties
        dynamic data = JObject.Parse(json);
        dynamic properties = data["Properties"];

        Assert.AreEqual("TestCase.FullyQualifiedName", properties[0]["Key"]["Id"].Value);
        Assert.AreEqual("sampleTestClass.sampleTestCase", properties[0]["Value"].Value);
        Assert.AreEqual("TestCase.ExecutorUri", properties[1]["Key"]["Id"].Value);
        Assert.AreEqual("executor://sampleTestExecutor", properties[1]["Value"].Value);
        Assert.AreEqual("TestCase.Source", properties[2]["Key"]["Id"].Value);
        Assert.AreEqual("sampleTest.dll", properties[2]["Value"].Value);
        Assert.AreEqual("TestCase.CodeFilePath", properties[3]["Key"]["Id"].Value);
        Assert.AreEqual("/user/src/testFile.cs", properties[3]["Value"].Value);
        Assert.AreEqual("TestCase.DisplayName", properties[4]["Key"]["Id"].Value);
        Assert.AreEqual("sampleTestCase", properties[4]["Value"].Value);
        Assert.AreEqual("TestCase.Id", properties[5]["Key"]["Id"].Value);
        Assert.AreEqual("be78d6fc-61b0-4882-9d07-40d796fd96ce", properties[5]["Value"].Value);
        Assert.AreEqual("TestCase.LineNumber", properties[6]["Key"]["Id"].Value);
        Assert.AreEqual(999, properties[6]["Value"].Value);

        // Traits require special handling with TestPlatformContract resolver. It should be null without it.
        Assert.AreEqual("TestObject.Traits", properties[7]["Key"]["Id"].Value);
        Assert.IsNotNull(properties[7]["Value"]);
    }

    [TestMethod]
    public void TestCaseObjectShouldContainAllPropertiesOnDeserialization()
    {
        var json = "{\"Properties\":[{\"Key\":{\"Id\":\"TestCase.FullyQualifiedName\",\"Label\":\"FullyQualifiedName\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestClass.sampleTestCase\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.ExecutorUri\",\"Label\":\"Executor Uri\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Uri\"},\"Value\":\"executor://sampleTestExecutor\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.Source\",\"Label\":\"Source\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTest.dll\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.CodeFilePath\",\"Label\":\"File Path\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"/user/src/testFile.cs\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.DisplayName\",\"Label\":\"Name\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"sampleTestCase\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.Id\",\"Label\":\"Id\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Guid\"},\"Value\":\"be78d6fc-61b0-4882-9d07-40d796fd96ce\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.LineNumber\",\"Label\":\"Line Number\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Int32\"},\"Value\":999},"
                   + "{\"Key\":{\"Id\":\"TestObject.Traits\",\"Label\":\"Traits\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":5,\"ValueType\":\"System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]\"},\"Value\":[{\"Key\":\"Priority\",\"Value\":\"0\"},{\"Key\":\"Category\",\"Value\":\"unit\"}]}]}";
        var test = Deserialize<TestCase>(json);

        Assert.AreEqual(TestCase.CodeFilePath, test.CodeFilePath);
        Assert.AreEqual(TestCase.DisplayName, test.DisplayName);
        Assert.AreEqual(TestCase.ExecutorUri, test.ExecutorUri);
        Assert.AreEqual(TestCase.FullyQualifiedName, test.FullyQualifiedName);
        Assert.AreEqual(TestCase.LineNumber, test.LineNumber);
        Assert.AreEqual(TestCase.Source, test.Source);
        Assert.AreEqual(TestCase.Traits.First().Name, test.Traits.First().Name);
        Assert.AreEqual(TestCase.Id, test.Id);
    }

    [TestMethod]
    public void TestCaseObjectShouldSerializeWindowsPathWithEscaping()
    {
        var test = new TestCase("a.b", new Uri("uri://x"), @"C:\Test\TestAssembly.dll");

        var json = Serialize(test);

        // Use raw deserialization to validate basic properties
        dynamic data = JObject.Parse(json);
        dynamic properties = data["Properties"];
        Assert.AreEqual(@"TestCase.Source", properties[2]["Key"]["Id"].Value);
        Assert.AreEqual(@"C:\Test\TestAssembly.dll", properties[2]["Value"].Value);
    }

    [TestMethod]
    public void TestCaseObjectShouldDeserializeEscapedWindowsPath()
    {
        var json = "{\"Properties\":[{\"Key\":{\"Id\":\"TestCase.FullyQualifiedName\",\"Label\":\"FullyQualifiedName\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"a.b\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.ExecutorUri\",\"Label\":\"Executor Uri\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Uri\"},\"Value\":\"uri://x\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.Source\",\"Label\":\"Source\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"C:\\\\Test\\\\TestAssembly.dll\"}]}";

        var test = Deserialize<TestCase>(json);

        Assert.AreEqual(@"C:\Test\TestAssembly.dll", test.Source);
    }

    [TestMethod]
    public void TestCaseObjectShouldSerializeTraitsWithSpecialCharacters()
    {
        var test = new TestCase("a.b", new Uri("uri://x"), @"/tmp/a.b.dll");
        test.Traits.Add("t", @"SDJDDHW>,:&^%//\\\\");

        var json = Serialize(test);

        // Use raw deserialization to validate basic properties
        dynamic data = JObject.Parse(json);
        dynamic properties = data["Properties"];
        Assert.AreEqual(@"TestObject.Traits", properties[7]["Key"]["Id"].Value);
        Assert.AreEqual("[{\"Key\":\"t\",\"Value\":\"SDJDDHW>,:&^%//\\\\\\\\\\\\\\\\\"}]", properties[7]["Value"].ToString(Formatting.None));
    }

    [TestMethod]
    public void TestCasePropertiesShouldGetRegisteredAsPartOfDeserialization()
    {
        TestProperty.TryUnregister("DummyProperty", out var _);
        var json = "{\"Properties\":[{\"Key\":{\"Id\":\"TestCase.FullyQualifiedName\",\"Label\":\"FullyQualifiedName\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"a.b\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.ExecutorUri\",\"Label\":\"Executor Uri\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Uri\"},\"Value\":\"uri://x\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.Source\",\"Label\":\"Source\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"/tmp/a.b.dll\"},"
                   + "{\"Key\":{\"Id\":\"DummyProperty\",\"Label\":\"DummyPropertyLabel\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":5,\"ValueType\":\"System.String\"},\"Value\":\"dummyString\"},"
                   + "{\"Key\":{\"Id\":\"TestObject.Traits\",\"Label\":\"Traits\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":5,\"ValueType\":\"System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]\"},\"Value\":[{\"Key\":\"t\",\"Value\":\"SDJDDHW>,:&^%//\\\\\\\\\\\\\\\\\"}]}]}";
        _ = Deserialize<TestCase>(json);

        VerifyDummyPropertyIsRegistered();
    }

    #endregion

    #region v2 Tests

    [TestMethod]
    public void TestCaseJsonShouldContainAllPropertiesOnSerializationV2()
    {
        var json = Serialize(TestCase, 2);

        // Use raw deserialization to validate basic properties
        dynamic data = JObject.Parse(json);
        dynamic properties = data["Properties"];

        // Traits require special handling with TestPlatformContract resolver. It should be null without it.
        Assert.AreEqual("TestObject.Traits", properties[0]["Key"]["Id"].Value);
        Assert.IsNotNull(properties[0]["Value"]);

        Assert.AreEqual("be78d6fc-61b0-4882-9d07-40d796fd96ce", data["Id"].Value);
        Assert.AreEqual("sampleTestClass.sampleTestCase", data["FullyQualifiedName"].Value);
        Assert.AreEqual("sampleTestCase", data["DisplayName"].Value);
        Assert.AreEqual("sampleTest.dll", data["Source"].Value);
        Assert.AreEqual("executor://sampleTestExecutor", data["ExecutorUri"].Value);
        Assert.AreEqual("/user/src/testFile.cs", data["CodeFilePath"].Value);
        Assert.AreEqual(999, data["LineNumber"].Value);
    }

    [TestMethod]
    public void TestCaseObjectShouldContainAllPropertiesOnDeserializationV2()
    {
        var json = "{\"Id\": \"be78d6fc-61b0-4882-9d07-40d796fd96ce\",\"FullyQualifiedName\": \"sampleTestClass.sampleTestCase\",\"DisplayName\": \"sampleTestCase\",\"ExecutorUri\": \"executor://sampleTestExecutor\",\"Source\": \"sampleTest.dll\",\"CodeFilePath\": \"/user/src/testFile.cs\", \"LineNumber\": 999,"
                   + "\"Properties\": [{ \"Key\": { \"Id\": \"TestObject.Traits\", \"Label\": \"Traits\", \"Category\": \"\", \"Description\": \"\", \"Attributes\": 5, \"ValueType\": \"System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]\"}, \"Value\": [{\"Key\": \"Priority\",\"Value\": \"0\"}, {\"Key\": \"Category\",\"Value\": \"unit\"}]}]}";

        var test = Deserialize<TestCase>(json, 2);

        Assert.AreEqual(TestCase.CodeFilePath, test.CodeFilePath);
        Assert.AreEqual(TestCase.DisplayName, test.DisplayName);
        Assert.AreEqual(TestCase.ExecutorUri, test.ExecutorUri);
        Assert.AreEqual(TestCase.FullyQualifiedName, test.FullyQualifiedName);
        Assert.AreEqual(TestCase.LineNumber, test.LineNumber);
        Assert.AreEqual(TestCase.Source, test.Source);
        Assert.AreEqual(TestCase.Traits.First().Name, test.Traits.First().Name);
        Assert.AreEqual(TestCase.Id, test.Id);
    }

    [TestMethod]
    public void TestCaseObjectShouldSerializeTraitsWithSpecialCharactersV2()
    {
        var test = new TestCase("a.b", new Uri("uri://x"), @"/tmp/a.b.dll");
        test.Traits.Add("t", @"SDJDDHW>,:&^%//\\\\");

        var json = Serialize(test, 2);

        // Use raw deserialization to validate basic properties
        dynamic data = JObject.Parse(json);
        dynamic properties = data["Properties"];
        Assert.AreEqual(@"TestObject.Traits", properties[0]["Key"]["Id"].Value);
        Assert.AreEqual("[{\"Key\":\"t\",\"Value\":\"SDJDDHW>,:&^%//\\\\\\\\\\\\\\\\\"}]", properties[0]["Value"].ToString(Formatting.None));
    }

    [TestMethod]
    public void TestCaseObjectShouldSerializeWindowsPathWithEscapingV2()
    {
        var test = new TestCase("a.b", new Uri("uri://x"), @"C:\Test\TestAssembly.dll");

        var json = Serialize(test, 2);

        // Use raw deserialization to validate basic properties
        dynamic data = JObject.Parse(json);
        Assert.AreEqual(@"C:\Test\TestAssembly.dll", data["Source"].Value);
    }

    [TestMethod]
    public void TestCaseObjectShouldDeserializeEscapedWindowsPathV2()
    {
        var json = "{\"Id\":\"4e35ed85-a5e8-946e-fb14-0d3de2304e74\",\"FullyQualifiedName\":\"a.b\",\"DisplayName\":\"a.b\",\"ExecutorUri\":\"uri://x\",\"Source\":\"C:\\\\Test\\\\TestAssembly.dll\",\"CodeFilePath\":null,\"LineNumber\":-1,\"Properties\":[]}";

        var test = Deserialize<TestCase>(json, 2);

        Assert.AreEqual(@"C:\Test\TestAssembly.dll", test.Source);
    }

    [TestMethod]
    public void TestCasePropertiesShouldGetRegisteredAsPartOfDeserializationV2()
    {
        TestProperty.TryUnregister("DummyProperty", out var _);
        var json = "{\"Id\": \"be78d6fc-61b0-4882-9d07-40d796fd96ce\",\"FullyQualifiedName\": \"sampleTestClass.sampleTestCase\",\"DisplayName\": \"sampleTestCase\",\"ExecutorUri\": \"executor://sampleTestExecutor\",\"Source\": \"sampleTest.dll\",\"CodeFilePath\": \"/user/src/testFile.cs\", \"LineNumber\": 999,"
                   + "\"Properties\": [{\"Key\":{\"Id\":\"DummyProperty\",\"Label\":\"DummyPropertyLabel\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":5,\"ValueType\":\"System.String\"},\"Value\":\"dummyString\"},"
                   + "{ \"Key\": { \"Id\": \"TestObject.Traits\", \"Label\": \"Traits\", \"Category\": \"\", \"Description\": \"\", \"Attributes\": 5, \"ValueType\": \"System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]\"}, \"Value\": [{\"Key\": \"Priority\",\"Value\": \"0\"}, {\"Key\": \"Category\",\"Value\": \"unit\"}]}]}";
        _ = Deserialize<TestCase>(json, 2);

        VerifyDummyPropertyIsRegistered();
    }

    #endregion

    #region Common Tests

    [TestMethod]
    [DataRow(1)]
    [DataRow(2)]
    public void TestCaseObjectShouldDeserializeTraitsWithSpecialCharacters(int version)
    {
        var json = "{\"Properties\":[{\"Key\":{\"Id\":\"TestCase.FullyQualifiedName\",\"Label\":\"FullyQualifiedName\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.String\"},\"Value\":\"a.b\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.ExecutorUri\",\"Label\":\"Executor Uri\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":1,\"ValueType\":\"System.Uri\"},\"Value\":\"uri://x\"},"
                   + "{\"Key\":{\"Id\":\"TestCase.Source\",\"Label\":\"Source\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"/tmp/a.b.dll\"},"
                   + "{\"Key\":{\"Id\":\"TestObject.Traits\",\"Label\":\"Traits\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":5,\"ValueType\":\"System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]\"},\"Value\":[{\"Key\":\"t\",\"Value\":\"SDJDDHW>,:&^%//\\\\\\\\\\\\\\\\\"}]}]}";

        var test = Deserialize<TestCase>(json, version);

        var traits = test.Traits.ToArray();
        Assert.AreEqual(1, traits.Length);
        Assert.AreEqual(@"SDJDDHW>,:&^%//\\\\", traits[0].Value);
    }

    #endregion

    private static string Serialize<T>(T data, int version = 1)
    {
        return JsonDataSerializer.Instance.Serialize(data, version);
    }

    private static T Deserialize<T>(string json, int version = 1)
    {
        return JsonDataSerializer.Instance.Deserialize<T>(json, version)!;
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
