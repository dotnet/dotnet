// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
#endif
using Microsoft.Win32;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Resources.Host;

/// <summary>
/// Host detector.
/// </summary>
internal sealed class HostDetector : IResourceDetector
{
#if !NETFRAMEWORK
    private const string ETCMACHINEID = "/etc/machine-id";
    private const string ETCVARDBUSMACHINEID = "/var/lib/dbus/machine-id";
    private readonly Func<OSPlatform, bool> isOsPlatform;
    private readonly Func<IEnumerable<string>> getFilePaths;
    private readonly Func<string?> getMacOsMachineId;
#endif
    private readonly Func<string?> getWindowsMachineId;

    /// <summary>
    /// Initializes a new instance of the <see cref="HostDetector"/> class.
    /// </summary>
    public HostDetector()
        : this(
#if !NETFRAMEWORK
        RuntimeInformation.IsOSPlatform,
        GetFilePaths,
        GetMachineIdMacOs,
#endif
        GetMachineIdWindows)
    {
    }

#if !NETFRAMEWORK
    public HostDetector(
        Func<IEnumerable<string>> getFilePaths,
        Func<string?> getMacOsMachineId,
        Func<string?> getWindowsMachineId)
        : this(
            RuntimeInformation.IsOSPlatform,
            getFilePaths,
            getMacOsMachineId,
            getWindowsMachineId)
    {
    }
#endif

    internal HostDetector(
#if !NETFRAMEWORK
        Func<OSPlatform, bool> isOsPlatform,
        Func<IEnumerable<string>> getFilePaths,
        Func<string?> getMacOsMachineId,
#endif
        Func<string?> getWindowsMachineId)
    {
#if !NETFRAMEWORK
        Guard.ThrowIfNull(isOsPlatform);
        Guard.ThrowIfNull(getFilePaths);
        Guard.ThrowIfNull(getMacOsMachineId);
#endif
        Guard.ThrowIfNull(getWindowsMachineId);

#if !NETFRAMEWORK
        this.isOsPlatform = isOsPlatform;
        this.getFilePaths = getFilePaths;
        this.getMacOsMachineId = getMacOsMachineId;
#endif
        this.getWindowsMachineId = getWindowsMachineId;
    }

#if !NETFRAMEWORK
    public static string? MapArchitectureToOtel(Architecture arch) =>
        arch switch
        {
            Architecture.X86 => "x86",
            Architecture.X64 => "amd64",
            Architecture.Arm => "arm32",
            Architecture.Arm64 => "arm64",
#if NET
            Architecture.S390x => "s390x",
            Architecture.Armv6 => "arm32",
            Architecture.Ppc64le => "ppc64",

            // The following architectures do not have a mapping in OTel spec: https://github.com/open-telemetry/semantic-conventions/blob/v1.39.0/docs/resource/host.md
            Architecture.Wasm => null,
            Architecture.LoongArch64 => null,
#if NET10_0_OR_GREATER
            Architecture.RiscV64 => null,
#endif
#endif
            _ => null,
        };
#endif

    /// <summary>
    /// Detects the resource attributes from host.
    /// </summary>
    /// <returns>Resource with key-value pairs of resource attributes.</returns>
    public Resource Detect()
    {
        try
        {
            var attributes = new List<KeyValuePair<string, object>>(3)
            {
                new(HostSemanticConventions.AttributeHostName, Environment.MachineName),
            };

            var machineId = this.GetMachineId();

            if (machineId != null && !string.IsNullOrEmpty(machineId))
            {
                attributes.Add(new(HostSemanticConventions.AttributeHostId, machineId));
            }

#if !NETFRAMEWORK
            var arch = MapArchitectureToOtel(RuntimeInformation.OSArchitecture);
            if (arch != null)
            {
                attributes.Add(new(HostSemanticConventions.AttributeHostArch, arch));
            }
#endif
#if NET471_OR_GREATER
#error Architecture is available in .NET Framework 4.7.1+, enable it when we move to that as minimum supported version
#endif

            return new Resource(attributes);
        }
        catch (InvalidOperationException ex)
        {
            // Handling InvalidOperationException due to https://learn.microsoft.com/dotnet/api/system.environment.machinename#exceptions
            HostResourceEventSource.Log.ResourceAttributesExtractException(nameof(HostDetector), ex);
        }

        return Resource.Empty;
    }

#if !NETFRAMEWORK
    internal static string? ParseMacOsOutput(string? output)
    {
        if (output == null || string.IsNullOrEmpty(output))
        {
            return null;
        }

        var lines = output.Split([Environment.NewLine], StringSplitOptions.None);

        foreach (var line in lines)
        {
#if NET
            if (line.Contains("IOPlatformUUID", StringComparison.OrdinalIgnoreCase))
#else
            if (line.IndexOf("IOPlatformUUID", StringComparison.OrdinalIgnoreCase) >= 0)
#endif
            {
                var parts = line.Split('"');

                if (parts.Length > 3)
                {
                    return parts[3];
                }
            }
        }

        return null;
    }

    private static IEnumerable<string> GetFilePaths()
    {
        yield return ETCMACHINEID;
        yield return ETCVARDBUSMACHINEID;
    }
#endif

#if !NETFRAMEWORK
    private static string? GetMachineIdMacOs()
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "sh",
                Arguments = "-c \"ioreg -rd1 -c IOPlatformExpertDevice\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            var sb = new StringBuilder();
            using var process = Process.Start(startInfo);
            if (process != null)
            {
                var isExited = process.WaitForExit(5000);
                if (isExited)
                {
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();

                    if (!string.IsNullOrEmpty(error))
                    {
                        HostResourceEventSource.Log.FailedToExtractResourceAttributes(nameof(HostDetector), error);
                        return null;
                    }

                    sb.Append(output);
                    return sb.ToString();
                }
                else
                {
                    HostResourceEventSource.Log.ProcessTimeout("Process did not exit within the given timeout");
                    return null;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            HostResourceEventSource.Log.ResourceAttributesExtractException(nameof(HostDetector), ex);
        }

        return null;
    }
#endif

#pragma warning disable CA1416
    // stylecop wants this protected by System.OperatingSystem.IsWindows
    // this type only exists in .NET 5+
    private static string? GetMachineIdWindows()
    {
        try
        {
            using var subKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", false);
            return subKey?.GetValue("MachineGuid") as string ?? null;
        }
        catch (Exception ex)
        {
            HostResourceEventSource.Log.ResourceAttributesExtractException(nameof(HostDetector), ex);
        }

        return null;
    }
#pragma warning restore CA1416

    private string? GetMachineId() =>
#if NETFRAMEWORK
        this.getWindowsMachineId();
#else
        this.isOsPlatform(OSPlatform.Windows) ? this.getWindowsMachineId() :
        this.isOsPlatform(OSPlatform.Linux) ? this.GetMachineIdLinux() :
        this.isOsPlatform(OSPlatform.OSX) ? ParseMacOsOutput(this.getMacOsMachineId()) : null;
#endif

#if !NETFRAMEWORK
    private string? GetMachineIdLinux()
    {
        var paths = this.getFilePaths();

        foreach (var path in paths)
        {
            if (File.Exists(path))
            {
                try
                {
                    return File.ReadAllText(path).Trim();
                }
                catch (Exception ex)
                {
                    HostResourceEventSource.Log.ResourceAttributesExtractException(nameof(HostDetector), ex);
                }
            }
        }

        return null;
    }
#endif
}
