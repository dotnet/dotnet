// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.UnitTests;

[TestClass]
public class DataCollectionEnvironmentVariableTests
{
    [TestMethod]
    public void ConstructorShouldThrowExceptionIfKeyValueIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () =>
            {
                var envvariable = new DataCollectionEnvironmentVariable(default, null!);
            });
    }

    [TestMethod]
    public void FirstDataCollectorThatRequestedShouldReturnTheFirstdataCollectorRequestingThatEnvVariable()
    {
        var envValPair = new KeyValuePair<string, string>("key", "value");
        var envvariable = new DataCollectionEnvironmentVariable(envValPair, "datacollector");
        envvariable.AddRequestingDataCollector("datacollector1");

        Assert.AreEqual("datacollector", envvariable.FirstDataCollectorThatRequested);
    }

    [TestMethod]
    public void FirstDataCollectorThatRequestedShouldSetNameAndValue()
    {
        var envValPair = new KeyValuePair<string, string>("key", "value");
        var envvariable = new DataCollectionEnvironmentVariable(envValPair, "datacollector");
        envvariable.AddRequestingDataCollector("datacollector1");

        Assert.AreEqual("key", envvariable.Name);
        Assert.AreEqual("value", envvariable.Value);
    }
}
