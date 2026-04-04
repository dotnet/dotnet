// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single type

using System.Runtime.InteropServices;
using System.Security.Principal;
using Xunit;

namespace OpenTelemetry.Tests;

internal enum TestPlatform
{
    Unknown = 0,
    Windows = 1,
    Linux = 2,
    OSX = 3,
}

internal sealed class TestPlatformHelpers
{
    public static bool IsProcessElevated(TestPlatform platform)
    {
        switch (platform)
        {
            case TestPlatform.Linux:
                var userId = SystemNativeUnix.GetEUid();
                return userId == 0;

            case TestPlatform.Windows:
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    using var identity = WindowsIdentity.GetCurrent();
                    return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
                }

                return false;

            // TODO: Add support for this check on other platforms as needed.
            default:
                throw new NotSupportedException($"TestPlatform '{platform}' is not supported for elevation check.");
        }
    }

    // From: https://github.com/dotnet/corefx/blob/v2.2.8/src/Common/src/Interop/Unix/System.Native/Interop.GetEUid.cs
    private static class SystemNativeUnix
    {
#pragma warning disable CA5392 // Use DefaultDllImportSearchPaths attribute for P/Invokes
        [DllImport("libc", EntryPoint = "geteuid", SetLastError = true)]
        internal static extern uint GetEUid();
#pragma warning restore CA5392 // Use DefaultDllImportSearchPaths attribute for P/Invokes
    }
}

internal sealed class SkipUnlessPlatformMatchesFactAttribute : FactAttribute
{
    public SkipUnlessPlatformMatchesFactAttribute(TestPlatform platform, bool requireElevatedProcess = false)
    {
        var osPlatform = platform switch
        {
            TestPlatform.Windows => OSPlatform.Windows,
            TestPlatform.Linux => OSPlatform.Linux,
            TestPlatform.OSX => OSPlatform.OSX,
            TestPlatform.Unknown => throw new NotSupportedException("TestPlatform 'Unknown' is not supported"),
            _ => throw new NotSupportedException($"TestPlatform '{platform}' is not supported"),
        };

        if (!RuntimeInformation.IsOSPlatform(osPlatform))
        {
            this.Skip = $"Skipped because current platform does not match requested '{platform}' platform.";
            return;
        }

        if (requireElevatedProcess
            && !TestPlatformHelpers.IsProcessElevated(platform))
        {
            this.Skip = $"Skipped because current process isn't elevated.";
            return;
        }
    }
}

internal sealed class SkipUnlessPlatformMatchesTheoryAttribute : TheoryAttribute
{
    public SkipUnlessPlatformMatchesTheoryAttribute(TestPlatform platform, bool requireElevatedProcess = false)
    {
        var osPlatform = platform switch
        {
            TestPlatform.Windows => OSPlatform.Windows,
            TestPlatform.Linux => OSPlatform.Linux,
            TestPlatform.OSX => OSPlatform.OSX,
            TestPlatform.Unknown => throw new NotSupportedException("TestPlatform 'Unknown' is not supported"),
            _ => throw new NotSupportedException($"TestPlatform '{platform}' is not supported"),
        };

        if (!RuntimeInformation.IsOSPlatform(osPlatform))
        {
            this.Skip = $"Skipped because current platform does not match requested '{platform}' platform.";
            return;
        }

        if (requireElevatedProcess
            && !TestPlatformHelpers.IsProcessElevated(platform))
        {
            this.Skip = $"Skipped because current process isn't elevated.";
            return;
        }
    }
}
