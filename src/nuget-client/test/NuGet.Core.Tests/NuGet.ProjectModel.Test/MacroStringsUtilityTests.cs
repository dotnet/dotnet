// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class MacroStringsUtilityTests
    {
        [Theory]
        [InlineData("C:\\Users\\me", "C:\\Users\\me", "$(UserProfile)", "$(UserProfile)")]
        [InlineData("C:\\Users\\me\\path", "C:\\Users\\me", "$(UserProfile)", "$(UserProfile)\\path")]
        [InlineData("C:\\Users\\me\\.nuget\\packages", "C:\\Users\\me", "$(UserProfile)", "$(UserProfile)\\.nuget\\packages")]
        [InlineData("C:\\Users\\Me\\.nuget\\packages", "C:\\Users\\me", "$(UserProfile)", "C:\\Users\\Me\\.nuget\\packages")]
        [InlineData("C:\\Users\\me\\.nuget\\packages", "C:\\Users\\Me", "$(UserProfile)", "C:\\Users\\me\\.nuget\\packages")]
        public void ApplyMacro_VariousTestCases(string testString, string macroValue, string macroName, string expected)
        {
            var actual = MacroStringsUtility.ApplyMacro(testString, macroValue, macroName, StringComparison.Ordinal);
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("C:\\Users\\me", "C:\\Users\\me", "$(UserProfile)", "$(UserProfile)")]
        [InlineData("C:\\Users\\me\\path", "C:\\Users\\me", "$(UserProfile)", "$(UserProfile)\\path")]
        [InlineData("C:\\Users\\me\\.nuget\\packages", "C:\\Users\\me", "$(UserProfile)", "$(UserProfile)\\.nuget\\packages")]
        [InlineData("C:\\Users\\Me\\.nuget\\packages", "C:\\Users\\me", "$(UserProfile)", "C:\\Users\\Me\\.nuget\\packages")]
        [InlineData("C:\\Users\\me\\.nuget\\packages", "C:\\Users\\Me", "$(UserProfile)", "C:\\Users\\me\\.nuget\\packages")]
        public void Extract_VariousTestCases(string expected, string macroValue, string macroName, string testString)
        {
            var actual = MacroStringsUtility.ExtractMacro(testString, macroValue, macroName);
            actual.Should().Be(expected);
        }

        [Fact]
        public void ApplyMacros_AppliesEachMacroIndividually()
        {
            List<string> testStrings = new()
            {
                "C:\\Users\\me",
                "C:\\Users\\me\\path",
                "C:\\Users\\me\\.nuget\\packages",
                "C:\\Users\\Me\\.nuget\\packages",
            };

            List<string> expected = new()
            {
                "$(UserProfile)",
                "$(UserProfile)\\path",
                "$(UserProfile)\\.nuget\\packages",
                "C:\\Users\\Me\\.nuget\\packages",
            };

            MacroStringsUtility.ApplyMacros(testStrings, "C:\\Users\\me", "$(UserProfile)", StringComparison.Ordinal);
            testStrings.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ExtractMacros_ExtractsEachMacroIndividually()
        {
            List<string> testStrings = new()
            {
                "$(UserProfile)",
                "$(UserProfile)\\path",
                "$(UserProfile)\\.nuget\\packages",
                "C:\\Users\\Me\\.nuget\\packages",
            };

            List<string> expected = new()
            {
                "C:\\Users\\me",
                "C:\\Users\\me\\path",
                "C:\\Users\\me\\.nuget\\packages",
                "C:\\Users\\Me\\.nuget\\packages",
            };

            MacroStringsUtility.ExtractMacros(testStrings, "C:\\Users\\me", "$(UserProfile)");
            testStrings.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ApplyMacros_WithIgnoreCaseComparer_AppliesEachMacroIndividually()
        {
            List<string> testStrings = new()
            {
                "C:\\Users\\me",
                "C:\\Users\\me\\path",
                "C:\\Users\\me\\.nuget\\packages",
                "C:\\Users\\Me\\.nuget\\packages",
            };

            List<string> expected = new()
            {
                "$(UserProfile)",
                "$(UserProfile)\\path",
                "$(UserProfile)\\.nuget\\packages",
                "$(UserProfile)\\.nuget\\packages",
            };

            MacroStringsUtility.ApplyMacros(testStrings, "c:\\users\\me", "$(UserProfile)", StringComparison.OrdinalIgnoreCase);
            testStrings.Should().BeEquivalentTo(expected);
        }
    }
}
