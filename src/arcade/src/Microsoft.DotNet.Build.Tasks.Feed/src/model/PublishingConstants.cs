// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.DotNet.Build.Tasks.Feed.Model
{
    public class PublishingConstants
    {
        public static readonly string ExpectedFeedUrlSuffix = "index.json";

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
        public const string FeedStagingForInstallers = "https://dotnetbuilds.blob.core.windows.net/public";
        public const string FeedStagingForChecksums = "https://dotnetbuilds.blob.core.windows.net/public-checksums";

        public const string FeedStagingInternalForInstallers = "https://dotnetbuilds.blob.core.windows.net/internal";
        public const string FeedStagingInternalForChecksums = "https://dotnetbuilds.blob.core.windows.net/internal-checksums";

        private const string FeedGeneralTesting = "https://pkgs.dev.azure.com/dnceng/public/_packaging/general-testing/nuget/v3/index.json";

        private const string FeedDotNetExperimental = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-experimental/nuget/v3/index.json";
        
        public const string FeedDotNetEng = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json";

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

        private const string FeedDotNetLibrariesShipping = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-libraries/nuget/v3/index.json";
        private const string FeedDotNetLibrariesTransport = "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-libraries-transport/nuget/v3/index.json";

        private const string FeedDotNetLibrariesInternalShipping = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet-libraries-internal/nuget/v3/index.json";
        private const string FeedDotNetLibrariesInternalTransport = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/dotnet-libraries-internal-transport/nuget/v3/index.json";

        private const string FeedGeneralTestingInternal = "https://pkgs.dev.azure.com/dnceng/internal/_packaging/general-testing-internal/nuget/v3/index.json";

        private const SymbolTargetType InternalSymbolTargets = SymbolTargetType.SymWeb;
        private const SymbolTargetType PublicAndInternalSymbolTargets = SymbolTargetType.Msdl | SymbolTargetType.SymWeb;

        private static List<string> FilenamesToExclude = new List<string>() { 
            "MergedManifest.xml"
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

        private static TargetFeedSpecification[] DotNet8WorkloadFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet8Workloads, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet8Workloads, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet8InternalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet8InternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet8InternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet9Feeds =
        {
            (TargetFeedContentType.Package, FeedDotNet9Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet9Transport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet9InternalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet9InternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet9InternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet9WorkloadFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet9Workloads, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet9Workloads, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet10Feeds =
        {
            (TargetFeedContentType.Package, FeedDotNet10Shipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet10Transport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNet10InternalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet10InternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet10InternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
        };

        private static TargetFeedSpecification[] DotNet10WorkloadFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNet10Workloads, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNet10Workloads, AssetSelection.NonShippingOnly),
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

        private static TargetFeedSpecification[] DotNetLibrariesFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNetLibrariesShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNetLibrariesTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingForChecksums),
        };

        private static TargetFeedSpecification[] DotNetLibrariesInternalFeeds =
        {
            (TargetFeedContentType.Package, FeedDotNetLibrariesInternalShipping, AssetSelection.ShippingOnly),
            (TargetFeedContentType.Package, FeedDotNetLibrariesInternalTransport, AssetSelection.NonShippingOnly),
            (InstallersAndSymbols, FeedStagingInternalForInstallers),
            (TargetFeedContentType.Checksum, FeedStagingInternalForChecksums),
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

            // .NET 6,
            new TargetChannelConfig(
                id: 1296,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "6.0" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6 Eng,
            new TargetChannelConfig(
                id: 2293,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "eng/net6" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6 Eng - Validation,
            new TargetChannelConfig(
                id: 2294,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "eng/net6validation" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET 6 Internal,
            new TargetChannelConfig(
                id: 2097,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/6.0" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6 Private,
            new TargetChannelConfig(
                id: 2693,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/6.0-private" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.1xx SDK,
            new TargetChannelConfig(
                id: 1792,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "6.0.1xx" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.1xx SDK Internal,
            new TargetChannelConfig(
                id: 2098,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/6.0.1xx" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.2xx SDK,
            new TargetChannelConfig(
                id: 2434,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "6.0.2xx" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 2435,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/6.0.2xx" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.3xx SDK,
            new TargetChannelConfig(
                id: 2551,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "6.0.3xx" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.3xx SDK Internal,
            new TargetChannelConfig(
                id: 2552,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/6.0.3xx"},
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.4xx SDK,
            new TargetChannelConfig(
                id: 2696,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "6.0.4xx", "6.0" },
                targetFeeds: DotNet6Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.4xx SDK Internal,
            new TargetChannelConfig(
                id: 2697,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/6.0.4xx", "internal/6.0" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 6.0.Nxx SDK Private,
            new TargetChannelConfig(
                id: 2695,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/6.0.Nxx-private" },
                targetFeeds: DotNet6InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8,
            new TargetChannelConfig(
                id: 3073,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "8.0" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8 Workload Release,
            new TargetChannelConfig(
                id: 4610,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "8.0-workloads" },
                targetFeeds: DotNet8WorkloadFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8 Eng,
            new TargetChannelConfig(
                id: 3885,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "eng/net8" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8 Eng - Validation,
            new TargetChannelConfig(
                id: 3886,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "eng/net8validation" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET 8 Internal,
            new TargetChannelConfig(
                id: 3880,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/8.0" },
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8 Private,
            new TargetChannelConfig(
                id: 4120,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/8.0-private" },
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.1xx SDK,
            new TargetChannelConfig(
                id: 3074,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "8.0.1xx" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.1xx SDK Internal,
            new TargetChannelConfig(
                id: 3881,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/8.0.1xx" },
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.2xx SDK,
            new TargetChannelConfig(
                id: 4036,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "8.0.2xx" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 4266,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/8.0.2xx" },
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.3xx SDK,
            new TargetChannelConfig(
                id: 4267,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "8.0.3xx", "8.0" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.3xx SDK Internal,
            new TargetChannelConfig(
                id: 4268,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/8.0.3xx", "internal/8.0" },
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.4xx SDK,
            new TargetChannelConfig(
                id: 4586,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "8.0.4xx" },
                targetFeeds: DotNet8Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 8.0.4xx SDK Internal,
            new TargetChannelConfig(
                id: 4609,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/8.0.4xx" },
                targetFeeds: DotNet8InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9,
            new TargetChannelConfig(
                id: 3883,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "9.0" },
                targetFeeds: DotNet9Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9 Eng,
            new TargetChannelConfig(
                id: 5175,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "eng/net9" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9 Eng - Validation,
            new TargetChannelConfig(
                id: 5176,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "eng/net9validation" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET 9 Internal,
            new TargetChannelConfig(
                id: 5128,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/9.0" },
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9 Private,
            new TargetChannelConfig(
                id: 5129,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/9.0-private" },
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9 Workload Release,
            new TargetChannelConfig(
                id: 4611,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "9.0-workloads" },
                targetFeeds: DotNet9WorkloadFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9.0.1xx SDK,
            new TargetChannelConfig(
                id: 3884,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "9.0.1xx", "9.0" },
                targetFeeds: DotNet9Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9.0.1xx SDK Internal,
            new TargetChannelConfig(
                id: 5127,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/9.0.1xx", "internal/9.0" },
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9.0.2xx SDK,
            new TargetChannelConfig(
                id: 5286,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "9.0.2xx" },
                targetFeeds: DotNet9Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9.0.2xx SDK Internal,
            new TargetChannelConfig(
                id: 5287,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "internal/9.0.2xx" },
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9.0.3xx SDK,
            new TargetChannelConfig(
                id: 6417,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["9.0.3xx"],
                targetFeeds: DotNet9Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 9.0.3xx SDK Internal,
            new TargetChannelConfig(
                id: 6418,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/9.0.3xx"],
                targetFeeds: DotNet9InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10,
            new TargetChannelConfig(
                id: 5172,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "10.0" },
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 Workload Release,
            new TargetChannelConfig(
                id: 5174,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "10.0-workloads" },
                targetFeeds: DotNet10WorkloadFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx SDK,
            new TargetChannelConfig(
                id: 5173,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "10.0.1xx", "10.0" },
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 Preview 1,
            new TargetChannelConfig(
                id: 6545,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0-preview1"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 Preview 2,
            new TargetChannelConfig(
                id: 6547,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0-preview2"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 Preview 3,
            new TargetChannelConfig(
                id: 6549,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0-preview3"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 Preview 4,
            new TargetChannelConfig(
                id: 6551,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0-preview4"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 Preview 5,
            new TargetChannelConfig(
                id: 6553,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0-preview5"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 Preview 6,
            new TargetChannelConfig(
                id: 6555,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0-preview6"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 Preview 7,
            new TargetChannelConfig(
                id: 6557,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0-preview7"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 RC 1,
            new TargetChannelConfig(
                id: 6494,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0-rc1"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 RC 1 Internal,
            new TargetChannelConfig(
                id: 6496,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0-rc1"],
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 RC 2,
            new TargetChannelConfig(
                id: 6498,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0-rc2"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10 RC 2 Internal,
            new TargetChannelConfig(
                id: 6500,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0-rc2"],
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx RC 1,
            new TargetChannelConfig(
                id: 6573,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0.1xx-rc1"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx RC 1 Internal,
            new TargetChannelConfig(
                id: 6575,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0.1xx-rc1"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx RC 2,
            new TargetChannelConfig(
                id: 6577,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0.1xx-rc2"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx RC 2 Internal,
            new TargetChannelConfig(
                id: 6579,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["internal/10.0.1xx-rc2"],
                targetFeeds: DotNet10InternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx SDK Preview 1,
            new TargetChannelConfig(
                id: 6476,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0.1xx-preview1"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx SDK Preview 2,
            new TargetChannelConfig(
                id: 6478,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0.1xx-preview2"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx SDK Preview 3,
            new TargetChannelConfig(
                id: 6484,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0.1xx-preview3"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx SDK Preview 4,
            new TargetChannelConfig(
                id: 6486,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0.1xx-preview4"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx SDK Preview 5,
            new TargetChannelConfig(
                id: 6488,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0.1xx-preview5"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx SDK Preview 6,
            new TargetChannelConfig(
                id: 6490,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0.1xx-preview6"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET 10.0.1xx SDK Preview 7,
            new TargetChannelConfig(
                id: 6492,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: ["10.0.1xx-preview7"],
                targetFeeds: DotNet10Feeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude),

            // .NET Core Experimental,
            new TargetChannelConfig(
                id: 562,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetExperimentalFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Core Tooling Dev,
            new TargetChannelConfig(
                id: 548,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Core Tooling Release,
            new TargetChannelConfig(
                id: 549,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Eng - Latest,
            new TargetChannelConfig(
                id: 2,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "eng" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET Eng - Validation,
            new TargetChannelConfig(
                id: 9,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "eng/validation" },
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                flatten: false),

            // .NET Eng Services - Int,
            new TargetChannelConfig(
                id: 678,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Eng Services - Prod,
            new TargetChannelConfig(
                id: 679,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetEngFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Internal Tooling,
            new TargetChannelConfig(
                id: 551,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsInternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Libraries,
            new TargetChannelConfig(
                id: 1648,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetLibrariesFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET Libraries Internal,
            new TargetChannelConfig(
                id: 3882,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetLibrariesInternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            
            // .NET AP 1,
            new TargetChannelConfig(
                id: 4122,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsInternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            
            // .NET AP 2,
            new TargetChannelConfig(
                id: 4123,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsInternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // .NET AP 3,
            new TargetChannelConfig(
                id: 4124,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsInternalFeeds,
                symbolTargetType: InternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // General Testing,
            new TargetChannelConfig(
                id: 529,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "generaltesting" },
                targetFeeds: GeneralTestingFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets),

            // General Testing Internal,
            new TargetChannelConfig(
                id: 1647,
                isInternal: true,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>() { "generaltestinginternal" },
                targetFeeds: GeneralTestingInternalFeeds,
                symbolTargetType: InternalSymbolTargets),

            // VS 16.6,
            new TargetChannelConfig(
                id: 1010,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 16.7,
            new TargetChannelConfig(
                id: 1011,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 16.8,
            new TargetChannelConfig(
                id: 1154,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 16.9,
            new TargetChannelConfig(
                id: 1473,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 16.10,
            new TargetChannelConfig(
                id: 1692,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 16.11,
            new TargetChannelConfig(
                id: 1926,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 17.0,
            new TargetChannelConfig(
                id: 1853,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 17.1,
            new TargetChannelConfig(
                id: 2346,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 17.2,
            new TargetChannelConfig(
                id: 2542,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            
            // VS 17.3,
            new TargetChannelConfig(
                id: 2692,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            
            // VS 17.4,
            new TargetChannelConfig(
                id: 2914,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 17.5,
            new TargetChannelConfig(
                id: 3257,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),

            // VS 17.6,
            new TargetChannelConfig(
                id: 3434,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            // VS 17.7,
            new TargetChannelConfig(
                id: 3581,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            // VS 17.8,
            new TargetChannelConfig(
                id: 3582,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            // VS 17.9,
            new TargetChannelConfig(
                id: 4015,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            // VS 17.10
            new TargetChannelConfig(
                id: 4165,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            // VS 17.11
            new TargetChannelConfig(
                id: 4544,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
                // VS 17.12
            new TargetChannelConfig(
                id: 4906,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            // VS 17.13
            new TargetChannelConfig(
                id: 5288,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
            // VS 17.14
            new TargetChannelConfig(
                id: 6136,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: new List<string>(),
                targetFeeds: DotNetToolsFeeds,
                symbolTargetType: PublicAndInternalSymbolTargets,
                filenamesToExclude: FilenamesToExclude,
                flatten: false),
        };
        #endregion
    }
}
