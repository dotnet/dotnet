// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class PackageDeprecationMetadataContextInfoFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(PackageDeprecationMetadataContextInfo expectedResult)
        {
            var formatters = new IMessagePackFormatter[]
            {
                AlternatePackageMetadataContextInfoFormatter.Instance,
            };
            var resolvers = new IFormatterResolver[] { MessagePackSerializerOptions.Standard.Resolver };
            var options = MessagePackSerializerOptions.Standard.WithSecurity(MessagePackSecurity.UntrustedData).WithResolver(CompositeResolver.Create(formatters, resolvers));

            PackageDeprecationMetadataContextInfo? actualResult = SerializeThenDeserialize(PackageDeprecationMetadataContextInfoFormatter.Instance, expectedResult, options);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.Message, actualResult!.Message);
            Assert.Equal(expectedResult.Reasons.First(), actualResult.Reasons.First());
            Assert.Equal(expectedResult.AlternatePackage is null, actualResult.AlternatePackage is null);

            if (expectedResult.AlternatePackage is object)
            {
                Assert.Equal(expectedResult.AlternatePackage.PackageId, actualResult.AlternatePackage!.PackageId);
                Assert.Equal(expectedResult.AlternatePackage.VersionRange, actualResult.AlternatePackage.VersionRange);
            }
        }

        public static TheoryData<PackageDeprecationMetadataContextInfo> TestData => new()
            {
                { new PackageDeprecationMetadataContextInfo("message", new List<string>{"string1" }, new AlternatePackageMetadataContextInfo("packageId", new VersionRange(new NuGetVersion("0.1")))) },
                { new PackageDeprecationMetadataContextInfo(null, new List<string>{"string1" }, new AlternatePackageMetadataContextInfo("packageId", new VersionRange(new NuGetVersion("0.1")))) },
                { new PackageDeprecationMetadataContextInfo(string.Empty, new List<string>{"string1" }, new AlternatePackageMetadataContextInfo("packageId", new VersionRange(new NuGetVersion("0.1")))) },
            };
    }
}
