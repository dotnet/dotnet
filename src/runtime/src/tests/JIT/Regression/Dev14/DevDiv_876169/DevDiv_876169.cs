// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;

public class Repro
{
    public static int Main()
    {
        //We used to incorrectly generate an infinite loop by
        //emitting a jump instruction to itself
        //The correct behaviour would be to immediately exit the loop

        int i = 0;
        while (i < 0 || i < 1)
        {
            i++;
        }
        Console.WriteLine("PASS!");
        return 100;
    }
}
