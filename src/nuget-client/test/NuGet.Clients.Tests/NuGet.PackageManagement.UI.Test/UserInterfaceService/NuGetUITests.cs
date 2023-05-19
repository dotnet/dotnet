// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.UI.Utility;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using StreamJsonRpc;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    [Collection(MockedVS.Collection)]
    public class NuGetUITests : IDisposable
    {
        private readonly TestDirectory _testDirectory;

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

        private NuGetUI CreateNuGetUI()
        {
            return CreateNuGetUI(Mock.Of<INuGetUILogger>(), Mock.Of<INuGetUILogger>());
        }

        private NuGetUI CreateNuGetUI(INuGetUILogger defaultLogger, INuGetUILogger projectLogger)
        {
            var uiContext = CreateNuGetUIContext();

            return new NuGetUI(
                Mock.Of<ICommonOperations>(),
                new NuGetUIProjectContext(
                    Mock.Of<ICommonOperations>(),
                    projectLogger,
                    Mock.Of<ISourceControlManagerProvider>()),
                defaultLogger,
                uiContext);
        }

        private NuGetUIContext CreateNuGetUIContext()
        {
            var sourceRepositoryProvider = Mock.Of<ISourceRepositoryProvider>();
            var packageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                Mock.Of<ISettings>(),
                _testDirectory.Path);

            return new NuGetUIContext(
                Mock.Of<IServiceBroker>(),
                Mock.Of<IReconnectingNuGetSearchService>(),
                Mock.Of<IVsSolutionManager>(),
                new NuGetSolutionManagerServiceWrapper(),
                packageManager,
                new UIActionEngine(
                    sourceRepositoryProvider,
                    packageManager,
                    Mock.Of<INuGetLockService>()),
                Mock.Of<IPackageRestoreManager>(),
                Mock.Of<IOptionsPageActivator>(),
                Mock.Of<IUserSettingsManager>(),
                new NuGetSourcesServiceWrapper(),
                Mock.Of<ISettings>());
        }
    }
}
