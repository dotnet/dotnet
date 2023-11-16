// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.LinkedEditingRange;

public class LinkedEditingRangeEndpointTest : TagHelperServiceTestBase
{
    public LinkedEditingRangeEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task Handle_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var uri = new Uri("file://path/test.razor");
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Position = new Position { Line = 1, Character = 3 } // <te[||]st1></test1>
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_TagHelperStartTag_ReturnsCorrectRange()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1></test1>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Position = new Position { Line = 1, Character = 3 } // <te[||]st1></test1>
        };

        var expectedRanges = new Range[]
        {
            new Range
            {
                Start = new Position { Line = 1, Character = 1 },
                End = new Position { Line = 1, Character = 6 }
            },
            new Range
            {
                Start = new Position { Line = 1, Character = 9 },
                End = new Position { Line = 1, Character = 14 }
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeEndpoint.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_TagHelperStartTag_ReturnsCorrectRange_EndSpan()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1></test1>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Position = new Position { Line = 1, Character = 6 } // <test1[||]></test1>
        };

        var expectedRanges = new Range[]
        {
            new Range
            {
                Start = new Position { Line = 1, Character = 1 },
                End = new Position { Line = 1, Character = 6 }
            },
            new Range
            {
                Start = new Position { Line = 1, Character = 9 },
                End = new Position { Line = 1, Character = 14 }
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeEndpoint.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_TagHelperEndTag_ReturnsCorrectRange()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1></test1>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Position = new Position { Line = 1, Character = 9 } // <test1></[||]test1>
        };

        var expectedRanges = new Range[]
        {
            new Range
            {
                Start = new Position { Line = 1, Character = 1 },
                End = new Position { Line = 1, Character = 6 }
            },
            new Range
            {
                Start = new Position { Line = 1, Character = 9 },
                End = new Position { Line = 1, Character = 14 }
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeEndpoint.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_NoTag_ReturnsNull()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1></test1>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Position = new Position { Line = 0, Character = 1 } // @[||]addTagHelper *
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_SelfClosingTagHelper_ReturnsNull()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1 />
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Position = new Position { Line = 1, Character = 3 } // <te[||]st1 />
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_NestedTagHelperStartTags_ReturnsCorrectRange()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <test1><test1></test1></test1>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Position = new Position { Line = 1, Character = 1 } // <[||]test1><test1></test1></test1>
        };

        var expectedRanges = new Range[]
        {
            new Range
            {
                Start = new Position { Line = 1, Character = 1 },
                End = new Position { Line = 1, Character = 6 }
            },
            new Range
            {
                Start = new Position { Line = 1, Character = 24 },
                End = new Position { Line = 1, Character = 29 }
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeEndpoint.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_HTMLStartTag_ReturnsCorrectRange()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <body></body>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Position = new Position { Line = 1, Character = 3 } // <bo[||]dy></body>
        };

        var expectedRanges = new Range[]
        {
            new Range
            {
                Start = new Position { Line = 1, Character = 1 },
                End = new Position { Line = 1, Character = 5 }
            },
            new Range
            {
                Start = new Position { Line = 1, Character = 8 },
                End = new Position { Line = 1, Character = 12 }
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeEndpoint.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_HTMLEndTag_ReturnsCorrectRange()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <body></body>
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Position = new Position { Line = 1, Character = 8 } // <body></[||]body>
        };

        var expectedRanges = new Range[]
        {
            new Range
            {
                Start = new Position { Line = 1, Character = 1 },
                End = new Position { Line = 1, Character = 5 }
            },
            new Range
            {
                Start = new Position { Line = 1, Character = 8 },
                End = new Position { Line = 1, Character = 12 }
            }
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Equal(expectedRanges, result.Ranges);
        Assert.Equal(LinkedEditingRangeEndpoint.WordPattern, result.WordPattern);
    }

    [Fact]
    public async Task Handle_SelfClosingHTMLTag_ReturnsNull()
    {
        // Arrange
        var txt = """
            @addTagHelper *, TestAssembly
            <body />
            """;
        var codeDocument = CreateCodeDocument(txt, isRazorFile: false, DefaultTagHelpers);
        var uri = new Uri("file://path/test.razor");
        var documentContext = CreateDocumentContext(uri, codeDocument);
        var endpoint = new LinkedEditingRangeEndpoint(LoggerFactory);
        var request = new LinkedEditingRangeParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Position = new Position { Line = 1, Character = 3 } // <bo[||]dy />
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(request, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void VerifyWordPatternCorrect()
    {
        // Assert
        Assert.True(Regex.Match("Test", LinkedEditingRangeEndpoint.WordPattern).Length == 4);
        Assert.True(Regex.Match("!Test", LinkedEditingRangeEndpoint.WordPattern).Length == 5);
        Assert.True(Regex.Match("!Test.Test2", LinkedEditingRangeEndpoint.WordPattern).Length == 11);

        Assert.True(Regex.Match("Te>st", LinkedEditingRangeEndpoint.WordPattern).Length != 5);
        Assert.True(Regex.Match("Te/st", LinkedEditingRangeEndpoint.WordPattern).Length != 5);
        Assert.True(Regex.Match("Te\\st", LinkedEditingRangeEndpoint.WordPattern).Length != 5);
        Assert.True(Regex.Match("Te!st", LinkedEditingRangeEndpoint.WordPattern).Length != 5);
        Assert.True(Regex.Match("""
            Te
            st
            """,
            LinkedEditingRangeEndpoint.WordPattern).Length != 4 + Environment.NewLine.Length);
    }
}
