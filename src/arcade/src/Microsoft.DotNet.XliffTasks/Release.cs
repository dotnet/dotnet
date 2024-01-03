// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;

namespace XliffTasks
{
    internal static class Release
    {
        public static void Assert(bool condition)
        {
            Debug.Assert(condition);

            if (!condition)
            {
                throw new InvalidOperationException("Assertion failure.");
            }
        }
    }
}