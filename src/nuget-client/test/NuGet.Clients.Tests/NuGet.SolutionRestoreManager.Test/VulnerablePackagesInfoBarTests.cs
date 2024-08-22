// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.VisualStudio;
using Test.Utility;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    [Collection(MockedVS.Collection)]
    public class VulnerablePackagesInfoBarTests
    {
        public VulnerablePackagesInfoBarTests(GlobalServiceProvider sp)
        {
            sp.Reset();
        }

        [Fact]
        public void Constructor_SubscribesToCloseEvents()
        {
            // Arrange
            TestSolutionManager solutionManager = new TestSolutionManager(string.Empty);

            // Act
            VulnerablePackagesInfoBar infoBar = new VulnerablePackagesInfoBar(solutionManager, new Lazy<IPackageManagerLaunchService>());

            EventHandler e = typeof(TestSolutionManager)
                .GetField(nameof(TestSolutionManager.SolutionClosed), BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(solutionManager) as EventHandler;
            e.Should().NotBeNull();

            Delegate[] subscribers = e.GetInvocationList();

            // Assert
            subscribers.Should().HaveCount(1);
            subscribers[0].Target.Should().BeSameAs(infoBar);
        }

        [Fact]
        public async Task ReportVulnerabilitiesAsync_WhenNoVulnerabilitiesExist_InfoBarIsNotLaunched()
        {
            // Arrange
            TestSolutionManager solutionManager = new TestSolutionManager(string.Empty);
            VulnerablePackagesInfoBar infoBar = new VulnerablePackagesInfoBar(solutionManager, new Lazy<IPackageManagerLaunchService>());

            // Act
            await infoBar.ReportVulnerabilitiesAsync(hasVulnerabilitiesInSolution: false, CancellationToken.None);

            // Assert
            infoBar._infoBarVisible.Should().BeFalse();
            infoBar._wasInfoBarHidden.Should().BeFalse();
            infoBar._wasInfoBarClosed.Should().BeFalse();
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/13391")]
        public async Task OnSolutionClosed_ResetsInfoBarStatusProperties()
        {
            // Arrange
            TestSolutionManager solutionManager = new TestSolutionManager(string.Empty);
            VulnerablePackagesInfoBar infoBar = new VulnerablePackagesInfoBar(solutionManager, new Lazy<IPackageManagerLaunchService>());

            infoBar._infoBarVisible = true;
            infoBar._wasInfoBarHidden = true;
            infoBar._wasInfoBarClosed = true;
            // Act
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            solutionManager.OnSolutionClosed();

            // Assert
            infoBar._infoBarVisible.Should().BeFalse();
            infoBar._wasInfoBarHidden.Should().BeFalse();
            infoBar._wasInfoBarClosed.Should().BeFalse();
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/13391")]
        public async Task OnSolutionClosed_WithInfoBarVisible_ClosesInfoBar()
        {
            // Arrange
            TestSolutionManager solutionManager = new TestSolutionManager(string.Empty);
            VulnerablePackagesInfoBar infoBar = new VulnerablePackagesInfoBar(solutionManager, new Lazy<IPackageManagerLaunchService>());

            infoBar._infoBarVisible = true;
            infoBar._wasInfoBarHidden = true;
            infoBar._wasInfoBarClosed = true;

            Mock<IVsInfoBarUIElement> infoBarUIElement = new Mock<IVsInfoBarUIElement>();


            infoBar._infoBarUIElement = infoBarUIElement.Object;
            // Act
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            solutionManager.OnSolutionClosed();

            // Assert
            infoBar._infoBarVisible.Should().BeFalse();
            infoBar._wasInfoBarHidden.Should().BeFalse();
            infoBar._wasInfoBarClosed.Should().BeFalse();
            infoBarUIElement.Verify(ui => ui.Close(), Times.Once());
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/13391")]
        public async Task OnSolutionClosed_WithInfoBarNotVisible_DoesNotAttemptToCloseInfoBar()
        {
            // Arrange
            TestSolutionManager solutionManager = new TestSolutionManager(string.Empty);
            VulnerablePackagesInfoBar infoBar = new VulnerablePackagesInfoBar(solutionManager, new Lazy<IPackageManagerLaunchService>());

            infoBar._infoBarVisible = false;
            infoBar._wasInfoBarHidden = true;
            infoBar._wasInfoBarClosed = false;

            Mock<IVsInfoBarUIElement> infoBarUIElement = new Mock<IVsInfoBarUIElement>();

            infoBar._infoBarUIElement = infoBarUIElement.Object;
            // Act
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            solutionManager.OnSolutionClosed();

            // Assert
            infoBar._infoBarVisible.Should().BeFalse();
            infoBar._wasInfoBarHidden.Should().BeFalse();
            infoBar._wasInfoBarClosed.Should().BeFalse();
            infoBarUIElement.Verify(ui => ui.Close(), Times.Never());
        }
    }
}
