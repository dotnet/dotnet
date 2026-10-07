// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.ProjectModel;
using NuGet.VisualStudio.SolutionExplorer.Models;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.SolutionExplorer.Models
{
    public class AssetsFileLogMessageTests
    {
        [Fact]
        public void Constructor_WithMissingLibraryId_Throws()
        {
            var logMessage = new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "message");

            Assert.Throws<ArgumentException>(
                () => new AssetsFileLogMessage("project.csproj", logMessage));
        }

        [Fact]
        public void Equals_WithMissingLibraryId_ReturnsFalse()
        {
            var logMessage = new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "message")
            {
                LibraryId = "library"
            };
            var assetsFileLogMessage = new AssetsFileLogMessage("project.csproj", logMessage);
            var other = new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1000, "message");

            Assert.False(assetsFileLogMessage.Equals(other, "project.csproj"));
        }
    }
}
