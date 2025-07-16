// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed partial class LspDynamicFileProvider
{
    private sealed class LspTextChangesTextLoader(
        TextDocument? document,
        RazorTextChange[] changes,
        byte[] checksum,
        SourceHashAlgorithm checksumAlgorithm,
        int? codePage,
        Uri razorUri,
        IRazorClientLanguageServerManager razorClientLanguageServerManager) : TextLoader
    {
        private readonly TextDocument? _document = document;
        private readonly ImmutableArray<TextChange> _changes = changes.SelectAsArray(c => c.ToTextChange());
        private readonly byte[] _checksum = checksum;
        private readonly SourceHashAlgorithm _checksumAlgorithm = checksumAlgorithm;
        private readonly int? _codePage = codePage;
        private readonly DocumentUri _razorUri = new(razorUri);
        private readonly IRazorClientLanguageServerManager _razorClientLanguageServerManager = razorClientLanguageServerManager;
        private readonly Lazy<SourceText> _emptySourceText = new(() =>
        {
            var encoding = codePage is null ? null : Encoding.GetEncoding(codePage.Value);
            return SourceText.From("", checksumAlgorithm: checksumAlgorithm, encoding: encoding);
        });

        public override async Task<TextAndVersion> LoadTextAndVersionAsync(LoadTextOptions options, CancellationToken cancellationToken)
        {
            try
            {
                if (_document is null)
                {
                    var text = ApplyChanges(_emptySourceText.Value, _changes);
                    return TextAndVersion.Create(text, VersionStamp.Default.GetNewerVersion());
                }

                var sourceText = await _document.GetTextAsync(cancellationToken).ConfigureAwait(false);

                // Validate the checksum information so the edits are known to be correct

                if (IsSourceTextMatching(sourceText))
                {
                    var version = await _document.GetTextVersionAsync(cancellationToken).ConfigureAwait(false);
                    var newText = ApplyChanges(sourceText, _changes);
                    return TextAndVersion.Create(newText, version.GetNewerVersion());
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // This happens if ApplyChanges tries to apply an invalid TextChange.
                // This is recoverable but incurs a perf hit for getting the full text below.

                // TODO: Add ability to capture a fault here in EA. There's something wrong if
                // the Checksum matches but the text changes can't be applied.
            }

            return await GetFullDocumentFromServerAsync(cancellationToken).ConfigureAwait(false);
        }

        private bool IsSourceTextMatching(SourceText sourceText)
        {
            if (sourceText.ChecksumAlgorithm != _checksumAlgorithm)
            {
                return false;
            }

            if (sourceText.Encoding?.CodePage != _codePage)
            {
                return false;
            }

            if (!sourceText.GetChecksum().SequenceEqual(_checksum))
            {
                return false;
            }

            return true;
        }

        private async Task<TextAndVersion> GetFullDocumentFromServerAsync(CancellationToken cancellationToken)
        {
            var response = await _razorClientLanguageServerManager.SendRequestAsync<RazorProvideDynamicFileParams, RazorProvideDynamicFileResponse>(
                ProvideRazorDynamicFileInfoMethodName,
                new RazorProvideDynamicFileParams
                {
                    RazorDocument = new()
                    {
                        DocumentUri = _razorUri,
                    },
                    FullText = true
                },
                cancellationToken).ConfigureAwait(false);

            var text = ApplyChanges(_emptySourceText.Value, response.Edits.SelectAsArray(e => e.ToTextChange()));
            return TextAndVersion.Create(text, VersionStamp.Default.GetNewerVersion());
        }

        private static SourceText ApplyChanges(SourceText sourceText, ImmutableArray<TextChange> changes)
        {
            foreach (var change in changes)
            {
                sourceText = sourceText.WithChanges(change);
            }

            return sourceText;
        }
    }
}

