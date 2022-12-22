// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class RequestFactoryTests
    {
        [Fact]
        public void RequestFactory_RestorePackagesArgRelativeToCwd()
        {
            // If a packages argument is provided, GetEffectiveGlobalPackagesFolder() should ignore
            // the provided root path and any configuration information and resolve relative to the
            // current working directory.

            // Arrange
            var globalPackagesFolder = "MyPackages";
            var restoreArgs = new RestoreArgs()
            {
                GlobalPackagesFolder = globalPackagesFolder
            };

            // Act
            var resolvedGlobalPackagesFolder = restoreArgs.GetEffectiveGlobalPackagesFolder("C:\\Dummy", null);

            // Assert
            var expectedResolvedGlobalPackagesFolder = Path.GetFullPath(globalPackagesFolder);
            Assert.Equal(expectedResolvedGlobalPackagesFolder, resolvedGlobalPackagesFolder);
        }

        [Fact]
        public async Task RequestFactory_ReadDGSpec()
        {
            // Arrange
            var cache = new RestoreCommandProvidersCache();
            var provider = new DependencyGraphFileRequestProvider(cache);

            using (var workingDir = TestDirectory.Create())
            {
                var dgSpec = Path.Combine(workingDir, "project.dg");

                Directory.CreateDirectory(Path.GetDirectoryName(dgSpec));

                File.WriteAllText(dgSpec, DGSpec);

                var context = new RestoreArgs();
                using (var cacheContext = new SourceCacheContext())
                {
                    context.CacheContext = cacheContext;
                    context.Log = new TestLogger();

                    // Act
                    var supports = await provider.Supports(dgSpec);
                    Assert.Equal(true, supports);

                    var requests = await provider.CreateRequests(dgSpec, context);
                    Assert.Equal(1, requests.Count);
                }
            }
        }
        private static string DGSpec = @"

{
  ""format"": 1,
  ""restore"": {
    ""C:\\Users\\ConsoleApp1\\ConsoleApp1.csproj"": {}
  },
  ""projects"": {
    ""C:\\Users\\ConsoleApp1\\ConsoleApp1.csproj"": {
      ""version"": ""1.0.0"",
      ""restore"": {
        ""projectUniqueName"": ""C:\\Users\\ConsoleApp1\\ConsoleApp1.csproj"",
        ""projectName"": ""ConsoleApp1"",
        ""projectPath"": ""C:\\Users\\ConsoleApp1\\ConsoleApp1.csproj"",
        ""packagesPath"": ""C:\\Users\\.nuget\\packages\\"",
        ""outputPath"": ""C:\\Users\\Documents\\Visual Studio 2017\\Projects\\ConsoleApp7\\ConsoleApp1\\obj\\"",
        ""projectStyle"": ""PackageReference"",
        ""configFilePaths"": [
        ],
        ""fallbackFolders"": [
          ""C:\\Users\\.dotnet\\NuGetFallbackFolder""
        ],
        ""originalTargetFrameworks"": [
          ""netcoreapp1.1""
        ],
        ""sources"": {
          ""https://api.nuget.org/v3/index.json"": {},
          ""C:\\Program Files (x86)\\Microsoft SDKs\\NuGetPackages\\"": {}
        },
        ""frameworks"": {
          ""netcoreapp1.1"": {
            ""projectReferences"": {}
          }
        }
      },
      ""frameworks"": {
        ""netcoreapp1.1"": {
          ""dependencies"": {
            ""Microsoft.NETCore.App"": {
              ""target"": ""Package"",
              ""version"": ""1.1.1""
            },
            ""jQuery"": {
              ""target"": ""Package"",
              ""version"": ""3.1.1""
            }
          }
        }
      }
    }
  }
}";
    }
}
