// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol
{
    internal static class Types
    {
        public static readonly Uri PackageSearchResult = new Uri("http://schema.nuget.org/schema#PackageSearchResult");
        public static readonly Uri PackageIdentity = new Uri("http://schema.nuget.org/schema#PackageIdentity");
        public static readonly Uri PackageDescription = new Uri("http://schema.nuget.org/schema#PackageDescription");
        public static readonly Uri PackageLicensing = new Uri("http://schema.nuget.org/schema#PackageLicensing");
        public static readonly Uri PackageDependencies = new Uri("http://schema.nuget.org/schema#PackageDependencies");
        public static readonly Uri DependencyGroup = new Uri("http://schema.nuget.org/schema#DependencyGroup");
        public static readonly Uri Dependency = new Uri("http://schema.nuget.org/schema#Dependency");
        public static readonly Uri Stats = new Uri("http://schema.nuget.org/schema#Stats");
    }

    public static class JsonProperties
    {
        public const string Data = "data";

        public const string SubjectId = "@id";
        public const string Type = "@type";

        public const string PackageId = "id";
        public const string Version = "version";
        public const string Title = "title";
        public const string Summary = "summary";
        public const string Description = "description";
        public const string Authors = "authors";
        public const string Owners = "owners";
        public const string IconUrl = "iconUrl";
        public const string LicenseUrl = "licenseUrl";
        public const string LicenseExpression = "licenseExpression";
        public const string LicenseExpressionVersion = "licenseExpressionVersion";
        public const string ProjectUrl = "projectUrl";
        public const string ReadmeUrl = "readmeUrl";
        public const string Tags = "tags";
        public const string DownloadCount = "totalDownloads";
        public const string Created = "created";
        public const string LastEdited = "lastEdited";
        public const string Published = "published";
        public const string RequireLicenseAcceptance = "requireLicenseAcceptance";
        public const string DependencyGroups = "dependencyGroups";
        public const string LatestVersion = "latestVersion";
        public const string TargetFramework = "targetFramework";
        public const string Dependencies = "dependencies";
        public const string Range = "range";
        public const string MinimumClientVersion = "minClientVersion";
        public const string Language = "language";
        public const string PackageContent = "packageContent";
        public const string Versions = "versions";
        public const string PrefixReserved = "verified";
        public const string Listed = "listed";

        // repository signing certificate properties
        public const string Subject = "subject";
        public const string Issuer = "issuer";
        public const string NotBefore = "notBefore";
        public const string NotAfter = "notAfter";
        public const string ContentUrl = "contentUrl";
        public const string SigningCertificates = "signingCertificates";
        public const string AllRepositorySigned = "allRepositorySigned";
        public const string Fingerprints = "fingerprints";

        // deprecation properties
        public const string Deprecation = "deprecation";
        public const string DeprecationMessage = "message";
        public const string DeprecationReasons = "reasons";
        public const string AlternatePackage = "alternatePackage";

        // vulnerability properties
        public const string Vulnerabilities = "vulnerabilities";
        public const string AdvisoryUrl = "advisoryUrl";
        public const string Severity = "severity";
        public const string Url = "url";
    }
}
