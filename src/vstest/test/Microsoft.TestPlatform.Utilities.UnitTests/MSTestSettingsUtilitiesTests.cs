// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Utilities.Tests;

[TestClass]
public class MsTestSettingsUtilitiesTests
{
    #region IsLegacyTestSettingsFile tests

    [TestMethod]
    public void IsLegacyTestSettingsFileShouldReturnTrueIfTestSettingsExtension()
    {
        Assert.IsTrue(MSTestSettingsUtilities.IsLegacyTestSettingsFile("C:\\temp\\t.testsettings"));
    }

    [TestMethod]
    public void IsLegacyTestSettingsFileShouldReturnTrueIfTestRunConfigExtension()
    {
        Assert.IsTrue(MSTestSettingsUtilities.IsLegacyTestSettingsFile("C:\\temp\\t.testrunConfig"));
    }

    [TestMethod]
    public void IsLegacyTestSettingsFileShouldReturnTrueIfVsmdiExtension()
    {
        Assert.IsTrue(MSTestSettingsUtilities.IsLegacyTestSettingsFile("C:\\temp\\t.vsmdi"));
    }

    #endregion

    #region Import tests

    [TestMethod]
    public void ImportShouldThrowIfNotLegacySettingsFile()
    {
        var defaultRunSettingsXml = "<RunSettings></RunSettings>";
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(defaultRunSettingsXml);

        Action action =
            () =>
                MSTestSettingsUtilities.Import(
                    "C:\\temp\\r.runsettings",
                    xmlDocument);
        var exception = Assert.ThrowsExactly<XmlException>(action);
        Assert.AreEqual("Unexpected settings file specified.", exception.Message);
    }

    [TestMethod]
    public void ImportShouldThrowIfDefaultRunSettingsIsIncorrect()
    {
        var defaultRunSettingsXml = "<DataRunSettings></DataRunSettings>";
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(defaultRunSettingsXml);

        Action action =
            () =>
                MSTestSettingsUtilities.Import(
                    "C:\\temp\\r.testsettings",
                    xmlDocument);
        var exception = Assert.ThrowsExactly<XmlException>(action);
        Assert.AreEqual("Could not find 'RunSettings' node.", exception.Message);
    }

    [TestMethod]
    public void ImportShouldEmbedTestSettingsInformation()
    {
        var defaultRunSettingsXml = "<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(defaultRunSettingsXml);
        var finalxPath = MSTestSettingsUtilities.Import(
            "C:\\temp\\r.testsettings",
            xmlDocument);

        var finalSettingsXml = finalxPath.CreateNavigator()!.OuterXml;

        var expectedSettingsXml = string.Join(Environment.NewLine,
            "<RunSettings>",
            "  <MSTest>",
            "    <SettingsFile>C:\\temp\\r.testsettings</SettingsFile>",
            "    <ForcedLegacyMode>true</ForcedLegacyMode>",
            "  </MSTest>",
            "  <RunConfiguration></RunConfiguration>",
            "</RunSettings>"
        );

        Assert.AreEqual(expectedSettingsXml, finalSettingsXml);
    }

    [TestMethod]
    public void ImportShouldEmbedTestSettingsAndDefaultRunConfigurationInformation()
    {
        var defaultRunSettingsXml = "<RunSettings></RunSettings>";
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(defaultRunSettingsXml);
        var finalxPath = MSTestSettingsUtilities.Import(
            "C:\\temp\\r.testsettings",
            xmlDocument);

        var finalSettingsXml = finalxPath.CreateNavigator()!.OuterXml;

        var expectedSettingsXml = string.Join(Environment.NewLine,
            "<RunSettings>",
            "  <RunConfiguration />",
            "  <MSTest>",
            "    <SettingsFile>C:\\temp\\r.testsettings</SettingsFile>",
            "    <ForcedLegacyMode>true</ForcedLegacyMode>",
            "  </MSTest>",
            "</RunSettings>"
        );

        Assert.AreEqual(expectedSettingsXml, finalSettingsXml);
    }

    #endregion
}
