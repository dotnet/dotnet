// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NetworkCallCountTest
    {
        [SkipMono(Skip = "https://github.com/NuGet/Home/issues/8594")]
        public void NetworkCallCount_RestoreLargePackagesConfigWithMultipleSourcesWithAllMissingPackages()
        {
            // Arrange
            var testCount = 100;

            using (var server2 = new MockServer())
            using (var server1 = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath1 = Path.Combine(workingPath, "repo");
                var repositoryPath2 = Path.Combine(workingPath, "repo2");
                var repositoryPath3 = Path.Combine(workingPath, "repo3");
                var allRepo = Path.Combine(workingPath, "allRepo");
                var packagesFolderPath = pathContext.PackagesV2;

                Directory.CreateDirectory(allRepo);
                Directory.CreateDirectory(repositoryPath1);
                Directory.CreateDirectory(repositoryPath2);
                Directory.CreateDirectory(repositoryPath3);

                // Prepare for packages.config file
                var packagesConfigPath = Path.Combine(pathContext.SolutionRoot, "packages.config");

                var doc = new XDocument();
                var root = new XElement(XName.Get("packages"));
                doc.Add(root);

                var expectedPackages = new HashSet<PackageIdentity>();

                for (int i = 0; i < testCount; i++)
                {
                    var id = $"package{Guid.NewGuid()}_{i}";
                    var version = $"1.0.{i}";

                    var entry = new XElement(XName.Get("package"));
                    entry.Add(new XAttribute(XName.Get("id"), id));
                    entry.Add(new XAttribute(XName.Get("version"), version));

                    root.Add(entry);
                }

                Util.CreateFile(pathContext.SolutionRoot, "packages.config", doc.ToString());

                // step1: Server setup
                var indexJson = Util.CreateIndexJson();
                var indexJson2 = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server1);
                Util.AddRegistrationResource(indexJson, server1);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                Util.AddFlatContainerResource(indexJson2, server2);
                Util.AddRegistrationResource(indexJson2, server2);
                var hitsByUrl2 = new ConcurrentDictionary<string, int>();

                server1.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server1, indexJson, repositoryPath1);
                });

                server2.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl2, server2, indexJson2, repositoryPath2);
                });

                server1.Start();
                server2.Start();

                // step2: Add sources into NuGet.Config file
                var sources = new List<string>() { server2.Uri + "nuget", server1.Uri + "index.json", repositoryPath3 };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                // Prepare for NuGet.exe command
                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    packagesConfigPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true,
                    timeOutInMilliseconds: (int)TimeSpan.FromMinutes(3).TotalMilliseconds);

                var allPackages = LocalFolderUtility.GetPackagesV2(packagesFolderPath, NullLogger.Instance);

                // Assert
                Assert.True(0 != r.ExitCode, r.Output + " " + r.Errors);

                Assert.Equal(1, hitsByUrl["/index.json"]);
                Assert.Equal(100, hitsByUrl2.Keys.Count(s => s.StartsWith("/nuget/Packages")));

                // The "/Packages" endpoint falls back to "/FindPackagesById" if no package is found.
                Assert.Equal(100, hitsByUrl2.Keys.Count(s => s.StartsWith("/nuget/FindPackagesById")));

                Assert.Equal(0, allPackages.Count());
            }
        }

        [SkipMono(Skip = "https://github.com/NuGet/Home/issues/7878")]
        public void NetworkCallCount_RestoreLargePackagesConfigWithMultipleSourcesWithPartialMissingPackages()
        {
            // Arrange
            var testCount = 100;

            using (var server2 = new MockServer())
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "repo");
                var repositoryPath2 = Path.Combine(workingPath, "repo2");
                var repositoryPath3 = Path.Combine(workingPath, "repo3");
                var allRepo = Path.Combine(workingPath, "allRepo");
                var packagesFolderPath = pathContext.PackagesV2;


                Directory.CreateDirectory(allRepo);
                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(repositoryPath2);
                Directory.CreateDirectory(repositoryPath3);

                // Prepare for packages.config file
                var packagesConfigPath = Path.Combine(pathContext.SolutionRoot, "packages.config");

                var doc = new XDocument();
                var root = new XElement(XName.Get("packages"));
                doc.Add(root);

                var expectedPackages = new HashSet<PackageIdentity>();

                for (int i = 0; i < testCount; i++)
                {
                    var id = $"package{Guid.NewGuid()}_{i}";
                    var version = $"1.0.{i}";

                    var entry = new XElement(XName.Get("package"));
                    entry.Add(new XAttribute(XName.Get("id"), id));
                    entry.Add(new XAttribute(XName.Get("version"), version));

                    if (i % 3 == 0)
                    {
                        // Missing
                    }
                    else if (i % 25 == 0)
                    {
                        Util.CreatePackage(repositoryPath3, id, version);
                    }
                    else if (i % 10 == 0)
                    {
                        Util.CreatePackage(repositoryPath, id, version);
                        Util.CreatePackage(repositoryPath2, id, version);
                    }
                    else
                    {
                        Util.CreatePackage(repositoryPath, id, version);
                    }

                    if (i % 3 != 0)
                    {
                        // Add to the all repo also if it is not missing
                        Util.CreatePackage(allRepo, id, version);
                        expectedPackages.Add(new PackageIdentity(id, new NuGetVersion(version)));
                    }

                    root.Add(entry);
                }

                Util.CreateFile(pathContext.SolutionRoot, "packages.config", doc.ToString());

                // Step1: Server setup
                var indexJson = Util.CreateIndexJson();
                var indexJson2 = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                Util.AddFlatContainerResource(indexJson2, server2);
                Util.AddRegistrationResource(indexJson2, server2);
                var hitsByUrl2 = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server2.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl2, server2, indexJson2, repositoryPath2);
                });

                server.Start();
                server2.Start();

                // step2: Add sources into NuGet.Config file
                var sources = new List<string>() { server2.Uri + "nuget", server.Uri + "index.json", repositoryPath3 };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                // Prepare for NuGet.exe command
                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    packagesConfigPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true,
                    timeOutInMilliseconds: (int)TimeSpan.FromMinutes(3).TotalMilliseconds);

                var allPackages = LocalFolderUtility.GetPackagesV2(packagesFolderPath, NullLogger.Instance);

                // Assert
                Assert.True(0 != r.ExitCode, r.Output + " " + r.Errors);

                Assert.Equal(1, hitsByUrl["/index.json"]);

                Assert.Equal(expectedPackages.Count, allPackages.Count());

                foreach (var package in expectedPackages)
                {
                    Assert.True(allPackages.Any(p => p.Identity.Id == package.Id
                        && p.Identity.Version.ToNormalizedString() == package.Version.ToNormalizedString()));
                }
            }
        }

        [SkipMono(Skip = "https://github.com/NuGet/Home/issues/8594")]
        public void NetworkCallCount_RestoreLargePackagesConfigWithMultipleSourcesMainlyV3()
        {
            // Arrange
            var testCount = 100;

            using (var server2 = new MockServer())
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "repo");
                var repositoryPath2 = Path.Combine(workingPath, "repo2");
                var repositoryPath3 = Path.Combine(workingPath, "repo3");
                var allRepo = Path.Combine(workingPath, "allRepo");
                var packagesFolderPath = pathContext.PackagesV2;

                Directory.CreateDirectory(workingPath);
                Directory.CreateDirectory(allRepo);
                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(repositoryPath2);
                Directory.CreateDirectory(repositoryPath3);

                // Prepare for packages.config file
                var packagesConfigPath = Path.Combine(pathContext.SolutionRoot, "packages.config");

                var doc = new XDocument();
                var root = new XElement(XName.Get("packages"));
                doc.Add(root);

                var expectedPackages = new HashSet<PackageIdentity>();

                for (int i = 0; i < testCount; i++)
                {
                    var id = $"package{i}";
                    var version = $"1.0.{i}";

                    var entry = new XElement(XName.Get("package"));
                    entry.Add(new XAttribute(XName.Get("id"), id));
                    entry.Add(new XAttribute(XName.Get("version"), version));

                    if (i % 25 == 0)
                    {
                        Util.CreatePackage(repositoryPath3, id, version);
                    }
                    else if (i % 10 == 0)
                    {
                        Util.CreatePackage(repositoryPath, id, version);
                        Util.CreatePackage(repositoryPath2, id, version);
                    }
                    else
                    {
                        Util.CreatePackage(repositoryPath, id, version);
                    }

                    // Add to the all repo also
                    Util.CreatePackage(allRepo, id, version);

                    root.Add(entry);

                    expectedPackages.Add(new PackageIdentity(id, new NuGetVersion(version)));
                }

                Util.CreateFile(pathContext.SolutionRoot, "packages.config", doc.ToString());

                // Step1: Server setup
                var indexJson = Util.CreateIndexJson();
                var indexJson2 = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                Util.AddFlatContainerResource(indexJson2, server2);
                Util.AddRegistrationResource(indexJson2, server2);
                var hitsByUrl2 = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server2.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl2, server2, indexJson2, repositoryPath2);
                });

                server.Start();
                server2.Start();

                // step2: Add sources into NuGet.Config file
                var sources = new List<string>() { server2.Uri + "nuget", server.Uri + "index.json", repositoryPath3 };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                // Prepare for NuGet.exe command
                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    packagesConfigPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true,
                    timeOutInMilliseconds: (int)TimeSpan.FromMinutes(3).TotalMilliseconds);

                var allPackages = LocalFolderUtility.GetPackagesV2(packagesFolderPath, NullLogger.Instance);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                Assert.Equal(testCount, allPackages.Count());

                foreach (var package in expectedPackages)
                {
                    Assert.True(allPackages.Any(p => p.Identity.Id == package.Id
                        && p.Identity.Version.ToNormalizedString() == package.Version.ToNormalizedString()));
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void NetworkCallCount_RestoreLargePackagesConfigWithMultipleSourcesMainlyV2()
        {
            // Arrange
            var testCount = 100;

            using (var server2 = new MockServer())
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "repo");
                var repositoryPath2 = Path.Combine(workingPath, "repo2");
                var repositoryPath3 = Path.Combine(workingPath, "repo3");
                var allRepo = Path.Combine(workingPath, "allRepo");
                var packagesFolderPath = pathContext.PackagesV2;

                Directory.CreateDirectory(allRepo);
                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(repositoryPath2);
                Directory.CreateDirectory(repositoryPath3);

                // Prepare for packages.config file
                var packagesConfigPath = Path.Combine(pathContext.SolutionRoot, "packages.config");

                var doc = new XDocument();
                var root = new XElement(XName.Get("packages"));
                doc.Add(root);

                var expectedPackages = new HashSet<PackageIdentity>();

                for (int i = 0; i < testCount; i++)
                {
                    var id = $"package{i}";
                    var version = $"1.0.{i}";

                    var entry = new XElement(XName.Get("package"));
                    entry.Add(new XAttribute(XName.Get("id"), id));
                    entry.Add(new XAttribute(XName.Get("version"), version));

                    if (i % 25 == 0)
                    {
                        Util.CreatePackage(repositoryPath3, id, version);
                    }
                    else if (i % 10 == 0)
                    {
                        Util.CreatePackage(repositoryPath, id, version);
                        Util.CreatePackage(repositoryPath2, id, version);
                    }
                    else
                    {
                        Util.CreatePackage(repositoryPath, id, version);
                    }

                    // Add to the all repo also
                    Util.CreatePackage(allRepo, id, version);

                    root.Add(entry);

                    expectedPackages.Add(new PackageIdentity(id, new NuGetVersion(version)));
                }

                Util.CreateFile(pathContext.SolutionRoot, "packages.config", doc.ToString());

                // Step1: Server setup
                var indexJson = Util.CreateIndexJson();
                var indexJson2 = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                Util.AddFlatContainerResource(indexJson2, server2);
                Util.AddRegistrationResource(indexJson2, server2);
                var hitsByUrl2 = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server2.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl2, server2, indexJson2, repositoryPath2);
                });

                server.Start();
                server2.Start();

                // step2: Add sources into NuGet.Config file
                var sources = new List<string>() { server2.Uri + "index.json", server.Uri + "nuget", repositoryPath3 };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                // Prepare for NuGet.exe command
                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    packagesConfigPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true,
                    timeOutInMilliseconds: (int)TimeSpan.FromMinutes(3).TotalMilliseconds);

                var allPackages = LocalFolderUtility.GetPackagesV2(packagesFolderPath, NullLogger.Instance);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                Assert.Equal(testCount, allPackages.Count());

                foreach (var package in expectedPackages)
                {
                    Assert.True(allPackages.Any(p => p.Identity.Id == package.Id
                        && p.Identity.Version.ToNormalizedString() == package.Version.ToNormalizedString()));
                }
            }
        }

        [SkipMono(Skip = "https://github.com/NuGet/Home/issues/8594")]
        public void NetworkCallCount_CancelPackageDownloadForV3()
        {
            // Arrange
            using (var server2 = new MockServer())
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                var packagesFolder = pathContext.PackagesV2;
                Directory.CreateDirectory(packagesFolder);
                var globalFolder = pathContext.UserPackagesFolder;

                // Server setup
                var indexJson = Util.CreateIndexJson();
                var indexJson2 = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);

                Util.AddFlatContainerResource(indexJson2, server2);
                Util.AddRegistrationResource(indexJson2, server2);

                var hitsByUrl = new ConcurrentDictionary<string, int>();
                var hitsByUrl2 = new ConcurrentDictionary<string, int>();

                var v2ResetEvent = new ManualResetEventSlim(true);
                var v3ResetEvent = new ManualResetEventSlim(false);

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath, v2ResetEvent, v3ResetEvent);
                });

                server2.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl2, server2, indexJson2, repositoryPath, v2ResetEvent, v3ResetEvent);
                });

                server.Start();
                server2.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "index.json", server2.Uri + "nuget" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    slnPath,
                    "-Verbosity",
                    "detailed"
                };

                var task = Task.Run(() =>
                {
                    // Wait until all packages exist before allowing v2 to return
                    while (Directory.GetDirectories(packagesFolder, "*", SearchOption.TopDirectoryOnly).Count() < 3)
                    {
                        Thread.Sleep(100);
                    }

                    v3ResetEvent.Set();
                });

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                task.Wait();

                var globalFolderCount = Directory.GetDirectories(
                    globalFolder, "*", SearchOption.TopDirectoryOnly)
                    .Count();

                var packagesFolderCount = Directory.GetDirectories(
                    packagesFolder, "*", SearchOption.TopDirectoryOnly)
                    .Count();

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                Assert.Equal(3, packagesFolderCount);   // project.json packages still go here
                Assert.Equal(6, globalFolderCount);
            }
        }

        [SkipMono(Skip = "https://github.com/NuGet/Home/issues/8594")]
        public void NetworkCallCount_CancelPackageDownloadForV2()
        {
            // Arrange
            using (var server2 = new MockServer())
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                var packagesFolder = pathContext.PackagesV2;
                Directory.CreateDirectory(packagesFolder);
                var globalFolder = pathContext.UserPackagesFolder;

                // Server setup
                var indexJson = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();
                var hitsByUrl2 = new ConcurrentDictionary<string, int>();

                var v2ResetEvent = new ManualResetEventSlim(false);
                var v3ResetEvent = new ManualResetEventSlim(true);

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath, v2ResetEvent, v3ResetEvent);
                });

                server2.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl2, server2, indexJson, repositoryPath, v2ResetEvent, v3ResetEvent);
                });

                server.Start();
                server2.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "index.json", server2.Uri + "nuget" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    slnPath,
                    "-Verbosity",
                    "detailed"
                };

                var task = Task.Run(() =>
                {
                    // Wait until all packages exist before allowing v2 to return
                    while (Directory.GetDirectories(packagesFolder, "*", SearchOption.TopDirectoryOnly).Count() < 3)
                    {
                        Thread.Sleep(100);
                    }

                    v2ResetEvent.Set();
                });

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true,
                    timeOutInMilliseconds: int.MaxValue);

                task.Wait();

                var globalFolderCount = Directory.GetDirectories(
                    globalFolder, "*", SearchOption.TopDirectoryOnly)
                    .Count();

                var packagesFolderCount = Directory.GetDirectories(
                    packagesFolder, "*", SearchOption.TopDirectoryOnly)
                    .Count();

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                Assert.Equal(3, packagesFolderCount);
                Assert.Equal(6, globalFolderCount);
            }
        }

        [SkipMono(Skip = "https://github.com/NuGet/Home/issues/8594")]
        public void NetworkCallCount_RestoreSolutionMultipleSourcesV2V3AndLocal()
        {
            // Arrange
            using (var server2 = new MockServer())
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                // Server setup
                var indexJson = Util.CreateIndexJson();
                var indexJson2 = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);

                Util.AddFlatContainerResource(indexJson2, server2);
                Util.AddRegistrationResource(indexJson2, server2);

                var hitsByUrl = new ConcurrentDictionary<string, int>();
                var hitsByUrl2 = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server2.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl2, server2, indexJson2, repositoryPath);
                });

                server.Start();
                server2.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "index.json", server2.Uri + "nuget", repositoryPath };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    slnPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                // Network calls can happen multiple times here with cancelation
                foreach ((var url, var hits) in hitsByUrl)
                {
                    Assert.True(2 >= hits, url + " " + hits);
                }

                foreach ((var url, var hits) in hitsByUrl2)
                {
                    Assert.True(2 >= hits, url + " " + hits);
                }
            }
        }

        [Fact]
        public void NetworkCallCount_InstallVersionFromV2()
        {
            // Arrange
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                var outputPath = Path.Combine(workingPath, "output");
                Directory.CreateDirectory(outputPath);

                Util.CreatePackage(repositoryPath, "packageA", "1.0.0");

                // Server setup
                var indexJson = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "nuget" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "install",
                    "packageA",
                    "-Version",
                    "1.0.0",
                    "-OutputDirectory",
                    outputPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                foreach ((var url, var hits) in hitsByUrl)
                {
                    Assert.True(1 == hits, url);
                }
            }
        }

        [Fact]
        public void NetworkCallCount_InstallVersionFromV3()
        {
            // Arrange
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                var outputPath = Path.Combine(workingPath, "output");
                Directory.CreateDirectory(outputPath);

                Util.CreatePackage(repositoryPath, "packageA", "1.0.0");

                // Server setup
                var indexJson = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "index.json" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "install",
                    "packageA",
                    "-Version",
                    "1.0.0",
                    "-OutputDirectory",
                    outputPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal(1, hitsByUrl["/index.json"]);
                Assert.Equal(1, hitsByUrl["/reg/packagea/index.json"]);

                foreach ((var url, var hits) in hitsByUrl)
                {
                    Assert.True(1 == hits, url);
                }
            }
        }

        [Fact]
        public void NetworkCallCount_InstallLatestFromV2()
        {
            // Arrange
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                var outputPath = Path.Combine(workingPath, "output");
                Directory.CreateDirectory(outputPath);

                Util.CreatePackage(repositoryPath, "packageA", "1.0.0");

                // Server setup
                var indexJson = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "nuget" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "install",
                    "packageA",
                    "-OutputDirectory",
                    outputPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                foreach ((var url, var hits) in hitsByUrl)
                {
                    Assert.True(1 == hits, url);
                }
            }
        }

        [Fact]
        public void NetworkCallCount_InstallLatestFromV3()
        {
            // Arrange
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                var outputPath = Path.Combine(workingPath, "output");
                Directory.CreateDirectory(outputPath);

                Util.CreatePackage(repositoryPath, "packageA", "1.0.0");

                // Server setup
                var indexJson = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "index.json" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "install",
                    "packageA",
                    "-OutputDirectory",
                    outputPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal(1, hitsByUrl["/index.json"]);
                Assert.Equal(1, hitsByUrl["/reg/packagea/index.json"]);

                foreach ((var url, var hits) in hitsByUrl)
                {
                    Assert.True(1 == hits, url);
                }
            }
        }

        [SkipMono(Skip = "https://github.com/NuGet/Home/issues/8594")]
        public void NetworkCallCount_RestoreSolutionMultipleSourcesV2V3()
        {
            // Arrange
            using (var server2 = new MockServer())
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");
                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                // Server setup
                var indexJson = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();
                var hitsByUrl2 = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server2.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl2, server2, indexJson, repositoryPath);
                });

                server.Start();
                server2.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "index.json", server2.Uri + "nuget" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    slnPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                Assert.Equal(1, hitsByUrl["/index.json"]);
            }
        }

        [SkipMono(Skip = "https://github.com/NuGet/Home/issues/8594")]
        public void NetworkCallCount_RestoreSolutionMultipleSourcesTwoV2()
        {
            // Arrange
            using (var server2 = new MockServer())
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                // Server setup
                var indexJson = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();
                var hitsByUrl2 = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server2.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl2, server2, indexJson, repositoryPath);
                });

                server.Start();
                server2.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "nuget", server2.Uri + "nuget" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    slnPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                foreach ((var url, var hits) in hitsByUrl)
                {
                    Assert.True(1 == hits, url + " hits: " + hits);
                }

                foreach ((var url, var hits) in hitsByUrl2)
                {
                    Assert.True(1 == hits, url + " hits: " + hits);
                }
            }
        }

        [SkipMono(Skip = "https://github.com/NuGet/Home/issues/8594")]
        public void NetworkCallCount_RestoreSolutionMultipleSourcesTwoV3()
        {
            // Arrange
            using (var server2 = new MockServer())
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                // Server setup
                var indexJson = Util.CreateIndexJson();
                var indexJson2 = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);

                Util.AddFlatContainerResource(indexJson2, server2);
                Util.AddRegistrationResource(indexJson2, server2);

                var hitsByUrl = new ConcurrentDictionary<string, int>();
                var hitsByUrl2 = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server2.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl2, server2, indexJson2, repositoryPath);
                });

                server.Start();
                server2.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "index.json", server2.Uri + "index.json" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    slnPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                foreach ((var url, var hits) in hitsByUrl)
                {
                    Assert.True(1 == hits, url);
                }

                foreach ((var url, var hits) in hitsByUrl2)
                {
                    Assert.True(1 == hits, url);
                }
            }
        }

        [Fact]
        public void NetworkCallCount_RestoreSolutionV3WithoutFlatContainer()
        {
            // Arrange
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                // Server setup
                var indexJson = Util.CreateIndexJson();

                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "index.json" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    slnPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal(1, hitsByUrl["/index.json"]);

                // PackageE is hit twice, once from packages.config and the other from project.json.
                // The rest should only be hit once.
                foreach ((var url, var hits) in hitsByUrl.Where(s => s.Key != "/reg/packagee/index.json"))
                {
                    Assert.True(1 == hits, url + $" was hit {hits} times instead of 1");
                }
            }
        }

        [Fact]
        public void NetworkCallCount_RestoreSolutionWithPackagesConfigAndProjectJsonV3()
        {
            // Arrange
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                // Server setup
                var indexJson = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "index.json" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    slnPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal(1, hitsByUrl["/index.json"]);

                foreach ((var url, var hits) in hitsByUrl)
                {
                    Assert.True(1 == hits, url);
                }
            }
        }

        [Fact]
        public void NetworkCallCount_RestoreSolutionWithPackagesConfigAndProjectJsonV2()
        {
            // Arrange
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                CreateMixedConfigAndJson(pathContext.SolutionRoot);
                var repositoryPath = Path.Combine(pathContext.SolutionRoot, "repo");

                var slnPath = Path.Combine(pathContext.SolutionRoot, "test.sln");

                // Server setup
                var indexJson = Util.CreateIndexJson();

                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);
                Util.AddFlatContainerResource(indexJson, server);
                var hitsByUrl = new ConcurrentDictionary<string, int>();

                server.Get.Add("/", request =>
                {
                    return ServerHandler(request, hitsByUrl, server, indexJson, repositoryPath);
                });

                server.Start();

                // Add sources into NuGet.Config file
                var sources = new List<string>() { server.Uri + "nuget" };
                var settings = pathContext.Settings;
                SimpleTestSettingsContext.RemoveSource(settings.XML, "source");
                var section = SimpleTestSettingsContext.GetOrAddSection(settings.XML, "packageSources");
                for (int i = 0; i < sources.Count; i++)
                {
                    SimpleTestSettingsContext.AddEntry(section, $"source{i}", sources[i]);
                }
                settings.Save();

                var nugetexe = Util.GetNuGetExePath();
                string[] args = new string[] {
                    "restore",
                    slnPath,
                    "-Verbosity",
                    "detailed",
                    "-DisableParallelProcessing"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                foreach ((var url, var hits) in hitsByUrl)
                {
                    Assert.True(1 == hits, url);
                }
            }
        }

        private void CreateMixedConfigAndJson(string workingPath)
        {
            var repositoryPath = Path.Combine(workingPath, "repo");

            var proj1Dir = Path.Combine(workingPath, "proj1");
            var proj2Dir = Path.Combine(workingPath, "proj2");
            var proj3Dir = Path.Combine(workingPath, "proj3");
            var proj4Dir = Path.Combine(workingPath, "proj4");

            Directory.CreateDirectory(repositoryPath);
            Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));

            Util.CreatePackage(repositoryPath, "packageA", "1.0.0");
            Util.CreatePackage(repositoryPath, "packageB", "1.0.0");
            Util.CreatePackage(repositoryPath, "packageC", "1.0.0");

            Util.CreatePackage(repositoryPath, "packageD", "1.0.0");
            Util.CreatePackage(repositoryPath, "packageE", "1.0.0");
            Util.CreatePackage(repositoryPath, "packageF", "1.0.0");

            Util.CreateFile(
               proj1Dir,
               "packages.config",
               @"<packages>
                     <package id=""packageA"" version=""1.0.0"" />
                     <package id=""packageB"" version=""1.0.0"" />
                  </packages>");

            Util.CreateFile(
               proj2Dir,
               "packages.config",
               @"<packages>
                     <package id=""packageC"" version=""1.0.0"" />
                     <package id=""packageB"" version=""1.0.0"" />
                  </packages>");

            Util.CreateFile(proj3Dir, "project.json",
                                            @"{
                                            'dependencies': {
                                                'packageD': '1.0.0',
                                                'packageE': '1.0.*'
                                            },
                                            'frameworks': {
                                                        'uap10.0': { }
                                                    }
                                            }");

            Util.CreateFile(proj4Dir, "project.json",
                                            @"{
                                            'dependencies': {
                                                'packageE': '1.0.0',
                                                'packageF': '*'
                                            },
                                            'frameworks': {
                                                        'uap10.0': { }
                                                    }
                                            }");

            Util.CreateFile(proj1Dir, "proj1.csproj", Util.GetCSProjXML("proj1"));
            Util.CreateFile(proj2Dir, "proj2.csproj", Util.GetCSProjXML("proj2"));
            Util.CreateFile(proj3Dir, "proj3.csproj", Util.GetCSProjXML("proj3"));
            Util.CreateFile(proj4Dir, "proj4.csproj", Util.GetCSProjXML("proj4"));

            var slnPath = Path.Combine(workingPath, "test.sln");

            Util.CreateFile(workingPath, "test.sln",
                       @"
                        Microsoft Visual Studio Solution File, Format Version 12.00
                        # Visual Studio 14
                        VisualStudioVersion = 14.0.23107.0
                        MinimumVisualStudioVersion = 10.0.40219.1
                        Project(""{AAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj1"", ""proj1\proj1.csproj"", ""{AA6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{BBE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj2"", ""proj2\proj2.csproj"", ""{BB6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{CCE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj3"", ""proj3\proj3.csproj"", ""{CC6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{DDE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj4"", ""proj4\proj4.csproj"", ""{DD6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Global
                            GlobalSection(SolutionConfigurationPlatforms) = preSolution
                                Debug|Any CPU = Debug|Any CPU
                                Release|Any CPU = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(ProjectConfigurationPlatforms) = postSolution
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {BB6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {BB6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {CC6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {CC6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {DD6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {DD6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                            EndGlobalSection
                            GlobalSection(SolutionProperties) = preSolution
                                HideSolutionNode = FALSE
                            EndGlobalSection
                        EndGlobal
                        ");
        }

        private Action<HttpListenerResponse> ServerHandler(
            HttpListenerRequest request,
            ConcurrentDictionary<string, int> hitsByUrl,
            MockServer server,
            JObject indexJson,
            string repositoryPath)
        {
            return ServerHandler(request,
                hitsByUrl,
                server,
                indexJson,
                repositoryPath,
                new ManualResetEventSlim(true),
                new ManualResetEventSlim(true));
        }

        private Action<HttpListenerResponse> ServerHandler(
            HttpListenerRequest request,
            ConcurrentDictionary<string, int> hitsByUrl,
            MockServer server,
            JObject indexJson,
            string repositoryPath,
            ManualResetEventSlim v2DownloadWait,
            ManualResetEventSlim v3DownloadWait)
        {
            try
            {
                var path = server.GetRequestUrlAbsolutePath(request);
                var parts = request.Url.AbsolutePath.Split('/');

                // track hits on the url
                var urlHits = hitsByUrl.AddOrUpdate(server.GetRequestRawUrl(request), 1, (s, i) => i + 1);

                if (path == "/index.json")
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.StatusCode = 200;
                        response.ContentType = "text/javascript";
                        MockServer.SetResponseContent(response, indexJson.ToString());
                    });
                }
                else if (path.StartsWith("/nuget/Packages(Id="))
                {
                    var splitOnSingleQuote = path.Split('\'');

                    var id = splitOnSingleQuote.Skip(1).First();
                    var version = splitOnSingleQuote.Skip(3).First();

                    var file = new FileInfo(Path.Combine(repositoryPath, $"{id}.{version}.nupkg"));

                    if (file.Exists)
                    {
                        return new Action<HttpListenerResponse>(response =>
                        {
                            response.ContentType = "application/atom+xml;type=entry;charset=utf-8";
                            var odata = server.ToOData(new PackageArchiveReader(file.OpenRead()));
                            MockServer.SetResponseContent(response, odata);
                        });
                    }
                    else
                    {
                        return new Action<HttpListenerResponse>(response =>
                        {
                            response.StatusCode = 404;
                        });
                    }
                }
                else if (path.StartsWith("/packages/"))
                {
                    v3DownloadWait.Wait();

                    var file = new FileInfo(Path.Combine(repositoryPath, parts.Last()));

                    if (file.Exists)
                    {
                        return new Action<HttpListenerResponse>(response =>
                        {
                            response.ContentType = "application/zip";
                            using (var stream = file.OpenRead())
                            {
                                var content = stream.ReadAllBytes();
                                MockServer.SetResponseContent(response, content);
                            }
                        });
                    }
                    else
                    {
                        return new Action<HttpListenerResponse>(response =>
                        {
                            response.StatusCode = 404;
                        });
                    }
                }
                else if (path.StartsWith("/package/") || path.StartsWith("/nuget/package/"))
                {
                    v2DownloadWait.Wait();

                    var id = parts.Reverse().Skip(1).First();
                    var version = parts.Last();

                    var file = new FileInfo(Path.Combine(repositoryPath, $"{id}.{version}.nupkg"));

                    if (file.Exists)
                    {
                        return new Action<HttpListenerResponse>(response =>
                        {
                            response.ContentType = "application/zip";
                            using (var stream = file.OpenRead())
                            {
                                var content = stream.ReadAllBytes();
                                MockServer.SetResponseContent(response, content);
                            }
                        });
                    }
                    else
                    {
                        return new Action<HttpListenerResponse>(response =>
                        {
                            response.StatusCode = 404;
                        });
                    }
                }
                else if (path.StartsWith("/reg/") && path.EndsWith("/index.json"))
                {
                    var id = parts.Reverse().Skip(1).First();
                    var version = "1.0.0";

                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.ContentType = "text/javascript";

                        var regBlob = Util.CreateSinglePackageRegistrationBlob(server, id, version);

                        MockServer.SetResponseContent(response, regBlob.ToString());
                    });
                }
                else if (path.StartsWith("/flat/") && path.EndsWith("/index.json"))
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.ContentType = "text/javascript";

                        MockServer.SetResponseContent(response, @"{
                              ""versions"": [
                                ""1.0.0""
                              ]
                            }");
                    });
                }
                else if (path.StartsWith("/flat/") && path.EndsWith(".nupkg"))
                {
                    v3DownloadWait.Wait();

                    var file = new FileInfo(Path.Combine(repositoryPath, parts.Last()));

                    if (file.Exists)
                    {
                        return new Action<HttpListenerResponse>(response =>
                        {
                            response.ContentType = "application/zip";
                            using (var stream = file.OpenRead())
                            {
                                var content = stream.ReadAllBytes();
                                MockServer.SetResponseContent(response, content);
                            }
                        });
                    }
                    else
                    {
                        return new Action<HttpListenerResponse>(response =>
                        {
                            response.StatusCode = 404;
                        });
                    }
                }
                else if (path.StartsWith("/nuget/FindPackagesById()"))
                {
                    var id = request.QueryString.Get("id").Trim('\'');
                    var packages = LocalFolderUtility.GetPackagesV2(repositoryPath, id, NullLogger.Instance);

                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.ContentType = "application/atom+xml;type=feed;charset=utf-8";
                        var feed = server.ToODataFeed(packages.Select(e => new FileInfo(e.Path)), "FindPackagesById");
                        MockServer.SetResponseContent(response, feed);
                    });
                }
                else if (path == "/nuget/$metadata")
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        MockServer.SetResponseContent(response, Util.GetMockServerResource());
                    });
                }
                else if (path == "/nuget")
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.StatusCode = 200;
                    });
                }

                throw new Exception("This test needs to be updated to support: " + path);
            }
            catch (Exception)
            {
                // Debug here
                throw;
            }
        }
    }
}
