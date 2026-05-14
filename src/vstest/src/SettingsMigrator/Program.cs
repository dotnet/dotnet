// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using SettingsMigrator;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.SettingsMigrator.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.SettingsMigrator;

/// <summary>
/// Entry point for SettingsMigrator.
/// </summary>
public static class Program
{
    /// <summary>
    /// Main entry point. Hands off execution to Migrator.
    /// </summary>
    /// <param name="args">Arguments on the command line</param>
    /// <returns>Exit code</returns>
    public static int Main(string[] args)
    {
        var pathResolver = new PathResolver();
        string? newFilePath = pathResolver.GetTargetPath(args);

        if (!newFilePath.IsNullOrEmpty())
        {
            string oldFilePath = args[0];
            var migrator = new Migrator();
            migrator.Migrate(oldFilePath, newFilePath);
        }
        else
        {
            Console.WriteLine(CommandLineResources.ValidUsage);
            return 1;
        }

        return 0;
    }
}
