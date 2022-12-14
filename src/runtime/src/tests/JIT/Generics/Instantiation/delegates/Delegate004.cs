// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;

internal delegate T GenDelegate<T>(T p1, out T p2);

internal class Foo<T>
{
    virtual public T Function(T i, out T j)
    {
        j = i;
        return i;
    }
}

internal class Test_Delegate004
{
    public static int Main()
    {
        int i, j;
        Foo<int> inst = new Foo<int>();
        GenDelegate<int> MyDelegate = new GenDelegate<int>(inst.Function);
        i = MyDelegate(10, out j);

        if ((i != 10) || (j != 10))
        {
            Console.WriteLine("Failed Sync Invocation");
            return 1;
        }

        Console.WriteLine("Test Passes");
        return 100;
    }
}

