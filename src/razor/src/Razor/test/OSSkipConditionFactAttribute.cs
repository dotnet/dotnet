// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Microsoft.AspNetCore.Razor;

public class OSSkipConditionFactAttribute : FactAttribute
{
    /// <summary>
    /// A <see cref="FactAttribute"/> that configures <see cref="FactAttribute.Skip"/> on the specified platforms.
    /// </summary>
    /// <param name="skippedPlatforms">Valid values include <c>WINDOWS</c>, <c>LINUX</c>, <c>OSX</c>, and <c>FREEBSD</c>.
    /// <see href="https://source.dot.net/#System.Runtime.InteropServices.RuntimeInformation/System/Runtime/InteropServices/RuntimeInformation/OSPlatform.cs,26fa53454c093915"/></param>
    public OSSkipConditionFactAttribute(string[] skippedPlatforms)
    {
        SetSkipIfNecessary(this, skippedPlatforms);
    }

    internal static void SetSkipIfNecessary(FactAttribute fact, string[] skippedPlatforms)
    {
        foreach (var platform in skippedPlatforms)
        {
            var osPlatform = platform switch
            {
                "Windows" => OSPlatform.Windows,
                "Linux" => OSPlatform.Linux,
                "OSX" => OSPlatform.OSX,
#if NET
                "FreeBSD" => OSPlatform.FreeBSD,
#endif
                _ => throw new NotSupportedException($"Unsupported platform: {platform}")
            };

            if (RuntimeInformation.IsOSPlatform(osPlatform))
            {
                fact.Skip = $"Ignored on {platform}";
                break;
            }
        }
    }
}

public class OSSkipConditionTheoryAttribute : TheoryAttribute
{
    public OSSkipConditionTheoryAttribute(string[] skippedPlatforms)
    {
        OSSkipConditionFactAttribute.SetSkipIfNecessary(this, skippedPlatforms);
    }
}
