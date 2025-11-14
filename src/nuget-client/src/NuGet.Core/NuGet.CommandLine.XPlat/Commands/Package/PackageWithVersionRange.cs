// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Commands.Package
{
    internal record PackageWithVersionRange : IEqualityComparer<PackageWithVersionRange>
    {
        public required string Id { get; init; }
        public required VersionRange? VersionRange { get; init; }

        internal static IReadOnlyList<PackageWithVersionRange> Parse(ArgumentResult result)
        {
            if (result.Tokens.Count == 0)
            {
                return [];
            }

            List<PackageWithVersionRange> packages = new List<PackageWithVersionRange>(result.Tokens.Count);

            foreach (var token in result.Tokens)
            {
                string? packageId;
                VersionRange? newVersion;
                int separatorIndex = token.Value.IndexOf('@');
                if (separatorIndex < 0)
                {
                    packageId = token.Value;
                    newVersion = null;
                }
                else
                {
                    packageId = token.Value.Substring(0, separatorIndex);
                    string versionString = token.Value.Substring(separatorIndex + 1);
                    if (string.IsNullOrEmpty(versionString))
                    {
                        result.AddError(Messages.Error_MissingVersion(token.Value));
                        return [];
                    }
                    if (!VersionRange.TryParse(versionString, out newVersion))
                    {
                        result.AddError(Messages.Error_InvalidVersionRange(versionString));
                        return [];
                    }
                }

                var package = new PackageWithVersionRange
                {
                    Id = packageId,
                    VersionRange = newVersion
                };
                packages.Add(package);
            }

            return packages;
        }

        public bool Equals(PackageWithVersionRange? x, PackageWithVersionRange? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            if (!x.Id.Equals(y.Id, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return VersionRangeComparer.Default.Equals(x.VersionRange, y.VersionRange);
        }

        public int GetHashCode([DisallowNull] PackageWithVersionRange obj)
        {
            HashCode hash = new HashCode();
            hash.Add(obj.Id, StringComparer.OrdinalIgnoreCase);
            hash.Add(obj.VersionRange);
            return hash.ToHashCode();
        }
    }
}
