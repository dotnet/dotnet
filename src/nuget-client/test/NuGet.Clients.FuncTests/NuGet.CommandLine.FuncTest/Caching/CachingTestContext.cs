// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;

namespace NuGet.CommandLine.Test.Caching
{
    public class CachingTestContext
    {
        public CachingTestContext(TestDirectory testDirectory, MockServer mockServer, INuGetExe nuGetExe)
        {
            TestDirectory = testDirectory;
            MockServer = mockServer;
            NuGetExe = nuGetExe;

            PackageFramework = FrameworkConstants.CommonFrameworks.Net45;
            PackageIdentityA = new PackageIdentity("TestPackageA", new NuGetVersion("1.0.0"));
            PackageIdentityB = new PackageIdentity("TestPackageB", new NuGetVersion("1.0.0"));

            InitializeFiles();
            InitializeServer();

            MockServer.Start();
        }

        public TestDirectory TestDirectory { get; }
        public MockServer MockServer { get; }

        public NuGetFramework PackageFramework { get; }
        public PackageIdentity PackageIdentityA { get; }
        public PackageIdentity PackageIdentityB { get; }

        public INuGetExe NuGetExe { get; private set; }
        public string WorkingPath { get; private set; }
        public string GlobalPackagesPath { get; private set; }
        public string IsolatedHttpCachePath { get; private set; }
        public string InputPackagesPath { get; private set; }
        public string ProjectJsonPath { get; private set; }
        public string ProjectPath { get; private set; }
        public string PackagesConfigPath { get; private set; }
        public string OutputPackagesPath { get; private set; }

        public string PackageAVersionAPath { get; private set; }
        public string PackageAVersionBPath { get; private set; }
        public string PackageBPath { get; private set; }
        public string CurrentPackageAPath { get; set; }
        public bool IsPackageAAvailable { get; set; } = true;
        public bool IsPackageBAvailable { get; set; } = true;

        public string V2Source { get; private set; }
        public string V3Source { get; private set; }

        public string CurrentSource { get; set; }
        public bool NoCache { get; set; }
        public bool DirectDownload { get; set; }

        public bool Debug { get; set; }

