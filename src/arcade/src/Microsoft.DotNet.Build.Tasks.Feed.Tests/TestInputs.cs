// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public static class TestInputs
    {
        public static string GetFullPath(string relativeTestInputPath)
        {
            return Path.Combine(
                Path.GetDirectoryName(typeof(TestInputs).Assembly.Location),
                "TestInputs",
                relativeTestInputPath);
        }

        public static byte[] ReadAllBytes(string relativeTestInputPath)
        {
            var path = GetFullPath(relativeTestInputPath);
            return File.ReadAllBytes(path);
        }
    }
}
