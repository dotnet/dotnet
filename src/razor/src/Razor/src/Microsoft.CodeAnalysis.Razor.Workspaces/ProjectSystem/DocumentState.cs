// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem.Sources;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed partial class DocumentState
{
    public HostDocument HostDocument { get; }
    public int Version { get; }

    private readonly ITextAndVersionSource _textAndVersionSource;
    private readonly GeneratedOutputSource _generatedOutputSource;

    private DocumentState(HostDocument hostDocument, ITextAndVersionSource textAndVersionSource)
    {
        HostDocument = hostDocument;
        Version = 1;
        _textAndVersionSource = textAndVersionSource;
        _generatedOutputSource = new();
    }

    private DocumentState(DocumentState oldState, ITextAndVersionSource textAndVersionSource)
    {
        HostDocument = oldState.HostDocument;
        Version = oldState.Version + 1;
        _textAndVersionSource = textAndVersionSource;
        _generatedOutputSource = new();
    }

    public static DocumentState Create(HostDocument hostDocument, SourceText text)
        => new(hostDocument, CreateTextAndVersionSource(text));

    public static DocumentState Create(HostDocument hostDocument, TextLoader textLoader)
        => new(hostDocument, CreateTextAndVersionSource(textLoader));

    private static ConstantTextAndVersionSource CreateTextAndVersionSource(SourceText text, VersionStamp? version = null)
        => new(text, version ?? VersionStamp.Create());

    private static LoadableTextAndVersionSource CreateTextAndVersionSource(TextLoader textLoader)
        => new(textLoader);

    public bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result)
        => _generatedOutputSource.TryGetValue(out result);

    public ValueTask<RazorCodeDocument> GetGeneratedOutputAsync(DocumentSnapshot document, CancellationToken cancellationToken)
        => _generatedOutputSource.GetValueAsync(document, cancellationToken);

    public bool TryGetTextAndVersion([NotNullWhen(true)] out TextAndVersion? result)
        => _textAndVersionSource.TryGetValue(out result);

    public ValueTask<TextAndVersion> GetTextAndVersionAsync(CancellationToken cancellationToken)
        => _textAndVersionSource.GetValueAsync(cancellationToken);

    public bool TryGetText([NotNullWhen(true)] out SourceText? result)
    {
        if (TryGetTextAndVersion(out var textAndVersion))
        {
            result = textAndVersion.Text;
            return true;
        }

        result = null;
        return false;
    }

    public ValueTask<SourceText> GetTextAsync(CancellationToken cancellationToken)
    {
        return TryGetText(out var text)
            ? new(text)
            : GetTextCoreAsync(cancellationToken);

        async ValueTask<SourceText> GetTextCoreAsync(CancellationToken cancellationToken)
        {
            var textAsVersion = await GetTextAndVersionAsync(cancellationToken).ConfigureAwait(false);

            return textAsVersion.Text;
        }
    }

    public bool TryGetTextVersion(out VersionStamp result)
    {
        if (TryGetTextAndVersion(out var textAndVersion))
        {
            result = textAndVersion.Version;
            return true;
        }

        result = default;
        return false;
    }

    public ValueTask<VersionStamp> GetTextVersionAsync(CancellationToken cancellationToken)
    {
        return TryGetTextVersion(out var version)
            ? new(version)
            : GetTextVersionCoreAsync(cancellationToken);

        async ValueTask<VersionStamp> GetTextVersionCoreAsync(CancellationToken cancellationToken)
        {
            var textAsVersion = await GetTextAndVersionAsync(cancellationToken).ConfigureAwait(false);

            return textAsVersion.Version;
        }
    }

    public DocumentState WithConfigurationChange()
        => new(this, _textAndVersionSource);

    public DocumentState WithImportsChange()
        => new(this, _textAndVersionSource);

    public DocumentState WithProjectWorkspaceStateChange()
        => new(this, _textAndVersionSource);

    public DocumentState WithText(SourceText text, VersionStamp textVersion)
        => new(this, CreateTextAndVersionSource(text, textVersion));

    public DocumentState WithTextLoader(TextLoader textLoader)
        => ReferenceEquals(textLoader, _textAndVersionSource.TextLoader)
            ? this
            : new(this, CreateTextAndVersionSource(textLoader));
}
