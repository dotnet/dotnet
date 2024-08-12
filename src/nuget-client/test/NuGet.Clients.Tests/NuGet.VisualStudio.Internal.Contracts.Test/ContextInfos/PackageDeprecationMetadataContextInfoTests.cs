// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class PackageDeprecationMetadataContextInfoTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(PackageDeprecationMetadataContextInfo expectedResult)
        {
            PackageDeprecationMetadataContextInfo? actualResult = SerializeThenDeserialize(PackageDeprecationMetadataContextInfoFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.Message, actualResult!.Message);
            Assert.Equal(expectedResult.AlternatePackage is null, actualResult.AlternatePackage is null);

            if (expectedResult.AlternatePackage is object)
            {
                Assert.Equal(expectedResult.AlternatePackage.PackageId, actualResult.AlternatePackage!.PackageId);
                Assert.Equal(expectedResult.AlternatePackage.VersionRange, actualResult.AlternatePackage.VersionRange);
            }

            Assert.Equal(expectedResult.Reasons, actualResult.Reasons);
        }

        public static TheoryData<PackageDeprecationMetadataContextInfo> TestData => new()
            {
                {
                    new PackageDeprecationMetadataContextInfo(
                        "message",
                        new List<string> {"reason" },
                        new AlternatePackageMetadataContextInfo("packageid", new VersionRange(new NuGetVersion("1.0"))))
                }
            };
    }
}
