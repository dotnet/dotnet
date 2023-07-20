// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Common;
using NuGet.ProjectModel;
using Xunit;

namespace NuGet.Commands.Test.RestoreCommandTests.RequestFactory
{
    public class DependencyGraphSpecRequestProviderTests
    {
        [Fact]
        public void GetMessagesForProject_WithMultipleMessages_SelectedMessagesForCorrectProject()
        {
            // Arrange
            var project1 = @"z:\src\solution\project1\project1.csproj";
            var project2 = @"z:\src\solution\project2\project2.csproj";

            var project1Error = RestoreLogMessage.CreateError(NuGetLogCode.NU1001, "project1 error");
            project1Error.ProjectPath = project1;

            var project2Error = RestoreLogMessage.CreateError(NuGetLogCode.NU1002, "project2 error");
            project2Error.ProjectPath = project2;

            var solutionMessages = new List<IAssetsLogMessage>()
            {
                AssetsLogMessage.Create(project1Error),
                AssetsLogMessage.Create(project2Error)
            };

            // Act
            IReadOnlyList<IAssetsLogMessage> actual = DependencyGraphSpecRequestProvider.GetMessagesForProject(solutionMessages, project1);

            // Assert
            var actualError = Assert.Single(actual);
            Assert.Equal(NuGetLogCode.NU1001, actualError.Code);
        }
    }
}
