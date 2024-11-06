// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Utilities;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Test.Common;

public static class PathUtilities
{
    public static string CreateRootedPath(params string[] parts)
    {
        var result = Path.Combine(parts);

        if (!Path.IsPathRooted(result))
        {
            result = PlatformInformation.IsWindows
                ? @"C:\" + result
                : "/" + result;
        }

        return result;
    }

    public static Uri GetUri(params string[] parts)
    {
        return new($"{Uri.UriSchemeFile}{Uri.SchemeDelimiter}{Path.Combine(parts)}");
    }

    public static void AssertEquivalent(string? expectedFilePath, string? actualFilePath)
    {
        Assert.True(FilePathNormalizer.AreFilePathsEquivalent(expectedFilePath, actualFilePath));
    }
}
