// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public class ProjectActionTests
    {
        private static readonly string Id = "a";
        private static readonly string ProjectId = "b";
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(id: "c", NuGetVersion.Parse("1.2.3"));
        private static readonly ImplicitProjectAction ImplicitAction = new ImplicitProjectAction(id: "d", PackageIdentity, NuGetProjectActionType.Install);
        private static readonly ProjectAction Action = new ProjectAction(Id, ProjectId, PackageIdentity, NuGetProjectActionType.Install, new[] { ImplicitAction });

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_WhenIdIsNullOrEmpty_Throws(string id)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => new ProjectAction(id, ProjectId, PackageIdentity, NuGetProjectActionType.Install, implicitActions: null));

            Assert.StartsWith("The argument cannot be null or empty.", exception.Message);
            Assert.Equal("id", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_WhenProjectIdIsNullOrEmpty_Throws(string projectId)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => new ProjectAction(Id, projectId, PackageIdentity, NuGetProjectActionType.Install, implicitActions: null));

            Assert.StartsWith("The argument cannot be null or empty.", exception.Message);
            Assert.Equal("projectId", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenPackageIdentityIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                () => new ProjectAction(Id, ProjectId, packageIdentity: null, NuGetProjectActionType.Uninstall, implicitActions: null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.Equal("packageIdentity", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenArgumentsAreValid_InitializesMembers()
        {
            const NuGetProjectActionType projectActionType = NuGetProjectActionType.Install;

            var action = new ProjectAction(Id, ProjectId, PackageIdentity, projectActionType, new[] { ImplicitAction });

            Assert.Equal(Id, action.Id);
            Assert.Equal(ProjectId, action.ProjectId);
            Assert.Equal(PackageIdentity, action.PackageIdentity);
            Assert.Equal(projectActionType, action.ProjectActionType);
            Assert.Equal(new[] { ImplicitAction }, action.ImplicitActions);
        }

        [Fact]
        public void Equals_WithObject_WhenArgumentIsNull_ReturnsFalse()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.False(Action.Equals(obj: null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        [Fact]
        public void Equals_WithObject_WhenArgumentIsNotProjectAction_ReturnsFalse()
        {
            Assert.False(Action.Equals(obj: string.Empty));
        }

        [Fact]
        public void Equals_WithObject_WhenArgumentIsDifferentProjectAction_ReturnsFalse()
        {
            var otherAction = new ProjectAction(
                id: "b",
                projectId: "c",
                new PackageIdentity("d", NuGetVersion.Parse("2.3.4")),
                NuGetProjectActionType.Install,
                implicitActions: null);

            Assert.False(Action.Equals(obj: otherAction));
        }

        [Fact]
        public void Equals_WithObject_WhenArgumentIsEqualProjectAction_ReturnsTrue()
        {
            var otherAction = new ProjectAction(
                Action.Id,
                Action.ProjectId,
                Action.PackageIdentity,
                Action.ProjectActionType,
                Action.ImplicitActions);

            Assert.True(Action.Equals(obj: otherAction));
        }

        [Fact]
        public void Equals_WithObject_WhenArgumentIsSameProjectAction_ReturnsTrue()
        {
            Assert.True(Action.Equals(obj: Action));
        }

        [Fact]
        public void Equals_WithProjectAction_WhenArgumentIsNull_ReturnsFalse()
        {
            Assert.False(Action.Equals(other: null));
        }

        [Fact]
        public void Equals_WithProjectAction_WhenArgumentIsDifferentProjectAction_ReturnsFalse()
        {
            var otherAction = new ProjectAction(
                id: "b",
                projectId: "c",
                new PackageIdentity("d", NuGetVersion.Parse("2.3.4")),
                NuGetProjectActionType.Install,
                implicitActions: null);

            Assert.False(Action.Equals(other: otherAction));
        }

        [Fact]
        public void Equals_WithProjectAction_WhenArgumentIsEqualProjectAction_ReturnsTrue()
        {
            var otherAction = new ProjectAction(
                Action.Id,
                Action.ProjectId,
                Action.PackageIdentity,
                Action.ProjectActionType,
                Action.ImplicitActions);

            Assert.True(Action.Equals(other: otherAction));
        }

        [Fact]
        public void Equals_WithProjectAction_WhenArgumentIsSameProjectAction_ReturnsTrue()
        {
            Assert.True(Action.Equals(other: Action));
        }

        [Fact]
        public void GetHashCode_Always_ReturnsIdHashCode()
        {
            int expectedResult = Action.Id.GetHashCode();
            int actualResult = Action.GetHashCode();

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
