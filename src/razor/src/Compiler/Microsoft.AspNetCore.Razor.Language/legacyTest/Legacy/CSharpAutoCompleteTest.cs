// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

public class CSharpAutoCompleteTest() : ParserTestBase(layer: TestProject.Layer.Compiler, validateSpanEditHandlers: true)
{
    [Fact]
    public void FunctionsDirectiveAutoCompleteAtEOF()
    {
        // Arrange, Act & Assert
        ParseDocumentTest("@functions{", new[] { FunctionsDirective.Directive });
    }

    [Fact]
    public void SectionDirectiveAutoCompleteAtEOF()
    {
        // Arrange, Act & Assert
        ParseDocumentTest("@section Header {", new[] { SectionDirective.Directive });
    }

    [Fact]
    public void VerbatimBlockAutoCompleteAtEOF()
    {
        ParseDocumentTest("@{");
    }

    [Fact]
    public void FunctionsDirectiveAutoCompleteAtStartOfFile()
    {
        // Arrange, Act & Assert
        ParseDocumentTest("""
            @functions{
            foo
            """, new[] { FunctionsDirective.Directive });
    }

    [Fact]
    public void SectionDirectiveAutoCompleteAtStartOfFile()
    {
        // Arrange, Act & Assert
        ParseDocumentTest("""
            @section Header {
            <p>Foo</p>
            """, new[] { SectionDirective.Directive });
    }

    [Fact]
    public void VerbatimBlockAutoCompleteAtStartOfFile()
    {
        ParseDocumentTest("""
            @{
            <p></p>
            """);
    }
}
