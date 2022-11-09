// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.Protocol;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class FeedPackagePruningTests
    {
        [Fact]
        public async Task FeedPackagePruning_GivenThatAV3FeedPrunesAPackageDuringRestoreVerifyRestoreRecoversAsync()
        {
            // Arrange
            using (var server = new MockServer())
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var testLogger = new TestLogger();
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
                var serverRepoPath = Path.Combine(pathContext.WorkingDirectory, "serverPackages");

                var packageX100 = new SimpleTestPackageContext("x", "1.0.0");
                var packageX200 = new SimpleTestPackageContext("x", "2.0.0");

                await SimpleTestPackageUtility.CreatePackagesAsync(
                    serverRepoPath,
                    packageX100,
                    packageX200);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                        "a",
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net45"));
                projectA.AddPackageToAllFrameworks(packageX200);
                solution.Projects.Add(projectA);

                var projectB = SimpleTestProjectContext.CreateNETCore(
                        "b",
                        pathContext.SolutionRoot,
                        NuGetFramework.Parse("net45"));
                projectB.AddPackageToAllFrameworks(packageX100);
                solution.Projects.Add(projectB);

                solution.Create(pathContext.SolutionRoot);

                // Server setup
                var indexJson = Util.CreateIndexJson();
                Util.AddFlatContainerResource(indexJson, server);
                Util.AddRegistrationResource(indexJson, server);

                server.Get.Add("/", request =>
                {
                    return ServerHandlerV3(request, server, indexJson, serverRepoPath);
                });

                server.Start();

                var feedUrl = server.Uri + "index.json";

                // Restore x 2.0.0 and populate the http cache
                var r = Util.Restore(pathContext, projectA.ProjectPath, 0, "-Source", feedUrl);

                // Delete x 1.0.0
                File.Delete(LocalFolderUtility.GetPackageV2(serverRepoPath, packageX100.Identity, testLogger).Path);

                // Act
                // Restore x 1.0.0
                r = Util.Restore(pathContext, projectB.ProjectPath, 0, "-Source", feedUrl);

                var xLib = projectB.AssetsFile.Libraries.SingleOrDefault(e => e.Name == "x");

                // Assert
                Assert.Equal("2.0.0", xLib.Version.ToNormalizedString());
            }
        }

        private Action<HttpListenerResponse> ServerHandlerV3(
            HttpListenerRequest request,
            MockServer server,
            JObject indexJson,
            string repositoryPath)
        {
            try
            {
                var path = server.GetRequestUrlAbsolutePath(request);
                var parts = request.Url.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (path == "/index.json")
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.StatusCode = 200;
                        response.ContentType = "text/javascript";
                        MockServer.SetResponseContent(response, indexJson.ToString());
                    });
                }
                else if (path.StartsWith("/flat/") && path.EndsWith("/index.json"))
                {
                    return new Action<HttpListenerResponse>(response =>
                    {
                        response.ContentType = "text/javascript";

                        var versionsJson = JObject.Parse(@"{ ""versions"": [] }");
                        var array = versionsJson["versions"] as JArray;

                        var id = parts[parts.Length - 2];

                        foreach (var pkg in LocalFolderUtility.GetPackagesV2(repositoryPath, id, new TestLogger()))
                        {
                            array.Add(pkg.Identity.Version.ToNormalizedString());
                        }

                        MockServer.SetResponseContent(response, versionsJson.ToString());
                    });
                }
                else if (path.StartsWith("/flat/") && path.EndsWith(".nupkg"))
                {
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
