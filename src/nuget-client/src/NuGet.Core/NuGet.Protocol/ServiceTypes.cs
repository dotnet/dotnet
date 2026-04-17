// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol
{
    public static class ServiceTypes
    {
        public static readonly string Version200 = "/2.0.0";
        public static readonly string Version300beta = "/3.0.0-beta";
        public static readonly string Version300rc = "/3.0.0-rc";
        public static readonly string Version300 = "/3.0.0";
        public static readonly string Version340 = "/3.4.0";
        public static readonly string Version360 = "/3.6.0";
        public static readonly string Versioned = "/Versioned";
        public static readonly string Version470 = "/4.7.0";
        public static readonly string Version490 = "/4.9.0";
        public static readonly string Version500 = "/5.0.0";
        public static readonly string Version510 = "/5.1.0";
        internal const string Version670 = "/6.7.0";
        internal const string Version6110 = "/6.11.0";
        internal const string Version6130 = "/6.13.0";

        public static readonly string[] SearchQueryService = { "SearchQueryService" + Versioned, "SearchQueryService" + Version340, "SearchQueryService" + Version300beta };
        public static readonly string[] RegistrationsBaseUrl = { $"RegistrationsBaseUrl{Versioned}", $"RegistrationsBaseUrl{Version360}", $"RegistrationsBaseUrl{Version340}", $"RegistrationsBaseUrl{Version300rc}", $"RegistrationsBaseUrl{Version300beta}", "RegistrationsBaseUrl" };
        public static readonly string[] SearchAutocompleteService = { "SearchAutocompleteService" + Versioned, "SearchAutocompleteService" + Version300beta };
        public static readonly string[] ReportAbuse = { "ReportAbuseUriTemplate" + Versioned, "ReportAbuseUriTemplate" + Version300 };
        internal static readonly string[] ReadmeFileUrl = { "ReadmeUriTemplate" + Versioned, "ReadmeUriTemplate" + Version6130 };
        public static readonly string[] PackageDetailsUriTemplate = { "PackageDetailsUriTemplate" + Version510 };
        public static readonly string[] LegacyGallery = { "LegacyGallery" + Versioned, "LegacyGallery" + Version200 };
        public static readonly string[] PackagePublish = { "PackagePublish" + Versioned, "PackagePublish" + Version200 };
        public static readonly string[] PackageBaseAddress = { "PackageBaseAddress" + Versioned, "PackageBaseAddress" + Version300 };
        public static readonly string[] RepositorySignatures = { "RepositorySignatures" + Version500, "RepositorySignatures" + Version490, "RepositorySignatures" + Version470 };
        public static readonly string[] SymbolPackagePublish = { "SymbolPackagePublish" + Version490 };
        internal static readonly string[] VulnerabilityInfo = { "VulnerabilityInfo" + Version670 };
        internal static readonly string[] OwnerDetailsUriTemplate = { "OwnerDetailsUriTemplate" + Version6110 };
    }
}
