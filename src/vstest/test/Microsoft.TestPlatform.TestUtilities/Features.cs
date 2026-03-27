// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// Table of features and the versions in which they were introduced, so we can test with them in compatibility data sources.
/// As the versions of the compatibility matrix get updated, the tests will eventually become irrelevant and can be deleted.
/// </summary>
public static class Features
{
    // History of features for reference, do not delete:
    // public const string ATTACH_DEBUGGER_FLOW = nameof(ATTACH_DEBUGGER_FLOW);
    // public const string MULTI_TFM = nameof(MULTI_TFM);
    public const string MSTEST_EXAMPLE_FEATURE = nameof(MSTEST_EXAMPLE_FEATURE);
    

    public static IImmutableDictionary<string, Feature> TestPlatformFeatures { get; } = new Dictionary<string, Feature>
    {
        // History of features for reference.
        // [ATTACH_DEBUGGER_FLOW] = new(version: "v16.7.0-preview-20200519-01", issue: "https://github.com/microsoft/vstest/pull/2325"),
        // [MULTI_TFM] = new(version: "v17.3.0", issue: "https://github.com/microsoft/vstest/pull/3412")
    }.ToImmutableDictionary();

    public static IImmutableDictionary<string, Feature> AdapterFeatures { get; internal set; } = new Dictionary<string, Feature>
    {
        [MSTEST_EXAMPLE_FEATURE] = new("2.2.8", issue: "This feature does not actually exist."),
    }.ToImmutableDictionary();
}
