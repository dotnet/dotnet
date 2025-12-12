// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Build.Manifest;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Build.Tasks.Feed.Model
{
    public class PublishingConstants
    {
        public static readonly string ExpectedFeedUrlSuffix = "index.json";

        /// <summary>
        /// Mapping of Azure storage accounts to their corresponding CDN URLs.
        /// Used to replace blob storage URLs with CDN URLs for both aka.ms links and asset locations.
        /// </summary>
        public static readonly Dictionary<string, string> AccountsWithCdns = new()
        {
            {"dotnetcli.blob.core.windows.net", "builds.dotnet.microsoft.com" },
            {"dotnetbuilds.blob.core.windows.net", "ci.dot.net" }
        };

        // Matches package feeds like
        // https://pkgs.dev.azure.com/dnceng/public/_packaging/public-feed-name/nuget/v3/index.json
        // or https://pkgs.dev.azure.com/dnceng/_packaging/internal-feed-name/nuget/v3/index.json
        public static readonly string AzDoNuGetFeedPattern =
            @"https://pkgs.dev.azure.com/(?<account>[a-zA-Z0-9-]+)/(?<visibility>[a-zA-Z0-9-]+/)?_packaging/(?<feed>.+)/nuget/v3/index.json";

        public static readonly TargetFeedContentType[] InstallersAndSymbols =
        [
            TargetFeedContentType.OSX,
            TargetFeedContentType.Deb,
            TargetFeedContentType.Rpm,
            TargetFeedContentType.Node,
            TargetFeedContentType.BinaryLayout,
            TargetFeedContentType.Installer,
            TargetFeedContentType.Maven,
            TargetFeedContentType.VSIX,
            TargetFeedContentType.Badge,
            TargetFeedContentType.Symbols,
            TargetFeedContentType.Other
        ];

        public static readonly TargetFeedContentType[] InstallersAndChecksums =
        [
            TargetFeedContentType.OSX,
            TargetFeedContentType.Deb,
            TargetFeedContentType.Rpm,
            TargetFeedContentType.Node,
            TargetFeedContentType.BinaryLayout,
            TargetFeedContentType.Installer,
            TargetFeedContentType.Maven,
            TargetFeedContentType.VSIX,
            TargetFeedContentType.Badge,
            TargetFeedContentType.Checksum,
            TargetFeedContentType.Other
        ];

        public static readonly TargetFeedContentType[] Packages =
        [
            TargetFeedContentType.Package,
            TargetFeedContentType.CorePackage,
            TargetFeedContentType.ToolingPackage,
            TargetFeedContentType.InfrastructurePackage,
            TargetFeedContentType.LibraryPackage,
        ];

        public enum BuildQuality
        {
            [Description("daily")]
            Daily,

            [Description("signed")]
            Signed,

            [Description("validated")]
            Validated,

            [Description("preview")]
            Preview,

            [Description("")]
            GA
        }

        #region Target Channel Config Feeds
        public const string FeedStagingForInstallers = "https://dotnetbuilds.blob.core.windows.net/public";
        public const string FeedStagingForChecksums = "https://dotnetbuilds.blob.core.windows.net/public-checksums";

        public const string FeedStagingInternalForInstallers = "https://dotnetbuilds.blob.core.windows.net/internal";
        public const string FeedStagingInternalForChecksums = "https://dotnetbuilds.blob.core.windows.net/internal-checksums";

        public const string FeedDevForInstallers = "https://dotnetbuilds.blob.core.windows.net/dev";
        public const string FeedDevInternalForInstallers = "https://dotnetbuilds.blob.core.windows.net/dev-internal";

        private const string FeedDev = "https://pkgs.dev.azure.com/dnceng/public/_packaging/general-testing/nuget/v3/index.json";

        private const string FeedDotNetExperimental = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-experimental/nuget/v3/index.json";

        private const string FeedDotNetExperimentalInternal = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet-experimental-internal/nuget/v3/index.json";

        public const string FeedDotNetEng = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json";

        private const string FeedDotNetEngInternal = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet-eng-internal/nuget/v3/index.json";

        private const string FeedDotNetTools = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json";

        private const string FeedDotNetToolsInternal = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet-tools-internal/nuget/v3/index.json";

        private const string FeedDotNet6Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6/nuget/v3/index.json";
        private const string FeedDotNet6Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6-transport/nuget/v3/index.json";

        private const string FeedDotNet6InternalShipping = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet6-internal/nuget/v3/index.json";
        private const string FeedDotNet6InternalTransport = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet6-internal-transport/nuget/v3/index.json";

        private const string FeedDotNet7Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7/nuget/v3/index.json";
        private const string FeedDotNet7Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7-transport/nuget/v3/index.json";

        private const string FeedDotNet7InternalShipping = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet7-internal/nuget/v3/index.json";
        private const string FeedDotNet7InternalTransport = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet7-internal-transport/nuget/v3/index.json";

        private const string FeedDotNet8Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet8/nuget/v3/index.json";
        private const string FeedDotNet8Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet8-transport/nuget/v3/index.json";
        private const string FeedDotNet8Workloads = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet8-workloads/nuget/v3/index.json";

        private const string FeedDotNet8InternalShipping = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet8-internal/nuget/v3/index.json";
        private const string FeedDotNet8InternalTransport = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet8-internal-transport/nuget/v3/index.json";

        private const string FeedDotNet9Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet9/nuget/v3/index.json";
        private const string FeedDotNet9Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet9-transport/nuget/v3/index.json";
        private const string FeedDotNet9Workloads = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet9-workloads/nuget/v3/index.json";

        private const string FeedDotNet9InternalShipping = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet9-internal/nuget/v3/index.json";
        private const string FeedDotNet9InternalTransport = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet9-internal-transport/nuget/v3/index.json";

        private const string FeedDotNet10Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet10/nuget/v3/index.json";
        private const string FeedDotNet10Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet10-transport/nuget/v3/index.json";
        private const string FeedDotNet10Workloads = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet10-workloads/nuget/v3/index.json";

        private const string FeedDotNet10InternalShipping = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet10-internal/nuget/v3/index.json";
        private const string FeedDotNet10InternalTransport = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet10-internal-transport/nuget/v3/index.json";

        private const string FeedDotNet11Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet11/nuget/v3/index.json";
        private const string FeedDotNet11Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet11-transport/nuget/v3/index.json";
        private const string FeedDotNet11Workloads = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet11-workloads/nuget/v3/index.json";

        private const string FeedDotNetLibrariesShipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-libraries/nuget/v3/index.json";
        private const string FeedDotNetLibrariesTransport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-libraries-transport/nuget/v3/index.json";

        private const string FeedDotNetLibrariesInternalShipping = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet-libraries-internal/nuget/v3/index.json";
        private const string FeedDotNetLibrariesInternalTransport = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet-libraries-internal-transport/nuget/v3/index.json";

        private const string FeedGeneralTestingInternal = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/general-testing-internal/nuget/v3/index.json";

        private static TargetFeedSpecification[] DotNet6Feeds =
        {
            (Packages, FeedDotNet6Shipping, AssetSelection.ShippingOnly),
            (Packages, FeedDotNet6Transport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet6InternalFeeds =
        {
            (Packages, FeedDotNet6InternalShipping, AssetSelection.ShippingOnly),
            (Packages, FeedDotNet6InternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet8Feeds =
        {
            (Packages, FeedDotNet8Shipping, AssetSelection.ShippingOnly),
            (Packages, FeedDotNet8Transport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet8WorkloadFeeds =
        {
            (Packages, FeedDotNet8Workloads, AssetSelection.ShippingOnly),
            (Packages, FeedDotNet8Workloads, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet8InternalFeeds =
        {
            (Packages, FeedDotNet8InternalShipping, AssetSelection.ShippingOnly),
            (Packages, FeedDotNet8InternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet9Feeds =
        {
            (Packages, FeedDotNet9Shipping, AssetSelection.ShippingOnly),
            (Packages, FeedDotNet9Transport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet9InternalFeeds =
        {
            (Packages, FeedDotNet9InternalShipping, AssetSelection.ShippingOnly),
            (Packages, FeedDotNet9InternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet9WorkloadFeeds =
        {
            (Packages, FeedDotNet9Workloads, AssetSelection.ShippingOnly),
            (Packages, FeedDotNet9Workloads, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet10Feeds =
        {
            (TargetFeedContentType.Package, FeedDotNet10Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet10Transport, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.InfrastructurePackage, FeedDotNetEng, AssetSelection.ShippingOnly),
            (TargetFeedContentType.InfrastructurePackage, FeedDotNetEng, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.CorePackage, FeedDotNet10Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.CorePackage, FeedDotNet10Transport, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.LibraryPackage, FeedDotNetLibrariesShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.LibraryPackage, FeedDotNetLibrariesTransport, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.ToolingPackage, FeedDotNetTools, AssetSelection.ShippingOnly),
            (TargetFeedContentType.ToolingPackage, FeedDotNetTools, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet10InternalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet10InternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet10InternalTransport, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.InfrastructurePackage, FeedDotNetEngInternal, AssetSelection.ShippingOnly),
            (TargetFeedContentType.InfrastructurePackage, FeedDotNetEngInternal, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.CorePackage, FeedDotNet10InternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.CorePackage, FeedDotNet10InternalTransport, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.LibraryPackage, FeedDotNetLibrariesInternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.LibraryPackage, FeedDotNetLibrariesInternalTransport, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.ToolingPackage, FeedDotNetToolsInternal, AssetSelection.ShippingOnly),
            (TargetFeedContentType.ToolingPackage, FeedDotNetToolsInternal, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet10WorkloadFeeds =
        {
            (Packages, FeedDotNet10Workloads, AssetSelection.ShippingOnly),
            (Packages, FeedDotNet10Workloads, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet11Feeds =
        {
            (TargetFeedContentType.Package, FeedDotNet11Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet11Transport, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.InfrastructurePackage, FeedDotNetEng, AssetSelection.ShippingOnly),
            (TargetFeedContentType.InfrastructurePackage, FeedDotNetEng, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.CorePackage, FeedDotNet11Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.CorePackage, FeedDotNet11Transport, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.LibraryPackage, FeedDotNetLibrariesShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.LibraryPackage, FeedDotNetLibrariesTransport, AssetSelection.NonShippingOnly),
            (TargetFeedContentType.ToolingPackage, FeedDotNetTools, AssetSelection.ShippingOnly),
            (TargetFeedContentType.ToolingPackage, FeedDotNetTools, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet11WorkloadFeeds =
        {
            (Packages, FeedDotNet11Workloads, AssetSelection.ShippingOnly),
            (Packages, FeedDotNet11Workloads, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNetEngFeeds =
        {
            (Packages, FeedDotNetEng, AssetSelection.ShippingOnly),
            (Packages, FeedDotNetEng, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNetToolsFeeds =
        {
            (Packages, FeedDotNetTools, AssetSelection.ShippingOnly),
            (Packages, FeedDotNetTools, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNetToolsInternalFeeds =
        {
            (Packages, FeedDotNetToolsInternal, AssetSelection.ShippingOnly),
            (Packages, FeedDotNetToolsInternal, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNetExperimentalFeeds =
        {
            (Packages, FeedDotNetExperimental, AssetSelection.ShippingOnly),
            (Packages, FeedDotNetExperimental, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNetExperimentalInternalFeeds =
        {
            (Packages, FeedDotNetExperimentalInternal, AssetSelection.ShippingOnly),
            (Packages, FeedDotNetExperimentalInternal, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNetLibrariesFeeds =
        {
            (Packages, FeedDotNetLibrariesShipping, AssetSelection.ShippingOnly),
            (Packages, FeedDotNetLibrariesTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNetLibrariesInternalFeeds =
        {
            (Packages, FeedDotNetLibrariesInternalShipping, AssetSelection.ShippingOnly),
            (Packages, FeedDotNetLibrariesInternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] GeneralTestingFeeds =
        {
            (Packages, FeedDev, AssetSelection.ShippingOnly),
            (Packages, FeedDev, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedDevForInstallers),
            (TargetFeedContentType.Checksum, FeedDevForInstallers),
        };

        private static TargetFeedSpecification[] GeneralTestingInternalFeeds =
        {
            (Packages, FeedGeneralTestingInternal, AssetSelection.ShippingOnly),
            (Packages, FeedGeneralTestingInternal, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedDevInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedDevInternalForInstallers),
        };
        #endregion

        public static readonly ImmutableList<Regex> DefaultAkaMSCreateLinkPatterns =
        [
            new Regex(@"\.rpm(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.zip(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.version(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.deb(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.gz(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.pkg(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.msi(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.exe(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.svg(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.tgz(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.jar(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"\.pom(\.sha512)?$", RegexOptions.IgnoreCase),
            new Regex(@"productcommit", RegexOptions.IgnoreCase),
            new Regex(@"productversion", RegexOptions.IgnoreCase)
        ];

        public static readonly ImmutableList<Regex> DefaultAkaMSDoNotCreateLinkPatterns =
        [
            new Regex(@"wixpack", RegexOptions.IgnoreCase),
        ];

        private static readonly ImmutableList<Regex> UnifiedBuildAkaMSDoNotCreateLinkPatterns =
        [
            ..DefaultAkaMSDoNotCreateLinkPatterns,
            new Regex(@"productversion", RegexOptions.IgnoreCase)
        ];

        public static readonly List<TargetChannelConfig> ChannelInfos = [

            // How TO: Adding publishing for a new channel:
            // 1. If not already complete, add desired using `darc add-channel`. Please using follow naming conventions from
            //    other channels.
            // 2. Note channel IDs for each one. You can also obtain these channel IDs with `darc get-channels`
            // 3. In this file, add a TargetChannelConfig element to the list. Please keep channels in order.
            //    The TargetChannelConfig notes the:
            //    - id: Id of the channel to enable publishing for:
            //    - isInternal: Whether this channel is internal or public. All internal channels should have a name suffixed with "Internal"
            //    - akaMSChannelNames: For any non-package files that are produced by the build, there will be stable aka.ms links produced
            //      for these files. The channel names note the prefix for the aka.ms link. Typically:
            //      aka.ms/dotnet/<channel>/<quality>/<path to file with version numbers removed>.
            //      Depending on the channel and time of shipping, different aka.ms channel names may be used. Generally, SDKs get an
            //      aka.ms channel name that corresponds to the SDK band, and if they are the latest SDK (in preview), then also a channel name
            //      for the major.minor of the corresponding .NET release.
            //    - targetFeeds: Tuples of target feeds for packages and blobs. These will generally correspond to the major.minor release,
            //      and will be "internal only" (e.g. DotNet7InternalFeeds) for internal channels. Again, please see existing channel setups.
            //    - symbolTargetType: List of symbol targets. Internal channels should use SymbolPublishVisibility.Internal and public channels should use SymbolPublishVisibility.Public
            //    - filenamesToExclude: Usually left as FilenamesToExclude.

            #region .NET 6 Channels

            // .NET 6,
            new TargetChannelConfig(
                id: 1296,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["6.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 6 Eng,
            new TargetChannelConfig(
                id: 2293,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["eng/net6"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 6 Eng - Validation,
            new TargetChannelConfig(
                id: 2294,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [ "eng/net6validation" ],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET 6 Internal,
            new TargetChannelConfig(
                id: 2097,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/6.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 6 Private,
            new TargetChannelConfig(
                id: 2693,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/6.0-private"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 6.0.1xx SDK,
            new TargetChannelConfig(
                id: 1792,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["6.0.1xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 6.0.1xx SDK Internal,
            new TargetChannelConfig(
                id: 2098,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/6.0.1xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 6.0.2xx SDK,
            new TargetChannelConfig(
                id: 2434,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["6.0.2xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 6.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 2435,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/6.0.2xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 6.0.3xx SDK,
            new TargetChannelConfig(
                id: 2551,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["6.0.3xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 6.0.3xx SDK Internal,
            new TargetChannelConfig(
                id: 2552,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/6.0.3xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 6.0.4xx SDK,
            new TargetChannelConfig(
                id: 2696,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["6.0.4xx", "6.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 6.0.4xx SDK Internal,
            new TargetChannelConfig(
                id: 2697,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/6.0.4xx", "internal/6.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 6.0.Nxx SDK Private,
            new TargetChannelConfig(
                id: 2695,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/6.0.Nxx-private"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            #endregion

            #region .NET 8 Channels

            // .NET 8,
            new TargetChannelConfig(
                id: 3073,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["8.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 8 Workload Release,
            new TargetChannelConfig(
                id: 4610,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["8.0-workloads"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8WorkloadFeeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 8 Eng,
            new TargetChannelConfig(
                id: 3885,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["eng/net8"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 8 Eng - Validation,
            new TargetChannelConfig(
                id: 3886,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["eng/net8validation"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET 8 Internal,
            new TargetChannelConfig(
                id: 3880,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/8.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 8 Private,
            new TargetChannelConfig(
                id: 4120,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/8.0-private"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 8 HotFix Internal,
            new TargetChannelConfig(
                id: 8624,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/8.0-hotfix"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 8.0.1xx SDK,
            new TargetChannelConfig(
                id: 3074,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["8.0.1xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 8.0.1xx SDK Internal,
            new TargetChannelConfig(
                id: 3881,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/8.0.1xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 8.0.1xx SDK HotFix Internal,
            new TargetChannelConfig(
                id: 8625,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/8.0.1xx-hotfix"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 8.0.2xx SDK,
            new TargetChannelConfig(
                id: 4036,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["8.0.2xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 8.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 4266,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/8.0.2xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 8.0.3xx SDK,
            new TargetChannelConfig(
                id: 4267,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["8.0.3xx", "8.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 8.0.3xx SDK Internal,
            new TargetChannelConfig(
                id: 4268,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/8.0.3xx", "internal/8.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 8.0.3xx SDK HotFix Internal,
            new TargetChannelConfig(
                id: 8627,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/8.0.3xx-hotfix"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 8.0.4xx SDK,
            new TargetChannelConfig(
                id: 4586,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["8.0.4xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 8.0.4xx SDK Internal,
            new TargetChannelConfig(
                id: 4609,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/8.0.4xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 8.0.4xx SDK HotFix Internal,
            new TargetChannelConfig(
                id: 8628,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/8.0.4xx-hotfix"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            #endregion

            #region .NET 9 Channels

            // .NET 9,
            new TargetChannelConfig(
                id: 3883,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["9.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 9 Eng,
            new TargetChannelConfig(
                id: 5175,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["eng/net9"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 9 Eng - Validation,
            new TargetChannelConfig(
                id: 5176,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["eng/net9validation"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET 9 Internal,
            new TargetChannelConfig(
                id: 5128,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/9.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 9 Private,
            new TargetChannelConfig(
                id: 5129,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/9.0-private"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 9 HotFix Internal,
            new TargetChannelConfig(
                id: 8629,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/9.0-hotfix"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 9 Workload Release,
            new TargetChannelConfig(
                id: 4611,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["9.0-workloads"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9WorkloadFeeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 9.0.1xx SDK,
            new TargetChannelConfig(
                id: 3884,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["9.0.1xx", "9.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 9.0.1xx SDK Internal,
            new TargetChannelConfig(
                id: 5127,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/9.0.1xx", "internal/9.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 9.0.1xx SDK HotFix Internal,
            new TargetChannelConfig(
                id: 8630,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/9.0.1xx-hotfix"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 9.0.2xx SDK,
            new TargetChannelConfig(
                id: 5286,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["9.0.2xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 9.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 5287,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/9.0.2xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 9.0.3xx SDK,
            new TargetChannelConfig(
                id: 6417,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["9.0.3xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 9.0.3xx SDK Internal,
            new TargetChannelConfig(
                id: 6418,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/9.0.3xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 9.0.3xx SDK HotFix Internal,
            new TargetChannelConfig(
                id: 8632,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/9.0.3xx-hotfix"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            #endregion

            #region .NET 10 Channels

            // .NET 10,
            new TargetChannelConfig(
                id: 5172,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 10 Internal,
            new TargetChannelConfig(
                id: 5177,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 10 Private,
            new TargetChannelConfig(
                id: 8710,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0-private"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 10 Eng,
            new TargetChannelConfig(
                id: 8394,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["eng/net10"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 10 Eng - Validation,
            new TargetChannelConfig(
                id: 8395,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["eng/net10validation"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET 10 Workload Release,
            new TargetChannelConfig(
                id: 5174,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [ "10.0-workloads" ],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10WorkloadFeeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 10.0.1xx SDK,
            new TargetChannelConfig(
                id: 5173,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [ "10.0.1xx", "10.0" ],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 10.0.1xx SDK Release,
            new TargetChannelConfig(
                id: 8859,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [ "10.0.1xx-release", "10.0-release" ],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 10.0.1xx SDK Internal,
            new TargetChannelConfig(
                id: 5178,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0.1xx", "internal/10.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 10.0.1xx SDK Release Internal,
            new TargetChannelConfig(
                id: 8858,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0.1xx-release", "internal/10.0-release"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 10.0.2xx SDK,
            new TargetChannelConfig(
                id: 8856,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [ "10.0.2xx", "10.0" ],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 10.0.2xx SDK Release,
            new TargetChannelConfig(
                id: 8860,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [ "10.0.2xx-release" ],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 10.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 8857,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0.2xx"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 10.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 8861,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0.2xx-release"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 10 RC 2,
            new TargetChannelConfig(
                id: 6498,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0-rc2"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 10 RC 2 Internal,
            new TargetChannelConfig(
                id: 6500,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0-rc2"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            // .NET 10.0.1xx RC 2,
            new TargetChannelConfig(
                id: 6577,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0.1xx-rc2"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 10.0.1xx RC 2 Internal,
            new TargetChannelConfig(
                id: 6579,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0.1xx-rc2"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal),

            #endregion

            #region .NET 11 Channels

            // .NET 11,
            new TargetChannelConfig(
                id: 8297,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["11.0"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet11Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 11 Workload Release,
            new TargetChannelConfig(
                id: 8299,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [ "11.0-workloads" ],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet11WorkloadFeeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // .NET 11.0.1xx SDK,
            new TargetChannelConfig(
                id: 8298,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [ "11.0.1xx", "11.0" ],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: UnifiedBuildAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet11Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            #endregion

            #region Other .NET Channels

            // .NET Core Experimental,
            new TargetChannelConfig(
                id: 562,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetExperimentalFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET Experimental Internal,
            new TargetChannelConfig(
                id: 6820,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetExperimentalInternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal,
                flatten: false),

            // .NET Core Tooling Dev,
            new TargetChannelConfig(
                id: 548,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET Core Tooling Release,
            new TargetChannelConfig(
                id: 549,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET Eng - Latest,
            new TargetChannelConfig(
                id: 2,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["eng"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET Eng - Validation,
            new TargetChannelConfig(
                id: 9,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["eng/validation"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET Eng Services - Int,
            new TargetChannelConfig(
                id: 678,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET Eng Services - Prod,
            new TargetChannelConfig(
                id: 679,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET Internal Tooling,
            new TargetChannelConfig(
                id: 551,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsInternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal,
                flatten: false),

            // .NET Libraries,
            new TargetChannelConfig(
                id: 1648,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetLibrariesFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // .NET Libraries Internal,
            new TargetChannelConfig(
                id: 3882,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetLibrariesInternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal,
                flatten: false),
            
            // .NET AP 1,
            new TargetChannelConfig(
                id: 4122,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsInternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal,
                flatten: false),
            
            // .NET AP 2,
            new TargetChannelConfig(
                id: 4123,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsInternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal,
                flatten: false),

            // .NET AP 3,
            new TargetChannelConfig(
                id: 4124,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsInternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal,
                flatten: false),

            // Aspire 9.x
            new TargetChannelConfig(
                id: 8103,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["9/aspire"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // Aspire 9.x RCs
            new TargetChannelConfig(
                id: 8104,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["9/aspire/rc"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // Aspire 9.x GA
            new TargetChannelConfig(
                id: 8105,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["9/aspire/ga"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNet9Feeds,
                symbolTargetType: SymbolPublishVisibility.Public),

            // General Testing,
            new TargetChannelConfig(
                id: 529,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["generaltesting"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: GeneralTestingFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                isProduction: false),

            // General Testing Internal,
            new TargetChannelConfig(
                id: 1647,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["generaltestinginternal"],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: GeneralTestingInternalFeeds,
                symbolTargetType: SymbolPublishVisibility.Internal,
                isProduction: false),

            #endregion

            #region VS Channels

            // VS 16.6,
            new TargetChannelConfig(
                id: 1010,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // VS 16.7,
            new TargetChannelConfig(
                id: 1011,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // VS 16.8,
            new TargetChannelConfig(
                id: 1154,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // VS 16.9,
            new TargetChannelConfig(
                id: 1473,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // VS 16.10,
            new TargetChannelConfig(
                id: 1692,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // VS 16.11,
            new TargetChannelConfig(
                id: 1926,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // VS 17.0,
            new TargetChannelConfig(
                id: 1853,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // VS 17.1,
            new TargetChannelConfig(
                id: 2346,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // VS 17.2,
            new TargetChannelConfig(
                id: 2542,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.3,
            new TargetChannelConfig(
                id: 2692,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.4,
            new TargetChannelConfig(
                id: 2914,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // VS 17.5,
            new TargetChannelConfig(
                id: 3257,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),

            // VS 17.6,
            new TargetChannelConfig(
                id: 3434,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.7,
            new TargetChannelConfig(
                id: 3581,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.8,
            new TargetChannelConfig(
                id: 3582,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.9,
            new TargetChannelConfig(
                id: 4015,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.10
            new TargetChannelConfig(
                id: 4165,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.11
            new TargetChannelConfig(
                id: 4544,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.12
            new TargetChannelConfig(
                id: 4906,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.13
            new TargetChannelConfig(
                id: 5288,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.14
            new TargetChannelConfig(
                id: 6136,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // VS 17.15
            new TargetChannelConfig(
                id: 6989,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            // 18.0
            new TargetChannelConfig(
                id: 7987,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // 18.1
            new TargetChannelConfig(
                id: 8703,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // 18.2
            new TargetChannelConfig(
                id: 8704,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // 18.3
            new TargetChannelConfig(
                id: 8705,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // 18.4
            new TargetChannelConfig(
                id: 8706,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // 18.5
            new TargetChannelConfig(
                id: 8707,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            
            // 18.6
            new TargetChannelConfig(
                id: 8708,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: [],
                akaMSCreateLinkPatterns: DefaultAkaMSCreateLinkPatterns,
                akaMSDoNotCreateLinkPatterns: DefaultAkaMSDoNotCreateLinkPatterns,
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: SymbolPublishVisibility.Public,
                flatten: false),
            #endregion
        ];
    }
}
