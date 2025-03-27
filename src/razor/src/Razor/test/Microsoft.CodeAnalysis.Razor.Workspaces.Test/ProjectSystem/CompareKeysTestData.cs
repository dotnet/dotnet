// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal static class CompareKeysTestData
{
    private static readonly ProjectKey s_forwardSlash1 = new("/path/to/project/obj");
    private static readonly ProjectKey s_backslash1 = new(@"\path\to\project\obj");
    private static readonly ProjectKey s_forwardSlash2 = new("/path/to/proj/obj");
    private static readonly ProjectKey s_backslash2 = new(@"\path\to\proj\obj");

    private const string DocumentFilePath1 = @"\path\to\project\file1.razor";
    private const string DocumentFilePath2 = @"\path\to\project\file2.razor";

    internal enum CompareResult { Equal, LessThan, GreaterThan }

    public static TheoryData<ProjectKey, ProjectKey, CompareResult> ProjectKeys =>
       new()
       {
           { ProjectKey.Unknown, ProjectKey.Unknown, CompareResult.Equal },
           { ProjectKey.Unknown, s_forwardSlash1, CompareResult.GreaterThan },
           { s_forwardSlash1, ProjectKey.Unknown, CompareResult.LessThan },
           { s_forwardSlash1, s_forwardSlash1, CompareResult.Equal },
           { s_forwardSlash1, s_backslash1, CompareResult.Equal },
           { s_backslash1, s_forwardSlash1, CompareResult.Equal },
           { s_forwardSlash2, s_forwardSlash1, CompareResult.LessThan },
           { s_forwardSlash2, s_backslash1, CompareResult.LessThan },
           { s_backslash2, s_forwardSlash1, CompareResult.LessThan },
           { s_forwardSlash1, s_forwardSlash2, CompareResult.GreaterThan },
           { s_forwardSlash1, s_backslash2, CompareResult.GreaterThan },
           { s_backslash1, s_forwardSlash2, CompareResult.GreaterThan }
       };

    public static TheoryData<DocumentKey, DocumentKey, CompareResult> DocumentKeys =>
       new()
       {
           { new(ProjectKey.Unknown, DocumentFilePath1), new(ProjectKey.Unknown, DocumentFilePath1), CompareResult.Equal },
           { new(ProjectKey.Unknown, DocumentFilePath1), new(s_forwardSlash1, DocumentFilePath1), CompareResult.GreaterThan },
           { new(s_forwardSlash1, DocumentFilePath1), new(ProjectKey.Unknown, DocumentFilePath1), CompareResult.LessThan },
           { new(s_forwardSlash1, DocumentFilePath1), new(s_forwardSlash1, DocumentFilePath1), CompareResult.Equal },
           { new(s_forwardSlash1, DocumentFilePath1), new(s_forwardSlash1, DocumentFilePath2), CompareResult.LessThan },
           { new(s_forwardSlash1, DocumentFilePath2), new(s_forwardSlash1, DocumentFilePath1), CompareResult.GreaterThan }
       };
}
