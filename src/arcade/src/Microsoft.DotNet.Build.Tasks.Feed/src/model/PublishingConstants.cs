// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.DotNet.Build.Tasks.Feed.Model
{
    public class PublishingConstants
    {
        public static readonly string LegacyDotNetBlobFeedURL = "https://dotnetfeed.blob.core.windows.net/dotnet-core/index.json";
        public static readonly string ExpectedFeedUrlSuffix = "index.json";

        // Matches package feeds like
        // https://dotnet-feed-internal.azurewebsites.net/container/dotnet-core-internal/sig/dsdfasdfasdf234234s/se/2020-02-02/darc-int-dotnet-arcade-services-babababababe-08/index.json
        public static readonly string AzureStorageProxyFeedPattern =
            @"(?<feedURL>https://([a-z-]+).azurewebsites.net/container/(?<container>[^/]+)/sig/\w+/se/([0-9]{4}-[0-9]{2}-[0-9]{2})/(?<baseFeedName>darc-(?<type>int|pub)-(?<repository>.+?)-(?<sha>[A-Fa-f0-9]{7,40})-?(?<subversion>\d*)/))index.json";

        // Matches package feeds like the one below. Special case for static internal proxy-backed feed
        // https://dotnet-feed-internal.azurewebsites.net/container/dotnet-core-internal/sig/dsdfasdfasdf234234s/se/2020-02-02/darc-int-dotnet-arcade-services-babababababe-08/index.json
        public static readonly string AzureStorageProxyFeedStaticPattern =
            @"(?<feedURL>https://([a-z-]+).azurewebsites.net/container/(?<container>[^/]+)/sig/\w+/se/([0-9]{4}-[0-9]{2}-[0-9]{2})/(?<baseFeedName>[^/]+/))index.json";

        // Matches package feeds like
        // https://dotnetfeed.blob.core.windows.net/dotnet-core/index.json
        public static readonly string AzureStorageStaticBlobFeedPattern =
            @"https://([a-z-]+).blob.core.windows.net/[^/]+/index.json";

        // Matches package feeds like
        // https://pkgs.dev.azure.com/dnceng/public/_packaging/public-feed-name/nuget/v3/index.json
        // or https://pkgs.dev.azure.com/dnceng/_packaging/internal-feed-name/nuget/v3/index.json
        public static readonly string AzDoNuGetFeedPattern =
            @"https://pkgs.dev.azure.com/(?<account>[a-zA-Z0-9-]+)/(?<visibility>[a-zA-Z0-9-]+/)?_packaging/(?<feed>.+)/nuget/v3/index.json";

        public static readonly TargetFeedContentType[] InstallersAndSymbols = {
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
        };

        public static readonly TargetFeedContentType[] InstallersAndChecksums = {
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
        };

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
        public const string FeedForChecksums = "https://dotnetclichecksums.blob.core.windows.net/dotnet/index.json";
        public const string FeedForInstallers = "https://dotnetcli.blob.core.windows.net/dotnet/index.json";

        private const string FeedInternalForChecksums = "https://dotnetclichecksumsmsrc.blob.core.windows.net/dotnet/index.json";
        private const string FeedInternalForInstallers = "https://dotnetclimsrc.blob.core.windows.net/dotnet/index.json";

        public const string FeedStagingForInstallers = "https://dotnetbuilds.blob.core.windows.net/public";
        public const string FeedStagingForChecksums = "https://dotnetbuilds.blob.core.windows.net/public-checksums";

        public const string FeedStagingInternalForInstallers = "https://dotnetbuilds.blob.core.windows.net/internal";
        public const string FeedStagingInternalForChecksums = "https://dotnetbuilds.blob.core.windows.net/internal-checksums";

        private const string FeedGeneralTesting = "https://pkgs.dev.azure.com/dnceng/public/_packaging/general-testing/nuget/v3/index.json";

        private const string FeedDotNetExperimental = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-experimental/nuget/v3/index.json";
        
        public const string FeedDotNetEng = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json";

        private const string FeedDotNetTools = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json";

        private const string FeedDotNetToolsInternal = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet-tools-internal/nuget/v3/index.json";

        private const string FeedDotNet31Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet3.1/nuget/v3/index.json";
        private const string FeedDotNet31Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet3.1-transport/nuget/v3/index.json";

        private const string FeedDotNet31InternalShipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet3.1-internal/nuget/v3/index.json";
        private const string FeedDotNet31InternalTransport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet3.1-internal-transport/nuget/v3/index.json";

        private const string FeedDotNet31Blazor = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet3.1-blazor/nuget/v3/index.json";

        private const string FeedDotNet5Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet5/nuget/v3/index.json";
        private const string FeedDotNet5Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet5-transport/nuget/v3/index.json";

        private const string FeedDotNet6Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6/nuget/v3/index.json";
        private const string FeedDotNet6Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6-transport/nuget/v3/index.json";

        private const string FeedDotNet7Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7/nuget/v3/index.json";
        private const string FeedDotNet7Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7-transport/nuget/v3/index.json";

        private const string FeedDotNet8Shipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet8/nuget/v3/index.json";
        private const string FeedDotNet8Transport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet8-transport/nuget/v3/index.json";

        private const string FeedDotNet7InternalShipping = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet7-internal/nuget/v3/index.json";
        private const string FeedDotNet7InternalTransport = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet7-internal-transport/nuget/v3/index.json";

        private const string FeedDotNet6InternalShipping = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet6-internal/nuget/v3/index.json";
        private const string FeedDotNet6InternalTransport = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet6-internal-transport/nuget/v3/index.json";

        private const string FeedDotNet5InternalShipping = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet5-internal/nuget/v3/index.json";
        private const string FeedDotNet5InternalTransport = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet5-internal-transport/nuget/v3/index.json";

        private const string FeedDotNetLibrariesShipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-libraries/nuget/v3/index.json";
        private const string FeedDotNetLibrariesTransport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-libraries-transport/nuget/v3/index.json";

        private const string FeedGeneralTestingInternal = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/general-testing-internal/nuget/v3/index.json";

        private const SymbolTargetType InternalSymbolTargets = SymbolTargetType.SymWeb;
        private const SymbolTargetType PublicAndInternalSymbolTargets = SymbolTargetType.Msdl | SymbolTargetType.SymWeb;

        private static List<string> FilenamesToExclude = new List<string>() { 
            "MergedManifest.xml"
        };

        private static TargetFeedSpecification[] DotNet31Feeds =
        {
            (TargetFeedContentType.Package, FeedDotNet31Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet31Transport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedForInstallers),
            (TargetFeedContentType.Checksum, FeedForChecksums),
        };

        private static TargetFeedSpecification[] DotNet31InternalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet31InternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet31InternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet31BlazorFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet31Blazor, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet31Blazor, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet5Feeds =
        {
            (TargetFeedContentType.Package, FeedDotNet5Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet5Transport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedForInstallers),
            (TargetFeedContentType.Checksum, FeedForChecksums),
        };

        private static TargetFeedSpecification[] DotNet5InternalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet5InternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet5InternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet6Feeds =
        {
            (TargetFeedContentType.Package, FeedDotNet6Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet6Transport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet6InternalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet6InternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet6InternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet7Feeds =
        {
            (TargetFeedContentType.Package, FeedDotNet7Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet7Transport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet7InternalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet7InternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet7InternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet8Feeds =
        {
            (TargetFeedContentType.Package, FeedDotNet8Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet8Transport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNetEngFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNetEng, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNetEng, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNetToolsFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNetTools, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNetTools, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNetToolsInternalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNetToolsInternal, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNetToolsInternal, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNetExperimentalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNetExperimental, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNetExperimental, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] GeneralTestingFeeds =
        {
            (TargetFeedContentType.Package, FeedGeneralTesting, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedGeneralTesting, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] GeneralTestingInternalFeeds =
        {
            (TargetFeedContentType.Package, FeedGeneralTestingInternal, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedGeneralTestingInternal, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };
        #endregion

        #region Target Channel Configs
        public static readonly List<TargetChannelConfig> ChannelInfos = new List<TargetChannelConfig>() {

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
            //    - symbolTargetType: List of symbol targets. Internal channels should use InternalSymbolTargets and public channels should use PublicAndInternalSymbolTargets
            //    - filenamesToExclude: Usually left as FilenamesToExclude.

            // .NET 3 Eng,
            new TargetChannelConfig(
                id: 344,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET 3 Eng - Validation,
            new TargetChannelConfig(
                id: 390,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET 5,
            new TargetChannelConfig(
                id: 1299,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "5.0" },
                targetFeeds: DotNet5Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 5 Eng,
            new TargetChannelConfig(
                id: 1495,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "eng/net5" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET 5 Eng - Validation,
            new TargetChannelConfig(
                id: 1496,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "eng/net5validation" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET 5 Internal,
            new TargetChannelConfig(
                id: 1300,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "internal/5.0" },
                targetFeeds: DotNet5InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 5.0.2xx SDK,
            new TargetChannelConfig(
                id: 1518,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "5.0.2xx" },
                targetFeeds: DotNet5Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 5.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 1519,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "internal/5.0.2xx" },
                targetFeeds: DotNet5InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 5.0.3xx SDK,
            new TargetChannelConfig(
                id: 1754,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "5.0.3xx" },
                targetFeeds: DotNet5Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 5.0.3xx SDK Internal,
            new TargetChannelConfig(
                id: 1755,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "internal/5.0.3xx" },
                targetFeeds: DotNet5InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 5.0.4xx SDK,
            new TargetChannelConfig(
                id: 1985,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "5.0.4xx", "5.0" },
                targetFeeds: DotNet5Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 5.0.4xx SDK Internal,
            new TargetChannelConfig(
                id: 1986,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "internal/5.0.4xx" },
                targetFeeds: DotNet5InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 5.0.5xx SDK Internal,
            new TargetChannelConfig(
                id: 2788,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "internal/5.0.5xx" },
                targetFeeds: DotNet5InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6,
            new TargetChannelConfig(
                id: 1296,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "6.0" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6 Eng,
            new TargetChannelConfig(
                id: 2293,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "eng/net6" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6 Eng - Validation,
            new TargetChannelConfig(
                id: 2294,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "eng/net6validation" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET 6 Internal,
            new TargetChannelConfig(
                id: 2097,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/6.0" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6 MAUI,
            new TargetChannelConfig(
                id: 2453,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "maui/6.0" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6 Private,
            new TargetChannelConfig(
                id: 2693,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/6.0-private" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.1xx SDK,
            new TargetChannelConfig(
                id: 1792,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "6.0.1xx" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.1xx SDK Internal,
            new TargetChannelConfig(
                id: 2098,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/6.0.1xx" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.2xx SDK,
            new TargetChannelConfig(
                id: 2434,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "6.0.2xx" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 2435,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/6.0.2xx" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.3xx SDK,
            new TargetChannelConfig(
                id: 2551,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "6.0.3xx" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.3xx SDK Internal,
            new TargetChannelConfig(
                id: 2552,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/6.0.3xx"},
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.4xx SDK,
            new TargetChannelConfig(
                id: 2696,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "6.0.4xx", "6.0" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.4xx SDK Internal,
            new TargetChannelConfig(
                id: 2697,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/6.0.4xx", "internal/6.0" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.Nxx SDK Private,
            new TargetChannelConfig(
                id: 2695,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/6.0.Nxx-private" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 7,
            new TargetChannelConfig(
                id: 2236,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "7.0" },
                targetFeeds: DotNet7Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 7 Eng,
            new TargetChannelConfig(
                id: 3114,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "eng/net7" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 7 Eng - Validation,
            new TargetChannelConfig(
                id: 3115,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Next,
                akaMSChannelNames: new List<string>() { "eng/net7validation" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET 7 Internal,
            new TargetChannelConfig(
                id: 3035,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/7.0" },
                targetFeeds: DotNet7InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 7.0.1xx SDK,
            new TargetChannelConfig(
                id: 2237,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "7.0.1xx", "7.0" },
                targetFeeds: DotNet7Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 7.0.1xx SDK Internal,
            new TargetChannelConfig(
                id: 3038,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/7.0.1xx", "internal/7.0" },
                targetFeeds: DotNet7InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 7.0.2xx SDK,
            new TargetChannelConfig(
                id: 3259,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "7.0.2xx" },
                targetFeeds: DotNet7Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 7.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 3260,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/7.0.2xx" },
                targetFeeds: DotNet7InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 7.0.3xx SDK,
            new TargetChannelConfig(
                id: 3436,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "7.0.3xx" },
                targetFeeds: DotNet7Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 7.0.3xx SDK Internal,
            new TargetChannelConfig(
                id: 3435,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/7.0.3xx" },
                targetFeeds: DotNet7InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8,
            new TargetChannelConfig(
                id: 3073,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "8.0" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8 Preview 1,
            new TargetChannelConfig(
                id: 3437,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "8.0-preview1" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),
            
            // .NET 8 Preview 2,
            new TargetChannelConfig(
                id: 3438,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "8.0-preview2" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8 Preview 3,
            new TargetChannelConfig(
                id: 3439,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "8.0-preview3" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8 Preview 4,
            new TargetChannelConfig(
                id: 3440,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "8.0-preview4" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.1xx SDK,
            new TargetChannelConfig(
                id: 3074,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "8.0.1xx", "8.0" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.1xx SDK Preview 1,
            new TargetChannelConfig(
                id: 3441,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "8.0.1xx-preview1", "8.0-preview1" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.1xx SDK Preview 2,
            new TargetChannelConfig(
                id: 3442,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "8.0.1xx-preview2", "8.0-preview2" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.1xx SDK Preview 3,
            new TargetChannelConfig(
                id: 3443,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "8.0.1xx-preview3", "8.0-preview3" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.1xx SDK Preview 4,
            new TargetChannelConfig(
                id: 3444,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "8.0.1xx-preview4", "8.0-preview4" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core 3.1 Internal Servicing,
            new TargetChannelConfig(
                id: 550,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/3.1" },
                targetFeeds: DotNet31InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core 3.1 Release,
            new TargetChannelConfig(
                id: 129,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "3.1" },
                targetFeeds: DotNet31Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core Experimental,
            new TargetChannelConfig(
                id: 562,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetExperimentalFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // NET Core SDK 3.1.1xx,
            new TargetChannelConfig(
                id: 560,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "3.1.1xx" },
                targetFeeds: DotNet31Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core SDK 3.1.1xx Internal,
            new TargetChannelConfig(
                id: 559,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/3.1.1xx" },
                targetFeeds: DotNet31InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core SDK 3.1.2xx,
            new TargetChannelConfig(
                id: 558,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "3.1.2xx" },
                targetFeeds: DotNet31Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core SDK 3.1.2xx Internal,
            new TargetChannelConfig(
                id: 557,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/3.1.2xx" },
                targetFeeds: DotNet31InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core SDK 3.1.3xx,
            new TargetChannelConfig(
                id: 759,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "3.1.3xx" },
                targetFeeds: DotNet31Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core SDK 3.1.3xx Internal,
            new TargetChannelConfig(
                id: 760,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/3.1.3xx" },
                targetFeeds: DotNet31InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core SDK 3.1.4xx,
            new TargetChannelConfig(
                id: 921,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "3.1.4xx" },
                targetFeeds: DotNet31Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core SDK 3.1.4xx Internal,
            new TargetChannelConfig(
                id: 922,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "internal/3.1.4xx" },
                targetFeeds: DotNet31InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core Tooling Dev,
            new TargetChannelConfig(
                id: 548,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Core Tooling Release,
            new TargetChannelConfig(
                id: 549,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Eng - Latest,
            new TargetChannelConfig(
                id: 2,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "eng" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET Eng - Validation,
            new TargetChannelConfig(
                id: 9,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "eng/validation" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET Eng Services - Int,
            new TargetChannelConfig(
                id: 678,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Eng Services - Prod,
            new TargetChannelConfig(
                id: 679,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Internal Tooling,
            new TargetChannelConfig(
                id: 551,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsInternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Libraries,
            new TargetChannelConfig(
                id: 1648,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: new TargetFeedSpecification[]
                {
                    (TargetFeedContentType.Package, FeedDotNetLibrariesShipping, AssetSelection.ShippingOnly),
                    (TargetFeedContentType.Package, FeedDotNetLibrariesTransport, AssetSelection.NonShippingOnly),
                    (InstallersAndSymbols, FeedForInstallers),
                    (TargetFeedContentType.Checksum, FeedForChecksums),
                },
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // General Testing,
            new TargetChannelConfig(
                id: 529,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "generaltesting" },
                targetFeeds: GeneralTestingFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets),

            // General Testing Internal,
            new TargetChannelConfig(
                id: 1647,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>() { "generaltestinginternal" },
                targetFeeds: GeneralTestingInternalFeeds,
                symbolTargetType: InternalSymbolTargets),

            // VS 16.6,
            new TargetChannelConfig(
                id: 1010,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 16.7,
            new TargetChannelConfig(
                id: 1011,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 16.8,
            new TargetChannelConfig(
                id: 1154,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 16.9,
            new TargetChannelConfig(
                id: 1473,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 16.10,
            new TargetChannelConfig(
                id: 1692,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 16.11,
            new TargetChannelConfig(
                id: 1926,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 17.0,
            new TargetChannelConfig(
                id: 1853,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 17.1,
            new TargetChannelConfig(
                id: 2346,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 17.2,
            new TargetChannelConfig(
                id: 2542,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            
            // VS 17.3,
            new TargetChannelConfig(
                id: 2692,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            
            // VS 17.4,
            new TargetChannelConfig(
                id: 2914,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 17.5,
            new TargetChannelConfig(
                id: 3257,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 17.6,
            new TargetChannelConfig(
                id: 3434,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.All,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
        };
        #endregion
    }
}
