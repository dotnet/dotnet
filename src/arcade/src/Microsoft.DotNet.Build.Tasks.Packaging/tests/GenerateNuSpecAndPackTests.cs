// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Microsoft.DotNet.Build.Tasks.Packaging.Tests
{
    public class GenerateNuSpecAndPackTests
    {
        private Log _log;
        private TestBuildEngine _engine;

        public GenerateNuSpecAndPackTests(ITestOutputHelper output)
        {
            _log = new Log(output);
            _engine = new TestBuildEngine(_log);
        }
        
        [Fact]
        public void TestSimplePackage()
        {
            string nuspec = $"{nameof(TestSimplePackage)}.nuspec";

            if (File.Exists(nuspec))
            {
                File.Delete(nuspec);
            }

            var generateNuSpec = CreateGenerateNuSpecTask(
                nameof(TestSimplePackage), 
                nuspec, 
                dependencies: new[] { CreateDependency("someDependency", "0.0.0-test") });

            Execute(generateNuSpec).Should().BeTrue();

            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);

            File.Exists(nuspec).Should().BeTrue();

            var nuspecs = new[] { new TaskItem(nuspec) };

            var nuGetPack = CreateNuGetPackTask(nuspecs, Directory.GetCurrentDirectory());

            Execute(nuGetPack).Should().BeTrue();
            _log.ErrorsLogged.Should().Be(0);
            _log.WarningsLogged.Should().Be(0);
            File.Exists($"{generateNuSpec.Id}.{generateNuSpec.Version}.nupkg").Should().BeTrue();
        }

        private bool Execute(ITask task)
        {
            task.BuildEngine = _engine;
            _log.Reset();
            return task.Execute();
        }

        private ITaskItem CreateDependency(string id, string version, string targetFramework = null)
        {
            var item = new TaskItem(id);
            item.SetMetadata("Version", version);
            if (!String.IsNullOrEmpty(targetFramework))
            {
                item.SetMetadata("TargetFramework", targetFramework);
            }
            return item;
        }

        private static NuGetPack CreateNuGetPackTask(
            ITaskItem[] nuspecs,
            string outputDirectory,
            ITaskItem[] additionalLibPackageExcludes = null,
            ITaskItem[] additionalSymbolPackageExcludes = null,
            string baseDirectory = null,
            bool createPackedPackage = false,
            bool createSymbolPackage = false,
            bool excludeEmptyDirectories = true,
            bool includeSymbolsInPackage = false,
            ITaskItem[] nuspecProperties = null,
            string packageVersion = null,
            string packedPackageNamePrefix = null,
            string symbolPackageOutputDirectory = null)
        {
            return new NuGetPack()
            {
                AdditionalLibPackageExcludes = additionalLibPackageExcludes ?? Array.Empty<ITaskItem>(),
                AdditionalSymbolPackageExcludes = additionalSymbolPackageExcludes ?? Array.Empty<ITaskItem>(),
                BaseDirectory = baseDirectory,
                CreatePackedPackage = createPackedPackage,
                CreateSymbolPackage = createSymbolPackage,
                ExcludeEmptyDirectories = excludeEmptyDirectories,
                IncludeSymbolsInPackage = includeSymbolsInPackage,
                NuspecProperties = nuspecProperties ?? Array.Empty<ITaskItem>(),
                Nuspecs = nuspecs,
                OutputDirectory = outputDirectory,
                PackageVersion = packageVersion,
                PackedPackageNamePrefix = packedPackageNamePrefix,
                SymbolPackageOutputDirectory = symbolPackageOutputDirectory
            };
        }

        private static GenerateNuSpec CreateGenerateNuSpecTask(
            string id,
            string outputFileName,
            string authors = "author1;author2",
            string copyright = "(c) fabrikam corp",
            string description = "description",
            ITaskItem[] dependencies = null,
            bool developmentDependency = false,
            ITaskItem[] files = null,
            ITaskItem[] frameworkReferences = null,
            string iconUrl = "http://fabrikam.com/myicon",
            string inputFileName = null,
            string language = null,
            string licenseUrl = null,
            string minClientVersion = "3.0",
            string owners = "owner1;owner2",
            string licenseExpression = "MIT",
            string[] packageTypes = null,
            string projectUrl = "http://fabrikam.com",
            ITaskItem[] references = null,
            string releaseNotes = null,
            string repositoryBranch = "master",
            string repositoryCommit = "8675309",
            string repositoryType = "git",
            string repositoryUrl = "http://github.com/microsoft/fabrikam/",
            bool requireLicenseAcceptance = true,
            bool serviceable = true,
            string summary = "summary",
            string tags = "tag1;tag2",
            string title = "title",
            string version = "0.0.0-test")
        {

            return new GenerateNuSpec()
            {
                Authors = authors,
                Copyright = copyright,
                Dependencies = dependencies ?? Array.Empty<ITaskItem>(),
                Description = description,
                DevelopmentDependency = developmentDependency,
                Files = files ?? Array.Empty<ITaskItem>(),
                FrameworkReferences = frameworkReferences ?? Array.Empty<ITaskItem>(),
                IconUrl = iconUrl,
                Id = id,
                InputFileName = inputFileName,
                Language = language,
                LicenseUrl = licenseUrl,
                MinClientVersion = minClientVersion,
                OutputFileName = outputFileName,
                Owners = owners,
                PackageLicenseExpression = licenseExpression,
                PackageTypes = packageTypes ?? Array.Empty<string>(),
                ProjectUrl = projectUrl,
                References = references ?? Array.Empty<ITaskItem>(),
                ReleaseNotes = releaseNotes,
                RepositoryBranch = repositoryBranch,
                RepositoryCommit = repositoryCommit,
                RepositoryType = repositoryType,
                RepositoryUrl = repositoryUrl,
                RequireLicenseAcceptance = requireLicenseAcceptance,
                Serviceable = serviceable,
                Summary = summary,
                Tags = tags,
                Title = title,
                Version = version
            };
        }
    }
}
