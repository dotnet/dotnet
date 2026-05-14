// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class AlternatePackageMetadataContextInfoFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(AlternatePackageMetadataContextInfo expectedResult)
        {
            AlternatePackageMetadataContextInfo? actualResult = SerializeThenDeserialize(AlternatePackageMetadataContextInfoFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.PackageId, actualResult!.PackageId);
            Assert.Equal(expectedResult.VersionRange, actualResult.VersionRange);
        }

        public static TheoryData<AlternatePackageMetadataContextInfo> TestData => new()
            {
                { new AlternatePackageMetadataContextInfo(packageId: "packageid", new VersionRange(new NuGetVersion("1.0"))) }
            };
    }
}
