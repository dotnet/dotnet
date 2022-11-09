// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Configuration;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class PackageSourceUpdateOptionsFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(PackageSourceUpdateOptions))]
#pragma warning disable CS0618 // Type or member is obsolete

        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(PackageSourceUpdateOptions expectedResult)
        {
            PackageSourceUpdateOptions? actualResult = SerializeThenDeserialize(PackageSourceUpdateOptionsFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.UpdateCredentials, actualResult!.UpdateCredentials);
            Assert.Equal(expectedResult.UpdateEnabled, actualResult.UpdateEnabled);
        }

        public static TheoryData PackageSourceUpdateOptions => new TheoryData<PackageSourceUpdateOptions>
            {
                { new PackageSourceUpdateOptions(updateCredentials: true, updateEnabled: false) },
                { new PackageSourceUpdateOptions(updateCredentials: false, updateEnabled: true) },
            };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
