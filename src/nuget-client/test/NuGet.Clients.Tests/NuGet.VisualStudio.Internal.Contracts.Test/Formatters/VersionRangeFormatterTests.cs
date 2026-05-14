// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class VersionRangeFormatterTests : FormatterTests
    {
        private static readonly NuGetVersion MinVersion = NuGetVersion.Parse("1.0.0");
        private static readonly NuGetVersion MaxVersion = NuGetVersion.Parse("2.0.0");
        private static readonly FloatRange FloatRange = new FloatRange(NuGetVersionFloatBehavior.PrereleaseRevision, MinVersion, "*");

        [Theory]
        [MemberData(nameof(VersionRanges))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(VersionRange expectedResult)
        {
            VersionRange? actualResult = SerializeThenDeserialize(VersionRangeFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        public static TheoryData<VersionRange> VersionRanges => new()
            {
                { new VersionRange(MinVersion) },
                { new VersionRange(MinVersion, FloatRange) },
                { new VersionRange(MinVersion, includeMinVersion: true, MaxVersion, includeMaxVersion: false, FloatRange) },
                { new VersionRange(MinVersion, includeMinVersion: false, MaxVersion, includeMaxVersion: true, FloatRange) }
            };
    }
}
