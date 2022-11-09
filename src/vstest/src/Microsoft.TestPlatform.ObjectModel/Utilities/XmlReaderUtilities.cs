// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

/// <summary>
/// Utility methods for working with an XmlReader.
/// </summary>
public static class XmlReaderUtilities
{
    private const string RunSettingsRootNodeName = "RunSettings";

    /// <summary>
    /// Reads up to the next Element in the document.
    /// </summary>
    /// <param name="reader">Reader to move to the next element.</param>
    public static void ReadToNextElement(this XmlReader reader)
    {
        ValidateArg.NotNull(reader, nameof(reader));
        while (!reader.EOF && reader.Read() && reader.NodeType != XmlNodeType.Element)
        {
        }
    }

    /// <summary>
    /// Skips the current element and moves to the next Element in the document.
    /// </summary>
    /// <param name="reader">Reader to move to the next element.</param>
    public static void SkipToNextElement(this XmlReader reader)
    {
        ValidateArg.NotNull(reader, nameof(reader));
        reader.Skip();

        if (reader.NodeType != XmlNodeType.Element)
        {
            reader.ReadToNextElement();
        }
    }

    /// <summary>
    /// Reads to the root node of the run settings and verifies that it is a "RunSettings" node.
    /// </summary>
    /// <param name="path">Path to the file.</param>
    /// <param name="reader">XmlReader for the file.</param>
    public static void ReadToRootNode(XmlReader reader)
    {
        ValidateArg.NotNull(reader, nameof(reader));

        // Read to the root node.
        reader.ReadToNextElement();

        // Verify that it is a "RunSettings" node.
        if (reader.Name != RunSettingsRootNodeName)
        {
            throw new SettingsException(Resources.Resources.InvalidRunSettingsRootNode);
        }
    }

}
