// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class VersionInfoContextInfoFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(VersionInfoContextInfo expectedResult)
        {
            var formatters = new IMessagePackFormatter[]
                {
                    PackageSearchMetadataContextInfoFormatter.Instance,
                    PackageVulnerabilityMetadataContextInfoFormatter.Instance
                };
            var resolvers = new IFormatterResolver[] { MessagePackSerializerOptions.Standard.Resolver };
            var options = MessagePackSerializerOptions.Standard.WithSecurity(MessagePackSecurity.UntrustedData).WithResolver(CompositeResolver.Create(formatters, resolvers));

            VersionInfoContextInfo? actualResult = SerializeThenDeserialize(VersionInfoContextInfoFormatter.Instance, expectedResult, options);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.DownloadCount, actualResult!.DownloadCount);
            Assert.Equal(expectedResult.Version, actualResult.Version);
            Assert.Equal(expectedResult.PackageDeprecationMetadata is null, actualResult.PackageDeprecationMetadata is null);

            if (expectedResult.PackageDeprecationMetadata is object)
            {
                Assert.Equal(expectedResult.PackageDeprecationMetadata.Message, actualResult.PackageDeprecationMetadata!.Message);
            }

            Assert.Equal(expectedResult.PackageSearchMetadata is null, actualResult.PackageSearchMetadata is null);

            if (expectedResult.PackageSearchMetadata is object)
            {
                Assert.Equal(expectedResult.PackageSearchMetadata.Identity, actualResult.PackageSearchMetadata!.Identity);
            }
        }

        public static TheoryData TestData => new TheoryData<VersionInfoContextInfo>
            {
                {
                    new VersionInfoContextInfo(NuGetVersion.Parse("1.0.0"), 100)
                    {
                        PackageSearchMetadata = PackageSearchMetadataContextInfo.Create(new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
                        {
                            Identity = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("4.3.0")),
                        }),
                        PackageDeprecationMetadata = new PackageDeprecationMetadataContextInfo("message", new List<string>{"string1" }, new AlternatePackageMetadataContextInfo("packageId", new VersionRange(new NuGetVersion("0.1"))))
                    }
                }
            };
    }
}
