// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.FuncTest
{
    public class CommandRunnerTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CommandRunnerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Run_AlwaysCaptureStdout()
        {
            // Arrange
            var tasks = Enumerable
                .Range(0, 8)
                .Select(taskIndex =>
                {
                    return Task.Run(() =>
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            VerifyWithCommandRunner();
                        }

                        return true;
                    });
                })
                .ToList();

            // Act
            var successes = await Task.WhenAll(tasks);

            // Assert
            Assert.All(successes, Assert.True);
        }

        [Fact]
        public void Run_CanGetExitCode()
        {
            string fileName;
            string args;

            if (RuntimeEnvironmentHelper.IsWindows)
            {
                fileName = Environment.GetEnvironmentVariable("COMSPEC");
                args = "/c exit 16";
            }
            else
            {
                fileName = "/bin/bash";
                args = "-c \"exit 16\"";
            }

            CommandRunnerResult result = CommandRunner.Run(
                fileName,
                arguments: args,
                timeOutInMilliseconds: 10000,
                testOutputHelper: _testOutputHelper);

            result.ExitCode.Should().Be(16);
        }

        [Fact]
        public void Run_CanWriteToStandardInput()
        {
            string fileName;
            string args;

            if (RuntimeEnvironmentHelper.IsWindows)
            {
                fileName = Environment.GetEnvironmentVariable("COMSPEC");
                args = "/k";
            }
            else
            {
                fileName = "/bin/bash";
                args = "";
            }

            CommandRunnerResult result = CommandRunner.Run(
                fileName,
                arguments: args,
                inputAction: (writer) =>
                {
                    writer.WriteLine("echo Hello & exit 0");
                },
                timeOutInMilliseconds: 5000,
                testOutputHelper: _testOutputHelper);

            result.Output.Should().Contain("Hello");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(32)]
        [InlineData(64)]
        public void Run_DoesNotHangWhenReadingLargeStdout(int outputSizeInKilobytes)
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                // Write the line of the desired size.
                var filePath = Path.Combine(testDirectory, "file.txt");
                var expectedLineContent = new string('a', 14);
                var expectedLine = expectedLineContent + "\r\n"; // a line that is 16 bytes long
                var expectedLineCount = (1024 * outputSizeInKilobytes) / 16;
                using (var fileStream = File.OpenWrite(filePath))
                using (var fileWriter = new StreamWriter(fileStream, Encoding.ASCII))
                {
                    for (var i = 0; i < expectedLineCount; i++)
                    {
                        fileWriter.Write(expectedLine);
                    }
                }

                // Run a program that just reads a file to stdout.
                string fileName;
                string args;
                if (RuntimeEnvironmentHelper.IsWindows)
                {
                    fileName = Environment.GetEnvironmentVariable("COMSPEC");
                    args = $"/c type {filePath}";
                }
                else
                {
                    fileName = "/bin/cat";
                    args = filePath;
                }

                // Act
                var result = CommandRunner.Run(
                    fileName,
                    Directory.GetCurrentDirectory(),
                    args,
                    testOutputHelper: _testOutputHelper);

                // Assert
                Assert.Equal(0, result.ExitCode);

                var actualLineCount = 0;
                using (var stringReader = new StringReader(result.Output))
                {
                    string actualLineContent;
                    while ((actualLineContent = stringReader.ReadLine()) != null)
                    {
                        actualLineCount++;
                        Assert.Equal(expectedLineContent, actualLineContent);
                    }
                }

                Assert.Equal(expectedLineCount, actualLineCount);
            }
        }

        [Fact]
        public void Run_TimesOut()
        {
            string fileName;
            string args;

            if (RuntimeEnvironmentHelper.IsWindows)
            {
                fileName = Environment.GetEnvironmentVariable("COMSPEC");
                args = "/c ping 127.0.0.1 -n 10";
            }
            else
            {
                fileName = "/bin/ping";
                args = "127.0.0.1 -c 10";
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            new Action(() => CommandRunner.Run(fileName, arguments: args, timeOutInMilliseconds: 1000, testOutputHelper: _testOutputHelper, timeoutRetryCount: 0))
                .Should().Throw<TimeoutException>();

            stopwatch.Stop();

            stopwatch.Elapsed.TotalSeconds.Should().BeLessThanOrEqualTo(2);
        }

        private void VerifyWithCommandRunner()
        {
            // Arrange
            var expected = Guid.NewGuid().ToString();

            string fileName;
            string args;
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                fileName = Environment.GetEnvironmentVariable("COMSPEC");
                args = $"/c echo {expected}";
            }
            else
            {
                fileName = "/bin/echo";
                args = expected;
            }

            // Act
            var result = CommandRunner.Run(
                fileName,
                Directory.GetCurrentDirectory(),
                args,
                testOutputHelper: _testOutputHelper);

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains(expected, result.Output);
        }
    }
}
