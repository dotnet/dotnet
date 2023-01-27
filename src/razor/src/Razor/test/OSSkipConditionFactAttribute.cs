// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
        foreach (var platform in skippedPlatforms)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create(platform)))
            {
                Skip = $"Ignored on {platform}";
                break;
            }
        }
    }
}
