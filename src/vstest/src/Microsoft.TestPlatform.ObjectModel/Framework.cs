// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

using System.Globalization;

using NuGetClone.Frameworks;
using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Class for target Framework for the test container
/// </summary>
public class Framework
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. | Suppressed as we know values are set
    private Framework()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    /// <summary>
    /// Default .Net target framework.
    /// </summary>
#if NETFRAMEWORK
    public static Framework DefaultFramework { get; } = Framework.FromString(".NETFramework,Version=v4.0")!;
#else
    public static Framework DefaultFramework { get; } = Framework.FromString(".NETCoreApp,Version=v1.0")!;
#endif

    /// <summary>
    /// Gets the FullName of framework.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the framework name such as .NETCoreApp.
    /// </summary>
    public string FrameworkName { get; private set; }

    /// <summary>
    /// Common short name, as well as directory name, such as net5.0. Is null when the framework is not correct.
    /// </summary>
    public string? ShortName { get; private set; }

    /// <summary>
    /// Gets the framework version.
    /// </summary>
    public string Version { get; private set; }

    /// <summary>
    /// Returns a valid framework or null when it cannot parse it, or when given null.
    /// </summary>
    /// <param name="frameworkString">Framework name</param>
    /// <returns>A framework object</returns>
    public static Framework? FromString(string? frameworkString)
    {
        if (frameworkString.IsNullOrWhiteSpace())
        {
            return null;
        }

        string name, frameworkName, version;
        string? shortName = null;
        try
        {
            // IDE always sends framework in form of ENUM, which always throws exception
            // This throws up in first chance exception, refer Bug https://devdiv.visualstudio.com/DefaultCollection/DevDiv/_workitems/edit/591142
            var formattedFrameworkString = frameworkString.Trim().ToLower(CultureInfo.InvariantCulture);
            string? mappedShortName = null;
            switch (formattedFrameworkString)
            {
                case "framework35":
                    mappedShortName = "net3.5";
                    break;

                case "framework40":
                    mappedShortName = "net4.0";
                    break;

                case "framework45":
                    mappedShortName = "net4.5";
                    break;

                case "frameworkcore10":
                    mappedShortName = "netcoreapp1.0";
                    break;

                case "frameworkuap10":
                    mappedShortName = "uap10.0";
                    break;
            }

            if (mappedShortName != null)
            {
                frameworkString = mappedShortName;
            }

            var nugetFramework = NuGetFramework.Parse(frameworkString);
            if (nugetFramework.IsUnsupported)
                return null;

            // e.g. .NETFramework,Version=v3.5
            name = nugetFramework.DotNetFrameworkName;
            // e.g. net35
            try
            {
                // .NETPortable4.5 for example, is not a valid framework
                // and this will throw.
                shortName = nugetFramework.GetShortFolderName();
            }
            catch (Exception ex)
            {
                EqtTrace.Error(ex);
            }
            // e.g. .NETFramework
            frameworkName = nugetFramework.Framework;
            // e.g. 3.5.0.0
            version = nugetFramework.Version.ToString();

        }
        catch
        {
            return null;
        }

        return new Framework() { Name = name, ShortName = shortName, FrameworkName = frameworkName, Version = version };
    }

    /// <summary>
    /// Returns full name of the framework.
    /// </summary>
    /// <returns>String presentation of the object.</returns>
    public override string ToString()
    {
        return Name;
    }
}
