// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace DtaLikeHost;

internal static class Program
{
    private static int Main()
    {
        // Opts in to the xxHash128 test id algorithm. This has to happen before anything touches
        // TestCase.Id: the feature flag is read on first use and cached for the lifetime of the
        // process. It also has to happen at all - with the SHA1 default the Span-using xxHash code
        // is never JIT-compiled, so a System.Memory binding break would go completely unnoticed by
        // this host.
        Environment.SetEnvironmentVariable("VSTEST_DISABLE_XXHASH128_TESTCASE_ID", "0");

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

        if (!RunTestCaseIdScenario())
        {
            return 1;
        }

        Console.WriteLine("OK - no binding exception.");
        return 0;
    }

    /// <summary>
    /// Computes a test case id with the xxHash128 algorithm, which is where ObjectModel needs
    /// <c>System.Memory</c> - the vendored xxHash128 implementation uses <c>Span&lt;T&gt;</c> and
    /// <c>BinaryPrimitives</c>, neither of which .NET Framework ships inbox.
    /// </summary>
    /// <returns><see langword="true"/> when the id was computed and System.Memory really loaded.</returns>
    private static bool RunTestCaseIdScenario()
    {
        var objectModelAsm = typeof(TestCase).Assembly;
        Console.WriteLine($"ObjectModel.dll path:    {objectModelAsm.Location}");
        Console.WriteLine($"ObjectModel.dll version: {objectModelAsm.GetName().Version}");
        foreach (var r in objectModelAsm.GetReferencedAssemblies())
        {
            if (r.Name == "System.Memory" || r.Name == "System.Runtime.CompilerServices.Unsafe" || r.Name == "System.Buffers")
            {
                Console.WriteLine($"  ObjectModel.dll references {r.Name}, Version={r.Version}");
            }
        }

        Guid id;
        Guid sha1Id;
        Guid xxHashId;
        try
        {
            // TestCase.Id resolves the algorithm from VSTEST_DISABLE_XXHASH128_TESTCASE_ID and, when
            // xxHash128 is opted in to, goes through EqtHash.GuidFromStringXxHash128 -> XxHash128 ->
            // Span<T>, which forces the CLR to resolve System.Memory at the version baked into
            // ObjectModel.dll's metadata.
            var testCase = new TestCase("SomeNamespace.SomeClass.SomeTest", new Uri("executor://dta-like-host"), "SomeTests.dll");
            id = testCase.Id;
            Console.WriteLine($"TestCase.Id (xxhash128 opted in): {id}");

            // Exercise both hashes directly as well, so the two ids can be compared in-process.
            const string seed = "dta-like-host";
            sha1Id = EqtHash.GuidFromString(seed);
            xxHashId = EqtHash.GuidFromStringXxHash128(seed);
            Console.WriteLine($"EqtHash.GuidFromString('{seed}'):  {sha1Id}");
            Console.WriteLine($"EqtHash.GuidFromStringXxHash128('{seed}'): {xxHashId}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("REPRO HIT: exception computing a test case id with xxhash128:");
            Console.Error.WriteLine(ex);
            return false;
        }

        return VerifyXxHashId(id, "TestCase.Id")
            && VerifyXxHashId(xxHashId, "EqtHash.GuidFromStringXxHash128")
            && VerifyIdsDiffer(sha1Id, xxHashId)
            && VerifySystemMemoryLoaded();
    }

    /// <summary>
    /// Verifies an id was actually produced by the xxHash128 scheme rather than by SHA1.
    /// </summary>
    /// <remarks>
    /// <para>
    /// xxHash128 ids are RFC 9562 version 8 UUIDs with the hashing scheme version stamped into the
    /// top nibble, so the string form always starts with the scheme version ('1') and its third
    /// group always starts with the UUID version ('8'). Checking the shape is what keeps this host
    /// honest: if the algorithm selection ever stopped taking effect, the Span-using code would not
    /// run and the whole scenario would pass without proving anything.
    /// </para>
    /// <para>
    /// The '1' is TestIdGuid.CurrentHashVersion, which cannot be referenced from here because it is
    /// internal to the product. If that constant is ever bumped, this check has to be updated in
    /// step, otherwise it fails for a legitimate change rather than a real binding problem.
    /// </para>
    /// </remarks>
    private static bool VerifyXxHashId(Guid id, string origin)
    {
        if (id == Guid.Empty)
        {
            Console.Error.WriteLine($"REPRO HIT: {origin} produced an empty guid.");
            return false;
        }

        var text = id.ToString("D");
        var groups = text.Split('-');
        if (text[0] != '1' || groups[2][0] != '8')
        {
            Console.Error.WriteLine(
                $"REPRO HIT: {origin} produced '{text}', which is not an xxHash128 id (expected a leading " +
                "'1' hash version nibble and a leading '8' UUID version nibble). The xxHash128 code path " +
                "did not run, so this host proved nothing about System.Memory.");
            return false;
        }

        return true;
    }

    private static bool VerifyIdsDiffer(Guid sha1Id, Guid xxHashId)
    {
        if (sha1Id == xxHashId)
        {
            Console.Error.WriteLine("REPRO HIT: the SHA1 and xxHash128 ids for the same input are identical.");
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

        Console.WriteLine($"System.Memory loaded:    {systemMemory.GetName().Version}");
        Console.WriteLine($"System.Memory path:      {systemMemory.Location}");
        Console.WriteLine("OK - xxhash128 test id computed, System.Memory bound.");
        return true;
    }
}
