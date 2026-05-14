// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace Microsoft.DotNet.SourceBuild.Tasks.Tests
{
    public class WriteBuildOutputPropsTests
    {
        [Fact]
        public void TestPropertyNameHelpers()
        {
            Assert.Equal(
                "MicrosoftNETCoreAppRefPackageVersion",
                WriteBuildOutputProps.GetPropertyName("Microsoft.NETCore.App.Ref"));

            Assert.Equal(
                "MicrosoftNETCoreAppRefVersion",
                WriteBuildOutputProps.GetAlternatePropertyName("Microsoft.NETCore.App.Ref"));
        }
    }
}
