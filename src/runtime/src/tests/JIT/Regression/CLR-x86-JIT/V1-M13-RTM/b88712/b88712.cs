// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;

public struct AA
{
    public static void Static5()
    {
        float a = 125.0f;
        a += (a *= 60.0f);
    }
    public static int Main()
    {
        Static5();
        return 100;
    }
}
