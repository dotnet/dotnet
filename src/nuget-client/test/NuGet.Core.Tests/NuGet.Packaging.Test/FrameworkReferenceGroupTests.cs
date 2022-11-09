// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static NuGet.Frameworks.FrameworkConstants;

namespace NuGet.Packaging.Test
{
    public class FrameworkReferenceGroupTests
    {
        [Fact]
        public void FrameworkReferenceGroup_ThrowsForNullTargetFramework()
        {
            Assert.Throws<ArgumentNullException>(() => new FrameworkReferenceGroup(null, new FrameworkReference[] { }));
        }

        [Fact]
        public void FrameworkReferenceGroup_ThrowsForNullFrameworkReferenceCollection()
        {
            Assert.Throws<ArgumentNullException>(() => new FrameworkReferenceGroup(CommonFrameworks.NetCoreApp30, null));
        }

        [Fact]
        public void FrameworkReferenceGroup_EqualsAndHashCode_AccountForOrdering()
        {
            var frameworkReference1 = new FrameworkReference("ASPNET");
            var frameworkReference2 = new FrameworkReference("WPF");

            var frameworkReferenceGroup1 = new FrameworkReferenceGroup(CommonFrameworks.NetCoreApp30, new FrameworkReference[] { frameworkReference1, frameworkReference2 });
            var frameworkReferenceGroup2 = new FrameworkReferenceGroup(CommonFrameworks.NetCoreApp30, new FrameworkReference[] { frameworkReference2, frameworkReference1 });

            Assert.Equal(frameworkReferenceGroup1, frameworkReferenceGroup2);
            Assert.Equal(frameworkReferenceGroup1.GetHashCode(), frameworkReferenceGroup2.GetHashCode());
        }

        [Fact]
        public void FrameworkReferenceGroup_EqualsAndHashCode_AccountForTargetFramework()
        {
            var frameworkReference1 = new FrameworkReference("ASPNET");
            var frameworkReference2 = new FrameworkReference("WPF");

            var frameworkReferenceGroup1 = new FrameworkReferenceGroup(CommonFrameworks.NetCoreApp30, new FrameworkReference[] { frameworkReference1, frameworkReference2 });
            var frameworkReferenceGroup2 = new FrameworkReferenceGroup(CommonFrameworks.NetCoreApp22, new FrameworkReference[] { frameworkReference1, frameworkReference2 });

            Assert.NotEqual(frameworkReferenceGroup1, frameworkReferenceGroup2);
            Assert.NotEqual(frameworkReferenceGroup1.GetHashCode(), frameworkReferenceGroup2.GetHashCode());
        }

    }
}
