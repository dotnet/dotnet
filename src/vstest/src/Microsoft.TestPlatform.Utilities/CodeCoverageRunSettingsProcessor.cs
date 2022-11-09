// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

/// <summary>
/// Represents the run settings processor for code coverage data collectors.
/// </summary>
public class CodeCoverageRunSettingsProcessor
{
    /// <summary>
    /// Represents the default settings loaded as an <see cref="XmlNode"/>.
    /// </summary>
    private readonly XmlNode _defaultSettingsRootNode;
    /// <summary>
    /// Constructs an <see cref="CodeCoverageRunSettingsProcessor"/> object.
    /// </summary>
    ///
    /// <param name="defaultSettingsRootNode">The default settings root node.</param>
    public CodeCoverageRunSettingsProcessor(XmlNode defaultSettingsRootNode)
    {
        _defaultSettingsRootNode = defaultSettingsRootNode ?? throw new ArgumentNullException(nameof(defaultSettingsRootNode));
    }

    #region Public Interface
    /// <summary>
    /// Processes the current settings for the code coverage data collector.
    /// </summary>
    ///
    /// <param name="currentSettings">The code coverage settings.</param>
    ///
    /// <returns>An updated version of the current run settings.</returns>
    public XmlNode? Process(string? currentSettings)
    {
        if (currentSettings.IsNullOrEmpty())
        {
            return null;
        }

        // Load current settings from string.
        var document = new XmlDocument();
        document.LoadXml(currentSettings);

        return Process(document.DocumentElement);
    }

    /// <summary>
    /// Processes the current settings for the code coverage data collector.
    /// </summary>
    ///
    /// <param name="currentSettingsDocument">
    /// The code coverage settings document.
    /// </param>
    ///
    /// <returns>An updated version of the current run settings.</returns>
    public XmlNode? Process(XmlDocument? currentSettingsDocument)
    {
        return currentSettingsDocument == null ? null : Process(currentSettingsDocument.DocumentElement);
    }

    /// <summary>
    /// Processes the current settings for the code coverage data collector.
    /// </summary>
    ///
    /// <param name="currentSettingsRootNode">The code coverage root element.</param>
    ///
    /// <returns>An updated version of the current run settings.</returns>
    public XmlNode? Process(XmlNode? currentSettingsRootNode)
    {
        if (currentSettingsRootNode == null)
        {
            return null;
        }

        // Get the code coverage node from the current settings. If unable to get any
        // particular component down the path just add the default values for that component
        // from the default settings document and return since there's nothing else to be done.
        var codeCoveragePathComponents = new List<string>() { "CodeCoverage" };
        var currentCodeCoverageNode = SelectNodeOrAddDefaults(
            currentSettingsRootNode,
            _defaultSettingsRootNode,
            codeCoveragePathComponents);

        // Cannot extract current code coverage node from the given settings so we bail out.
        // However, the default code coverage node has already been added to the document's
        // root.
        if (currentCodeCoverageNode == null)
        {
            return currentSettingsRootNode;
        }

        // Get the code coverage node from the default settings.
        var defaultCodeCoverageNode = ExtractNode(
            _defaultSettingsRootNode,
            BuildPath(codeCoveragePathComponents));

        // Create the exclusion type list.
        var exclusions = new List<IList<string>>
        {
            new List<string> { "ModulePaths", "Exclude" },
            new List<string> { "Attributes", "Exclude" },
            new List<string> { "Sources", "Exclude" },
            new List<string> { "Functions", "Exclude" }
        };

        foreach (var exclusion in exclusions)
        {
            // Get the <Exclude> node for the current exclusion type. If unable to get any
            // particular component down the path just add the default values for that
            // component from the default settings document and continue since there's nothing
            // else to be done.
            var currentNode = SelectNodeOrAddDefaults(
                currentCodeCoverageNode,
                defaultCodeCoverageNode,
                exclusion);

            // Check if the node extraction was successful and we should process the current
            // node in order to merge the current exclusion rules with the default ones.
            if (currentNode == null)
            {
                continue;
            }

            // Extract the <Exclude> node from the default settings.
            var defaultNode = ExtractNode(
                defaultCodeCoverageNode,
                BuildPath(exclusion));

            // Merge the current and default settings for the current exclusion rule.
            MergeNodes(currentNode, defaultNode);
        }

        return currentSettingsRootNode;
    }
    #endregion

