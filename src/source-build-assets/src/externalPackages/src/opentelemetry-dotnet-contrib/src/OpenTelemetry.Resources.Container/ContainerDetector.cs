// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.RegularExpressions;
using OpenTelemetry.Resources.Container.Utils;

namespace OpenTelemetry.Resources.Container;

/// <summary>
/// Resource detector for application running in Container environment.
/// </summary>
internal sealed partial class ContainerDetector : IResourceDetector
{
    private const string Filepath = "/proc/self/cgroup";
    private const string FilepathV2 = "/proc/self/mountinfo";
    private const string Hostname = "hostname";

    /// <summary>
    /// CGroup Parse Versions.
    /// </summary>
    internal enum ParseMode
    {
        /// <summary>
        /// Represents CGroupV1.
        /// </summary>
        V1,

        /// <summary>
        /// Represents CGroupV2.
        /// </summary>
        V2,
    }

    /// <summary>
    /// Detects the resource attributes from Container.
    /// </summary>
    /// <returns>Resource with key-value pairs of resource attributes.</returns>
    public Resource Detect()
    {
        var cGroupBuild = this.BuildResource(Filepath, ParseMode.V1);
        if (cGroupBuild == Resource.Empty)
        {
            cGroupBuild = this.BuildResource(FilepathV2, ParseMode.V2);
        }

        return cGroupBuild;
    }

    /// <summary>
    /// Builds the resource attributes from Container Id in file path.
    /// </summary>
    /// <param name="path">File path where container id exists.</param>
    /// <param name="cgroupVersion">CGroup Version of file to parse from.</param>
    /// <returns>Returns Resource with list of key-value pairs of container resource attributes if container id exists else empty resource.</returns>
    internal Resource BuildResource(string path, ParseMode cgroupVersion)
    {
        var containerId = this.ExtractContainerId(path, cgroupVersion);

        return string.IsNullOrEmpty(containerId) ? Resource.Empty : new Resource([new(ContainerSemanticConventions.AttributeContainerId, containerId!)]);
    }

    /// <summary>
    /// Gets the Container Id from the line after removing the prefix and suffix from the cgroupv1 format.
    /// </summary>
    /// <param name="line">line read from cgroup file.</param>
    /// <returns>Container Id, Null if not found.</returns>
    private static string? GetIdFromLineV1(string line)
    {
        // This cgroup output line should have the container id in it
        var lastSlashIndex = line.LastIndexOf('/');
        if (lastSlashIndex < 0)
        {
            return null;
        }

        var lastSection = line.Substring(lastSlashIndex + 1);
        var startIndex = lastSection.LastIndexOf('-');
        var endIndex = lastSection.LastIndexOf('.');

        var containerId = RemovePrefixAndSuffixIfNeeded(lastSection, startIndex, endIndex);

        return string.IsNullOrEmpty(containerId) || !EncodingUtils.IsValidHexString(containerId) ? null : containerId;
    }

    /// <summary>
    /// Gets the Container Id from the line of the cgroupv2 format.
    /// </summary>
    /// <param name="line">line read from cgroup file.</param>
    /// <returns>Container Id, Null if not found.</returns>
    private static string? GetIdFromLineV2(string line)
    {
        string? containerId = null;
        var match = IdFromLineV2Regex().Match(line);
        if (match.Success)
        {
            containerId = match.Groups[1].Value;
        }

        return string.IsNullOrEmpty(containerId) || !EncodingUtils.IsValidHexString(containerId!) ? null : containerId;
    }

#if NET
    [GeneratedRegex(@".*/.+/([\w+-.]{64})/.*$")]
    private static partial Regex IdFromLineV2Regex();
#else
#pragma warning disable SA1201 // A field should not follow a method
    private static readonly Regex IdFromLineV2RegexField = new(@".*/.+/([\w+-.]{64})/.*$");
#pragma warning restore SA1201 // A field should not follow a method

    private static Regex IdFromLineV2Regex() => IdFromLineV2RegexField;
#endif

    private static string RemovePrefixAndSuffixIfNeeded(string input, int startIndex, int endIndex)
    {
        startIndex = (startIndex == -1) ? 0 : startIndex + 1;

        if (endIndex == -1)
        {
            endIndex = input.Length;
        }

        return input.Substring(startIndex, endIndex - startIndex);
    }

    /// <summary>
    /// Extracts Container Id from path using the cgroupv1 format.
    /// </summary>
    /// <param name="path">cgroup path.</param>
    /// <param name="cgroupVersion">CGroup Version of file to parse from.</param>
    /// <returns>Container Id, Null if not found or exception being thrown.</returns>
    private string? ExtractContainerId(string path, ParseMode cgroupVersion)
    {
        try
        {
            if (!File.Exists(path))
            {
                return null;
            }

            foreach (var line in File.ReadLines(path))
            {
                string? containerId = null;
                if (!string.IsNullOrEmpty(line))
                {
                    if (cgroupVersion == ParseMode.V1)
                    {
                        containerId = GetIdFromLineV1(line);
                    }
#if NET
                    else if (cgroupVersion == ParseMode.V2 && line.Contains(Hostname, StringComparison.Ordinal))
#else
                    else if (cgroupVersion == ParseMode.V2 && line.Contains(Hostname))
#endif
                    {
                        containerId = GetIdFromLineV2(line);
                    }
                }

                if (!string.IsNullOrEmpty(containerId))
                {
                    return containerId;
                }
            }
        }
        catch (Exception ex)
        {
            ContainerExtensionsEventSource.Log.ExtractResourceAttributesException($"{nameof(ContainerDetector)} : Failed to extract Container id from path", ex);
        }

        return null;
    }
}
