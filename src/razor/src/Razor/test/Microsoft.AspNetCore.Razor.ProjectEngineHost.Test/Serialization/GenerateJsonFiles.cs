// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

// Uncomment to easily generate new JSON files
//#define GENERATE_JSON_FILES

using System;
using System.Collections.Immutable;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

///////////////////////////////////////////////////////////////////////////////
//
// Note: The JSON files used for testing are very large. When making
// significant changes to the JSON format for tag helpers or RazorProjectInfo, it
// can be helpful to update the ObjectWriters first and the write new JSON files
// before updating the ObjectReaders. This avoids having to make a series of
// manual edits to the JSON resource files.
//
// 1. Update ObjectWriters to write the new JSON format.
// 2. Uncomment the GENERATE_JSON_FILES #define above.
// 3. Run the GenerateNewJsonFiles test below.
// 4. Update ObjectReaders to read the new JSON format.
// 5. Comment the GENERATE_JSON_FILES #define again.
// 6. Run all of the tests in SerializerValidationTest to ensure that the
//    new JSON files deserialize correctly.
//
///////////////////////////////////////////////////////////////////////////////

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Test.Serialization;

public class GenerateJsonFiles(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
#if GENERATE_JSON_FILES
    internal static readonly bool ShouldGenerate = true;
#else
    internal static readonly bool ShouldGenerate = false;
#endif

    // This is to prevent you from accidentally checking in with GenerateJsonFiles = true
    [Fact]
    public void GenerateJsonFilesMustBeFalse()
    {
        Assert.False(ShouldGenerate, "GenerateJsonFiles should be set back to false before you check in!");
    }

    // This updates shared JSON files
#if GENERATE_JSON_FILES
    [Theory]
#else
    [Theory(Skip = "Run with /p:GenerateJsonFiles=true or uncomment #define GENERATE_JSON_FILES to run this test.")]
#endif
    [MemberData(nameof(JsonFiles))]
    public void GenerateNewJsonFiles(JsonFile jsonFile)
    {
        var filePath = Path.Combine([GetSharedFilesRoot(), .. jsonFile.PathParts]);

        if (jsonFile.IsRazorProjectInfo)
        {
            var original = DeserializeProjectInfoFromFile(filePath);
            JsonDataConvert.SerializeToFile(original, filePath, indented: true);
        }
        else
        {
            var original = DeserializeTagHelperArrayFromFile(filePath);
            JsonDataConvert.SerializeToFile(original, filePath, indented: true);
        }
    }

    public readonly record struct JsonFile(string[] PathParts, bool IsRazorProjectInfo)
    {
        public static JsonFile TagHelpers(params string[] pathParts)
            => new(pathParts, IsRazorProjectInfo: false);

        public static JsonFile RazorProjectInfo(params string[] pathParts)
            => new(pathParts, IsRazorProjectInfo: true);
    }

    public static TheoryData<JsonFile> JsonFiles =>
        new()
        {
            JsonFile.TagHelpers("Compiler", "taghelpers.json"),
            JsonFile.TagHelpers("Tooling", "BlazorServerApp.TagHelpers.json"),
            JsonFile.TagHelpers("Tooling", "taghelpers.json"),
            JsonFile.TagHelpers("Tooling", "Telerik", "Kendo.Mvc.Examples.taghelpers.json"),
            JsonFile.RazorProjectInfo("Tooling", "project.razor.json"),
            JsonFile.RazorProjectInfo("Tooling", "Telerik", "Kendo.Mvc.Examples.project.razor.json")
        };

    private static RazorProjectInfo DeserializeProjectInfoFromFile(string filePath)
    {
        using var reader = new StreamReader(filePath);
        return JsonDataConvert.DeserializeProjectInfo(reader);
    }

    private static ImmutableArray<TagHelperDescriptor> DeserializeTagHelperArrayFromFile(string filePath)
    {
        using var reader = new StreamReader(filePath);
        return JsonDataConvert.DeserializeTagHelperArray(reader);
    }

    private static string GetSharedFilesRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null && !File.Exists(Path.Combine(current.FullName, "Razor.sln")))
        {
            current = current.Parent;
        }

        Assert.NotNull(current);

        return Path.Combine(current.FullName, "src", "Shared", "files");
    }
}
