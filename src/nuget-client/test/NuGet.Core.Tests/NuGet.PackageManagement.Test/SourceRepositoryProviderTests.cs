// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Test.Utility;
using Xunit;

namespace NuGet.Test
{
    public class SourceRepositoryProviderTests
    {
        [Fact]
        public void TestSourceRepoPackageSourcesChanged()
        {
            // Arrange
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var localPackageSource = new Configuration.PackageSource(localAppDataPath);
            var oldPackageSources = new List<Configuration.PackageSource> { localPackageSource };
            var packageSourceProvider = new TestPackageSourceProvider(oldPackageSources);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(packageSourceProvider);

            // Act
            var oldEffectivePackageSources = sourceRepositoryProvider.GetRepositories().ToList();

            // Assert
            Assert.Equal(1, oldEffectivePackageSources.Count);
            Assert.Equal(localAppDataPath, oldEffectivePackageSources[0].PackageSource.Source);

            // Main Act
            var newPackageSources = new List<Configuration.PackageSource> { TestSourceRepositoryUtility.V3PackageSource, localPackageSource };
            packageSourceProvider.SavePackageSources(newPackageSources);

            var newEffectivePackageSources = sourceRepositoryProvider.GetRepositories().ToList();

            // Main Assert
            Assert.Equal(2, newEffectivePackageSources.Count);
            Assert.Equal(TestSourceRepositoryUtility.V3PackageSource.Source, newEffectivePackageSources[0].PackageSource.Source);
            Assert.Equal(localAppDataPath, newEffectivePackageSources[1].PackageSource.Source);
        }

        [Fact]
        public void TestSourceRepoPackageSourcesChanged2()
        {
            // Arrange
            using (var settingsPath = TestPackageSourceSettings.CreateAndGetSettingFilePath())
            {
                var settings = new Configuration.Settings(settingsPath);
                var packageSourceProvider = new Configuration.PackageSourceProvider(settings);
                var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(packageSourceProvider);

                // Act
                var oldEffectivePackageSources = sourceRepositoryProvider.GetRepositories().ToList();

                // Assert
                Assert.Equal(1, oldEffectivePackageSources.Count);
                Assert.Equal(TestSourceRepositoryUtility.V2PackageSource.Source, oldEffectivePackageSources[0].PackageSource.Source);

                // Main Act
                var newPackageSources = new List<Configuration.PackageSource>
                {
                    TestSourceRepositoryUtility.V3PackageSource,
                    TestSourceRepositoryUtility.V2PackageSource
                };
                packageSourceProvider.SavePackageSources(newPackageSources);

                var newEffectivePackageSources = sourceRepositoryProvider.GetRepositories().ToList();

                // Main Assert
                Assert.Equal(2, newEffectivePackageSources.Count);
                Assert.Equal(TestSourceRepositoryUtility.V3PackageSource.Source, newEffectivePackageSources[0].PackageSource.Source);
                Assert.Equal(TestSourceRepositoryUtility.V2PackageSource.Source, newEffectivePackageSources[1].PackageSource.Source);
            }
        }
    }
}
