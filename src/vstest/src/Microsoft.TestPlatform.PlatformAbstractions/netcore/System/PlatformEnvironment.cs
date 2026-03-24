// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

/// <inheritdoc />
public class PlatformEnvironment : IEnvironment
{
    /// <inheritdoc />
    public PlatformArchitecture Architecture
    {
        get
        {
            return RuntimeInformation.OSArchitecture switch
            {
                System.Runtime.InteropServices.Architecture.X86 => PlatformArchitecture.X86,
                System.Runtime.InteropServices.Architecture.X64 => PlatformArchitecture.X64,
                System.Runtime.InteropServices.Architecture.Arm => PlatformArchitecture.ARM,
                System.Runtime.InteropServices.Architecture.Arm64 => PlatformArchitecture.ARM64,
                // The symbolic value is only available with .NET 6
                // preview 6 or later, so use the numerical value for now.
                // case System.Runtime.InteropServices.Architecture.S390x:
                (Architecture)5 => PlatformArchitecture.S390x,
                (Architecture)6 => PlatformArchitecture.LoongArch64,
                (Architecture)8 => PlatformArchitecture.Ppc64le,
                (Architecture)9 => PlatformArchitecture.RiscV64,
                _ => throw new NotSupportedException(),
            };
        }
    }

    /// <inheritdoc />
    public PlatformOperatingSystem OperatingSystem
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return PlatformOperatingSystem.Windows;
            }

            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? PlatformOperatingSystem.OSX : PlatformOperatingSystem.Unix;
        }
    }

    /// <inheritdoc />
    public string OperatingSystemVersion
    {
        get
        {
            return RuntimeInformation.OSDescription;
        }
    }

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
