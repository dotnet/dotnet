// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Resolver;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Core.FuncTest
{
    public class ResolverTests
    {
        [Fact]
        public void ResolveDependenciesForVeryDeepGraph()
        {
            // Arrange
            var target = CreatePackage("Package0", "1.0", new Dictionary<string, string>() {
                { "Package1", "1.0.0" },
                { "Package2", "1.0.0" },
                { "Package3", "1.0.0" },
                { "Package4", "1.0.0" },
                { "Package5", "1.0.0" },
                { "Package6", "1.0.0" },
                { "Package7", "1.0.0" },
                { "Package8", "1.0.0" },
            });

            var sourceRepository = new List<ResolverPackage>();
            sourceRepository.Add(target);

            var next = 1;

            // make lots of packages
            for (var j = 1; j < 1000; j++)
            {
                next = j + 1;
                sourceRepository.Add(CreatePackage($"Package{j}", "1.0.0", new Dictionary<string, string>() { { $"Package{next}", "1.0.0" } }));
            }

            sourceRepository.Add(CreatePackage($"Package{next}", "1.0.0"));

            var resolver = new PackageResolver();

            var context = new PackageResolverContext(DependencyBehavior.Lowest,
                new[] { target.Id }, Enumerable.Empty<string>(),
                Enumerable.Empty<PackageReference>(),
                new[] { target },
                sourceRepository,
                Enumerable.Empty<PackageSource>(),
                Common.NullLogger.Instance);

            // Act
            var packages = resolver.Resolve(context, CancellationToken.None);

            // Assert
            Assert.Equal(1001, packages.Count());
        }

        private ResolverPackage CreatePackage(string id, string version, IDictionary<string, string> dependencies = null)
        {
            var deps = new List<PackageDependency>();

            if (dependencies != null)
            {
                foreach (var dep in dependencies)
                {
                    VersionRange range = null;

                    if (dep.Value != null)
                    {
                        range = VersionRange.Parse(dep.Value);
                    }

                    deps.Add(new PackageDependency(dep.Key, range));
                }
            }

            return new ResolverPackage(id, NuGetVersion.Parse(version), deps, true, false);
        }
    }
}
