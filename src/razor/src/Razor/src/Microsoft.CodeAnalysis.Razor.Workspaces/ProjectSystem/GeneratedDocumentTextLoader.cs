// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class GeneratedDocumentTextLoader(IDocumentSnapshot document, string filePath) : TextLoader
{
    private readonly IDocumentSnapshot _document = document;
    private readonly string _filePath = filePath;
    private readonly VersionStamp _version = VersionStamp.Create();

    public override async Task<TextAndVersion> LoadTextAndVersionAsync(LoadTextOptions options, CancellationToken cancellationToken)
    {
        var output = await _document.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);

        var csharpSourceText = output.GetCSharpDocument().Text;

        // If the encoding isn't UTF8, edit-continue won't work.
        Debug.Assert(csharpSourceText.Encoding == Encoding.UTF8);

        return TextAndVersion.Create(csharpSourceText, _version, _filePath);
    }
}
