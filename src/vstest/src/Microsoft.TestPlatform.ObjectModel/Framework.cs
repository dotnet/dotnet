// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

using System.Globalization;

using NuGet.Frameworks;

using static NuGet.Frameworks.FrameworkConstants;

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

        string name, version;
        try
        {
            // IDE always sends framework in form of ENUM, which always throws exception
            // This throws up in first chance exception, refer Bug https://devdiv.visualstudio.com/DefaultCollection/DevDiv/_workitems/edit/591142
            var formattedFrameworkString = frameworkString.Trim().ToLower(CultureInfo.InvariantCulture);
            switch (formattedFrameworkString)
            {
                case "framework35":
                    name = CommonFrameworks.Net35.DotNetFrameworkName;
                    version = CommonFrameworks.Net35.Version.ToString();
                    break;

                case "framework40":
                    name = CommonFrameworks.Net4.DotNetFrameworkName;
                    version = CommonFrameworks.Net4.Version.ToString();
                    break;

                case "framework45":
                    name = CommonFrameworks.Net45.DotNetFrameworkName;
                    version = CommonFrameworks.Net45.Version.ToString();
                    break;

                case "frameworkcore10":
                    name = CommonFrameworks.NetCoreApp10.DotNetFrameworkName;
                    version = CommonFrameworks.NetCoreApp10.Version.ToString();
                    break;

                case "frameworkuap10":
                    name = CommonFrameworks.UAP10.DotNetFrameworkName;
                    version = CommonFrameworks.UAP10.Version.ToString();
                    break;

                default:
                    var nugetFramework = NuGetFramework.Parse(frameworkString);
                    if (nugetFramework.IsUnsupported)
                        return null;

                    name = nugetFramework.DotNetFrameworkName;
                    version = nugetFramework.Version.ToString();

                    break;
            }
        }
        catch
        {
            return null;
        }

        return new Framework() { Name = name, Version = version };
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
