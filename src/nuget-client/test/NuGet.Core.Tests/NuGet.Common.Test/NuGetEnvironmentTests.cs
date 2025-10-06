// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    }
}
