// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.Shell.TableControl;
using Moq;
using NuGet.SolutionRestoreManager.ErrorListFixers;
using NuGet.VisualStudio;
using Test.Utility.Threading;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    [Collection(DispatcherThreadCollection.CollectionName)]
    public class NuGetCPMErrorListEntryFixerTests
    {
        public NuGetCPMErrorListEntryFixerTests(DispatcherThreadFixture fixture)
        {
            Assumes.Present(fixture);

            NuGetUIThreadHelper.SetCustomJoinableTaskFactory(fixture.JoinableTaskFactory);
        }

        [Theory]
        [InlineData("NU1507")]
        public void CanFix_WithSupportedNuGetCPMCode_ReturnsTrue(string code)
        {
            // Arrange
            NuGetCPMErrorListEntryFixer fixer = new();
            ITableEntryHandle entry = ErrorListEntryTestUtility.CreateEntry(code);

            // Act
            bool result = fixer.CanFix(entry);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TryFix_WithNonMatchingEntry_ReturnsFalse()
        {
            // Arrange
            Mock<IResolveSupplyChainSecurityService> service = new();
            NuGetCPMErrorListEntryFixer fixer = new()
            {
                ResolveSupplyChainSecurityService = new Lazy<IResolveSupplyChainSecurityService>(() => service.Object),
            };
            ITableEntryHandle entry = ErrorListEntryTestUtility.CreateEntry("NU1508");

            // Act
            bool result = fixer.TryFix(entry);

            // Assert
            Assert.False(result);
            service.Verify(
                s => s.LaunchResolveAsync(
                    It.IsAny<ResolveSupplyChainSecuritySource>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Never());
        }

        [Fact]
        public void TryFix_WithMatchingEntryAndMissingService_ReturnsFalse()
        {
            // Arrange
            NuGetCPMErrorListEntryFixer fixer = new();
            ITableEntryHandle entry = ErrorListEntryTestUtility.CreateEntry("NU1507");

            // Act
            bool result = fixer.TryFix(entry);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("NU1507")]
        public void TryFix_WithMatchingEntryAndAvailableService_LaunchesErrorListResolve(string code)
        {
            // Arrange
            using ManualResetEventSlim serviceInvoked = new();
            Mock<IResolveSupplyChainSecurityService> service = new();
            service
                .Setup(s => s.LaunchResolveAsync(
                    source: ResolveSupplyChainSecuritySource.ErrorList,
                    prompt: $"Resolve {code} by reviewing my NuGet supply chain security configuration.",
                    cancellationToken: It.IsAny<CancellationToken>()))
                .Callback(() => serviceInvoked.Set())
                .Returns(Task.CompletedTask);
            NuGetCPMErrorListEntryFixer fixer = new()
            {
                ResolveSupplyChainSecurityService = new Lazy<IResolveSupplyChainSecurityService>(() => service.Object),
            };
            ITableEntryHandle entry = ErrorListEntryTestUtility.CreateEntry(code);

            // Act
            bool result = fixer.TryFix(entry);

            // Assert
            Assert.True(result);
            Assert.True(serviceInvoked.Wait(TimeSpan.FromSeconds(5)));
            service.Verify(
                s => s.LaunchResolveAsync(
                    source: ResolveSupplyChainSecuritySource.ErrorList,
                    prompt: $"Resolve {code} by reviewing my NuGet supply chain security configuration.",
                    cancellationToken: It.IsAny<CancellationToken>()),
                Times.Once());
        }
    }
}
