// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using Shouldly;
using System.Linq;
using Xunit;

namespace Microsoft.Build.Locator.Tests
{
    public class SemanticVersionParserTests
    {
        [Fact]
        public void TryParseTest_ReleaseVersion()
        {
            var version = "7.0.333";

            var isParsed = SemanticVersionParser.TryParse(version, out var parsedVerion);

            parsedVerion.ShouldNotBeNull();
            isParsed.ShouldBeTrue();
            parsedVerion.Major.ShouldBe(7);
            parsedVerion.Minor.ShouldBe(0);
            parsedVerion.Patch.ShouldBe(333);
            parsedVerion.ReleaseLabels.ShouldBeEmpty();
        }

        [Fact]
        public void TryParseTest_PreviewVersion()
        {
            var version = "8.0.0-preview.6.23329.7";

            var isParsed = SemanticVersionParser.TryParse(version, out var parsedVerion);

            parsedVerion.ShouldNotBeNull();
            isParsed.ShouldBeTrue();
            parsedVerion.Major.ShouldBe(8);
            parsedVerion.Minor.ShouldBe(0);
            parsedVerion.Patch.ShouldBe(0);
            parsedVerion.ReleaseLabels.ShouldBe(new[] { "preview", "6", "23329", "7" });
        }

        [Fact]
        public void TryParseTest_InvalidInput_LeadingZero()
        {
            var version = "0.0-preview.6";

            var isParsed = SemanticVersionParser.TryParse(version, out var parsedVerion);

            Assert.Null(parsedVerion);
            isParsed.ShouldBeFalse();
        }

        [Fact]
        public void TryParseTest_InvalidInput_FourPartsVersion()
        {
            var version = "5.0.3.4";

            var isParsed = SemanticVersionParser.TryParse(version, out var parsedVerion);

            Assert.Null(parsedVerion);
            isParsed.ShouldBeFalse();
        }

        [Fact]
        public void VersionSortingTest_WithPreview()
        {
            var versions = new[] { "7.0.7", "8.0.0-preview.6.23329.7", "8.0.0-preview.3.23174.8" };

            var maxVersion = versions.Select(v => SemanticVersionParser.TryParse(v, out var parsedVerion) ? parsedVerion : null).Max();

            maxVersion.OriginalValue.ShouldBe("8.0.0-preview.6.23329.7");
        }

        [Fact]
        public void VersionSortingTest_ReleaseOnly()
        {
            var versions = new[] { "7.0.7", "3.7.2", "10.0.0" };

            var maxVersion = versions.Max(v => SemanticVersionParser.TryParse(v, out var parsedVerion) ? parsedVerion : null);

            maxVersion.OriginalValue.ShouldBe("10.0.0");
        }

        [Fact]
        public void VersionSortingTest_WithInvalidFolderNames()
        {
            var versions = new[] { "7.0.7", "3.7.2", "dummy", "5.7.8.9" };

            var maxVersion = versions.Max(v => SemanticVersionParser.TryParse(v, out var parsedVerion) ? parsedVerion : null);

            maxVersion.OriginalValue.ShouldBe("7.0.7");
        }

        [Fact]
        public void VersionSortingTest_WithAllInvalidFolderNames()
        {
            var versions = new[] { "dummy", "5.7.8.9" };

            var maxVersion = versions.Max(v => SemanticVersionParser.TryParse(v, out var parsedVerion) ? parsedVerion : null);

            maxVersion.ShouldBeNull();
        }
    }
}
#endif