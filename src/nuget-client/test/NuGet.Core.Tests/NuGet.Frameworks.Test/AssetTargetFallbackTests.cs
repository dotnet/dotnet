// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NuGet.Frameworks.Test
{
    public class AssetTargetFallbackTests
    {
        private static readonly IReadOnlyList<NuGetFramework> SampleFrameworkList = new NuGetFramework[] { new NuGetFramework(".NETCoreApp") };

        [Fact]
        public void Constructor_WithNullFramework_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new AssetTargetFallbackFramework(framework: null!, fallbackFrameworks: SampleFrameworkList));
        }

        [Fact]
        public void Constructor_WithNullFallbacks_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new AssetTargetFallbackFramework(framework: NuGetFramework.AnyFramework, fallbackFrameworks: null!));
        }

        [Fact]
        public void Constructor_WithEmptyFallbacks_Throws()
        {
            Assert.Throws<ArgumentException>(() => new AssetTargetFallbackFramework(framework: NuGetFramework.AnyFramework, fallbackFrameworks: new NuGetFramework[] { }));
        }

        [Theory]
        [InlineData("net5.0")]
        [InlineData("net45")]
        [InlineData("net5.0-windows10.0.6508.1")]
        public void NuGetFrameworkFullComparer_WithAssetTargetFallback_Succeeds(string shortFrameworkName)
        {
            var nugetFramework = NuGetFramework.Parse(shortFrameworkName);
            var assetTargetFallback = new AssetTargetFallbackFramework(nugetFramework, fallbackFrameworks: SampleFrameworkList);
            var comparer = NuGetFrameworkFullComparer.Instance;
            Assert.True(comparer.Equals(nugetFramework, assetTargetFallback));
        }

        [Theory]
        [InlineData("net5.0", "net5.0", true)]
        [InlineData("net45", "net45", true)]
        [InlineData("net5.0-windows10.0.6508.1", "net5.0-windows10.0.6508.1", true)]
        [InlineData("net5.0-windows10.0.6508.1", "net5.0-windows10.0.6508.2", false)]
        public void NuGetFrameworkEquals_WithAssetTargetFallback_Succeeds(string shortFrameworkName, string atfRootFrameworkName, bool equals)
        {
            var nugetFramework = NuGetFramework.Parse(shortFrameworkName);
            var assetTargetFallback = new AssetTargetFallbackFramework(NuGetFramework.Parse(atfRootFrameworkName), fallbackFrameworks: SampleFrameworkList);
            nugetFramework.Equals(assetTargetFallback).Should().Be(equals);
        }

        [Theory]
        [InlineData("net5.0")]
        [InlineData("net45")]
        [InlineData("net5.0-windows10.0.6508.1")]
        public void AssetTargetFrameworkEquals_WithNonAssetTargetFallbackFramework_Succeeds(string shortFrameworkName)
        {
            var nugetFramework = NuGetFramework.Parse(shortFrameworkName);
            var assetTargetFallback = new AssetTargetFallbackFramework(nugetFramework, fallbackFrameworks: SampleFrameworkList);
            Assert.False(assetTargetFallback.Equals((object)nugetFramework));
        }

        [Fact]
        public void Equals_WithDualCompatibilityFramework_DifferentFromPlainFramework()
        {
            var net6Win = NuGetFramework.Parse("net6.0-windows7.0");
            var native = NuGetFramework.Parse("native");
            var fallbacks = new NuGetFramework[] { NuGetFramework.Parse("net461") };

            var atfPlain = new AssetTargetFallbackFramework(net6Win, fallbacks);
            var atfDcf = new AssetTargetFallbackFramework(
                new DualCompatibilityFramework(net6Win, native), fallbacks);

            atfPlain.Equals(atfDcf).Should().BeFalse(
                because: "ATF wrapping DualCompatibilityFramework(net6.0, native) must differ from ATF wrapping plain net6.0");
            atfDcf.Equals(atfPlain).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_WithDualCompatibilityFramework_DifferentFromPlainFramework()
        {
            var net6Win = NuGetFramework.Parse("net6.0-windows7.0");
            var native = NuGetFramework.Parse("native");
            var fallbacks = new NuGetFramework[] { NuGetFramework.Parse("net461") };

            var atfPlain = new AssetTargetFallbackFramework(net6Win, fallbacks);
            var atfDcf = new AssetTargetFallbackFramework(
                new DualCompatibilityFramework(net6Win, native), fallbacks);

            atfPlain.GetHashCode().Should().NotBe(atfDcf.GetHashCode(),
                because: "hash codes must differ to avoid cache collisions in LockFileBuilderCache");
        }

        [Fact]
        public void Equals_TwoDualCompatibilityFrameworks_WithSameSecondary_AreEqual()
        {
            var net6Win = NuGetFramework.Parse("net6.0-windows7.0");
            var native = NuGetFramework.Parse("native");
            var fallbacks = new NuGetFramework[] { NuGetFramework.Parse("net461") };

            var atf1 = new AssetTargetFallbackFramework(
                new DualCompatibilityFramework(net6Win, native), fallbacks);
            var atf2 = new AssetTargetFallbackFramework(
                new DualCompatibilityFramework(net6Win, native), fallbacks);

            atf1.Equals(atf2).Should().BeTrue();
            atf1.GetHashCode().Should().Be(atf2.GetHashCode());
        }
    }
}
