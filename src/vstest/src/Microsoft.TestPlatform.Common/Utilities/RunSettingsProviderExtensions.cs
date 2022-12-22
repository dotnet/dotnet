// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.Common.Utilities;

/// <summary>
/// Utilities to get the run settings from the provider and the command line options specified.
/// </summary>
internal static class RunSettingsProviderExtensions
{
    public const string EmptyRunSettings = @"<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";

    /// <summary>
    /// Pattern used to find parameter node.
    /// </summary>
    private const string ParameterString = "Parameter";

    /// <summary>
    /// Pattern that indicates Attribute name.
    /// </summary>
    private const string AttributeNameString = "AttrName";

    /// <summary>
    /// Pattern that indicates  Attribute value.
    /// </summary>
    private const string AttributeValueString = "AttrValue";

    /// <summary>
    /// Attribute name key for test run parameter node
    /// </summary>
    private const string NameString = "name";

    /// <summary>
    /// Attribute value key for test run parameter node
    /// </summary>
    private const string ValueString = "value";

    public static void UpdateRunSettings(this IRunSettingsProvider runSettingsProvider, string runsettingsXml)
    {
        ValidateArg.NotNull(runSettingsProvider, nameof(runSettingsProvider));
        ValidateArg.NotNullOrWhiteSpace(runsettingsXml, nameof(runsettingsXml));

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(runsettingsXml);
        runSettingsProvider.SetActiveRunSettings(runSettings);
    }

    public static void AddDefaultRunSettings(this IRunSettingsProvider runSettingsProvider)
    {
        ValidateArg.NotNull(runSettingsProvider, nameof(runSettingsProvider));
        var runSettingsXml = runSettingsProvider.ActiveRunSettings?.SettingsXml;

        if (runSettingsXml.IsNullOrWhiteSpace())
        {
            runSettingsXml = EmptyRunSettings;
        }

        runSettingsXml = AddDefaultRunSettings(runSettingsXml);
        runSettingsProvider.UpdateRunSettings(runSettingsXml);
    }

    public static void UpdateRunSettingsXmlDocumentInnerXml(XmlDocument xmlDocument, string key, string data)
    {
        var node = GetXmlNode(xmlDocument, key) ?? CreateNode(xmlDocument, key);
        TPDebug.Assert(node is not null, "node is null");
        node.InnerXml = data;
    }

    public static void UpdateRunSettingsNode(this IRunSettingsProvider runSettingsProvider, string key, string data)
    {
        ValidateArg.NotNull(runSettingsProvider, nameof(runSettingsProvider));
        ValidateArg.NotNullOrWhiteSpace(key, nameof(key));
        ValidateArg.NotNull(data, nameof(data));

        var xmlDocument = runSettingsProvider.GetRunSettingXmlDocument();
        UpdateRunSettingsXmlDocumentInnerText(xmlDocument, key, data);
        runSettingsProvider.UpdateRunSettings(xmlDocument.OuterXml);
    }

    /// <summary>
    /// Matches with test run parameter node pattern and returns that match.
    /// </summary>
    /// <param name="runSettingsProvider"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static Match GetTestRunParameterNodeMatch(this IRunSettingsProvider _, string node)
    {
        var attrName = $"(?<{AttributeNameString}>[\\w.:-]+)";
        var attrValue = $"(?<{AttributeValueString}>.+)";
        Regex regex = new($"{Constants.TestRunParametersName}.{ParameterString}\\(name\\s*=\\s*\"{attrName}\"\\s*,\\s*value\\s*=\\s*\"{attrValue}\"\\)");
        Match match = regex.Match(node);
        return match;
    }

    /// <summary>
    /// If test run parameter exists already it will override with new value otherwise this will add new test run parameter.
    /// </summary>
    /// <param name="runSettingsProvider"></param>
    /// <param name="match"></param>
    public static void UpdateTestRunParameterSettingsNode(this IRunSettingsProvider runSettingsProvider, Match match)
    {
        ValidateArg.NotNull(runSettingsProvider, nameof(runSettingsProvider));
        var xmlDocument = runSettingsProvider.GetRunSettingXmlDocument();
        XmlNode testRunParameterNode = GetXmlNode(xmlDocument, Constants.TestRunParametersName) ?? xmlDocument.CreateElement(Constants.TestRunParametersName);
        var attrName = match.Groups[AttributeNameString].Value;
        var attrValue = match.Groups[AttributeValueString].Value;

        if (!TryOverrideAttributeValue(testRunParameterNode, attrName, attrValue))
        {
            XmlElement element = xmlDocument.CreateElement(ParameterString);
            element.SetAttribute(NameString, attrName);
            element.SetAttribute(ValueString, attrValue);
            testRunParameterNode.AppendChild(element);
            xmlDocument.DocumentElement!.AppendChild(testRunParameterNode);
        }

        runSettingsProvider.UpdateRunSettings(xmlDocument.OuterXml);
    }

