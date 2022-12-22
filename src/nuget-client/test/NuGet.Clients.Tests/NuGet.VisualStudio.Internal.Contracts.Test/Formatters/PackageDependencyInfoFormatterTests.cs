// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class PackageDependencyInfoFormatterTests : FormatterTests
    {
        private static readonly string PackageId = "a";
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(PackageId, NuGetVersion.Parse("1.0.0"));
        private static readonly VersionRange VersionRange = new VersionRange(NuGetVersion.Parse("1.0.0"));

        [Theory]
        [MemberData(nameof(PackageDependencyInfos))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(PackageDependencyInfo expectedResult)
        {
            PackageDependencyInfo? actualResult = SerializeThenDeserialize(PackageDependencyInfoFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        public static TheoryData PackageDependencyInfos => new TheoryData<PackageDependencyInfo>
            {
                { new PackageDependencyInfo(PackageIdentity.Id, PackageIdentity.Version) },
                { new PackageDependencyInfo(PackageIdentity, Enumerable.Empty<PackageDependency>()) },
                { new PackageDependencyInfo(PackageIdentity, new [] { new PackageDependency("b", VersionRange) }) },
            };
    }
}
