// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

namespace NuGet.Versioning.Test
{
    public class VersionRangeFloatParsingTests
    {
        [Fact]
        public void VersionRangeFloatParsing_Prerelease()
        {
            var range = VersionRange.Parse("1.0.0-*");

            Assert.True(range.MinVersion?.IsPrerelease);
        }

        [Theory]
        [InlineData("1.0.0-*", "1.0.0-0")]
        [InlineData("1.0.0-0*", "1.0.0-0")]
        [InlineData("1.0.0--*", "1.0.0--")]
        [InlineData("1.0.0-a-*", "1.0.0-a-")]
        [InlineData("1.0.0-a.*", "1.0.0-a.0")]
        [InlineData("1.*-*", "1.0.0-0")]
        [InlineData("1.0.*-0*", "1.0.0-0")]
        [InlineData("1.0.*--*", "1.0.0--")]
        [InlineData("1.0.*-a-*", "1.0.0-a-")]
        [InlineData("1.0.*-a.*", "1.0.0-a.0")]
        public void VersionRangeFloatParsing_PrereleaseWithNumericOnlyLabelVerifyMinVersion(string rangeString, string expected)
        {
            var range = VersionRange.Parse(rangeString);

            Assert.Equal(expected, range.MinVersion?.ToNormalizedString());
        }

        [Theory]
        [InlineData("1.0.0-0")]
        [InlineData("1.0.0-100")]
        [InlineData("1.0.0-0.0.0.0")]
        [InlineData("1.0.0-0+0-0")]
        public void VersionRangeFloatParsing_PrereleaseWithNumericOnlyLabelVerifySatisfies(string version)
        {
            var range = VersionRange.Parse("1.0.0-*");

            Assert.True(range.Satisfies(NuGetVersion.Parse(version)));
        }

        [Theory]
        [InlineData("1.0.0-a*", "1.0.0-a.0")]
        [InlineData("1.0.0-a*", "1.0.0-a-0")]
        [InlineData("1.0.0-a*", "1.0.0-a")]
        [InlineData("1.0.*-a*", "1.0.0-a")]
        [InlineData("1.*-a*", "1.0.0-a")]
        [InlineData("*-a*", "1.0.0-a")]
        public void VersionRangeFloatParsing_VerifySatisfiesForFloatingRange(string rangeString, string version)
        {
            var range = VersionRange.Parse(rangeString);

            Assert.True(range.Satisfies(NuGetVersion.Parse(version)));
        }

        [Theory]
        [InlineData("1.0.0-*", "0", "")]
        [InlineData("1.0.0-a*", "a", "a")]
        [InlineData("1.0.0-a-*", "a-", "a-")]
        [InlineData("1.0.0-a.*", "a.0", "a.")]
        [InlineData("1.0.0-0*", "0", "0")]
        [InlineData("1.0.*-0*", "0", "0")]
        [InlineData("1.*-0*", "0", "0")]
        [InlineData("*-0*", "0", "0")]
        [InlineData("1.0.*-*", "0", "")]
        [InlineData("1.*-*", "0", "")]
        [InlineData("*-*", "0", "")]
        [InlineData("1.0.*-a*", "a", "a")]
        [InlineData("1.*-a*", "a", "a")]
        [InlineData("*-a*", "a", "a")]
        [InlineData("1.0.*-a-*", "a-", "a-")]
        [InlineData("1.*-a-*", "a-", "a-")]
        [InlineData("*-a-*", "a-", "a-")]
        [InlineData("1.0.*-a.*", "a.0", "a.")]
        [InlineData("1.*-a.*", "a.0", "a.")]
        [InlineData("*-a.*", "a.0", "a.")]
        public void VersionRangeFloatParsing_VerifyReleaseLabels(string rangeString, string versionLabel, string originalLabel)
        {
            var range = VersionRange.Parse(rangeString);

            Assert.Equal(versionLabel, range.Float?.MinVersion.Release);
            Assert.Equal(originalLabel, range.Float?.OriginalReleasePrefix);
        }

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("[1.0.0]")]
        [InlineData("(0.0.0, )")]
        [InlineData("[1.0.0, )")]
        [InlineData("[1.0.0, 2.0.0)")]
        public void VersionRangeFloatParsing_NoFloat(string rangeString)
        {
            var range = VersionRange.Parse(rangeString);

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.0.0"),
                    NuGetVersion.Parse("1.1.0")
                };

