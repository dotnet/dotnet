// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.ProjectModel;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    public class ProjectRestoreReferenceComparerTests
    {
        [Fact]
        public void GetHashCode_NullReference_ThrowsArgumentNullException()
        {
            static void CallMethod()
            {
                ProjectRestoreReferenceComparer.Default.GetHashCode(null);
            }

            Assert.Throws<ArgumentNullException>(CallMethod);
        }

        [Fact]
        public void Equals_NullReference_ThrowsArgumentNullException()
        {
            // Arrange
            ProjectRestoreReference projectReference = new()
            {
                ProjectPath = @"n:\path\to\project.csproj"
            };
            projectReference.ProjectUniqueName = projectReference.ProjectPath;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ProjectRestoreReferenceComparer.Default.Equals(null, projectReference));
            Assert.Throws<ArgumentNullException>(() => ProjectRestoreReferenceComparer.Default.Equals(projectReference, null));
        }

        [Theory]
        [InlineData(@"n:\path\to\project.csproj", @"n:\path\to\project.csproj", true)]
        [InlineData(@"n:\path\to\project.csproj", @"N:\path\to\project.csproj", true)]
        [InlineData(@"n:\src\prj1\prj1.csproj", @"n:\src\prj2\prj2.csproj", false)]
        public void GetHashCode_CompareHashCodeOfTwoProjects_HaveExpectedResult(string path1, string path2, bool areEqual)
        {
            // Arrange
            ProjectRestoreReference project1 = new()
            {
                ProjectPath = path1,
                ProjectUniqueName = path1
            };

            ProjectRestoreReference project2 = new()
            {
                ProjectPath = path2,
                ProjectUniqueName = path2
            };

            // Act
            var hashCode1 = ProjectRestoreReferenceComparer.Default.GetHashCode(project1);
            var hashCode2 = ProjectRestoreReferenceComparer.Default.GetHashCode(project2);

            // Assert hashCo
            Assert.Equal(areEqual, hashCode1 == hashCode2);
        }

        [Theory]
        [InlineData(@"n:\path\to\project.csproj", @"n:\path\to\project.csproj", true)]
        [InlineData(@"n:\path\to\project.csproj", @"N:\path\to\project.csproj", true)]
        [InlineData(@"n:\src\prj1\prj1.csproj", @"n:\src\prj2\prj2.csproj", false)]
        public void Equals_CompareHashCodeOfTwoProjects_HaveExpectedResult(string path1, string path2, bool areEqual)
        {
            // Arrange
            ProjectRestoreReference project1 = new()
            {
                ProjectPath = path1,
                ProjectUniqueName = path1
            };

            ProjectRestoreReference project2 = new()
            {
                ProjectPath = path2,
                ProjectUniqueName = path2
            };

            // Act
            var actual = ProjectRestoreReferenceComparer.Default.Equals(project1, project2);

            // Assert hashCo
            Assert.Equal(areEqual, actual);
        }
    }
}
