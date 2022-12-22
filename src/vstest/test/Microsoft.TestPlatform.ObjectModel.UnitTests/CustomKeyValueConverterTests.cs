// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

[TestClass]
public class CustomKeyValueConverterTests
{
    private readonly CustomKeyValueConverter _customKeyValueConverter;

    public CustomKeyValueConverterTests()
    {
        _customKeyValueConverter = new CustomKeyValueConverter();
    }

    [TestMethod]
    public void CustomKeyValueConverterShouldDeserializeWellformedJson()
    {
        var json = "[{ \"Key\": \"key1\", \"Value\": \"val1\" }]";

        var data = _customKeyValueConverter.ConvertFrom(null, CultureInfo.InvariantCulture, json) as KeyValuePair<string, string>[];

        Assert.IsNotNull(data);
        Assert.AreEqual(1, data.Length);
        Assert.AreEqual("key1", data[0].Key);
        Assert.AreEqual("val1", data[0].Value);
    }

    [TestMethod]
    public void CustomKeyValueConverterShouldDeserializeKeyValuePairArray()
    {
        var json = "[{ \"Key\": \"key1\", \"Value\": \"val1\" }, { \"Key\": \"key2\", \"Value\": \"val2\" }]";

        var data = _customKeyValueConverter.ConvertFrom(null, CultureInfo.InvariantCulture, json) as KeyValuePair<string, string>[];

        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Length);
        Assert.AreEqual("key1", data[0].Key);
        Assert.AreEqual("val1", data[0].Value);
        Assert.AreEqual("key2", data[1].Key);
        Assert.AreEqual("val2", data[1].Value);
    }

    [TestMethod]
    public void CustomKeyValueConverterShouldDeserializeEmptyArray()
    {
        var json = "[]";

        var data = _customKeyValueConverter.ConvertFrom(null, CultureInfo.InvariantCulture, json) as KeyValuePair<string, string>[];

        Assert.IsNotNull(data);
        Assert.AreEqual(0, data.Length);
    }

    [TestMethod]
    public void CustomKeyValueConverterShouldDeserializeEmptyKeyOrValue()
    {
        var json = "[{ \"Key\": \"\", \"Value\": \"\" }]";

        var data = _customKeyValueConverter.ConvertFrom(null, CultureInfo.InvariantCulture, json) as KeyValuePair<string, string>[];

        Assert.IsNotNull(data);
        Assert.AreEqual(1, data.Length);
        Assert.AreEqual(string.Empty, data[0].Key);
        Assert.AreEqual(string.Empty, data[0].Value);
    }

    [TestMethod]
    public void CustomKeyValueConverterShouldDeserializeDuplicateKeysKvps()
    {
        var json = "[{ \"Key\": \"key1\", \"Value\": \"val1\" }, { \"Key\": \"key1\", \"Value\": \"val2\" }]";

        var data = _customKeyValueConverter.ConvertFrom(null, CultureInfo.InvariantCulture, json) as KeyValuePair<string, string>[];

        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Length);
        Assert.AreEqual("key1", data[0].Key);
        Assert.AreEqual("val1", data[0].Value);
        Assert.AreEqual("key1", data[1].Key);
        Assert.AreEqual("val2", data[1].Value);
    }

    [TestMethod]
    public void CustomKeyValueConverterShouldDeserializeNullValue()
    {
        var data = _customKeyValueConverter.ConvertFrom(null, CultureInfo.InvariantCulture, null) as KeyValuePair<string, string>[];

        Assert.IsNull(data);
    }
}