        private void InitializeServer()
        {
            var baseUrl = MockServer.Uri.TrimEnd(new[] { '/' });
            var builder = new MockResponseBuilder(baseUrl);

            CurrentSource = builder.GetV2Source();
            V2Source = builder.GetV2Source();
            V3Source = builder.GetV3Source();

            AddPackageEndpoints(builder, PackageIdentityA, () => CurrentPackageAPath, () => IsPackageAAvailable);
            AddPackageEndpoints(builder, PackageIdentityB, () => PackageBPath, () => IsPackageBAvailable);

            // Add the V3 service index.
            MockServer.Get.Add(
                builder.GetV3IndexPath(),
                request =>
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        var mockResponse = builder.BuildV3IndexResponse(MockServer.Uri);

                        response.ContentType = mockResponse.ContentType;
                        MockServer.SetResponseContent(response, mockResponse.Content);
                    });
                });

            // Add the V2 "service index".
            var v2IndexPath = builder.GetV2IndexPath();
            MockServer.Get.Add(
                v2IndexPath,
                request =>
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        if (!request.RawUrl.EndsWith(v2IndexPath))
                        {
                            response.StatusCode = 404;
                            return;
                        }

                        var mockResponse = builder.BuildV2IndexResponse();

                        response.ContentType = mockResponse.ContentType;
                        MockServer.SetResponseContent(response, mockResponse.Content);
                    });
                });
        }

        private void AddPackageEndpoints(MockResponseBuilder builder, PackageIdentity identity, Func<string> getPackagePath, Func<bool> isAvailable)
        {

            // Add the /nuget/Packages(Id='',Version='') endpoint.
            MockServer.Get.Add(
                builder.GetODataPath(identity),
                request =>
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        if (!isAvailable())
                        {
                            response.StatusCode = 404;
                            return;
                        }

                        var packagePath = getPackagePath();
                        var mockResponse = builder.BuildODataResponse(packagePath);

                        response.ContentType = mockResponse.ContentType;
                        MockServer.SetResponseContent(response, mockResponse.Content);
                    });
                });

            // Add the /nuget/FindPackagesById()?id=''&semVerLevel=2.0.0 endpoint.
            MockServer.Get.Add(
                builder.GetFindPackagesByIdPath(identity.Id),
                request =>
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        var packagePaths = new List<string>();

                        if (isAvailable())
                        {
                            packagePaths.Add(getPackagePath());
                        }

                        var mockResponse = builder.BuildFindPackagesByIdResponse(packagePaths);

                        response.ContentType = mockResponse.ContentType;
                        MockServer.SetResponseContent(response, mockResponse.Content);
                    });
                });

            // Add the registration index.
            MockServer.Get.Add(
                builder.GetRegistrationIndexPath(identity.Id),
                request =>
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        if (!isAvailable())
                        {
                            response.StatusCode = 404;
                            return;
                        }

                        var mockResponse = builder.BuildRegistrationIndexResponse(MockServer.Uri, new PackageIdentity[] { identity });

                        response.ContentType = mockResponse.ContentType;
                        MockServer.SetResponseContent(response, mockResponse.Content);
                    });
                });

            // Add the flat index
            MockServer.Get.Add(
                builder.GetFlatIndexPath(identity.Id),
                request =>
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        if (!isAvailable())
                        {
                            response.StatusCode = 404;
                            return;
                        }

                        var mockResponse = builder.BuildFlatIndex(identity.Version);

                        response.ContentType = mockResponse.ContentType;
                        MockServer.SetResponseContent(response, mockResponse.Content);
                    });
                });

            // Add the .nupkg download.
            Func<HttpListenerRequest, object> downloadAction = request =>
            {
                return new Action<HttpListenerResponse>(response =>
                {
                    if (!isAvailable())
                    {
                        response.StatusCode = 404;
                        return;
                    }

                    var packagePath = getPackagePath();
                    var mockResponse = builder.BuildDownloadResponse(packagePath);

                    response.ContentType = mockResponse.ContentType;
                    MockServer.SetResponseContent(response, mockResponse.Content);
                });
            };

            MockServer.Get.Add(builder.GetFlatDownloadPath(identity), downloadAction);
            MockServer.Get.Add(builder.GetDownloadPath(identity), downloadAction);
        }

        private void InitializeFiles()
        {
            WorkingPath = Path.Combine(TestDirectory, "working");
            Directory.CreateDirectory(WorkingPath);

            GlobalPackagesPath = Path.Combine(TestDirectory, "globalPackagesFolder");
            Directory.CreateDirectory(GlobalPackagesPath);

            IsolatedHttpCachePath = Path.Combine(TestDirectory, "httpCache");
            Directory.CreateDirectory(IsolatedHttpCachePath);

            InputPackagesPath = Path.Combine(TestDirectory, "packages");
            Directory.CreateDirectory(InputPackagesPath);

            PackageAVersionAPath = MakeTestPackage(InputPackagesPath, PackageIdentityA, $"{PackageIdentityA}.a.nupkg", "a.txt");
            PackageAVersionBPath = MakeTestPackage(InputPackagesPath, PackageIdentityA, $"{PackageIdentityA}.b.nupkg", "b.txt");
            PackageBPath = MakeTestPackage(InputPackagesPath, PackageIdentityB, $"{PackageIdentityB}.nupkg", "c.txt");

            CurrentPackageAPath = PackageAVersionAPath;

            PackagesConfigPath = Path.Combine(WorkingPath, "packages.config");
            ProjectJsonPath = Path.Combine(WorkingPath, "project.json");
            ProjectPath = Path.Combine(WorkingPath, "project.csproj");

            OutputPackagesPath = Path.Combine(WorkingPath, "packages");

            Directory.CreateDirectory(OutputPackagesPath);
        }

        internal void CreateNuGetConfig(string workingDirectory, string source)
        {
            string nugetConfigContent =
                $@"<?xml version='1.0' encoding='utf-8'?>
<configuration>
  <packageSources>
    <clear />
    <add key=""http-feed"" value=""{source}"" allowInsecureConnections=""true""/>
  </packageSources>
  <packageSourceMapping>
    <clear />
  </packageSourceMapping>
</configuration>";
            var fileName = NuGet.Configuration.Settings.DefaultSettingsFileName;

            File.WriteAllText(Path.Combine(workingDirectory, fileName), nugetConfigContent);
        }

        private string MakeTestPackage(string repositoryPath, PackageIdentity identity, string packageFileName, string contentFileName)
        {
            var directory = Path.Combine(repositoryPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(directory);

            File.WriteAllBytes(Path.Combine(directory, contentFileName), new byte[0]);

            var assemblyFileName = "assembly.dll";
            File.WriteAllBytes(Path.Combine(directory, assemblyFileName), new byte[0]);

            var packageBuilder = new Packaging.PackageBuilder();

            packageBuilder.Id = identity.Id;
            packageBuilder.Version = identity.Version;
            packageBuilder.AddFiles(directory, contentFileName, contentFileName);
            packageBuilder.AddFiles(directory, assemblyFileName, $"lib/{PackageFramework.GetShortFolderName()}/assembly.dll");
            packageBuilder.Authors.Add("NuGet");
            packageBuilder.Description = "A test package";

            var destination = Path.Combine(repositoryPath, packageFileName);
            using (var destinationStream = new FileStream(destination, FileMode.Create, FileAccess.Write))
            {
                packageBuilder.Save(destinationStream);
            }

            Directory.Delete(directory, true);

            return destination;
        }

        public void WritePackagesConfig(PackageIdentity packageIdentity)
        {
            var content = $@"<packages>
  <package id=""{packageIdentity.Id}"" version=""{packageIdentity.Version}"" targetFramework=""{PackageFramework.GetShortFolderName()}"" />
</packages>";

            File.WriteAllText(PackagesConfigPath, content);
        }

        public void WriteProjectJson(PackageIdentity packageIdentity)
        {
            var content = $@"{{
  ""dependencies"": {{
    ""{packageIdentity.Id}"": ""{packageIdentity.Version}""
  }},
  ""frameworks"": {{
    ""{PackageFramework.GetShortFolderName()}"": {{}}
  }}
}}";

            File.WriteAllText(ProjectJsonPath, content);
        }

        public void WriteProject()
        {
            var content = Util.GetCSProjXML("project");

            File.WriteAllText(ProjectPath, content);
        }

        public async Task AddToGlobalPackagesFolderAsync(PackageIdentity identity, string packagePath)
        {
            using (var fileStream = new FileStream(packagePath, FileMode.Open, FileAccess.Read))
            {
                using (await GlobalPackagesFolderUtility.AddPackageAsync(
                    source: null,
                    packageIdentity: identity,
                    packageStream: fileStream,
                    globalPackagesFolder: GlobalPackagesPath,
                    parentId: Guid.Empty,
                    clientPolicyContext: null,
                    logger: NullLogger.Instance,
                    token: CancellationToken.None))
                {
                }
            }
        }

        public void AddPackageToHttpCache(PackageIdentity identity, string packagePath)
        {
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = InitializeHttpCacheResult(identity, sourceCacheContext);

                Directory.CreateDirectory(Path.GetDirectoryName(result.CacheFile));

                File.Delete(result.CacheFile);

                File.Copy(packagePath, result.CacheFile);
            }
        }

        public bool IsPackageInHttpCache(PackageIdentity identity)
        {
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = InitializeHttpCacheResult(identity, sourceCacheContext);

                return File.Exists(result.CacheFile);
            }
        }

        private HttpCacheResult InitializeHttpCacheResult(PackageIdentity identity, SourceCacheContext sourceCacheContext)
        {
            return HttpCacheUtility.InitializeHttpCacheResult(
                NuGetExe.GetHttpCachePath(this),
                new Uri(CurrentSource),
                $"nupkg_{identity.Id}.{identity.Version}",
                HttpSourceCacheContext.Create(sourceCacheContext, retryCount: 0));
        }

        public string GetPackagePathInGlobalPackagesFolder(PackageIdentity identity)
        {
            using (var result = GlobalPackagesFolderUtility.GetPackage(identity, GlobalPackagesPath))
            {
                if (result == null)
                {
                    return null;
                }

                var resolver = new VersionFolderPathResolver(GlobalPackagesPath);
                return resolver.GetInstallPath(identity.Id, identity.Version);
            }
        }

        public string GetPackagePathInOutputDirectory(PackageIdentity identity)
        {
            var project = new FolderNuGetProject(OutputPackagesPath);

            var path = project.GetInstalledPath(identity);

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return path;
        }

        public bool IsPackageInGlobalPackagesFolder(PackageIdentity identity)
        {
            return GetPackagePathInGlobalPackagesFolder(identity) != null;
        }

        public bool IsPackageInOutputDirectory(PackageIdentity identity)
        {
            return GetPackagePathInOutputDirectory(identity) != null;
        }

        public bool IsPackageAVersionA(string packagePath)
        {
            var path = Path.Combine(packagePath, "a.txt");
            return File.Exists(path);
        }

        public bool IsPackageAVersionB(string packagePath)
        {
            var path = Path.Combine(packagePath, "b.txt");
            return File.Exists(path);
        }

        public void ClearHttpCache()
        {
            NuGetExe.ClearHttpCache(this);
        }

        public CommandRunnerResult Execute(string args)
        {
            return NuGetExe.Execute(this, args);
        }

        public string FinishArguments(string args)
        {
            args += $" -Source {CurrentSource}";

            if (NoCache)
            {
                args += " -NoCache";
            }

            if (DirectDownload)
            {
                args += " -DirectDownload";
            }

            return args;
        }
    }
}
