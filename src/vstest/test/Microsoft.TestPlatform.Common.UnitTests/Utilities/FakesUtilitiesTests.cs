// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.Utilities;

[TestClass]
public class FakesUtilitiesTests
{
    [TestMethod]
    public void FakesSettingsShouldThrowExceptionIfSourcesArePassedAsNull()
    {
        string runSettingsXml = @"<RunSettings><RunConfiguration><TargetFrameworkVersion>.netstandard,Version=5.0</TargetFrameworkVersion></RunConfiguration ></RunSettings>";
        Assert.ThrowsException<ArgumentNullException>(() => FakesUtilities.GenerateFakesSettingsForRunConfiguration(null!, runSettingsXml));
    }

    [TestMethod]
    public void FakesSettingsShouldThrowExceptionIfRunSettingsIsPassedAsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => FakesUtilities.GenerateFakesSettingsForRunConfiguration(Array.Empty<string>(), null!));
    }

    [TestMethod]
    public void FakesSettingsShouldBeNotGeneratedIfFakeConfiguratorAssemblyIsNotPresent()
    {
        string runSettingsXml = @"<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        var generatedRunSettings = FakesUtilities.GenerateFakesSettingsForRunConfiguration(new string[] { @"C:\temp\UT.dll" }, runSettingsXml);
        Assert.AreEqual(generatedRunSettings, runSettingsXml);
    }

    [TestMethod]
    public void FakesDataCollectorSettingsShouldBeOverridden()
    {
        string runSettingsXml = @"<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        var doc = new XmlDocument();
        using (var xmlReader = XmlReader.Create(
                   new StringReader(runSettingsXml),
                   new XmlReaderSettings() { CloseInput = true }))
        {
            doc.Load(xmlReader);
        }

        var dataCollectorNode = new DataCollectorSettings()
        {
            AssemblyQualifiedName = FakesUtilities.FakesMetadata.DataCollectorAssemblyQualifiedName,
            Uri = new Uri(FakesUtilities.FakesMetadata.DataCollectorUriV1),
            FriendlyName = FakesUtilities.FakesMetadata.FriendlyName,
            IsEnabled = true,
            Configuration = doc.FirstChild as XmlElement
        };
        XmlRunSettingsUtilities.InsertDataCollectorsNode(doc.CreateNavigator()!, dataCollectorNode);

        var dataCollectorNode2 = new DataCollectorSettings()
        {
            AssemblyQualifiedName = FakesUtilities.FakesMetadata.DataCollectorAssemblyQualifiedName,
            Uri = new Uri(FakesUtilities.FakesMetadata.DataCollectorUriV2),
            FriendlyName = FakesUtilities.FakesMetadata.FriendlyName,
            IsEnabled = true,
            Configuration = doc.FirstChild as XmlElement
        };
        FakesUtilities.InsertOrReplaceFakesDataCollectorNode(doc, dataCollectorNode2);

        Assert.IsFalse(XmlRunSettingsUtilities.ContainsDataCollector(doc, FakesUtilities.FakesMetadata.DataCollectorUriV1));
        Assert.IsTrue(XmlRunSettingsUtilities.ContainsDataCollector(doc, FakesUtilities.FakesMetadata.DataCollectorUriV2));
    }

    [TestMethod]
    public void FakesDataCollectorSettingsShouldBeInserted()
    {
        string runSettingsXml = @"<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        var doc = new XmlDocument();
        using (var xmlReader = XmlReader.Create(
                   new StringReader(runSettingsXml),
                   new XmlReaderSettings() { CloseInput = true }))
        {
            doc.Load(xmlReader);
        }

        var dataCollectorNode2 = new DataCollectorSettings()
        {
            AssemblyQualifiedName = FakesUtilities.FakesMetadata.DataCollectorAssemblyQualifiedName,
            Uri = new Uri(FakesUtilities.FakesMetadata.DataCollectorUriV2),
            FriendlyName = FakesUtilities.FakesMetadata.FriendlyName,
            IsEnabled = true,
            Configuration = doc.FirstChild as XmlElement
        };
        FakesUtilities.InsertOrReplaceFakesDataCollectorNode(doc, dataCollectorNode2);
        Assert.IsTrue(XmlRunSettingsUtilities.ContainsDataCollector(doc, FakesUtilities.FakesMetadata.DataCollectorUriV2));
    }

    [TestMethod]
    public void OtherRunsettingsShouldNotBeChanged()
    {
        string runSettingsXml = @"<RunSettings><RunConfiguration><TargetFrameworkVersion>FrameworkCore10</TargetFrameworkVersion></RunConfiguration></RunSettings>";
        var doc = new XmlDocument();
        using (var xmlReader = XmlReader.Create(
                   new StringReader(runSettingsXml),
                   new XmlReaderSettings() { CloseInput = true }))
        {
            doc.Load(xmlReader);
        }

        var dataCollectorNode2 = new DataCollectorSettings()
        {
            AssemblyQualifiedName = FakesUtilities.FakesMetadata.DataCollectorAssemblyQualifiedName,
            Uri = new Uri(FakesUtilities.FakesMetadata.DataCollectorUriV2),
            FriendlyName = FakesUtilities.FakesMetadata.FriendlyName,
            IsEnabled = true,
            Configuration = doc.CreateElement("Configuration")
        };
        FakesUtilities.InsertOrReplaceFakesDataCollectorNode(doc, dataCollectorNode2);
        Assert.IsTrue(XmlRunSettingsUtilities.ContainsDataCollector(doc, FakesUtilities.FakesMetadata.DataCollectorUriV2));
        XmlNodeList nodes = doc.SelectNodes("//RunSettings/RunConfiguration/TargetFrameworkVersion")!;
        Assert.AreEqual("FrameworkCore10", nodes[0]!.InnerText);
    }
}
