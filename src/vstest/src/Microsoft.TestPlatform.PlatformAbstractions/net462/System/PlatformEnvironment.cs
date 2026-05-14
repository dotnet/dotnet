// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK || NETSTANDARD2_0

using System;
using System.ComponentModel;
using System.Diagnostics;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

/// <inheritdoc />
public class PlatformEnvironment : IEnvironment
{
    private PlatformArchitecture? _architecture;

    /// <inheritdoc />
    public PlatformArchitecture Architecture
    {
        get
        {
            return _architecture ??= Environment.Is64BitOperatingSystem
                ? IsArm64()
                    ? PlatformArchitecture.ARM64
                    : PlatformArchitecture.X64
                : PlatformArchitecture.X86;
        }
    }

    private static bool IsArm64()
    {
        try
        {
            var currentProcess = Process.GetCurrentProcess();
            if (!NativeMethods.IsWow64Process2(currentProcess.Handle, out ushort _, out ushort nativeMachine))
            {
                throw new Win32Exception();
            }

            // If nativeMachine is IMAGE_FILE_MACHINE_ARM64 it means that we're running on ARM64 architecture device.
            return nativeMachine == NativeMethods.IMAGE_FILE_MACHINE_ARM64;
        }
        catch
        {
            // At the moment we cannot log messages inside the Microsoft.TestPlatform.PlatformAbstractions.
            // We did an attempt in https://github.com/microsoft/vstest/pull/3422 - 17.2.0-preview-20220301-01 - but we reverted after
            // because we broke a scenario where for .NET Framework application inside the test host
            // we loaded runner version of Microsoft.TestPlatform.PlatformAbstractions but newer version Microsoft.TestPlatform.ObjectModel(the one close
            // to the test container) and the old PlatformAbstractions doesn't contain the methods expected by the new ObjectModel throwing
            // a MissedMethodException.
        }

        return false;
    }

    /// <inheritdoc />
    public PlatformOperatingSystem OperatingSystem
    {
        get
        {
            // Ensure the value is detected appropriately for Desktop CLR, Mono CLR 1.x and Mono
            // CLR 2.x. See below link for more information:
            // http://www.mono-project.com/docs/faq/technical/#how-to-detect-the-execution-platform
            int p = (int)Environment.OSVersion.Platform;
            return p is 4 or 6 or 128
                ? PlatformOperatingSystem.Unix
                : PlatformOperatingSystem.Windows;
        }
    }

    /// <inheritdoc />
    public string OperatingSystemVersion => Environment.OSVersion.ToString();

    /// <inheritdoc />
    public int ProcessorCount => Environment.ProcessorCount;

    /// <inheritdoc />
    public void Exit(int exitcode)
    {
        Environment.Exit(exitcode);
    }

    /// <inheritdoc />
    public int GetCurrentManagedThreadId()
        => Environment.CurrentManagedThreadId;
}

#endif
