// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Utilities.Tests;

[TestClass]
public class ClientUtilitiesTests
{
    [TestMethod]
    public void FixRelativePathsInRunSettingsShouldThrowIfDocumentIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => ClientUtilities.FixRelativePathsInRunSettings(null!, "c:\\temp"));
    }

    [TestMethod]
    public void FixRelativePathsInRunSettingsShouldThrowIfPathIsNullOrEmpty()
    {
        Assert.ThrowsException<ArgumentNullException>(() => ClientUtilities.FixRelativePathsInRunSettings(new XmlDocument(), null!));
        Assert.ThrowsException<ArgumentNullException>(() => ClientUtilities.FixRelativePathsInRunSettings(new XmlDocument(), ""));
    }

    [TestMethod]
    public void FixRelativePathsInRunSettingsShouldNotModifyAnEmptyRunSettings()
    {
        var runSettingsXml = "<RunSettings></RunSettings>";

        var doc = new XmlDocument();
        doc.LoadXml(runSettingsXml);

        var currentAssemblyLocation = typeof(ClientUtilitiesTests).GetTypeInfo().Assembly.Location;

        ClientUtilities.FixRelativePathsInRunSettings(doc, currentAssemblyLocation);

        var finalSettingsXml = doc.OuterXml;

        var expectedRunSettingsXml = string.Concat("<RunSettings><RunSettingsDirectory>",
            Path.GetDirectoryName(currentAssemblyLocation),
            "</RunSettingsDirectory></RunSettings>");

        Assert.AreEqual(expectedRunSettingsXml, finalSettingsXml);
    }

    [TestMethod]
    public void FixRelativePathsInRunSettingsShouldModifyRelativeTestSettingsFilePath()
    {
        var runSettingsXml = "<RunSettings><MSTest><SettingsFile>..\\remote.testsettings</SettingsFile></MSTest></RunSettings>";

        var doc = new XmlDocument();
        doc.LoadXml(runSettingsXml);

        var currentAssemblyLocation = typeof(ClientUtilitiesTests).GetTypeInfo().Assembly.Location;

        ClientUtilities.FixRelativePathsInRunSettings(doc, currentAssemblyLocation);

        var finalSettingsXml = doc.OuterXml;

        var expectedPath = Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(currentAssemblyLocation)!, "..\\remote.testsettings"));
        var expectedSettingsXml = string.Concat(
            "<RunSettings><MSTest><SettingsFile>",
            expectedPath,
            "</SettingsFile></MSTest><RunSettingsDirectory>",
            Path.GetDirectoryName(currentAssemblyLocation),
            "</RunSettingsDirectory></RunSettings>");

        Assert.AreEqual(expectedSettingsXml, finalSettingsXml);
    }

    [TestMethod]
    public void FixRelativePathsInRunSettingsShouldNotModifyAbsoluteTestSettingsFilePath()
    {
        var absolutePath = Path.Combine(Path.GetTempPath(), "remote.testsettings");
        var runSettingsXml = $"<RunSettings><MSTest><SettingsFile>{absolutePath}</SettingsFile></MSTest></RunSettings>";

        var doc = new XmlDocument();
        doc.LoadXml(runSettingsXml);

        var currentAssemblyLocation = typeof(ClientUtilitiesTests).GetTypeInfo().Assembly.Location;

        ClientUtilities.FixRelativePathsInRunSettings(doc, currentAssemblyLocation);

        var finalSettingsXml = doc.OuterXml;

        var expectedRunSettingsXml = string.Concat($"<RunSettings><MSTest><SettingsFile>{absolutePath}</SettingsFile></MSTest><RunSettingsDirectory>",
            Path.GetDirectoryName(currentAssemblyLocation),
            "</RunSettingsDirectory></RunSettings>");

        Assert.AreEqual(expectedRunSettingsXml, finalSettingsXml);
    }

    [TestMethod]
    public void FixRelativePathsInRunSettingsShouldNotModifyEmptyTestSettingsFilePath()
    {
        var runSettingsXml = "<RunSettings><MSTest><SettingsFile></SettingsFile></MSTest></RunSettings>";

        var doc = new XmlDocument();
        doc.LoadXml(runSettingsXml);

        var currentAssemblyLocation = typeof(ClientUtilitiesTests).GetTypeInfo().Assembly.Location;

        ClientUtilities.FixRelativePathsInRunSettings(doc, currentAssemblyLocation);

        var finalSettingsXml = doc.OuterXml;

        var expectedRunSettingsXml = string.Concat("<RunSettings><MSTest><SettingsFile></SettingsFile></MSTest><RunSettingsDirectory>",
            Path.GetDirectoryName(currentAssemblyLocation),
            "</RunSettingsDirectory></RunSettings>");

        Assert.AreEqual(expectedRunSettingsXml, finalSettingsXml);
    }

    [TestMethod]
    public void FixRelativePathsInRunSettingsShouldModifyRelativeResultsDirectory()
    {
        var runSettingsXml = "<RunSettings><RunConfiguration><ResultsDirectory>..\\results</ResultsDirectory></RunConfiguration></RunSettings>";

        var doc = new XmlDocument();
        doc.LoadXml(runSettingsXml);

        var currentAssemblyLocation = typeof(ClientUtilitiesTests).GetTypeInfo().Assembly.Location;

        ClientUtilities.FixRelativePathsInRunSettings(doc, currentAssemblyLocation);

        var finalSettingsXml = doc.OuterXml;

        var expectedPath = Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(currentAssemblyLocation)!, "..\\results"));
        var expectedSettingsXml = string.Concat(
            "<RunSettings><RunConfiguration><ResultsDirectory>",
            expectedPath,
            "</ResultsDirectory></RunConfiguration><RunSettingsDirectory>",
            Path.GetDirectoryName(currentAssemblyLocation),
            "</RunSettingsDirectory></RunSettings>");

        Assert.AreEqual(expectedSettingsXml, finalSettingsXml);
    }

    [TestMethod]
    public void FixRelativePathsInRunSettingsShouldNotModifyAbsoluteResultsDirectory()
    {
        var absolutePath = Path.Combine(Path.GetTempPath(), "results");
        var runSettingsXml = $"<RunSettings><RunConfiguration><ResultsDirectory>{absolutePath}</ResultsDirectory></RunConfiguration></RunSettings>";

        var doc = new XmlDocument();
        doc.LoadXml(runSettingsXml);

        var currentAssemblyLocation = typeof(ClientUtilitiesTests).GetTypeInfo().Assembly.Location;

        ClientUtilities.FixRelativePathsInRunSettings(doc, currentAssemblyLocation);

        var finalSettingsXml = doc.OuterXml;

        var expectedRunSettingsXml = string.Concat($"<RunSettings><RunConfiguration><ResultsDirectory>{absolutePath}</ResultsDirectory></RunConfiguration><RunSettingsDirectory>",
            Path.GetDirectoryName(currentAssemblyLocation),
            "</RunSettingsDirectory></RunSettings>");

        Assert.AreEqual(expectedRunSettingsXml, finalSettingsXml);
    }

    [TestMethod]
    public void FixRelativePathsInRunSettingsShouldNotModifyEmptyResultsDirectory()
    {
        var runSettingsXml = "<RunSettings><RunConfiguration><ResultsDirectory></ResultsDirectory></RunConfiguration></RunSettings>";

        var doc = new XmlDocument();
        doc.LoadXml(runSettingsXml);

        var currentAssemblyLocation = typeof(ClientUtilitiesTests).GetTypeInfo().Assembly.Location;

        ClientUtilities.FixRelativePathsInRunSettings(doc, currentAssemblyLocation);

        var finalSettingsXml = doc.OuterXml;

        var expectedRunSettingsXml = string.Concat("<RunSettings><RunConfiguration><ResultsDirectory></ResultsDirectory></RunConfiguration><RunSettingsDirectory>",
            Path.GetDirectoryName(currentAssemblyLocation),
            "</RunSettingsDirectory></RunSettings>");

        Assert.AreEqual(expectedRunSettingsXml, finalSettingsXml);
    }

    [TestMethod]
    public void FixRelativePathsInRunSettingsShouldExpandEnvironmentVariable()
    {
        try
        {
            Environment.SetEnvironmentVariable("TEST_TEMP", Path.GetTempPath());
            // using TEST_TEMP because TMP or TEMP, or HOME are not defined across all tested OSes
            // Using \\ instead of platform specifc path separator does not matter, because the paths are not interpreted by the OS.
            var runSettingsXml = "<RunSettings><RunConfiguration><ResultsDirectory>%TEST_TEMP%\\results</ResultsDirectory><DotNetHostPath>%TEST_TEMP%\\hostpath</DotNetHostPath></RunConfiguration></RunSettings>";

            var doc = new XmlDocument();
            doc.LoadXml(runSettingsXml);

            var currentAssemblyLocation = typeof(ClientUtilitiesTests).GetTypeInfo().Assembly.Location;

            ClientUtilities.FixRelativePathsInRunSettings(doc, currentAssemblyLocation);

            var finalSettingsXml = doc.OuterXml;

            var expectedResultsPath = $"{Environment.GetEnvironmentVariable("TEST_TEMP")}\\results";
            var expectedDotNetHostPath = $"{Environment.GetEnvironmentVariable("TEST_TEMP")}\\hostpath";

            var expectedSettingsXml = string.Concat(
                "<RunSettings><RunConfiguration><ResultsDirectory>",
                expectedResultsPath,
                "</ResultsDirectory><DotNetHostPath>",
                expectedDotNetHostPath,
                "</DotNetHostPath></RunConfiguration><RunSettingsDirectory>",
                Path.GetDirectoryName(currentAssemblyLocation),
                "</RunSettingsDirectory></RunSettings>");

            Assert.AreEqual(expectedSettingsXml, finalSettingsXml);
        }
        finally
        {
            Environment.SetEnvironmentVariable("TEST_TEMP", null);
        }
    }
}
