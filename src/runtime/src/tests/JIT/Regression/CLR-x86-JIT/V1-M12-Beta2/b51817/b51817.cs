// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace QQ
{
    using System;

    public class AA
    {
        private static void Test(TypedReference arg, String result) { }
        public static int Main()
        {
            DateTime[] t = new DateTime[200];
            t[1] = new DateTime(100, 10, 1);
            Test(__makeref(t[1]), t[1].ToString());
            return 100;
        }
    }
}
