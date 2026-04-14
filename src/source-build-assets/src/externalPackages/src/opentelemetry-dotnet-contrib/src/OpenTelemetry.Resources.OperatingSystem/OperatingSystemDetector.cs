// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK
using System.Runtime.InteropServices;
#endif
#if NET
using System.Xml.Linq;
#endif
using static OpenTelemetry.Resources.OperatingSystem.OperatingSystemSemanticConventions;

namespace OpenTelemetry.Resources.OperatingSystem;

/// <summary>
/// Operating system detector.
/// </summary>
internal sealed class OperatingSystemDetector : IResourceDetector
{
    private const string RegistryKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
#if NET
    private const string KernelOsRelease = "/proc/sys/kernel/osrelease";
    private static readonly string[] DefaultEtcOsReleasePaths =
    [
        "/etc/os-release",
        "/usr/lib/os-release"
    ];

    private static readonly string[] DefaultPlistFilePaths =
    [
        "/System/Library/CoreServices/SystemVersion.plist",
        "/System/Library/CoreServices/ServerVersion.plist"
    ];
#endif

    private readonly string? osType;
    private readonly string? registryKey;
#if NET
    private readonly string? kernelOsRelease;
    private readonly string[]? etcOsReleasePaths;
    private readonly string[]? plistFilePaths;
#endif

    internal OperatingSystemDetector()
#if NET
        : this(GetOSType(), RegistryKey, KernelOsRelease, DefaultEtcOsReleasePaths, DefaultPlistFilePaths)
#else
        : this(GetOSType(), RegistryKey)
#endif
    {
    }

#if NET
    /// <summary>
    /// Initializes a new instance of the <see cref="OperatingSystemDetector"/> class for testing.
    /// </summary>
    /// <param name="osType">The target platform identifier, specifying the operating system type from SemanticConventions.</param>
    /// <param name="registryKey">The string path in the Windows Registry to retrieve specific Windows attributes.</param>
    /// <param name="kernelOsRelease">The string path to the file used to obtain Linux build id.</param>
    /// <param name="etcOsReleasePath">The string path to the file used to obtain Linux attributes.</param>
    /// <param name="plistFilePaths">An array of file paths used to retrieve MacOS attributes from plist files.</param>
    internal OperatingSystemDetector(string? osType, string? registryKey, string? kernelOsRelease, string[]? etcOsReleasePath, string[]? plistFilePaths)
#else
    /// <summary>
    /// Initializes a new instance of the <see cref="OperatingSystemDetector"/> class for testing.
    /// </summary>
    /// <param name="osType">The target platform identifier, specifying the operating system type from SemanticConventions.</param>
    /// <param name="registryKey">The string path in the Windows Registry to retrieve specific Windows attributes.</param>
    internal OperatingSystemDetector(string? osType, string? registryKey)
#endif
    {
        this.osType = osType;
        this.registryKey = registryKey;
#if NET
        this.kernelOsRelease = kernelOsRelease;
        this.etcOsReleasePaths = etcOsReleasePath;
        this.plistFilePaths = plistFilePaths;
#endif
    }

    /// <summary>
    /// Detects the resource attributes from the operating system.
    /// </summary>
    /// <returns>Resource with key-value pairs of resource attributes.</returns>
    ///
    public Resource Detect()
    {
        var attributes = new List<KeyValuePair<string, object>>(5);
        if (this.osType == null)
        {
            return Resource.Empty;
        }

        attributes.Add(new KeyValuePair<string, object>(AttributeOperatingSystemType, this.osType));

        AddAttributeIfNotNullOrEmpty(attributes, AttributeOperatingSystemDescription, GetOSDescription());

        switch (this.osType)
        {
            case OperatingSystemsValues.Windows:
                this.AddWindowsAttributes(attributes);
                break;
#if NET
            case OperatingSystemsValues.Linux:
                this.AddLinuxAttributes(attributes);
                break;
            case OperatingSystemsValues.Darwin:
                this.AddMacOSAttributes(attributes);
                break;
#endif
            default:
                break;
        }

        return new Resource(attributes);
    }

    private static void AddAttributeIfNotNullOrEmpty(List<KeyValuePair<string, object>> attributes, string key, object? value)
    {
        if (value == null)
        {
            OperatingSystemResourcesEventSource.Log.FailedToValidateValue("The provided value is null");
            return;
        }

        if (value is string strValue && string.IsNullOrEmpty(strValue))
        {
            OperatingSystemResourcesEventSource.Log.FailedToValidateValue("The provided value string is empty.");
            return;
        }

        attributes.Add(new KeyValuePair<string, object>(key, value!));
    }

    private static string? GetOSType()
    {
#if NETFRAMEWORK
        return OperatingSystemsValues.Windows;
#else
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? OperatingSystemsValues.Windows :
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? OperatingSystemsValues.Linux :
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? OperatingSystemsValues.Darwin : null;
#endif
    }

