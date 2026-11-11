// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;

namespace DtaLikeHost;

internal static class Program
{
    private static int Main()
    {
        // Report what Common.dll expects and what we ship next to it, so the mismatch
        // (or agreement) is visible in the console output regardless of whether the
        // CLR actually fails to bind.
        var commonAsm = typeof(FilterExpressionWrapper).Assembly;
        Console.WriteLine($"Common.dll path:     {commonAsm.Location}");
        Console.WriteLine($"Common.dll version:  {commonAsm.GetName().Version}");
        foreach (var r in commonAsm.GetReferencedAssemblies())
        {
            if (r.Name == "System.Collections.Immutable" || r.Name == "System.Reflection.Metadata")
            {
                Console.WriteLine($"  Common.dll references {r.Name}, Version={r.Version}");
            }
        }

        try
        {
            // A simple equality filter produces a FastFilter, which triggers
            // FastFilter.Builder.ctor -> ImmutableDictionary.CreateBuilder(...)
            // -> forces the CLR to resolve System.Collections.Immutable at the version
            //    baked into Common.dll's metadata.
            var wrapper = new FilterExpressionWrapper("TestCategory=Foo");
            Console.WriteLine($"FilterExpressionWrapper constructed: FilterString='{wrapper.FilterString}', ParseError='{wrapper.ParseError}'");

            // Reflect on the private FastFilter field to prove it was actually built.
            var fastFilterField = typeof(FilterExpressionWrapper).GetField("FastFilter", BindingFlags.Instance | BindingFlags.NonPublic);
            var fastFilter = fastFilterField?.GetValue(wrapper);
            Console.WriteLine($"FastFilter built:    {fastFilter is not null}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("REPRO HIT: exception constructing FilterExpressionWrapper:");
            Console.Error.WriteLine(ex);
            return 1;
        }

        Console.WriteLine("OK - no binding exception.");
        return 0;
    }
}
