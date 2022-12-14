// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
public class a
{
    static int temp = 0;

    public static void MiddleMethod()
    {
        temp += 1;
        try
        {
            temp *= 2;
            throw new System.ArgumentException();
        }
        finally
        {
            temp += 5;
            Console.WriteLine("In Finally");
        }
        temp *= 3;
        Console.WriteLine("Done...");
    }

    public static int Main()
    {
        Console.WriteLine("Starting....");

        try
        {
            MiddleMethod();
        }
        catch
        {
            if (temp == 7)
                Console.WriteLine("PASS");
            return 100;
        }
        Console.WriteLine("Failed - temp = " + temp);
        return 0;
    }
}
