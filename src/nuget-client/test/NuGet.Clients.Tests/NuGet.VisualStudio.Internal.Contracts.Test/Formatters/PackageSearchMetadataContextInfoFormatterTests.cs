// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Licenses;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class PackageSearchMetadataContextInfoFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(PackageSearchMetadataContextInfo expectedResult)
        {
            var formatters = new IMessagePackFormatter[]
            {
                LicenseMetadataFormatter.Instance,
                PackageDependencyFormatter.Instance,
                PackageDependencyGroupFormatter.Instance,
                PackageVulnerabilityMetadataContextInfoFormatter.Instance,
            };
            var resolvers = new IFormatterResolver[] { MessagePackSerializerOptions.Standard.Resolver };
            var options = MessagePackSerializerOptions.Standard.WithSecurity(MessagePackSecurity.UntrustedData).WithResolver(CompositeResolver.Create(formatters, resolvers));

            PackageSearchMetadataContextInfo? actualResult = SerializeThenDeserialize(PackageSearchMetadataContextInfoFormatter.Instance, expectedResult, options);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.Identity, actualResult!.Identity);
            Assert.Equal(expectedResult.DependencySets, actualResult.DependencySets);
            Assert.Equal(expectedResult.Description, actualResult.Description);
            Assert.Equal(expectedResult.DownloadCount, actualResult.DownloadCount);
            Assert.Equal(expectedResult.IconUrl, actualResult.IconUrl);
            Assert.Equal(expectedResult.IsListed, actualResult.IsListed);
            Assert.Equal(expectedResult.RecommenderVersion, actualResult.RecommenderVersion);
            Assert.Equal(expectedResult.IsRecommended, actualResult.IsRecommended);
            Assert.Equal(expectedResult.LicenseUrl, actualResult.LicenseUrl);
            Assert.Equal(expectedResult.RequireLicenseAcceptance, actualResult.RequireLicenseAcceptance);
            Assert.Equal(expectedResult.Summary, actualResult.Summary);
            Assert.Equal(expectedResult.Tags, actualResult.Tags);
            Assert.Equal(expectedResult.Authors, actualResult.Authors);
            Assert.Equal(expectedResult.Owners, actualResult.Owners);
            Assert.Equal(expectedResult.PackageDetailsUrl, actualResult.PackageDetailsUrl);
            Assert.Equal(expectedResult.PrefixReserved, actualResult.PrefixReserved);
            Assert.Equal(expectedResult.ProjectUrl, actualResult.ProjectUrl);
            Assert.Equal(expectedResult.PackagePath, actualResult.PackagePath);
            Assert.Equal(expectedResult.Published, actualResult.Published);
            Assert.Equal(expectedResult.ReportAbuseUrl, actualResult.ReportAbuseUrl);
            Assert.Equal(expectedResult.Title, actualResult.Title);
            Assert.Equal(expectedResult.Vulnerabilities, actualResult.Vulnerabilities);
        }

        public static TheoryData TestData => new TheoryData<PackageSearchMetadataContextInfo>
            {
                { PackageSearchMetadataContextInfo.Create(new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
                    {
                        Identity = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("4.3.0")),
                        PackageDetailsUrl = new Uri("http://nuget.org"),
                        Authors = "authors",
                        DependencySets = new List<PackageDependencyGroup>{ new PackageDependencyGroup(new NuGetFramework(".NETFramework", new Version(4,5)), new List<PackageDependency>() { new PackageDependency("a") }) },
                        Vulnerabilities = new PackageVulnerabilityMetadata[] { JsonExtensions.FromJson<PackageVulnerabilityMetadata>("{ 'AdvisoryUrl': 'http://www.nuget.org', 'Severity': '2' }") },
                        Description = "description",
                        DownloadCount = 1000,
                        IconUrl = new Uri("http://nuget.org"),
                        IsListed = true,
                        LicenseMetadata = new LicenseMetadata(LicenseType.Expression, "MIT", NuGetLicenseExpression.Parse("MIT"), null, LicenseMetadata.EmptyVersion),
                        LicenseUrl = new Uri("http://nuget.org"),
                        Owners = "nuget",
                        PackagePath = @"c:\packages\package.nupkg",
                        PrefixReserved = true,
                        ProjectUrl = new Uri("http://nuget.org"),
                        Published = new DateTimeOffset(DateTime.Now.AddDays(-1)),
                        ReportAbuseUrl = new Uri("http://nuget.org"),
                        RequireLicenseAcceptance = true,
                        Summary = "summary",
                        Tags = "tags",
                        Title = "title"
                    })
                },
                { PackageSearchMetadataContextInfo.Create(new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata() { }) }
            };
    }
}
