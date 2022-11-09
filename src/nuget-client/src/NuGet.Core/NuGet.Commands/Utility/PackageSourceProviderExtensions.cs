// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Commands
{
    public static class PackageSourceProviderExtensions
    {
        public static PackageSource ResolveSource(IEnumerable<PackageSource> availableSources, string source)
        {
            var resolvedSource = availableSources.FirstOrDefault(
                f => f.Source.Equals(source, StringComparison.OrdinalIgnoreCase) ||
                    f.Name.Equals(source, StringComparison.OrdinalIgnoreCase));

            if (resolvedSource == null)
            {
                ValidateSource(source);
                return new PackageSource(source);
            }
            else
            {
                return resolvedSource;
            }
        }

        public static string ResolveAndValidateSource(this IPackageSourceProvider sourceProvider, string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            var sources = sourceProvider.LoadPackageSources().Where(s => s.IsEnabled);
            var result = ResolveSource(sources, source);
            ValidateSource(result.Source);
            return result.Source;
        }

        private static void ValidateSource(string source)
        {
            Uri result = UriUtility.TryCreateSourceUri(source, UriKind.Absolute);
            if (result == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidSource, source));
            }
        }
    }
}
