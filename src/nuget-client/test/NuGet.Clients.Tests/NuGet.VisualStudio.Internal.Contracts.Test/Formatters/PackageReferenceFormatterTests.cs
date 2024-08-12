// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class PackageReferenceFormatterTests : FormatterTests
    {
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(id: "a", NuGetVersion.Parse("1.0.0"));
        private static readonly NuGetFramework Framework = NuGetFramework.Parse("net50");
        private static readonly VersionRange VersionRange = new VersionRange(NuGetVersion.Parse("2.0.0"));

        [Theory]
        [MemberData(nameof(PackageReferences))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(PackageReference expectedResult)
        {
            PackageReference? actualResult = SerializeThenDeserialize(PackageReferenceFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.AllowedVersions, actualResult!.AllowedVersions);
            Assert.Equal(expectedResult.HasAllowedVersions, actualResult.HasAllowedVersions);
            Assert.Equal(expectedResult.IsDevelopmentDependency, actualResult.IsDevelopmentDependency);
            Assert.Equal(expectedResult.IsUserInstalled, actualResult.IsUserInstalled);
            Assert.Equal(expectedResult.PackageIdentity, actualResult.PackageIdentity);
            Assert.Equal(expectedResult.RequireReinstallation, actualResult.RequireReinstallation);
            Assert.Equal(expectedResult.TargetFramework, actualResult.TargetFramework);
        }

        public static TheoryData<PackageReference> PackageReferences => new()
            {
                { new PackageReference(PackageIdentity, Framework) },
                { new PackageReference(PackageIdentity, Framework, userInstalled: true) },
                { new PackageReference(PackageIdentity, Framework, userInstalled: false, developmentDependency: true, requireReinstallation: false) },
                { new PackageReference(PackageIdentity, Framework, userInstalled: true, developmentDependency: false, requireReinstallation: true, VersionRange) }
             };
    }
}
