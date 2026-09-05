// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.TestPlatform.Hashing;

/// <summary>
/// The algorithms available for computing a test case id.
/// </summary>
internal enum TestCaseIdAlgorithm
{
    /// <summary>
    /// The SHA1 based ids, unversioned. These are the ids vstest computes unless a run opts in to
    /// xxHash128, and every id it has produced by default historically.
    /// </summary>
    Sha1,

    /// <summary>
    /// xxHash128 based ids, carrying the hashing scheme version in the produced GUID.
    /// </summary>
    XxHash128,
}

/// <summary>
/// Resolves which algorithm computes a test case id, from the
/// <see cref="FeatureFlag.VSTEST_DISABLE_XXHASH128_TESTCASE_ID"/> feature flag.
/// </summary>
/// <remarks>
/// <para>
/// Shared source, compiled into <c>Microsoft.TestPlatform.ObjectModel</c>, where <c>TestCase</c>
/// computes its own id. <c>Microsoft.TestPlatform.CrossPlatEngine</c>, where the
/// Microsoft.Testing.Platform path has to compute the id in the runner process, uses that same
/// definition through <c>InternalsVisibleTo</c>, so both resolve the same value from the same input.
/// </para>
/// <para>
/// The flag is an opt-out from xxHash128 rather than a selector naming an algorithm, following the
/// convention every other <c>VSTEST_DISABLE_*</c> flag in this repo follows. Naming the algorithm
/// being disabled rather than "the new one" is what makes a value survive the release that flips the
/// default: <c>1</c> selects SHA1 and <c>0</c> selects xxHash128 both before and after, and the flip
/// itself is the deletion of one entry from <c>FeatureFlag.DefaultValues</c>.
/// </para>
/// </remarks>
internal static class TestCaseIdAlgorithmResolver
{
    /// <summary>
    /// The feature flag that disables the xxHash128 ids.
    /// </summary>
    /// <remarks>
    /// Read by whichever process builds the test case: the testhost on the classic path, the runner
    /// on the Microsoft.Testing.Platform path. It can be set in the environment directly, or declared
    /// in runsettings under <c>RunConfiguration/EnvironmentVariables</c>.
    /// </remarks>
    public const string FeatureFlagName = FeatureFlag.VSTEST_DISABLE_XXHASH128_TESTCASE_ID;

    /// <summary>
    /// The value of <see cref="FeatureFlagName"/> that opts in to the xxHash128 ids.
    /// </summary>
    /// <remarks>
    /// <see cref="FeatureFlag"/> treats every other value as setting the flag, so there is no
    /// constant for the opposite choice: anything that is not <c>0</c> disables xxHash128.
    /// </remarks>
    public const string OptInValue = "0";

    /// <summary>
    /// The algorithm this process computes ids with, from its own environment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="FeatureFlag"/> reads the environment on first use rather than at type load, and
    /// caches the result, which is exactly what an id needs: reading it lazily makes the choice
    /// observable from a test instead of depending on when the type happened to be touched, and
    /// caching it keeps an id stable for the lifetime of the process even if the environment changes
    /// underneath. Threads racing on first use all receive the value that won the cache insertion,
    /// so no process can hand out two different ids for the same test.
    /// </para>
    /// <para>
    /// Which algorithm this is by default is stated in one place only,
    /// <c>FeatureFlag.DefaultValues</c>. Changing it there will fail the pinned expectations that
    /// record what ids actually are - the ids in TestCaseTests, the serialized payloads in
    /// TestResultSerializationTests, and the test that asserts the default directly. That is the
    /// intended blast radius rather than a bug: those are exactly the places whose whole job is to
    /// notice that ids moved.
    /// </para>
    /// <para>
    /// Only <c>TestCase</c> ever reads this. The Microsoft.Testing.Platform path hands the converter
    /// a <see langword="null"/> algorithm when a run declares nothing, which leaves the id to
    /// <c>TestCase</c> rather than resolving it a second time - so the runner cannot disagree with
    /// the testhost about the default, because it never reads the default itself.
    /// </para>
    /// </remarks>
    public static TestCaseIdAlgorithm Ambient
        => FeatureFlag.Instance.IsSet(FeatureFlagName)
            ? TestCaseIdAlgorithm.Sha1
            : TestCaseIdAlgorithm.XxHash128;

