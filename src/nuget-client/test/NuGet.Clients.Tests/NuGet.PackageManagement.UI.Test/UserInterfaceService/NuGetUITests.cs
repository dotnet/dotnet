// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.Telemetry;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using StreamJsonRpc;
using Xunit;
using ContractsItemFilter = NuGet.VisualStudio.Internal.Contracts.ItemFilter;

namespace NuGet.PackageManagement.UI.Test
{
    [Collection(MockedVS.Collection)]
    public class NuGetUITests : IDisposable
    {
        private readonly TestDirectory _testDirectory;
        private TelemetryEvent _lastTelemetryEvent;

        public NuGetUITests(GlobalServiceProvider sp)
        {
            sp.Reset();
            _testDirectory = TestDirectory.Create();
        }

        public void Dispose()
        {
            _testDirectory.Dispose();
        }

        [Fact]
        public void ShowError_WhenArgumentIsSignatureExceptionWithNullResults_DoesNotThrow()
        {
            var exception = new SignatureException(message: "a");

            Assert.Null(exception.Results);

            using (NuGetUI ui = CreateNuGetUI())
            {
                ui.ShowError(exception);
            }
        }

        [Fact]
        public void ShowError_WhenArgumentIsRemoteInvocationExceptionForSignatureException_ShowsError()
        {
            var remoteError = new RemoteError(
                typeName: typeof(SignatureException).FullName,
                new LogMessage(LogLevel.Error, message: "a", NuGetLogCode.NU3018),
                new[]
                {
                    new LogMessage(LogLevel.Error, message: "b", NuGetLogCode.NU3019),
                    new LogMessage(LogLevel.Warning, message: "c", NuGetLogCode.NU3027)
                },
                projectContextLogMessage: "d",
                activityLogMessage: "e");
            var exception = new RemoteInvocationException(
                message: "f",
                errorCode: 0,
                errorData: null,
                deserializedErrorData: remoteError);
            var defaultLogger = new Mock<INuGetUILogger>();
            var projectLogger = new Mock<INuGetUILogger>();

            defaultLogger.Setup(x => x.ReportError(It.Is<ILogMessage>(logMessage => ReferenceEquals(logMessage, remoteError.LogMessage))));
            defaultLogger.Setup(x => x.ReportError(It.Is<ILogMessage>(logMessage => ReferenceEquals(logMessage, remoteError.LogMessages[0]))));
            defaultLogger.Setup(x => x.ReportError(It.Is<ILogMessage>(logMessage => ReferenceEquals(logMessage, remoteError.LogMessages[1]))));
            projectLogger.Setup(x => x.Log(It.Is<ILogMessage>(logMessage => ReferenceEquals(logMessage, remoteError.LogMessage))));
            projectLogger.Setup(x => x.Log(It.Is<ILogMessage>(logMessage => ReferenceEquals(logMessage, remoteError.LogMessages[0]))));
            projectLogger.Setup(x => x.Log(It.Is<ILogMessage>(logMessage => ReferenceEquals(logMessage, remoteError.LogMessages[1]))));

            using (NuGetUI ui = CreateNuGetUI(defaultLogger.Object, projectLogger.Object))
            {
                ui.ShowError(exception);

                defaultLogger.VerifyAll();
                projectLogger.VerifyAll();
            }
        }

        [Fact]
        public void ShowError_WhenArgumentIsNotRemoteInvocationException_ShowsError()
        {
            var exception = new DivideByZeroException();
            var defaultLogger = new Mock<INuGetUILogger>();
            var projectLogger = new Mock<INuGetUILogger>();
            const bool indent = false;

            defaultLogger.Setup(
                x => x.ReportError(
                    It.Is<ILogMessage>(
                        logMessage => logMessage.Level == LogLevel.Error
                        && logMessage.Message == ExceptionUtilities.DisplayMessage(exception, indent))));
            projectLogger.Setup(
                x => x.Log(
                    It.Is<MessageLevel>(level => level == MessageLevel.Error),
                    It.Is<string>(message => message == exception.ToString())));

            using (NuGetUI ui = CreateNuGetUI(defaultLogger.Object, projectLogger.Object))
            {
                ui.ShowError(exception);

                defaultLogger.VerifyAll();
                projectLogger.VerifyAll();
            }
        }

