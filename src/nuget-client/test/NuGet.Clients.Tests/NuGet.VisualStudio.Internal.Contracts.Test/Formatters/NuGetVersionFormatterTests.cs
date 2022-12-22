// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class NuGetVersionFormatterTests : FormatterTests
    {
        private static readonly NuGetVersion PackageVersion = NuGetVersion.Parse("1.2.3");
        private static readonly NuGetVersion PackageVersionWithPrerelease = NuGetVersion.Parse("1.2.3-xyz");
        private static readonly NuGetVersion PackageVersionWithBuild = NuGetVersion.Parse("1.2.3+456");
        private static readonly NuGetVersion PackageVersionWithPrereleaseAndBuild = NuGetVersion.Parse("1.2.3-xyz+456");

        [Theory]
        [MemberData(nameof(Versions))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(NuGetVersion expectedResult)
        {
            NuGetVersion? actualResult = SerializeThenDeserialize(NuGetVersionFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedResult.OriginalVersion, actualResult!.OriginalVersion);
            Assert.Equal(expectedResult.ToString(), actualResult.ToString());
        }

        public static TheoryData Versions => new TheoryData<NuGetVersion>
            {
                { PackageVersion },
                { PackageVersionWithPrerelease },
                { PackageVersionWithBuild },
                { PackageVersionWithPrereleaseAndBuild }
            };
    }
}
