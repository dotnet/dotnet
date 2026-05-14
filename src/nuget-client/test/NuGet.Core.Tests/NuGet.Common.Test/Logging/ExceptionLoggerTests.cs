// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using Xunit;

namespace NuGet.Common.Test.Logging
{
    public class ExceptionLoggerTests
    {
        [Fact]
        public void ExceptionLogger_ShowStack_MissingVariable()
        {
            // Arrange
            var tc = new TestContext();

            // Act & Assert
            tc.VerifyShouldShowStack(false);
        }

        [Fact]
        public void ExceptionLogger_ShowStack_NonBoolVariable()
        {
            // Arrange
            var tc = new TestContext();
            tc
                .EnvironmentVariableReader
                .Setup(x => x.GetEnvironmentVariable(It.IsAny<string>()))
                .Returns("not-a-bool");

            // Act & Assert
            tc.VerifyShouldShowStack(false);
        }

        [Fact]
        public void ExceptionLogger_ShowStack_FalseVariable()
        {
            // Arrange
            var tc = new TestContext();
            tc
                .EnvironmentVariableReader
                .Setup(x => x.GetEnvironmentVariable(It.IsAny<string>()))
                .Returns("false");

            // Act & Assert
            tc.VerifyShouldShowStack(false);
        }

        [Fact]
        public void ExceptionLogger_ShowStack_TrueVariable()
        {
            // Arrange
            var tc = new TestContext();
            tc
                .EnvironmentVariableReader
                .Setup(x => x.GetEnvironmentVariable(It.IsAny<string>()))
                .Returns("true");

            // Act & Assert
            tc.VerifyShouldShowStack(true);
        }

        [Fact]
        public void ExceptionLogger_ShowStack_CachesValue()
        {
            // Arrange
            var tc = new TestContext();
            tc
                .EnvironmentVariableReader
                .Setup(x => x.GetEnvironmentVariable(It.IsAny<string>()))
                .Returns("true");

            // Act
            tc.Target = new ExceptionLogger(tc.EnvironmentVariableReader.Object);
            var first = tc.Target.ShowStack;
            tc
                .EnvironmentVariableReader
                .Setup(x => x.GetEnvironmentVariable(It.IsAny<string>()))
                .Returns("false");
            var second = tc.Target.ShowStack;

            // Assert
            Assert.True(first);
            Assert.True(second);
            tc.EnvironmentVariableReader.Verify(
                x => x.GetEnvironmentVariable("NUGET_SHOW_STACK"),
                Times.Once);
            tc.EnvironmentVariableReader.Verify(
                x => x.GetEnvironmentVariable(It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public void ExceptionLogger_ShowStack_RealEnvironment()
        {
            // Arrange
            var instance = ExceptionLogger.Instance;

            // Act & Assert
            var showStack = instance.ShowStack;

            // No assertion because the environment may vary. The point here is that no
            // exception is thrown.
        }

        private class TestContext
        {
            public TestContext()
            {
                EnvironmentVariableReader = new Mock<IEnvironmentVariableReader>();
                Exception = new Exception("Some exception message.");
            }

            public Mock<IEnvironmentVariableReader> EnvironmentVariableReader { get; }
            public Exception Exception { get; }
            public ExceptionLogger? Target { get; set; }

            public void VerifyShouldShowStack(bool expected)
            {
                Target = new ExceptionLogger(EnvironmentVariableReader.Object);

                Assert.Equal(expected, Target.ShowStack);

                EnvironmentVariableReader.Verify(
                    x => x.GetEnvironmentVariable("NUGET_SHOW_STACK"),
                    Times.Once);
                EnvironmentVariableReader.Verify(
                    x => x.GetEnvironmentVariable(It.IsAny<string>()),
                    Times.Once);
            }
        }
    }
}
