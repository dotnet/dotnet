// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
using System.Threading;

public class My
{

    static void Worker()
    {
        GC.Collect();
        Thread.Sleep(5);
    }

    public static int Main()
    {

        Thread t = new Thread(new ThreadStart(Worker));
        t.Start();

        long x = 1;
        for (long i = 0; i < 100000; i++)
        {
            x *= i;
        }
        Console.WriteLine((object)x);

        return 100;
    }

}
