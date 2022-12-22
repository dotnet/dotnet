// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Utilities;

/// <summary>
/// Test sources utility class
/// </summary>
internal class TestSourcesUtility
{
    /// <summary>
    /// Gets test sources from adapter source map
    /// </summary>
    /// <param name="adapterSourceMap"> The test list. </param>
    /// <returns> List of test Sources </returns>
    internal static IEnumerable<string>? GetSources(Dictionary<string, IEnumerable<string>?>? adapterSourceMap)
    {
        IEnumerable<string> sources = new List<string>();
        return adapterSourceMap?.Values?.Aggregate(sources, (current, enumerable) => enumerable is not null ? current.Concat(enumerable) : current);
    }

    /// <summary>
    /// Gets test sources from test case list
    /// </summary>
    /// <param name="tests"> The test list. </param>
    /// <returns> List of test Sources </returns>
    [return: NotNullIfNotNull("tests")]
    internal static IEnumerable<string>? GetSources(IEnumerable<TestCase>? tests)
    {
        return tests?.Select(tc => tc.Source).Distinct();
    }

    /// <summary>
    /// Gets default code base path for in-proc collector from test sources
    /// </summary>
    /// <param name="adapterSourceMap"> The test list. </param>
    /// <returns> List of test Sources </returns>
    internal static string? GetDefaultCodebasePath(Dictionary<string, IEnumerable<string>?> adapterSourceMap)
    {
        var source = GetSources(adapterSourceMap)?.FirstOrDefault();
        return source != null ? Path.GetDirectoryName(source) : null;
    }

    /// <summary>
    /// Gets default code base path for in-proc collector from test sources
    /// </summary>
    /// <param name="tests"> The test list. </param>
    /// <returns> List of test Sources </returns>
    internal static string? GetDefaultCodebasePath(IEnumerable<TestCase> tests)
    {
        var source = GetSources(tests)?.FirstOrDefault();
        return source != null ? Path.GetDirectoryName(source) : null;
    }
}
