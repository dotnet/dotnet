// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Remote.Razor.Formatting;

[Export(typeof(IFormattingCodeDocumentProvider)), Shared]
internal sealed class RemoteFormattingCodeDocumentProvider : IFormattingCodeDocumentProvider
{
    public Task<RazorCodeDocument> GetCodeDocumentAsync(IDocumentSnapshot snapshot)
    {
        // Formatting always uses design time
        return snapshot.GetGeneratedOutputAsync(forceDesignTimeGeneratedOutput: true);
    }
}
