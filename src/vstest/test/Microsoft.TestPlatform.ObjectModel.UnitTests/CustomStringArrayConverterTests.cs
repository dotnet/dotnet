// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

[TestClass]
public class CustomStringArrayConverterTests
{
    private readonly CustomStringArrayConverter _customStringArrayConverter;

    public CustomStringArrayConverterTests()
    {
        _customStringArrayConverter = new CustomStringArrayConverter();
    }

    [TestMethod]
    public void CustomStringArrayConverterShouldDeserializeWellformedJson()
    {
        var json = "[ \"val2\", \"val1\" ]";

        var data = _customStringArrayConverter.ConvertFrom(null, CultureInfo.InvariantCulture, json) as string[];

        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Length);
        CollectionAssert.AreEqual(new[] { "val2", "val1" }, data);
    }

    [TestMethod]
    public void CustomStringArrayConverterShouldDeserializeEmptyArray()
    {
        var json = "[]";

        var data = _customStringArrayConverter.ConvertFrom(null, CultureInfo.InvariantCulture, json) as string[];

        Assert.IsNotNull(data);
        Assert.AreEqual(0, data.Length);
    }

    [TestMethod]
    public void CustomStringArrayConverterShouldDeserializeNullKeyOrValue()
    {
        var json = "[null, \"val\"]";

        var data = _customStringArrayConverter.ConvertFrom(null, CultureInfo.InvariantCulture, json) as string[];

        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Length);
        Assert.IsNull(data[0]);
        Assert.AreEqual("val", data[1]);
    }

    [TestMethod]
    public void CustomStringArrayConverterShouldDeserializeEmptyKeyOrValue()
    {
        var json = "[\"\", \"\"]";

        var data = _customStringArrayConverter.ConvertFrom(null, CultureInfo.InvariantCulture, json) as string[];

        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Length);
        Assert.AreEqual(string.Empty, data[0]);
        Assert.AreEqual(string.Empty, data[1]);
    }

    [TestMethod]
    public void CustomStringArrayConverterShouldDeserializeNullValue()
    {
        var data = _customStringArrayConverter.ConvertFrom(null, CultureInfo.InvariantCulture, null) as string[];

        Assert.IsNull(data);
    }
}
