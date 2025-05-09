// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.PackageManagement.UI.Utility;
using NuGet.ProjectManagement;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Utility
{
    public class RestoreBarLoggerTests
    {
        [Fact]
        public void Constructor_NullParameterNuGetProjectContext_Throws()
        {
            // Arrange
            INuGetProjectContext? nuGetProjectContext = null;

            // Act
            Action act = () => new RestoreBarLogger(nuGetProjectContext!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("nuGetProjectContext");
        }

        [Fact]
        public async Task LogAsync_InvokesNuGetProjectContextLog_Always()
        {
            // Arrange
            var mockNuGetProjectContext = new Mock<INuGetProjectContext>();
            var message = Mock.Of<ILogMessage>();
            var restoreBarLogger = new RestoreBarLogger(mockNuGetProjectContext.Object);

            // Act
            await restoreBarLogger.LogAsync(message);

            // Assert
            mockNuGetProjectContext.Verify(nuGetProjectContext => nuGetProjectContext.Log(It.IsAny<ILogMessage>()), Times.Once());
        }

        [Fact]
        public void Log_InvokesNuGetProjectContextLog_Always()
        {
            // Arrange
            var mockNuGetProjectContext = new Mock<INuGetProjectContext>();
            var message = Mock.Of<ILogMessage>();
            var restoreBarLogger = new RestoreBarLogger(mockNuGetProjectContext.Object);

            // Act
            restoreBarLogger.Log(message);

            // Assert
            mockNuGetProjectContext.Verify(nuGetProjectContext => nuGetProjectContext.Log(It.IsAny<ILogMessage>()), Times.Once());
        }

        [Fact]
        public void LogError_InvokesNuGetProjectContextLog_Always()
        {
            // Arrange
            var mockNuGetProjectContext = new Mock<INuGetProjectContext>();
            var message = Mock.Of<ILogMessage>();
            var restoreBarLogger = new RestoreBarLogger(mockNuGetProjectContext.Object);

            // Act
            restoreBarLogger.LogError(data: "Message should be logged");

            // Assert
            mockNuGetProjectContext.Verify(nuGetProjectContext => nuGetProjectContext.Log(It.IsAny<ILogMessage>()), Times.Once());
        }

        [Fact]
        public void LogWarning_InvokesNuGetProjectContextLog_Always()
        {
            // Arrange
            var mockNuGetProjectContext = new Mock<INuGetProjectContext>();
            var message = Mock.Of<ILogMessage>();
            var restoreBarLogger = new RestoreBarLogger(mockNuGetProjectContext.Object);

            // Act
            restoreBarLogger.LogWarning(data: "Message should be logged");

            // Assert
            mockNuGetProjectContext.Verify(nuGetProjectContext => nuGetProjectContext.Log(It.IsAny<ILogMessage>()), Times.Once());
        }

        [Fact]
        public void LogInformation_InvokesNuGetProjectContextLog_Never()
        {
            // Arrange
            var mockNuGetProjectContext = new Mock<INuGetProjectContext>();
            var restoreBarLogger = new RestoreBarLogger(mockNuGetProjectContext.Object);

            // Act
            restoreBarLogger.LogInformation(data: "Message which should be ignored");

            // Assert
            mockNuGetProjectContext.Verify(nuGetProjectContext => nuGetProjectContext.Log(It.IsAny<ILogMessage>()), Times.Never());
        }

        [Fact]
        public void LogInformationSummary_InvokesNuGetProjectContextLog_Never()
        {
            // Arrange
            var mockNuGetProjectContext = new Mock<INuGetProjectContext>();
            var restoreBarLogger = new RestoreBarLogger(mockNuGetProjectContext.Object);

            // Act
            restoreBarLogger.LogInformationSummary(data: "Message which should be ignored");

            // Assert
            mockNuGetProjectContext.Verify(nuGetProjectContext => nuGetProjectContext.Log(It.IsAny<ILogMessage>()), Times.Never());
        }

        [Fact]
        public void LogDebug_InvokesNuGetProjectContextLog_Never()
        {
            // Arrange
            var mockNuGetProjectContext = new Mock<INuGetProjectContext>();
            var restoreBarLogger = new RestoreBarLogger(mockNuGetProjectContext.Object);

            // Act
            restoreBarLogger.LogDebug(data: "Message which should be ignored");

            // Assert
            mockNuGetProjectContext.Verify(nuGetProjectContext => nuGetProjectContext.Log(It.IsAny<ILogMessage>()), Times.Never());
        }


        [Fact]
        public void LogMinimal_InvokesNuGetProjectContextLog_Never()
        {
            // Arrange
            var mockNuGetProjectContext = new Mock<INuGetProjectContext>();
            var restoreBarLogger = new RestoreBarLogger(mockNuGetProjectContext.Object);

            // Act
            restoreBarLogger.LogMinimal(data: "Message which should be ignored");

            // Assert
            mockNuGetProjectContext.Verify(nuGetProjectContext => nuGetProjectContext.Log(It.IsAny<ILogMessage>()), Times.Never());
        }

        [Fact]
        public void LogVerbose_InvokesNuGetProjectContextLog_Never()
        {
            // Arrange
            var mockNuGetProjectContext = new Mock<INuGetProjectContext>();
            var restoreBarLogger = new RestoreBarLogger(mockNuGetProjectContext.Object);

            // Act
            restoreBarLogger.LogVerbose(data: "Message which should be ignored");

            // Assert
            mockNuGetProjectContext.Verify(nuGetProjectContext => nuGetProjectContext.Log(It.IsAny<ILogMessage>()), Times.Never());
        }
    }
}
