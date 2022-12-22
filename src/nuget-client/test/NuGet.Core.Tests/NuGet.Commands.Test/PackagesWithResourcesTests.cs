// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class PackagesWithResourcesTests
    {
        [Fact]
        public async Task Resources_AppearInLockFileWithAppropriateLocaleValue()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var repository = Path.Combine(workingDir, "repository");
                Directory.CreateDirectory(repository);
                var projectDir = Path.Combine(workingDir, "project");
                Directory.CreateDirectory(projectDir);
                var packagesDir = Path.Combine(workingDir, "packages");
                Directory.CreateDirectory(packagesDir);

                var file = new FileInfo(Path.Combine(repository, "packageA.1.0.0.nupkg"));

                using (var zip = new ZipArchive(File.Create(file.FullName), ZipArchiveMode.Create))
                {
                    zip.AddEntry("lib/net46/MyPackage.dll", new byte[] { 0 });
                    zip.AddEntry("lib/net46/en-US/MyPackage.resources.dll", new byte[] { 0 });
                    zip.AddEntry("lib/net46/en-CA/MyPackage.resources.dll", new byte[] { 0 });
                    zip.AddEntry("lib/net46/fr-CA/MyPackage.resources.dll", new byte[] { 0 });

                    zip.AddEntry("packageA.nuspec", @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package xmlns=""http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd"">
                        <metadata>
                            <id>packageA</id>
                            <version>1.0.0</version>
                            <title />
                            <contentFiles>
                                <files include=""**/*.*"" copyToOutput=""TRUE"" flatten=""true"" />
                            </contentFiles>
                        </metadata>
                        </package>", Encoding.UTF8);
                }

                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(repository));

                var configJson = JObject.Parse(@"{
                    ""dependencies"": {
                    ""packageA"": ""1.0.0""
                    },
                    ""frameworks"": {
                    ""_FRAMEWORK_"": {}
                    }
                }".Replace("_FRAMEWORK_", framework));

                var specPath = Path.Combine(projectDir, "TestProject", "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", specPath).EnsureProjectJsonRestoreMetadata();

                var request = new TestRestoreRequest(spec, sources, packagesDir, logger);
                request.LockFilePath = Path.Combine(projectDir, "project.lock.json");

                var command = new RestoreCommand(request);

                // Act
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), null);
                var lib = target.Libraries.Single();
                var resourceAssemblies = lib.ResourceAssemblies;

                AssertResourceAssembly(resourceAssemblies, "lib/net46/en-US/MyPackage.resources.dll", "en-US");
                AssertResourceAssembly(resourceAssemblies, "lib/net46/en-CA/MyPackage.resources.dll", "en-CA");
                AssertResourceAssembly(resourceAssemblies, "lib/net46/fr-CA/MyPackage.resources.dll", "fr-CA");
            }
        }

        private void AssertResourceAssembly(IList<LockFileItem> items, string path, string locale)
        {
            var item = items.Single(i => i.Path.Equals(path));
            if (locale == null)
            {
                Assert.False(item.Properties.ContainsKey("locale"));
            }
            else
            {
                Assert.Equal(locale, item.Properties["locale"]);
            }
        }
    }
}
