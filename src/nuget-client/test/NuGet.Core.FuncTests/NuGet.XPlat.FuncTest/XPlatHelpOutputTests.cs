// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.CommandLine.XPlat;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    public class XPlatHelpOutputTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XPlatHelpOutputTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public static IEnumerable<object[]> ParseSuccessfully
        {
            get
            {
                yield return new object[] { new string[] { "remove", "source", "SourceDosNotExist" } };
                yield return new object[] { new string[] { "enable", "source", "SourceDosNotExist" } };
                yield return new object[] { new string[] { "disable", "source", "SourceDosNotExist" } };
            }
        }

        public static IEnumerable<object[]> FailParsing
        {
            get
            {
                yield return new object[] { new string[] { "removee", "sources", "source" } };
                yield return new object[] { new string[] { "addd", "sources", "source" } };
                yield return new object[] { new string[] { "enablee", "sources", "source" } };
                yield return new object[] { new string[] { "disablee", "sources", "source" } };
                yield return new object[] { new string[] { "listt", "sources", "source" } };
                yield return new object[] { new string[] { "add", "Test", "Case" } };
            }
        }

        [Theory]
        [MemberData(nameof(FailParsing))]
        public void MainInternal_OnParsingError_ShowsHelp(string[] args)
        {
            // Arrange
            var originalConsoleOut = Console.Out;
            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            var log = new TestCommandOutputLogger(_testOutputHelper);

            // Act
            var exitCode = Program.MainInternal(args.ToArray(), log);
            Console.SetOut(originalConsoleOut);

            // Assert
            var output = consoleOutput.ToString();
            Assert.Contains("Usage", output);
            Assert.Equal(1, exitCode);
        }

        [Theory]
        [MemberData(nameof(ParseSuccessfully))]
        public void MainInternal_OnParsingSuccess_DoesNotShowsHelp(string[] args)
        {
            // Arrange
            var originalConsoleOut = Console.Out;
            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            var log = new TestCommandOutputLogger(_testOutputHelper);

            // Act
            var exitCode = Program.MainInternal(args.ToArray(), log);
            Console.SetOut(originalConsoleOut);

            // Assert
            var output = consoleOutput.ToString();
            Assert.DoesNotContain("Usage", output);
            Assert.Equal(1, exitCode);
        }
    }
}
