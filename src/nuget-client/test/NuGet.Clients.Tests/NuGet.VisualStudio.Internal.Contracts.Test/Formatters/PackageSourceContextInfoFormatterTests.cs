// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class PackageSourceContextInfoFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(PackageSourceContextInfo expectedResult)
        {
            PackageSourceContextInfo? actualResult = SerializeThenDeserialize(PackageSourceContextInfoFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        public static TheoryData TestData => new TheoryData<PackageSourceContextInfo>
            {
                { new PackageSourceContextInfo("source", "name", isEnabled: true, protocolVersion: 3) },
                { new PackageSourceContextInfo("source", "name", isEnabled: true) },
                { new PackageSourceContextInfo("source", "name") },
                { new PackageSourceContextInfo("source") }
            };
    }
}
