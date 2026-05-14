// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class AssetTargetFallbackTests
    {
        [Fact]
        public void GivenAssetTargetFallbackTrueVerifyValuePersisted()
        {
            var spec = PackageSpecTestUtility.GetSpec(assetTargetFallback: true, NuGetFramework.Parse("netcoreapp2.0"));

            var outSpec = spec.RoundTrip();
            outSpec.TargetFrameworks[0].AssetTargetFallback.Should().BeTrue();
        }

        [Fact]
        public void GivenAssetTargetFallbackFalseVerifyValuePersisted()
        {
            var spec = PackageSpecTestUtility.GetSpec(assetTargetFallback: false, NuGetFramework.Parse("netcoreapp2.0"));

            var outSpec = spec.RoundTrip();
            outSpec.TargetFrameworks[0].AssetTargetFallback.Should().BeFalse();
        }

        [Fact]
        public void GivenImportsVerifyValuePersisted()
        {
            var net461 = NuGetFramework.Parse("net461");
            var spec = PackageSpecTestUtility.GetSpec(assetTargetFallback: true, NuGetFramework.Parse("netcoreapp2.0"));
            spec.TargetFrameworks[0] = new TargetFrameworkInformation(spec.TargetFrameworks[0]) { Imports = [net461] };

            var outSpec = spec.RoundTrip();
            outSpec.TargetFrameworks[0].Imports.Should().BeEquivalentTo(new[] { net461 });
        }

        [Fact]
        public void GivenAssetTargetFallbackVerifyFrameworkIsAssetTargetFallbackFramework()
        {
            var net461 = NuGetFramework.Parse("net461");
            var projectFramework = NuGetFramework.Parse("netcoreapp2.0");
            var spec = PackageSpecTestUtility.GetSpec(assetTargetFallback: true, projectFramework);
            spec.TargetFrameworks[0] = new TargetFrameworkInformation(spec.TargetFrameworks[0]) { Imports = [net461] };

            var outSpec = spec.RoundTrip();

            var outFramework = spec.TargetFrameworks[0].FrameworkName;

            outFramework.Should().Be(new AssetTargetFallbackFramework(projectFramework, new[] { net461 }));
        }

        [Fact]
        public void GivenAssetTargetFallbackDiffersVerifyEquality()
        {
            var spec1 = PackageSpecTestUtility.GetSpec(assetTargetFallback: true, "netcoreapp2.0");
            var spec2 = PackageSpecTestUtility.GetSpec(assetTargetFallback: false, "netcoreapp2.0");

            spec1.Should().NotBe(spec2);
            spec1.GetHashCode().Should().NotBe(spec2.GetHashCode());
        }

        [Fact]
        public void GivenAssetTargetFallbackEqualityIsTransitive()
        {
            var A1 = new AssetTargetFallbackFramework(NuGetFramework.Parse("netcoreapp2.0"), new[] { NuGetFramework.Parse("net461") });
            var A2 = new AssetTargetFallbackFramework(NuGetFramework.Parse("netcoreapp2.0"), new[] { NuGetFramework.Parse("net461") });
            var B1 = new AssetTargetFallbackFramework(NuGetFramework.Parse("netcoreapp2.0"), new[] { NuGetFramework.Parse("net462") });
            var B2 = new AssetTargetFallbackFramework(NuGetFramework.Parse("netcoreapp2.0"), new[] { NuGetFramework.Parse("net462") });
            var C1 = new AssetTargetFallbackFramework(NuGetFramework.Parse("netcoreapp1.0"), new[] { NuGetFramework.Parse("net462") });
            var C2 = new AssetTargetFallbackFramework(NuGetFramework.Parse("netcoreapp1.0"), new[] { NuGetFramework.Parse("net462") });

            // Check equivalents
            A1.Should().Be(A2);
            B1.Should().Be(B2);
            C1.Should().Be(C2);

            // Rest of the A1 -> B1,B2,C1,C2
            A1.Should().NotBe(B1);
            A1.Should().NotBe(B2);
            A1.Should().NotBe(C1);
            A1.Should().NotBe(C2);

            // Rest of the A2 -> B1,B2,C1,C2
            A2.Should().NotBe(B1);
            A2.Should().NotBe(B2);
            A2.Should().NotBe(C1);
            A2.Should().NotBe(C2);

            // B1 -> C1, C2
            B1.Should().NotBe(C1);
            B1.Should().NotBe(C2);

            // B2 -> C1, C2
            B2.Should().NotBe(C1);
            B2.Should().NotBe(C2);


        }
    }
}
