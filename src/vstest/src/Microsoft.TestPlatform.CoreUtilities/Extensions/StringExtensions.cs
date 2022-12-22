// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Add double quote around string. Useful in case of path which has white space in between.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string AddDoubleQuote(this string value)
    {
        return "\"" + value + "\"";
    }
}
