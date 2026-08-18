// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Build.Framework;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Build.Tasks.Pack.Test
{
    public class PackTaskTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PackTaskTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void PackTask_DelegatesToPackLogic()
        {
            // Arrange
            var packArgs = new PackArgs();
            var packageBuilder = new PackageBuilder();
            var packCommandRunner = new PackCommandRunner(null, null);
            IPackTaskRequest<IMSBuildItem> request = null;

            var logic = new Mock<IPackTaskLogic>();
            logic
                .Setup(x => x.GetPackArgs(It.IsAny<IPackTaskRequest<IMSBuildItem>>()))
                .Returns(packArgs)
                .Callback<IPackTaskRequest<IMSBuildItem>>(r => request = r);
            logic
                .Setup(x => x.GetPackageBuilder(It.IsAny<IPackTaskRequest<IMSBuildItem>>()))
                .Returns(packageBuilder);
            logic
                .Setup(x => x.GetPackCommandRunner(It.IsAny<IPackTaskRequest<IMSBuildItem>>(), packArgs, packageBuilder))
                .Returns(packCommandRunner);

            var target = new PackTask();
            target.PackTaskLogic = logic.Object;

            // Act
            var result = target.Execute();

            // Assert
            // We cannot mock the PackCommandRunner because it's not overridable.
            Assert.False(result);
            Assert.NotNull(request);
            logic.Verify(x => x.GetPackArgs(request));
            logic.Verify(x => x.GetPackageBuilder(request));
            logic.Verify(x => x.GetPackCommandRunner(request, packArgs, packageBuilder));
            logic.Verify(x => x.BuildPackage(packCommandRunner));
        }

        [Fact]
        public void PackTask_Dispose()
        {
            using (var directory = TestDirectory.Create())
            {
                var nuspecPath = Path.Combine(directory, "test.nuspec");
                File.WriteAllText(nuspecPath, @"
<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">
  <metadata>
    <id>Test</id>
    <summary>Summary</summary>
    <description>Description</description>
    <version>1.0.0</version>
    <authors>Microsoft</authors>
    <dependencies>
      <dependency id=""System.Collections.Immutable"" version=""4.3.0"" />
    </dependencies>
  </metadata>
</package>
");

                var builder = new PackageBuilder();

                var runner = new PackCommandRunner(
                    new PackArgs
                    {
                        CurrentDirectory = directory,
                        OutputDirectory = directory,
                        Path = nuspecPath,
                        Exclude = Array.Empty<string>(),
                        Symbols = true,
                        Logger = NullLogger.Instance
                    },
                    MSBuildProjectFactory.ProjectCreator,
                    builder);

                runner.RunPackageBuild();
            }
        }

        [Fact]
        public void PackTask_TrimsWhitespace()
        {
            // Arrange
            var target = new PackTask
            {
                AssemblyName = " AssemblyName \t ",
                BuildOutputFolders = new string[] { " BuildOutputFolder \t " },
                Copyright = " Copyright \t ",
                Description = " Description \t ",
                IconUrl = " IconUrl \t ",
                LicenseUrl = " LicenseUrl \t ",
                MinClientVersion = " MinClientVersion \t ",
                NuspecOutputPath = " NuspecOutputPath \t ",
                PackageId = " PackageId \t ",
                PackageOutputPath = " PackageOutputPath \t ",
                PackageVersion = " PackageVersion \t ",
                ProjectUrl = " ProjectUrl \t ",
                ReleaseNotes = " ReleaseNotes \t ",
                RepositoryType = " RepositoryType \t ",
                RepositoryUrl = " RepositoryUrl \t ",
                RepositoryCommit = " RepositoryCommit \t ",
                RepositoryBranch = " RepositoryBranch \t "
            };

            // Act
            var actual = GetRequest(target);

            // Assert
            Assert.Equal("AssemblyName", actual.AssemblyName);
            Assert.Equal("BuildOutputFolder", actual.BuildOutputFolders[0]);
            Assert.Equal("Copyright", actual.Copyright);
            Assert.Equal("Description", actual.Description);
            Assert.Equal("IconUrl", actual.IconUrl);
            Assert.Equal("LicenseUrl", actual.LicenseUrl);
            Assert.Equal("MinClientVersion", actual.MinClientVersion);
            Assert.Equal("NuspecOutputPath", actual.NuspecOutputPath);
            Assert.Equal("PackageId", actual.PackageId);
            Assert.Equal("PackageOutputPath", actual.PackageOutputPath);
            Assert.Equal("PackageVersion", actual.PackageVersion);
            Assert.Equal("ProjectUrl", actual.ProjectUrl);
            Assert.Equal("ReleaseNotes", actual.ReleaseNotes);
            Assert.Equal("RepositoryType", actual.RepositoryType);
            Assert.Equal("RepositoryUrl", actual.RepositoryUrl);
            Assert.Equal("RepositoryCommit", actual.RepositoryCommit);
            Assert.Equal("RepositoryBranch", actual.RepositoryBranch);
        }

        [Fact]
        public void PackTask_CoalescesWhitespaceToNull()
        {
            // Arrange
            var target = new PackTask
            {
                AssemblyName = " \t ",
                BuildOutputFolders = Array.Empty<string>(),
                Copyright = " \t ",
                Description = " \t ",
                IconUrl = " \t ",
                LicenseUrl = " \t ",
                MinClientVersion = " \t ",
                NuspecOutputPath = " \t ",
                PackageId = " \t ",
                PackageOutputPath = " \t ",
                PackageVersion = " \t ",
                ProjectUrl = " \t ",
                ReleaseNotes = " \t ",
                RepositoryType = " \t ",
                RepositoryUrl = " \t ",
                RepositoryCommit = " \t ",
                RepositoryBranch = " \t ",
            };

            // Act
            var actual = GetRequest(target);

            // Assert
            Assert.Null(actual.AssemblyName);
            Assert.Empty(actual.BuildOutputFolders);
            Assert.Null(actual.Copyright);
            Assert.Null(actual.Description);
            Assert.Null(actual.IconUrl);
            Assert.Null(actual.LicenseUrl);
            Assert.Null(actual.MinClientVersion);
            Assert.Null(actual.NuspecOutputPath);
            Assert.Null(actual.PackageId);
            Assert.Null(actual.PackageOutputPath);
            Assert.Null(actual.PackageVersion);
            Assert.Null(actual.ProjectUrl);
            Assert.Null(actual.ReleaseNotes);
            Assert.Null(actual.RepositoryType);
            Assert.Null(actual.RepositoryUrl);
            Assert.Null(actual.RepositoryCommit);
            Assert.Null(actual.RepositoryBranch);
        }

        [Fact]
        public void PackTask_CleanUpArraysOfStrings()
        {
            // Arrange
            var target = new PackTask
            {
                Authors = new[] { "", "  ", " Authors \t ", null },
                PackageTypes = new[] { "", "  ", " PackageTypes \t ", null },
                Tags = new[] { "", "  ", " Tags \t ", null },
                TargetFrameworks = new[] { "", "  ", " TargetFrameworks \t ", null }
            };

            // Act
            var actual = GetRequest(target);

            // Assert
            Assert.Equal(new[] { "Authors" }, actual.Authors);
            Assert.Equal(new[] { "PackageTypes" }, actual.PackageTypes);
            Assert.Equal(new[] { "Tags" }, actual.Tags);
            Assert.Equal(new[] { "TargetFrameworks" }, actual.TargetFrameworks);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PackTask_CopiesBooleanValues(bool value)
        {
            // Arrange
            var target = new PackTask
            {
                ContinuePackingAfterGeneratingNuspec = value,
                IncludeBuildOutput = value,
                IncludeSource = value,
                IncludeSymbols = value,
                InstallPackageToOutputPath = value,
                IsTool = value,
                NoPackageAnalysis = value,
                OutputFileNamesWithoutVersion = value,
                RequireLicenseAcceptance = value,
                DevelopmentDependency = value,
                Serviceable = value
            };

            // Act
            var actual = GetRequest(target);

            // Assert
            Assert.Equal(value, actual.ContinuePackingAfterGeneratingNuspec);
            Assert.Equal(value, actual.IncludeBuildOutput);
            Assert.Equal(value, actual.IncludeSource);
            Assert.Equal(value, actual.IncludeSymbols);
            Assert.Equal(value, actual.InstallPackageToOutputPath);
            Assert.Equal(value, actual.IsTool);
            Assert.Equal(value, actual.NoPackageAnalysis);
            Assert.Equal(value, actual.OutputFileNamesWithoutVersion);
            Assert.Equal(value, actual.RequireLicenseAcceptance);
            Assert.Equal(value, actual.DevelopmentDependency);
            Assert.Equal(value, actual.Serviceable);
        }

        [Fact]
        public void PackTask_WrapsTaskItems()
        {
            // Arrange
            var target = new PackTask
            {
                FrameworkAssemblyReferences = new[] { null, new Mock<ITaskItem>().Object },
                PackageFiles = new[] { null, new Mock<ITaskItem>().Object },
                PackageFilesToExclude = new[] { null, new Mock<ITaskItem>().Object },
                PackItem = new Mock<ITaskItem>().Object,
                SourceFiles = new[] { null, new Mock<ITaskItem>().Object }
            };

            // Act
            var actual = GetRequest(target);

            // Assert
            Assert.Equal(1, actual.FrameworkAssemblyReferences.OfType<MSBuildTaskItem>().Count());
            Assert.Equal(1, actual.PackageFiles.OfType<MSBuildTaskItem>().Count());
            Assert.Equal(1, actual.PackageFilesToExclude.OfType<MSBuildTaskItem>().Count());
            Assert.NotNull(actual.PackItem);
            Assert.Equal(1, actual.SourceFiles.OfType<MSBuildTaskItem>().Count());
        }

        [Fact]
        public void PackTask_ConvertsNullArraysToEmptyArrays()
        {
            // Arrange
            var target = new PackTask
            {
                FrameworkAssemblyReferences = null,
                Authors = null,
                PackageFiles = null,
                PackageFilesToExclude = null,
                PackageTypes = null,
                SourceFiles = null,
                Tags = null,
                TargetFrameworks = null,
                BuildOutputInPackage = null,
                TargetPathsToSymbols = null
            };

            // Act
            var actual = GetRequest(target);

            // Assert
            Assert.Equal(0, actual.FrameworkAssemblyReferences.Length);
            Assert.Equal(0, actual.Authors.Length);
            Assert.Equal(0, actual.PackageFiles.Length);
            Assert.Equal(0, actual.PackageFilesToExclude.Length);
            Assert.Equal(0, actual.PackageTypes.Length);
            Assert.Equal(0, actual.SourceFiles.Length);
            Assert.Equal(0, actual.Tags.Length);
            Assert.Equal(0, actual.TargetFrameworks.Length);
            Assert.Equal(0, actual.BuildOutputInPackage.Length);
            Assert.Equal(0, actual.TargetPathsToSymbols.Length);
        }

        [Fact]
        public void PackTask_MapsAllProperties()
        {
            // Arrange
            var target = new PackTask
            {
                AssemblyName = "AssemblyName",
                FrameworkAssemblyReferences = Array.Empty<ITaskItem>(),
                Authors = Array.Empty<string>(),
                AllowedOutputExtensionsInPackageBuildOutputFolder = Array.Empty<string>(),
                AllowedOutputExtensionsInSymbolsPackageBuildOutputFolder = Array.Empty<string>(),
                BuildOutputFolders = new string[] { "lib", "embed" },
                ContentTargetFolders = new string[] { "ContentTargetFolders" },
                ContinuePackingAfterGeneratingNuspec = true,
                Copyright = "Copyright",
                Description = "Description",
                DevelopmentDependency = true,
                IconUrl = "IconUrl",
                IncludeBuildOutput = true,
                IncludeSource = true,
                IncludeSymbols = true,
                IsTool = true,
                LicenseUrl = "LicenseUrl",
                MinClientVersion = "MinClientVersion",
                NoPackageAnalysis = true,
                NuspecOutputPath = "NuspecOutputPath",
                NuspecProperties = Array.Empty<string>(),
                PackItem = null, // This is asserted by other tests. It does not serialize well.
                PackageFiles = Array.Empty<ITaskItem>(),
                PackageFilesToExclude = Array.Empty<ITaskItem>(),
                PackageId = "PackageId",
                PackageOutputPath = "PackageOutputPath",
                PackageTypes = Array.Empty<string>(),
                PackageVersion = "PackageVersion",
                ProjectReferencesWithVersions = Array.Empty<ITaskItem>(),
                ProjectUrl = "ProjectUrl",
                ReleaseNotes = "ReleaseNotes",
                RepositoryType = "RepositoryType",
                RepositoryUrl = "RepositoryUrl",
                RepositoryCommit = "RepositoryCommit",
                RepositoryBranch = "RepositoryBranch",
                RequireLicenseAcceptance = true,
                Serviceable = true,
                SourceFiles = Array.Empty<ITaskItem>(),
                Tags = Array.Empty<string>(),
                TargetFrameworks = Array.Empty<string>(),
                BuildOutputInPackage = Array.Empty<ITaskItem>(),
                TargetPathsToSymbols = Array.Empty<ITaskItem>(),
                FrameworksWithSuppressedDependencies = Array.Empty<ITaskItem>(),
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new OrderedContractResolver(),
                Formatting = Formatting.Indented
            };

            var jsonModelBefore = JObject.FromObject(target, JsonSerializer.Create(settings));

            // Exclude properties on the build task but not used for the pack task request.
            var excludedBuildEngineProperty = new List<string>(jsonModelBefore.Properties().
                Where(p => p.Name.StartsWith("BuildEngine", StringComparison.OrdinalIgnoreCase)).
                Select(p => p.Name));

            var excludedOtherProperties = new[]
            {
                "HostObject",
                "Log",
                "PackTaskLogic",
            };

            foreach (var property in excludedBuildEngineProperty)
            {
                jsonModelBefore.Remove(property);
            }

            foreach (var property in excludedOtherProperties)
            {
                jsonModelBefore.Remove(property);
            }

            var expectedJson = JsonConvert.SerializeObject(jsonModelBefore, settings);

            // Act
            var actual = GetRequest(target);

            // Assert
            var actualJson = JsonConvert.SerializeObject(actual, settings);
            Assert.Equal(expectedJson, actualJson);
        }

        private IPackTaskRequest<IMSBuildItem> GetRequest(PackTask target)
        {
            // Arrange
            IPackTaskRequest<IMSBuildItem> request = null;

            var logic = new Mock<IPackTaskLogic>();
            logic
                .Setup(x => x.GetPackArgs(It.IsAny<IPackTaskRequest<IMSBuildItem>>()))
                .Callback<IPackTaskRequest<IMSBuildItem>>(r => request = r);

            target.PackTaskLogic = logic.Object;

            // Act
            var result = target.Execute();

            // Assert
            return request;
        }

        /// <summary>
        /// Source: http://stackoverflow.com/a/11309106
        /// </summary>
        private class OrderedContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                return base.CreateProperties(type, memberSerialization)
                    .OrderBy(p => p.PropertyName)
                    .ToList();
            }
        }


        public static IEnumerable<object[]> PackageFileNameTestCases => PackageFileNameTestCase.TestCases;

        // This unit test verifies that GetPackOutputItemsTask outputs the expected file name.
        [Theory]
        [MemberData(nameof(PackageFileNameTestCases))]
        public void PackTask_EnsureFileNames(PackageFileNameTestCase testCase)
        {
            using var testDirectory = TestDirectory.Create();
            string outputDir = Path.Combine(testDirectory, "output");
            Directory.CreateDirectory(outputDir);

            string objDir = Path.Combine(testDirectory, "obj");
            Directory.CreateDirectory(objDir);

            if (testCase.IncludeSymbols)
            {
                //needs .pdb file  (see PackCommandRunner.BuildSymbolsPackage)
                string binDir = Path.Combine(testDirectory, "bin");
                Directory.CreateDirectory(binDir);
                File.WriteAllBytes(Path.Combine(binDir, "dummy.pdb"), new byte[0]);
            }

            // Create nuspec when the test scenario uses one.
            PackageFileNameTestsCommon.CreateNuspecFile(testCase, testDirectory);

            // Create project.assets.json
            var path = string.Join(".", typeof(PackTaskLogicTests).Namespace, "compiler.resources", "json.assets.project");
            using (var mstream = GetType().Assembly.GetManifestResourceStream(path))
            {
                Assert.NotNull(mstream);
                using var reader = new StreamReader(mstream);
                var contents = reader.ReadToEnd();
                File.WriteAllText(Path.Combine(objDir, "project.assets.json"), contents);
            }

            // dummy BuildEngine
            System.Text.StringBuilder logError = new System.Text.StringBuilder();
            System.Text.StringBuilder logWarning = new System.Text.StringBuilder();
            var mockEngine = new Mock<IBuildEngine>();
            mockEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>()))
                .Callback<BuildErrorEventArgs>((e) => { logError.Append(e.Message); });
            mockEngine.Setup(x => x.LogWarningEvent(It.IsAny<BuildWarningEventArgs>()))
                .Callback<BuildWarningEventArgs>((e) => { logWarning.Append(e.Message); });

            var nuspecProps = new List<string>();
            if (!string.IsNullOrWhiteSpace(testCase.VersionNuspecProperties))
            {
                nuspecProps.Add("version=" + testCase.VersionNuspecProperties);
            }
            if (!string.IsNullOrWhiteSpace(testCase.IdNuspecProperties))
            {
                nuspecProps.Add("id=" + testCase.IdNuspecProperties);
            }

            var packTask = new PackTask()
            {
                PackItem = FileTaskItem.FromPath(Path.Combine(testDirectory, PackageFileNameTestsCommon.FILENAME_PROJECT_FILE)),
                RestoreOutputPath = Path.Combine(testDirectory, "obj"),

                Authors = ["Nuget Team"],
                Description = "description",
                BuildOutputInPackage = [],
                ContinuePackingAfterGeneratingNuspec = true,
                NuspecBasePath = testDirectory,
                // Dummy for Logger (see Microsoft.Build.Utilities.TaskLoggingHelper.LogWarning)
                BuildEngine = mockEngine.Object,
                PackageId = PackageFileNameTestCase.IdProjProp,
                PackageVersion = testCase.VersionProjProp,
                PackageOutputPath = outputDir,
                NuspecOutputPath = outputDir,
                NuspecFile = (testCase.UseNuspecFile ? Path.Combine(testDirectory, PackageFileNameTestsCommon.FILENAME_NUSPEC_FILE) : null),
                IncludeSymbols = testCase.IncludeSymbols,
                SymbolPackageFormat = PackageFileNameTestsCommon.GetSymbolPackageFormatText(testCase.SymbolPackageFormat),
                OutputFileNamesWithoutVersion = testCase.OutputFileNamesWithoutVersion,
                NuspecProperties = nuspecProps.Count > 0 ? nuspecProps.ToArray() : null
            };

            Assert.True(packTask.Execute(), "PackTask.Execute Fail\r\n" + logError.ToString());

            _testOutputHelper.WriteLine(logWarning.ToString());
            _testOutputHelper.WriteLine(logError.ToString());

            // get generated files
            string[] outputExtensions = GetOutputExtensions(testCase.IncludeSymbols, testCase.SymbolPackageFormat);
            var nupkgGeneratedFiles = outputExtensions
                    .SelectMany(outputExtension => Directory.GetFiles(testDirectory, $"*{outputExtension}", SearchOption.AllDirectories))
                    .Where(line => !line.StartsWith(objDir))
                    .Distinct().ToArray();

            nupkgGeneratedFiles.Length.Should().Be(testCase.OutputNupkgNames.Length, because: "Output nupkg names must match the number of generated files.");

            // compare generated and testCase
            foreach (string outputNupkgName in testCase.OutputNupkgNames)
            {
                var matchCountInFileSystem = nupkgGeneratedFiles.Count(file => string.Equals(outputNupkgName, Path.GetFileName(file), StringComparison.OrdinalIgnoreCase));
                Assert.True(matchCountInFileSystem == 1, $"{outputNupkgName} is not found in filesystem. [{string.Join(" , ", nupkgGeneratedFiles.Select(Path.GetFileName))}]");
            }

            // It is very important that the input here is exactly the same as what PackTask is configured with.
            var getPackageOutputTask = new GetPackOutputItemsTask()
            {
                PackageId = packTask.PackageId,
                PackageVersion = packTask.PackageVersion,
                PackageOutputPath = packTask.PackageOutputPath,
                NuspecOutputPath = packTask.NuspecOutputPath,
                NuspecFile = packTask.NuspecFile,
                NuspecProperties = packTask.NuspecProperties,
                IncludeSource = packTask.IncludeSource,
                IncludeSymbols = packTask.IncludeSymbols,
                SymbolPackageFormat = packTask.SymbolPackageFormat,
                OutputFileNamesWithoutVersion = packTask.OutputFileNamesWithoutVersion,
            };

            Assert.True(getPackageOutputTask.Execute(), "GetPackOutputItemsTask.Execute Fail\r\n" + logError.ToString());

            ITaskItem[] outputPaths = getPackageOutputTask.OutputPackItems;
            foreach (var outputPath in outputPaths)
            {
                if (outputPath.GetMetadata("Extension") == ".nuspec" && testCase.UseNuspecFile)
                {
                    continue;
                }
                Assert.True(File.Exists(outputPath.GetMetadata("FullPath")), $"{outputPath} is not found in filesystem");
            }
        }

        static string[] GetOutputExtensions(bool includeSymbols, SymbolPackageFormat symbolPackageFormat)
        {
            if (includeSymbols)
            {
                return symbolPackageFormat switch
                {
                    SymbolPackageFormat.Snupkg => [".nupkg", ".snupkg"],
                    SymbolPackageFormat.SymbolsNupkg => [".nupkg", ".symbols.nupkg"],
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
            else
            {
                return [".nupkg"];
            }
        }

        class FileTaskItem : ITaskItem
        {
            public static FileTaskItem FromPath(string path)
            {
                var fullpath = Path.GetFullPath(path);

                FileTaskItem item = new FileTaskItem(fullpath);
                item.SetMetadata("RootDir", Path.GetDirectoryName(fullpath) ?? "");
                item.SetMetadata("Directory", Path.GetDirectoryName(fullpath) ?? "");
                item.SetMetadata("FileName", Path.GetFileName(fullpath));
                item.SetMetadata("Extension", Path.GetExtension(fullpath));
                item.SetMetadata("FullPath", fullpath);
                return item;
            }


            private Dictionary<string, string> _dic = new Dictionary<string, string>();

            public FileTaskItem(string itemSpec)
            {
                ItemSpec = itemSpec;
            }

            public string GetMetadata(string metadataName) => _dic[metadataName];
            public void SetMetadata(string metadataName, string metadataValue) => _dic[metadataName] = metadataValue;
            public void RemoveMetadata(string metadataName) => _dic.Remove(metadataName);
            public void CopyMetadataTo(ITaskItem destinationItem) => throw new NotSupportedException();
            public System.Collections.IDictionary CloneCustomMetadata() => throw new NotSupportedException();
            public string ItemSpec { get; set; } = "";
            public System.Collections.ICollection MetadataNames => _dic.Keys;
            public int MetadataCount => _dic.Count;
        }
    }
}