    private static bool TryOverrideAttributeValue(XmlNode xmlNode, string attrName, string attrValue)
    {
        foreach (XmlNode node in xmlNode.ChildNodes)
        {
            if (string.Compare(node.Attributes![NameString]!.Value, attrName) == 0)
            {
                node.Attributes[ValueString]!.Value = attrValue;
                return true;
            }
        }

        return false;
    }

    public static void UpdateRunSettingsNodeInnerXml(this IRunSettingsProvider runSettingsProvider, string key, string xml)
    {
        ValidateArg.NotNull(runSettingsProvider, nameof(runSettingsProvider));
        ValidateArg.NotNullOrWhiteSpace(key, nameof(key));
        ValidateArg.NotNull(xml, nameof(xml));

        var xmlDocument = runSettingsProvider.GetRunSettingXmlDocument();
        UpdateRunSettingsXmlDocumentInnerXml(xmlDocument, key, xml);
        runSettingsProvider.UpdateRunSettings(xmlDocument.OuterXml);
    }

    public static string? QueryRunSettingsNode(this IRunSettingsProvider runSettingsProvider, string key)
    {
        ValidateArg.NotNull(runSettingsProvider, nameof(runSettingsProvider));
        ValidateArg.NotNullOrWhiteSpace(key, nameof(key));

        var xmlDocument = runSettingsProvider.GetRunSettingXmlDocument();
        var node = GetXmlNode(xmlDocument, key);
        return node?.InnerText;
    }

    internal static XmlNode? GetXmlNode(XmlDocument xmlDocument, string key)
    {
        var xPath = key.Replace('.', '/');
        var node = xmlDocument.SelectSingleNode($"//RunSettings/{xPath}");
        return node;
    }

    internal static void UpdateRunSettingsXmlDocumentInnerText(XmlDocument xmlDocument, string key, string data)
    {
        var node = GetXmlNode(xmlDocument, key) ?? CreateNode(xmlDocument, key);
        TPDebug.Assert(node is not null, "node is null");
        node.InnerText = data;
    }

    /// <summary>
    /// Gets the effective run settings adding the default run settings if not already present.
    /// </summary>
    /// <param name="runSettings"> The run settings XML. </param>
    /// <returns> Effective run settings. </returns>
    private static string AddDefaultRunSettings(string runSettings)
    {
        var architecture = Constants.DefaultPlatform;
        var framework = Framework.DefaultFramework;
        var defaultResultsDirectory = Path.Combine(Directory.GetCurrentDirectory(), Constants.ResultsDirectoryName);

        using var stream = new StringReader(runSettings);
        using var reader = XmlReader.Create(stream, XmlRunSettingsUtilities.ReaderSettings);
        var document = new XmlDocument();
        document.Load(reader);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(document, architecture, framework, defaultResultsDirectory);
        return document.OuterXml;
    }

    private static XmlNode? CreateNode(XmlDocument doc, string xPath)
    {
        var path = xPath.Split('.');
        XmlNode? node = null;
        XmlNode? parent = doc.DocumentElement!;

        for (var i = 0; i < path.Length; i++)
        {
            TPDebug.Assert(parent is not null, "parent is null");
            node = parent.SelectSingleNode(path[i]) ?? parent.AppendChild(doc.CreateElement(path[i]));
            parent = node;
        }

        return node;
    }

    private static XmlDocument GetRunSettingXmlDocument(this IRunSettingsProvider runSettingsProvider)
    {
        ValidateArg.NotNull(runSettingsProvider, nameof(runSettingsProvider));
        var doc = new XmlDocument();

        if (!StringUtils.IsNullOrEmpty(runSettingsProvider.ActiveRunSettings?.SettingsXml))
        {
            var settingsXml = runSettingsProvider.ActiveRunSettings.SettingsXml;

#if NETFRAMEWORK
            using var reader = XmlReader.Create(new StringReader(settingsXml), new XmlReaderSettings() { XmlResolver = null, CloseInput = true, DtdProcessing = DtdProcessing.Prohibit });
#else
            using var reader = XmlReader.Create(new StringReader(settingsXml),
                new XmlReaderSettings() { CloseInput = true, DtdProcessing = DtdProcessing.Prohibit });
#endif
            doc.Load(reader);
        }
        else
        {
#if NETFRAMEWORK
            doc = (XmlDocument)XmlRunSettingsUtilities.CreateDefaultRunSettings();
#else
            using var reader =
                XmlReader.Create(
                    new StringReader(
                        XmlRunSettingsUtilities.CreateDefaultRunSettings().CreateNavigator()!.OuterXml),
                    new XmlReaderSettings() { CloseInput = true, DtdProcessing = DtdProcessing.Prohibit });
            doc.Load(reader);
#endif
        }
        return doc;
    }
}
