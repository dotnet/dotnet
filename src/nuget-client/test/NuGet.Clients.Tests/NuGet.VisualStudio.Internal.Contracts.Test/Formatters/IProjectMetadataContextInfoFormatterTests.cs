// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public class IProjectMetadataContextInfoFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(IProjectMetadataContextInfos))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(IProjectMetadataContextInfo expectedResult)
        {
            IProjectMetadataContextInfo? actualResult = SerializeThenDeserialize(IProjectMetadataContextInfoFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.FullPath, actualResult!.FullPath);
            Assert.Equal(expectedResult.Name, actualResult.Name);
            Assert.Equal(expectedResult.ProjectId, actualResult.ProjectId);
            Assert.Equal(expectedResult.SupportedFrameworks, actualResult.SupportedFrameworks);
            Assert.Equal(expectedResult.TargetFramework, actualResult.TargetFramework);
            Assert.Equal(expectedResult.UniqueName, actualResult.UniqueName);
        }

        public static TheoryData<IProjectMetadataContextInfo> IProjectMetadataContextInfos => new()
            {
                {
                    new ProjectMetadataContextInfo(
                        fullPath: null,
                        name: null,
                        projectId: null,
                        supportedFrameworks: null,
                        targetFramework: null,
                        uniqueName: null)
                },
                {
                    new ProjectMetadataContextInfo(
                        fullPath: string.Empty,
                        name: string.Empty,
                        projectId: string.Empty,
                        supportedFrameworks: Array.Empty<NuGetFramework>(),
                        targetFramework: null,
                        uniqueName: string.Empty)
                },
                {
                    new ProjectMetadataContextInfo(
                        fullPath: "a",
                        name: "b",
                        projectId: "c",
                        supportedFrameworks: new[] { NuGetFramework.Parse("net472"), NuGetFramework.Parse("net48") },
                        targetFramework: NuGetFramework.Parse("net50"),
                        uniqueName: "d")
                }
            };
    }
}