        [Fact]
        public void ShowError_WhenArgumentIsRemoteInvocationExceptionForOtherException_ShowsError()
        {
            var remoteException = new ArgumentException(message: "a", new DivideByZeroException(message: "b"));
            var remoteError = RemoteErrorUtility.ToRemoteError(remoteException);
            var exception = new RemoteInvocationException(
                message: "c",
                errorCode: 0,
                errorData: null,
                deserializedErrorData: remoteError);
            var defaultLogger = new Mock<INuGetUILogger>();
            var projectLogger = new Mock<INuGetUILogger>();

            defaultLogger.Setup(
                x => x.ReportError(
                    It.Is<ILogMessage>(logMessage => ReferenceEquals(logMessage, remoteError.LogMessage))));
            projectLogger.Setup(
                x => x.Log(
                    It.Is<MessageLevel>(level => level == MessageLevel.Error),
                    It.Is<string>(message => message == remoteException.ToString())));

            using (NuGetUI ui = CreateNuGetUI(defaultLogger.Object, projectLogger.Object))
            {
                ui.ShowError(exception);

                defaultLogger.VerifyAll();
                projectLogger.VerifyAll();
            }
        }

        [Fact]
        public void LaunchNuGetOptionsDialog_PackageSourceMappingNull_TelemetryNotEmitted()
        {
            // Arrange
            SetupTelemetryListener();
            NuGetUI nuGetUI = CreateNuGetUI();

            // Act
            nuGetUI.LaunchNuGetOptionsDialog(packageSourceMappingActionViewModel: null);

            // Assert
            Assert.Null(_lastTelemetryEvent);
        }

        [Fact]
        public void LaunchNuGetOptionsDialog_PackageSourceMappingDisabled_TelemetryPropertiesMatchState()
        {
            // Arrange
            SetupTelemetryListener();

            ItemFilter currentTab = ItemFilter.All;
            ContractsItemFilter contractsItemFilter = UIUtility.ToContractsItemFilter(currentTab);
            bool isSolution = true;
            IReadOnlyDictionary<string, IReadOnlyList<string>> patterns = ImmutableDictionary.Create<string, IReadOnlyList<string>>();
            Mock<PackageSourceMapping> mockPackageSourceMapping = new(patterns);
            NuGetUI nuGetUI = CreateNuGetUI(Mock.Of<INuGetUILogger>(), Mock.Of<INuGetUILogger>(), currentTab, isSolution, mockPackageSourceMapping);

            var packageSourceMappingActionViewModel = PackageSourceMappingActionViewModel.Create(nuGetUI);

            // Act
            nuGetUI.LaunchNuGetOptionsDialog(packageSourceMappingActionViewModel);

            // Assert
            Assert.False(nuGetUI.UIContext.PackageSourceMapping.IsEnabled);
            Assert.NotNull(_lastTelemetryEvent);
            Assert.Equal(NavigationType.Button, _lastTelemetryEvent[NavigatedTelemetryEvent.NavigationTypePropertyName]);
            Assert.Equal(contractsItemFilter, _lastTelemetryEvent[NavigatedTelemetryEvent.CurrentTabPropertyName]);
            Assert.Equal(isSolution, _lastTelemetryEvent[NavigatedTelemetryEvent.IsSolutionViewPropertyName]);
            Assert.Equal(PackageSourceMappingStatus.Disabled, _lastTelemetryEvent[NavigatedTelemetryEvent.PackageSourceMappingStatusPropertyName]);
        }

        [Fact]
        public void LaunchNuGetOptionsDialog_PackageSourceMappingNotMapped_TelemetryPropertiesMatchState()
        {
            // Arrange
            SetupTelemetryListener();
            // Enable Package Source Mapping by creating at least 1 source and pattern.
            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { "a" } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);
            ItemFilter currentTab = ItemFilter.UpdatesAvailable;
            ContractsItemFilter contractsItemFilter = UIUtility.ToContractsItemFilter(currentTab);
            bool isSolution = false;
            NuGetUI nuGetUI = CreateNuGetUI(Mock.Of<INuGetUILogger>(), Mock.Of<INuGetUILogger>(), currentTab, isSolution, mockPackageSourceMapping);

            var packageSourceMappingActionViewModel = PackageSourceMappingActionViewModel.Create(nuGetUI);

            // Act
            nuGetUI.LaunchNuGetOptionsDialog(packageSourceMappingActionViewModel);

            // Assert
            Assert.True(nuGetUI.UIContext.PackageSourceMapping.IsEnabled);
            Assert.NotNull(_lastTelemetryEvent);
            Assert.Equal(NavigationType.Button, _lastTelemetryEvent[NavigatedTelemetryEvent.NavigationTypePropertyName]);
            Assert.Equal(contractsItemFilter, _lastTelemetryEvent[NavigatedTelemetryEvent.CurrentTabPropertyName]);
            Assert.Equal(isSolution, _lastTelemetryEvent[NavigatedTelemetryEvent.IsSolutionViewPropertyName]);
            Assert.Equal(PackageSourceMappingStatus.NotMapped, _lastTelemetryEvent[NavigatedTelemetryEvent.PackageSourceMappingStatusPropertyName]);
        }

