// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
public class A
{
    public static int Main()
    {
        Main1();
        return 100;
    }
    public static void Main1()
    {
        bool b = false;
        while (b)
            break;
        try
        {
            do
            {
                continue;
            } while (new object[] { }[0] != null);
        }
        catch (Exception) { }
    }
}
