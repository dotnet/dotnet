// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.CommandLine.Parsing;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update
{
    internal record Package
    {
        public required string Id { get; init; }
        public required VersionRange? VersionRange { get; init; }

        internal static IReadOnlyList<Package> Parse(ArgumentResult result)
        {
            if (result.Tokens.Count == 0)
            {
                return [];
            }

            List<Package> packages = new List<Package>(result.Tokens.Count);

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

                var package = new Package
                {
                    Id = packageId,
                    VersionRange = newVersion
                };
                packages.Add(package);
            }

            return packages;
        }
    }
}
