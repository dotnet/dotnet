// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class FrameworkReferenceTests
    {

        [Fact]
        public void FrameworkReference_ThrowsForNullTargetFramework()
        {
            Assert.Throws<ArgumentNullException>(() => new FrameworkReference(null));
        }

        [Fact]
        public void FrameworkReference_FrameworkNameIsCaseInsensitive()
        {
            var frameworkReference1 = new FrameworkReference("Microsoft");
            var frameworkReference2 = new FrameworkReference("microSoft");

            Assert.Equal(frameworkReference1, frameworkReference2);
        }


        [Fact]
        public void FrameworkReference_EqualObjectsHaveSameHashCode()
        {
            var frameworkReference1 = new FrameworkReference("Microsoft");
            var frameworkReference2 = new FrameworkReference("microSoft");

            Assert.Equal(frameworkReference1, frameworkReference2);
            Assert.Equal(frameworkReference1.GetHashCode(), frameworkReference2.GetHashCode());
        }
    }
}
