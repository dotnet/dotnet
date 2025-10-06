// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.FuncTest
{
    public class RetryRunnerTest
    {
        private readonly ITestOutputHelper _output;

        public RetryRunnerTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void RunWithRetries_WhenNoException_ShouldReturnResult()
        {
            // Arrange
            int maxRetries = 3;
            int runCount = 0;
            Func<int> func = () =>
            {
                runCount++;
                return 42;
            };

            // Act
            int result = RetryRunner.RunWithRetries<int, Exception>(func, maxRetries, _output);

            // Assert
            result.Should().Be(42);
            runCount.Should().Be(1);
        }

        [Fact]
        public void RunWithRetries_OnException_ShouldRetry()
        {
            // Arrange
            int maxRetries = 1;
            int runCount = 0;
            Func<int> func = () =>
            {
                runCount++;
                if (runCount < maxRetries + 1)
                {
                    throw new InvalidOperationException("Simulated exception");
                }
                return 42;
            };

            // Act
            int result = RetryRunner.RunWithRetries<int, InvalidOperationException>(func, maxRetries, _output);

            // Assert
            result.Should().Be(42);
            runCount.Should().Be(2); // Includes initial attempt
        }

        [Fact]
        public void RunWithRetries_OnSuccess_ShouldNotRetry()
        {
            // Arrange
            int maxRetries = 1;
            int runCount = 0;
            Func<int> func = () =>
            {
                runCount++;
                if (runCount < maxRetries + 1)
                {
                    throw new InvalidOperationException("Simulated exception");
                }
                return 42;
            };

            // Act
            int result = RetryRunner.RunWithRetries<int, InvalidOperationException>(func, maxRetries, _output);

            // Assert
            result.Should().Be(42);
            runCount.Should().Be(2); // Only initial attempt
        }

        [Fact]
        public void RunWithRetries_WhenMaxRetriesIsZero_ShouldNotRetry()
        {
            // Arrange
            int maxRetries = 0;
            int runCount = 0;
            Func<int> func = () =>
            {
                runCount++;
                throw new InvalidOperationException("Simulated exception");
            };

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => RetryRunner.RunWithRetries<int, Exception>(func, maxRetries, _output));
            runCount.Should().Be(1);
        }

        [Fact]
        public void RunWithRetries_NonSpecifiedException_ShouldThrow()
        {
            // Arrange
            int runCount = 0;
            Func<int> func = () =>
            {
                runCount++;
                throw new ArgumentException("Simulated exception");
            };

            // Act & Assert
            Action act = () => RetryRunner.RunWithRetries<int, InvalidOperationException>(func);
            act.Should().Throw<ArgumentException>(); // Should not catch and retry ArgumentException
            runCount.Should().Be(1);
        }
    }
}