            Assert.Equal("1.0.0", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeFloatParsing_FloatPrerelease()
        {
            var range = VersionRange.Parse("1.0.0-*");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.0.0-alpha"),
                    NuGetVersion.Parse("1.0.0-beta")
                };

            Assert.Equal("1.0.0-beta", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeFloatParsing_FloatPrereleaseMatchVersion()
        {
            var range = VersionRange.Parse("1.0.0-*");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.0.0-beta"),
                    NuGetVersion.Parse("1.0.1-omega"),
                };

            Assert.Equal("1.0.0-beta", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeFloatParsing_FloatPrereleasePrefix()
        {
            var range = VersionRange.Parse("1.0.0-beta.*");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.0.0-beta.1"),
                    NuGetVersion.Parse("1.0.0-beta.2"),
                    NuGetVersion.Parse("1.0.0-omega.3"),
                };

            Assert.Equal("1.0.0-beta.2", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Fact]
        public void VersionRangeFloatParsing_FloatPrereleasePrefixSemVerLabelMix()
        {
            var range = VersionRange.Parse("1.0.0-beta.*");

            var versions = new List<NuGetVersion>()
                {
                    NuGetVersion.Parse("1.0.0-beta.1"),
                    NuGetVersion.Parse("1.0.0-beta.2"),
                    NuGetVersion.Parse("1.0.0-beta.a"),
                };

            Assert.Equal("1.0.0-beta.a", range.FindBestMatch(versions)?.ToNormalizedString());
        }

        [Theory]
        [InlineData("[]")]
        [InlineData("[*]")]
        [InlineData("[1.0.0, 1.1.*)")]
        [InlineData("[1.0.0, 2.0.*)")]
        [InlineData("(, 2.*.*)")]
        [InlineData("<1.0.*")]
        [InlineData("<=1.0.*")]
        [InlineData("1.0.0<")]
        [InlineData("1.0.0~")]
        [InlineData("~1.*.*")]
        [InlineData("~*")]
        [InlineData("~")]
        [InlineData("^")]
        [InlineData("^*")]
        [InlineData(">=*")]
        [InlineData("1.*.0")]
        [InlineData("1.*.0-beta-*")]
        [InlineData("1.*.0-beta")]
        [InlineData("1.0.0.0.*")]
        [InlineData("=1.0.*")]
        [InlineData("1.0.0+*")]
        [InlineData("1.0.**")]
        [InlineData("1.0.*-*bla")]
        [InlineData("1.0.*-*bla+*")]
        [InlineData("**")]
        [InlineData("1.0.0-preview.*+blabla")]
        [InlineData("1.0.*--")]
        [InlineData("1.0.*-alpha*+")]
        [InlineData("1.0.*-")]
        public void VersionRangeFloatParsing_Invalid(string rangeString)
        {
            VersionRange? range = null;
            Assert.False(VersionRange.TryParse(rangeString, out range));
        }

        [Theory]
        [InlineData("*")]
        [InlineData("1.*")]
        [InlineData("1.0.*")]
        [InlineData("1.0.0.*")]
        [InlineData("1.0.0.0-beta")]
        [InlineData("1.0.0.0-beta*")]
        [InlineData("1.0.0")]
        [InlineData("1.0")]
        [InlineData("[1.0.*, )")]
        [InlineData("[1.0.0-beta.*, 2.0.0)")]
        [InlineData("1.0.0-beta.*")]
        [InlineData("1.0.0-beta-*")]
        [InlineData("1.0.*-bla*")]
        [InlineData("1.0.*-*")]
        [InlineData("1.0.*-preview.1.*")]
        [InlineData("1.0.*-preview.1*")]
        [InlineData("1.0.0--")]
        [InlineData("1.0.0-bla*")]
        [InlineData("1.0.*--*")]
        [InlineData("1.0.0--*")]
        public void VersionRangeFloatParsing_Valid(string rangeString)
        {
            VersionRange? range = null;
            Assert.True(VersionRange.TryParse(rangeString, out range));
        }

        [Theory]
        [InlineData("1.0.0", "[1.0.0, )")]
        [InlineData("1.0.*", "[1.0.0, )")]
        [InlineData("[1.0.*, )", "[1.0.0, )")]
        [InlineData("[1.*, )", "[1.0.0, )")]
        [InlineData("[1.*, 2.0)", "[1.0.0, 2.0.0)")]
        [InlineData("*", "[0.0.0, )")]
        public void VersionRangeFloatParsing_LegacyEquivalent(string rangeString, string legacyString)
        {
            VersionRange? range = null;
            Assert.True(VersionRange.TryParse(rangeString, out range));

            Assert.Equal(legacyString, range!.ToLegacyString());
        }

        [Theory]
        [InlineData("1.0.0-beta*")]
        [InlineData("1.0.0-beta.*")]
        [InlineData("1.0.0-beta-*")]
        public void VersionRangeFloatParsing_CorrectFloatRange(string rangeString)
        {
            VersionRange? range = null;
            Assert.True(VersionRange.TryParse(rangeString, out range));

            Assert.Equal(rangeString, range!.Float?.ToString());
        }

        [Theory]
        [InlineData("1.0.0;2.0.0", "*", "2.0.0")]
        [InlineData("1.0.0;2.0.0", "0.*", "1.0.0")]
        [InlineData("1.0.0;2.0.0", "[*, )", "2.0.0")]
        [InlineData("1.0.0;2.0.0;3.0.0", "(1.0.*, )", "2.0.0")]
        [InlineData("1.0.0;2.0.0;3.0.0", "(1.0.*, 2.0.0)", null)]
        [InlineData("1.1.0;1.2.0-rc.1;1.2.0-rc.2;2.0.0;3.0.0-beta.1", "*", "2.0.0")]
        [InlineData("1.1.0;1.2.0-rc.1;1.2.0-rc.2;2.0.0;3.0.0-beta.1", "1.*", "1.1.0")]
        [InlineData("1.1.0;1.2.0-rc.1;1.2.0-rc.2;2.0.0;3.0.0-beta.1", "1.2.0-*", "1.2.0-rc.2")]
        [InlineData("1.1.0;1.2.0-rc.1;1.2.0-rc.2;2.0.0;3.0.0-beta.1", "*-*", "3.0.0-beta.1")]
        [InlineData("1.1.0;1.2.0-rc.1;1.2.0-rc.2;2.0.0;3.0.0-beta.1", "1.*-*", "1.2.0-rc.2")]
        [InlineData("1.1.0;1.2.0-rc.1;1.2.0-rc.2;2.0.0;3.0.0-beta.1", "*-rc.*", "2.0.0")]
        [InlineData("1.1.0;1.2.0-rc.1;1.2.0-rc.2;1.2.0-rc1;2.0.0;3.0.0-beta.1", "1.*-rc*", "1.2.0-rc1")]
        [InlineData("1.1.0;1.2.0-rc.1;1.2.0-rc.2;1.2.0-rc1;1.10.0;2.0.0;3.0.0-beta.1", "1.1*-*", "1.10.0")]
        public void VersionRangeFloatParsing_FindsBestMatch(string availableVersions, string declaredRange, string expectedVersion)
        {
            var range = VersionRange.Parse(declaredRange);

            var versions = new List<NuGetVersion>();
            foreach (var version in availableVersions.Split(';'))
            {
                versions.Add(NuGetVersion.Parse(version));
            }

            Assert.Equal(expectedVersion, range.FindBestMatch(versions)?.ToNormalizedString());
        }
    }
}
