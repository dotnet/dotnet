// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class PackageDependencyFormatterTests : FormatterTests
    {
        private static readonly IReadOnlyList<string> Exclude = new[] { "b", "c" };
        private static readonly IReadOnlyList<string> Include = new[] { "d", "e" };
        private static readonly string PackageId = "a";
        private static readonly VersionRange VersionRange = new VersionRange(NuGetVersion.Parse("1.0.0"));

        [Theory]
        [MemberData(nameof(PackageDependencies))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(PackageDependency expectedResult)
        {
            PackageDependency? actualResult = SerializeThenDeserialize(PackageDependencyFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        public static TheoryData<PackageDependency> PackageDependencies => new()
            {
                { new PackageDependency(PackageId) },
                { new PackageDependency(PackageId, VersionRange) },
                { new PackageDependency(PackageId, VersionRange, Include, Exclude) }
            };
    }
}
