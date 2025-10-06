// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Utility
{
    public class PackageSourceMappingUtilityTests
    {
        [Fact]
        public void AddNewSourceMappingsFromAddedPackages_PackageNotMappedToMultipleEnabledSources_CreatesSingleNewMapping()
        {
            // Arrange
            string topLevelPackageId = "a";
            Dictionary<string, SortedSet<string>> newSourceMappings = new();

            // Existing Package Source Mappings.
            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "b", "c", "d" } },
                { "sourceB", new List<string>() { "b", "c", "d" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);

            // Configure packages which are added by preview restore and may need new Package Source Mappings.
            string selectedSourceName = "sourceC";
            List<AccessiblePackageIdentity> added = new List<AccessiblePackageIdentity>()
            {
                ConvertToAccessiblePackageIdentity("a"), // The only added package missing from existing mappings.
                ConvertToAccessiblePackageIdentity("b"),
                ConvertToAccessiblePackageIdentity("c"),
                ConvertToAccessiblePackageIdentity("d"),
            };

            // Repeat the Act to simulate multiple ProjectActions to ensure a unique resulting set of packages is created.
            for (int i = 0; i < 2; i++)
            {
                // Act
                PackageSourceMappingUtility.AddNewSourceMappingsFromAddedPackages(
                    ref newSourceMappings,
                    selectedSourceName,
                    topLevelPackageId,
                    added,
                    mockPackageSourceMapping.Object,
                    globalPackageFolders: null,
                    enabledSourceRepositories: new List<SourceRepository>(capacity: 2)
                    {
                        new SourceRepository(new PackageSource("sourceA"), new List<INuGetResourceProvider>()),
                    },
                    logger: Mock.Of<INuGetUILogger>());

                // Assert
                Assert.Equal(1, newSourceMappings.Count);
                Assert.True(newSourceMappings.ContainsKey(selectedSourceName));
                Assert.Equal(1, newSourceMappings[selectedSourceName].Count);
            }
        }

        /// <summary>
        /// Install package `a` which is not currently mapped.
        /// Selected source: `sourceA`
        /// Enabled sources: `sourceA`, `sourceB`, `sourceC`.
        /// Added packages are: `a`, `b`, `c`, `d`.
        /// Source mappings exist for: `a`, `b`, `d`.
        /// Transitive dependency `c` does not exist in the GPF, so it should now be mapped to the selected source (`sourceA`).
        /// </summary>
        [Fact]
        public void AddNewSourceMappingsFromAddedPackages_TransitiveDoesNotExistInGPF_CreatesNewMappingToSelectedSource()
        {
            // Arrange
            string topLevelPackageId = "a";
            string selectedSourceName = "sourceA";
            AccessiblePackageIdentity packageC = ConvertToAccessiblePackageIdentity("c");

            // Existing Package Source Mappings.
            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "a", "b", } },
                { "sourceB", new List<string>() { "d" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);
            var contextPackageC = new SimpleTestPackageContext(packageC);

            // Configure packages which are added by preview restore and may need new Package Source Mappings.
            List<AccessiblePackageIdentity> added = new List<AccessiblePackageIdentity>()
            {
                ConvertToAccessiblePackageIdentity("a"),
                ConvertToAccessiblePackageIdentity("b"),
                packageC,
                ConvertToAccessiblePackageIdentity("d"),
            };

            SourceRepository sourceA = CreateSourceRepository("sourceA");
            SourceRepository sourceB = CreateSourceRepository("sourceB");
            SourceRepository sourceC = CreateSourceRepository("sourceC");

            IReadOnlyList<SourceRepository> enabledSourceRepositories = new List<SourceRepository>(capacity: added.Count)
            {
                sourceA,
                sourceB,
                sourceC,
            };

            using SimpleTestPathContext gpfContext = new SimpleTestPathContext();

            SourceRepository gpfSourceRepository = CreateGPFSourceRepository(gpfContext.UserPackagesFolder);

            IReadOnlyList<SourceRepository> globalPackageFolders = new List<SourceRepository>(capacity: 1)
            {
                gpfSourceRepository
            };

            Dictionary<string, SortedSet<string>> newSourceMappings = new();

            // Repeat the Act to simulate multiple ProjectActions to ensure a unique resulting set of packages is created.
            for (int i = 0; i < 2; i++)
            {
                // Act
                PackageSourceMappingUtility.AddNewSourceMappingsFromAddedPackages(
                    ref newSourceMappings,
                    selectedSourceName,
                    topLevelPackageId,
                    added,
                    mockPackageSourceMapping.Object,
                    globalPackageFolders,
                    enabledSourceRepositories,
                    logger: Mock.Of<INuGetUILogger>());

                // Assert
                Assert.Equal(1, newSourceMappings.Count);
                Assert.True(newSourceMappings.ContainsKey(selectedSourceName));
                Assert.Equal(1, newSourceMappings[selectedSourceName].Count);
                Assert.Contains(packageC.Id, newSourceMappings[selectedSourceName]);
            }
        }

        /// <summary>
        /// Install package `a` which is not currently mapped.
        /// Selected source: `sourceA`
        /// Enabled sources: `sourceA`, `sourceB`, `sourceC`.
        /// Added packages are: `a`, `b`, `c`, `d`.
        /// Source mappings exist for: `a`, `b`, `d`.
        /// Transitive dependency `c` exists in the GPF to `sourceC`, which is an enabled source and should now be mapped.
        /// </summary>
        [Fact]
        public async Task AddNewSourceMappingsFromAddedPackages_TransitiveExistsInGPFToEnabledSource_CreatesNewMappingToGPFSource()
        {
            // Arrange
            string topLevelPackageId = "a";
            string selectedSourceName = "sourceA";
            AccessiblePackageIdentity packageC = ConvertToAccessiblePackageIdentity("c");

            // Existing Package Source Mappings.
            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "a", "b", } },
                { "sourceB", new List<string>() { "d" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);
            var contextPackageC = new SimpleTestPackageContext(packageC);

            // Configure packages which are added by preview restore and may need new Package Source Mappings.
            List<AccessiblePackageIdentity> added = new List<AccessiblePackageIdentity>()
            {
                ConvertToAccessiblePackageIdentity("a"),
                ConvertToAccessiblePackageIdentity("b"),
                packageC,
                ConvertToAccessiblePackageIdentity("d"),
            };

            SourceRepository sourceA = CreateSourceRepository("sourceA");
            SourceRepository sourceB = CreateSourceRepository("sourceB");
            SourceRepository sourceC = CreateSourceRepository("sourceC");

            IReadOnlyList<SourceRepository> enabledSourceRepositories = new List<SourceRepository>(capacity: added.Count)
            {
                sourceA,
                sourceB,
                sourceC,
            };

            using SimpleTestPathContext gpfContext = new SimpleTestPathContext();

            SourceRepository gpfSourceRepository = CreateGPFSourceRepository(gpfContext.UserPackagesFolder);
            await SimpleTestPackageUtility.CreateFolderFeedV3WithNupkgMetadataAsync(
                root: gpfContext.UserPackagesFolder,
                nupkgMetadataSource: sourceC.PackageSource.Source,
                contextPackageC);

            string nupkgMetadataSourceName = sourceC.PackageSource.Name;

            IReadOnlyList<SourceRepository> globalPackageFolders = new List<SourceRepository>(capacity: 1)
            {
                gpfSourceRepository
            };

            Dictionary<string, SortedSet<string>> newSourceMappings = new();

            // Repeat the Act to simulate multiple ProjectActions to ensure a unique resulting set of packages is created.
            for (int i = 0; i < 2; i++)
            {
                // Act
                PackageSourceMappingUtility.AddNewSourceMappingsFromAddedPackages(
                    ref newSourceMappings,
                    selectedSourceName,
                    topLevelPackageId,
                    added,
                    mockPackageSourceMapping.Object,
                    globalPackageFolders,
                    enabledSourceRepositories,
                    logger: Mock.Of<INuGetUILogger>());

                // Assert
                Assert.Equal(1, newSourceMappings.Count);
                Assert.True(newSourceMappings.ContainsKey(nupkgMetadataSourceName));
                Assert.Equal(1, newSourceMappings[nupkgMetadataSourceName].Count);
                Assert.Contains(packageC.Id, newSourceMappings[nupkgMetadataSourceName]);
            }
        }

        /// <summary>
        /// Install package `a` which is not currently mapped.
        /// Selected source: `sourceA`
        /// Enabled sources: `sourceA`, `sourceB`.
        /// Added packages are: `a`, `b`, `c`, `d`, `e`, `f`.
        /// Source mappings exist for: `a`, `b`, `d`.
        /// Transitive dependency `c`, `e`, `f` exist in the GPF, but the indicated package source `sourceC` is not enabled so
        /// we log NU1110 for each.
        /// </summary>
        [Fact]
        public async Task AddNewSourceMappingsFromAddedPackages_TransitiveExistsInGPFToNotEnabledSource_LogsAndThrowsNU1110()
        {
            // Arrange
            string topLevelPackageId = "a";
            string selectedSourceName = "sourceA";

            List<ILogMessage> loggedMessages = new(capacity: 10);
            Mock<INuGetUILogger> mockLogger = new Mock<INuGetUILogger>();
            mockLogger.Setup(logger => logger.Log(It.IsAny<ILogMessage>()))
                .Callback<ILogMessage>(loggedMessage =>
                {
                    loggedMessages.Add(loggedMessage);
                });
            INuGetUILogger logger = mockLogger.Object;

            AccessiblePackageIdentity packageC = ConvertToAccessiblePackageIdentity("c");
            AccessiblePackageIdentity packageE = ConvertToAccessiblePackageIdentity("e");
            AccessiblePackageIdentity packageF = ConvertToAccessiblePackageIdentity("f");

            // Existing Package Source Mappings.
            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "a", "b", } },
                { "sourceB", new List<string>() { "d" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);
            var contextPackageC = new SimpleTestPackageContext(packageC);
            var contextPackageE = new SimpleTestPackageContext(packageE);
            var contextPackageF = new SimpleTestPackageContext(packageF);

            // Configure packages which are added by preview restore and may need new Package Source Mappings.
            List<AccessiblePackageIdentity> added = new List<AccessiblePackageIdentity>()
            {
                ConvertToAccessiblePackageIdentity("a"),
                ConvertToAccessiblePackageIdentity("b"),
                packageC,
                ConvertToAccessiblePackageIdentity("d"),
                packageE,
                packageF
            };

            SourceRepository sourceA = CreateSourceRepository("sourceA");
            SourceRepository sourceB = CreateSourceRepository("sourceB");
            SourceRepository sourceC = CreateSourceRepository("sourceC");

            IReadOnlyList<SourceRepository> enabledSourceRepositories = new List<SourceRepository>(capacity: added.Count)
            {
                sourceA,
                sourceB,
            };

            using SimpleTestPathContext gpfContext = new SimpleTestPathContext();

            SourceRepository gpfSourceRepository = CreateGPFSourceRepository(gpfContext.UserPackagesFolder);
            await SimpleTestPackageUtility.CreateFolderFeedV3WithNupkgMetadataAsync(
                root: gpfContext.UserPackagesFolder,
                nupkgMetadataSource: sourceC.PackageSource.Source,
                contextPackageC,
                contextPackageE,
                contextPackageF);

            string nupkgMetadataSourceName = sourceC.PackageSource.Name;

            IReadOnlyList<SourceRepository> globalPackageFolders = new List<SourceRepository>(capacity: 1)
            {
                gpfSourceRepository
            };

            Dictionary<string, SortedSet<string>> newSourceMappings = new();

            // Repeat the Act to simulate multiple ProjectActions to ensure a unique resulting set of packages is created.
            for (int i = 0; i < 2; i++)
            {
                // Reset logger for each project.
                loggedMessages.Clear();

                // Act
                bool isSuccessful = PackageSourceMappingUtility.AddNewSourceMappingsFromAddedPackages(
                    ref newSourceMappings,
                    selectedSourceName,
                    topLevelPackageId,
                    added,
                    mockPackageSourceMapping.Object,
                    globalPackageFolders,
                    enabledSourceRepositories,
                    logger);

                // Assert
                int expectedErrorCount = 3;

                var expectedExceptionMessages = new string[expectedErrorCount];
                expectedExceptionMessages[0] = $"The package `{contextPackageC.Id}` is available in the Global packages folder," +
                    $" but the source it came from `{sourceC.PackageSource.Source}` is not one of the configured sources.";
                expectedExceptionMessages[1] = $"The package `{contextPackageE.Id}` is available in the Global packages folder," +
                    $" but the source it came from `{sourceC.PackageSource.Source}` is not one of the configured sources.";
                expectedExceptionMessages[2] = $"The package `{contextPackageF.Id}` is available in the Global packages folder," +
                    $" but the source it came from `{sourceC.PackageSource.Source}` is not one of the configured sources.";

                Assert.False(isSuccessful);
                Assert.Equal(expectedErrorCount, loggedMessages.Count);

                for (int j = 0; j < expectedErrorCount; j++)
                {
                    ILogMessage loggedMessage = loggedMessages[j];
                    Assert.Equal(LogLevel.Error, loggedMessage.Level);
                    Assert.Equal(NuGetLogCode.NU1110, loggedMessage.Code);
                    Assert.Equal(expectedExceptionMessages[j], loggedMessage.Message);
                }
            }
        }

        private AccessiblePackageIdentity ConvertToAccessiblePackageIdentity(string packageId)
        {
            return new AccessiblePackageIdentity(new PackageIdentity(packageId, NuGetVersion.Parse("1.0.0")));
        }

        private static SourceRepository CreateGPFSourceRepository(string gpfPath)
        {
            PackageSource gpfPackageSource = new(gpfPath);

            var mockSourceRepository = new Mock<SourceRepository>(gpfPackageSource, new List<INuGetResourceProvider>());
            mockSourceRepository.SetupGet(m => m.PackageSource).Returns(gpfPackageSource);

            return mockSourceRepository.Object;
        }

        private static SourceRepository CreateSourceRepository(string name, bool isEnabled = true)
        {
            string source = $"https://{name}.testsource.com/v3/index.json";
            PackageSource packageSource = new(source, name);
            packageSource.IsEnabled = isEnabled;

            var mockSourceRepository = new Mock<SourceRepository>(packageSource, new List<INuGetResourceProvider>());
            mockSourceRepository.SetupGet(m => m.PackageSource).Returns(packageSource);

            return mockSourceRepository.Object;
        }
    }
}
