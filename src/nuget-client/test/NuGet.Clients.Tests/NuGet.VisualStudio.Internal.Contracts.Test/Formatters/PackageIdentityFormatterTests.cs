// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class PackageIdentityFormatterTests : FormatterTests
    {
        private static readonly string PackageId = "a";
        private static readonly NuGetVersion PackageVersion = NuGetVersion.Parse("1.2.3");

        [Theory]
        [MemberData(nameof(PackageIdentities))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(PackageIdentity expectedResult)
        {
            PackageIdentity? actualResult = SerializeThenDeserialize(PackageIdentityFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        public static TheoryData<PackageIdentity> PackageIdentities => new()
            {
                { new PackageIdentity(PackageId, version: null) },
                { new PackageIdentity(PackageId, PackageVersion) }
            };
    }
}
