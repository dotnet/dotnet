// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal static class TestHostProject
{
    public static HostProject Create(string filePath)
        => Create(filePath, intermediateOutputPath: Path.Combine(Path.GetDirectoryName(filePath) ?? @"\\path", "obj"));

    public static HostProject Create(string filePath, string intermediateOutputPath)
        => new(filePath, intermediateOutputPath, RazorConfiguration.Default, rootNamespace: "TestRootNamespace");
}
