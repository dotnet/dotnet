// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Test
{
    public class UninstallResolverTests
    {
        private static PackageDependencyInfo CreatePackageDependencyInfo(PackageIdentity packageIdentity, params string[] dependencies)
        {
            if (dependencies != null
                && dependencies.Length % 2 != 0)
            {
                throw new ArgumentException("dependencies array length should be even");
            }

            var dependencyList = new List<Packaging.Core.PackageDependency>();
            if (dependencies != null)
            {
                for (var i = 0; i < dependencies.Length; i += 2)
                {
                    var packageDependency = new Packaging.Core.PackageDependency(dependencies[i], VersionRange.Parse(dependencies[i + 1]));
                    dependencyList.Add(packageDependency);
                }
            }

            return new PackageDependencyInfo(packageIdentity, dependencyList);
        }

        private static readonly PackageIdentity A1 = new PackageIdentity("A", new NuGetVersion("1.0"));
        private static PackageIdentity A2 = new PackageIdentity("A", new NuGetVersion("2.0"));
        private static readonly PackageIdentity B1 = new PackageIdentity("B", new NuGetVersion("1.0"));
        private static PackageIdentity B2 = new PackageIdentity("B", new NuGetVersion("2.0"));
        private static readonly PackageIdentity C1 = new PackageIdentity("C", new NuGetVersion("1.0"));
        private static readonly PackageIdentity D1 = new PackageIdentity("D", new NuGetVersion("1.0"));
        private static readonly PackageIdentity E1 = new PackageIdentity("E", new NuGetVersion("1.0"));
        private static PackageIdentity F1 = new PackageIdentity("F", new NuGetVersion("1.0"));

        private static readonly List<PackageDependencyInfo> PackageDependencyInfo1 = new List<PackageDependencyInfo>
            {
                CreatePackageDependencyInfo(A1, B1.Id, "1.0", C1.Id, "1.0"),
                CreatePackageDependencyInfo(C1, D1.Id, "1.0", E1.Id, "1.0")
            };

        private static readonly IEnumerable<PackageIdentity> InstalledPackages1 = new List<PackageIdentity>
            {
                A1,
                B1,
                C1,
                D1,
                E1
            };

        private static readonly List<PackageDependencyInfo> DiamondDependencyInfo = new List<PackageDependencyInfo>
            {
                CreatePackageDependencyInfo(A1, B1.Id, "1.0", C1.Id, "1.0"),
                CreatePackageDependencyInfo(B1, D1.Id, "1.0"),
                CreatePackageDependencyInfo(C1, D1.Id, "1.0")
            };

        private static readonly IEnumerable<PackageIdentity> DiamondDependencyInstalledPackages = new List<PackageIdentity>
            {
                A1,
                B1,
                C1,
                D1
            };

        private static readonly List<PackageDependencyInfo> DeepDiamondDependencyInfo = new List<PackageDependencyInfo>
            {
                CreatePackageDependencyInfo(A1, B1.Id, "1.0", C1.Id, "1.0"),
                CreatePackageDependencyInfo(B1, D1.Id, "1.0"),
                CreatePackageDependencyInfo(C1, E1.Id, "1.0"),
                CreatePackageDependencyInfo(E1, D1.Id, "1.0")
            };

        private static readonly IEnumerable<PackageIdentity> DeepDiamondDependencyInstalledPackages = new List<PackageIdentity>
            {
                A1,
                B1,
                C1,
                D1,
                E1
            };

        [Fact]
        public void TestUninstallResolverGetDependentsDict()
        {
            IDictionary<PackageIdentity, HashSet<PackageIdentity>> dependenciesDict;
            var dependentsDict = UninstallResolver.GetPackageDependents(PackageDependencyInfo1,
                InstalledPackages1,
                out dependenciesDict);

            var keysList = dependentsDict.Keys.ToList();
            var valuesList = dependentsDict.Values.ToList();

            Assert.Equal(4, dependentsDict.Count);
            Assert.Equal(B1, keysList[0]);
            Assert.Contains(A1, valuesList[0]);
            Assert.Equal(C1, keysList[1]);
            Assert.Contains(A1, valuesList[1]);
            Assert.Equal(D1, keysList[2]);
            Assert.Contains(C1, valuesList[2]);
            Assert.Equal(E1, keysList[3]);
            Assert.Contains(C1, valuesList[3]);
        }

        [Fact]
        public void TestUninstallResolverDependenciesDict()
        {
            IDictionary<PackageIdentity, HashSet<PackageIdentity>> dependenciesDict;
            var dependentsDict = UninstallResolver.GetPackageDependents(PackageDependencyInfo1,
                InstalledPackages1,
                out dependenciesDict);

            var keysList = dependenciesDict.Keys.ToList();
            var valuesList = dependenciesDict.Values.ToList();

            Assert.Equal(2, dependenciesDict.Count);
            Assert.Equal(A1, keysList[0]);
            Assert.Contains(B1, valuesList[0]);
            Assert.Contains(C1, valuesList[0]);
            Assert.Equal(C1, keysList[1]);
            Assert.Contains(D1, valuesList[1]);
            Assert.Contains(E1, valuesList[1]);
        }

        [Fact]
        public void TestUninstallResolverSimplePass()
        {
            // Act
            var result = UninstallResolver.GetPackagesToBeUninstalled(A1,
                PackageDependencyInfo1,
                InstalledPackages1,
                new UninstallationContext(removeDependencies: true)).ToList();

            // Assert
            Assert.Equal(5, result.Count);
            Assert.True(result[0].Equals(A1));
            Assert.True(result[1].Equals(B1));
            Assert.True(result[2].Equals(C1));
            Assert.True(result[3].Equals(D1));
            Assert.True(result[4].Equals(E1));
        }

        [Fact]
        public void TestUninstallResolverSimpleFail()
        {
            // Act
            Exception exception = null;
            try
            {
                var result = UninstallResolver.GetPackagesToBeUninstalled(E1,
                    PackageDependencyInfo1,
                    InstalledPackages1,
                    new UninstallationContext(removeDependencies: true)).ToList();
            }
            catch (InvalidOperationException ex)
            {
                exception = ex;
            }
            catch (AggregateException ex)
            {
                exception = ExceptionUtility.Unwrap(ex);
            }

            // Assert
            Assert.NotNull(exception);
            Assert.True(exception is InvalidOperationException);
            Assert.Equal("Unable to uninstall 'E.1.0.0' because 'C.1.0.0' depends on it.",
                exception.Message);
        }

        [Fact]
        public void DiamondDependencyUninstall()
        {
            // Act
            var result = UninstallResolver.GetPackagesToBeUninstalled(A1,
                DiamondDependencyInfo,
                DiamondDependencyInstalledPackages,
                new UninstallationContext(removeDependencies: true)).ToList();

            // Assert
            Assert.Equal(4, result.Count);
            Assert.True(result[0].Equals(A1));
            Assert.True(result[1].Equals(B1));
            Assert.True(result[2].Equals(C1));
            Assert.True(result[3].Equals(D1));
        }

        [Fact]
        public void DeepDiamondDependencyUninstall()
        {
            // Act
            var result = UninstallResolver.GetPackagesToBeUninstalled(A1,
                DeepDiamondDependencyInfo,
                DeepDiamondDependencyInstalledPackages,
                new UninstallationContext(removeDependencies: true)).ToList();

            // Assert
            Assert.Equal(5, result.Count);
            Assert.True(result[0].Equals(A1));
            Assert.True(result[1].Equals(B1));
            Assert.True(result[2].Equals(C1));
            Assert.True(result[3].Equals(E1));
            Assert.True(result[4].Equals(D1));
        }
    }
}
