// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class ProjectActionFormatterTests : FormatterTests
    {
        private static readonly string Id = "a";
        private static readonly string ProjectId = "b";
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(id: "c", NuGetVersion.Parse("1.2.3"));
        private static readonly PackageIdentity PackageIdentityWithoutVersion = new PackageIdentity(id: "d", version: null);
        private static readonly ImplicitProjectAction[] ImplicitProjectActions = new[]
            {
                new ImplicitProjectAction(id: "e", new PackageIdentity(id: "f", NuGetVersion.Parse("4.5.6")), NuGetProjectActionType.Install )
            };

        [Theory]
        [MemberData(nameof(ProjectActions))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(ProjectAction expectedResult)
        {
            ProjectAction? actualResult = SerializeThenDeserialize(ProjectActionFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        public static TheoryData ProjectActions => new TheoryData<ProjectAction>
            {
                { new ProjectAction(Id, ProjectId, PackageIdentityWithoutVersion, NuGetProjectActionType.Uninstall, implicitActions: null) },
                { new ProjectAction(Id, ProjectId, PackageIdentity, NuGetProjectActionType.Install, ImplicitProjectActions) }
            };
    }
}