    /// <summary>
    /// Selects the node from the current settings node using the given
    /// <see cref="XPathNavigator"/> style path. If unable to select the requested node it adds
    /// default settings along the path.
    /// </summary>
    ///
    /// <param name="currentRootNode">
    /// The root node from the current settings document for the extraction.
    /// </param>
    /// <param name="defaultRootNode">
    /// The corresponding root node from the default settings document.
    /// </param>
    /// <param name="pathComponents">The path components.</param>
    ///
    /// <returns>The requested node if successful, <see cref="null"/> otherwise.</returns>
    private static XmlNode? SelectNodeOrAddDefaults(
        XmlNode currentRootNode,
        XmlNode? defaultRootNode,
        IList<string> pathComponents)
    {
        var currentNode = currentRootNode;
        var partialPath = new StringBuilder();

        partialPath.Append('.');

        foreach (var component in pathComponents)
        {
            var currentPathComponent = "/" + component;

            // Append the current path component to the partial path.
            partialPath.Append(currentPathComponent);

            // Extract the node corresponding to the latest path component.
            var tempNode = ExtractNode(currentNode, "." + currentPathComponent);

            // Extraction is pruned here because we shouldn't be processing the current node.
            if (tempNode != null && !ShouldProcessCurrentExclusion(tempNode))
            {
                return null;
            }

            // If the current node extraction is unsuccessful then add the corresponding
            // default settings node and bail out.
            if (tempNode == null)
            {
                var defaultNode = ExtractNode(defaultRootNode, partialPath.ToString());
                if (defaultNode == null)
                {
                    return null;
                }

                var importedChild = currentNode.OwnerDocument!.ImportNode(defaultNode, true);
                currentNode.AppendChild(importedChild);

                return null;
            }

            // Node corresponding to the latest path component is the new root node for the
            // next extraction.
            currentNode = tempNode;
        }

        return currentNode;
    }

    /// <summary>
    /// Checks if we should process the current exclusion node.
    /// </summary>
    ///
    /// <param name="node">The current exclusion node.</param>
    ///
    /// <returns>
    /// <see cref="true"/> if the node should be processed, <see cref="false"/> otherwise.
    /// </returns>
    private static bool ShouldProcessCurrentExclusion(XmlNode node)
    {
        const string attributeName = "mergeDefaults";

        if (node.Attributes == null)
        {
            return true;
        }

        foreach (XmlAttribute attribute in node.Attributes)
        {
            // If the attribute is present and set on 'false' we skip processing for the
            // current exclusion.
            if (attribute.Name == attributeName
                && bool.TryParse(attribute.Value, out var value)
                && !value)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Assembles a relative path from the path given as components.
    /// </summary>
    ///
    /// <returns>A relative path built from path components.</returns>
    private static string BuildPath(IList<string> pathComponents)
    {
        return string.Join("/", new[] { "." }.Concat(pathComponents));
    }

    /// <summary>
    /// Extracts the node specified by the current path using the provided node as root.
    /// </summary>
    ///
    /// <param name="node">The root to be used for extraction.</param>
    /// <param name="path">The path used to specify the requested node.</param>
    ///
    /// <returns>The extracted node if successful, <see cref="null"/> otherwise.</returns>
    private static XmlNode? ExtractNode(XmlNode? node, string path)
    {
        try
        {
            return node?.SelectSingleNode(path);
        }
        catch (XPathException ex)
        {
            EqtTrace.Error(
                "CodeCoverageRunSettingsProcessor.ExtractNode: Cannot select single node \"{0}\".",
                ex.Message);
        }

        return null;
    }

    /// <summary>
    /// Merges the current settings rules with the default settings rules.
    /// </summary>
    ///
    /// <param name="currentNode">The current settings root node.</param>
    /// <param name="defaultNode">The default settings root node.</param>
    private static void MergeNodes(XmlNode currentNode, XmlNode? defaultNode)
    {
        var exclusionCache = new HashSet<string>();

        // Add current exclusions to the exclusion cache.
        foreach (XmlNode child in currentNode.ChildNodes)
        {
            exclusionCache.Add(child.OuterXml);
        }

        if (defaultNode is null)
        {
            return;
        }

        // Iterate through default exclusions and import missing ones.
        foreach (XmlNode child in defaultNode.ChildNodes)
        {
            if (exclusionCache.Contains(child.OuterXml))
            {
                continue;
            }

            // Import missing default exclusions.
            var importedChild = currentNode.OwnerDocument!.ImportNode(child, true);
            currentNode.AppendChild(importedChild);
        }
    }
}
