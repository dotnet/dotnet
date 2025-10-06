// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal class GeneratedDocumentTextLoader(IDocumentSnapshot document, string filePath) : TextLoader
{
    private readonly IDocumentSnapshot _document = document;
    private readonly string _filePath = filePath;
    private readonly VersionStamp _version = VersionStamp.Create();

    public override async Task<TextAndVersion> LoadTextAndVersionAsync(LoadTextOptions options, CancellationToken cancellationToken)
    {
        var output = await _document.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);

        var csharpSourceText = output.GetRequiredCSharpDocument().Text;

        // If the encoding isn't UTF8, edit-continue won't work.
        Debug.Assert(csharpSourceText.Encoding == Encoding.UTF8);

        return TextAndVersion.Create(csharpSourceText, _version, _filePath);
    }
}
