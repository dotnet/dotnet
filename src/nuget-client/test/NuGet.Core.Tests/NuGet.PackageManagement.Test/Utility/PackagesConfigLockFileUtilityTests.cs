using System.Collections.Generic;
using System.IO;
using System.Threading;
using Moq;
using NuGet.Frameworks;
using NuGet.PackageManagement.Utility;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.Test.Utility
{
    public class PackagesConfigLockFileUtilityTests
    {
        [Fact]
        public void GetPackagesLockFilePath_MsbuildProperty()
        {
            // Arrage
            var projectName = "testproj";
            var logger = new TestLogger();
            var expected = "somewhere\\my.lock.json";

            using (var rootFolder = TestDirectory.Create())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(rootFolder, projectName));
                var targetFramework = NuGetFramework.Parse("net46");

                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(targetFramework, new TestNuGetProjectContext(), projectFolder.FullName);
                var project = new TestMSBuildNuGetProject(msBuildNuGetProjectSystem, rootFolder, projectFolder.FullName);
                msBuildNuGetProjectSystem.SetPropertyValue("NuGetLockFilePath", expected);

                // Act
                var lockFile = PackagesConfigLockFileUtility.GetPackagesLockFilePath(project);

                // Assert
                Assert.Equal(Path.Combine(projectFolder.FullName, expected), lockFile);
            }
        }

        [Fact]
        public void GetPackagesLockFilePath_PackagesProjectLockJson()
        {
            // Arrange
            var projectName = "testproj";
            var logger = new TestLogger();
            var expected = $"packages.{projectName}.lock.json";

            using (var rootFolder = TestDirectory.Create())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(rootFolder, projectName));
                var targetFramework = NuGetFramework.Parse("net46");
                projectFolder.Create();
                File.WriteAllText(Path.Combine(projectFolder.FullName, expected), string.Empty);

                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(targetFramework, new TestNuGetProjectContext(), projectFolder.FullName, projectName: projectName);
                var project = new TestMSBuildNuGetProject(msBuildNuGetProjectSystem, rootFolder, projectFolder.FullName);

                // Act
                var lockFile = PackagesConfigLockFileUtility.GetPackagesLockFilePath(project);

                //Assert
                Assert.Equal(Path.Combine(projectFolder.FullName, expected), lockFile);
            }
        }

        [Fact]
        public void GetPackagesLockFilePath_PackagesLockJson()
        {
            // Arrange
            var projectName = "testproj";
            var logger = new TestLogger();

            using (var rootFolder = TestDirectory.Create())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(rootFolder, projectName));
                var targetFramework = NuGetFramework.Parse("net46");

                var msBuildNuGetProjectSystem = new TestMSBuildNuGetProjectSystem(targetFramework, new TestNuGetProjectContext(), projectFolder.FullName);
                var project = new TestMSBuildNuGetProject(msBuildNuGetProjectSystem, rootFolder, projectFolder.FullName);

                // Act
                var lockFile = PackagesConfigLockFileUtility.GetPackagesLockFilePath(project);

                // Assert
                Assert.Equal(Path.Combine(projectFolder.FullName, "packages.lock.json"), lockFile);
            }
        }

        [Fact]
        public void ApplyChanges_AddInstalledPackage()
        {
            // Arrange
            var lockFile = new PackagesLockFile
            {
                Targets = new List<PackagesLockFileTarget>
                {
                    new PackagesLockFileTarget()
                }
            };

            var actionList = new List<NuGetProjectAction>
            {
                NuGetProjectAction.CreateInstallProjectAction(new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0")), null, null)
            };

            var contentHashUtility = new Mock<IPackagesConfigContentHashProvider>();

            // Act
            PackagesConfigLockFileUtility.ApplyChanges(lockFile, actionList, contentHashUtility.Object, CancellationToken.None);

            // Assert
            Assert.Equal(1, lockFile.Targets[0].Dependencies.Count);
            Assert.Equal(actionList[0].PackageIdentity.Id, lockFile.Targets[0].Dependencies[0].Id);
            Assert.Equal(actionList[0].PackageIdentity.Version, lockFile.Targets[0].Dependencies[0].ResolvedVersion);
        }

        [Fact]
        public void ApplyChanges_RemoveUninstalledPackage()
        {
            // Arrange
            var lockFile = new PackagesLockFile
            {
                Targets = new List<PackagesLockFileTarget>
                {
                    new PackagesLockFileTarget
                    {
                        Dependencies = new List<LockFileDependency>
                        {
                            new LockFileDependency
                            {
                                Id = "packageA",
                                ResolvedVersion = NuGetVersion.Parse("1.0.0")
                            }
                        }
                    }
                }
            };

            var actionList = new List<NuGetProjectAction>
            {
                NuGetProjectAction.CreateUninstallProjectAction(new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0")), null)
            };

            var contentHashUtility = new Mock<IPackagesConfigContentHashProvider>();

            // Act
            PackagesConfigLockFileUtility.ApplyChanges(lockFile, actionList, contentHashUtility.Object, CancellationToken.None);

            // Assert
            Assert.Equal(0, lockFile.Targets[0].Dependencies.Count);
        }

        [Fact]
        public void ApplyChanges_UpgradeInstalledPackage()
        {
            // Arrange
            var lockFile = new PackagesLockFile
            {
                Targets = new List<PackagesLockFileTarget>
                {
                    new PackagesLockFileTarget
                    {
                        Dependencies = new List<LockFileDependency>
                        {
                            new LockFileDependency
                            {
                                Id = "packageA",
                                ResolvedVersion = NuGetVersion.Parse("1.0.0")
                            }
                        }
                    }
                }
            };

            var actionList = new List<NuGetProjectAction>
            {
                NuGetProjectAction.CreateUninstallProjectAction(new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0")), null),
                NuGetProjectAction.CreateInstallProjectAction(new PackageIdentity("packageA", NuGetVersion.Parse("2.0.0")), null, null)
            };

            var contentHashUtility = new Mock<IPackagesConfigContentHashProvider>();

            // Act
            PackagesConfigLockFileUtility.ApplyChanges(lockFile, actionList, contentHashUtility.Object, CancellationToken.None);

            // Assert
            Assert.Equal(1, lockFile.Targets[0].Dependencies.Count);
            Assert.Equal(actionList[0].PackageIdentity.Id, lockFile.Targets[0].Dependencies[0].Id);
            Assert.Equal(NuGetVersion.Parse("2.0.0"), lockFile.Targets[0].Dependencies[0].ResolvedVersion);
        }

        [Fact]
        public void ApplyChanges_SortPackages_FirstPackage()
        {
            // Arrange
            var lockFile = new PackagesLockFile
            {
                Targets = new List<PackagesLockFileTarget>
                {
                    new PackagesLockFileTarget
                    {
                        Dependencies = new List<LockFileDependency>
                        {
                            new LockFileDependency
                            {
                                Id = "packageB",
                                ResolvedVersion = NuGetVersion.Parse("1.0.0")
                            },
                            new LockFileDependency
                            {
                                Id = "packageD",
                                ResolvedVersion = NuGetVersion.Parse("1.0.0")
                            }
                        }
                    }
                }
            };

            var actionList = new List<NuGetProjectAction>
            {
                NuGetProjectAction.CreateInstallProjectAction(new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0")), null, null)
            };

            var contentHashUtility = new Mock<IPackagesConfigContentHashProvider>();

            // Act
            PackagesConfigLockFileUtility.ApplyChanges(lockFile, actionList, contentHashUtility.Object, CancellationToken.None);

            // Assert
            Assert.Equal(3, lockFile.Targets[0].Dependencies.Count);
            Assert.Equal("packageA", lockFile.Targets[0].Dependencies[0].Id);
            Assert.Equal("packageB", lockFile.Targets[0].Dependencies[1].Id);
            Assert.Equal("packageD", lockFile.Targets[0].Dependencies[2].Id);
        }

        [Fact]
        public void ApplyChanges_SortPackages_MiddleOfList()
        {
            // Arrange
            var lockFile = new PackagesLockFile
            {
                Targets = new List<PackagesLockFileTarget>
                {
                    new PackagesLockFileTarget
                    {
                        Dependencies = new List<LockFileDependency>
                        {
                            new LockFileDependency
                            {
                                Id = "packageB",
                                ResolvedVersion = NuGetVersion.Parse("1.0.0")
                            },
                            new LockFileDependency
                            {
                                Id = "packageD",
                                ResolvedVersion = NuGetVersion.Parse("1.0.0")
                            }
                        }
                    }
                }
            };

            var actionList = new List<NuGetProjectAction>
            {
                NuGetProjectAction.CreateInstallProjectAction(new PackageIdentity("packageC", NuGetVersion.Parse("1.0.0")), null, null)
            };

            var contentHashUtility = new Mock<IPackagesConfigContentHashProvider>();

            // Act
            PackagesConfigLockFileUtility.ApplyChanges(lockFile, actionList, contentHashUtility.Object, CancellationToken.None);

            // Assert
            Assert.Equal(3, lockFile.Targets[0].Dependencies.Count);
            Assert.Equal("packageB", lockFile.Targets[0].Dependencies[0].Id);
            Assert.Equal("packageC", lockFile.Targets[0].Dependencies[1].Id);
            Assert.Equal("packageD", lockFile.Targets[0].Dependencies[2].Id);
        }

        [Fact]
        public void ApplyChanges_SortPackages_LastPackage()
        {
            // Arrange
            var lockFile = new PackagesLockFile
            {
                Targets = new List<PackagesLockFileTarget>
                {
                    new PackagesLockFileTarget
                    {
                        Dependencies = new List<LockFileDependency>
                        {
                            new LockFileDependency
                            {
                                Id = "packageB",
                                ResolvedVersion = NuGetVersion.Parse("1.0.0")
                            },
                            new LockFileDependency
                            {
                                Id = "packageD",
                                ResolvedVersion = NuGetVersion.Parse("1.0.0")
                            }
                        }
                    }
                }
            };

            var actionList = new List<NuGetProjectAction>
            {
                NuGetProjectAction.CreateInstallProjectAction(new PackageIdentity("packageE", NuGetVersion.Parse("1.0.0")), null, null)
            };

            var contentHashUtility = new Mock<IPackagesConfigContentHashProvider>();

            // Act
            PackagesConfigLockFileUtility.ApplyChanges(lockFile, actionList, contentHashUtility.Object, CancellationToken.None);

            // Assert
            Assert.Equal(3, lockFile.Targets[0].Dependencies.Count);
            Assert.Equal("packageB", lockFile.Targets[0].Dependencies[0].Id);
            Assert.Equal("packageD", lockFile.Targets[0].Dependencies[1].Id);
            Assert.Equal("packageE", lockFile.Targets[0].Dependencies[2].Id);
        }
    }
}
