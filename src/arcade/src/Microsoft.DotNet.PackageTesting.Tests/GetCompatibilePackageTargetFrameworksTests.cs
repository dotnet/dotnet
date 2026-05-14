// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using Xunit;

namespace Microsoft.DotNet.PackageTesting.Tests
{
    public class GetCompatibilePackageTargetFrameworksTests
    {
        public GetCompatibilePackageTargetFrameworksTests()
        {
            GetCompatiblePackageTargetFrameworks.Initialize("netcoreapp3.1;net5.0;net6.0;net461;net462;net471;net472;netstandard2.0;netstandard2.1");
        }

        public static IEnumerable<object[]> PackageTfmData => new List<object[]>
        {
            // single target framework in package
            new object[]
            {  
                new List<string> 
                { 
                    @"lib/netstandard2.0/TestPackage.dll"
                },
                new List<NuGetFramework> 
                { 
                    FrameworkConstants.CommonFrameworks.NetStandard20, 
                    FrameworkConstants.CommonFrameworks.Net461, 
                    FrameworkConstants.CommonFrameworks.Net462,
                    FrameworkConstants.CommonFrameworks.NetCoreApp31
                } 
            },
            new object[]
            {
                new List<string>
                {
                    @"runtimes/win/lib/netstandard2.0/TestPackage.dll",
                },
                new List<NuGetFramework>
                {
                    FrameworkConstants.CommonFrameworks.NetStandard20,
                    FrameworkConstants.CommonFrameworks.Net461,
                    FrameworkConstants.CommonFrameworks.Net462,
                    FrameworkConstants.CommonFrameworks.NetCoreApp31
                }
            },
            new object[]
            {
                new List<string>
                {
                    @"lib/net5.0/TestPackage.dll",
                    @"runtimes/win/lib/netstandard2.0/TestPackage.dll"
                },
                new List<NuGetFramework>
                {
                    FrameworkConstants.CommonFrameworks.NetStandard20,
                    FrameworkConstants.CommonFrameworks.Net461,
                    FrameworkConstants.CommonFrameworks.Net462,
                    FrameworkConstants.CommonFrameworks.NetCoreApp31,
                    FrameworkConstants.CommonFrameworks.Net50
                }
            },
            new object[]
            {
                new List<string>
                {
                    @"lib/netcoreapp3.1/TestPackage.dll"
                },
                new List<NuGetFramework>
                {
                    FrameworkConstants.CommonFrameworks.NetCoreApp31
                }
            },
            new object[]
            {
                new List<string>
                {
                    @"lib/netcoreapp3.1/TestPackage.dll",
                    @"lib/net461/TestPackage.dll"
                },
                new List<NuGetFramework>
                {
                    FrameworkConstants.CommonFrameworks.NetCoreApp31,
                    FrameworkConstants.CommonFrameworks.Net461
                }
            },
            new object[]
            {
                new List<string>
                {
                    @"runtimes/unix/lib/netcoreapp3.1/TestPackage.dll",
                    @"runtimes/win/lib/netstandard2.0/TestPackage.dll"
                },
                new List<NuGetFramework>
                {
                    FrameworkConstants.CommonFrameworks.NetCoreApp31,
                    FrameworkConstants.CommonFrameworks.NetStandard20,
                    FrameworkConstants.CommonFrameworks.Net461,
                    FrameworkConstants.CommonFrameworks.Net462
                }
            },
            new object[]
            {
                new List<string>
                {
                    @"lib/net5.0/TestPackage.dll",
                    @"lib/net472/TestPackage.dll",
                    @"runtimes/win/lib/netstandard2.0/TestPackage.dll"
                },
                new List<NuGetFramework>
                {
                    FrameworkConstants.CommonFrameworks.NetStandard20,
                    FrameworkConstants.CommonFrameworks.Net461,
                    FrameworkConstants.CommonFrameworks.Net462,
                    NuGetFramework.Parse("net472"),
                    FrameworkConstants.CommonFrameworks.NetCoreApp31,
                    FrameworkConstants.CommonFrameworks.Net50
                }
            },
            new object[]
            {
                new List<string>
                {
                    @"lib/net461/TestPackage.dll"
                },
                new List<NuGetFramework>
                {
                    FrameworkConstants.CommonFrameworks.Net461,
                }
            },
        };

        [Theory]
        [MemberData(nameof(PackageTfmData))]
        public void GetCompatibleFrameworks(List<string> filePaths, List<NuGetFramework> expectedTestFrameworks)
        {
            Package package = new("TestPackage", "1.0.0", filePaths, Enumerable.Empty<NuGetFramework>());
            IEnumerable<NuGetFramework> actualTestFrameworks = GetCompatiblePackageTargetFrameworks.GetTestFrameworks(package, "netcoreapp3.1");
            CollectionsEqual(expectedTestFrameworks, actualTestFrameworks);
        }

        [Fact]
        public void GetCompatibleFrameworksFromDependencies()
        {
            var dependencyFrameworks = new[]
            {
                FrameworkConstants.CommonFrameworks.NetCoreApp21,
                FrameworkConstants.CommonFrameworks.NetCoreApp31,
                FrameworkConstants.CommonFrameworks.NetStandard20,
                FrameworkConstants.CommonFrameworks.NetStandard21,
                NuGetFramework.Parse("net6.0"),
            };
            Package package = new("TestPackage", "1.0.0", Enumerable.Empty<string>(), dependencyFrameworks);
            IEnumerable<NuGetFramework> actualTestFrameworks = GetCompatiblePackageTargetFrameworks.GetTestFrameworks(package, "netcoreapp3.1");

            var expectedTestFrameworks = new[]
            {
                NuGetFramework.Parse("net6.0"),
                FrameworkConstants.CommonFrameworks.NetCoreApp31,
                FrameworkConstants.CommonFrameworks.Net461,
                FrameworkConstants.CommonFrameworks.Net462,
                FrameworkConstants.CommonFrameworks.NetStandard20,
                FrameworkConstants.CommonFrameworks.NetStandard21
            };
            CollectionsEqual(expectedTestFrameworks, actualTestFrameworks);
        }

        private static void CollectionsEqual<T>(IEnumerable<T> T1, IEnumerable<T> T2)
        {
            foreach (var item in T1)
            {
                Assert.Contains(item, T2);
            }
            foreach (var item in T2)
            {
                Assert.Contains(item, T1);
            }
        }
    }
}
