// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    public class UnresolvedMessagesTests
    {
        [Fact]
        public async Task GivenAnUnresolvedProjectWithExistingProjectVerifyMessage()
        {
            using (var working = TestDirectory.Create())
            {
                var path = Path.Combine(working, "project.csproj");
                File.WriteAllText(path, "test");

                var range = new LibraryRange(path, VersionRange.All, LibraryDependencyTarget.ExternalProject);
                var versions = new List<NuGetVersion>() { NuGetVersion.Parse("1.0.0-beta"), NuGetVersion.Parse("4.4.0-beta.2+test") };

                var message = await GetMessage(range, versions);

                message.Code.Should().Be(NuGetLogCode.NU1105);
                message.LibraryId.Should().Be(path);
                message.Message.Should().Contain($"Unable to find project information for '{path}'");
                message.TargetGraphs.Should().BeEquivalentTo(new[] { "abc" });
                message.Level.Should().Be(LogLevel.Error);
            }
        }

        [Fact]
        public async Task GivenAnUnresolvedProjectWithNotFoundVerifyMessage()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "notfound.csproj");
            var range = new LibraryRange(path, VersionRange.All, LibraryDependencyTarget.ExternalProject);
            var versions = new List<NuGetVersion>() { NuGetVersion.Parse("1.0.0-beta"), NuGetVersion.Parse("4.4.0-beta.2+test") };

            var message = await GetMessage(range, versions);

            message.Code.Should().Be(NuGetLogCode.NU1104);
            message.LibraryId.Should().Be(path);
            message.Message.Should().Contain($"Unable to find project '{path}'");
            message.TargetGraphs.Should().BeEquivalentTo(new[] { "abc" });
            message.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public async Task GivenAnUnresolvedReferenceVerifyMessage()
        {
            var range = new LibraryRange("x", VersionRange.All, LibraryDependencyTarget.Reference);
            var versions = new List<NuGetVersion>() { NuGetVersion.Parse("1.0.0-beta"), NuGetVersion.Parse("4.4.0-beta.2+test") };

            var message = await GetMessage(range, versions);

            message.Code.Should().Be(NuGetLogCode.NU1100);
            message.LibraryId.Should().Be("x");
            message.Message.Should().Contain("Unable to resolve 'reference/x ' for 'abc'");
            message.TargetGraphs.Should().BeEquivalentTo(new[] { "abc" });
            message.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public async Task GivenAnUnresolvedPackageWithPreRelVersionsAndPreRelRangeVerifyMessage()
        {
            var range = new LibraryRange("x", VersionRange.Parse("[4.0.0-beta, 5.0.0]"), LibraryDependencyTarget.Package);
            var versions = new List<NuGetVersion>() { NuGetVersion.Parse("1.0.0-beta"), NuGetVersion.Parse("4.4.0-beta.2+test") };

            var message = await GetMessage(range, versions);

            message.Code.Should().Be(NuGetLogCode.NU1102);
            message.LibraryId.Should().Be("x");
            message.Message.Should().Contain("Unable to find package x with version (>= 4.0.0-beta && <= 5.0.0)");
            message.Message.Should().Contain("Found 2 version(s) in http://nuget.org/a/ [ Nearest version: 4.4.0-beta.2 ]");
            message.TargetGraphs.Should().BeEquivalentTo(new[] { "abc" });
            message.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public async Task GivenAnUnresolvedPackageWithPreRelVersionsVerifyMessage()
        {
            var range = new LibraryRange("x", VersionRange.Parse("[4.0.0, 5.0.0]"), LibraryDependencyTarget.Package);
            var versions = new List<NuGetVersion>() { NuGetVersion.Parse("1.0.0-beta"), NuGetVersion.Parse("4.4.0-beta.2+test") };

            var message = await GetMessage(range, versions);

            message.Code.Should().Be(NuGetLogCode.NU1103);
            message.LibraryId.Should().Be("x");
            message.Message.Should().Contain("Unable to find a stable package x with version (>= 4.0.0 && <= 5.0.0)");
            message.Message.Should().Contain("Found 2 version(s) in http://nuget.org/a/ [ Nearest version: 4.4.0-beta.2 ]");
            message.TargetGraphs.Should().BeEquivalentTo(new[] { "abc" });
            message.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public async Task GivenAnUnresolvedPackageWithVersionsVerifyMessage()
        {
            var range = new LibraryRange("x", VersionRange.Parse("[4.0.0]"), LibraryDependencyTarget.Package);
            var versions = new List<NuGetVersion>() { NuGetVersion.Parse("1.0.0"), NuGetVersion.Parse("5.0.0") };

            var message = await GetMessage(range, versions);

            message.Code.Should().Be(NuGetLogCode.NU1102);
            message.LibraryId.Should().Be("x");
            message.Message.Should().Contain("Unable to find package x with version (= 4.0.0)");
            message.Message.Should().Contain("Found 2 version(s) in http://nuget.org/a/ [ Nearest version: 5.0.0 ]");
            message.TargetGraphs.Should().BeEquivalentTo(new[] { "abc" });
            message.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public async Task GivenAnUnresolvedPackageWithNoVersionsAndMultipleSourcesVerifyMessage()
        {
            var range = new LibraryRange("x", LibraryDependencyTarget.Package);
            var versions = new List<NuGetVersion>();

            var token = CancellationToken.None;
            var logger = new TestLogger();
            var provider1 = GetProvider("http://nuget.org/a/", versions);
            var provider2 = GetProvider("http://nuget.org/b/", versions);
            var cacheContext = new Mock<SourceCacheContext>();
            var remoteLibraryProviders = new List<IRemoteDependencyProvider>() { provider1.Object, provider2.Object };
            var targetGraphName = "abc";

            var message = await UnresolvedMessages.GetMessageAsync(targetGraphName, range, remoteLibraryProviders, false, remoteLibraryProviders, cacheContext.Object, logger, token);

            message.Code.Should().Be(NuGetLogCode.NU1101);
            message.LibraryId.Should().Be("x");
            message.Message.Should().Be("Unable to find package x. No packages exist with this id in source(s): http://nuget.org/a/, http://nuget.org/b/");
            message.TargetGraphs.Should().BeEquivalentTo(new[] { targetGraphName });
            message.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public async Task GivenAnUnresolvedPackageWithNoVersionsVerifyMessage()
        {
            var range = new LibraryRange("x", LibraryDependencyTarget.Package);
            var versions = new List<NuGetVersion>();

            var message = await GetMessage(range, versions);

            message.Code.Should().Be(NuGetLogCode.NU1101);
            message.LibraryId.Should().Be("x");
            message.Message.Should().Be("Unable to find package x. No packages exist with this id in source(s): http://nuget.org/a/");
            message.TargetGraphs.Should().BeEquivalentTo(new[] { "abc" });
            message.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public async Task GivenAMultipleSourcesVerifyInfosReturned()
        {
            var versions1 = new[]
            {
                NuGetVersion.Parse("1.0.0"),
                NuGetVersion.Parse("2.0.0"),
                NuGetVersion.Parse("3.0.0-beta"),
            };

            var versions2 = new[]
            {
                NuGetVersion.Parse("1.0.0")
            };

            var provider1 = GetProvider("http://nuget.org/a/", versions1);
            var provider2 = GetProvider("http://nuget.org/b/", versions2);

            var cacheContext = new Mock<SourceCacheContext>();
            var remoteLibraryProviders = new List<IRemoteDependencyProvider>() { provider1.Object, provider2.Object };

            List<KeyValuePair<PackageSource, ImmutableArray<NuGetVersion>>> infos = await UnresolvedMessages.GetSourceInfosForIdAsync(id: "a", remoteLibraryProviders: remoteLibraryProviders, sourceCacheContext: cacheContext.Object, logger: NullLogger.Instance, token: CancellationToken.None);

            infos.Count.Should().Be(2);
            infos[0].Value.Should().BeEquivalentTo(versions1);
            infos[1].Value.Should().BeEquivalentTo(versions2);
            infos[0].Key.Source.Should().Be("http://nuget.org/a/");
            infos[1].Key.Source.Should().Be("http://nuget.org/b/");
        }

        [Fact]
        public async Task GivenAnEmptySourceVerifySourceInfoContainsAllVersions()
        {
            var versions = new[]
            {
                NuGetVersion.Parse("1.0.0"),
                NuGetVersion.Parse("2.0.0"),
                NuGetVersion.Parse("3.0.0-beta"),
            };

            var source = new PackageSource("http://nuget.org/a/");
            var context = new Mock<SourceCacheContext>();
            var provider = new Mock<IRemoteDependencyProvider>();
            provider.Setup(e => e.GetAllVersionsAsync(It.IsAny<string>(), It.IsAny<SourceCacheContext>(), It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => versions);
            provider.SetupGet(e => e.Source).Returns(source);

            var info = await UnresolvedMessages.GetSourceInfoForIdAsync(provider.Object, "a", context.Object, NullLogger.Instance, CancellationToken.None);

            info.Value.Should().BeEquivalentTo(versions);
            info.Key.Source.Should().Be(source.Source);
        }

        [Fact]
        public async Task GivenAnEmptySourceVerifySourceInfoContainsNoVersions()
        {
            var source = new PackageSource("http://nuget.org/a/");
            var context = new Mock<SourceCacheContext>();
            var provider = new Mock<IRemoteDependencyProvider>();
            provider.Setup(e => e.GetAllVersionsAsync(It.IsAny<string>(), It.IsAny<SourceCacheContext>(), It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => Enumerable.Empty<NuGetVersion>());
            provider.SetupGet(e => e.Source).Returns(source);

            var info = await UnresolvedMessages.GetSourceInfoForIdAsync(provider.Object, "a", context.Object, NullLogger.Instance, CancellationToken.None);

            info.Value.Should().BeEmpty();
            info.Key.Source.Should().Be(source.Source);
        }

        [Fact]
        public void GivenARangeWithExclusiveBoundsVerifyExactMatchesCanStillBeSelected()
        {
            var range = VersionRange.Parse("(1.0.0, 2.0.0)");
            var versions = ImmutableArray.Create(
                NuGetVersion.Parse("1.0.0"),
                NuGetVersion.Parse("2.0.0")
            );

            UnresolvedMessages.GetBestMatch(versions, range).Should().BeEquivalentTo(NuGetVersion.Parse("1.0.0"));
        }

        [Fact]
        public void GivenARangeWithExclusiveBoundsVerifyExactMatchesCanStillBeSelectedForUpper()
        {
            var range = VersionRange.Parse("(1.0.0, 2.0.0)");
            var versions = ImmutableArray.Create(
                NuGetVersion.Parse("2.0.0")
            );

            UnresolvedMessages.GetBestMatch(versions, range).Should().BeEquivalentTo(NuGetVersion.Parse("2.0.0"));
        }

        [Theory]
        // in the range first
        [InlineData("1.0.0", "0.1.0,1.0.0,3.0.0,4.0.0")]
        // then above the range
        [InlineData("3.0.0", "0.1.0,0.2.0,3.0.0,4.0.0")]
        // include prerelease also
        [InlineData("3.0.0-beta", "0.1.0,0.2.0,3.0.0-beta,3.0.0,4.0.0")]
        [InlineData("4.0.0", "0.1.0,0.2.0,4.0.0")]
        // then below the range
        [InlineData("0.2.0", "0.1.0,0.2.0")]
        [InlineData("0.1.0-beta", "0.1.0-beta")]
        public void GivenRangeOf1To2VerifyBestMatch(string expected, string versionStrings)
        {
            var range = VersionRange.Parse("[1.0.0, 2.0.0]");
            var versions = ImmutableArray.CreateRange(versionStrings.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(NuGetVersion.Parse));

            UnresolvedMessages.GetBestMatch(versions, range).Should().BeEquivalentTo(NuGetVersion.Parse(expected));
        }

        [Theory]
        // in the range first
        [InlineData("1.0.0", "4.0.0,3.0.0,1.0.0,0.1.0")]
        // then above the range
        [InlineData("3.0.0", "4.0.0,3.0.0,0.2.0,0.1.0")]
        // include prerelease also
        [InlineData("3.0.0-beta", "4.0.0,3.0.0,3.0.0-beta,0.2.0,0.1.0")]
        [InlineData("4.0.0", "4.0.0,0.2.0,0.1.0")]
        // then below the range
        [InlineData("0.2.0", "0.2.0,0.1.0")]
        [InlineData("0.1.0-beta", "0.1.0-beta")]
        public void GivenRangeOf1To2ReversedVerifyBestMatch(string expected, string versionStrings)
        {
            var range = VersionRange.Parse("[1.0.0, 2.0.0]");
            var versions = ImmutableArray.CreateRange(versionStrings.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(NuGetVersion.Parse));

            UnresolvedMessages.GetBestMatch(versions, range).Should().BeEquivalentTo(NuGetVersion.Parse(expected));
        }

        [Theory]
        //Any time there is a upper bound, we go to the first version above the upper bound
        [InlineData("[1.*,2.0.0]", "3.0.0", "0.1.0,0.3.0,0.9.9,3.0.0")] // has LowerBound, has UpperBound, floating - inclusivity doesn't matter for this command as it's not selecting assets.
        [InlineData("(1.0.1,2.0.0]", "3.0.0", "0.1.0,0.3.0,0.9.9,3.0.0")] // has LowerBound, has UpperBound, not floating
        [InlineData("(,2.0.0]", "2.0.1", "2.0.1,2.5.0,3.0.0")] // no LowerBound, has UpperBound, not floating => no LowerBound, has UpperBound, floating is not a valid scenario
                                                               // if it has a lower bound, it's always the one under
        [InlineData("[1.0.0,)", "0.9", "0.0.1,0.0.5,0.1,0.9")] // lower bound, no upper bound, no floating
        [InlineData("[1.*,)", "0.9", "0.0.1,0.0.5,0.1,0.9")] // lower bound, no upper bound, floating
        [InlineData("*", "2.1.0-preview1-final", "0.0.1-alpha,2.1.0-preview1-final")] // lower bound, no upper bound, floating, https://github.com/NuGet/Home/issues/6658, inclusivity doesn't matter as it's not selecting assets
        [InlineData("[1.*, 2.0.0]", "3.0.0", "0.1.0,0.3.0,3.0.0,4.0.0")] // lower bound, upper bound, floating - Version immediately above upper bound chosen
        public void GivenVersionRangeVerifyBestMatch(string versionRange, string expectedVersion, string versionStrings)
        {
            var range = VersionRange.Parse(versionRange);
            var versions = ImmutableArray.CreateRange(versionStrings.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(NuGetVersion.Parse));
            Assert.Null(range.FindBestMatch(versions));
            UnresolvedMessages.GetBestMatch(versions, range).Should().BeEquivalentTo(NuGetVersion.Parse(expectedVersion));
        }

        [Fact]
        public void GivenNoVersionsVerifyBestMatch()
        {
            var range = VersionRange.Parse("2.0.0");
            var versions = ImmutableArray<NuGetVersion>.Empty;

            UnresolvedMessages.GetBestMatch(versions, range).Should().BeNull();
        }

        [Fact]
        public void GivenASourceInfoVerifyFullFormatting()
        {
            var range = VersionRange.Parse("2.0.0");
            var sourceInfo = new KeyValuePair<PackageSource, ImmutableArray<NuGetVersion>>(
                key: new PackageSource("http://nuget.org/a/"),
                value: [NuGetVersion.Parse("1.0.0")]);

            var s = UnresolvedMessages.FormatSourceInfo(sourceInfo, range);

            s.Should().Be("Found 1 version(s) in http://nuget.org/a/ [ Nearest version: 1.0.0 ]");
        }

        [Fact]
        public void GivenASourceInfoWithNoVersionsVerifyOutputString()
        {
            var range = VersionRange.Parse("2.0.0");
            var sourceInfo = new KeyValuePair<PackageSource, ImmutableArray<NuGetVersion>>(
                key: new PackageSource("http://nuget.org/a/"),
                value: []);

            var s = UnresolvedMessages.FormatSourceInfo(sourceInfo, range);

            s.Should().Be("Found 0 version(s) in http://nuget.org/a/");
        }

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("[1.0.0]")]
        [InlineData("(1.0.0, 2.0.0)")]
        [InlineData("1.0.*")]
        public void GivenAStableRangeVerifyIsPrereleaseAllowedFalse(string s)
        {
            var range = VersionRange.Parse(s);

            UnresolvedMessages.IsPrereleaseAllowed(range).Should().BeFalse();
        }

        [Theory]
        [InlineData("1.0.0-a")]
        [InlineData("[1.0.0-b]")]
        [InlineData("(1.0.0-a, 2.0.0)")]
        [InlineData("(1.0.0, 2.0.0-a)")]
        [InlineData("1.0.0-*")]
        [InlineData("1.0.0-beta.*")]
        public void GivenAStableRangeVerifyIsPrereleaseAllowedTrue(string s)
        {
            var range = VersionRange.Parse(s);

            UnresolvedMessages.IsPrereleaseAllowed(range).Should().BeTrue();
        }

        [Fact]
        public void GivenANullRangeVerifyIsPrereleaseAllowedFalse()
        {
            UnresolvedMessages.IsPrereleaseAllowed(null).Should().BeFalse();
        }

        [Fact]
        public void GivenAPreRelRangeAndAPreRelVersionInRangeVerifyHasPrereleaseVersionsOnlyTrue()
        {
            var range = VersionRange.Parse("( , 2.0.0-alpha)");
            var versions = new[] { NuGetVersion.Parse("1.0.1-beta") };

            UnresolvedMessages.HasPrereleaseVersionsOnly(range, versions).Should().BeTrue();
        }

        [Fact]
        public void GivenAStableRangeAndAPreRelVersionInRangeVerifyHasPrereleaseVersionsOnlyTrue()
        {
            var range = VersionRange.Parse("1.0.0");
            var versions = new[] { NuGetVersion.Parse("1.0.1-beta") };

            UnresolvedMessages.HasPrereleaseVersionsOnly(range, versions).Should().BeTrue();
        }

        [Fact]
        public void GivenAStableRangeAndAStableVersionInRangeVerifyHasPrereleaseVersionsOnlyFalse()
        {
            var range = VersionRange.Parse("1.0.0");
            var versions = new[] { NuGetVersion.Parse("1.0.0") };

            UnresolvedMessages.HasPrereleaseVersionsOnly(range, versions).Should().BeFalse();
        }

        [Fact]
        public void GivenAStableRangeAndNoVersionsVerifyHasPrereleaseVersionsOnlyFalse()
        {
            var range = VersionRange.Parse("1.0.0");

            UnresolvedMessages.HasPrereleaseVersionsOnly(range, new List<NuGetVersion>()).Should().BeFalse();
        }

        [Fact]
        public void GivenAStableRangeAndAStableVersionOutOfRangeVerifyHasPrereleaseVersionsOnlyFalse()
        {
            var range = VersionRange.Parse("2.0.0");
            var versions = new[] { NuGetVersion.Parse("1.0.0") };

            UnresolvedMessages.HasPrereleaseVersionsOnly(range, versions).Should().BeFalse();
        }

        [Fact]
        public void GivenAStableRangeAndAPreRelVersionOutOfRangeVerifyHasPrereleaseVersionsOnlyFalse()
        {
            var range = VersionRange.Parse("2.0.0");
            var versions = new[] { NuGetVersion.Parse("1.0.0-beta") };

            UnresolvedMessages.HasPrereleaseVersionsOnly(range, versions).Should().BeFalse();
        }

        [Fact]
        public async Task GivenAnUnreachableSource_DoesNotThrow()
        {
            var source = new PackageSource("http://nuget.org/a/");
            var context = new Mock<SourceCacheContext>();
            var provider = new Mock<IRemoteDependencyProvider>();
            provider.Setup(e => e.GetAllVersionsAsync(It.IsAny<string>(), It.IsAny<SourceCacheContext>(), It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => null); // unreachable sources would return null
            provider.SetupGet(e => e.Source).Returns(source);

            var info = await UnresolvedMessages.GetSourceInfoForIdAsync(provider.Object, "a", context.Object, NullLogger.Instance, CancellationToken.None);

            info.Value.Should().BeEmpty();
            info.Key.Source.Should().Be(source.Source);
        }

        [Fact]
        public async Task GetMessageAsync_WithPackageSourceMappingAndNoMatchingProviders_NU1100IncludesSourceMappingDetails()
        {
            var libraryId = "x";
            var range = new LibraryRange(libraryId, LibraryDependencyTarget.Package);
            bool isPackageSourceMappingEnabled = true;
            var provider1 = GetProvider("http://nuget.org/a/", new List<NuGetVersion>());
            var enabledProviders = new List<IRemoteDependencyProvider>() { };
            var allProviders = new List<IRemoteDependencyProvider>() { provider1.Object };
            var targetGraphName = "targetGraphName";

            var message = await UnresolvedMessages.GetMessageAsync(targetGraphName, range, enabledProviders, isPackageSourceMappingEnabled, allProviders, new Mock<SourceCacheContext>().Object, new TestLogger(), CancellationToken.None);

            message.Code.Should().Be(NuGetLogCode.NU1100);
            message.LibraryId.Should().Be(libraryId);
            message.Message.Should().Be($"Unable to resolve '{libraryId}' for '{targetGraphName}'. PackageSourceMapping is enabled, the following source(s) were not considered: http://nuget.org/a/.");
            message.TargetGraphs.Should().BeEquivalentTo(new[] { targetGraphName });
            message.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public async Task GetMessageAsync_WithPackageSourceMappingAndProvidersNotConsidered_NU1101IncludesSourceMappingDetails()
        {
            var libraryId = "x";
            var range = new LibraryRange(libraryId, LibraryDependencyTarget.Package);
            bool isPackageSourceMappingEnabled = true;
            var provider1 = GetProvider("http://nuget.org/a/", new List<NuGetVersion>());
            var provider2 = GetProvider("http://nuget.org/b/", new List<NuGetVersion>());
            var provider3 = GetProvider("http://nuget.org/c/", new List<NuGetVersion>());
            var enabledProviders = new List<IRemoteDependencyProvider>() { provider1.Object };
            var allProviders = new List<IRemoteDependencyProvider>() { provider3.Object, provider1.Object, provider2.Object };
            var targetGraphName = "targetGraphName";

            var message = await UnresolvedMessages.GetMessageAsync(targetGraphName, range, enabledProviders, isPackageSourceMappingEnabled, allProviders, new Mock<SourceCacheContext>().Object, new TestLogger(), CancellationToken.None);

            message.Code.Should().Be(NuGetLogCode.NU1101);
            message.LibraryId.Should().Be(libraryId);
            message.Message.Should().Be($"Unable to find package x. No packages exist with this id in source(s): http://nuget.org/a/. PackageSourceMapping is enabled, the following source(s) were not considered: http://nuget.org/b/, http://nuget.org/c/.");
            message.TargetGraphs.Should().BeEquivalentTo(new[] { targetGraphName });
            message.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public async Task GetMessageAsync_WithPackageSourceMappingAndProvidersNotConsidered_NU1102IncludesSourceMappingDetails()
        {
            var libraryId = "x";
            var range = new LibraryRange(libraryId, VersionRange.Parse("6.0.0"), LibraryDependencyTarget.Package);
            bool isPackageSourceMappingEnabled = true;
            var provider1 = GetProvider("http://nuget.org/a/", new List<NuGetVersion>() { NuGetVersion.Parse("6.0.0") });
            var provider2 = GetProvider("http://nuget.org/b/", new List<NuGetVersion>());
            var provider3 = GetProvider("http://nuget.org/c/", new List<NuGetVersion>());
            var enabledProviders = new List<IRemoteDependencyProvider>() { provider1.Object };
            var allProviders = new List<IRemoteDependencyProvider>() { provider3.Object, provider1.Object, provider2.Object };
            var targetGraphName = "targetGraphName";

            var message = await UnresolvedMessages.GetMessageAsync(targetGraphName, range, enabledProviders, isPackageSourceMappingEnabled, allProviders, new Mock<SourceCacheContext>().Object, new TestLogger(), CancellationToken.None);

            message.Code.Should().Be(NuGetLogCode.NU1102);
            message.LibraryId.Should().Be(libraryId);
            message.Message.Should().Be($"Unable to find package x with version (>= 6.0.0)" +
                Environment.NewLine +
                "  - Found 1 version(s) in http://nuget.org/a/ [ Nearest version: 6.0.0 ]" +
                Environment.NewLine +
                "  - Versions from http://nuget.org/b/ were not considered" +
                Environment.NewLine +
                "  - Versions from http://nuget.org/c/ were not considered"
                );
            message.TargetGraphs.Should().BeEquivalentTo(new[] { targetGraphName });
            message.Level.Should().Be(LogLevel.Error);
        }

        private static Mock<IRemoteDependencyProvider> GetProvider(string source, IEnumerable<NuGetVersion> versions)
        {
            var provider = new Mock<IRemoteDependencyProvider>();
            provider.Setup(e => e.GetAllVersionsAsync(It.IsAny<string>(), It.IsAny<SourceCacheContext>(), It.IsAny<ILogger>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => versions);
            provider.SetupGet(e => e.Source).Returns(new PackageSource(source));

            return provider;
        }

        private static async Task<RestoreLogMessage> GetMessage(LibraryRange range, List<NuGetVersion> versions)
        {
            var token = CancellationToken.None;
            var logger = new TestLogger();
            var provider = GetProvider("http://nuget.org/a/", versions);
            var cacheContext = new Mock<SourceCacheContext>();
            var remoteLibraryProviders = new List<IRemoteDependencyProvider>() { provider.Object };
            var targetGraphName = "abc";

            var message = await UnresolvedMessages.GetMessageAsync(targetGraphName, range, remoteLibraryProviders, false, remoteLibraryProviders, cacheContext.Object, logger, token);
            return message;
        }
    }
}
