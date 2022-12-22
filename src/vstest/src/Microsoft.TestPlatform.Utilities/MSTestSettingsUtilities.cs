// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using UtilitiesResources = Microsoft.VisualStudio.TestPlatform.Utilities.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

/// <summary>
/// The legacy mstest.exe settings utilities.
/// </summary>
public static class MSTestSettingsUtilities
{
    /// <summary>
    /// Imports the parameter settings file in the default runsettings.
    /// </summary>
    /// <param name="settingsFile">
    /// Settings file which need to be imported. The file extension of the settings file will be specified by <paramref name="SettingsFileExtension"/> property.
    /// </param>
    /// <param name="defaultRunSettings"> Input RunSettings document to which settings file need to be imported. </param>
    /// <returns> Updated RunSetting Xml document with imported settings. </returns>
    public static XmlDocument Import(string settingsFile, XmlDocument defaultRunSettings)
    {
        ValidateArg.NotNull(settingsFile, nameof(settingsFile));
        ValidateArg.NotNull(defaultRunSettings, nameof(defaultRunSettings));

        if (IsLegacyTestSettingsFile(settingsFile) == false)
        {
            throw new XmlException(string.Format(CultureInfo.CurrentCulture, UtilitiesResources.UnExpectedSettingsFile));
        }

        var navigator = defaultRunSettings.CreateNavigator()!;

        if (!navigator.MoveToChild(Constants.RunSettingsName, string.Empty))
        {
            throw new XmlException(UtilitiesResources.NoRunSettingsNodeFound);
        }

        var settingsNode = GenerateMsTestXml(settingsFile);

        defaultRunSettings.DocumentElement!.PrependChild(defaultRunSettings.ImportNode(settingsNode, true));

        // Adding RunConfig
        if (!navigator.MoveToChild(Constants.RunConfigurationSettingsName, string.Empty))
        {
            var doc = new XmlDocument();
            var runConfigurationNode = doc.CreateElement(Constants.RunConfigurationSettingsName);

            defaultRunSettings.DocumentElement.PrependChild(defaultRunSettings.ImportNode(runConfigurationNode, true));
        }

        return defaultRunSettings;
    }

    public static bool IsLegacyTestSettingsFile(string? settingsFile)
    {
        return string.Equals(Path.GetExtension(settingsFile), ".testSettings", StringComparison.OrdinalIgnoreCase)
            || string.Equals(Path.GetExtension(settingsFile), ".testrunConfig", StringComparison.OrdinalIgnoreCase)
            || string.Equals(Path.GetExtension(settingsFile), ".vsmdi", StringComparison.OrdinalIgnoreCase);
    }

    private static XmlElement GenerateMsTestXml(string settingsFile)
    {
        // Generate the MSTest xml
        //
        // <MSTest>
        //   <TestSettingsFile>C:\local.testsettings</TestSettingsFile>
        //   <ForcedLegacyMode>true</ForcedLegacyMode>
        // </MSTest>
        //
        XmlDocument doc = new();
        XmlElement mstestNode = doc.CreateElement("MSTest");

        XmlElement testSettingsFileNode = doc.CreateElement("SettingsFile");
        testSettingsFileNode.InnerXml = settingsFile;
        mstestNode.AppendChild(testSettingsFileNode);

        XmlElement forcedLegacyModeNode = doc.CreateElement("ForcedLegacyMode");
        forcedLegacyModeNode.InnerXml = "true";
        mstestNode.AppendChild(forcedLegacyModeNode);

        return mstestNode;
    }
}
