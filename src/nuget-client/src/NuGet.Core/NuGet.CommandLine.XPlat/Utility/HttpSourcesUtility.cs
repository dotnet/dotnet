// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Configuration;

namespace NuGet.CommandLine.XPlat.Utility
{
    internal static class HttpSourcesUtility
    {
        public static List<PackageSource> GetDisallowedInsecureHttpSources(IReadOnlyList<PackageSource> packageSources)
        {
            if (packageSources == null || packageSources.Count == 0)
            {
                return new();
            }

            List<PackageSource> httpPackageSources = null;

            foreach (PackageSource packageSource in packageSources)
            {
                if (packageSource.IsHttp && !packageSource.IsHttps && !packageSource.AllowInsecureConnections)
                {
                    httpPackageSources ??= new(capacity: packageSources.Count);
                    httpPackageSources.Add(packageSource);
                }
            }

            return httpPackageSources ?? new();
        }

        public static string BuildHttpSourceErrorMessage(List<PackageSource> httpSources, string commandName)
        {
            string error = null;

            if (httpSources.Count == 1)
            {
                error = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Error_HttpServerUsage,
                    commandName,
                    httpSources[0]);
            }
            else if (httpSources.Count > 1)
            {
                error = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Error_HttpServerUsage_MultipleSources,
                    commandName,
                    Environment.NewLine + string.Join(Environment.NewLine, httpSources.Select(e => e.Name)));
            }

            return error;
        }
    }
}
