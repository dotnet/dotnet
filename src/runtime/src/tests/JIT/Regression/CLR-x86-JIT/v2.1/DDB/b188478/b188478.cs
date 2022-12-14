// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class My
{
    public static int Main()
    {
        My[] s = new My[0];
        IList<My> ls = (IList<My>)s;
        ReadOnlyCollection<My> roc = new ReadOnlyCollection<My>(ls);
        Console.WriteLine(roc.Count);
        return 100;
    }
}