    private static string GetOSDescription()
    {
#if NET
        return RuntimeInformation.OSDescription;
#else
        return Environment.OSVersion.ToString();
#endif
    }

#pragma warning disable CA1416
    private void AddWindowsAttributes(List<KeyValuePair<string, object>> attributes)
    {
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(this.registryKey!);
            if (key != null)
            {
                AddAttributeIfNotNullOrEmpty(attributes, AttributeOperatingSystemBuildId, key.GetValue("CurrentBuildNumber")?.ToString());
                AddAttributeIfNotNullOrEmpty(attributes, AttributeOperatingSystemName, key.GetValue("ProductName")?.ToString());
                AddAttributeIfNotNullOrEmpty(attributes, AttributeOperatingSystemVersion, key.GetValue("CurrentVersion")?.ToString());
            }
        }
        catch (Exception ex)
        {
            OperatingSystemResourcesEventSource.Log.ResourceAttributesExtractException("Failed to get Windows attributes", ex);
        }
    }
#pragma warning restore CA1416

#if NET
    // based on:
    // https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/Interop/Linux/os-release/Interop.OSReleaseFile.cs
    private void AddLinuxAttributes(List<KeyValuePair<string, object>> attributes)
    {
        try
        {
            var etcOsReleasePath = this.etcOsReleasePaths!.FirstOrDefault(File.Exists);
            if (string.IsNullOrEmpty(etcOsReleasePath))
            {
                OperatingSystemResourcesEventSource.Log.FailedToFindFile("Failed to find the os-release file");
                return;
            }

            var osReleaseContent = File.ReadAllLines(etcOsReleasePath);
            ReadOnlySpan<char> buildId = default, name = default, version = default;

            foreach (var line in osReleaseContent)
            {
                var lineSpan = line.AsSpan();

                _ = TryGetFieldValue(lineSpan, "BUILD_ID=", ref buildId) ||
                    TryGetFieldValue(lineSpan, "NAME=", ref name) ||
                    TryGetFieldValue(lineSpan, "VERSION_ID=", ref version);
            }

            var buildIdContent = buildId.IsEmpty ? File.ReadAllText(this.kernelOsRelease!).Trim() : buildId.ToString();

            AddAttributeIfNotNullOrEmpty(attributes, AttributeOperatingSystemBuildId, buildIdContent);
            AddAttributeIfNotNullOrEmpty(attributes, AttributeOperatingSystemName, name.IsEmpty ? "Linux" : name.ToString());
            AddAttributeIfNotNullOrEmpty(attributes, AttributeOperatingSystemVersion, version.IsEmpty ? null : version.ToString());
        }
        catch (Exception ex)
        {
            OperatingSystemResourcesEventSource.Log.ResourceAttributesExtractException("Failed to get Linux attributes", ex);
        }

        static bool TryGetFieldValue(ReadOnlySpan<char> line, ReadOnlySpan<char> prefix, ref ReadOnlySpan<char> value)
        {
            if (!line.StartsWith(prefix))
            {
                return false;
            }

            var fieldValue = line.Slice(prefix.Length);

            // Remove enclosing quotes if present.
            if (fieldValue.Length >= 2 &&
                (fieldValue[0] == '"' || fieldValue[0] == '\'') &&
                fieldValue[0] == fieldValue[^1])
            {
                fieldValue = fieldValue[1..^1];
            }

            value = fieldValue;
            return true;
        }
    }

    private void AddMacOSAttributes(List<KeyValuePair<string, object>> attributes)
    {
        try
        {
            var plistFilePath = this.plistFilePaths!.FirstOrDefault(File.Exists);
            if (string.IsNullOrEmpty(plistFilePath))
            {
                OperatingSystemResourcesEventSource.Log.FailedToFindFile("No suitable plist file found");
                return;
            }

            var doc = XDocument.Load(plistFilePath);
            var dict = doc.Root?.Element("dict");

            string? buildId = null, name = null, version = null;

            if (dict != null)
            {
                var keys = dict.Elements("key").ToList();
                var values = dict.Elements("string").ToList();

                if (keys.Count != values.Count)
                {
                    OperatingSystemResourcesEventSource.Log.FailedToValidateValue($"Failed to get MacOS attributes: The number of keys does not match the number of values. Keys count: {keys.Count}, Values count: {values.Count}");
                    return;
                }

                for (var i = 0; i < keys.Count; i++)
                {
                    switch (keys[i].Value)
                    {
                        case "ProductBuildVersion":
                            buildId = values[i].Value;
                            break;
                        case "ProductName":
                            name = values[i].Value;
                            break;
                        case "ProductVersion":
                            version = values[i].Value;
                            break;
                        default:
                            break;
                    }
                }
            }

            AddAttributeIfNotNullOrEmpty(attributes, AttributeOperatingSystemBuildId, buildId);
            AddAttributeIfNotNullOrEmpty(attributes, AttributeOperatingSystemName, name);
            AddAttributeIfNotNullOrEmpty(attributes, AttributeOperatingSystemVersion, version);
        }
        catch (Exception ex)
        {
            OperatingSystemResourcesEventSource.Log.ResourceAttributesExtractException("Failed to get MacOS attributes", ex);
        }
    }
#endif
}
