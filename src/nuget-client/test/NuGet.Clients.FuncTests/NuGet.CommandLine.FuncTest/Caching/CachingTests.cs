// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.CommandLine.Test.Caching
{
    /// <summary>
    /// The thought behind this test suite is to validate every permutation of test matrix comprised
    /// of the following variables:
    /// - Caching aspect (implementation of ICachingTest)
    /// - Installation command (implementations of ICachingCommand)
    /// - Caching options (NoCache or DirectDownload)
    /// - Server type (V2 or V3)
    /// </summary>
    public class CachingTests
    {
        /// <summary>
        /// This is a sanity check test that does not verify any caching.
        /// </summary>
        [Theory]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V2)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V3)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V2)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V3)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V2)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V3)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V2)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V3)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V2)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V3)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V2)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V3)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V2)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V3)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V2)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V3)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V2)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V3)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V2)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V3)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V2)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V3)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V2)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V3)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3)]
        public async Task NuGetExe_Caching_InstallsToDestinationFolder(Type type, CachingType caching, ServerType server)
        {
            // Arrange
            var nuGetExe = await GetNuGetExeAsync();

            // Act
            var validations = await CachingTestRunner.ExecuteAsync(
                typeof(InstallsToDestinationFolderTest),
                type,
                nuGetExe,
                caching,
                server);

            // Assert
            foreach (var validation in validations)
            {
                validation.Assert(CachingValidationType.CommandSucceeded, true);
                validation.Assert(CachingValidationType.PackageInstalled, true);
            }
        }

        /// <summary>
        /// Inconsistencies tracked here:
        /// https://github.com/NuGet/Home/issues/3244
        /// </summary>
        [Theory]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V2, true, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V3, true, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V2, false, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V3, false, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, true, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, true, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V2, true, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V3, true, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V2, false, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V3, false, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V2, true, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V3, true, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V2, true, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V3, true, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V2, false, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V3, false, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, true, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, true, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V2, true, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V3, true, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V2, true, true)] // Should fail?
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V3, true, true)] // Should fail?
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V2, true, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V3, true, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, true, true)] // Should fail?
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, true, true)] // Should fail?
        public async Task NuGetExe_Caching_AllowsMissingPackageOnSource(Type type, CachingType caching, ServerType server, bool success, bool installed)
        {
            // Arrange
            var nuGetExe = await GetNuGetExeAsync();

            // Act
            var validations = await CachingTestRunner.ExecuteAsync(
                typeof(AllowsMissingPackageOnSourceTest),
                type,
                nuGetExe,
                caching,
                server);

            // Assert
            foreach (var validation in validations)
            {
                validation.Assert(CachingValidationType.CommandSucceeded, success);
                validation.Assert(CachingValidationType.PackageInstalled, installed);
            }
        }

        /// <summary>
        /// The -DirectDownload switch allows the users to skip the writing a package to the global packages directory.
        /// This does not apply to a project.json restore since the global packages directory itself is considering the
        /// destination directory.
        /// </summary>
        [Theory]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V2, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V3, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V2, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V3, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V2, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V3, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V2, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V3, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V2, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V3, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V2, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V3, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V3, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V3, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, true)]
        public async Task NuGetExe_Caching_PopulatesGlobalPackagesFolder(Type type, CachingType caching, ServerType server, bool success)
        {
            // Arrange
            var nuGetExe = await GetNuGetExeAsync();

            // Act
            var validations = await CachingTestRunner.ExecuteAsync(
                typeof(PopulatesGlobalPackagesFolderTest),
                type,
                nuGetExe,
                caching,
                server);

            // Assert

            foreach (var validation in validations)
            {
                validation.Assert(CachingValidationType.CommandSucceeded, true);
                validation.Assert(CachingValidationType.PackageInGlobalPackagesFolder, success);
            }
        }

        /// <summary>
        /// -NoCache disables reading from the global packages folder.
        /// </summary>
        [Theory]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V2, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V3, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V2, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V3, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V2, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V3, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V3, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V3, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, true)]
        public async Task NuGetExe_Caching_UsesGlobalPackagesFolderCopy(Type type, CachingType caching, ServerType server, bool success)
        {
            // Arrange
            var nuGetExe = await GetNuGetExeAsync();

            // Act
            var validations = await CachingTestRunner.ExecuteAsync(
                typeof(UsesGlobalPackagesFolderCopyTest),
                type,
                nuGetExe,
                caching,
                server);

            // Assert
            foreach (var validation in validations)
            {
                validation.Assert(CachingValidationType.CommandSucceeded, true);
                validation.Assert(CachingValidationType.PackageInstalled, true);
                validation.Assert(CachingValidationType.PackageFromGlobalPackagesFolderUsed, success);
                validation.Assert(CachingValidationType.PackageFromSourceNotUsed, success);
            }
        }

        [Theory]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V2, true, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V3, true, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V2, true, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V3, true, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V2, true, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V3, true, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, true, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, true, false)]
        public async Task NuGetExe_Caching_DoesNotNoOp(Type type, CachingType caching, ServerType server, bool success, bool noOp)
        {
            // Arrange
            var nuGetExe = await GetNuGetExeAsync();

            // Act
            var validations = await CachingTestRunner.ExecuteAsync(
                typeof(UsesGlobalPackageFolderCopyOnEveryRunTest),
                type,
                nuGetExe,
                caching,
                server);

            // Assert
            var firstPass = true;
            foreach (var validation in validations)
            {
                validation.Assert(CachingValidationType.CommandSucceeded, true);
                validation.Assert(CachingValidationType.PackageInstalled, true);
                validation.Assert(CachingValidationType.PackageFromGlobalPackagesFolderUsed, success);
                validation.Assert(CachingValidationType.PackageFromSourceNotUsed, success);
                if (firstPass)
                {
                    firstPass = false;
                    validation.Assert(CachingValidationType.RestoreNoOp, false);
                }
                else
                {
                    validation.Assert(CachingValidationType.RestoreNoOp, noOp);
                }
            }
        }

        /// <summary>
        /// Currently, only project.json restore uses the HTTP cache. Eventually, the
        /// packages.config should use the HTTP cache as well.
        /// https://github.com/NuGet/Home/issues/3132
        /// </summary>
        [Theory]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V3, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V3, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        public async Task NuGetExe_Caching_UsesHttpCacheCopy(Type type, CachingType caching, ServerType server, bool success)
        {
            // Arrange
            var nuGetExe = await GetNuGetExeAsync();

            // Act
            var validations = await CachingTestRunner.ExecuteAsync(
                typeof(UsesHttpCacheCopyTest),
                type,
                nuGetExe,
                caching,
                server);

            // Assert
            foreach (var validation in validations)
            {
                validation.Assert(CachingValidationType.CommandSucceeded, true);
                validation.Assert(CachingValidationType.PackageInstalled, true);
                validation.Assert(CachingValidationType.PackageFromHttpCacheUsed, success);
                validation.Assert(CachingValidationType.PackageFromSourceNotUsed, success);
            }
        }

        /// <summary>
        /// Currently, only project.json restore uses the HTTP cache. Eventually, the
        /// packages.config should use the HTTP cache as well.
        /// https://github.com/NuGet/Home/issues/3132
        /// </summary>
        [Theory]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V3, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V2, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V3, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        public async Task NuGetExe_Caching_WritesToHttpCache(Type type, CachingType caching, ServerType server, bool success)
        {
            // Arrange
            var nuGetExe = await GetNuGetExeAsync();

            // Act
            var validations = await CachingTestRunner.ExecuteAsync(
                typeof(WritesToHttpCacheTest),
                type,
                nuGetExe,
                caching,
                server);

            // Assert
            foreach (var validation in validations)
            {
                validation.Assert(CachingValidationType.CommandSucceeded, true);
                validation.Assert(CachingValidationType.PackageInHttpCache, success);
            }
        }

        /// <summary>
        /// project.json restores do not use .nugetdirectdownload temporary files. Also, when you do not specify
        /// -DirectDownload, .nugetdirectdownload files are not cleaned up.
        /// </summary>
        [Theory]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.Default, ServerType.V3, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(InstallPackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.Default, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(InstallSpecificVersionCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.Default, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, true)]
        [InlineData(typeof(RestorePackagesConfigCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, true)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V2, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.Default, ServerType.V3, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V2, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache, ServerType.V3, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.DirectDownload, ServerType.V3, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V2, false)]
        [InlineData(typeof(RestorePackageReferenceCommand), CachingType.NoCache | CachingType.DirectDownload, ServerType.V3, false)]
        public async Task NuGetExe_Caching_CleansUpDirectDownload(Type type, CachingType caching, ServerType server, bool success)
        {
            // Arrange
            var nuGetExe = await GetNuGetExeAsync();

            // Act
            var validations = await CachingTestRunner.ExecuteAsync(
                typeof(CleansUpDirectDownloadTest),
                type,
                nuGetExe,
                caching,
                server);

            // Assert
            foreach (var validation in validations)
            {
                validation.Assert(CachingValidationType.CommandSucceeded, true);
                validation.Assert(CachingValidationType.DirectDownloadFilesDoNotExist, success);
            }
        }

        private static async Task<INuGetExe> GetNuGetExeAsync()
        {
            await Task.Yield();

            var nuGetExe = NuGetExe.GetBuiltNuGetExe();

            return nuGetExe;
        }
    }
}
