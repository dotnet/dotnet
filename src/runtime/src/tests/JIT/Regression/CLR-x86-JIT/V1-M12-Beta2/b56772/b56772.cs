// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;

public class InternTest
{
    public static int Main()
    {
        StringBuilder sb = new StringBuilder().Append('A').Append('B').Append('C');

        switch (sb.ToString())
        {
            case "ABC":
                Console.WriteLine("Worked Correctly");
                return 100;
            default:
                Console.WriteLine("FAILED!");
                return 1;
        }
    }
}
