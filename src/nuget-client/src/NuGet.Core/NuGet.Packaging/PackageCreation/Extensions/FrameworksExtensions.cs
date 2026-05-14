// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Versioning;
using NuGet.Frameworks;

namespace NuGet.Packaging
{
    public static class FrameworksExtensions
    {
        // NuGet.Frameworks doesn't have the equivalent of the old VersionUtility.GetFrameworkString
        // which is relevant for building packages. This isn't needed for net5.0+ frameworks.
        public static string GetFrameworkString(this NuGetFramework self)
        {
            bool isNet5Era = (self.Version.Major >= 5 && StringComparer.OrdinalIgnoreCase.Equals(FrameworkConstants.FrameworkIdentifiers.NetCoreApp, self.Framework));
            if (isNet5Era)
            {
                return self.GetShortFolderName();
            }

            var frameworkName = new FrameworkName(self.DotNetFrameworkName);
            string name = frameworkName.Identifier + frameworkName.Version;
            if (string.IsNullOrEmpty(frameworkName.Profile))
            {
                return name;
            }
            return name + "-" + frameworkName.Profile;
        }
    }
}
