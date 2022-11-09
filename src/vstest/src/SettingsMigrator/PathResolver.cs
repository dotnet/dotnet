// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace Microsoft.VisualStudio.TestPlatform.SettingsMigrator;

/// <summary>
/// Used to resolve the inputs provided by the user to paths needed by migrator.
/// </summary>
public class PathResolver
{
    private const string RunSettingsExtension = ".runsettings";

    /// <summary>
    /// Gets the target path based on user inputs.
    /// </summary>
    /// <param name="args">User inputs</param>
    /// <returns>New file path to create</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Part of the public API")]
    public string? GetTargetPath(string[] args)
    {
        string? newFilePath = null;
        if (args.Length < 1 || !Path.IsPathRooted(args[0]))
        {
            return newFilePath;
        }

        if (args.Length == 1)
        {
            var oldFilePath = args[0];
            var newFileName = string.Concat(Path.GetFileNameWithoutExtension(oldFilePath), "_", DateTime.Now.ToString("MM-dd-yyyy_hh-mm-ss", CultureInfo.CurrentCulture), RunSettingsExtension);
            newFilePath = Path.Combine(Path.GetDirectoryName(oldFilePath)!, newFileName);
        }
        else if (args.Length == 2)
        {
            newFilePath = args[1];
            if (!Path.IsPathRooted(newFilePath) || !string.Equals(Path.GetExtension(newFilePath), RunSettingsExtension, StringComparison.OrdinalIgnoreCase))
            {
                newFilePath = null;
            }
        }

        return newFilePath;
    }
}
