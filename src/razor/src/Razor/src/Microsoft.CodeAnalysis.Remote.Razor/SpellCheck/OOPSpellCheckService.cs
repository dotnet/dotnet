// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.SpellCheck;

namespace Microsoft.CodeAnalysis.Remote.Razor.SpellCheck;

[Export(typeof(ISpellCheckService)), Shared]
[method: ImportingConstructor]
internal sealed class OOPSpellCheckService(
    ICSharpSpellCheckRangeProvider csharpSpellCheckService,
    IDocumentMappingService documentMappingService)
    : SpellCheckService(csharpSpellCheckService, documentMappingService)
{
}
