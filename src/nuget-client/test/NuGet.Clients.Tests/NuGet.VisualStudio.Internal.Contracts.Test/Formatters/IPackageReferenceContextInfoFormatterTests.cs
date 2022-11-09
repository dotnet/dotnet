// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class IPackageReferenceContextInfoFormatterTests : FormatterTests
    {
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(id: "a", NuGetVersion.Parse("1.2.3"));
        private static readonly NuGetFramework Framework = NuGetFramework.Parse("net50");
        private static readonly PackageReference PackageReference = new PackageReference(PackageIdentity, Framework);

        [Theory]
        [MemberData(nameof(IPackageReferenceContextInfos))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(IPackageReferenceContextInfo expectedResult)
        {
            IPackageReferenceContextInfo? actualResult = SerializeThenDeserialize(IPackageReferenceContextInfoFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.Identity, actualResult!.Identity);
            Assert.Equal(expectedResult.Framework, actualResult.Framework);
            Assert.Equal(expectedResult.AllowedVersions, actualResult.AllowedVersions);
            Assert.Equal(expectedResult.IsAutoReferenced, actualResult.IsAutoReferenced);
            Assert.Equal(expectedResult.IsUserInstalled, actualResult.IsUserInstalled);
            Assert.Equal(expectedResult.IsDevelopmentDependency, actualResult.IsDevelopmentDependency);
        }

        public static TheoryData IPackageReferenceContextInfos => new TheoryData<IPackageReferenceContextInfo>
            {
                { PackageReferenceContextInfo.Create(PackageIdentity, Framework) },
                { PackageReferenceContextInfo.Create(PackageIdentity, framework: null) },
                { PackageReferenceContextInfo.Create(PackageReference) }
            };
    }
}
