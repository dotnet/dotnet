// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
