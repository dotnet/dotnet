// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.Commanding;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Name(nameof(ViewCodeCommandHandler))]
[Export(typeof(ICommandHandler))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
internal sealed class ViewCodeCommandHandler : ICommandHandler<ViewCodeCommandArgs>
{
    // Because query status happens all the time we want to cache the File.Exists checks for a reasonable amount of time
    private const int CacheTimeoutMilliseconds = 10000;
    private static readonly Stopwatch s_fileExistsStopwatch = Stopwatch.StartNew();
    private static readonly Dictionary<string, (bool exists, long addedMs)> s_fileExistsCache = new();

    private static readonly ImmutableHashSet<string> s_relatedRazorFileSuffixes = ImmutableHashSet.CreateRange(StringComparer.OrdinalIgnoreCase, new[] { RazorLSPConstants.CSHTMLFileExtension, RazorLSPConstants.RazorFileExtension });

    private static readonly CommandState s_availableCommandState = new(isAvailable: true, displayText: SR.View_Code);
    private readonly DocumentInteractionManager _documentInteractionManager;
    private readonly ITextDocumentFactoryService _textDocumentFactoryService;
    private readonly JoinableTaskContext _joinableTaskContext;

    public string DisplayName => nameof(ViewCodeCommandHandler);

    [ImportingConstructor]
    public ViewCodeCommandHandler(
        DocumentInteractionManager documentInteractionManager,
        ITextDocumentFactoryService textDocumentFactoryService,
        JoinableTaskContext joinableTaskContext)
    {
        _documentInteractionManager = documentInteractionManager;
        _textDocumentFactoryService = textDocumentFactoryService;
        _joinableTaskContext = joinableTaskContext;
    }

    public CommandState GetCommandState(ViewCodeCommandArgs args)
    {
        if (TryGetCSharpFilePath(args.SubjectBuffer, out _))
        {
            return s_availableCommandState;
        }

        return CommandState.Unavailable;
    }

    public bool ExecuteCommand(ViewCodeCommandArgs args, CommandExecutionContext executionContext)
    {
        if (TryGetCSharpFilePath(args.SubjectBuffer, out var csharpFilePath))
        {
            _joinableTaskContext.Factory.Run(() => _documentInteractionManager.OpenDocumentAsync(csharpFilePath, CancellationToken.None));

            return true;
        }

        return false;
    }

    private bool TryGetCSharpFilePath(ITextBuffer buffer, [NotNullWhen(true)] out string? codeFilePath)
    {
        codeFilePath = null;

        if (!_textDocumentFactoryService.TryGetTextDocument(buffer, out var document) ||
            document?.FilePath is null)
        {
            return false;
        }

        var filePath = document.FilePath;
        var extension = Path.GetExtension(filePath);

        if (!s_relatedRazorFileSuffixes.Contains(extension))
        {
            return false;
        }

        codeFilePath = Path.ChangeExtension(filePath, extension + RazorLSPConstants.CSharpFileExtension);

        var now = s_fileExistsStopwatch.ElapsedMilliseconds;

        if (!s_fileExistsCache.TryGetValue(codeFilePath, out var cache) ||
            now - cache.addedMs > CacheTimeoutMilliseconds)
        {
            var exists = File.Exists(codeFilePath);
            s_fileExistsCache[codeFilePath] = (exists, now);
        }

        return s_fileExistsCache[codeFilePath].exists;
    }
}
