// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Formatting;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class FormattingLanguageServerClient(HtmlFormattingService htmlFormattingService, ILoggerFactory loggerFactory) : IClientConnection
{
    private readonly HtmlFormattingService _htmlFormattingService = htmlFormattingService;
    private readonly Dictionary<string, RazorCodeDocument> _documents = [];
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public InitializeResult ServerSettings
        => throw new NotImplementedException();

    public void AddCodeDocument(RazorCodeDocument codeDocument)
    {
        var path = FilePathNormalizer.Normalize(codeDocument.Source.FilePath);
        _documents.Add("/" + path, codeDocument);
    }

    private async Task<RazorDocumentFormattingResponse> FormatAsync(DocumentOnTypeFormattingParams @params)
    {
        var generatedHtml = GetGeneratedHtml(@params.TextDocument.DocumentUri.GetRequiredParsedUri());

        var edits = await _htmlFormattingService.GetOnTypeFormattingEditsAsync(_loggerFactory, @params.TextDocument.DocumentUri.GetRequiredParsedUri(), generatedHtml, @params.Position, @params.Options.InsertSpaces, @params.Options.TabSize);

        return new()
        {
            Edits = edits
        };
    }

    private async Task<RazorDocumentFormattingResponse> FormatAsync(DocumentFormattingParams @params)
    {
        var generatedHtml = GetGeneratedHtml(@params.TextDocument.DocumentUri.GetRequiredParsedUri());

        var edits = await _htmlFormattingService.GetDocumentFormattingEditsAsync(_loggerFactory, @params.TextDocument.DocumentUri.GetRequiredParsedUri(), generatedHtml, @params.Options.InsertSpaces, @params.Options.TabSize);

        return new()
        {
            Edits = edits
        };
    }

    private string GetGeneratedHtml(Uri uri)
    {
        var codeDocument = _documents[uri.GetAbsoluteOrUNCPath()];
        var generatedHtml = codeDocument.GetHtmlDocument().Text.ToString();
        return generatedHtml.Replace("\r", "").Replace("\n", "\r\n");
    }

    public async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
    {
        if (@params is DocumentFormattingParams formattingParams &&
            string.Equals(method, CustomMessageNames.RazorHtmlFormattingEndpoint, StringComparison.Ordinal))
        {
            var response = await FormatAsync(formattingParams);

            return Convert(response);
        }
        else if (@params is DocumentOnTypeFormattingParams onTypeFormattingParams &&
            string.Equals(method, CustomMessageNames.RazorHtmlOnTypeFormattingEndpoint, StringComparison.Ordinal))
        {
            var response = await FormatAsync(onTypeFormattingParams);

            return Convert(response);
        }

        throw new NotImplementedException();

        static TResponse Convert(RazorDocumentFormattingResponse response)
        {
            return response is TResponse typedResponse
                ? typedResponse
                : throw new InvalidOperationException();
        }
    }

    public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
