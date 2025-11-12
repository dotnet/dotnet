// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Common.Migrations;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Common.Test
{
    public class NuGetEnvironmentTests
    {
        [PlatformFact(Platform.Linux)]
        public void GetFolderPath_Temp_Success()
        {
            var nuGetTempDirectory = NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp);
            Assert.Equal("700", Migration1.GetPermissions(nuGetTempDirectory).ToString());
        }

        public static IEnumerable<object[]> AllNuGetFolderPaths()
        {
            foreach (NuGetFolderPath folderPath in (NuGetFolderPath[])System.Enum.GetValues(typeof(NuGetFolderPath)))
            {
                if (folderPath == NuGetFolderPath.DefaultMsBuildPath)
                {
                    // Skip DefaultMsBuildPath as it throws on Mac and Linux
                    continue;
                }
                yield return new object[] { folderPath };
            }
        }

        [Theory]
        [MemberData(nameof(AllNuGetFolderPaths))]
        public void GetFolderPath_MultipleCalls_ReturnsCachedInstance(NuGetFolderPath folder)
        {
            var firstCall = NuGetEnvironment.GetFolderPath(folder);
            var secondCall = NuGetEnvironment.GetFolderPath(folder);
            Assert.Same(firstCall, secondCall);
        }
    }
}
