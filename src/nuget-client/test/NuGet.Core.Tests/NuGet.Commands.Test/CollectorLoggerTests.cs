// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Moq;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class RestoreCollectorLoggerTests
    {

        [Fact]
        public void CollectorLogger_DoesNotPassLogMessagesToInnerLoggerByDefault()
        {

            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Debug, hideWarningsAndErrors: false);

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug"));
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose"));
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information"));
            collector.Log(new RestoreLogMessage(LogLevel.Warning, "Warning"));
            collector.Log(new RestoreLogMessage(LogLevel.Error, "Error"));

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotPassLogCallsToInnerLoggerByDefault()
        {
            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Debug, hideWarningsAndErrors: false);

            // Act
            collector.Log(LogLevel.Debug, "Debug");
            collector.Log(LogLevel.Verbose, "Verbose");
            collector.Log(LogLevel.Information, "Information");
            collector.Log(LogLevel.Warning, "Warning");
            collector.Log(LogLevel.Error, "Error");

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotPassLogMessagesToInnerLoggerByDefaultWithHideErrorsAndWarnings()
        {

            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Debug, hideWarningsAndErrors: true);

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug"));
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose"));
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information"));
            collector.Log(new RestoreLogMessage(LogLevel.Warning, "Warning"));
            collector.Log(new RestoreLogMessage(LogLevel.Error, "Error"));

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Never());
        }

        [Fact]
        public void CollectorLogger_DoesNotPassLogCallsToInnerLoggerByDefaultWithHideErrorsAndWarnings()
        {
            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Debug, hideWarningsAndErrors: true);

            // Act
            collector.Log(LogLevel.Debug, "Debug");
            collector.Log(LogLevel.Verbose, "Verbose");
            collector.Log(LogLevel.Information, "Information");
            collector.Log(LogLevel.Warning, "Warning");
            collector.Log(LogLevel.Error, "Error");

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Never());
        }


        [Fact]
        public void CollectorLogger_DoesNotPassLogCallsToInnerLoggerByDefaultWithFilePath()
        {
            // Arrange
            var projectPath = @"kung/fu/fighting.csproj";
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Debug, hideWarningsAndErrors: false);
            collector.ApplyRestoreInputs(
                new PackageSpec()
                {
                    RestoreMetadata = new ProjectRestoreMetadata()
                    {
                        ProjectPath = projectPath
                    }
                });

            // Act
            collector.Log(LogLevel.Debug, "Debug");
            collector.Log(LogLevel.Verbose, "Verbose");
            collector.Log(LogLevel.Information, "Information");
            collector.Log(LogLevel.Warning, "Warning");
            collector.Log(LogLevel.Error, "Error");

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once(), filePath: projectPath);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once(), filePath: projectPath);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once(), filePath: projectPath);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once(), filePath: projectPath);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once(), filePath: projectPath);
        }

        [Fact]
        public void CollectorLogger_PassesLogMessagesToInnerLoggerOnlyWithShouldDisplay()
        {

            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Debug, hideWarningsAndErrors: true);

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = false });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = false });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, "Warning") { ShouldDisplay = false });
            collector.Log(new RestoreLogMessage(LogLevel.Error, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_PassesLogMessagesToInnerLoggerWithShouldDisplay()
        {

            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Debug, hideWarningsAndErrors: true);

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_PassesLogMessagesToInnerLoggerWithNoShouldDisplayAndHideWarningsAndErrors()
        {

            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Debug, hideWarningsAndErrors: false);

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, "Warning") { ShouldDisplay = false });
            collector.Log(new RestoreLogMessage(LogLevel.Error, "Error") { ShouldDisplay = false });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_PassesLogMessagesToInnerLoggerWithShouldDisplayAndHideWarningsAndErrors()
        {

            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Debug, hideWarningsAndErrors: false);

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_PassesLogMessagesToInnerLoggerWithLessVerbosity()
        {
            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Verbose, hideWarningsAndErrors: true);

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_PassesLogMessagesToInnerLoggerWithLeastVerbosity()
        {
            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object, LogLevel.Error, hideWarningsAndErrors: false);

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }


        [Fact]
        public void CollectorLogger_CollectsErrors()
        {
            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object);

            // Act
            var errorsStart = collector.Errors.ToArray();
            collector.LogError("ErrorA");
            var errorsA = collector.Errors.ToArray();
            collector.LogError("ErrorB");
            collector.LogError("ErrorC");
            var errorsAbc = collector.Errors.ToArray();
            var errordEnd = collector.Errors.ToArray();

            // Assert
            Assert.Empty(errorsStart);
            Assert.Equal(new[] { "ErrorA" }, errorsA.Select(e => e.Message));
            Assert.Equal(new[] { "ErrorA", "ErrorB", "ErrorC" }, errorsAbc.Select(e => e.Message));
            Assert.Equal(new[] { "ErrorA", "ErrorB", "ErrorC" }, errordEnd.Select(e => e.Message));
        }

        [Fact]
        public void CollectorLogger_CollectsWarnings()
        {
            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object);

            // Act
            var warningsStart = collector.Errors.ToArray();
            collector.LogWarning("WarningA");
            var warningA = collector.Errors.ToArray();
            collector.LogWarning("WarningB");
            collector.LogWarning("WarningC");
            var warningAbc = collector.Errors.ToArray();
            var warningsEnd = collector.Errors.ToArray();

            // Assert
            Assert.Empty(warningsStart);
            Assert.Equal(new[] { "WarningA" }, warningA.Select(e => e.Message));
            Assert.Equal(new[] { "WarningA", "WarningB", "WarningC" }, warningAbc.Select(e => e.Message));
            Assert.Equal(new[] { "WarningA", "WarningB", "WarningC" }, warningsEnd.Select(e => e.Message));
        }

        [Fact]
        public void CollectorLogger_CollectsOnlyErrorsAndWarnings()
        {
            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object);

            // Act
            var warningsStart = collector.Errors.ToArray();
            collector.LogWarning("WarningA");
            collector.LogDebug("Debug");
            var warningA = collector.Errors.ToArray();
            collector.LogInformation("Information");
            collector.LogWarning("WarningB");
            collector.LogWarning("WarningC");
            var warningAbc = collector.Errors.ToArray();
            var warningsEnd = collector.Errors.ToArray();

            // Assert
            Assert.Empty(warningsStart);
            Assert.Equal(new[] { "WarningA" }, warningA.Select(e => e.Message));
            Assert.Equal(new[] { "WarningA", "WarningB", "WarningC" }, warningAbc.Select(e => e.Message));
            Assert.Equal(new[] { "WarningA", "WarningB", "WarningC" }, warningsEnd.Select(e => e.Message));
        }

        [Fact]
        public void CollectorLogger_DoesNotCollectNonErrorAndNonWarnings()
        {
            // Arrange
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object);

            // Act
            collector.LogDebug("Debug");
            collector.LogVerbose("Verbose");
            collector.LogInformation("Information");
            var errors = collector.Errors.ToArray();

            // Assert
            Assert.Empty(errors);
        }

        [Fact]
        public void CollectorLogger_LogsWarningsAsErrorsErrorsForProjectWideWarnAsErrorSet()
        {

            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    null,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1601, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once(), NuGetLogCode.NU1601);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning As Error: Warning", Times.Once(), NuGetLogCode.NU1500);
        }


        [Fact]
        public void CollectorLogger_LogsWarningsAsErrorsForProjectAllWarningsAsErrors()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = true;
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    null,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning As Error: Warning", Times.Once());
        }

        [Fact]
        public void CollectorLogger_LogsWarningsAsErrorsForProjectAllWarningsAsErrorsAndWarnAsErrorSet()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = true;
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    null,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1601, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1603, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning As Error: Warning", Times.Exactly(3));
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForProjectWideNoWarnSet()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    null,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificNoWarnSet()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    packageSpecificWarningProperties,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_LogsWarningsForPackageSpecificNoWarnSetButWarningsWithoutLibraryId()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    packageSpecificWarningProperties,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning"));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }


        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificNoWarnSetWithMultipleEntries()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1601, libraryId, targetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1605, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                   new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                   packageSpecificWarningProperties,
                   null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1605, "Warning", libraryId, frameworkString));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificNoWarnSetWithListOfEntries()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.AddRangeOfCodes(new List<NuGetLogCode> { NuGetLogCode.NU1500, NuGetLogCode.NU1601, NuGetLogCode.NU1605 }, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    packageSpecificWarningProperties,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1605, "Warning", libraryId, frameworkString));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificNoWarnSetWithListOfOneEntry()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.AddRangeOfCodes(new List<NuGetLogCode> { NuGetLogCode.NU1500 }, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    packageSpecificWarningProperties,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1605, "Warning", libraryId, frameworkString));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never(), NuGetLogCode.NU1500);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once(), NuGetLogCode.NU1605);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once(), NuGetLogCode.NU1601);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificNoWarnSetForGlobalTfmWithFullTfmMatch()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    packageSpecificWarningProperties,
                    new List<NuGetFramework> { targetFramework })
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificNoWarnSetForGlobalTfmWithPartialTfmMatch()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var netcoreFrameworkString = "netcoreapp1.0";
            var netcoreTargetFramework = NuGetFramework.Parse(netcoreFrameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    packageSpecificWarningProperties,
                    new List<NuGetFramework> { targetFramework, netcoreTargetFramework })
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificNoWarnSetWithMultipleEntriesForGlobalTfmWithFullTfmMatch()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1601, libraryId, targetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1605, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    packageSpecificWarningProperties,
                    new List<NuGetFramework> { targetFramework })
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1605, "Warning", libraryId));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificNoWarnSetWithMultipleEntriesForGlobalTfmWithPartialTfmMatch()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var netcoreFrameworkString = "netcoreapp1.0";
            var netcoreTargetFramework = NuGetFramework.Parse(netcoreFrameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1601, libraryId, targetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1605, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    packageSpecificWarningProperties,
                    new List<NuGetFramework> { targetFramework, netcoreTargetFramework })
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1605, "Warning", libraryId));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once(), NuGetLogCode.NU1500);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once(), NuGetLogCode.NU1601);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Once(), NuGetLogCode.NU1605);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForProjectWideNoWarnSetAndAllWarningsAsErrors()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = true;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    null,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForProjectWideNoWarnSetAndWarnAsErrorSet()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    null,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificNoWarnSetAndWarnAsErrorSet()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    null,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificAndProjectWideNoWarnSet()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = false;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1601, libraryId, targetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1605, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    packageSpecificWarningProperties,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1605, "Warning", libraryId, frameworkString));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        [Fact]
        public void CollectorLogger_DoesNotLogsWarningsForPackageSpecificNoWarnSetAndProjectWideAllWarningsAsErrors()
        {
            // Arrange
            var libraryId = "test_library";
            var frameworkString = "net45";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warningsNotAsErrors = new HashSet<NuGetLogCode>();
            var allWarningsAsErrors = true;
            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1601, libraryId, targetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1605, libraryId, targetFramework);

            var innerLogger = new Mock<ILogger>();
            var collector = new RestoreCollectorLogger(innerLogger.Object)
            {
                ProjectWarningPropertiesCollection = new WarningPropertiesCollection(
                    new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                    packageSpecificWarningProperties,
                    null)
            };

            // Act
            collector.Log(new RestoreLogMessage(LogLevel.Debug, "Debug") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Verbose, "Verbose") { ShouldDisplay = true });
            collector.Log(new RestoreLogMessage(LogLevel.Information, "Information") { ShouldDisplay = true });
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1605, "Warning", libraryId, frameworkString));
            collector.Log(RestoreLogMessage.CreateWarning(NuGetLogCode.NU1107, "Warning", libraryId, frameworkString));
            collector.Log(new RestoreLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "Error") { ShouldDisplay = true });

            // Assert
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Debug, "Debug", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Verbose, "Verbose", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Information, "Information", Times.Once());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Warning, "Warning", Times.Never());
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Warning As Error: Warning", Times.Once(), NuGetLogCode.NU1107);
            VerifyInnerLoggerCalls(innerLogger, LogLevel.Error, "Error", Times.Once());
        }

        private void VerifyInnerLoggerCalls(Mock<ILogger> innerLogger, LogLevel messageLevel, string message, Times times, NuGetLogCode code = NuGetLogCode.Undefined, string filePath = null)
        {
            innerLogger.Verify(x => x.Log(It.Is<RestoreLogMessage>(l =>
            l.Level == messageLevel &&
            l.Message == message &&
            (code == NuGetLogCode.Undefined || l.Code == code) &&
            (filePath == null || filePath == l.FilePath))),
            times);
        }
    }
}
