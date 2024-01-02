// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetMockServerTests
    {
        [Theory]
        [InlineData(ProjectStyle.PackageReference)]
        [InlineData(ProjectStyle.PackagesConfig)]
        public void MockServer_VerifySessionId(ProjectStyle projectStyle)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var packageA = new FileInfo(Util.CreateTestPackage("a", "1.0.0", pathContext.PackageSource));
                var packageB = new FileInfo(Util.CreateTestPackage("b", "1.0.0", pathContext.PackageSource));

                string inputPath = null;

                switch (projectStyle)
                {
                    case ProjectStyle.PackagesConfig:
                        inputPath = Util.CreateFile(
                            pathContext.SolutionRoot,
                            "packages.config",
@"<packages>
  <package id=""a"" version=""1.0.0"" targetFramework=""net45"" />
  <package id=""b"" version=""1.0.0"" targetFramework=""net45"" />
</packages>");
                        break;

                    case ProjectStyle.PackageReference:
                        var project = SimpleTestProjectContext.CreateNETCore("proj", pathContext.SolutionRoot, NuGetFramework.Parse("net46"));

                        project.AddPackageToAllFrameworks(new SimpleTestPackageContext("a", "1.0.0"), new SimpleTestPackageContext("b", "1.0.0"));

                        project.Save();

                        inputPath = project.ProjectPath;
                        break;
                }

                var ids = new ConcurrentBag<string>();

                using (var server = Util.CreateMockServer(new[] { packageA, packageB }))
                {
                    server.RequestObserver = context =>
                    {
                        ids.Add(context.Request.Headers.Get(ProtocolConstants.SessionId));
                    };

                    server.Start();

                    var result = Util.Restore(pathContext, inputPath, 0, "-Source", server.Uri + "nuget");

                    result.Success.Should().BeTrue();
                    ids.All(s => !string.IsNullOrEmpty(s) && Guid.TryParse(s, out var r)).Should().BeTrue("the values should guids");
                    ids.GroupBy(i => i).Any(i => i.Count() > 1).Should().BeTrue("At least one session should have been reused");
                }
            }
        }
    }
}
