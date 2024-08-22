// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using NuGet.Packaging;
using NuGet.Packaging.Licenses;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class LicenseMetadataFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(LicenseMetadata expectedResult)
        {
            var formatters = new IMessagePackFormatter[]
            {
                LicenseMetadataFormatter.Instance,
            };
            var resolvers = new IFormatterResolver[] { MessagePackSerializerOptions.Standard.Resolver };
            var options = MessagePackSerializerOptions.Standard.WithSecurity(MessagePackSecurity.UntrustedData).WithResolver(CompositeResolver.Create(formatters, resolvers));

            LicenseMetadata? actualResult = SerializeThenDeserialize(LicenseMetadataFormatter.Instance, expectedResult, options);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.License, actualResult!.License);
            Assert.Equal(expectedResult.Type, actualResult.Type);
            Assert.Equal(expectedResult.Version, actualResult.Version);
            Assert.Equal(expectedResult.WarningsAndErrors, actualResult.WarningsAndErrors);
        }

        public static TheoryData<LicenseMetadata> TestData => new()
            {
                new LicenseMetadata(LicenseType.Expression,
                    "MIT",
                    NuGetLicenseExpression.Parse("MIT"),
                    null,
                    LicenseMetadata.EmptyVersion),

                new LicenseMetadata(LicenseType.Expression,
                    "Apache-2.0",
                    NuGetLicenseExpression.Parse("Apache-2.0"),
                    null,
                    new Version("2.0")),

                new LicenseMetadata(LicenseType.File, "license.txt", null, null, LicenseMetadata.CurrentVersion),
            };
    }
}
