// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class SearchResultContextInfoFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(SearchResultContextInfo expectedResult)
        {
            var formatters = new IMessagePackFormatter[]
            {
                PackageSearchMetadataContextInfoFormatter.Instance,
                PackageVulnerabilityMetadataContextInfoFormatter.Instance,
            };
            var resolvers = new IFormatterResolver[] { MessagePackSerializerOptions.Standard.Resolver };
            var options = MessagePackSerializerOptions.Standard.WithSecurity(MessagePackSecurity.UntrustedData).WithResolver(CompositeResolver.Create(formatters, resolvers));

            SearchResultContextInfo? actualResult = SerializeThenDeserialize(SearchResultContextInfoFormatter.Instance, expectedResult, options);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.OperationId, actualResult!.OperationId);
            Assert.Equal(expectedResult.HasMoreItems, actualResult.HasMoreItems);
            Assert.Equal(expectedResult.SourceLoadingStatus, actualResult.SourceLoadingStatus);
            Assert.Equal(expectedResult.PackageSearchItems.Count, actualResult.PackageSearchItems.Count);
        }

        private static List<IPackageSearchMetadata> PackageSearchMetadata = new List<IPackageSearchMetadata>()
        {
             new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
             {
                Identity = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("4.3.0")),
                PackageDetailsUrl = new Uri("http://nuget.org"),
                Authors = "Microsoft",
                Vulnerabilities = new List<PackageVulnerabilityMetadata> { JsonExtensions.FromJson<PackageVulnerabilityMetadata>("{ 'AdvisoryUrl': 'http://www.nuget.org', 'Severity': '2' }") }
             }
        };


        public static TheoryData<SearchResultContextInfo> TestData => new()
            {
                { new SearchResultContextInfo() },
                { new SearchResultContextInfo(Guid.NewGuid()) },
                { new SearchResultContextInfo(
                    new List<PackageSearchMetadataContextInfo>(PackageSearchMetadata.Select(psm => PackageSearchMetadataContextInfo.Create(psm))),
                    new ReadOnlyDictionary<string, LoadingStatus>(new Dictionary<string, LoadingStatus>
                    {
                        { "Loading", LoadingStatus.Loading }
                    }),
                    hasMoreItems: true) },
                { new SearchResultContextInfo(new List<PackageSearchMetadataContextInfo>(PackageSearchMetadata.Select(psm => PackageSearchMetadataContextInfo.Create(psm))),
                    new ReadOnlyDictionary<string, LoadingStatus>(new Dictionary<string, LoadingStatus>
                    {
                        { "Loading", LoadingStatus.Loading }
                    }),
                    hasMoreItems: true,
                    operationId: Guid.NewGuid()) },
            };
    }
}