    /// <summary>
    /// Resolves the algorithm a declared value of the flag selects.
    /// </summary>
    /// <remarks>
    /// Applies the rule <see cref="FeatureFlag"/> applies to a value it finds in the environment: the
    /// value is trimmed, and anything other than <see cref="OptInValue"/> counts as setting the flag.
    /// There is deliberately no notion of an unrecognized value - for a boolean flag there is none,
    /// and inventing one here would make a runsettings declaration mean something different from the
    /// same text in the environment.
    /// </remarks>
    public static TestCaseIdAlgorithm Resolve(string declaredValue)
        => declaredValue.Trim() != OptInValue
            ? TestCaseIdAlgorithm.Sha1
            : TestCaseIdAlgorithm.XxHash128;

    /// <summary>
    /// Resolves the algorithm declared by a set of environment variables, typically the ones a run
    /// declares in runsettings <c>RunConfiguration/EnvironmentVariables</c>.
    /// </summary>
    /// <returns>
    /// <see langword="null"/> when the run does not declare the flag, so the caller can fall back to
    /// <see cref="Ambient"/> - the environment of whichever process ends up computing the id, which
    /// is where the default lives. A declared value wins over an inherited one, so a run that says
    /// something explicit about ids is never silently overridden.
    /// </returns>
    /// <remarks>
    /// <para>
    /// One input cannot be resolved the same way this value would be resolved on the classic path,
    /// because the two operating systems do not agree with each other there. A value that is empty
    /// reads as "not declared": Windows deletes an environment variable set to the empty string, so
    /// on the classic path there such a declaration falls back to the default; on non-Windows it
    /// survives and reads as setting the flag. Falling back is the better of the two to reproduce: an
    /// empty value is what a run gets by accident, from an unset property or an empty element in
    /// runsettings, and taking that as an explicit opt-out that overrides the ambient environment
    /// would be a strange thing to infer from an accident. Whitespace is deliberately not included -
    /// it survives on both operating systems, so it can and does mirror them exactly.
    /// </para>
    /// <para>
    /// The name is matched case-insensitively even though environment variables are case-sensitive on
    /// non-Windows, so a lowercase key in runsettings is honoured here and would be ignored by a
    /// testhost on Linux. Matching case-sensitively would instead make the same runsettings behave
    /// differently on Windows and Linux, which is the worse failure of the two.
    /// </para>
    /// <para>
    /// Both asymmetries are reachable today, not only after the default moves, but they need a run to
    /// supply the pathological input *and* an ambient value that disagrees with it. On non-Windows a
    /// lowercase-keyed declaration is honoured here and ignored by a testhost; an empty declaration
    /// falls back to the ambient value here and reads as setting the flag in a testhost. So an MTP run
    /// and a classic run given the same runsettings can compute different ids. Neither alternative
    /// removes this - the operating systems disagree about empty variables, and matching the key
    /// case-sensitively would make the same runsettings behave differently on Windows and Linux - so
    /// what is chosen here is the least surprising of the available answers rather than a correct one.
    /// </para>
    /// </remarks>
    public static TestCaseIdAlgorithm? ResolveDeclared(IDictionary<string, string?>? environmentVariables)
    {
        if (environmentVariables is null)
        {
            return null;
        }

        foreach (KeyValuePair<string, string?> variable in environmentVariables)
        {
            // The dictionary is not necessarily keyed case-insensitively, and on non-Windows the
            // classic path keys it case-sensitively on purpose, so match the name rather than index.
            if (string.Equals(variable.Key, FeatureFlagName, StringComparison.OrdinalIgnoreCase))
            {
                // A null value is what an unset variable reads as, and FeatureFlag only consults its
                // defaults when the variable reads as null, so treat it - and an empty value, for the
                // reason above - as not declared rather than as a declaration. Matched as a pattern
                // rather than with string.IsNullOrEmpty, which is not annotated on every target
                // framework this file is compiled for and so does not narrow the value to non-null.
                return variable.Value is not (null or "") ? Resolve(variable.Value) : null;
            }
        }

        return null;
    }
}
