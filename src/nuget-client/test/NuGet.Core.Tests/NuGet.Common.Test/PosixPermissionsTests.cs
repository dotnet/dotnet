// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common.Migrations;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Common.Test
{
    public class PosixPermissionsTests
    {
        [PlatformTheory(Platform.Darwin, Platform.Linux)]
        [InlineData("077")]
        [InlineData("777")]
        public void Parse_PosixPermissions_Success(string permissions)
        {
            var posixPermissions = PosixPermissions.Parse(permissions);
            Assert.Equal(permissions, posixPermissions.ToString());
        }

        [PlatformTheory(Platform.Darwin, Platform.Linux)]
        [InlineData("644", "022", true)]
        [InlineData("755", "022", true)]
        [InlineData("775", "002", true)]
        [InlineData("644", "222", false)]
        [InlineData("777", "022", false)]
        [InlineData("777", "002", false)]
        public void SatisfiesUmask_PermissionsSatisfyUmask_Success(string permissions, string umask, bool expectedResult)
        {
            var posixPermissions = PosixPermissions.Parse(permissions);
            var umaskPermissions = PosixPermissions.Parse(umask);

            bool result = posixPermissions.SatisfiesUmask(umaskPermissions);

            Assert.Equal(expectedResult, result);
        }

        [PlatformTheory(Platform.Darwin, Platform.Linux)]
        [InlineData("777", "002", "775")]
        [InlineData("775", "022", "755")]
        [InlineData("644", "022", "644")]
        [InlineData("664", "022", "644")]
        [InlineData("666", "222", "444")]
        public void WithUmask_ApplyUmaskOnPermissions_Success(string permissions, string umask, string expectedpermissions)
        {
            var posixPermissions = PosixPermissions.Parse(permissions);
            var umaskPermissions = PosixPermissions.Parse(umask);

            PosixPermissions newPermissions = posixPermissions.WithUmask(umaskPermissions);

            Assert.Equal(expectedpermissions, newPermissions.ToString());
        }
    }
}
