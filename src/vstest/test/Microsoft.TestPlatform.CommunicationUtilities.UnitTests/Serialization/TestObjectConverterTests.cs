// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.TestPlatform.CommunicationUtilities.UnitTests.TestDoubles;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

[TestClass]
public class TestObjectConverterTests
{
    [TestMethod]
    public void TestObjectJsonShouldContainOnlyProperties()
    {
        var json = Serialize(new TestableTestObject());

        Assert.AreEqual("{\"Properties\":[]}", json);
    }

    [TestMethod]
    public void TestObjectShouldCreateDefaultObjectOnDeserializationOfJsonWithEmptyProperties()
    {
        var test = Deserialize<TestableTestObject>("{\"Properties\":[]}");

        Assert.IsNotNull(test);
        Assert.AreEqual(0, test.Properties.Count());
    }

    [TestMethod]
    public void TestCaseObjectShouldSerializeCustomProperties()
    {
        var test = new TestableTestObject();
        var testProperty1 = TestProperty.Register("1", "label1", typeof(Guid), typeof(TestableTestObject));
        var testPropertyData1 = Guid.Parse("02048dfd-3da7-475d-a011-8dd1121855ec");
        var testProperty2 = TestProperty.Register("2", "label2", typeof(int), typeof(TestableTestObject));
        var testPropertyData2 = 29;
        test.SetPropertyValue(testProperty1, testPropertyData1);
        test.SetPropertyValue(testProperty2, testPropertyData2);

        var json = Serialize(test);

        // Use raw deserialization to validate basic properties
        // Because properties are backed up by a ConcurrentDictionary we don't have control over the order of serialization
        var expectedJsonWithKey1First = "{\"Properties\":[{\"Key\":{\"Id\":\"1\",\"Label\":\"label1\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.Guid\"},\"Value\":\"02048dfd-3da7-475d-a011-8dd1121855ec\"},{\"Key\":{\"Id\":\"2\",\"Label\":\"label2\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.Int32\"},\"Value\":29}]}";
        var expectedJsonWithKey2First = "{\"Properties\":[{\"Key\":{\"Id\":\"2\",\"Label\":\"label2\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.Int32\"},\"Value\":29},{\"Key\":{\"Id\":\"1\",\"Label\":\"label1\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.Guid\"},\"Value\":\"02048dfd-3da7-475d-a011-8dd1121855ec\"}]}";

        if (json != expectedJsonWithKey1First && json != expectedJsonWithKey2First)
        {
            Assert.Fail($"Was expecting <{json}> to be either <{expectedJsonWithKey1First}> or <{expectedJsonWithKey2First}>.");
        }
    }

    [TestMethod]
    public void TestObjectShouldSerializeStringArrayValueForProperty()
    {
        var test = new TestableTestObject();
        var testProperty1 = TestProperty.Register("11", "label1", typeof(string[]), typeof(TestableTestObject));
        var testPropertyData1 = new[] { "val1", "val2" };
        test.SetPropertyValue(testProperty1, testPropertyData1);

        var json = Serialize(test);

        var expectedJson = "{\"Properties\":[{\"Key\":{\"Id\":\"11\",\"Label\":\"label1\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String[]\"},\"Value\":[\"val1\",\"val2\"]}]}";
        Assert.AreEqual(expectedJson, json);
    }

    [TestMethod]
    public void TestObjectShouldSerializeDateTimeOffsetForProperty()
    {
        var test = new TestableTestObject();
        var testProperty1 = TestProperty.Register("12", "label1", typeof(DateTimeOffset), typeof(TestableTestObject));
        var testPropertyData1 = DateTimeOffset.MaxValue;
        test.SetPropertyValue(testProperty1, testPropertyData1);

        var json = Serialize(test);

        var expectedJson = "{\"Properties\":[{\"Key\":{\"Id\":\"12\",\"Label\":\"label1\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"9999-12-31T23:59:59.9999999+00:00\"}]}";
        Assert.AreEqual(expectedJson, json);
    }

