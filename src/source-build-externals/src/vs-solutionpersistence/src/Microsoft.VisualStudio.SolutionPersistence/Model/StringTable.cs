// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// Table of strings that can be used to remove duplicate string allocations.
/// This is helpful in the solution model as several types of strings are repeated.
/// </summary>
public sealed class StringTable
{
    // string deduplication facility (we can expect a lot of similar strings in solution files we want to compact while building the model).
    private readonly HashSet<string> strings = new HashSet<string>(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="StringTable"/> class.
    /// Creates an empty string table.
    /// </summary>
    public StringTable()
    {
    }

    /// <summary>
    /// Attempts to get a string from the table.
    /// If not found the string is added to the table.
    /// </summary>
    /// <param name="str">The string to search for.</param>
    /// <returns>The string to use in the model.</returns>
    public string GetString(StringSpan str)
    {
        if (str.IsEmpty)
        {
            return string.Empty;
        }

        // CONSIDER: We could use hashcodes to try to find strings without allocating.
        return this.GetString(str.ToString());
    }

    /// <summary>
    /// Attempts to get a string from the table.
    /// If not found the string is added to the table.
    /// </summary>
    /// <param name="str">The string to search for.</param>
    /// <returns>The string to use in the model.</returns>
    [return: NotNullIfNotNull(nameof(str))]
    public string? GetString(string? str)
    {
        if (str is null)
        {
            return null;
        }

        if (str.Length == 0)
        {
            return string.Empty;
        }

        if (this.strings.TryGetValue(str, out string? result))
        {
            return result;
        }

        _ = this.strings.Add(str);
        return str;
    }

    internal void AddString(string str)
    {
        _ = this.GetString(str);
    }
}
