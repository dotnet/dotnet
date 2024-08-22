// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Configuration;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class VSRestoreSettingsUtilityTests
    {
        [Theory]
        [MemberData(nameof(GetVSRestoreSettingsUtilities_RestoreSourceData))]
        public void VSRestoreSettingsUtilities_RestoreSource(string[] restoreSources, string[] expectedRestoreSources)
        {
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                //Set Up
                var spec = new PackageSpec
                {
                    RestoreMetadata = new ProjectRestoreMetadata
                    {
                        ProjectPath = @"C:\project\projectPath.csproj",
                        Sources = restoreSources.Select(e => new PackageSource(e)).ToList()
                    }
                };
                var settings = new Settings(mockBaseDirectory);

                //Act
                var actualSources = VSRestoreSettingsUtilities.GetSources(settings, spec);

                //Assert
                Assert.True(
                       Enumerable.SequenceEqual(expectedRestoreSources.OrderBy(t => t), actualSources.Select(e => e.Source).OrderBy(t => t)),
                       "expected: " + string.Join(",", expectedRestoreSources.ToArray()) + "\nactual: " + string.Join(",", actualSources.Select(e => e.Source).ToArray()));
            }
        }

        public static IEnumerable<object[]> GetVSRestoreSettingsUtilities_RestoreSourceData()
        {
            yield return new object[] {
                new string[] { @"C:\source1" },
                new string[] { @"C:\source1" }
            };

            yield return new object[]
            {
                new string[] { @"Clear" },
                new string[] { }
            };

            yield return new object[]
            {
                new string[] { @"Clear", VSRestoreSettingsUtilities.AdditionalValue, @"C:\additionalSource" },
                new string[] { @"C:\additionalSource" }
            };

            yield return new object[] {
                new string[] { @"C:\source1", VSRestoreSettingsUtilities.AdditionalValue, @"C:\additionalSource" },
                new string[] { @"C:\source1" ,@"C:\additionalSource" }
            };

            yield return new object[]
            {
                new string[] { VSRestoreSettingsUtilities.AdditionalValue, @"C:\additionalSource" },
                new string[] { NuGetConstants.V3FeedUrl, @"C:\additionalSource" }
            };
        }

        [Theory]
        [MemberData(nameof(GetVSRestoreSettingsUtilities_FallbackFolderData))]
        public void VSRestoreSettingsUtilities_FallbackFolder(string[] fallbackFolders, string[] expectedFallbackFolders)
        {
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                //Set Up
                var spec = new PackageSpec
                {
                    RestoreMetadata = new ProjectRestoreMetadata
                    {
                        ProjectPath = @"C:\project\projectPath.csproj",
                        FallbackFolders = fallbackFolders.ToList()
                    }
                };
                var settings = new Settings(mockBaseDirectory);
                settings.AddOrUpdate(ConfigurationConstants.FallbackPackageFolders, new AddItem("defaultFallback", @"C:\defaultFallback"));

                //Act
                var actualFallbackFolders = VSRestoreSettingsUtilities.GetFallbackFolders(settings, spec);

                //Assert
                Assert.True(
                       Enumerable.SequenceEqual(expectedFallbackFolders.OrderBy(t => t), actualFallbackFolders.OrderBy(t => t)),
                       "expected: " + string.Join(",", expectedFallbackFolders.ToArray()) + "\nactual: " + string.Join(",", actualFallbackFolders.ToArray()));
            }
        }

        public static IEnumerable<object[]> GetVSRestoreSettingsUtilities_FallbackFolderData()
        {
            yield return new object[] {
                new string[] { @"C:\fallback1" },
                new string[] { @"C:\fallback1" }
            };

            yield return new object[]
            {
                new string[] { @"Clear" },
                new string[] { }
            };

            yield return new object[]
            {
                new string[] { @"Clear", VSRestoreSettingsUtilities.AdditionalValue, @"C:\additionalFallback" },
                new string[] { @"C:\additionalFallback" }
            };

            yield return new object[] {
                new string[] { @"C:\fallback1", VSRestoreSettingsUtilities.AdditionalValue, @"C:\additionalFallback" },
                new string[] { @"C:\fallback1", @"C:\additionalFallback" }
            };

            yield return new object[]
            {
                new string[] { VSRestoreSettingsUtilities.AdditionalValue, @"C:\additionalFallback" },
                new string[] { @"C:\defaultFallback", @"C:\additionalFallback" }
            };
        }

        [Theory]
        [InlineData(@"C:\packagePath", @"C:\packagePath")]
        [InlineData(null, @"C:\defaultPackagesPath")]
        [InlineData("globalPackages", @"C:\project\globalPackages")]
        public void VSRestoreSettingsUtilities_PackagePath(string packagesPath, string expectedPackagesPath)
        {
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                // Set Up
                var spec = new PackageSpec
                {
                    RestoreMetadata = new ProjectRestoreMetadata
                    {
                        ProjectPath = @"C:\project\projectPath.csproj",
                        PackagesPath = packagesPath
                    }
                };
                var settings = new Settings(mockBaseDirectory);
                settings.AddOrUpdate(ConfigurationConstants.Config, new AddItem("globalPackagesFolder", @"C:\defaultPackagesPath"));

                // Act
                var actualPackagesPath = VSRestoreSettingsUtilities.GetPackagesPath(settings, spec);

                //Assert
                Assert.Equal(expectedPackagesPath, actualPackagesPath);
            }
        }

    }
}
