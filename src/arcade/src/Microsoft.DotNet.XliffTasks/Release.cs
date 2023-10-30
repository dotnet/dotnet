// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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