// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK || NETSTANDARD2_0

using System.Reflection;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

/// <summary>
/// Assembly Extensions
/// </summary>
public static class PlatformAssemblyExtensions
{
    /// <summary>
    /// Get current assembly location as per current platform
    /// </summary>
    /// <param name="assembly">Assembly</param>
    /// <returns>Returns Assembly location as per platform</returns>
    public static string GetAssemblyLocation(this Assembly assembly)
    {
        return assembly.Location;
    }
}

#endif
