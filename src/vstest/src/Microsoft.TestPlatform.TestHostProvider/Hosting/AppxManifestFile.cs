// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Xml.Linq;

namespace Microsoft.VisualStudio.TestPlatform.DesktopTestHostRuntimeProvider;

/// <summary> Wrapper for an appx manifest file. </summary>
internal static class AppxManifestFile
{
    /// <summary> Gets the app's exe name. </summary>
    /// <param name="filePath">
    /// AppxManifest filePath
    /// </param>
    /// <returns>ExecutableName</returns>
    public static string? GetApplicationExecutableName(string filePath)
    {
        if (File.Exists(filePath))
        {
            var doc = XDocument.Load(filePath);
            var ns = doc.Root!.Name.Namespace;

            return doc.Element(ns + "Package")!.
                Element(ns + "Applications")!.
                Element(ns + "Application")!.
                Attribute("Executable")!.Value;
        }

        return null;
    }
}
