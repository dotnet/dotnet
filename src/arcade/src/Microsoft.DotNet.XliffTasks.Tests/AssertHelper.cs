// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace XliffTasks.Tests
{
    static class AssertEx
    {
        public static void EqualIgnoringLineEndings(string expected, string actual)
        {
            Assert.Equal(
                expected.Replace("\r", string.Empty),
                actual.Replace("\r", string.Empty));
        }
    }
}
