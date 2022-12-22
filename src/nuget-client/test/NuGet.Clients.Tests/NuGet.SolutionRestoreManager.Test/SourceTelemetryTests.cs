// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.Telemetry;
using NuGet.Test.Utility;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    public class SourceTelemetryTests
    {
        private const string NumLocalFeeds = "NumLocalFeeds";
        private const string NumHTTPv2Feeds = "NumHTTPv2Feeds";
        private const string NumHTTPv3Feeds = "NumHTTPv3Feeds";
        private const string ParentId = "ParentId";
        private const string NuGetOrg = "NuGetOrg";
        private const string NotPresent = "NotPresent";
        private const string YesV2 = "YesV2";
        private const string YesV3 = "YesV3";
        private const string YesV3AndV2 = "YesV3AndV2";
        private const string VsOfflinePackages = "VsOfflinePackages";
        private const string DotnetCuratedFeed = "DotnetCuratedFeed";
        private const string ProtocolRequests = "protocol.requests";
        private const string ProtocolBytes = "protocol.bytes";
        private const string ProtocolDuration = "protocol.duration";
        private const string NumHTTPSv2Feeds = "NumHTTPSv2Feeds";
        private const string NumHTTPSv3Feeds = "NumHTTPSv3Feeds";

        private static readonly Guid Parent = Guid.Parse("33411664-388A-4C48-A607-A2C554171FCE");
        private static readonly PackageSourceTelemetry.Totals ProtocolDiagnosticTotals = new PackageSourceTelemetry.Totals(1, 2, TimeSpan.FromMilliseconds(3));

        [Fact]
        public void GivenEmptySourcesVerifyEventNameForRestore()
        {
            var sources = new List<PackageSource>();
            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);

            summary.Name.Should().Be("RestorePackageSourceSummary");
        }
        [Fact]
        public void GivenEmptySourcesVerifyEventNameForSearch()
        {
            var sources = new List<PackageSource>();
            var summary = SourceTelemetry.GetSearchSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);

            summary.Name.Should().Be("SearchPackageSourceSummary");
        }

        [Fact]
        public void GivenEmptySourcesVerifyZeroCounts()
        {
            var sources = new List<PackageSource>();
            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(0);
            summaryInts[NumHTTPv3Feeds].Should().Be(0);
            summaryInts[NumHTTPSv2Feeds].Should().Be(0);
            summaryInts[NumHTTPSv3Feeds].Should().Be(0);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(NotPresent);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Theory]
        [InlineData("https://www.nuget.org/api/v2/curated-feeds/microsoftdotnet/")]
        [InlineData("https://nuget.org/api/v2/curated-feeds/microsoftdotnet/")]
        [InlineData("https://www.nuget.org/api/v2/curated-feeds/microsoftdotnet")]
        [InlineData("https://www.nuget.org/api/v2/curated-feeds/MICROSOFTDOTNET/")]
        public void GivenNuGetCuratedFeedVerifySummary(string source)
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource(source)
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(1);
            summaryInts[NumHTTPv3Feeds].Should().Be(0);
            summaryInts[NumHTTPSv2Feeds].Should().Be(1);
            summaryInts[NumHTTPSv3Feeds].Should().Be(0);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(NotPresent);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(true);
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData(true)]
        [InlineData(false)]
        public void GivenVsPackagesSourceVerifySummary(bool trailingSlash)
        {
            // Specify both so this test works on 32-bit and 64-bit processes.
            var suffix = trailingSlash ? @"\" : string.Empty;
            var sources = new List<PackageSource>()
            {
                new PackageSource(@"C:\Program Files (x86)\Microsoft SDKs\NuGetPackages" + suffix),
                new PackageSource(@"C:\Program Files\Microsoft SDKs\NuGetPackages" + suffix)
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(2);
            summaryInts[NumHTTPv2Feeds].Should().Be(0);
            summaryInts[NumHTTPv3Feeds].Should().Be(0);
            summaryInts[NumHTTPSv2Feeds].Should().Be(0);
            summaryInts[NumHTTPSv3Feeds].Should().Be(0);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(NotPresent);
            summaryBools[VsOfflinePackages].Should().Be(true);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void GivenNuGetV2WithoutSubdomainVerifySummary()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("https://NuGet.org/api/v2/")
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(1);
            summaryInts[NumHTTPv3Feeds].Should().Be(0);
            summaryInts[NumHTTPSv2Feeds].Should().Be(1);
            summaryInts[NumHTTPSv3Feeds].Should().Be(0);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(YesV2);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void GivenOnlyNuGetV2VerifySummary()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("https://www.NuGet.org/api/v2/")
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(1);
            summaryInts[NumHTTPv3Feeds].Should().Be(0);
            summaryInts[NumHTTPSv2Feeds].Should().Be(1);
            summaryInts[NumHTTPSv3Feeds].Should().Be(0);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(YesV2);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void GivenOnlyNuGetV3WithoutSubdomainVerifySummary()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("https://nuget.org/v3/index.JSON")
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(0);
            summaryInts[NumHTTPv3Feeds].Should().Be(1);
            summaryInts[NumHTTPSv2Feeds].Should().Be(0);
            summaryInts[NumHTTPSv3Feeds].Should().Be(1);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(YesV3);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void GivenOnlyNuGetV3VerifySummary()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("https://api.nuget.org/v3/index.JSON")
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(0);
            summaryInts[NumHTTPv3Feeds].Should().Be(1);
            summaryInts[NumHTTPSv2Feeds].Should().Be(0);
            summaryInts[NumHTTPSv3Feeds].Should().Be(1);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(YesV3);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void GivenOnlyNuGetV3andV2VerifySummary()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("https://api.nuget.org/v3/index.json"),
                new PackageSource("https://www.NuGet.org/api/v2/"),
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(1);
            summaryInts[NumHTTPv3Feeds].Should().Be(1);
            summaryInts[NumHTTPSv2Feeds].Should().Be(1);
            summaryInts[NumHTTPSv3Feeds].Should().Be(1);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(YesV3AndV2);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void GivenOnlyNuGetV2andV3VerifySummary()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("https://www.NuGet.org/api/v2/"),
                new PackageSource("https://api.nuget.org/v3/index.json"),
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(1);
            summaryInts[NumHTTPv3Feeds].Should().Be(1);
            summaryInts[NumHTTPSv2Feeds].Should().Be(1);
            summaryInts[NumHTTPSv3Feeds].Should().Be(1);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(YesV3AndV2);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void GivenDisabledFeedsVerifyCount()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("https://www.NuGet.org/api/v2/", "n1", isEnabled: false),
                new PackageSource("https://api.nuget.org/v3/index.json", "n2", isEnabled: false),
                new PackageSource("packages")
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(1);
            summaryInts[NumHTTPv2Feeds].Should().Be(0);
            summaryInts[NumHTTPv3Feeds].Should().Be(0);
            summaryInts[NumHTTPSv2Feeds].Should().Be(0);
            summaryInts[NumHTTPSv3Feeds].Should().Be(0);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(NotPresent);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void VerifySubDomainNuGetOrgIsNotCounted()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("https://www.NuGet.org.myget.org/www.nuget.org/v2/"),
                new PackageSource("https://api.nuget.org.myget.org/api.nuget.org/index.json")
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(1);
            summaryInts[NumHTTPv3Feeds].Should().Be(1);
            summaryInts[NumHTTPSv2Feeds].Should().Be(1);
            summaryInts[NumHTTPSv3Feeds].Should().Be(1);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(NotPresent);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void LocalFeedsVerifyCount()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("packages"),
                new PackageSource(UriUtility.CreateSourceUri(Path.Combine(Path.GetTempPath(), "packages"), UriKind.Absolute).AbsoluteUri),
                new PackageSource(@"\\share\packages"),
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(3);
            summaryInts[NumHTTPv2Feeds].Should().Be(0);
            summaryInts[NumHTTPv3Feeds].Should().Be(0);
            summaryInts[NumHTTPSv2Feeds].Should().Be(0);
            summaryInts[NumHTTPSv3Feeds].Should().Be(0);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(NotPresent);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void GivenAnInvalidSourceVerifyNoFailures()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("file:/bad!|source"),
                new PackageSource("https:/bad"),
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(2);
            summaryInts[NumHTTPv2Feeds].Should().Be(0);
            summaryInts[NumHTTPv3Feeds].Should().Be(0);
            summaryInts[NumHTTPSv2Feeds].Should().Be(0);
            summaryInts[NumHTTPSv3Feeds].Should().Be(0);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(NotPresent);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void VerifyV3Feed()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("http://tempuri.local/index.json")
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(0);
            summaryInts[NumHTTPv3Feeds].Should().Be(1);
            summaryInts[NumHTTPSv2Feeds].Should().Be(0);
            summaryInts[NumHTTPSv3Feeds].Should().Be(0);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(NotPresent);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void VerifyV2Feed()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource("http://tempuri.local/packages/")
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryInts[NumLocalFeeds].Should().Be(0);
            summaryInts[NumHTTPv2Feeds].Should().Be(1);
            summaryInts[NumHTTPv3Feeds].Should().Be(0);
            summaryInts[NumHTTPSv2Feeds].Should().Be(0);
            summaryInts[NumHTTPSv3Feeds].Should().Be(0);
            summaryStrings[ParentId].Should().Be(Parent.ToString());
            summaryStrings[NuGetOrg].Should().Be(NotPresent);
            summaryBools[VsOfflinePackages].Should().Be(false);
            summaryBools[DotnetCuratedFeed].Should().Be(false);
        }

        [Fact]
        public void VerifyProtocolDiagnosticTotals()
        {
            var sources = new List<PackageSource>();
            var telemetry = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);

            telemetry[ProtocolRequests].Should().Be(1);
            telemetry[ProtocolBytes].Should().Be(2L);
            telemetry[ProtocolDuration].Should().Be(3.0);
        }

        [Fact]
        public void LocalAndHttpSources_WithHTTPSv2Feeds_FeedCountsAreCorrect()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource(@"\\share\packages"),
                new PackageSource("http://nugettest.org/v3/index.JSON"),
                new PackageSource("http://www.nuget.org/api/v2/curated-feeds/microsoftdotnet"), //v2
                new PackageSource("http://tempuri.local/index.json"),
                new PackageSource("http://nuget.org/v3/index.JSON"),
                new PackageSource("https://www.NuGet.org/api/v2/") //v2 and HTTPS
            };

            var summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            var summaryStrings = GetValuesAsStrings(summary);
            var summaryInts = GetValuesAsInts(summary);
            var summaryBools = GetValuesAsBools(summary);

            summaryStrings[ParentId].Should().Be(Parent.ToString());

            int? numLocalFeeds = summaryInts[NumLocalFeeds];
            int? numHTTPv2Feeds = summaryInts[NumHTTPv2Feeds];
            int? numHTTPv3Feeds = summaryInts[NumHTTPv3Feeds];
            int? numHTTPSv2Feeds = summaryInts[NumHTTPSv2Feeds];
            int? numHTTPSv3Feeds = summaryInts[NumHTTPSv3Feeds];

            numLocalFeeds.Should().Be(1);
            numHTTPv2Feeds.Should().Be(2);
            numHTTPv3Feeds.Should().Be(3);
            numHTTPSv2Feeds.Should().Be(1);
            numHTTPSv3Feeds.Should().Be(0);

            int? totalFeeds = numLocalFeeds + numHTTPv2Feeds + numHTTPv3Feeds;
            totalFeeds.Should().Be(sources.Count);
        }

        [Fact]
        public void LocalAndHttpSources_WithHTTPSv3Feeds_FeedCountsAreCorrect()
        {
            var sources = new List<PackageSource>()
            {
                new PackageSource(@"\\share\packages"),
                new PackageSource("https://nugettest.org/v3/index.JSON"),
                new PackageSource("http://tempuri.local/index.json"),
                new PackageSource("https://nuget.org/v3/index.JSON")
            };

            TelemetryEvent summary = SourceTelemetry.GetRestoreSourceSummaryEvent(Parent, sources, ProtocolDiagnosticTotals);
            Dictionary<string, string> summaryStrings = GetValuesAsStrings(summary);
            Dictionary<string, int?> summaryInts = GetValuesAsInts(summary);

            summaryStrings[ParentId].Should().Be(Parent.ToString());

            int? numLocalFeeds = summaryInts[NumLocalFeeds];
            int? numHTTPv2Feeds = summaryInts[NumHTTPv2Feeds];
            int? numHTTPv3Feeds = summaryInts[NumHTTPv3Feeds];
            int? numHTTPSv2Feeds = summaryInts[NumHTTPSv2Feeds];
            int? numHTTPSv3Feeds = summaryInts[NumHTTPSv3Feeds];

            numLocalFeeds.Should().Be(1);
            numHTTPv2Feeds.Should().Be(0);
            numHTTPv3Feeds.Should().Be(3);
            numHTTPSv2Feeds.Should().Be(0);
            numHTTPSv3Feeds.Should().Be(2);

            int? totalFeeds = numLocalFeeds + numHTTPv2Feeds + numHTTPv3Feeds;
            totalFeeds.Should().Be(sources.Count);
        }

        private static Dictionary<string, string> GetValuesAsStrings(TelemetryEvent item)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var e = item.GetEnumerator();

            while (e.MoveNext())
            {
                values.Add(e.Current.Key, e.Current.Value as string);
            }

            return values;
        }

        private static Dictionary<string, int?> GetValuesAsInts(TelemetryEvent item)
        {
            var values = new Dictionary<string, int?>(StringComparer.OrdinalIgnoreCase);
            var e = item.GetEnumerator();

            while (e.MoveNext())
            {
                values.Add(e.Current.Key, e.Current.Value as int?);
            }

            return values;
        }

        private static Dictionary<string, bool?> GetValuesAsBools(TelemetryEvent item)
        {
            var values = new Dictionary<string, bool?>(StringComparer.OrdinalIgnoreCase);
            var e = item.GetEnumerator();

            while (e.MoveNext())
            {
                values.Add(e.Current.Key, e.Current.Value as bool?);
            }

            return values;
        }
    }
}
