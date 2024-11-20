// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NuGet.CommandLine.XPlat;
using Test.Utility;
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

        public static IEnumerable<string> HelpCommands => new List<string>
        {
            "add",
            "config",
            "delete",
            "disable",
            "enable",
            "list",
            "locals",
            "push",
            "remove",
            "sign",
            "trust",
            "update",
            "verify",
            "why"
        };

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
            var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);
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
            var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);
            Console.SetOut(originalConsoleOut);

            // Assert
            var output = consoleOutput.ToString();
            Assert.DoesNotContain("Usage", output);
            Assert.Equal(1, exitCode);
        }

        [Fact]
        public void MainInternal_ShowsHelp()
        {
            // Arrange
            var originalConsoleOut = Console.Out;
            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            var log = new TestCommandOutputLogger(_testOutputHelper);

            // Act
            var exitCode = Program.MainInternal(Array.Empty<string>(), log, TestEnvironmentVariableReader.EmptyInstance);
            Console.SetOut(originalConsoleOut);

            // Assert
            var output = consoleOutput.ToString();

            var commandPattern = @"^\s{2}(\w+)\s{2,}"; // Matches lines starting with two spaces, a word (command), followed by at least two spaces
            IEnumerable<string> matches = Regex.Matches(output, commandPattern, RegexOptions.Multiline).Select(m => m.ToString().Trim());

            Assert.Equal(HelpCommands, matches);
            Assert.Equal(0, exitCode);
        }
    }
}