        [Fact]
        public void LaunchNuGetOptionsDialog_PackageSourceMappingIsMapped_TelemetryPropertiesMatchState()
        {
            // Arrange
            SetupTelemetryListener();
            string packageId = "a";
            var dictionary = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { packageId } }
            };
            var patterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(dictionary);
            var mockPackageSourceMapping = new Mock<PackageSourceMapping>(patterns);
            ItemFilter currentTab = ItemFilter.Installed;
            ContractsItemFilter contractsItemFilter = UIUtility.ToContractsItemFilter(currentTab);
            bool isSolution = true;
            NuGetUI nuGetUI = CreateNuGetUI(Mock.Of<INuGetUILogger>(), Mock.Of<INuGetUILogger>(), currentTab, isSolution, mockPackageSourceMapping);

            var packageSourceMappingActionViewModel = PackageSourceMappingActionViewModel.Create(nuGetUI);
            packageSourceMappingActionViewModel.PackageId = packageId;
            var _ = packageSourceMappingActionViewModel.IsPackageMapped; // Emulate the View binding to this property by invoking the getter which initializes the Property's backing field.

            // Act
            nuGetUI.LaunchNuGetOptionsDialog(packageSourceMappingActionViewModel);

            // Assert
            Assert.True(nuGetUI.UIContext.PackageSourceMapping.IsEnabled);
            Assert.NotNull(_lastTelemetryEvent);
            Assert.Equal(NavigationType.Button, _lastTelemetryEvent[NavigatedTelemetryEvent.NavigationTypePropertyName]);
            Assert.Equal(contractsItemFilter, _lastTelemetryEvent[NavigatedTelemetryEvent.CurrentTabPropertyName]);
            Assert.Equal(isSolution, _lastTelemetryEvent[NavigatedTelemetryEvent.IsSolutionViewPropertyName]);
            Assert.Equal(PackageSourceMappingStatus.Mapped, _lastTelemetryEvent[NavigatedTelemetryEvent.PackageSourceMappingStatusPropertyName]);
        }

        private NuGetUI CreateNuGetUI()
        {
            return CreateNuGetUI(Mock.Of<INuGetUILogger>(), Mock.Of<INuGetUILogger>());
        }
        private NuGetUI CreateNuGetUI(INuGetUILogger defaultLogger, INuGetUILogger projectLogger)
        {
            return CreateNuGetUI(defaultLogger, projectLogger, activeFilter: ItemFilter.All, isSolution: false, mockPackageSourceMapping: null);
        }

        private NuGetUI CreateNuGetUI(INuGetUILogger defaultLogger, INuGetUILogger projectLogger, ItemFilter activeFilter, bool isSolution, Mock<PackageSourceMapping> mockPackageSourceMapping)
        {
            var mockNuGetUIContext = new Mock<INuGetUIContext>();

            // Without this Setup, a MessageBox will be shown which could block tests indefinitely.
            mockNuGetUIContext.Setup(_ => _.OptionsPageActivator).Returns(Mock.Of<IOptionsPageActivator>());

            if (mockPackageSourceMapping != null)
            {
                mockNuGetUIContext.Setup(_ => _.PackageSourceMapping).Returns(mockPackageSourceMapping.Object);
            }

            var mockIPackageManagerControlViewModel = new Mock<IPackageManagerControlViewModel>();
            mockIPackageManagerControlViewModel.SetupGet(_ => _.ActiveFilter).Returns(activeFilter);
            mockIPackageManagerControlViewModel.SetupGet(_ => _.IsSolution).Returns(isSolution);

            var nugetUI = new NuGetUI(
                Mock.Of<ICommonOperations>(),
                new NuGetUIProjectContext(
                    Mock.Of<ICommonOperations>(),
                    projectLogger,
                    Mock.Of<ISourceControlManagerProvider>()),
                defaultLogger,
                mockNuGetUIContext.Object,
                mockIPackageManagerControlViewModel.Object);

            return nugetUI;
        }

        private Mock<ITelemetrySession> SetupTelemetryListener()
        {
            var telemetrySession = new Mock<ITelemetrySession>();
            telemetrySession
                .Setup(x => x.PostEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => _lastTelemetryEvent = x);
            var telemetryService = new NuGetVSTelemetryService(telemetrySession.Object);
            TelemetryActivity.NuGetTelemetryService = telemetryService;
            return telemetrySession;
        }
    }
}
