// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;

class BindHandleNull
{
    public static int Main()
    {
        return (new BindHandleNull().RunTest());
    }

    int RunTest()
    {
        try
        {
            ThreadPool.BindHandle(null);
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("Test passed");
            return (100);
        }
        catch(Exception e)
        {
            Console.WriteLine("Unexpected: {0}", e);
            return (98);
        }
        return (97);
    }
}
