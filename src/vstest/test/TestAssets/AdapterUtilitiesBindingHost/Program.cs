// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.TestPlatform.AdapterUtilities;

namespace AdapterUtilitiesBindingHost;

internal static class Program
{
    private static int Main()
    {
        var adapterUtilitiesAsm = typeof(TestIdProviderXxHash128).Assembly;
        Console.WriteLine($"AdapterUtilities.dll path:    {adapterUtilitiesAsm.Location}");
        Console.WriteLine($"AdapterUtilities.dll version: {adapterUtilitiesAsm.GetName().Version}");
        foreach (var r in adapterUtilitiesAsm.GetReferencedAssemblies())
        {
            if (r.Name == "System.Memory" || r.Name == "System.Runtime.CompilerServices.Unsafe" || r.Name == "System.Buffers")
            {
                Console.WriteLine($"  AdapterUtilities.dll references {r.Name}, Version={r.Version}");
            }
        }

        Guid id;
        try
        {
            // TestIdProviderXxHash128 hashes with the vendored xxHash128, which uses Span<T> and
            // BinaryPrimitives and therefore forces the CLR to resolve System.Memory at the version
            // baked into AdapterUtilities.dll's metadata. Unlike the SHA1 TestIdProvider, this is
            // the only provider that touches System.Memory, so it has to be the one exercised here.
            var provider = new TestIdProviderXxHash128();
            provider.AppendString("executor://adapter-utilities-binding-host");
            provider.AppendString("SomeTests.dll");
            provider.AppendString("SomeNamespace.SomeClass.SomeTest");
            id = provider.GetId();
            Console.WriteLine($"TestIdProviderXxHash128.GetId(): {id}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("REPRO HIT: exception computing an id with TestIdProviderXxHash128:");
            Console.Error.WriteLine(ex);
            return 1;
        }

        if (!VerifyXxHashId(id) || !VerifySystemMemoryLoaded())
        {
            return 1;
        }

        Console.WriteLine("OK - xxhash128 test id computed, System.Memory bound.");
        return 0;
    }

    /// <summary>
    /// Verifies the id was actually produced by the xxHash128 scheme.
    /// </summary>
    /// <remarks>
    /// <para>
    /// xxHash128 ids are RFC 9562 version 8 UUIDs with the hashing scheme version stamped into the
    /// top nibble, so the string form always starts with the scheme version ('1') and its third
    /// group always starts with the UUID version ('8'). Checking the shape keeps this host honest:
    /// an id that does not look like that was not produced by the code path this host exists to
    /// exercise.
    /// </para>
    /// <para>
    /// The '1' is TestIdGuid.CurrentHashVersion, which cannot be referenced from here because it is
    /// internal to the product. If that constant is ever bumped, this check has to be updated in
    /// step, otherwise it fails for a legitimate change rather than a real binding problem.
    /// </para>
    /// </remarks>
    private static bool VerifyXxHashId(Guid id)
    {
        if (id == Guid.Empty)
        {
            Console.Error.WriteLine("REPRO HIT: TestIdProviderXxHash128 produced an empty guid.");
            return false;
        }

        var text = id.ToString("D");
        var groups = text.Split('-');
        if (text[0] != '1' || groups[2][0] != '8')
        {
            Console.Error.WriteLine(
                $"REPRO HIT: TestIdProviderXxHash128 produced '{text}', which is not an xxHash128 id (expected a " +
                "leading '1' hash version nibble and a leading '8' UUID version nibble). The xxHash128 " +
                "code path did not run, so this host proved nothing about System.Memory.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Verifies <c>System.Memory</c> really made it into the AppDomain.
    /// </summary>
    /// <remarks>
    /// This is the assertion that turns "nothing threw" into evidence. A binding break is
    /// JIT-triggered, so a host that never reaches the Span-using code exits successfully while
    /// leaving the dependency completely unexercised. Requiring the assembly to be loaded makes
    /// that outcome a failure instead of a silent pass.
    /// </remarks>
    private static bool VerifySystemMemoryLoaded()
    {
        var systemMemory = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "System.Memory");

        if (systemMemory is null)
        {
            Console.Error.WriteLine(
                "REPRO HIT: System.Memory was never loaded into the AppDomain, even though an xxHash128 " +
                "id was computed. Either the id was not really computed with xxHash128, or the runtime " +
                "satisfied Span<T> from somewhere else - either way this host is no longer guarding the " +
                "dependency it exists to guard.");
            return false;
        }

        Console.WriteLine($"System.Memory loaded:         {systemMemory.GetName().Version}");
        Console.WriteLine($"System.Memory path:           {systemMemory.Location}");
        return true;
    }
}
