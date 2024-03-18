// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Microsoft.DotNet.VersionTools.Automation;
using Moq;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using System.IO;
using System;
using Xunit;

namespace Microsoft.DotNet.VersionTools.Cli.Tests;

public class VersionTrimmingOperationTests
{
    private const string ASSETS_DIRECTORY = @"\assets\directory";
    private const string SEARCH_PATTERN = "*.nupkg";

    [Fact]
    public void TestRemoveVersionFromFileNames()
    {
        var nupkgInfoFactory = new Mock<INupkgInfoFactory>();
        nupkgInfoFactory.Setup(m => m.CreateNupkgInfo(It.IsAny<string>()))
            .Returns(new NupkgInfo(new PackageIdentity("id", new NuGetVersion("8.0.0-dev"))));

        var fileProxy = new Mock<IFileProxy>();
        fileProxy.Setup(m => m.Move(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

        var directoryProxy = new Mock<IDirectoryProxy>();
        directoryProxy.Setup(m => m.Exists(ASSETS_DIRECTORY)).Returns(true);
        directoryProxy.Setup(m => m.GetFiles(ASSETS_DIRECTORY, SEARCH_PATTERN, SearchOption.AllDirectories))
            .Returns(new string[] {
                ASSETS_DIRECTORY + @"\package.8.0.0-dev.nupkg",
                ASSETS_DIRECTORY + @"\SubDir\package.8.0.0-dev.nupkg" });
        var logger = new Mock<ILogger>();
        logger.Setup(m => m.WriteMessage(It.IsAny<string>())).Verifiable();

        var operation = new VersionTrimmingOperation(
            new VersionTrimmingOperation.Context
            {
                NupkgInfoFactory = nupkgInfoFactory.Object,
                DirectoryProxy = directoryProxy.Object,
                FileProxy = fileProxy.Object,
                Logger = logger.Object,

                AssetsDirectory = ASSETS_DIRECTORY,
                SearchPattern = SEARCH_PATTERN,
                Recursive = true
            });

        operation.Execute().Should().Be(IOperation.ExitCode.Success);

        fileProxy.Verify(v => v.Move(
                ASSETS_DIRECTORY + @"\package.8.0.0-dev.nupkg",
                ASSETS_DIRECTORY + @"\package.nupkg"), Times.Exactly(1));
        fileProxy.Verify(v => v.Move(
                ASSETS_DIRECTORY + @"\SubDir\package.8.0.0-dev.nupkg",
                ASSETS_DIRECTORY + @"\SubDir\package.nupkg"), Times.Exactly(1));
        logger.Verify(v => v.WriteMessage(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public void TestInvalidAssetsDirectory()
    {
        var nupkgInfoFactory = new Mock<INupkgInfoFactory>();
        var fileProxy = new Mock<IFileProxy>();

        var directoryProxy = new Mock<IDirectoryProxy>();
        directoryProxy.Setup(m => m.Exists(ASSETS_DIRECTORY)).Returns(false);

        var logger = new Mock<ILogger>();
        logger.Setup(m => m.WriteError(It.IsAny<string>())).Verifiable();

        var operation = new VersionTrimmingOperation(
            new VersionTrimmingOperation.Context
            {
                NupkgInfoFactory = nupkgInfoFactory.Object,
                DirectoryProxy = directoryProxy.Object,
                FileProxy = fileProxy.Object,
                Logger = logger.Object,

                AssetsDirectory = ASSETS_DIRECTORY,
                SearchPattern = SEARCH_PATTERN,
                Recursive = true
            });

        operation.Execute().Should().Be(IOperation.ExitCode.ErrorFileNotFount);
        logger.Verify(v => v.WriteError(It.IsAny<string>()), Times.Exactly(1));
    }

    [Fact]
    public void TestInvalidInputNuGetAsset()
    {
        var nupkgInfoFactory = new Mock<INupkgInfoFactory>();
        nupkgInfoFactory.Setup(m => m.CreateNupkgInfo(It.IsAny<string>())).Throws(new InvalidDataException()).Verifiable();

        var fileProxy = new Mock<IFileProxy>();

        var directoryProxy = new Mock<IDirectoryProxy>();
        directoryProxy.Setup(m => m.Exists(ASSETS_DIRECTORY)).Returns(true);
        directoryProxy.Setup(m => m.GetFiles(ASSETS_DIRECTORY, "*.nupkg", SearchOption.AllDirectories))
            .Returns(new string[] { ASSETS_DIRECTORY + @"\file.nupkg" });

        var logger = new Mock<ILogger>();
        logger.Setup(m => m.WriteError(It.IsAny<string>())).Verifiable();

        var operation = new VersionTrimmingOperation(
            new VersionTrimmingOperation.Context
            {
                NupkgInfoFactory = nupkgInfoFactory.Object,
                DirectoryProxy = directoryProxy.Object,
                FileProxy = fileProxy.Object,
                Logger = logger.Object,

                AssetsDirectory = ASSETS_DIRECTORY,
                SearchPattern = "*.nupkg",
                Recursive = true
            });

        operation.Execute().Should().Be(IOperation.ExitCode.Success);

        nupkgInfoFactory.Verify(v => v.CreateNupkgInfo(
                ASSETS_DIRECTORY + @"\file.nupkg"), Times.Exactly(1));
        logger.Verify(v => v.WriteError(It.IsAny<string>()), Times.Exactly(1));
    }


    [Fact]
    public void TestSeachPatternIncludesNonNuGetAssets()
    {
        var nupkgInfoFactory = new Mock<INupkgInfoFactory>();
        var fileProxy = new Mock<IFileProxy>();

        var directoryProxy = new Mock<IDirectoryProxy>();
        directoryProxy.Setup(m => m.Exists(ASSETS_DIRECTORY)).Returns(true);
        directoryProxy.Setup(m => m.GetFiles(ASSETS_DIRECTORY, "*.json", SearchOption.AllDirectories))
            .Returns(new string[] { ASSETS_DIRECTORY + @"\file.json" });

        var logger = new Mock<ILogger>();

        var operation = new VersionTrimmingOperation(
            new VersionTrimmingOperation.Context
            {
                NupkgInfoFactory = nupkgInfoFactory.Object,
                DirectoryProxy = directoryProxy.Object,
                FileProxy = fileProxy.Object,
                Logger = logger.Object,

                AssetsDirectory = ASSETS_DIRECTORY,
                SearchPattern = "*.json",
                Recursive = true
            });

        Action shouldFail = () => operation.Execute();
        shouldFail.Should().Throw<NotImplementedException>();
    }
}
