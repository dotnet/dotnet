// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Configuration;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class PackageSourceFormatterTests : FormatterTests
    {
        private static readonly string Name = "a";
        private static readonly string Source = "https://nuget.test";

        [Theory]
        [MemberData(nameof(PackageSources))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(PackageSource expectedResult)
        {
            PackageSource? actualResult = SerializeThenDeserialize(PackageSourceFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.Name, actualResult!.Name);
            Assert.Equal(expectedResult.Source, actualResult.Source);
            Assert.Equal(expectedResult.IsEnabled, actualResult.IsEnabled);
            Assert.Equal(expectedResult.IsMachineWide, actualResult.IsMachineWide);
        }

        public static TheoryData PackageSources => new TheoryData<PackageSource>
            {
                { new PackageSource(Name, Source, isEnabled: true) },
                { new PackageSource(Name, Source, isEnabled: true) { IsMachineWide = true } },
                { new PackageSource(Name, Source, isEnabled: false) { IsMachineWide = false } }
            };
    }
}
