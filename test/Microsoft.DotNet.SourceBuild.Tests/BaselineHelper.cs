// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing;
using TestUtilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.SourceBuild.Tests
{
    internal partial class BaselineHelper
    {
        private const string SemanticVersionPlaceholder = "x.y.z";
        private const string SemanticVersionPlaceholderMatchingPattern = "*.*.*"; // wildcard pattern used to match on the version represented by the placeholder
        private const string NonSemanticVersionPlaceholder = "x.y";
        private const string NonSemanticVersionPlaceholderMatchingPattern = "*.*"; // wildcard pattern used to match on the version represented by the placeholder
        private const string TargetRidPlaceholder = "banana-rid";
        private const string TargetRidPlaceholderMatchingPattern = "*-*"; // wildcard pattern used to match on the rid represented by the placeholder
        private const string PortableRidPlaceholder = "portable-rid";
        private const string PortableRidPlaceholderMatchingPattern = "*-*"; // wildcard pattern used to match on the rid represented by the placeholder
        private const string SemanticVersionPattern = @"(0|[1-9]\d*)\.(0|[1-9]\d*)(\.(0|[1-9]\d*))+"
            + @"(((?:[-.]((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)))+"
            + @"(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?"
            + @"(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?";

        // Wrapper methods delegating to TestUtilities.BaselineHelper
        public static void CompareEntries(string baselineFileName, IOrderedEnumerable<string> actualEntries)
        {
            string? message = TestUtilities.BaselineHelper.CompareEntries(baselineFileName, actualEntries);
            Assert.True(message == null, message);
        }

        public static void CompareBaselineContents(string baselineFileName, string actualContents, ITestOutputHelper outputHelper, string baselineSubDir = "")
        {
            string? message = TestUtilities.BaselineHelper.CompareBaselineContents(baselineFileName, actualContents, outputHelper, Config.LogsDirectory, baselineSubDir);
            Assert.True(message == null, message);
        }

        public static void CompareFiles(string expectedFilePath, string actualFilePath, ITestOutputHelper outputHelper)
        {
            string? message = TestUtilities.BaselineHelper.CompareFiles(expectedFilePath, actualFilePath, outputHelper);
            Assert.True(message == null, message);
        }

        public static string DiffFiles(string file1Path, string file2Path, ITestOutputHelper outputHelper) =>
            TestUtilities.BaselineHelper.DiffFiles(file1Path, file2Path, outputHelper);

        public static string GetAssetsDirectory() => TestUtilities.BaselineHelper.GetAssetsDirectory();

        public static string GetBaselineFilePath(string baselineFileName, string baselineSubDir = "") =>
            TestUtilities.BaselineHelper.GetBaselineFilePath(baselineFileName, baselineSubDir);

        public static string RemoveRids(string diff, bool isPortable = false) =>
            isPortable ? diff.Replace(Config.PortableRid, PortableRidPlaceholder) : diff.Replace(Config.TargetRid, TargetRidPlaceholder);

        public static string RemoveVersions(string source)
        {
            // Remove version numbers for examples like "roslyn4.1", "net8.0", and "netstandard2.1".
            string pathSeparator = Regex.Escape(Path.DirectorySeparatorChar.ToString());
            string result = Regex.Replace(source, $@"{pathSeparator}(net|roslyn)([1-9][0-9]*)\.[0-9]+{pathSeparator}", match =>
            {
                string wordPart = match.Groups[1].Value;
                return $"{Path.DirectorySeparatorChar}{wordPart}{NonSemanticVersionPlaceholder}{Path.DirectorySeparatorChar}";
            });

            var semanticVersionRegex = GetSemanticVersionRegex();
            return semanticVersionRegex.Replace(result, SemanticVersionPlaceholder);
        }

        /// <summary>
        /// This returns a <see cref="Matcher"/> that can be used to match on a path whose versions and RID have been removed via
        /// <see cref="RemoveVersions(string)"/> and <see cref="RemoveRids(string, bool)"/>
        /// </summary>
        public static Matcher GetFileMatcherFromPath(string path)
        {
            path = path
                .Replace(SemanticVersionPlaceholder, SemanticVersionPlaceholderMatchingPattern)
                .Replace(NonSemanticVersionPlaceholder, NonSemanticVersionPlaceholderMatchingPattern)
                .Replace(TargetRidPlaceholder, TargetRidPlaceholderMatchingPattern)
                .Replace(PortableRidPlaceholder, PortableRidPlaceholderMatchingPattern);
            Matcher matcher = new();
            matcher.AddInclude(path);
            return matcher;
        }

        // Semantic version regex pattern to match semantic versions in paths and configuration strings.
        // Base regex source: https://semver.org/#is-there-a-suggested-regular-expression-regex-to-check-a-semver-string
        // The regex from semver.org has been modified to match versions that are either:
        // 1. Surrounded by path delimiters, periods, or hyphens: 
        //    - Uses lookbehind/lookahead (?<=[./-]) to require these characters before/after the version
        // 2. Enclosed in double quotes (as in JSON/XML strings/elements):
        //    - Uses lookbehind/lookahead (?<="") to match opening/closing quotes
        [GeneratedRegex(@"(?<=[./-])" + SemanticVersionPattern + @"(?=[/.-])|(?<="")" + SemanticVersionPattern + @"(?="")")]
        private static partial Regex GetSemanticVersionRegex();
    }
}
