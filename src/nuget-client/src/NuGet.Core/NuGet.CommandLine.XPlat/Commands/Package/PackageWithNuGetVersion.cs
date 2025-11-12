// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.CommandLine.Parsing;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Commands.Package
{
    internal record PackageWithNuGetVersion
    {
        public required string Id { get; init; }

        public NuGetVersion? NuGetVersion { get; init; }

        internal static IReadOnlyList<PackageWithNuGetVersion> Parse(ArgumentResult result)
        {
            if (result.Tokens.Count == 0)
            {
                return [];
            }

            var packages = new List<PackageWithNuGetVersion>(result.Tokens.Count);

            foreach (var token in result.Tokens)
            {
                string? packageId;
                NuGetVersion? newExactVersion = null;

                int separatorIndex = token.Value.IndexOf('@');

                if (separatorIndex < 0)
                {
                    packageId = token.Value;
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

                    if (!NuGetVersion.TryParse(versionString, out newExactVersion))
                    {
                        result.AddError(Messages.Error_InvalidVersion(versionString));
                        return [];
                    }
                }

                var package = new PackageWithNuGetVersion
                {
                    Id = packageId,
                    NuGetVersion = newExactVersion
                };

                packages.Add(package);
            }

            return packages;
        }
    }
}
