// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Xunit;

namespace MultitargetedNetFrameworkProject
{
    public class UnitTest1
    {
#if NET481
        public string TargetFramework { get; } = "NET481";
#endif
        [Fact]
        public void FailsUntilNet462ButPassesOnNewerNetFramework()
        {
            var expected = Environment.GetEnvironmentVariable("EXPECTED_TARGET_FRAMEWORK");

            Assert.Equal(expected, TargetFramework, ignoreCase: true);
        }
    }
}
