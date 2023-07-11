// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    using IPackageFile = NuGet.Packaging.IPackageFile;

    public static class Util
    {
        private static readonly string NupkgFileFormat = "{0}.{1}.nupkg";

        public static string GetMockServerResource()
        {
            return GetResource("NuGet.CommandLine.Test.compiler.resources.mockserver.xml");
        }

        public static string GetResource(string name)
        {
            using (var reader = new StreamReader(typeof(Util).GetTypeInfo().Assembly.GetManifestResourceStream(name)))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Restore a solution.
        /// </summary>
        public static CommandRunnerResult RestoreSolution(SimpleTestPathContext pathContext, int expectedExitCode = 0, params string[] additionalArgs)
        {
            return Restore(pathContext, pathContext.SolutionRoot, expectedExitCode, additionalArgs);
        }

        /// <summary>
        /// Run nuget.exe restore {inputPath}
        /// </summary>
        public static CommandRunnerResult Restore(SimpleTestPathContext pathContext, string inputPath, int expectedExitCode = 0, params string[] additionalArgs)
        {
            var nugetExe = GetNuGetExePath();

            var args = new string[]
                {
                    "restore",
                    inputPath,
                    "-Verbosity",
                    "detailed"
                };

            args = args.Concat(additionalArgs).ToArray();

            return RunCommand(pathContext, nugetExe, expectedExitCode, args);
        }

        public static CommandRunnerResult RunCommand(SimpleTestPathContext pathContext, string nugetExe, int expectedExitCode = 0, params string[] arguments)
        {
            // Store the dg file for debugging
            var dgPath = Path.Combine(pathContext.WorkingDirectory, "out.dg");
            var envVars = new Dictionary<string, string>()
                {
                    { "NUGET_HTTP_CACHE_PATH", pathContext.HttpCacheFolder }
                };

            // Act
            var r = CommandRunner.Run(
                nugetExe,
                pathContext.WorkingDirectory.Path,
                string.Join(" ", arguments),
                environmentVariables: envVars);

            // Assert
            Assert.True(expectedExitCode == r.ExitCode, r.Errors + "\n\n" + r.Output);

            return r;
        }

        public static string CreateTestPackage(
            string packageId,
            string version,
            string path,
            string framework,
            string dependencyPackageId,
            string dependencyPackageVersion)
        {
            var group = new PackageDependencyGroup(NuGetFramework.AnyFramework, new List<PackageDependency>()
            {
                new PackageDependency(dependencyPackageId, VersionRange.Parse(dependencyPackageVersion))
            });

            return CreateTestPackage(packageId, version, path,
                new List<NuGetFramework>() { NuGetFramework.Parse(framework) },
                new List<PackageDependencyGroup>() { group });
        }

        public static string CreateTestPackage(
            string packageId,
            string version,
            string path,
            List<NuGetFramework> frameworks,
            List<PackageDependencyGroup> dependencies)
        {
            var packageBuilder = new PackageBuilder
            {
                Id = packageId,
                Version = new NuGetVersion(version)
            };

            packageBuilder.Description = string.Format(
                CultureInfo.InvariantCulture,
                "desc of {0} {1}",
                packageId, version);

            foreach (var framework in frameworks)
            {
                var libPath = string.Format(
                    CultureInfo.InvariantCulture,
                    "lib/{0}/file.dll",
                    framework.GetShortFolderName());

                packageBuilder.Files.Add(CreatePackageFile(libPath));
            }

            packageBuilder.Authors.Add("test author");

            packageBuilder.DependencyGroups.AddRange(dependencies);

            var packageFileName = string.Format("{0}.{1}.nupkg", packageId, version);
            var packageFileFullPath = Path.Combine(path, packageFileName);

            Directory.CreateDirectory(path);
            using (var fileStream = File.Create(packageFileFullPath))
            {
                packageBuilder.Save(fileStream);
            }

            return packageFileFullPath;
        }

        public static string CreateTestPackage(
            string packageId,
            string version,
            string path,
            List<NuGetFramework> frameworks,
            params string[] contentFiles)
        {
            var packageBuilder = new PackageBuilder
            {
                Id = packageId,
                Version = new NuGetVersion(version)
            };
            packageBuilder.Description = string.Format(
                CultureInfo.InvariantCulture,
                "desc of {0} {1}",
                packageId, version);
            foreach (var framework in frameworks)
            {
                var libPath = string.Format(
                    CultureInfo.InvariantCulture,
                    "lib/{0}/{1}.dll",
                    framework.GetShortFolderName(),
                    packageId);

                packageBuilder.Files.Add(CreatePackageFile(libPath));
            }

            foreach (var contentFile in contentFiles)
            {
                var packageFilePath = Path.Combine("content", contentFile);
                var packageFile = CreatePackageFile(packageFilePath);
                packageBuilder.Files.Add(packageFile);
            }

            packageBuilder.Authors.Add("test author");

            var packageFileName = string.Format("{0}.{1}.nupkg", packageId, version);
            var packageFileFullPath = Path.Combine(path, packageFileName);
            using (var fileStream = File.Create(packageFileFullPath))
            {
                packageBuilder.Save(fileStream);
            }

            return packageFileFullPath;
        }

        /// <summary>
        /// Creates a test package.
        /// </summary>
        /// <param name="packageId">The id of the created package.</param>
        /// <param name="version">The version of the created package.</param>
        /// <param name="path">The directory where the package is created.</param>
        /// <returns>The full path of the created package file.</returns>
        public static string CreateTestPackage(
            string packageId,
            string version,
            string path,
            Uri licenseUrl = null,
            params string[] contentFiles)
        {
            var packageBuilder = new PackageBuilder
            {
                Id = packageId,
                Version = new NuGetVersion(version)
            };
            packageBuilder.Description = string.Format(
                CultureInfo.InvariantCulture,
                "desc of {0} {1}",
                packageId, version);

            if (licenseUrl != null)
            {
                packageBuilder.LicenseUrl = licenseUrl;
            }

            if (contentFiles == null || contentFiles.Length == 0)
            {
                packageBuilder.Files.Add(CreatePackageFile(Path.Combine("content", "test1.txt")));
            }
            else
            {
                foreach (var contentFile in contentFiles)
                {
                    var packageFilePath = Path.Combine("content", contentFile);
                    var packageFile = CreatePackageFile(packageFilePath);
                    packageBuilder.Files.Add(packageFile);
                }
            }

            packageBuilder.Authors.Add("test author");
            var packageFileName = BuildPackageString(packageId, version, NuGetConstants.PackageExtension);
            var packageFileFullPath = Path.Combine(path, packageFileName);
            Directory.CreateDirectory(path);
            using (var fileStream = File.Create(packageFileFullPath))
            {
                packageBuilder.Save(fileStream);
            }

            return packageFileFullPath;
        }

        /// <summary>
        /// Assembles a filename for the given package ID, version, and extension.
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="version"></param>
        /// <param name="extension">File extension with or without a leading dot (".").</param>
        /// <returns></returns>
        public static string BuildPackageString(string packageId, string version, string extension)
        {
            string dotPrefix = (!extension.StartsWith(".")) ? "." : string.Empty;
            return $"{packageId}.{version}{dotPrefix}{extension}";
        }

        /// <summary>
        /// Create a project.json based project. Returns the path to the project file.
        /// </summary>
        public static string CreateUAPProject(string directory, string projectJsonContent)
        {
            return CreateUAPProject(directory, projectJsonContent, "a");
        }

        /// <summary>
        /// Create a project.json based project. Returns the path to the project file.
        /// </summary>
        public static string CreateUAPProject(string directory, string projectJsonContent, string projectName)
        {
            return CreateUAPProject(directory, projectJsonContent, projectName, nugetConfigContent: null);
        }

        /// <summary>
        /// Create a project.json based project. Returns the path to the project file.
        /// </summary>
        public static string CreateUAPProject(string directory, string projectJsonContent, string projectName, string nugetConfigContent)
        {
            Directory.CreateDirectory(directory);
            var projectDir = directory;
            var projectFile = Path.Combine(projectDir, projectName + ".csproj");
            var projectJsonPath = Path.Combine(projectDir, "project.json");
            var configPath = Path.Combine(projectDir, "NuGet.Config");

            // Clean up and validate json
            var json = JObject.Parse(projectJsonContent);

            File.WriteAllText(projectJsonPath, json.ToString());
            File.WriteAllText(projectFile, GetCSProjXML(projectName));

            if (!string.IsNullOrEmpty(nugetConfigContent))
            {
                File.WriteAllText(configPath, nugetConfigContent);
            }

            return projectFile;
        }

        /// <summary>
        /// Creates a file with the specified content.
        /// </summary>
        /// <param name="directory">The directory of the created file.</param>
        /// <param name="fileName">The name of the created file.</param>
        /// <param name="fileContent">The content of the created file.</param>
        public static void CreateFile(string directory, string fileName, string fileContent)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fileFullName = Path.Combine(directory, fileName);
            CreateFile(fileFullName, fileContent);
        }

        public static void CreateFile(string fileFullName, string fileContent)
        {
            using (var writer = new StreamWriter(fileFullName))
            {
                writer.Write(fileContent);
            }
        }

        public static IPackageFile CreatePackageFile(string name)
        {
            var file = new Mock<IPackageFile>();
            file.SetupGet(f => f.Path).Returns(name);
            file.Setup(f => f.GetStream()).Returns(new MemoryStream());

            string effectivePath;
            var fx = FrameworkNameUtility.ParseNuGetFrameworkFromFilePath(name, out effectivePath);
            file.SetupGet(f => f.EffectivePath).Returns(effectivePath);
            file.SetupGet(f => f.NuGetFramework).Returns(fx);

            return file.Object;
        }

        /// <summary>
        /// Creates a mock server that contains the specified list of packages
        /// </summary>
        public static MockServer CreateMockServer(IList<FileInfo> packages)
        {
            var server = new MockServer();

            server.Get.Add("/nuget/$metadata", r =>
                   Util.GetMockServerResource());
            server.Get.Add("/nuget/FindPackagesById()", r =>
                new Action<HttpListenerResponse>(response =>
                {
                    response.ContentType = "application/atom+xml;type=feed;charset=utf-8";
                    string feed = server.ToODataFeed(packages, "FindPackagesById");
                    MockServer.SetResponseContent(response, feed);
                }));

            foreach (var file in packages)
            {
                var package = new PackageArchiveReader(file.OpenRead());
                var url = string.Format(
                    CultureInfo.InvariantCulture,
                    "/nuget/Packages(Id='{0}',Version='{1}')",
                    package.NuspecReader.GetId(),
                    package.NuspecReader.GetVersion());
                server.Get.Add(url, r =>
                    new Action<HttpListenerResponse>(response =>
                    {
                        response.ContentType = "application/atom+xml;type=entry;charset=utf-8";
                        var p1 = server.ToOData(package);
                        MockServer.SetResponseContent(response, p1);
                    }));

                // download url
                url = string.Format(
                    CultureInfo.InvariantCulture,
                    "/package/{0}/{1}",
                    package.NuspecReader.GetId(),
                    package.NuspecReader.GetVersion());
                server.Get.Add(url, r =>
                    new Action<HttpListenerResponse>(response =>
                    {
                        response.ContentType = "application/zip";
                        using (var stream = file.OpenRead())
                        {
                            var content = stream.ReadAllBytes();
                            MockServer.SetResponseContent(response, content);
                        }
                    }));
            }

            // fall through to "package not found"
            server.Get.Add("/nuget/Packages(Id='", r =>
                new Action<HttpListenerResponse>(response =>
                {
                    response.StatusCode = 404;
                    MockServer.SetResponseContent(response, @"<?xml version=""1.0"" encoding=""utf-8""?>
<m:error xmlns:m=""http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"">
  <m:code />
  <m:message xml:lang=""en-US"">Resource not found for the segment 'Packages'.</m:message>
</m:error>");
                }));

            server.Get.Add("/nuget", r =>
                new Action<HttpListenerResponse>(response =>
                {
                    response.StatusCode = 404;
                }));

            return server;
        }

        /// <summary>
        /// Path to nuget.exe for tests.
        /// </summary>
        public static string GetNuGetExePath()
        {
            const string fileName = "NuGet.exe";
            var targetDir = ConfigurationManager.AppSettings["TestTargetDir"] ?? Directory.GetCurrentDirectory();
            var nugetExe = Path.Combine(targetDir, "NuGet", fileName);
            // Revert to parent dir if not found under layout dir.
            if (!File.Exists(nugetExe)) nugetExe = Path.Combine(targetDir, fileName);
            if (!File.Exists(nugetExe)) throw new FileNotFoundException($"The NuGet executable is not present in '{targetDir}'", fileName);
            return nugetExe;
        }

        public static string GetTestablePluginPath()
        {
            const string fileName = "CredentialProvider.Testable.exe";
            var targetDir = ConfigurationManager.AppSettings["TestTargetDir"] ?? Directory.GetCurrentDirectory();
            var plugin = Path.Combine(targetDir, "TestableCredentialProvider", fileName);
            // Revert to parent dir if not found under layout dir.
            if (!File.Exists(plugin)) plugin = Path.Combine(targetDir, fileName);
            if (!File.Exists(plugin)) throw new FileNotFoundException($"The CredentialProvider executable is not present in '{targetDir}'", fileName);
            return plugin;
        }

        public static string GetTestablePluginDirectory()
        {
            return Path.GetDirectoryName(GetTestablePluginPath());
        }

        public static JObject CreateIndexJson()
        {
            return FeedUtilities.CreateIndexJson();
        }

        public static void AddFlatContainerResource(JObject index, MockServer server)
        {
            FeedUtilities.AddFlatContainerResource(index, server.Uri);
        }

        public static void AddRegistrationResource(JObject index, MockServer server)
        {
            FeedUtilities.AddRegistrationResource(index, server.Uri);
        }

        public static void AddLegacyGalleryResource(JObject index, MockServer serverV2, string relativeUri = null)
        {
            FeedUtilities.AddLegacyGalleryResource(index, serverV2.Uri, relativeUri);
        }

        public static void AddPublishResource(JObject index, MockServer publishServer)
        {
            FeedUtilities.AddPublishResource(index, publishServer.Uri);
        }

        public static void AddPublishSymbolsResource(JObject index, MockServer publishServer)
        {
            FeedUtilities.AddPublishSymbolsResource(index, publishServer.Uri);
        }

        public static void CreateConfigForGlobalPackagesFolder(string workingDirectory)
        {
            CreateNuGetConfig(workingDirectory, new List<string>());
        }

        public static void CreateNuGetConfig(string workingPath, List<string> sources)
        {
            var doc = new XDocument();
            var configuration = new XElement(XName.Get("configuration"));
            doc.Add(configuration);

            var config = new XElement(XName.Get("config"));
            configuration.Add(config);

            var globalFolder = new XElement(XName.Get("add"));
            globalFolder.Add(new XAttribute(XName.Get("key"), "globalPackagesFolder"));
            globalFolder.Add(new XAttribute(XName.Get("value"), Path.Combine(workingPath, "globalPackages")));
            config.Add(globalFolder);

            var solutionDir = new XElement(XName.Get("add"));
            solutionDir.Add(new XAttribute(XName.Get("key"), "repositoryPath"));
            solutionDir.Add(new XAttribute(XName.Get("value"), Path.Combine(workingPath, "packages")));
            config.Add(solutionDir);

            var packageSources = new XElement(XName.Get("packageSources"));
            configuration.Add(packageSources);
            packageSources.Add(new XElement(XName.Get("clear")));

            foreach (var source in sources)
            {
                var sourceEntry = new XElement(XName.Get("add"));
                sourceEntry.Add(new XAttribute(XName.Get("key"), source));
                sourceEntry.Add(new XAttribute(XName.Get("value"), source));
                packageSources.Add(sourceEntry);
            }

            var packageSourceMapping = new XElement(XName.Get("packageSourceMapping"));
            configuration.Add(packageSourceMapping);
            packageSourceMapping.Add(new XElement(XName.Get("clear")));

            Util.CreateFile(workingPath, "NuGet.Config", doc.ToString());
        }

        public static void CreateNuGetConfig(string workingPath, List<string> sources, List<string> pluginPaths)
        {
            CreateNuGetConfig(workingPath, sources);
            var existingConfig = Path.Combine(workingPath, "NuGet.Config");

            var doc = XDocument.Load(existingConfig);
            var config = doc.Descendants(XName.Get("config")).FirstOrDefault();

            foreach (var pluginPath in pluginPaths)
            {
                var key = "CredentialProvider.Plugin." + Path.GetFileNameWithoutExtension(pluginPath);
                var pluginElement = new XElement(XName.Get("add"));
                pluginElement.Add(new XAttribute(XName.Get("key"), key));
                pluginElement.Add(new XAttribute(XName.Get("value"), pluginPath));

                config.Add(pluginElement);
            }

            doc.Save(existingConfig);
        }

        public static void CreateNuGetConfig(string workingPath, List<string> sources, string packagesPath)
        {
            CreateNuGetConfig(workingPath, sources);
            var existingConfig = Path.Combine(workingPath, "NuGet.Config");

            var doc = XDocument.Load(existingConfig);
            var config = doc.Descendants(XName.Get("config")).FirstOrDefault();
            var repositoryPath = config.Descendants().First(x => x.Name == "add" && x.Attribute("key").Value == "repositoryPath").Attribute("value");
            repositoryPath.SetValue(packagesPath);

            doc.Save(existingConfig);
        }

        /// <summary>
        /// Create a simple package with a lib folder. This package should install everywhere.
        /// The package will be removed from the machine cache upon creation
        /// </summary>
        public static void CreatePackage(string repositoryPath, string id, string version)
        {

            var context = new SimpleTestPackageContext(id, version);
            context.AddFile("lib/uap/a.dll", "a");
            context.AddFile("lib/net45/a.dll", "a");
            context.AddFile("lib/native/a.dll", "a");
            context.AddFile("lib/win/a.dll", "a");
            context.AddFile("lib/net20/a.dll", "a");
            SimpleTestPackageUtility.CreateOPCPackage(context, repositoryPath);
        }

        /// <summary>
        /// Create a registration blob for a single package
        /// </summary>
        public static JObject CreateSinglePackageRegistrationBlob(MockServer server, string id, string version)
        {
            return FeedUtilities.CreatePackageRegistrationBlob(server.Uri, id, new KeyValuePair<string, bool>[] { new KeyValuePair<string, bool>(version, true) });
        }

        public static string CreateProjFileContent(
            string projectName = "proj1",
            string targetFrameworkVersion = "v4.7.2",
            string[] references = null,
            string[] contentFiles = null)
        {
            var project = CreateProjFileXmlContent(projectName, targetFrameworkVersion, references, contentFiles);
            return project.ToString();
        }

        public static XElement CreateProjFileXmlContent(
            string projectName = "proj1",
            string targetFrameworkVersion = "v4.7.2",
            string[] references = null,
            string[] contentFiles = null)
        {
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

            var project = new XElement(msbuild + "Project",
                new XAttribute("ToolsVersion", "4.0"), new XAttribute("DefaultTargets", "Build"));

            project.Add(new XElement(msbuild + "PropertyGroup",
                  new XElement(msbuild + "OutputType", "Library"),
                  new XElement(msbuild + "OutputPath", "out"),
                  new XElement(msbuild + "TargetFrameworkVersion", targetFrameworkVersion)));

            if (references != null && references.Any())
            {
                project.Add(new XElement(msbuild + "ItemGroup",
                        references.Select(r => new XElement(msbuild + "Reference", new XAttribute("Include", r)))));
            }

            if (contentFiles != null && contentFiles.Any())
            {
                project.Add(new XElement(msbuild + "ItemGroup",
                        contentFiles.Select(c => new XElement(msbuild + "Content", new XAttribute("Include", c)))));
            }

            project.Add(new XElement(msbuild + "ItemGroup",
                new XElement(msbuild + "Compile", new XAttribute("Include", "Source.cs"))));

            project.Add(new XElement(msbuild + "Import",
                new XAttribute("Project", @"$(MSBuildToolsPath)\Microsoft.CSharp.targets")));

            return project;
        }

        public static string CreateSolutionFileContent()
        {
            return @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj1"",
""proj1.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject";
        }

        public static void VerifyResultSuccess(CommandRunnerResult result, string expectedOutputMessage = null)
        {
            Assert.True(
                result.ExitCode == 0,
                "nuget.exe DID NOT SUCCEED: Ouput is " + result.Output + ". Error is " + result.Errors);

            if (!string.IsNullOrEmpty(expectedOutputMessage))
            {
                Assert.Contains(
                    expectedOutputMessage,
                    result.Output);
            }
        }

        /// <summary>
        /// Utility for asserting faulty executions of nuget.exe
        ///
        /// Asserts a non-zero status code and a message on stderr.
        /// </summary>
        /// <param name="result">An instance of <see cref="CommandRunnerResult"/> with command execution results</param>
        /// <param name="expectedErrorMessage">A portion of the error message to be sent</param>
        public static void VerifyResultFailure(CommandRunnerResult result,
                                               string expectedErrorMessage)
        {
            Assert.True(
                result.ExitCode != 0,
                "nuget.exe DID NOT FAIL: Ouput is " + result.Output + ". Error is " + result.Errors);

            Assert.True(
                result.Errors.Contains(expectedErrorMessage),
                "Expected error is " + expectedErrorMessage + ". Actual error is " + result.Errors);
        }

        public static void VerifyPackageExists(
            PackageIdentity packageIdentity,
            string packagesDirectory)
        {
            string normalizedId = packageIdentity.Id.ToLowerInvariant();
            string normalizedVersion = packageIdentity.Version.ToNormalizedString().ToLowerInvariant();

            var packageIdDirectory = Path.Combine(packagesDirectory, normalizedId);
            Assert.True(Directory.Exists(packageIdDirectory));

            var packageVersionDirectory = Path.Combine(packageIdDirectory, normalizedVersion);
            Assert.True(Directory.Exists(packageVersionDirectory));

            var nupkgFileName = GetNupkgFileName(normalizedId, normalizedVersion);

            var nupkgFilePath = Path.Combine(packageVersionDirectory, nupkgFileName);
            Assert.True(File.Exists(nupkgFilePath));

            var nupkgSHAFilePath = Path.Combine(packageVersionDirectory, nupkgFileName + ".sha512");
            Assert.True(File.Exists(nupkgSHAFilePath));

            var nuspecFilePath = Path.Combine(packageVersionDirectory, normalizedId + ".nuspec");
            Assert.True(File.Exists(nuspecFilePath));
        }

        public static void VerifyPackageDoesNotExist(
            PackageIdentity packageIdentity,
            string packagesDirectory)
        {
            string normalizedId = packageIdentity.Id.ToLowerInvariant();
            var packageIdDirectory = Path.Combine(packagesDirectory, normalizedId);
            Assert.False(Directory.Exists(packageIdDirectory));
        }

        public static void VerifyPackagesExist(
            IList<PackageIdentity> packages,
            string packagesDirectory)
        {
            foreach (var package in packages)
            {
                VerifyPackageExists(package, packagesDirectory);
            }
        }

        public static void VerifyPackagesDoNotExist(
            IList<PackageIdentity> packages,
            string packagesDirectory)
        {
            foreach (var package in packages)
            {
                VerifyPackageDoesNotExist(package, packagesDirectory);
            }
        }

        /// <summary>
        /// To verify packages created using TestPackages.GetLegacyTestPackage
        /// </summary>
        public static void VerifyExpandedLegacyTestPackagesExist(
            IList<PackageIdentity> packages,
            string packagesDirectory)
        {
            var versionFolderPathResolver
                = new VersionFolderPathResolver(packagesDirectory);

            var packageFiles = new[]
            {
                    "lib/test.dll",
                    "lib/net40/test40.dll",
                    "lib/net40/test40b.dll",
                    "lib/net45/test45.dll",
                };

            foreach (var package in packages)
            {
                Util.VerifyPackageExists(package, packagesDirectory);
                var packageRoot = versionFolderPathResolver.GetInstallPath(package.Id, package.Version);
                foreach (var packageFile in packageFiles)
                {
                    var filePath = Path.Combine(packageRoot, packageFile);
                    Assert.True(File.Exists(filePath), $"For {package}, {filePath} does not exist.");
                }
            }
        }

        public static string GetNupkgFileName(string normalizedId, string normalizedVersion)
        {
            return string.Format(NupkgFileFormat, normalizedId, normalizedVersion);
        }

        /// <summary>
        /// Creates a junction point from the specified directory to the specified target directory.
        /// </summary>
        /// <remarks>Only works on NTFS.</remarks>
        /// <param name="junctionPoint">The junction point path</param>
        /// <param name="targetDirectoryPath">The target directory</param>
        /// <param name="overwrite">If true overwrites an existing reparse point or empty directory</param>
        /// <exception cref="IOException">
        /// Thrown when the junction point could not be created or when
        /// an existing directory was found and <paramref name="overwrite" /> if false
        /// </exception>
        public static void CreateJunctionPoint(string junctionPoint, string targetDirectoryPath, bool overwrite)
        {
            targetDirectoryPath = Path.GetFullPath(targetDirectoryPath);

            if (!Directory.Exists(targetDirectoryPath))
            {
                throw new IOException("Target path does not exist or is not a directory.");
            }

            if (Directory.Exists(junctionPoint))
            {
                if (!overwrite)
                {
                    throw new IOException("Directory already exists and overwrite parameter is false.");
                }
            }
            else
            {
                Directory.CreateDirectory(junctionPoint);
            }

            NativeMethods.CreateReparsePoint(junctionPoint, targetDirectoryPath);
        }

        public static string GetXProjXML()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
                <Project ToolsVersion=""14.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
                  <PropertyGroup>
                    <VisualStudioVersion Condition=""'$(VisualStudioVersion)' == ''"">14.0</VisualStudioVersion>
                    <VSToolsPath Condition=""'$(VSToolsPath)' == ''"">$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v$(VisualStudioVersion)</VSToolsPath>
                  </PropertyGroup>
                  <Import Project=""$(VSToolsPath)\DNX\Microsoft.DNX.Props"" Condition=""'$(VSToolsPath)' != ''"" />
                  <PropertyGroup Label=""Globals"">
                    <ProjectGuid>82ff10c5-8724-4187-953e-5096ad90184f</ProjectGuid>
                  </PropertyGroup>
                  <ItemGroup>
                    <Service Include=""{82a7f48d-3b50-4b1e-b82e-3ada8210c358}"" />
                  </ItemGroup>
                  <Import Project=""$(VSToolsPath)\DNX\Microsoft.DNX.targets"" Condition=""'$(VSToolsPath)' != ''"" />
                </Project>";
        }

        /// <summary>
        /// Create a basic csproj file for net45.
        /// </summary>
        public static string GetCSProjXML(string projectName)
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
                <Project ToolsVersion=""14.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
                  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
                  <PropertyGroup>
                    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
                    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
                    <ProjectGuid>29b6f645-ae2a-4653-a142-d0de9341adba</ProjectGuid>
                    <OutputType>Library</OutputType>
                    <AppDesignerFolder>Properties</AppDesignerFolder>
                    <RootNamespace>$NAME$</RootNamespace>
                    <AssemblyName>$NAME$</AssemblyName>
                    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
                    <FileAlignment>512</FileAlignment>
                    <DebugSymbols>true</DebugSymbols>
                    <DebugType>full</DebugType>
                    <Optimize>false</Optimize>
                    <OutputPath>bin\Debug\</OutputPath>
                    <DefineConstants>DEBUG;TRACE</DefineConstants>
                    <ErrorReport>prompt</ErrorReport>
                    <WarningLevel>4</WarningLevel>
                  </PropertyGroup>
                  <ItemGroup>
                    <Reference Include=""System""/>
                    <Reference Include=""System.Core""/>
                    <Reference Include=""System.Xml.Linq""/>
                    <Reference Include=""System.Data.DataSetExtensions""/>
                    <Reference Include=""Microsoft.CSharp""/>
                    <Reference Include=""System.Data""/>
                    <Reference Include=""System.Net.Http""/>
                    <Reference Include=""System.Xml""/>
                  </ItemGroup>
                  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
                 </Project>".Replace("$NAME$", projectName);
        }

        public static string CreateBasicTwoProjectSolution(TestDirectory workingPath, string proj1ConfigFileName, string proj2ConfigFileName, bool redirectGlobalPackagesFolder = true)
        {
            var repositoryPath = Path.Combine(workingPath, "Repository");
            var proj1Directory = Path.Combine(workingPath, "proj1");
            var proj2Directory = Path.Combine(workingPath, "proj2");

            Directory.CreateDirectory(repositoryPath);
            Directory.CreateDirectory(proj1Directory);
            Directory.CreateDirectory(proj2Directory);

            CreateTestPackage("packageA", "1.1.0", repositoryPath);
            CreateTestPackage("packageB", "2.2.0", repositoryPath);

            CreateFile(workingPath, "a.sln",
                @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj1"", ""proj1\proj1.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj2"", ""proj2\proj2.csproj"", ""{42641DAE-D6C4-49D4-92EA-749D2573554A}""
EndProject");

            var include1 = proj1ConfigFileName;

            if (string.IsNullOrEmpty(include1))
            {
                include1 = Guid.NewGuid().ToString();
            }

            CreateFile(proj1Directory, "proj1.csproj",
                $@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='{include1}' />
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>");

            if (!string.IsNullOrEmpty(proj1ConfigFileName))
            {
                CreateConfigFile(proj1Directory, proj1ConfigFileName, "net45", new List<PackageIdentity>
                {
                    new PackageIdentity("packageA", new NuGetVersion("1.1.0"))
                });
            }

            var include2 = proj2ConfigFileName;

            if (string.IsNullOrEmpty(include2))
            {
                include2 = Guid.NewGuid().ToString();
            }

            CreateFile(proj2Directory, "proj2.csproj",
                $@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='{include2}' />
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>");

            if (!string.IsNullOrEmpty(proj2ConfigFileName))
            {
                CreateConfigFile(proj2Directory, proj2ConfigFileName, "net45", new List<PackageIdentity>
                {
                    new PackageIdentity("packageB", new NuGetVersion("2.2.0"))
                });
            }

            // If either project uses project.json, then define "globalPackagesFolder" so the package doesn't get
            // installed in the usual global packages folder.
            if ((IsProjectJson(proj1ConfigFileName) || IsProjectJson(proj2ConfigFileName)) && redirectGlobalPackagesFolder)
            {
                CreateFile(workingPath, "nuget.config",
@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <config>
    <add key=""globalPackagesFolder"" value=""GlobalPackages"" />
  </config>
</configuration>");
            }

            return repositoryPath;
        }

        public static string CreateBasicTwoProjectSolutionWithSolutionFilters(TestDirectory workingPath, string proj1ConfigFileName, string proj2ConfigFileName, bool redirectGlobalPackagesFolder = true)
        {
            var repositoryPath = CreateBasicTwoProjectSolution(workingPath, proj1ConfigFileName, proj2ConfigFileName, redirectGlobalPackagesFolder);
            var filterInSubfolderPath = Path.Combine(workingPath, "filter");

            Directory.CreateDirectory(filterInSubfolderPath);

            CreateFile(workingPath, "a.proj1.slnf", @"{
  ""solution"": {
    ""path"": ""a.sln"",
    ""projects"": [
      ""proj1\\proj1.csproj""
    ]
  }
}");

            CreateFile(workingPath, "a.proj2.slnf", @"{
  ""solution"": {
    ""path"": ""a.sln"",
    ""projects"": [
      ""proj2\\proj2.csproj""
    ]
  }
}");


            CreateFile(filterInSubfolderPath, "filterinsubfolder.slnf",  @"{
  ""solution"": {
    ""path"": ""..\\a.sln"",
    ""projects"": [
      ""proj1\\proj1.csproj"",
      ""proj2\\proj2.csproj"",
    ]
  }
}");


            return repositoryPath;
        }

        public static void CreateConfigFile(string path, string configFileName, string targetFramework, IEnumerable<PackageIdentity> packages)
        {
            var fileContent = IsProjectJson(configFileName)
                ? GetProjectJsonFileContents(targetFramework, packages)
                : GetPackagesConfigFileContents(targetFramework, packages);

            CreateFile(path, configFileName, fileContent);
        }

        public static string GetProjectJsonFileContents(string targetFramework, IEnumerable<PackageIdentity> packages)
        {
            var dependencies = string.Join(", ", packages.Select(package => $"'{package.Id}': '{package.Version}'"));
            return $@"
{{
  'dependencies': {{
    {dependencies}
  }},
  'frameworks': {{
    '{targetFramework}': {{ }}
  }}
}}";
        }

        public static string GetPackagesConfigFileContents(string targetFramework, IEnumerable<PackageIdentity> packages)
        {
            var dependencies = string.Join("\n", packages.Select(package => $@"<package id=""{package.Id}"" version=""{package.Version}"" targetFramework=""{targetFramework}"" />"));
            return $@"
<packages>
  {dependencies}
</packages>";
        }

        public static string GetMsbuildPathOnWindows()
        {
            return MsBuildUtility.GetMsBuildDirectoryFromMsBuildPath(null, null, null).Value.Path;
        }

        public static string GetHintPath(string path)
        {
            return @"<HintPath>.." + Path.DirectorySeparatorChar + path + @"</HintPath>";
        }

        private static bool IsProjectJson(string configFileName)
        {
            // Simply test the extension as that is all we care about
            return string.Equals(Path.GetExtension(configFileName), ".json", StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Verify non-zero status code and proper messages
        /// </summary>
        /// <remarks>Checks invalid arguments message in stderr, check help message in stdout</remarks>
        /// <param name="commandName">The nuget.exe command name to verify, without "nuget.exe" at the beginning</param>
        public static void TestCommandInvalidArguments(string command)
        {
            // Act
            var result = CommandRunner.Run(
                Util.GetNuGetExePath(),
                Directory.GetCurrentDirectory(),
                command);

            var commandSplit = command.Split(' ');

            // Break the test if no proper command is found
            if (commandSplit.Length < 1 || string.IsNullOrEmpty(commandSplit[0]))
                Assert.True(false, "command not found");

            var mainCommand = commandSplit[0];

            // Assert command
            Assert.Contains(mainCommand, result.Errors, StringComparison.InvariantCultureIgnoreCase);
            // Assert invalid argument message
            var invalidMessage = string.Format(": invalid arguments.", mainCommand);
            // Verify Exit code
            VerifyResultFailure(result, invalidMessage);
            // Verify traits of help message in stdout
            Assert.Contains("usage:", result.Output);
        }

        /// <summary>
        /// Create a basic nfproj file for .NET nanoFramework.
        /// </summary>
        public static string GetNFProjXML(
            string projectName,
            List<(string, string)> packages)
        {
            var projContent = new StringBuilder();

            projContent.AppendLine(
@"<?xml version=""1.0"" encoding=""utf-8""?>
    <Project ToolsVersion=""Current"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
        <PropertyGroup Label=""Globals"">
        <NanoFrameworkProjectSystemPath>$(MSBuildToolsPath)..\..\..\nanoFramework\v1.0\</NanoFrameworkProjectSystemPath>
        </PropertyGroup>
        <Import Project=""$(NanoFrameworkProjectSystemPath)NFProjectSystem.Default.props"" Condition=""Exists('$(NanoFrameworkProjectSystemPath)NFProjectSystem.Default.props')"" />
        <PropertyGroup>
        <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
        <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
        <ProjectTypeGuids>{11A8DD76-328B-46DF-9F39-F559912D0360};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
        <ProjectGuid>9e332e06-5faf-4861-8318-a806978b677d</ProjectGuid>
        <OutputType>Exe</OutputType>
        <AppDesignerFolder>Properties</AppDesignerFolder>
        <FileAlignment>512</FileAlignment>
        <RootNamespace>$NAME$</RootNamespace>
        <AssemblyName>$NAME$</AssemblyName>
        <TargetFrameworkVersion>v1.0</TargetFrameworkVersion>
        </PropertyGroup>
        <Import Project=""$(NanoFrameworkProjectSystemPath)NFProjectSystem.props"" Condition=""Exists('$(NanoFrameworkProjectSystemPath)NFProjectSystem.props')"" />
        <ItemGroup>");

            foreach ((string, string) package in packages)
            {
                projContent.AppendLine(
$@"        <Reference Include=""{package.Item1}, Version={package.Item2}, Culture=neutral"">
            <HintPath>..\packages\{package.Item1}.{package.Item2}\lib\{package.Item1}.dll</HintPath>
            <Private>True</Private>
            <SpecificVersion>True</SpecificVersion>
        </Reference>");
            }

            projContent.AppendLine(
@"        </ItemGroup>
            <ItemGroup>
            <None Include=""packages.config"" />
            </ItemGroup>
            <Import Project=""$(NanoFrameworkProjectSystemPath)NFProjectSystem.CSharp.targets"" Condition=""Exists('$(NanoFrameworkProjectSystemPath)NFProjectSystem.CSharp.targets')"" />
            <ProjectExtensions>
            <ProjectCapabilities>
                <ProjectConfigurationsDeclaredAsItems />
            </ProjectCapabilities>
            </ProjectExtensions>
        </Project>".Replace("$NAME$", projectName));

            return projContent.ToString();
        }

        /// <summary>
        /// Create a Solution file with .NET nanoFramework projects in it.
        /// </summary>
        /// <param name="projectList">List of .NET nanoFramework projects to add to the solution</param>
        /// <returns>Content of the Solution file</returns>
        public static string CreateNFSolutionFileContent(List<string> projectList)
        {
            var slnContent = new StringBuilder();

            slnContent.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");
            slnContent.AppendLine("# Visual Studio Version 16");
            slnContent.AppendLine("VisualStudioVersion = 16.0.31005.135");

            foreach (string project in projectList)
            {
                slnContent.AppendLine($"Project(\"{{11A8DD76-328B-46DF-9F39-F559912D0360}}\") = \"{project}\", \"{project}\\{project}.nfproj\", \"{{{Guid.NewGuid().ToString()}}}");
                slnContent.AppendLine("EndProject");
            }

            return slnContent.ToString();
        }

        /// <summary>
        /// Build packages.config from list of NuGet packages for .NET nanoFramework.
        /// </summary>
        /// <param name="packages">List of NuGet packages for .NET nanoFramework</param>
        /// <returns></returns>
        internal static string GetNFPackageConfig(List<(string, string)> packages)
        {
            var configContent = new StringBuilder();

            configContent.AppendLine("<packages>");

            foreach ((string, string) package in packages)
            {
                configContent.AppendLine($@"  <package id=""{package.Item1}"" version=""{package.Item2}"" targetFramework=""netnanoframework10"" />");
            }

            configContent.AppendLine("</packages>");

            return configContent.ToString();
        }
    }
}
