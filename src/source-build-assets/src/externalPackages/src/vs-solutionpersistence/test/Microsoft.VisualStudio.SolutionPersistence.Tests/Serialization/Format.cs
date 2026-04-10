// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// Tests related to formatting the solution file.
/// </summary>
public class Format
{
    /// <summary>
    /// Test that SLN converts to UTF-8 automatically from ASCII.
    /// </summary>
    [Fact]
    public async Task ConvertASCIItoUTF8Async()
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        const string asciiFolderName = "directory123";
        const string utf8FolderName = "répertoire123";

        SolutionModel solution = new SolutionModel();
        _ = solution.AddFolder($"/{asciiFolderName}/");

        solution.SerializerExtension = SolutionSerializers.SlnFileV12.CreateModelExtension();

        int codePageASCII = Encoding.ASCII.CodePage;
        int codePageUTF8 = Encoding.UTF8.CodePage;

        (SolutionModel asciiModel, FileContents asciiLines) = await SlnTestHelper.SaveAndReopenModelAsync(SolutionSerializers.SlnFileV12, solution, 1024 * 100);
        Assert.Equal(codePageASCII, GetSlnEncoding(asciiModel).CodePage);
        Assert.Contains(asciiFolderName, asciiLines.FullString);

        // Add a folder that requires encoding.
        _ = solution.AddFolder($"/{utf8FolderName}/");

        (SolutionModel utf8Model, FileContents utf8Lines) = await SlnTestHelper.SaveAndReopenModelAsync(SolutionSerializers.SlnFileV12, solution, 1024 * 100);
        Assert.Equal(codePageUTF8, GetSlnEncoding(utf8Model).CodePage);
        Assert.Contains(asciiFolderName, utf8Lines.FullString);
        Assert.Contains(utf8FolderName, utf8Lines.FullString);
    }
}
