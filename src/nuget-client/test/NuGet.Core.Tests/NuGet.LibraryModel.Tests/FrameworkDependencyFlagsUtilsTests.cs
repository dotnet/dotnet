// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

namespace NuGet.LibraryModel.Tests
{
    public class FrameworkDependencyFlagsUtilsTests
    {
        [Fact]
        public void FrameworkDependencyFlagsUtils_GetFlagString_ReturnsExpectedString()
        {
            Assert.Equal("all", FrameworkDependencyFlagsUtils.GetFlagString(FrameworkDependencyFlags.All));
            Assert.Equal("none", FrameworkDependencyFlagsUtils.GetFlagString(FrameworkDependencyFlags.None));
        }

        [Fact]
        public void FrameworkDependencyFlagsUtils_GetFlagsFromString_ReturnsExpectedFlags()
        {
            Assert.Equal(FrameworkDependencyFlags.All, FrameworkDependencyFlagsUtils.GetFlags("all"));
            Assert.Equal(FrameworkDependencyFlags.All, FrameworkDependencyFlagsUtils.GetFlags("All"));
            Assert.Equal(FrameworkDependencyFlags.None, FrameworkDependencyFlagsUtils.GetFlags("None"));
            Assert.Equal(FrameworkDependencyFlags.None, FrameworkDependencyFlagsUtils.GetFlags("none"));
            Assert.Equal(FrameworkDependencyFlags.None, FrameworkDependencyFlagsUtils.GetFlags((string?)null));
            Assert.Equal(FrameworkDependencyFlags.All, FrameworkDependencyFlagsUtils.GetFlags("none,all")); // Stupid to write this, but pointless to enforce that people don't :)
            Assert.Equal(FrameworkDependencyFlags.All, FrameworkDependencyFlagsUtils.GetFlags("all,none")); // Stupid to write this, but pointless to enforce that people don't :)
        }

        [Fact]
        public void FrameworkDependencyFlagsUtils_GetFlagsFromAnEnumerable_ReturnsExpectedFlags()
        {
            Assert.Equal(FrameworkDependencyFlags.None, FrameworkDependencyFlagsUtils.GetFlags((IEnumerable<string>?)null));
            Assert.Equal(FrameworkDependencyFlags.All, FrameworkDependencyFlagsUtils.GetFlags(new string[] { "all" }));
            Assert.Equal(FrameworkDependencyFlags.None, FrameworkDependencyFlagsUtils.GetFlags(new string[] { "none" }));
            Assert.Equal(FrameworkDependencyFlags.All, FrameworkDependencyFlagsUtils.GetFlags(new string[] { "all", "none" }));
            Assert.Equal(FrameworkDependencyFlags.All, FrameworkDependencyFlagsUtils.GetFlags(new string[] { "none", "all" }));
            Assert.Equal(FrameworkDependencyFlags.All, FrameworkDependencyFlagsUtils.GetFlags(new string[] { "All", "None" }));


        }
    }
}
