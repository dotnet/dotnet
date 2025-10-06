// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class MsBuildToolsetTest
    {
        [PlatformFact(Platform.Windows)]
        public void WhenNullIsPassedForVersionParameterThenMsBuildVersionIsFetchedFromPath_Success()
        {
            //Arrange
            var msbuildPath = Util.GetMsbuildPathOnWindows();

            //Act
            var toolset = new MsBuildToolset(version: null, path: msbuildPath);

            //Assert
            Assert.Equal(msbuildPath, toolset.Path);
            Assert.True(toolset.ParsedVersion.CompareTo(new Version()) > 0);
        }
    }
}
