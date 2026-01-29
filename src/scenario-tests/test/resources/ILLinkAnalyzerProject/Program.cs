// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;

internal sealed class Program
{
    public static void Main()
    {
        PrintMethods(typeof(string));
        Console.WriteLine("ILLink analyzer test completed!");
    }

    // This triggers ILLink trim analysis
    static void PrintMethods([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] Type type)
    {
        Console.WriteLine($"Type {type.Name} has {type.GetMethods().Length} public methods");
    }
}
