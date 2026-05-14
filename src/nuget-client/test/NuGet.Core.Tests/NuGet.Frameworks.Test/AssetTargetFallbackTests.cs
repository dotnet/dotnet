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
    }
}
