// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.ProjectManagement;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public class ProjectMetadataContextInfoTests
    {
        [Fact]
        public void Create_IfProjectMetadataIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                () => ProjectMetadataContextInfo.Create(projectMetadata: null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.Equal("projectMetadata", exception.ParamName);
        }

        [Fact]
        public void Create_IfProjectMetadataIsEmpty_DoesNotThrow()
        {
            ProjectMetadataContextInfo projectMetadata = ProjectMetadataContextInfo.Create(new Dictionary<string, object?>());

            AssertAllPropertiesAreNull(projectMetadata);
        }

        [Fact]
        public void Create_IfProjectMetadataValueDataTypesAreUnexpected_ReturnsNullPropertyValues()
        {
            var metadata = new Dictionary<string, object?>()
            {
                { NuGetProjectMetadataKeys.FullPath, 1 },
                { NuGetProjectMetadataKeys.Name, 1 },
                { NuGetProjectMetadataKeys.ProjectId, 1 },
                { NuGetProjectMetadataKeys.SupportedFrameworks, 1 },
                { NuGetProjectMetadataKeys.TargetFramework, 1 },
                { NuGetProjectMetadataKeys.UniqueName, 1 }
            };

            ProjectMetadataContextInfo projectMetadata = ProjectMetadataContextInfo.Create(metadata);

            AssertAllPropertiesAreNull(projectMetadata);
        }

        [Fact]
        public void Create_IfProjectMetadataValuesAreNull_ReturnsNullPropertyValues()
        {
            var metadata = new Dictionary<string, object?>()
            {
                { NuGetProjectMetadataKeys.FullPath, null },
                { NuGetProjectMetadataKeys.Name, null },
                { NuGetProjectMetadataKeys.ProjectId, null },
                { NuGetProjectMetadataKeys.SupportedFrameworks, null },
                { NuGetProjectMetadataKeys.TargetFramework, null },
                { NuGetProjectMetadataKeys.UniqueName, null }
            };

            ProjectMetadataContextInfo projectMetadata = ProjectMetadataContextInfo.Create(metadata);

            AssertAllPropertiesAreNull(projectMetadata);
        }

        [Fact]
        public void Create_IfProjectMetadataValuesAreNonNullAndValid_ReturnsNonNullPropertyValues()
        {
            var metadata = new Dictionary<string, object?>()
            {
                { NuGetProjectMetadataKeys.FullPath, "a" },
                { NuGetProjectMetadataKeys.Name, "b" },
                { NuGetProjectMetadataKeys.ProjectId, "c" },
                { NuGetProjectMetadataKeys.SupportedFrameworks, new[] { NuGetFramework.Parse("net48"), NuGetFramework.Parse("net50") } },
                { NuGetProjectMetadataKeys.TargetFramework, NuGetFramework.Parse("net50") },
                { NuGetProjectMetadataKeys.UniqueName, "d" }
            };

            ProjectMetadataContextInfo projectMetadata = ProjectMetadataContextInfo.Create(metadata);

            AssertEqual(metadata, projectMetadata);
        }

        [Fact]
        public void Create_IfTargetFrameworkValueIsString_ReturnsNuGetFrameworkValue()
        {
            NuGetFramework expectedFramework = NuGetFramework.Parse("net50");

            var metadata = new Dictionary<string, object?>()
            {
                { NuGetProjectMetadataKeys.TargetFramework, expectedFramework.DotNetFrameworkName }
            };

            ProjectMetadataContextInfo projectMetadata = ProjectMetadataContextInfo.Create(metadata);

            Assert.Equal(expectedFramework, projectMetadata.TargetFramework);
        }

        private static void AssertAllPropertiesAreNull(ProjectMetadataContextInfo projectMetadata)
        {
            Assert.Null(projectMetadata.FullPath);
            Assert.Null(projectMetadata.Name);
            Assert.Null(projectMetadata.ProjectId);
            Assert.Null(projectMetadata.SupportedFrameworks);
            Assert.Null(projectMetadata.TargetFramework);
            Assert.Null(projectMetadata.UniqueName);
        }

        private static void AssertEqual(Dictionary<string, object?> expectedResults, ProjectMetadataContextInfo actualResult)
        {
            Assert.Equal(expectedResults[NuGetProjectMetadataKeys.FullPath] as string, actualResult.FullPath);
            Assert.Equal(expectedResults[NuGetProjectMetadataKeys.Name] as string, actualResult.Name);
            Assert.Equal(expectedResults[NuGetProjectMetadataKeys.ProjectId] as string, actualResult.ProjectId);
            Assert.Equal((expectedResults[NuGetProjectMetadataKeys.SupportedFrameworks] as IEnumerable<NuGetFramework>)!, actualResult.SupportedFrameworks!);
            Assert.Equal(expectedResults[NuGetProjectMetadataKeys.TargetFramework] as NuGetFramework, actualResult.TargetFramework);
            Assert.Equal(expectedResults[NuGetProjectMetadataKeys.UniqueName] as string, actualResult.UniqueName);
        }
    }
}
