// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.VisualStudio.Common.Test.ProjectSystems
{
    public class ProjectNamesTests
    {
        private static readonly string GuidString = Guid.NewGuid().ToString();

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_FullNameIsNullOrEmpty_ThrowsArgumentException(string fullName)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            new ProjectNames(fullName,
                uniqueName: "proj",
                shortName: "proj",
                customUniqueName: "proj",
                projectId: GuidString));

            Assert.Equal("fullName", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_UniqueNameIsNullOrEmpty_ThrowsArgumentException(string uniqueName)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            new ProjectNames(fullName: "proj",
                uniqueName: uniqueName,
                shortName: "proj",
                customUniqueName: "proj",
                projectId: GuidString));

            Assert.Equal("uniqueName", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_ShortNameIsNullOrEmpty_ThrowsArgumentException(string shortName)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            new ProjectNames(fullName: "proj",
                uniqueName: "proj",
                shortName: shortName,
                customUniqueName: "proj",
                projectId: GuidString));

            Assert.Equal("shortName", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_CustomUniqeuNameIsNullOrEmpty_ThrowsArgumentException(string customFullName)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            new ProjectNames(fullName: "proj",
                uniqueName: "proj",
                shortName: "proj",
                customUniqueName: customFullName,
                projectId: GuidString));

            Assert.Equal("customUniqueName", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("proj")]
        [InlineData("b9f8e0c3d8374ca59c5cd9f4d80e1a4")] // one too few characters
        [InlineData("b9f8e0c3d8374ca59c5cd9f4d80e1a4bc")] // one too many characters
        public void Ctor_ProjectIdIsNotValidGuid_ThrowsArgumentException(string projectId)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            new ProjectNames(fullName: "proj",
                uniqueName: "proj",
                shortName: "proj",
                customUniqueName: "proj",
                projectId: projectId));

            Assert.Equal("projectId", exception.ParamName);
        }

        [Theory]
        [InlineData("b9f8e0c3d8374ca59c5cd9f4d80e1a4b")]
        [InlineData("b9f8e0c3-d837-4ca5-9c5c-d9f4d80e1a4b")]
        [InlineData("{b9f8e0c3-d837-4ca5-9c5c-d9f4d80e1a4b}")]
        [InlineData("B9F8E0C3-D837-4CA5-9C5C-D9F4D80E1A4B")]
        [InlineData("{B9F8E0C3-D837-4CA5-9C5C-D9F4D80E1A4B}")]
        public void Ctor_ProjectIdIsValidGuid_NormalizesGuid(string projectIdString)
        {
            var projectId = new ProjectNames(
                fullName: "proj",
                uniqueName: "proj",
                shortName: "proj",
                customUniqueName: "proj",
                projectId: projectIdString);

            Assert.Equal("b9f8e0c3-d837-4ca5-9c5c-d9f4d80e1a4b", projectId.ProjectId);
        }
    }
}
