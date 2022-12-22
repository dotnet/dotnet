// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    public class IVsTargetFrameworkInfo3Test
    {
        [Fact]
        public void IVsTargetFrameworkInfo3_IsIVsTargetFrameworkInfo2WithCentralPackageVersions()
        {
            // Arrange
            var targetFrameworkInfo3 = new VsTargetFrameworkInfo3(
                targetFrameworkMoniker: "4.0",
                packageReferences: new List<IVsReferenceItem>(),
                projectReferences: new List<IVsReferenceItem>(),
                packageDownloads: new List<IVsReferenceItem>(),
                frameworkReferences: new List<IVsReferenceItem>(),
                projectProperties: new List<IVsProjectProperty>(),
                centralPackageVersions: new List<IVsReferenceItem>());

            // Assert
            Assert.True(targetFrameworkInfo3 is IVsTargetFrameworkInfo2);
            Assert.Equal(0, ((IVsTargetFrameworkInfo3)targetFrameworkInfo3).CentralPackageVersions.Count);
        }
    }
}
