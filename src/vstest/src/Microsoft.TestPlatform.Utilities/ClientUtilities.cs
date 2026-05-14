// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

/// <summary>
/// Utilities used by the client to understand the environment of the current run.
/// </summary>
public static class ClientUtilities
{
    private const string TestSettingsFileXPath = "RunSettings/MSTest/SettingsFile";
    private const string ResultsDirectoryXPath = "RunSettings/RunConfiguration/ResultsDirectory";
    private const string DotnetHostPathXPath = "RunSettings/RunConfiguration/DotNetHostPath";
    private const string RunsettingsDirectory = "RunSettingsDirectory";

    /// <summary>
    /// Converts the relative paths in a runsetting file to absolute ones.
    /// </summary>
    /// <param name="xmlDocument">Xml Document containing Runsettings xml</param>
    /// <param name="path">Path of the .runsettings xml file</param>
    public static void FixRelativePathsInRunSettings(XmlDocument xmlDocument, string path)
    {
        ValidateArg.NotNull(xmlDocument, nameof(xmlDocument));
        ValidateArg.NotNullOrEmpty(path, nameof(path));

        var root = Path.GetDirectoryName(path)!;

        AddRunSettingsDirectoryNode(xmlDocument, root);

        var testRunSettingsNode = xmlDocument.SelectSingleNode(TestSettingsFileXPath);
        if (testRunSettingsNode != null)
        {
            FixNodeFilePath(testRunSettingsNode, root);
        }

        var resultsDirectoryNode = xmlDocument.SelectSingleNode(ResultsDirectoryXPath);
        if (resultsDirectoryNode != null)
        {
            FixNodeFilePath(resultsDirectoryNode, root);
        }

        var dotnetHostPathNode = xmlDocument.SelectSingleNode(DotnetHostPathXPath);
        if (dotnetHostPathNode != null)
        {
            FixNodeFilePath(dotnetHostPathNode, root);
        }
    }

    private static void AddRunSettingsDirectoryNode(XmlDocument doc, string path)
    {
        var node = doc.CreateNode(XmlNodeType.Element, RunsettingsDirectory, string.Empty);
        node.InnerXml = path;
        doc.DocumentElement!.AppendChild(node);
    }

    private static void FixNodeFilePath(XmlNode node, string root)
    {
        string fileName = node.InnerXml;
        fileName = Environment.ExpandEnvironmentVariables(fileName);

        if (!fileName.IsNullOrEmpty()
            && !Path.IsPathRooted(fileName))
        {
            // We have a relative file path...
            fileName = Path.Combine(root, fileName);
            fileName = Path.GetFullPath(fileName);
        }

        node.InnerXml = fileName;
    }
}
