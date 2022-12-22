// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK || NETSTANDARD2_0

using System.Diagnostics;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

public static class ProcessStartInfoExtensions
{
    /// <summary>
    /// Add environment variable that apply to this process and child processes.
    /// </summary>
    /// <param name="startInfo">The process start info</param>
    /// <param name="name">Environment Variable name. </param>
    /// <param name="value">Environment Variable value.</param>
    public static void AddEnvironmentVariable(this ProcessStartInfo startInfo, string name, string? value)
    {
        startInfo.EnvironmentVariables[name] = value;
    }
}

#endif
