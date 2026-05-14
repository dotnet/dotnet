// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

internal sealed class FileHelper
{
    private static readonly Dictionary<char, object?> InvalidFileNameChars;
    private static readonly Regex ReservedFileNamesRegex = new(@"(?i:^(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9]|CLOCK\$)(\..*)?)$");

    static FileHelper()
    {
        // Create a hash table of invalid chars.
        char[] invalidCharsArray = Path.GetInvalidFileNameChars();
        InvalidFileNameChars = new(invalidCharsArray.Length);
        foreach (char c in invalidCharsArray)
        {
            InvalidFileNameChars.Add(c, null);
        }
    }

    private FileHelper()
    {
    }
    /// <summary>
    /// Determines if a file name has invalid characters.
    /// </summary>
    /// <param name="fileName">File name to check.</param>
    /// <param name="invalidCharacters">Invalid characters which were found in the file name.</param>
    /// <returns>True if the file name is valid and false if the filename contains invalid characters.</returns>
    public static bool IsValidFileName(string fileName, out string? invalidCharacters)
    {
        bool result = true;
        //EqtAssert.StringNotNullOrEmpty(fileName, "fileName");

        // Find all of the invalid characters in the file name.
        invalidCharacters = null;
        for (int i = 0; i < fileName.Length; i++)
        {
            if (InvalidFileNameChars.ContainsKey(fileName[i]))
            {
                invalidCharacters = string.Concat(invalidCharacters, fileName[i]);
                result = false;
            }
        }

        return result;
    }

    /// <summary>
    /// Returns true if the file name specified is Windows reserved file name.
    /// </summary>
    /// <param name="fileName">The name of the file. Note: only a file name, does not expect to contain directory separators.</param>
    internal static bool IsReservedFileName(string fileName)
    {
        if (fileName.IsNullOrEmpty())
        {
            return false;
        }

        // CreateFile:
        // The following reserved device names cannot be used as the name of a file:
        // CON, PRN, AUX, NUL, COM1, COM2, COM3, COM4, COM5, COM6, COM7, COM8, COM9,
        // LPT1, LPT2, LPT3, LPT4, LPT5, LPT6, LPT7, LPT8, and LPT9.
        // Also avoid these names followed by an extension, for example, NUL.tx7.
        // Windows NT: CLOCK$ is also a reserved device name.
        return ReservedFileNamesRegex.Match(fileName).Success;
    }

}
