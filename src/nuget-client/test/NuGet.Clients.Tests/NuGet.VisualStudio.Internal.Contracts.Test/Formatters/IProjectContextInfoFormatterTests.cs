// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class IProjectContextInfoFormatterTests : FormatterTests
    {
        private static readonly string ProjectId = "a";

        [Theory]
        [MemberData(nameof(IProjectContextInfos))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(IProjectContextInfo expectedResult)
        {
            IProjectContextInfo? actualResult = SerializeThenDeserialize(IProjectContextInfoFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.ProjectId, actualResult!.ProjectId);
            Assert.Equal(expectedResult.ProjectStyle, actualResult.ProjectStyle);
            Assert.Equal(expectedResult.ProjectKind, actualResult.ProjectKind);
        }

        public static TheoryData IProjectContextInfos => new TheoryData<IProjectContextInfo>
            {
                { new ProjectContextInfo(ProjectId, ProjectModel.ProjectStyle.PackageReference, NuGetProjectKind.PackageReference) }
            };
    }
}
