// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Text;
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
        private readonly Uri _razorUri = razorUri;
        private readonly IRazorClientLanguageServerManager _razorClientLanguageServerManager = razorClientLanguageServerManager;
        private readonly Lazy<SourceText> _emptySourceText = new(() =>
        {
            var encoding = codePage is null ? null : Encoding.GetEncoding(codePage.Value);
            return SourceText.From("", checksumAlgorithm: checksumAlgorithm, encoding: encoding);
        });

        public override async Task<TextAndVersion> LoadTextAndVersionAsync(LoadTextOptions options, CancellationToken cancellationToken)
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
                        Uri = _razorUri,
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

