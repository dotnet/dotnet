// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

using ObjectModelResources = Microsoft.VisualStudio.TestPlatform.ObjectModel.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

/// <summary>
/// Utilities for the run settings XML.
/// </summary>
public static class XmlRunSettingsUtilities
{
    /// <summary>
    /// Gets the os architecture of the machine where this application is running
    /// </summary>
    public static Architecture OSArchitecture
    {
        get
        {
            var arch = new PlatformEnvironment().Architecture;

            return arch switch
            {
                PlatformArchitecture.X64 => Architecture.X64,
                PlatformArchitecture.X86 => Architecture.X86,
                PlatformArchitecture.ARM64 => Architecture.ARM64,
                PlatformArchitecture.ARM => Architecture.ARM,
                _ => Architecture.X64,
            };
        }
    }

    /// <summary>
    /// Gets the settings to be used while creating XmlReader for runsettings.
    /// </summary>
    public static XmlReaderSettings ReaderSettings
    {
        get
        {
            return new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true, DtdProcessing = DtdProcessing.Prohibit };
        }
    }

    /// <summary>
    /// Examines the given XPathNavigable representation of a runsettings file and determines if it has a configuration node
    /// for the data collector (used for Fakes and CodeCoverage)
    /// </summary>
    /// <param name="runSettingDocument"> XPathNavigable representation of a runsettings file </param>
    /// <param name="dataCollectorUri"> The data Collector Uri. </param>
    /// <returns> True if there is a datacollector configured. </returns>
    public static bool ContainsDataCollector(IXPathNavigable runSettingDocument, string dataCollectorUri)
    {
        ValidateArg.NotNull(runSettingDocument, nameof(runSettingDocument));
        ValidateArg.NotNull(dataCollectorUri, nameof(dataCollectorUri));

        var navigator = runSettingDocument.CreateNavigator();
        var nodes = navigator!.Select("/RunSettings/DataCollectionRunSettings/DataCollectors/DataCollector");

        foreach (XPathNavigator? dataCollectorNavigator in nodes)
        {
            var uri = dataCollectorNavigator?.GetAttribute("uri", string.Empty);
            if (string.Equals(dataCollectorUri, uri, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Get the list of friendly name of all data collector present in runsettings.
    /// </summary>
    /// <param name="runsettingsXml">runsettings xml string</param>
    /// <returns>List of friendly name</returns>
    public static IList<string> GetDataCollectorsFriendlyName(string? runsettingsXml)
    {
        var friendlyNameList = new List<string>();
        if (!runsettingsXml.IsNullOrWhiteSpace())
        {
            using var stream = new StringReader(runsettingsXml);
            using var reader = XmlReader.Create(stream, ReaderSettings);
            var document = new XmlDocument();
            document.Load(reader);

            var runSettingsNavigator = document.CreateNavigator();
            var nodes = runSettingsNavigator!.Select("/RunSettings/DataCollectionRunSettings/DataCollectors/DataCollector");

            foreach (XPathNavigator? dataCollectorNavigator in nodes)
            {
                var friendlyName = dataCollectorNavigator?.GetAttribute("friendlyName", string.Empty);
                friendlyNameList.Add(friendlyName!);
            }
        }

        return friendlyNameList;
    }

    /// <summary>
    /// Inserts a data collector settings in the file
    /// </summary>
    /// <param name="runSettingDocument">runSettingDocument</param>
    /// <param name="settings">settings</param>
    public static void InsertDataCollectorsNode(IXPathNavigable runSettingDocument, DataCollectorSettings settings)
    {
        ValidateArg.NotNull(runSettingDocument, nameof(runSettingDocument));
        ValidateArg.NotNull(settings, nameof(settings));

        var navigator = runSettingDocument.CreateNavigator()!;
        MoveToDataCollectorsNode(ref navigator);

        var settingsXml = settings.ToXml();
        var dataCollectorNode = settingsXml.CreateNavigator()!;
        dataCollectorNode.MoveToRoot();

        navigator.AppendChild(dataCollectorNode);
    }

    /// <summary>
    /// Returns RunConfiguration from settingsXml.
    /// </summary>
    /// <param name="settingsXml">The run settings.</param>
    /// <returns> The RunConfiguration node as defined in the settings xml.</returns>
    public static RunConfiguration GetRunConfigurationNode(string? settingsXml)
    {
        var nodeValue = GetNodeValue(settingsXml, Constants.RunConfigurationSettingsName, RunConfiguration.FromXml);
        if (nodeValue == default(RunConfiguration))
        {
            // Return default one.
            nodeValue = new RunConfiguration();
        }

        return nodeValue;
    }

    /// <summary>
    /// Gets the set of user defined test run parameters from settings xml as key value pairs.
    /// </summary>
    /// <param name="settingsXml">The run settings xml.</param>
    /// <returns>The test run parameters defined in the run settings.</returns>
    /// <remarks>If there is no test run parameters section defined in the settings xml a blank dictionary is returned.</remarks>
    public static Dictionary<string, object> GetTestRunParameters(string? settingsXml)
    {
        var nodeValue = GetNodeValue(settingsXml, Constants.TestRunParametersName, TestRunParameters.FromXml);
        if (nodeValue == default(Dictionary<string, object>))
        {
            // Return default.
            nodeValue = new Dictionary<string, object>();
        }

        return nodeValue;
    }

    /// <summary>
    /// Create a default run settings
    /// </summary>
    /// <returns>
    /// The <see cref="IXPathNavigable"/>.
    /// </returns>
    public static XmlDocument CreateDefaultRunSettings()
    {
        // Create a new default xml doc that looks like this:
        // <?xml version="1.0" encoding="utf-8"?>
        // <RunSettings>
        //   <DataCollectionRunSettings>
        //     <DataCollectors>
        //     </DataCollectors>
        //   </DataCollectionRunSettings>
        // </RunSettings>
        var doc = new XmlDocument();
        var xmlDeclaration = doc.CreateNode(XmlNodeType.XmlDeclaration, string.Empty, string.Empty);

        doc.AppendChild(xmlDeclaration);
        var runSettingsNode = doc.CreateElement(Constants.RunSettingsName);
        doc.AppendChild(runSettingsNode);

        var dataCollectionRunSettingsNode = doc.CreateElement(Constants.DataCollectionRunSettingsName);
        runSettingsNode.AppendChild(dataCollectionRunSettingsNode);

        var dataCollectorsNode = doc.CreateElement(Constants.DataCollectorsSettingName);
        dataCollectionRunSettingsNode.AppendChild(dataCollectorsNode);

        return doc;
    }

    /// <summary>
    /// Returns whether data collection is enabled in the parameter settings xml or not
    /// </summary>
    /// <param name="runSettingsXml"> The run Settings Xml. </param>
    /// <returns> True if data collection is enabled. </returns>
    public static bool IsDataCollectionEnabled(string? runSettingsXml)
    {
        var dataCollectionRunSettings = GetDataCollectionRunSettings(runSettingsXml);

        return dataCollectionRunSettings != null && dataCollectionRunSettings.IsCollectionEnabled;
    }

    /// <summary>
    /// Returns whether in-proc data collection is enabled in the parameter settings xml or not
    /// </summary>
    /// <param name="runSettingsXml"> The run Settings Xml. </param>
    /// <returns> True if data collection is enabled. </returns>
    public static bool IsInProcDataCollectionEnabled(string? runSettingsXml)
    {
        var dataCollectionRunSettings = GetInProcDataCollectionRunSettings(runSettingsXml);

        return dataCollectionRunSettings != null && dataCollectionRunSettings.IsCollectionEnabled;
    }

    /// <summary>
    /// Get DataCollection Run settings from the settings XML.
    /// </summary>
    /// <param name="runSettingsXml"> The run Settings Xml. </param>
    /// <returns> The <see cref="DataCollectionRunSettings"/>. </returns>
    public static DataCollectionRunSettings? GetDataCollectionRunSettings(string? runSettingsXml)
    {
        // use XmlReader to avoid loading of the plugins in client code (mainly from VS).
        if (runSettingsXml.IsNullOrWhiteSpace())
        {
            return null;
        }

        try
        {
            using var stringReader = new StringReader(runSettingsXml);
            var reader = XmlReader.Create(stringReader, ReaderSettings);

            // read to the fist child
            XmlReaderUtilities.ReadToRootNode(reader);
            reader.ReadToNextElement();

            // Read till we reach DC element or reach EOF
            while (!string.Equals(reader.Name, Constants.DataCollectionRunSettingsName) && !reader.EOF)
            {
                reader.SkipToNextElement();
            }

            // If reached EOF => DC element not there
            if (reader.EOF)
            {
                return null;
            }

            // Reached here => DC element present.
            return DataCollectionRunSettings.FromXml(reader);
        }
        catch (XmlException ex)
        {
            throw new SettingsException(string.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.CommonResources.MalformedRunSettingsFile, ex.Message), ex);
        }
    }

    /// <summary>
    /// Get InProc DataCollection Run settings
    /// </summary>
    /// <param name="runSettingsXml">
    /// The run Settings Xml.
    /// </param>
    /// <returns>Data collection run settings.</returns>
    public static DataCollectionRunSettings? GetInProcDataCollectionRunSettings(string? runSettingsXml)
    {
        // use XmlReader to avoid loading of the plugins in client code (mainly from VS).
        if (runSettingsXml.IsNullOrWhiteSpace())
        {
            return null;
        }

        runSettingsXml = runSettingsXml.Trim();
        using StringReader stringReader1 = new(runSettingsXml);
        XmlReader reader = XmlReader.Create(stringReader1, ReaderSettings);

        // read to the fist child
        XmlReaderUtilities.ReadToRootNode(reader);
        reader.ReadToNextElement();

        // Read till we reach In Proc IDC element or reach EOF
        while (!string.Equals(reader.Name, Constants.InProcDataCollectionRunSettingsName) && !reader.EOF)
        {
            reader.SkipToNextElement();
        }

        // If reached EOF => IDC element not there
        if (reader.EOF)
        {
            return null;
        }

        // Reached here => In Proc IDC element present.
        return DataCollectionRunSettings.FromXml(reader, Constants.InProcDataCollectionRunSettingsName, Constants.InProcDataCollectorsSettingName, Constants.InProcDataCollectorSettingName);
    }

    /// <summary>
    /// Get logger run settings from the settings XML.
    /// </summary>
    /// <param name="runSettings">The run Settings Xml.</param>
    /// <returns> The <see cref="LoggerRunSettings"/>. </returns>
    public static LoggerRunSettings? GetLoggerRunSettings(string? runSettings)
    {
        return GetNodeValue(
            runSettings,
            Constants.LoggerRunSettingsName,
            LoggerRunSettings.FromXml);
    }

    /// <summary>
    /// Throws a settings exception if the node the reader is on has attributes defined.
    /// </summary>
    /// <param name="reader"> The xml reader. </param>
    internal static void ThrowOnHasAttributes(XmlReader reader)
    {
        if (!reader.HasAttributes) return;

        string elementName = reader.Name;
        reader.MoveToNextAttribute();

        throw new SettingsException(
            string.Format(
                CultureInfo.CurrentCulture,
                ObjectModelResources.InvalidSettingsXmlAttribute,
                elementName,
                reader.Name));
    }

    /// <summary>
    /// Throws a settings exception if the node the reader is on doesn't have attributes defined.
    /// </summary>
    /// <param name="reader"> The xml reader. </param>
    internal static void ThrowOnNoAttributes(XmlReader reader)
    {
        if (reader.HasAttributes) return;

        throw new SettingsException(
            string.Format(
                CultureInfo.CurrentCulture,
                ObjectModelResources.InvalidSettingsXmlAttribute,
                reader.Name));
    }

    private static T? GetNodeValue<T>(string? settingsXml, string nodeName, Func<XmlReader, T> nodeParser)
    {
        // use XmlReader to avoid loading of the plugins in client code (mainly from VS).
        if (settingsXml.IsNullOrWhiteSpace())
        {
            return default;
        }

        try
        {
            using var stringReader = new StringReader(settingsXml);
            XmlReader reader = XmlReader.Create(stringReader, ReaderSettings);

            // read to the fist child
            XmlReaderUtilities.ReadToRootNode(reader);
            reader.ReadToNextElement();

            // Read till we reach nodeName element or reach EOF
            while (!string.Equals(reader.Name, nodeName, StringComparison.OrdinalIgnoreCase)
                   &&
                   !reader.EOF)
            {
                reader.SkipToNextElement();
            }

            if (!reader.EOF)
            {
                // read nodeName element.
                return nodeParser(reader);
            }
        }
        catch (XmlException ex)
        {
            throw new SettingsException(
                string.Format(CultureInfo.CurrentCulture, "{0} {1}", Resources.CommonResources.MalformedRunSettingsFile, ex.Message),
                ex);
        }

        return default;
    }

    /// <summary>
    /// Moves the given runsettings file navigator to the DataCollectors node in the runsettings xml.
    /// Throws XmlException if it was unable to find the DataCollectors node.
    /// </summary>
    /// <param name="runSettingsNavigator">XPathNavigator for a runsettings xml document.</param>
    private static void MoveToDataCollectorsNode(ref XPathNavigator runSettingsNavigator)
    {
        runSettingsNavigator.MoveToRoot();
        if (!runSettingsNavigator.MoveToChild("RunSettings", string.Empty))
        {
            throw new XmlException(string.Format(CultureInfo.CurrentCulture, ObjectModelResources.CouldNotFindXmlNode, "RunSettings"));
        }

        if (!runSettingsNavigator.MoveToChild("DataCollectionRunSettings", string.Empty))
        {
            runSettingsNavigator.AppendChildElement(string.Empty, "DataCollectionRunSettings", string.Empty, string.Empty);
            runSettingsNavigator.MoveToChild("DataCollectionRunSettings", string.Empty);
        }

        if (!runSettingsNavigator.MoveToChild("DataCollectors", string.Empty))
        {
            runSettingsNavigator.AppendChildElement(string.Empty, "DataCollectors", string.Empty, string.Empty);
            runSettingsNavigator.MoveToChild("DataCollectors", string.Empty);
        }
    }
}
