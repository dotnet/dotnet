// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Reflection;

namespace Utilities;

internal static class SlnAssets
{
    private const string SlnAssetsRoot = "SlnAssets.";
    private static readonly Assembly ResourceAssembly = typeof(SlnAssets).Assembly;

    public static ResourceName[] ClassicSlnFiles => GetAllSampleFiles(".sln").ToArray();

    #region Sample Classic Sln

    // Empty VS Solution template. ASCII encoding.
    public static ResourceStream ClassicSlnBlank => LoadResource("BlankSolution.sln");

    // Minimum Solution. UTF8 BOM encoding.
    public static ResourceStream ClassicSlnMin => LoadResource("Min.sln");

    // Medium SDK-style csproj solution. UTF8 BOM encoding.
    public static ResourceStream ClassicSlnCps => LoadResource("cps.sln");

    // Sample solution with multiple project types and shared project. UTF8 BOM encoding.
    public static ResourceStream ClassicSlnEverything => LoadResource("Everything.sln");

    public static ResourceStream ClassicSlnMany => LoadResource("SampleMany.sln");

    // Big SDK-style csproj solution. ASCII encoding.
    public static ResourceStream ClassicSlnOrchardCore => LoadResource("OrchardCore.sln");

    // A single C++ project with "Mobile"->"ARM64" platform, doesn't and build ARM64 sln platform. UTF8 BOM encoding.
    public static ResourceStream ClassicSlnSingleNativeProject => LoadResource("SingleNativeProject.sln");

    // A complex solution with multiple native and manager projects and configurations. UTF8 BOM encoding.
    public static ResourceStream ClassicSlnGiant => LoadResource("Giant.sln");

    // A larger solution with multiple .NET Framework project. ASCII encoding.
    public static ResourceStream ClassicSlnTraditional => LoadResource("Traditional.sln");

    // A solution with missing configurations. UTF8 BOM encoding.
    public static ResourceStream ClassicSlnMissingConfigurations => LoadResource("Configurations/MissingConfigurations.sln");

    #endregion

    #region Sample Xml Slnx

    // Solution with comments and user XML.
    public static ResourceStream XmlSlnxComments => LoadResource("Comments.slnx");

    // Solution with just a property bag and user XML.
    public static ResourceStream XmlSlnxJustProperties => LoadResource("SlnxWhitespace/JustProperties.slnx");

    // Solution with optional legacy values.
    public static ResourceStream XmlSlnxLegacyValues => LoadResource("Invalid/LegacyValues.slnx");

    // Solution with optional legacy values, but all obsolete values are removed.
    public static ResourceStream XmlSlnxLegacyValuesNoObsolete => LoadResource("LegacyValues-NoObsolete.slnx");

    // Solution with that was trimmed of all optional legacy values.
    public static ResourceStream XmlSlnxLegacyValuesTrimVS => LoadResource("LegacyValues-TrimVS.slnx");

    // Validate slnx file version
    public static ResourceStream XmlSlnxVersion => LoadResource("Version.slnx");

    public static ResourceStream XmlSlnxVersionMin => LoadResource("VersionMin.slnx");

    // A single C++ project with "Mobile"->"ARM64" platform, doesn't and build ARM64 sln platform.
    public static ResourceStream XmlSlnxSingleNativeProject => LoadResource("SingleNativeProject.slnx");

    // Empty VS Solution.
    public static ResourceStream XmlSlnxBlank => LoadResource("BlankSolution.slnx");

    // Medium SDK-style csproj solution.
    public static ResourceStream XmlSlnxCps => LoadResource("cps.slnx");

    // Sample solution with multiple project types.
    public static ResourceStream XmlSlnxEverything => LoadResource("Everything.slnx");

    public static ResourceStream XmlSlnxMany => LoadResource("SampleMany.slnx");

    // Big SDK-style csproj solution.
    public static ResourceStream XmlSlnxOrchardCore => LoadResource("OrchardCore.slnx");

    // Metadata for known project types.
    public static ResourceStream XmlBuiltInProjectTypes => LoadResource("Configurations/BuiltInProjectTypes.slnx");

    // A complex solution with multiple native and manager projects and configurations.
    public static ResourceStream XmlSlnxGiant => LoadResource("Giant.slnx");

    // A larger solution with multiple .NET Framework project.
    public static ResourceStream XmlSlnxTraditional => LoadResource("Traditional.slnx");

    // A solution with missing configurations.
    public static ResourceStream XmlSlnxMissingConfigurations => LoadResource("Configurations/MissingConfigurations.slnx");

    public static ResourceName[] XmlSlnxFiles => GetAllSampleFiles(".slnx").ToArray();

    #endregion

    #region Test result Slnx

    public static ResourceStream XmlSlnxProperties_Empty => LoadResource("SlnxWhitespace/JustProperties-empty.slnx");

    public static ResourceStream XmlSlnxProperties_Add0Add7 => LoadResource("SlnxWhitespace/JustProperties-add0add7.slnx");

    public static ResourceStream XmlSlnxProperties_No2No4 => LoadResource("SlnxWhitespace/JustProperties-no2no4.slnx");

    public static ResourceStream XmlSlnxProperties_NoComments => LoadResource("SlnxWhitespace/JustProperties-nocomments.slnx");

    #endregion

    public static IEnumerable<ResourceName> GetAllSampleFiles(string postFix)
    {
        string[] allResources = ResourceAssembly.GetManifestResourceNames();
        foreach (string fullResourceId in allResources)
        {
            StringSpan resourceName = fullResourceId.AsSpan();
            if (!resourceName.StartsWith(SlnAssetsRoot))
            {
                continue;
            }

            resourceName = resourceName.Slice(SlnAssetsRoot.Length);

            if (!resourceName.StartsWithIgnoreCase("Invalid") &&
                (resourceName.EndsWith(postFix + ".txt") || resourceName.EndsWith(postFix + ".xml")))
            {
                Stream? stream = ResourceAssembly.GetManifestResourceStream(fullResourceId);
                if (stream is not null)
                {
                    yield return new ResourceName(Path.GetFileNameWithoutExtension(resourceName).ToString(), fullResourceId);
                }
            }
        }
    }

    public static ResourceStream Load(this ResourceName resourceName)
    {
        return new ResourceStream(resourceName.Name, ResourceAssembly.GetManifestResourceStream(resourceName.FullResourceId)!);
    }

    public static ResourceStream LoadResource(string name)
    {
        name = name.Replace('/', '.');
        Stream? stream =
            ResourceAssembly.GetManifestResourceStream(SlnAssetsRoot + name + ".txt") ??
            ResourceAssembly.GetManifestResourceStream(SlnAssetsRoot + name + ".xml");
        if (stream is not null)
        {
            return new ResourceStream(name, stream);
        }

        // Create an error message to help diagnose the missing resource.
        string[] allResources = ResourceAssembly.GetManifestResourceNames();
        StringBuilder errorMessage = new StringBuilder($"Resource '{name}' not found.");
        _ = errorMessage.AppendLine("Resource found:");
        foreach (string resource in allResources)
        {
            _ = errorMessage.AppendLine(resource);
        }

        throw new InvalidOperationException(errorMessage.ToString());
    }
}
