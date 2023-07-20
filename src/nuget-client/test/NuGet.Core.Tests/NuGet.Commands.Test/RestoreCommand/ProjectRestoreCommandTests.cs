// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Repositories;
using NuGet.Test.Utility;
using Test.Utility.Commands;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Commands.Test
{
    public class ProjectRestoreCommandTests
    {
        private readonly TestLogger _logger;
        private readonly RestoreCollectorLogger _collector;

        public ProjectRestoreCommandTests(ITestOutputHelper output)
        {
            _logger = new TestLogger(output);
            _collector = new RestoreCollectorLogger(_logger, hideWarningsAndErrors: false);
        }

        [Fact]
        public async Task WalkDependenciesAsync_WithCancellationToken_ThrowsAsync()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectName = "TestProject";
                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var packageSpec = PackageReferenceSpecBuilder.Create(projectName, projectDirectory)
                    .WithTargetFrameworks(new string[] { "net46", "net47" })
                    .WithPackagesLockFile()
                    .Build();

                var restoreRequest = ProjectTestHelpers.CreateRestoreRequest(packageSpec, pathContext, _logger);

                var projectRestoreRequest = new ProjectRestoreRequest(restoreRequest, packageSpec, restoreRequest.ExistingLockFile, _collector); ;

                var cmd = new ProjectRestoreCommand(projectRestoreRequest);

                // Assert exception
                var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
                {
                    var cts = new CancellationTokenSource();
                    cts.Cancel();
                    // Act: call TryRestoreAsync() that calls WalkDependenciesAsync()
                    await cmd.TryRestoreAsync(
                        projectRange: It.IsAny<LibraryRange>(),
                        frameworkRuntimePairs: new FrameworkRuntimePair[] {
                            new FrameworkRuntimePair(NuGetFramework.Parse("net46"), null),
                            new FrameworkRuntimePair(NuGetFramework.Parse("net47"), null),
                        },
                        userPackageFolder: It.IsAny<NuGetv3LocalRepository>(),
                        fallbackPackageFolders: It.IsAny<IReadOnlyList<NuGetv3LocalRepository>>(),
                        remoteWalker: It.IsAny<RemoteDependencyWalker>(),
                        context: It.IsAny<RemoteWalkContext>(),
                        forceRuntimeGraphCreation: false,
                        token: cts.Token,
                        telemetryActivity: TelemetryActivity.Create(Guid.NewGuid(), "TestTelemetry"),
                        telemetryPrefix: "testTelemetryPrefix");
                });
            }
        }

    }
}
