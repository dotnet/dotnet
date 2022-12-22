// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.Protocol;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetMockServerTests
    {
        [Fact]
        public void MockServer_RestorePRVerifySessionId()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var nugetexe = Util.GetNuGetExePath();

                var packageA = new FileInfo(Util.CreateTestPackage("a", "1.0.0", pathContext.PackageSource));
                var packageB = new FileInfo(Util.CreateTestPackage("b", "1.0.0", pathContext.PackageSource));

                var project = SimpleTestProjectContext.CreateNETCore("proj", pathContext.SolutionRoot, NuGetFramework.Parse("net46"));

                project.AddPackageToAllFrameworks(new SimpleTestPackageContext("a", "1.0.0"));
                project.AddPackageToAllFrameworks(new SimpleTestPackageContext("b", "1.0.0"));

                project.Save();

                var ids = new List<string>();

                using (var server = Util.CreateMockServer(new[] { packageA, packageB }))
                {
                    server.RequestObserver = context =>
                    {
                        ids.Add(context.Request.Headers.Get(ProtocolConstants.SessionId));
                    };

                    server.Start();

                    var result = Util.Restore(pathContext, project.ProjectPath, 0, "-Source", server.Uri + "nuget");

                    result.Success.Should().BeTrue();

                    ids.Distinct().Count().Should().Be(1, "all requests should be in the same session");
                    ids.All(s => !string.IsNullOrEmpty(s) && Guid.TryParse(s, out var r)).Should().BeTrue("the values should guids");
                }
            }
        }

        [Fact]
        public void MockServer_RestorePCVerifySessionId()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var nugetexe = Util.GetNuGetExePath();

                var packageA = new FileInfo(Util.CreateTestPackage("a", "1.0.0", pathContext.PackageSource));
                var packageB = new FileInfo(Util.CreateTestPackage("b", "1.0.0", pathContext.PackageSource));

                Util.CreateFile(pathContext.SolutionRoot, "packages.config",
@"<packages>
  <package id=""a"" version=""1.0.0"" targetFramework=""net45"" />
  <package id=""b"" version=""1.0.0"" targetFramework=""net45"" />
</packages>");

                var ids = new List<string>();

                using (var server = Util.CreateMockServer(new[] { packageA, packageB }))
                {
                    server.RequestObserver = context =>
                    {
                        ids.Add(context.Request.Headers.Get(ProtocolConstants.SessionId));
                    };

                    server.Start();

                    var result = Util.Restore(pathContext, Path.Combine(pathContext.SolutionRoot, "packages.config"), 0, "-Source", server.Uri + "nuget");

                    result.Success.Should().BeTrue();

                    // Not all ids will be in the same session, this is due to how cache contexts are shared for packages.config
                    ids.Distinct().Count().Should().BeLessThan(ids.Count(), "Verify some requests share the same cache context and session id.");
                    ids.All(s => !string.IsNullOrEmpty(s) && Guid.TryParse(s, out var r)).Should().BeTrue("the values should guids");
                }
            }
        }
    }
}
