// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.PackageManagement.Test.NuGetPackageManagerTests
{
    public class ConstructorTests
    {
        [Fact]
        public void Constructor_WithNullISourceRepositoryProvider_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                null,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                null,
                new Mock<IRestoreProgressReporter>().Object
                ));
        }

        [Fact]
        public void Constructor_WithNullISettings_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                null,
                solutionManager.Object,
                new Mock<IDeleteOnRestartManager>().Object,
                new Mock<IRestoreProgressReporter>().Object
                ));
        }

        [Fact]
        public void Constructor_WithNullISolutionManager_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                null,
                new Mock<IDeleteOnRestartManager>().Object,
                new Mock<IRestoreProgressReporter>().Object
                ));
        }

        [Fact]
        public void Constructor_WithNullIDeleteOnRestartManager_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                null,
                new Mock<IRestoreProgressReporter>().Object
                ));
        }

        [Fact]
        public void Constructor_WithNullProgressReporter_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                new Mock<IDeleteOnRestartManager>().Object,
                reporter: null
                ));
        }

        [Fact]
        public void Constructor_With6Arguments_WithNullProgressReporter_DoesNotThrows()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            _ = new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                new Mock<IDeleteOnRestartManager>().Object,
                reporter: null,
                excludeVersion: true
                );
        }

        [Fact]
        public void Constructor_With6Arguments_WithNullISourceRepositoryProvider_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                null,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                null,
                new Mock<IRestoreProgressReporter>().Object,
                excludeVersion: true
                ));
        }

        [Fact]
        public void Constructor_With6Arguments_WithNullISettings_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                null,
                solutionManager.Object,
                new Mock<IDeleteOnRestartManager>().Object,
                new Mock<IRestoreProgressReporter>().Object,
                excludeVersion: true
                ));
        }

        [Fact]
        public void Constructor_With6Arguments_WithNullISolutionManager_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                null,
                new Mock<IDeleteOnRestartManager>().Object,
                new Mock<IRestoreProgressReporter>().Object,
                excludeVersion: true
                ));
        }

        [Fact]
        public void Constructor_With6Arguments_WithNullIDeleteOnRestartManager_Throws()
        {
            var solutionManager = new Mock<ISolutionManager>();
            solutionManager.Setup(e => e.SolutionDirectory).Returns(@"C:\");
            Assert.Throws<ArgumentNullException>(() => new NuGetPackageManager(
                new Mock<ISourceRepositoryProvider>().Object,
                new Mock<ISettings>().Object,
                solutionManager.Object,
                null,
                new Mock<IRestoreProgressReporter>().Object,
                excludeVersion: true
                ));
        }
    }
}
