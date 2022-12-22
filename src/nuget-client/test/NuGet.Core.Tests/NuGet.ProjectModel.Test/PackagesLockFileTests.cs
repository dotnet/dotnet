// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class PackagesLockFileTests
    {
        [Fact]
        public void PackagesLockFile_Equals()
        {
            Func<PackagesLockFile> getLockFile = () =>
            {
                var lockFile = new PackagesLockFile()
                {
                    Version = 1,

                    Targets = new List<PackagesLockFileTarget>()
                    {
                        new PackagesLockFileTarget()
                        {
                            TargetFramework = FrameworkConstants.CommonFrameworks.Net45,

                            Dependencies = new List<LockFileDependency>()
                            {
                                new LockFileDependency()
                                {
                                    Id = "PackageA",
                                    Type = PackageDependencyType.Direct,
                                    RequestedVersion = VersionRange.Parse("1.0.0"),
                                    ResolvedVersion = NuGetVersion.Parse("1.0.0"),
                                    ContentHash = "sha1",
                                    Dependencies = new List<PackageDependency>()
                                    {
                                        new PackageDependency("PackageB", VersionRange.Parse("1.0.0"))
                                    }
                                },
                                new LockFileDependency()
                                {
                                    Id = "PackageB",
                                    Type = PackageDependencyType.Transitive,
                                    ResolvedVersion = NuGetVersion.Parse("1.0.0"),
                                    ContentHash = "sha2"
                                }
                            }
                        },
                        new PackagesLockFileTarget()
                        {
                            TargetFramework = FrameworkConstants.CommonFrameworks.Net45,

                            RuntimeIdentifier = "win10-arm",

                            Dependencies = new List<LockFileDependency>()
                            {
                                new LockFileDependency()
                                {
                                    Id = "PackageA",
                                    Type = PackageDependencyType.Direct,
                                    RequestedVersion = VersionRange.Parse("1.0.0"),
                                    ResolvedVersion = NuGetVersion.Parse("1.0.0"),
                                    ContentHash = "sha3",
                                    Dependencies = new List<PackageDependency>()
                                    {
                                        new PackageDependency("runtime.win10-arm.PackageA", VersionRange.Parse("1.0.0"))
                                    }
                                },
                                new LockFileDependency()
                                {
                                    Id = "runtime.win10-arm.PackageA",
                                    Type = PackageDependencyType.Transitive,
                                    ResolvedVersion = NuGetVersion.Parse("1.0.0"),
                                    ContentHash = "sha4"
                                }
                            }
                        }
                    }
                };

                return lockFile;
            };

            var self = getLockFile();
            var other = getLockFile();

            Assert.NotSame(self, other);
            Assert.Equal(self, other);
        }
    }
}
