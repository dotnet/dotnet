// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

[TestClass]
public class SessionEventsTests
{
    private readonly SessionStartEventArgs _sessionStartEventArgs;

    public SessionEventsTests()
    {
        var properties = new Dictionary<string, object?>
        {
            { "property1", 1 },
            { "property2", 2 }
        };

        _sessionStartEventArgs = new SessionStartEventArgs(properties);
    }

    [TestMethod]
    public void SessionStartEventArgsGetPropertiesShouldGetPropertiesEnumerator()
    {
        var properties = _sessionStartEventArgs.GetProperties();
        int propertiesCount = 0;
        while (properties.MoveNext())
        {
            propertiesCount++;
        }

        Assert.AreEqual(2, propertiesCount);
    }

    [TestMethod]
    public void SessionStartEventArgsGetPropertyValueShouldGetPropertyValue()
    {
        var value = _sessionStartEventArgs.GetPropertyValue<int>("property1");

        Assert.AreEqual(1, value);
    }

    [TestMethod]
    public void SessionStartEventArgsGetPropertyValueShouldGetPropertyValueInObject()
    {
        var value = _sessionStartEventArgs.GetPropertyValue("property1");

        Assert.AreEqual(1, value);
    }
}
