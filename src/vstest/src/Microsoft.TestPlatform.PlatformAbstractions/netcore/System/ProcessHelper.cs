// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

public partial class ProcessHelper : IProcessHelper
{
    /// <inheritdoc/>
    public string GetCurrentProcessLocation()
    {
        return Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
    }

    /// <inheritdoc/>
    public nint GetProcessHandle(int processId)
    {
        // An IntPtr representing the value of the handle field.
        // If the handle has been marked invalid with SetHandleAsInvalid, this method still returns the original handle value, which can be a stale value.
        return Process.GetProcessById(processId).SafeHandle.DangerousGetHandle();
    }

    public PlatformArchitecture GetCurrentProcessArchitecture()
    {
        return RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => PlatformArchitecture.X86,
            Architecture.X64 => PlatformArchitecture.X64,
            Architecture.Arm => PlatformArchitecture.ARM,
            Architecture.Arm64 => PlatformArchitecture.ARM64,
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

    public PlatformArchitecture GetProcessArchitecture(int processId)
    {
        // Return the same as the current process.
        return GetCurrentProcessArchitecture();
    }
}

#endif