    [TestMethod]
    public void TestObjectShouldDeserializeCustomProperties()
    {
        var json = "{\"Properties\":[{\"Key\":{\"Id\":\"13\",\"Label\":\"label1\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.Guid\"},\"Value\":\"02048dfd-3da7-475d-a011-8dd1121855ec\"},{\"Key\":{\"Id\":\"2\",\"Label\":\"label2\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.Int32\"},\"Value\":29}]}";

        var test = Deserialize<TestableTestObject>(json);

        var properties = test.Properties.ToArray();
        Assert.AreEqual(2, properties.Length);
        Assert.AreEqual(Guid.Parse("02048dfd-3da7-475d-a011-8dd1121855ec"), test.GetPropertyValue(properties.First(x => x.Label == "label1")));
        Assert.AreEqual(29, test.GetPropertyValue(properties.First(x => x.Label == "label2")));
    }

    [TestMethod]
    public void TestObjectShouldDeserializeNullValueForProperty()
    {
        var json = "{\"Properties\":[{\"Key\":{\"Id\":\"14\",\"Label\":\"label1\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":null}]}";

        var test = Deserialize<TestableTestObject>(json);

        var properties = test.Properties.ToArray();
        Assert.AreEqual(1, properties.Length);
        Assert.IsTrue(string.IsNullOrEmpty(test.GetPropertyValue(properties[0])!.ToString()));
    }

    [TestMethod]
    public void TestObjectShouldDeserializeStringArrayValueForProperty()
    {
        var json = "{\"Properties\":[{\"Key\":{\"Id\":\"15\",\"Label\":\"label1\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.String[]\"},\"Value\":[\"val1\", \"val2\"]}]}";

        var test = Deserialize<TestableTestObject>(json);

        var properties = test.Properties.ToArray();
        Assert.AreEqual(1, properties.Length);
        CollectionAssert.AreEqual(new[] { "val1", "val2" }, (string[])test.GetPropertyValue(properties[0])!);
    }

    [TestMethod]
    public void TestObjectShouldDeserializeDatetimeOffset()
    {
        var json = "{\"Properties\":[{\"Key\":{\"Id\":\"16\",\"Label\":\"label1\",\"Category\":\"\",\"Description\":\"\",\"Attributes\":0,\"ValueType\":\"System.DateTimeOffset\"},\"Value\":\"9999-12-31T23:59:59.9999999+00:00\"}]}";

        var test = Deserialize<TestableTestObject>(json);

        var properties = test.Properties.ToArray();
        Assert.AreEqual(1, properties.Length);
        Assert.AreEqual(DateTimeOffset.MaxValue, test.GetPropertyValue(properties[0]));
    }

    [TestMethod]
    public void TestObjectShouldAddPropertyToTestPropertyStoreOnDeserialize()
    {
        var json = "{\"Properties\":[{\"Key\":{\"Id\":\"17\",\"Label\":\"label1\",\"Category\":\"c\",\"Description\":\"d\",\"Attributes\":0,\"ValueType\":\"System.String\"},\"Value\":\"DummyValue\"}]}";

        var test = Deserialize<TestableTestObject>(json);

        var property = TestProperty.Find("17");
        Assert.IsNotNull(property);
        Assert.AreEqual("17", property.Id);
        Assert.AreEqual("label1", property.Label);
        Assert.AreEqual("c", property.Category);
        Assert.AreEqual("d", property.Description);
        Assert.AreEqual(typeof(string), property.GetValueType());
        Assert.AreEqual(TestPropertyAttributes.None, property.Attributes);
        Assert.AreEqual("DummyValue", test.GetPropertyValue(property));
    }

    [TestMethod]
    public void TestObjectSetPropertyValueShouldNotConvertIfValueMatchesPropertyDataType()
    {
        var property = TestProperty.Register("98", "p1", typeof(bool), typeof(TestObject));
        var testobj = new TestableTestObject();

        // This should not throw even if the runtime type of boolean where as specified
        // type is object
        testobj.SetPropertyValue<object>(property, false);

        Assert.AreEqual(false, testobj.GetPropertyValue(property));
    }

    private static string Serialize<T>(T data)
    {
        return JsonDataSerializer.Instance.Serialize(data);
    }

    private static T Deserialize<T>(string json)
    {
        return JsonDataSerializer.Instance.Deserialize<T>(json)!;
    }
}
