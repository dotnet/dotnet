// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace Microsoft.DotNet.ScenarioTests.Common
{
    internal static class OperatingSystemFinder
    {
        internal static bool IsWindowsPlatform() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        internal static bool IsOSXPlatform() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        internal static bool IsLinuxPlatform() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        internal static OSPlatform GetPlatform()
        {
            if (IsWindowsPlatform())
                return OSPlatform.Windows;
            else if (IsOSXPlatform())
                return OSPlatform.OSX;
            else if (IsLinuxPlatform())
                return OSPlatform.Linux;
            else
                throw new NotImplementedException();
        }
    }
}
