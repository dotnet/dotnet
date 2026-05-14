// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public class ImplicitProjectActionTests
    {
        private static readonly string Id = "a";
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(id: "b", NuGetVersion.Parse("1.2.3"));
        private static readonly ImplicitProjectAction Action = new ImplicitProjectAction(Id, PackageIdentity, NuGetProjectActionType.Install);

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_WhenIdIsNullOrEmpty_Throws(string id)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => new ImplicitProjectAction(id, PackageIdentity, NuGetProjectActionType.Install));

            Assert.StartsWith("The argument cannot be null or empty.", exception.Message);
            Assert.Equal("id", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenPackageIdentityIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                () => new ImplicitProjectAction(Id, packageIdentity: null, NuGetProjectActionType.Uninstall));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.Equal("packageIdentity", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenArgumentsAreValid_InitializesMembers()
        {
            const NuGetProjectActionType projectActionType = NuGetProjectActionType.Install;

            var action = new ImplicitProjectAction(Id, PackageIdentity, projectActionType);

            Assert.Equal(Id, action.Id);
            Assert.Equal(PackageIdentity, action.PackageIdentity);
            Assert.Equal(projectActionType, action.ProjectActionType);
        }

        [Fact]
        public void Equals_WithObject_WhenArgumentIsNull_ReturnsFalse()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.False(Action.Equals(obj: null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        [Fact]
        public void Equals_WithObject_WhenArgumentIsNotImplicitProjectAction_ReturnsFalse()
        {
            Assert.False(Action.Equals(obj: string.Empty));
        }

        [Fact]
        public void Equals_WithObject_WhenArgumentIsDifferentImplicitProjectAction_ReturnsFalse()
        {
            var otherAction = new ImplicitProjectAction(
                id: "b",
                new PackageIdentity("c", NuGetVersion.Parse("2.3.4")),
                NuGetProjectActionType.Install);

            Assert.False(Action.Equals(obj: otherAction));
        }

        [Fact]
        public void Equals_WithObject_WhenArgumentIsEqualImplicitProjectAction_ReturnsTrue()
        {
            var otherAction = new ImplicitProjectAction(
                Action.Id,
                Action.PackageIdentity,
                Action.ProjectActionType);

            Assert.True(Action.Equals(obj: otherAction));
        }

        [Fact]
        public void Equals_WithObject_WhenArgumentIsSameImplicitProjectAction_ReturnsTrue()
        {
            Assert.True(Action.Equals(obj: Action));
        }

        [Fact]
        public void Equals_WithImplicitProjectAction_WhenArgumentIsNull_ReturnsFalse()
        {
            Assert.False(Action.Equals(other: null));
        }

        [Fact]
        public void Equals_WithImplicitProjectAction_WhenArgumentIsDifferentImplicitProjectAction_ReturnsFalse()
        {
            var otherAction = new ImplicitProjectAction(
                id: "b",
                new PackageIdentity("c", NuGetVersion.Parse("2.3.4")),
                NuGetProjectActionType.Install);

            Assert.False(Action.Equals(other: otherAction));
        }

        [Fact]
        public void Equals_WithImplicitProjectAction_WhenArgumentIsEqualImplicitProjectAction_ReturnsTrue()
        {
            var otherAction = new ImplicitProjectAction(
                Action.Id,
                Action.PackageIdentity,
                Action.ProjectActionType);

            Assert.True(Action.Equals(other: otherAction));
        }

        [Fact]
        public void Equals_WithImplicitProjectAction_WhenArgumentIsSameImplicitProjectAction_ReturnsTrue()
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
