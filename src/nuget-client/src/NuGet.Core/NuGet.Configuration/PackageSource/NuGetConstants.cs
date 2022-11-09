// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Configuration
{
    public static class NuGetConstants
    {
        public static readonly string NuGetHostName = "nuget.org";
        public static readonly string NuGetSymbolHostName = "nuget.smbsrc.net";

        public const string V3FeedUrl = "https://api.nuget.org/v3/index.json";
        public const string V2FeedUrl = "https://www.nuget.org/api/v2/";
        public static readonly string V2LegacyOfficialPackageSourceUrl = "https://nuget.org/api/v2/";
        public static readonly string V2LegacyFeedUrl = "https://go.microsoft.com/fwlink/?LinkID=230477";

        public static readonly string V1FeedUrl = "https://go.microsoft.com/fwlink/?LinkID=206669";

        /// <summary>
        /// NuGet.org gallery Url used as a source display name and as a default "id" when storing nuget.org API key.
        /// </summary>
        /// <remarks>
        /// Albeit this url is not actual feed we should keep it unchanged for back-compat with earlier NuGet versions.
        /// Typical scenario leading to adding this url to config file is to run setApiKey command without a source:
        /// nuget.exe setApiKey XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX
        /// </remarks>
        public static readonly string DefaultGalleryServerUrl = "https://www.nuget.org";

        public static readonly string ReadmeFileName = "readme.txt";
        public static readonly string NuGetSolutionSettingsFolder = ".nuget";

        public static readonly string PackageExtension = ".nupkg";
        public static readonly string SnupkgExtension = ".snupkg";
        public static readonly string SymbolsExtension = ".symbols" + PackageExtension;
        public static readonly string ManifestExtension = ".nuspec";
        public static readonly string ManifestSymbolsExtension = ".symbols" + ManifestExtension;
        public static readonly string ReadmeExtension = ".md";
        public static readonly string PackageReferenceFile = "packages.config";
        public static readonly string PackageSpecFileName = "project.json";

        public static readonly string FeedName = "nuget.org";

        public static readonly string DefaultConfigContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""nuget.org"" value=""https://api.nuget.org/v3/index.json"" protocolVersion=""3"" />
  </packageSources>
</configuration>";
    }
}
