// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Commands;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    public class SolutionUpToDateCheckerTests
    {
        [Fact]
        public void GetOutputFilePaths_GetOutputFilePaths_AllIntermediateOutputsGoToTheOutputFolder()
        {
            var packageSpec = GetPackageSpec("A");
            packageSpec.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties(restorePackagesWithLockFile: "true", nuGetLockFilePath: null, restoreLockedMode: true);

            SolutionUpToDateChecker.GetOutputFilePaths(packageSpec, out string assetsFilePath, out string cacheFilePath, out string targetsFilePath, out string propsFilePath, out string lockFilePath);

            var expectedIntermediateFolder = packageSpec.RestoreMetadata.OutputPath;
            var expectedLockFileFolder = Path.GetDirectoryName(packageSpec.RestoreMetadata.ProjectPath);

            Path.GetDirectoryName(assetsFilePath).Should().Be(expectedIntermediateFolder);
            Path.GetDirectoryName(cacheFilePath).Should().Be(expectedIntermediateFolder);
            Path.GetDirectoryName(targetsFilePath).Should().Be(expectedIntermediateFolder);
            Path.GetDirectoryName(propsFilePath).Should().Be(expectedIntermediateFolder);
            Path.GetDirectoryName(lockFilePath).Should().Be(expectedLockFileFolder);
        }

        [Fact]
        public void GetOutputFilePaths_WorksForProjectJson()
        {
            var packageSpec = GetProjectJsonPackageSpec("A");
            SolutionUpToDateChecker.GetOutputFilePaths(packageSpec, out string assetsFilePath, out string cacheFilePath, out string targetsFilePath, out string propsFilePath, out string lockFilePath);

            var expectedIntermediateFolder = packageSpec.RestoreMetadata.OutputPath;
            var expectedAssetsFolder = Path.GetDirectoryName(packageSpec.FilePath);

            Path.GetDirectoryName(assetsFilePath).Should().Be(expectedAssetsFolder);
            Path.GetDirectoryName(cacheFilePath).Should().Be(expectedIntermediateFolder);
            Path.GetDirectoryName(targetsFilePath).Should().Be(expectedIntermediateFolder);
            Path.GetDirectoryName(propsFilePath).Should().Be(expectedIntermediateFolder);
            lockFilePath.Should().BeNull();
        }

        // A => B => C
        [Fact]
        public void GetParents_WhenDirtySpecsListIsEmpty_ReturnsEmpty()
        {
            var projectA = GetPackageSpec("A");
            var projectB = GetPackageSpec("B");
            var projectC = GetPackageSpec("C");

            // A => B => C
            projectA = projectA.WithTestProjectReference(projectB);
            projectB = projectB.WithTestProjectReference(projectC);

            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddProject(projectA);
            dgSpec.AddRestore(projectA.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectB);
            dgSpec.AddRestore(projectB.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectC);
            dgSpec.AddRestore(projectC.RestoreMetadata.ProjectUniqueName);

            Assert.Empty(SolutionUpToDateChecker.GetParents(new List<string>(), dgSpec));
        }

        // A => B => D
        //   => C => E
        // D & E are dirty
        [Fact]
        public void GetParents_WhenEveryLeafNodeIsDirty_ReturnsAllProjectsInTheSolution()
        {
            var projectA = GetPackageSpec("A");
            var projectB = GetPackageSpec("B");
            var projectC = GetPackageSpec("C");
            var projectD = GetPackageSpec("D");
            var projectE = GetPackageSpec("E");

            // A => B & C
            projectA = projectA.WithTestProjectReference(projectB).WithTestProjectReference(projectC);
            // B => D
            projectB = projectB.WithTestProjectReference(projectD);
            // C => E
            projectC = projectC.WithTestProjectReference(projectE);

            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddProject(projectA);
            dgSpec.AddRestore(projectA.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectB);
            dgSpec.AddRestore(projectB.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectC);
            dgSpec.AddRestore(projectC.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectD);
            dgSpec.AddRestore(projectD.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectE);
            dgSpec.AddRestore(projectE.RestoreMetadata.ProjectUniqueName);

            var expected = GetUniqueNames(projectA, projectB, projectC, projectD, projectE);
            var actual = SolutionUpToDateChecker.GetParents(GetUniqueNames(projectD, projectE), dgSpec);

            actual.Should().BeEquivalentTo(expected);
        }

        // A => B => D
        //   => C => E
        // F => D
        [Fact]
        public void GetParents_WhenOnlyRootSpecsAreDirty_ReturnsOnlyTheSameDirtyProjects()
        {
            var projectA = GetPackageSpec("A");
            var projectB = GetPackageSpec("B");
            var projectC = GetPackageSpec("C");
            var projectD = GetPackageSpec("D");
            var projectE = GetPackageSpec("E");
            var projectF = GetPackageSpec("F");

            // A => B & C
            projectA = projectA.WithTestProjectReference(projectB).WithTestProjectReference(projectC);
            // B => D
            projectB = projectB.WithTestProjectReference(projectD);
            // C => E
            projectC = projectC.WithTestProjectReference(projectE);
            // F => D
            projectF = projectF.WithTestProjectReference(projectD);

            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddProject(projectA);
            dgSpec.AddRestore(projectA.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectB);
            dgSpec.AddRestore(projectB.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectC);
            dgSpec.AddRestore(projectC.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectD);
            dgSpec.AddRestore(projectD.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectE);
            dgSpec.AddRestore(projectE.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectF);
            dgSpec.AddRestore(projectF.RestoreMetadata.ProjectUniqueName);

            var expected = GetUniqueNames(projectA, projectF);
            var actual = SolutionUpToDateChecker.GetParents(GetUniqueNames(projectA, projectF), dgSpec);

            actual.Should().BeEquivalentTo(expected);
        }

        // A => B => D => F
        //   => C => E
        // G => D => F
        // H => C => E
        //   => I => F
        //   => J => K => L
        // M => L
        // E & L are dirty
        [Fact]
        public void GetParents_WithMultiLevelGraph_WhenALeafIsDirty_ReturnsProjectsFromEveryLevelAsDirty()
        {
            var projectA = GetPackageSpec("A");
            var projectB = GetPackageSpec("B");
            var projectC = GetPackageSpec("C");
            var projectD = GetPackageSpec("D");
            var projectE = GetPackageSpec("E");
            var projectF = GetPackageSpec("F");
            var projectG = GetPackageSpec("G");
            var projectH = GetPackageSpec("H");
            var projectI = GetPackageSpec("I");
            var projectJ = GetPackageSpec("J");
            var projectK = GetPackageSpec("K");
            var projectL = GetPackageSpec("L");
            var projectM = GetPackageSpec("M");

            // A => B & C
            projectA = projectA.WithTestProjectReference(projectB).WithTestProjectReference(projectC);
            // B => D
            projectB = projectB.WithTestProjectReference(projectD);
            // D => F
            projectD = projectD.WithTestProjectReference(projectF);
            // C => E
            projectC = projectC.WithTestProjectReference(projectE);
            // G => D
            projectG = projectG.WithTestProjectReference(projectD);
            // H => C
            projectH = projectH.WithTestProjectReference(projectC);
            // I => F
            projectI = projectI.WithTestProjectReference(projectF);
            // H => I
            projectH = projectH.WithTestProjectReference(projectI);
            // K => L
            projectK = projectK.WithTestProjectReference(projectL);
            // J => K
            projectJ = projectJ.WithTestProjectReference(projectK);
            // H => J
            projectH = projectH.WithTestProjectReference(projectJ);
            // M => L
            projectM = projectM.WithTestProjectReference(projectL);


            var dgSpec = new DependencyGraphSpec();
            dgSpec.AddProject(projectA);
            dgSpec.AddRestore(projectA.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectB);
            dgSpec.AddRestore(projectB.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectC);
            dgSpec.AddRestore(projectC.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectD);
            dgSpec.AddRestore(projectD.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectE);
            dgSpec.AddRestore(projectE.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectF);
            dgSpec.AddRestore(projectF.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectG);
            dgSpec.AddRestore(projectG.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectH);
            dgSpec.AddRestore(projectH.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectI);
            dgSpec.AddRestore(projectI.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectJ);
            dgSpec.AddRestore(projectJ.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectK);
            dgSpec.AddRestore(projectK.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectL);
            dgSpec.AddRestore(projectL.RestoreMetadata.ProjectUniqueName);
            dgSpec.AddProject(projectM);
            dgSpec.AddRestore(projectM.RestoreMetadata.ProjectUniqueName);

            var expected = GetUniqueNames(projectA, projectC, projectE, projectH, projectJ, projectK, projectL, projectM);
            var actual = SolutionUpToDateChecker.GetParents(GetUniqueNames(projectE, projectL), dgSpec);

            actual.Should().BeEquivalentTo(expected);
        }

        // This behavior is off, but it is consistent with how no-op is handled right now in the RestoreCommand.
        // A change in *any* child affects the parents, even if the child is not PR.
        // A => B => C
        //   => D
        //   => E => F
        // F is not a standard project.
        [Fact]
        public void PerformUpToDateCheck_WhenNonBuildIntegratedProjectIsAParentOfADirtySpec_ReturnsAListWithoutNonBuildIntegratedProjects()
        {
            using (var testFolder = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testFolder);
                var projectB = GetPackageSpec("B", testFolder);
                var projectC = GetPackageSpec("C", testFolder);
                var projectD = GetPackageSpec("D", testFolder);
                var projectE = GetPackageSpec("E", testFolder);
                var projectF = GetUnknownPackageSpec("F", testFolder);

                // A => B & D & E
                projectA = projectA.WithTestProjectReference(projectB).WithTestProjectReference(projectD).WithTestProjectReference(projectE);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);
                // E => F
                projectE = projectE.WithTestProjectReference(projectF);

                DependencyGraphSpec dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC, projectD, projectE, projectF);

                var checker = new SolutionUpToDateChecker();

                var actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                var expected = GetUniqueNames(projectA, projectB, projectC, projectD, projectE);
                actual.Should().BeEquivalentTo(expected);

                // Now we run
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>(), projectA, projectB, projectC, projectD, projectE);
                checker.SaveRestoreStatus(results);

                // Prepare the new DG Spec:
                // Make projectE dirty by setting a random value that's usually not there :)
                projectF = projectF.Clone();
                projectF.RestoreMetadata.PackagesPath = testFolder;
                dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC, projectD, projectE, projectF);

                // Act & Assert.
                expected = GetUniqueNames(projectA, projectE);
                actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public void PerformUpToDateCheck_WithNoChanges_ReturnsEmpty()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetPackageSpec("C", testDirectory.Path);

                // A => B
                projectA = projectA.WithTestProjectReference(projectB);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);

                var dgSpec = new DependencyGraphSpec();
                dgSpec.AddProject(projectA);
                dgSpec.AddRestore(projectA.RestoreMetadata.ProjectUniqueName);
                dgSpec.AddProject(projectB);
                dgSpec.AddRestore(projectB.RestoreMetadata.ProjectUniqueName);
                dgSpec.AddProject(projectC);
                dgSpec.AddRestore(projectC.RestoreMetadata.ProjectUniqueName);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                var expected = GetUniqueNames(projectA, projectB, projectC);
                actual.Should().BeEquivalentTo(expected);
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>(), projectA, projectB, projectC);
                checker.SaveRestoreStatus(results);

                // Act & Asset.
                actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                actual.Should().BeEmpty();
            }
        }

        // A -> B -> C
        // D
        // B is dirty when a reference B -> D gets added, A & B are returned.
        [Fact]
        public void PerformUpToDateCheck_LeafProjectChanges_ReturnsAllItsParents()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetPackageSpec("C", testDirectory.Path);
                var projectD = GetPackageSpec("D", testDirectory.Path);

                // A => B
                projectA = projectA.WithTestProjectReference(projectB);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);
                DependencyGraphSpec dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC, projectD);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                var expected = GetUniqueNames(projectA, projectB, projectC, projectD);
                actual.Should().BeEquivalentTo(expected);
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>(), projectA, projectB, projectC, projectD);
                checker.SaveRestoreStatus(results);

                // Set-up, B => D
                projectB = projectB.WithTestProjectReference(projectD);
                dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC, projectD);

                // Act & Assert
                actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                expected = GetUniqueNames(projectA, projectB);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        // A => B => C
        // Delete the outputs of C. Forces only that project to restore.
        [Fact]
        public void PerformUpToDateCheck_WhenALeafProjectHasDirtyOutputs_ReturnsOnlyThatProject()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetPackageSpec("C", testDirectory.Path);

                // A => B
                projectA = projectA.WithTestProjectReference(projectB);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);
                DependencyGraphSpec dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                var expected = GetUniqueNames(projectA, projectB, projectC);
                actual.Should().BeEquivalentTo(expected);
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>(), projectA, projectB, projectC);
                checker.SaveRestoreStatus(results);

                // Set-up, delete C's outputs
                SolutionUpToDateChecker.GetOutputFilePaths(projectC, out string assetsFilePath, out string _, out string _, out string _, out string _);
                File.Delete(assetsFilePath);

                // Act & Assert
                actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                expected = GetUniqueNames(projectC);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        // A => B => C
        // Delete the outputs of C. Forces only that project to restore.
        [Fact]
        public void PerformUpToDateCheck_WhenALeafProjectHasNoCacheFile_ReturnsOnlyThatProject()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetPackageSpec("C", testDirectory.Path);

                // A => B
                projectA = projectA.WithTestProjectReference(projectB);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);
                DependencyGraphSpec dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                var expected = GetUniqueNames(projectA, projectB, projectC);
                actual.Should().BeEquivalentTo(expected);
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>(), projectA, projectB, projectC);
                checker.SaveRestoreStatus(results);

                // Set-up, delete C's outputs
                SolutionUpToDateChecker.GetOutputFilePaths(projectC, out string _, out string cacheFilePath, out string _, out string _, out string _);
                File.Delete(cacheFilePath);

                // Act & Assert
                actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                expected = GetUniqueNames(projectC);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        // A => B
        //   => C
        // D
        //
        // C & D are project.json, C is dirty, returns A & C.
        [Fact]
        public void PerformUpToDateCheck_WithProjectJsonProjects_ReturnsOnlyDirtyProjects()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetProjectJsonPackageSpec("C", testDirectory.Path);
                var projectD = GetProjectJsonPackageSpec("D", testDirectory.Path);

                // A => B & C
                projectA = projectA.WithTestProjectReference(projectB).WithTestProjectReference(projectC);
                DependencyGraphSpec dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC, projectD);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                var expected = GetUniqueNames(projectA, projectB, projectC, projectD);
                actual.Should().BeEquivalentTo(expected);
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>(), projectA, projectB, projectC, projectD);
                checker.SaveRestoreStatus(results);

                // Set-up, make C dirty.
                projectC = projectC.Clone();
                projectC.RestoreMetadata.ConfigFilePaths = new List<string>() { "newFeed" };
                dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC, projectD);

                // Act & Assert
                actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                expected = GetUniqueNames(projectA, projectC);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public void PerformUpToDateCheck_WithFailedPastRestore_ReturnsADirtyProject()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetPackageSpec("C", testDirectory.Path);

                // A => B
                projectA = projectA.WithTestProjectReference(projectB);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);
                DependencyGraphSpec dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                new List<string>() { projectA.RestoreMetadata.ProjectUniqueName, projectB.RestoreMetadata.ProjectUniqueName, projectC.RestoreMetadata.ProjectUniqueName }.Should().BeEquivalentTo(actual);

                // Set-up, ensure the last status for projectC is a failure.
                var results = RunRestore(failedProjects: new HashSet<string>() { projectC.RestoreMetadata.ProjectUniqueName }, projectsWithWarnings: new HashSet<string>(), projectA, projectB, projectC);
                checker.SaveRestoreStatus(results);

                // Act & Assert
                actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                var expected = new List<string>() { projectC.RestoreMetadata.ProjectUniqueName };
                actual.Should().BeEquivalentTo(expected);
            }
        }

        // A => B => C
        // Delete the outputs of C. Forces only that project to restore.
        [Fact]
        public void PerformUpToDateCheck_WhenALeafProjectHasNoGlobalPackagesFolder_ReturnsOnlyThatProject()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetPackageSpec("C", testDirectory.Path);
                projectC.RestoreMetadata.PackagesPath = Path.Combine(testDirectory.Path, "gpf");
                // A => B
                projectA = projectA.WithTestProjectReference(projectB);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);
                DependencyGraphSpec dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                var expected = GetUniqueNames(projectA, projectB, projectC);
                actual.Should().BeEquivalentTo(expected);
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>(), projectA, projectB, projectC);
                checker.SaveRestoreStatus(results);

                // Set-up, delete C's outputs
                Directory.Delete(projectC.RestoreMetadata.PackagesPath, recursive: true);

                // Act & Assert
                actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                expected = GetUniqueNames(projectC);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public void PerformUpToDateCheck_WithNoChanges_ReplaysWarningsForProjectsWithoutSuppressedWarnings()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetPackageSpec("C", testDirectory.Path);

                // Pretend C is a legacy package reference projects, so we manage it's error list.
                projectC.RestoreSettings = new ProjectRestoreSettings { HideWarningsAndErrors = false };

                // A => B
                projectA = projectA.WithTestProjectReference(projectB);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);

                var dgSpec = new DependencyGraphSpec();
                dgSpec.AddProject(projectA);
                dgSpec.AddRestore(projectA.RestoreMetadata.ProjectUniqueName);
                dgSpec.AddProject(projectB);
                dgSpec.AddRestore(projectB.RestoreMetadata.ProjectUniqueName);
                dgSpec.AddProject(projectC);
                dgSpec.AddRestore(projectC.RestoreMetadata.ProjectUniqueName);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var testLogger = new TestLogger();
                var actual = checker.PerformUpToDateCheck(dgSpec, testLogger);
                var expected = GetUniqueNames(projectA, projectB, projectC);
                actual.Should().BeEquivalentTo(expected);
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>() { projectC.RestoreMetadata.ProjectUniqueName }, projectA, projectB, projectC);
                // The test logger will not contain any messages because we are not running actual restore.
                testLogger.WarningMessages.Should().BeEmpty();
                checker.SaveRestoreStatus(results);

                // Act & Asset.
                testLogger = new TestLogger();
                actual = checker.PerformUpToDateCheck(dgSpec, testLogger);
                testLogger.WarningMessages.Should().HaveCount(1);
                actual.Should().BeEmpty();
            }
        }

        [Fact]
        public void PerformUpToDateCheck_WithNoChanges_DoesNotReplayWarningsForProjectsWithSuppressedWarnings()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetPackageSpec("C", testDirectory.Path);

                // A => B
                projectA = projectA.WithTestProjectReference(projectB);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);

                var dgSpec = new DependencyGraphSpec();
                dgSpec.AddProject(projectA);
                dgSpec.AddRestore(projectA.RestoreMetadata.ProjectUniqueName);
                dgSpec.AddProject(projectB);
                dgSpec.AddRestore(projectB.RestoreMetadata.ProjectUniqueName);
                dgSpec.AddProject(projectC);
                dgSpec.AddRestore(projectC.RestoreMetadata.ProjectUniqueName);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var testLogger = new TestLogger();
                var actual = checker.PerformUpToDateCheck(dgSpec, testLogger);
                var expected = GetUniqueNames(projectA, projectB, projectC);
                actual.Should().BeEquivalentTo(expected);
                // ProjectC will have a warning, but these warnings are not cached because they are from projects that have their warnings/errors suppressed.
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>() { projectC.RestoreMetadata.ProjectUniqueName }, projectA, projectB, projectC);
                // The test logger will not contain any messages because we are not running actual restore.
                testLogger.WarningMessages.Should().BeEmpty();
                checker.SaveRestoreStatus(results);

                // Act & Assert.
                testLogger = new TestLogger();
                actual = checker.PerformUpToDateCheck(dgSpec, testLogger);
                testLogger.WarningMessages.Should().BeEmpty();
                actual.Should().BeEmpty();
            }
        }

        [Fact]
        public void PerformUpToDateCheck_WhenAProjectIsUnloaded_ItsWarningsAreNotReplayed()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetPackageSpec("C", testDirectory.Path);

                projectC.RestoreSettings.HideWarningsAndErrors = false;

                // A => B
                projectA = projectA.WithTestProjectReference(projectB);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);

                var dgSpec = new DependencyGraphSpec();
                dgSpec.AddProject(projectA);
                dgSpec.AddRestore(projectA.RestoreMetadata.ProjectUniqueName);
                dgSpec.AddProject(projectB);
                dgSpec.AddRestore(projectB.RestoreMetadata.ProjectUniqueName);
                dgSpec.AddProject(projectC);
                dgSpec.AddRestore(projectC.RestoreMetadata.ProjectUniqueName);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var testLogger = new TestLogger();
                var actual = checker.PerformUpToDateCheck(dgSpec, testLogger);
                var expected = GetUniqueNames(projectA, projectB, projectC);
                actual.Should().BeEquivalentTo(expected);
                // ProjectC will have a warning
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>() { projectC.RestoreMetadata.ProjectUniqueName }, projectA, projectB, projectC);
                // The test logger will not contain any messages because we are not running actual restore.
                testLogger.WarningMessages.Should().BeEmpty();
                checker.SaveRestoreStatus(results);

                // Run again to verify the warning is replayed once!
                testLogger = new TestLogger();
                actual = checker.PerformUpToDateCheck(dgSpec, testLogger);
                testLogger.WarningMessages.Should().HaveCount(1);
                actual.Should().BeEmpty();
                checker.SaveRestoreStatus(new RestoreSummary[] { });

                // Pretend project C has been unloaded
                dgSpec = new DependencyGraphSpec();
                dgSpec.AddProject(projectA);
                dgSpec.AddRestore(projectA.RestoreMetadata.ProjectUniqueName);
                dgSpec.AddProject(projectB);
                dgSpec.AddRestore(projectB.RestoreMetadata.ProjectUniqueName);

                // Act & Assert.
                testLogger = new TestLogger();
                actual = checker.PerformUpToDateCheck(dgSpec, testLogger);
                testLogger.WarningMessages.Should().BeEmpty();
                actual.Should().BeEmpty();
            }
        }

        // A => B => C
        //   => D
        // E
        // D is gonna be marked dirty when a project reference to E is added
        // The 2nd check should return D & A.
        // The 3rd check should return nothing.
        [Fact]
        public void ReportStatus_WhenPartialResultsAreAvailable_OldStatusIsRetained()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectA = GetPackageSpec("A", testDirectory.Path);
                var projectB = GetPackageSpec("B", testDirectory.Path);
                var projectC = GetPackageSpec("C", testDirectory.Path);
                var projectD = GetPackageSpec("D", testDirectory.Path);
                var projectE = GetPackageSpec("E", testDirectory.Path);

                // A => B
                projectA = projectA.WithTestProjectReference(projectB);
                // B => C
                projectB = projectB.WithTestProjectReference(projectC);
                // A => D
                projectA = projectA.WithTestProjectReference(projectD);

                DependencyGraphSpec dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC, projectD, projectE);

                var checker = new SolutionUpToDateChecker();

                // Preconditions, run 1st check
                var actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                var expected = GetUniqueNames(projectA, projectB, projectC, projectD, projectE);
                actual.Should().BeEquivalentTo(expected);
                var results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>(), projectA, projectB, projectC, projectD, projectE);
                checker.SaveRestoreStatus(results);

                // D => E
                projectD = projectD.WithTestProjectReference(projectE);

                // Set-up dg spec
                dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(projectA, projectB, projectC, projectD, projectE);
                // 2nd check
                actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                expected = GetUniqueNames(projectA, projectD);
                actual.Should().BeEquivalentTo(expected);
                results = RunRestore(failedProjects: new HashSet<string>(), projectsWithWarnings: new HashSet<string>(), projectA, projectD);
                checker.SaveRestoreStatus(results);

                // Finally, last check. Run for a 3rd time. Everything should be up to date
                actual = checker.PerformUpToDateCheck(dgSpec, NullLogger.Instance);
                expected = GetUniqueNames();
                actual.Should().BeEquivalentTo(expected);
            }
        }

        private List<string> GetUniqueNames(params PackageSpec[] packageSpecs)
        {
            var projects = new List<string>();
            foreach (var package in packageSpecs)
            {
                projects.Add(package.RestoreMetadata.ProjectUniqueName);
            }
            return projects;
        }

        private static PackageSpec GetUnknownPackageSpec(string projectName, string rootPath)
        {
            var packageSpec = new PackageSpec();
            var projectPath = Path.Combine(rootPath, projectName, $"{projectName}.csproj");
            packageSpec.RestoreMetadata = new ProjectRestoreMetadata()
            {
                ProjectUniqueName = projectPath,
                ProjectName = projectPath
            };
            packageSpec.FilePath = projectPath;

            return packageSpec;
        }

        private static PackageSpec GetPackageSpec(string projectName, string rootPath = @"C:\")
        {
            const string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net5.0"": {
                            ""dependencies"": {
                            }
                        }
                    }
                }";
            var packageSpec = JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, Path.Combine(rootPath, projectName, projectName)).WithTestRestoreMetadata();
            packageSpec.RestoreSettings.HideWarningsAndErrors = true; // Pretend this is running in VS and this is a .NET Core project.
            return packageSpec;
        }

        private static PackageSpec GetProjectJsonPackageSpec(string projectName, string rootPath = @"C:\")
        {
            const string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net5.0"": {
                            ""dependencies"": {
                            }
                        }
                    }
                }";
            var packageSpec = JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, Path.Combine(rootPath, projectName, projectName));

            var packageSpecFile = new FileInfo(packageSpec.FilePath);
            var projectDir = packageSpecFile.Directory.FullName;
            var projectPath = Path.Combine(projectDir, packageSpec.Name + ".csproj");

            packageSpec.RestoreMetadata = new ProjectRestoreMetadata();
            packageSpec.RestoreMetadata.CrossTargeting = packageSpec.TargetFrameworks.Count > 0;
            packageSpec.RestoreMetadata.OriginalTargetFrameworks = packageSpec.TargetFrameworks.Select(e => e.FrameworkName.GetShortFolderName()).ToList();
            packageSpec.RestoreMetadata.OutputPath = projectDir;
            packageSpec.RestoreMetadata.ProjectStyle = ProjectStyle.ProjectJson;
            packageSpec.RestoreMetadata.ProjectName = packageSpec.Name;
            packageSpec.RestoreMetadata.ProjectUniqueName = projectPath;
            packageSpec.RestoreMetadata.ProjectPath = projectPath;
            packageSpec.RestoreMetadata.ConfigFilePaths = new List<string>();
            packageSpec.RestoreMetadata.CentralPackageVersionsEnabled = false;

            foreach (var framework in packageSpec.TargetFrameworks.Select(e => e.FrameworkName))
            {
                packageSpec.RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(framework));
            }

            return packageSpec;
        }

        private static IReadOnlyList<RestoreSummary> RunRestore(HashSet<string> failedProjects, HashSet<string> projectsWithWarnings, params PackageSpec[] packageSpecs)
        {
            foreach (var spec in packageSpecs)
            {
                CreateDummyOutputFiles(spec);
            }

            return CreateRestoreSummaries(failedProjects, projectsWithWarnings, packageSpecs).ToImmutableList();
        }

        private static IEnumerable<RestoreSummary> CreateRestoreSummaries(HashSet<string> failedProjects, HashSet<string> projectsWithWarnings, params PackageSpec[] packageSpecs)
        {
            foreach (var spec in packageSpecs)
            {
                var status = !failedProjects.Contains(spec.RestoreMetadata.ProjectUniqueName);
                var warnings = projectsWithWarnings.Contains(spec.RestoreMetadata.ProjectUniqueName);
                yield return CreateRestoreSummary(spec, warnings, success: status);
            }
        }

        private static RestoreSummary CreateRestoreSummary(PackageSpec spec, bool warnings, bool success)
        {
            var warningMessages = warnings ?
                new IRestoreLogMessage[] { new RestoreLogMessage(LogLevel.Warning, "Warning, warning, warning!") } :
                new IRestoreLogMessage[] { };

            return new RestoreSummary(
                success: success,
                inputPath: spec.RestoreMetadata.ProjectUniqueName,
                configFiles: new ReadOnlyCollection<string>(new List<string>()),
                feedsUsed: new ReadOnlyCollection<string>(new List<string>()),
                installCount: 0,
                errors: new ReadOnlyCollection<IRestoreLogMessage>(warningMessages));
        }

        internal static void CreateDummyOutputFiles(PackageSpec packageSpec)
        {
            SolutionUpToDateChecker.GetOutputFilePaths(packageSpec, out string assetsFilePath, out string cacheFilePath, out string targetsFilePath, out string propsFilePath, out string lockFilePath);
            var globalPackagesFolderDummyFilePath = packageSpec.RestoreMetadata.PackagesPath != null ?
                Path.Combine(packageSpec.RestoreMetadata.PackagesPath, "dummyFile.txt") :
                null;
            CreateFile(assetsFilePath, cacheFilePath, targetsFilePath, propsFilePath, lockFilePath, globalPackagesFolderDummyFilePath);
        }

        private static void CreateFile(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (path != null)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    File.Create(path).Dispose();
                }
            }
        }
    }
}
