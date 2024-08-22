using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuGet.Common.Test
{
    public class RestoreLogMessageTests
    {

        [Theory]
        [InlineData(LogLevel.Error, NuGetLogCode.NU1000, "Error string", "net46")]
        [InlineData(LogLevel.Error, NuGetLogCode.NU1000, "Error string", "")]
        [InlineData(LogLevel.Warning, NuGetLogCode.NU1000, "Warning string", "net46")]
        [InlineData(LogLevel.Debug, NuGetLogCode.NU1000, "Debug string", "net46")]
        public void RestoreLogMessage_TestConstructorWithTargetGraph(LogLevel level, NuGetLogCode code, string message, string targetGraph)
        {
            // Arrange & Act
            var logMessage = new RestoreLogMessage(level, code, message, targetGraph);

            // Assert
            Assert.Equal(level, logMessage.Level);
            Assert.Equal(code, logMessage.Code);
            Assert.Equal(message, logMessage.Message);
            Assert.Equal(0, logMessage.StartLineNumber);
            Assert.Equal(0, logMessage.EndLineNumber);
            Assert.Equal(0, logMessage.StartColumnNumber);
            Assert.Equal(0, logMessage.EndColumnNumber);
            if (string.IsNullOrEmpty(targetGraph))
            {
                Assert.Equal(0, logMessage.TargetGraphs.Count);
            }
            else
            {
                Assert.NotNull(logMessage.TargetGraphs);
                Assert.Equal(1, logMessage.TargetGraphs.Count);
                Assert.Equal(targetGraph, logMessage.TargetGraphs.First());
            }
        }

        [Fact]
        public void RestoreLogMessage_TestConstructorWithAllFields()
        {
            // Arrange & Act
            var logMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "test log message")
            {
                TargetGraphs = new List<string>() { "net46", "netcoreapp1.0", "netstandard1.6" },
                WarningLevel = WarningLevel.Severe,
                FilePath = "kung\\fu\\fighting.targets",
                StartLineNumber = 11,
                EndLineNumber = 11,
                StartColumnNumber = 2,
                EndColumnNumber = 10
            };

            // Assert
            Assert.NotNull(logMessage);
            Assert.Equal(LogLevel.Warning, logMessage.Level);
            Assert.Equal(WarningLevel.Severe, logMessage.WarningLevel);
            Assert.Equal(NuGetLogCode.NU1500, logMessage.Code);
            Assert.Equal("kung\\fu\\fighting.targets", logMessage.FilePath);
            Assert.Equal(11, logMessage.StartLineNumber);
            Assert.Equal(11, logMessage.EndLineNumber);
            Assert.Equal(2, logMessage.StartColumnNumber);
            Assert.Equal(10, logMessage.EndColumnNumber);
            Assert.NotNull(logMessage.TargetGraphs);
            Assert.Equal(3, logMessage.TargetGraphs.Count);
            Assert.Equal("test log message", logMessage.Message);
        }

        [Theory]
        [InlineData(LogLevel.Error, NuGetLogCode.NU1000, "Error string")]
        [InlineData(LogLevel.Warning, NuGetLogCode.NU1500, "Warning string")]
        [InlineData(LogLevel.Debug, NuGetLogCode.NU1000, "Debug string")]
        public void RestoreLogMessage_TestConstructorWithoutTargetGraph(LogLevel level, NuGetLogCode code, string message)
        {
            // Arrange & Act
            var logMessage = new RestoreLogMessage(level, code, message);

            // Assert
            Assert.Equal(level, logMessage.Level);
            Assert.Equal(code, logMessage.Code);
            Assert.Equal(message, logMessage.Message);
            Assert.Equal(0, logMessage.StartLineNumber);
            Assert.Equal(0, logMessage.EndLineNumber);
            Assert.Equal(0, logMessage.StartColumnNumber);
            Assert.Equal(0, logMessage.EndColumnNumber);
            Assert.Equal(0, logMessage.TargetGraphs.Count);
        }

        [Theory]
        [InlineData(LogLevel.Error, NuGetLogCode.NU1000, "Error string")]
        [InlineData(LogLevel.Warning, NuGetLogCode.NU1500, "Warning string")]
        [InlineData(LogLevel.Debug, NuGetLogCode.Undefined, "Debug string")]
        public void RestoreLogMessage_TestConstructorWithoutCode(LogLevel level, NuGetLogCode expectedCode, string message)
        {
            // Arrange & Act
            var logMessage = new RestoreLogMessage(level, message);

            // Assert
            Assert.Equal(level, logMessage.Level);
            Assert.Equal(expectedCode, logMessage.Code);
            Assert.Equal(message, logMessage.Message);
            Assert.Equal(0, logMessage.StartLineNumber);
            Assert.Equal(0, logMessage.EndLineNumber);
            Assert.Equal(0, logMessage.StartColumnNumber);
            Assert.Equal(0, logMessage.EndColumnNumber);
            Assert.Equal(0, logMessage.TargetGraphs.Count);
        }


        [Theory]
        [InlineData(NuGetLogCode.NU1500, "Warning string", "packageId", new string[] { "net46" })]
        [InlineData(NuGetLogCode.NU1500, "Warning string", "packageId", new string[] { "net46", "netcoreapp1.0" })]
        public void RestoreLogMessage_TestCreateWarning(NuGetLogCode code, string message, string libraryId, string[] targetGraphs)
        {
            // Arrange & Act
            var logMessage = RestoreLogMessage.CreateWarning(code, message, libraryId, targetGraphs);

            // Assert
            Assert.Equal(LogLevel.Warning, logMessage.Level);
            Assert.Equal(code, logMessage.Code);
            Assert.Equal(message, logMessage.Message);
            Assert.Equal(0, logMessage.StartLineNumber);
            Assert.Equal(0, logMessage.EndLineNumber);
            Assert.Equal(0, logMessage.StartColumnNumber);
            Assert.Equal(0, logMessage.EndColumnNumber);
            targetGraphs.SequenceEqual(logMessage.TargetGraphs);
        }

        [Theory]
        [InlineData(NuGetLogCode.NU1000, "Error string", "packageId", new string[] { "net46" })]
        [InlineData(NuGetLogCode.NU1000, "Error string", "packageId", new string[] { "net46", "netcoreapp1.0" })]
        public void RestoreLogMessage_TestCreateError(NuGetLogCode code, string message, string libraryId, string[] targetGraphs)
        {
            // Arrange & Act
            var logMessage = RestoreLogMessage.CreateError(code, message, libraryId, targetGraphs);

            // Assert
            Assert.Equal(LogLevel.Error, logMessage.Level);
            Assert.Equal(code, logMessage.Code);
            Assert.Equal(message, logMessage.Message);
            Assert.Equal(0, logMessage.StartLineNumber);
            Assert.Equal(0, logMessage.EndLineNumber);
            Assert.Equal(0, logMessage.StartColumnNumber);
            Assert.Equal(0, logMessage.EndColumnNumber);
            targetGraphs.SequenceEqual(logMessage.TargetGraphs);
        }
    }
}
