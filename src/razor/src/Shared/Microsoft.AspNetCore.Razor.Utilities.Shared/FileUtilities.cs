// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.IO;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal static class FileUtilities
{
    /// <summary>
    /// Generate a file path adjacent to the input path that has the
    /// specified file extension, using numbers to differentiate for
    /// any collisions.
    /// </summary>
    /// <param name="path">The input file path.</param>
    /// <param name="extension">The input file extension with a prepended ".".</param>
    /// <returns>A non-existent file path with a name in the specified format and a corresponding extension.</returns>
    public static string GenerateUniquePath(string path, string extension)
    {
        var directoryName = Path.GetDirectoryName(path).AssumeNotNull();
        var baseFileName = Path.GetFileNameWithoutExtension(path);

        var n = 0;
        string uniquePath;
        do
        {
            var identifier = n > 0 ? n.ToString(CultureInfo.InvariantCulture) : string.Empty;  // Make it look nice

            uniquePath = Path.Combine(directoryName, $"{baseFileName}{identifier}{extension}");
            n++;
        }
        while (File.Exists(uniquePath));

        return uniquePath;
    }
}
