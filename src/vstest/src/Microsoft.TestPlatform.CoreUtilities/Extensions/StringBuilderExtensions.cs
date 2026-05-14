// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;

public static class StringBuilderExtensions
{
    /// <summary>
    /// Append given data from to string builder with new line.
    /// </summary>
    /// <param name="result">string builder</param>
    /// <param name="data">data to be appended.</param>
    /// <returns></returns>
    public static void AppendSafeWithNewLine(this StringBuilder result, string? data)
    {
        if (data.IsNullOrEmpty())
        {
            return;
        }

        // Don't append more data if already reached max length.
        if (result.Length >= result.MaxCapacity)
        {
            return;
        }

        // Add newline for readability.
        data += Environment.NewLine;

        // Append sub string of data if appending all the data exceeds max capacity.
        if (result.Length + data.Length >= result.MaxCapacity)
        {
            data = data.Substring(0, result.MaxCapacity - result.Length);
        }

        result.Append(data);
    }
}
