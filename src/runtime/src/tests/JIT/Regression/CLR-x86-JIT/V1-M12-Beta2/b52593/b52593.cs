// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Test
{
    using System;
    public class App
    {
        private static void Method1(TypedReference param1, object obj) { }
        public static int Main()
        {
            int[] an = { 0 };
            Method1(__makeref(an[0]), 1);
            return 100;
        }
    }
}
