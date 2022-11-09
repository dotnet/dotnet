// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class LockFileEqualityTests
    {
        [Fact]
        public void Equals_WithProjectFileDependencyGroupOutOfOrder_ReturnsTrue()
        {
            var leftSide = new LockFile
            {
                ProjectFileDependencyGroups = new List<ProjectFileDependencyGroup>
                {
                    new ProjectFileDependencyGroup("net462", new string[] { "a", "b", "c" }),
                    new ProjectFileDependencyGroup("net472", new string[] { "a", "c", "b" }),
                }
            };
            var rightSide = new LockFile
            {
                ProjectFileDependencyGroups = new List<ProjectFileDependencyGroup>
                {
                    new ProjectFileDependencyGroup("net472", new string[] { "a", "c", "b" }),
                    new ProjectFileDependencyGroup("net462", new string[] { "a", "b", "c" }),
                }
            };

            leftSide.Should().Be(rightSide);
        }

        [Fact]
        public void Equals_WithDifferentProjectFileDependencyGroup_ReturnsFalse()
        {
            var leftSide = new LockFile
            {
                ProjectFileDependencyGroups = new List<ProjectFileDependencyGroup>
                {
                    new ProjectFileDependencyGroup("net461", new string[] { "a", "b", "c" }),
                    new ProjectFileDependencyGroup("net462", new string[] { "a", "b", "c" }),
                    new ProjectFileDependencyGroup("net463", new string[] { "a", "b", "c" }),
                }
            };
            var rightSide = new LockFile
            {
                ProjectFileDependencyGroups = new List<ProjectFileDependencyGroup>
                {
                    new ProjectFileDependencyGroup("net461", new string[] { "a", "b", "c" }),
                    new ProjectFileDependencyGroup("net462", new string[] { "a", "b", "c" }),
                }
            };

            leftSide.Should().NotBe(rightSide);
        }

        [Fact]
        public void Equals_WithLockFileLibraryOutOfOrder_ReturnsTrue()
        {
            var leftSide = new LockFile
            {
                Libraries = new List<LockFileLibrary>
                {
                    new LockFileLibrary()
                    {
                        Name = "first"
                    },
                    new LockFileLibrary()
                    {
                        Name = "second"
                    },
                }
            };
            var rightSide = new LockFile
            {
                Libraries = new List<LockFileLibrary>
                {
                    new LockFileLibrary()
                    {
                        Name = "second"
                    },
                    new LockFileLibrary()
                    {
                        Name = "first"
                    },
                }
            };

            leftSide.Should().Be(rightSide);
        }

        [Fact]
        public void Equals_WithDifferentLockFileLibrary_ReturnsFalse()
        {
            var leftSide = new LockFile
            {
                Libraries = new List<LockFileLibrary>
                {
                    new LockFileLibrary()
                    {
                        Name = "first"
                    },
                    new LockFileLibrary()
                    {
                        Name = "second"
                    },
                    new LockFileLibrary()
                    {
                        Name = "extra"
                    },
                }
            };
            var rightSide = new LockFile
            {
                Libraries = new List<LockFileLibrary>
                {
                    new LockFileLibrary()
                    {
                        Name = "second"
                    },
                    new LockFileLibrary()
                    {
                        Name = "first"
                    },
                }
            };

            leftSide.Should().NotBe(rightSide);
        }

        [Fact]
        public void Equals_WithLockFilTargetOutOfOrder_ReturnsTrue()
        {
            var leftSide = new LockFile
            {
                Targets = new List<LockFileTarget>
                {
                    new LockFileTarget()
                    {
                        TargetFramework = NuGetFramework.Parse("net5.0"),
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new LockFileTargetLibrary()
                            {
                                Name = "first"
                            }
                        }
                    },
                    new LockFileTarget()
                    {
                        TargetFramework = NuGetFramework.Parse("net6.0"),
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new LockFileTargetLibrary()
                            {
                                Name = "second"
                            }
                        }
                    }
                }
            };
            var rightSide = new LockFile
            {
                Targets = new List<LockFileTarget>
                {
                    new LockFileTarget()
                    {
                        TargetFramework = NuGetFramework.Parse("net6.0"),
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new LockFileTargetLibrary()
                            {
                                Name = "second"
                            }
                        }
                    },
                    new LockFileTarget()
                    {
                        TargetFramework = NuGetFramework.Parse("net5.0"),
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new LockFileTargetLibrary()
                            {
                                Name = "first"
                            }
                        }
                    }
                }
            };

            leftSide.Should().Be(rightSide);
        }

        [Fact]
        public void Equals_WithDifferentLockFileTarget_ReturnsFalse()
        {
            var leftSide = new LockFile
            {
                Targets = new List<LockFileTarget>
                {
                    new LockFileTarget()
                    {
                        TargetFramework = NuGetFramework.Parse("net5.0"),
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new LockFileTargetLibrary()
                            {
                                Name = "first"
                            }
                        }
                    },
                    new LockFileTarget()
                    {
                        TargetFramework = NuGetFramework.Parse("net6.0"),
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new LockFileTargetLibrary()
                            {
                                Name = "second"
                            }
                        }
                    }
                }
            };
            var rightSide = new LockFile
            {
                Targets = new List<LockFileTarget>
                {
                    new LockFileTarget()
                    {
                        TargetFramework = NuGetFramework.Parse("net472")
                    },
                    new LockFileTarget()
                    {
                        TargetFramework = NuGetFramework.Parse("net6.0"),
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new LockFileTargetLibrary()
                            {
                                Name = "second"
                            }
                        }
                    },
                    new LockFileTarget()
                    {
                        TargetFramework = NuGetFramework.Parse("net5.0"),
                        Libraries = new List<LockFileTargetLibrary>
                        {
                            new LockFileTargetLibrary()
                            {
                                Name = "first"
                            }
                        }
                    }
                }
            };

            leftSide.Should().NotBe(rightSide);
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("A;b", "a;B", true)]
        [InlineData("a;b;c", "c;a;B", false)]
        [InlineData("a;b;c;d", "c;a;B", false)]
        public void Equals_WithPackageFolders(string left, string right, bool expected)
        {
            var leftSide = new LockFile
            {
                PackageFolders = left.Split(';').Select(e => new LockFileItem(e)).ToList()
            };
            var rightSide = new LockFile
            {
                PackageFolders = right.Split(';').Select(e => new LockFileItem(e)).ToList()
            };

            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("a", "a", true)]
        [InlineData("b;a", "a;b", true)]
        [InlineData("A;b", "a;B", false)]
        [InlineData("a;b;c", "c;a;B", false)]
        [InlineData("a;b;c;d", "c;a;b", false)]
        public void Equals_WithLogMessages(string left, string right, bool expected)
        {
            var leftSide = new LockFile
            {
                LogMessages = left.Split(';').Select(e => new AssetsLogMessage(LogLevel.Debug, NuGetLogCode.NU1004, e)).ToList<IAssetsLogMessage>()
            };
            var rightSide = new LockFile
            {
                LogMessages = right.Split(';').Select(e => new AssetsLogMessage(LogLevel.Debug, NuGetLogCode.NU1004, e)).ToList<IAssetsLogMessage>()
            };

            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Fact]
        public void Equals_WithEquivalentPackageSpec_ReturnsTrue()
        {
            var leftSide = new LockFile
            {
                PackageSpec = new PackageSpec()
                {
                    Title = "a"
                }
            };
            var rightSide = new LockFile
            {
                PackageSpec = new PackageSpec()
                {
                    Title = "a"
                }
            };

            leftSide.Should().Be(rightSide);
        }

        [Fact]
        public void Equals_WithDifferentPackageSpec_ReturnsFalse()
        {
            var leftSide = new LockFile
            {
                PackageSpec = new PackageSpec()
                {
                    Title = "a"
                }
            };
            var rightSide = new LockFile
            {
                PackageSpec = new PackageSpec()
                {
                    Title = "b"
                }
            };

            leftSide.Should().NotBe(rightSide);
        }

        [Fact]
        public void Equals_WithDifferentOrderCentralTransitiveDependencyGroup_ReturnsTrue()
        {
            var leftSide = new LockFile
            {
                CentralTransitiveDependencyGroups = new List<CentralTransitiveDependencyGroup>()
                {
                    new CentralTransitiveDependencyGroup(
                        NuGetFramework.Parse("net462"),
                        new LibraryDependency[]
                        {
                            new LibraryDependency()
                            {
                                LibraryRange = new LibraryRange()
                                {
                                    Name = "first"
                                }
                            }
                        }),
                    new CentralTransitiveDependencyGroup(
                        NuGetFramework.Parse("net461"),
                        new LibraryDependency[]
                        {
                            new LibraryDependency()
                            {
                                LibraryRange = new LibraryRange()
                                {
                                    Name = "second"
                                }
                            }
                        })
                }
            };
            var rightSide = new LockFile
            {
                CentralTransitiveDependencyGroups = new List<CentralTransitiveDependencyGroup>()
                 {
                    new CentralTransitiveDependencyGroup(
                        NuGetFramework.Parse("net461"),
                        new LibraryDependency[]
                        {
                            new LibraryDependency()
                            {
                                LibraryRange = new LibraryRange()
                                {
                                    Name = "second"
                                }
                            }
                        }),
                    new CentralTransitiveDependencyGroup(
                        NuGetFramework.Parse("net462"),
                        new LibraryDependency[]
                        {
                            new LibraryDependency()
                            {
                                LibraryRange = new LibraryRange()
                                {
                                    Name = "first"
                                }
                            }
                        })
                }
            };

            leftSide.Should().Be(rightSide);
        }

        [Fact]
        public void Equals_WithDifferentCentralTransitiveDependencyGroup_ReturnsFalse()
        {
            var leftSide = new LockFile
            {
                CentralTransitiveDependencyGroups = new List<CentralTransitiveDependencyGroup>()
                {
                    new CentralTransitiveDependencyGroup(
                        NuGetFramework.Parse("net462"),
                        new LibraryDependency[]
                        {
                            new LibraryDependency()
                            {
                                LibraryRange = new LibraryRange()
                                {
                                    Name = "first"
                                }
                            },
                             new LibraryDependency()
                            {
                                LibraryRange = new LibraryRange()
                                {
                                    Name = "second"
                                }
                            }
                        }),
                    new CentralTransitiveDependencyGroup(
                        NuGetFramework.Parse("net461"),
                        new LibraryDependency[]
                        {
                            new LibraryDependency()
                            {
                                LibraryRange = new LibraryRange()
                                {
                                    Name = "second"
                                }
                            }
                        })
                }
            };
            var rightSide = new LockFile
            {
                CentralTransitiveDependencyGroups = new List<CentralTransitiveDependencyGroup>()
                 {
                    new CentralTransitiveDependencyGroup(
                        NuGetFramework.Parse("net461"),
                        new LibraryDependency[]
                        {
                            new LibraryDependency()
                            {
                                LibraryRange = new LibraryRange()
                                {
                                    Name = "second"
                                }
                            }
                        }),
                    new CentralTransitiveDependencyGroup(
                        NuGetFramework.Parse("net462"),
                        new LibraryDependency[]
                        {
                            new LibraryDependency()
                            {
                                LibraryRange = new LibraryRange()
                                {
                                    Name = "first"
                                }
                            }
                        })
                }
            };

            leftSide.Should().NotBe(rightSide);
        }
    }
}
