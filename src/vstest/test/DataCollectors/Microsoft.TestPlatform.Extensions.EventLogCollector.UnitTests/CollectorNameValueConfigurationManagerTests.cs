// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Extensions.EventLogCollector.UnitTests;

[TestClass]
public class CollectorNameValueConfigurationManagerTests
{
    private const string ConfigurationString =
        @"<Configuration><Setting name=""key"" value=""value"" /></Configuration>";

    private const string EmptyConfigurationString =
        @"<Configuration/>";

    [TestMethod]
    public void ConstructorShouldInitializeNameValuePairDictionary()
    {
        XmlDocument xmlDocument = new();
        using (XmlReader reader = XmlReader.Create(new StringReader(ConfigurationString)))
        {
            xmlDocument.Load(reader);
        }

        var configManager = new CollectorNameValueConfigurationManager(xmlDocument.DocumentElement);

        Assert.AreEqual("value", configManager["key"]);
    }

    [TestMethod]
    public void ConstructorShouldNotInitializeNameValuePairIfEmptyXmlElementIsPassed()
    {
        XmlDocument xmlDocument = new();
        using (XmlReader reader = XmlReader.Create(new StringReader(EmptyConfigurationString)))
        {
            xmlDocument.Load(reader);
        }

        var configManager = new CollectorNameValueConfigurationManager(xmlDocument.DocumentElement);
        Assert.AreEqual(0, configManager.NameValuePairs.Count);
    }

    [TestMethod]
    public void ConstructorShouldNotInitializeNameValuePairNullIsPassed()
    {
        var configManager = new CollectorNameValueConfigurationManager(null);
        Assert.AreEqual(0, configManager.NameValuePairs.Count);
    }
}
