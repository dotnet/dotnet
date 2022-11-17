// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;


public class Class1
{
    [DllImport("fpcw.dll")]
    private static extern int RaiseFPException();

    public static int Main()
    {
        int retVal = RaiseFPException();

        return ( retVal==100 ) ? 100 : 101;
    }
}
