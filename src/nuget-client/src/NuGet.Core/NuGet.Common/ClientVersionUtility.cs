// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Reflection;

namespace NuGet.Common
{
    public static class ClientVersionUtility
    {
        // Cache the value since it cannot change
        private static string _clientVersion;

        /// <summary>
        /// Find the current NuGet client version from the assembly info as a string.
        /// If no value can be found an InvalidOperationException will be thrown.
        /// </summary>
        /// <remarks>This can contain prerelease labels if AssemblyInformationalVersionAttribute exists.</remarks>
        public static string GetNuGetAssemblyVersion()
        {
            if (_clientVersion == null)
            {
                string version = string.Empty;

                var assembly = typeof(ClientVersionUtility).GetTypeInfo().Assembly;

                var informationalVersionAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                if (informationalVersionAttr != null)
                {
                    // Attempt to read the full informational version if it exists
                    version = informationalVersionAttr.InformationalVersion;
                }
                else
                {
                    // Fallback to the .net assembly version
                    var versionAttr = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
                    if (versionAttr != null)
                    {
                        version = versionAttr.Version.ToString(CultureInfo.CurrentCulture);
                    }
                }

                // Verify a value was found
                if (string.IsNullOrEmpty(version))
                {
                    throw new InvalidOperationException(Strings.UnableToDetemineClientVersion);
                }

                _clientVersion = version;
            }

            return _clientVersion;
        }
    }
}
