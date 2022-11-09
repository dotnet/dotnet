// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Xunit;

namespace MultitargetedNetFrameworkProject
{
    public class UnitTest1
    {
#if NET462
        public string TargetFramework { get; } = "NET462";
#endif

#if NET47
        public string TargetFramework { get; } = "NET47";
#endif

#if NET471
        public string TargetFramework { get; } = "NET471";
#endif

#if NET472
        public string TargetFramework { get; } = "NET472";
#endif

#if NET48
        public string TargetFramework { get; } = "NET48";
#endif
        [Fact]
        public void FailsUntilNet462ButPassesOnNewerNetFramework()
        {
            var expected = Environment.GetEnvironmentVariable("EXPECTED_TARGET_FRAMEWORK");

            Assert.Equal(expected, TargetFramework, ignoreCase: true);
        }
    }
}
