// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using NuGet.Packaging.Core;
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
                PackageIdentity identity;
                try
                {
                    identity = PackageIdentity.Parse(token.Value);
                }
                catch (ArgumentException ex)
                {
                    result.AddError(ex.Message);
                    return [];
                }

                var package = new PackageWithNuGetVersion
                {
                    Id = identity.Id,
                    NuGetVersion = identity.HasVersion ? identity.Version : null
                };

                packages.Add(package);
            }

            return packages;
        }
    }
}
