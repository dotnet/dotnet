// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.IO;
using System.Text;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language;

public class StreamSourceDocumentTest
{
    [Fact]
    public void FilePath()
    {
        // Arrange
        var content = "Hello World!";
        var stream = CreateBOMStream(content, Encoding.UTF8);

        // Act
        var document = new StreamSourceDocument(stream, Encoding.UTF8, new RazorSourceDocumentProperties(filePath: "file.cshtml", relativePath: null));

        // Assert
        Assert.Equal("file.cshtml", document.FilePath);
    }

    [Fact]
    public void FilePath_Null()
    {
        // Arrange
        var content = "Hello World!";
        var stream = CreateBOMStream(content, Encoding.UTF8);

        // Act
        var document = new StreamSourceDocument(stream, Encoding.UTF8, new RazorSourceDocumentProperties(filePath: null, relativePath: null));

        // Assert
        Assert.Null(document.FilePath);
    }

    [Fact]
    public void RelativePath()
    {
        // Arrange
        var content = "Hello World!";
        var stream = CreateBOMStream(content, Encoding.UTF8);

        // Act
        var document = new StreamSourceDocument(stream, Encoding.UTF8, new RazorSourceDocumentProperties(filePath: null, relativePath: "file.cshtml"));

        // Assert
        Assert.Equal("file.cshtml", document.RelativePath);
    }

    [Fact]
    public void RelativePath_Null()
    {
        // Arrange
        var content = "Hello World!";
        var stream = CreateBOMStream(content, Encoding.UTF8);

        // Act
        var document = new StreamSourceDocument(stream, Encoding.UTF8, new RazorSourceDocumentProperties(filePath: null, relativePath: null));

        // Assert
        Assert.Null(document.RelativePath);
    }

    [Fact]
    public void GetChecksum_ReturnsCopiedChecksum()
    {
        // Arrange
        var content = "Hello World!";
        var stream = CreateBOMStream(content, Encoding.UTF8);
        var document = new StreamSourceDocument(stream, null, RazorSourceDocumentProperties.Default);

        // Act
        var firstChecksum = document.GetChecksum();
        var secondChecksum = document.GetChecksum();

        // Assert
        Assert.Equal(firstChecksum, secondChecksum);
        Assert.NotSame(firstChecksum, secondChecksum);
    }

    [Fact]
    public void GetChecksum_ComputesCorrectChecksum_UTF8()
    {
        // Arrange
        var content = "Hello World!";
        var stream = CreateBOMStream(content, Encoding.UTF8);
        var document = new StreamSourceDocument(stream, Encoding.UTF8, RazorSourceDocumentProperties.Default);
        var expectedChecksum = new byte[] { 59, 11, 228, 16, 192, 16, 42, 217, 137, 254, 79, 100, 97, 12, 34, 40, 242, 244, 47, 6, 192, 129, 57, 81, 112, 55, 144, 13, 41, 6, 235, 157 };

        // Act
        var checksum = document.GetChecksum();

        // Assert
        Assert.Equal(expectedChecksum, checksum);
    }

    [Fact]
    public void GetChecksum_ComputesCorrectChecksum_UTF32AutoDetect()
    {
        // Arrange
        var content = "Hello World!";
        var stream = CreateBOMStream(content, Encoding.UTF32);
        var document = new StreamSourceDocument(stream, null, RazorSourceDocumentProperties.Default);
        var expectedChecksum = new byte[] { 11, 12, 62, 196, 4, 182, 157, 136, 59, 24, 52, 47, 255, 37, 213, 148, 116, 228, 122, 200, 250, 197, 38, 178, 101, 98, 75, 216, 148, 134, 190, 58 };

        // Act
        var checksum = document.GetChecksum();

        // Assert
        Assert.Equal(expectedChecksum, checksum);
    }

    [Fact]
    public void ConstructedWithoutEncoding_DetectsEncoding()
    {
        // Arrange
        var content = TestRazorSourceDocument.CreateStreamContent(encoding: Encoding.UTF32);

        // Act
        var document = new StreamSourceDocument(content, null, RazorSourceDocumentProperties.Default);

        // Assert
        Assert.IsType<StreamSourceDocument>(document);
        Assert.Equal(Encoding.UTF32, document.Encoding);
    }

    [Fact]
    public void ConstructedWithoutEncoding_EmptyStream_DetectsEncoding()
    {
        // Arrange
        var content = TestRazorSourceDocument.CreateStreamContent(content: string.Empty, encoding: Encoding.UTF32);

        // Act
        var document = new StreamSourceDocument(content, null, RazorSourceDocumentProperties.Default);

        // Assert
        Assert.IsType<StreamSourceDocument>(document);
        Assert.Equal(Encoding.UTF32, document.Encoding);
    }

    [Fact]
    public void FailsOnMismatchedEncoding()
    {
        // Arrange
        var content = TestRazorSourceDocument.CreateStreamContent(encoding: Encoding.UTF32);
        var expectedMessage = Resources.FormatMismatchedContentEncoding(Encoding.UTF8.EncodingName, Encoding.UTF32.EncodingName);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => new StreamSourceDocument(content, Encoding.UTF8, RazorSourceDocumentProperties.Default));
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Theory]
    [InlineData(100000)]
    [InlineData(RazorSourceDocument.LargeObjectHeapLimitInChars)]
    [InlineData(RazorSourceDocument.LargeObjectHeapLimitInChars + 2)]
    [InlineData(RazorSourceDocument.LargeObjectHeapLimitInChars * 2 - 1)]
    [InlineData(RazorSourceDocument.LargeObjectHeapLimitInChars * 2)]
    public void DetectsSizeOfStreamForLargeContent(int contentLength)
    {
        // Arrange
        var content = new string('a', contentLength);
        var stream = TestRazorSourceDocument.CreateStreamContent(content);

        // Act
        var document = new StreamSourceDocument(stream, null, RazorSourceDocumentProperties.Default);

        // Assert
        var streamDocument = Assert.IsType<StreamSourceDocument>(document);
        Assert.IsType<LargeTextSourceDocument>(streamDocument._innerSourceDocument);
        Assert.Same(Encoding.UTF8, document.Encoding);
        Assert.Equal(content, ReadContent(document));
    }

    private static MemoryStream CreateBOMStream(string content, Encoding encoding)
    {
        var preamble = encoding.GetPreamble();
        var contentBytes = encoding.GetBytes(content);
        var buffer = new byte[preamble.Length + contentBytes.Length];
        preamble.CopyTo(buffer, 0);
        contentBytes.CopyTo(buffer, preamble.Length);
        var stream = new MemoryStream(buffer);
        return stream;
    }

    private static string ReadContent(RazorSourceDocument razorSourceDocument)
    {
        var buffer = new char[razorSourceDocument.Length];
        razorSourceDocument.CopyTo(0, buffer, 0, buffer.Length);

        return new string(buffer);
    }
}
