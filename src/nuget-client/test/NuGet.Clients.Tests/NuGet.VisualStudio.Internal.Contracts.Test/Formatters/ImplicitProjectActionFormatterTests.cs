// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class ImplicitProjectActionFormatterTests : FormatterTests
    {
        private static readonly string Id = "a";
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(id: "b", NuGetVersion.Parse("1.2.3"));

        [Theory]
        [MemberData(nameof(ImplicitProjectActions))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(ImplicitProjectAction expectedResult)
        {
            ImplicitProjectAction? actualResult = SerializeThenDeserialize(ImplicitProjectActionFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        public static TheoryData<ImplicitProjectAction> ImplicitProjectActions => new()
            {
                { new ImplicitProjectAction(Id, PackageIdentity, NuGetProjectActionType.Install) }
            };
    }
}
