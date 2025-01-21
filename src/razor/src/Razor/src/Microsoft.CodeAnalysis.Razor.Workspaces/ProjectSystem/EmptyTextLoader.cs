// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed class EmptyTextLoader : TextLoader
{
    private static readonly SourceText s_emptySourceText = SourceText.From(string.Empty, Encoding.UTF8);
    private static readonly VersionStamp s_version = VersionStamp.Create();

    public static TextLoader Instance { get; } = new EmptyTextLoader();

    private EmptyTextLoader()
    {
    }

    public override Task<TextAndVersion> LoadTextAndVersionAsync(LoadTextOptions options, CancellationToken cancellationToken)
    {
        var textAndVersion = TextAndVersion.Create(s_emptySourceText, s_version);
        return Task.FromResult(textAndVersion);
    }
}
