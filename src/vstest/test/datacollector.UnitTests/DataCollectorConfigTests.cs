// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollectorUnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.UnitTests;

[TestClass]
public class DataCollectorConfigTests
{
    [TestMethod]
    public void ConstructorShouldSetCorrectFriendlyNameAndUri()
    {
        var dataCollectorConfig = new DataCollectorConfig(typeof(CustomDataCollector));

        Assert.AreEqual("CustomDataCollector", dataCollectorConfig.FriendlyName);
        Assert.AreEqual("my://custom/datacollector", dataCollectorConfig.TypeUri!.ToString());
    }

    [TestMethod]
    public void ConstructorShouldThrowExceptionIfTypeIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => new DataCollectorConfig(null!));
    }

    [TestMethod]
    public void ConstructorShouldNotThrowExceptionIfUriIsNotSpecifiedInDataCollector()
    {
        var dataCollectorConfig = new DataCollectorConfig(typeof(CustomDataCollectorWithoutUri));
        Assert.AreEqual("CustomDataCollector", dataCollectorConfig.FriendlyName);
        Assert.IsNull(dataCollectorConfig.TypeUri);
    }

    [TestMethod]
    public void ConstructorShouldNotThrowExceptionIfFriendlyNameIsEmpty()
    {
        var dataCollectorConfig = new DataCollectorConfig(typeof(CustomDataCollectorWithEmptyFriendlyName));
        Assert.AreEqual("", dataCollectorConfig.FriendlyName);
        Assert.AreEqual("my://custom/datacollector", dataCollectorConfig.TypeUri!.ToString());
    }

    [TestMethod]
    public void ConstructorShouldNotThrowExceptionIfFriendlyNameIsNotSpecified()
    {
        var dataCollectorConfig = new DataCollectorConfig(typeof(CustomDataCollectorWithoutFriendlyName));
        Assert.AreEqual("", dataCollectorConfig.FriendlyName);
        Assert.AreEqual("my://custom/datacollector", dataCollectorConfig.TypeUri!.ToString());
    }
}
