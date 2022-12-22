// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal abstract class RazorComponentSearchEngine
{
    public abstract Task<DocumentSnapshot?> TryLocateComponentAsync(TagHelperDescriptor tagHelper);

    public abstract Task<TagHelperDescriptor?> TryGetTagHelperDescriptorAsync(DocumentSnapshot codeDocument, System.Threading.CancellationToken cancellationToken);
}
