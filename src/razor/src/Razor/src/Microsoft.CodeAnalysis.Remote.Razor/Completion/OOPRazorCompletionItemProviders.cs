// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.Completion;

[Export(typeof(IRazorCompletionItemProvider)), Shared]
internal sealed class OOPDirectiveCompletionItemProvider : DirectiveCompletionItemProvider;

[Export(typeof(IRazorCompletionItemProvider)), Shared]
internal sealed class OOPDirectiveAttributeCompletionItemProvider : DirectiveAttributeCompletionItemProvider;

[Export(typeof(IRazorCompletionItemProvider)), Shared]
internal sealed class OOPDirectiveAttributeParameterCompletionItemProvider : DirectiveAttributeParameterCompletionItemProvider;

[Export(typeof(IRazorCompletionItemProvider)), Shared]
[method: ImportingConstructor]
internal sealed class OOPDirectiveAttributeTransitionCompletionItemProvider(LanguageServerFeatureOptions languageServerFeatureOptions)
    : DirectiveAttributeTransitionCompletionItemProvider(languageServerFeatureOptions);

[Export(typeof(IRazorCompletionItemProvider)), Shared]
internal sealed class OOPMarkupTransitionCompletionItemProvider : MarkupTransitionCompletionItemProvider;

[Export(typeof(IRazorCompletionItemProvider)), Shared]
[method: ImportingConstructor]
internal sealed class OOPTagHelperCompletionProvider(ITagHelperCompletionService tagHelperCompletionService)
    : TagHelperCompletionProvider(tagHelperCompletionService);
