// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.CodeActions;

// Services

[Export(typeof(ICodeActionsService)), Shared]
[method: ImportingConstructor]
internal sealed class OOPCodeActionsService(
    IDocumentMappingService documentMappingService,
    [ImportMany] IEnumerable<IRazorCodeActionProvider> razorCodeActionProviders,
    [ImportMany] IEnumerable<ICSharpCodeActionProvider> csharpCodeActionProviders,
    [ImportMany] IEnumerable<IHtmlCodeActionProvider> htmlCodeActionProviders,
    LanguageServerFeatureOptions languageServerFeatureOptions)
    : CodeActionsService(documentMappingService, razorCodeActionProviders, csharpCodeActionProviders, htmlCodeActionProviders, languageServerFeatureOptions);

[Export(typeof(ICodeActionResolveService)), Shared]
[method: ImportingConstructor]
internal sealed class OOPCodeActionResolveService(
    [ImportMany] IEnumerable<IRazorCodeActionResolver> razorCodeActionResolvers,
    [ImportMany] IEnumerable<ICSharpCodeActionResolver> csharpCodeActionResolvers,
    [ImportMany] IEnumerable<IHtmlCodeActionResolver> htmlCodeActionResolvers,
    ILoggerFactory loggerFactory)
    : CodeActionResolveService(razorCodeActionResolvers, csharpCodeActionResolvers, htmlCodeActionResolvers, loggerFactory);

// Code Action Providers

[Export(typeof(IRazorCodeActionProvider)), Shared]
[method: ImportingConstructor]
internal sealed class OOPExtractToCodeBehindCodeActionProvider(ILoggerFactory loggerFactory) : ExtractToCodeBehindCodeActionProvider(loggerFactory);

[Export(typeof(IRazorCodeActionProvider)), Shared]
internal sealed class OOPExtractToComponentCodeActionProvider : ExtractToComponentCodeActionProvider;

[Export(typeof(IRazorCodeActionProvider)), Shared]
[method: ImportingConstructor]
internal sealed class OOPComponentAccessibilityCodeActionProvider(IFileSystem fileSystem) : ComponentAccessibilityCodeActionProvider(fileSystem);

[Export(typeof(IRazorCodeActionProvider)), Shared]
internal sealed class OOPGenerateMethodCodeActionProvider : GenerateMethodCodeActionProvider;

[Export(typeof(IRazorCodeActionProvider)), Shared]
internal sealed class OOPPromoteUsingDirectiveCodeActionProvider : PromoteUsingCodeActionProvider;

[Export(typeof(IRazorCodeActionProvider)), Shared]
internal sealed class OOPWrapAttributesCodeActionProvider : WrapAttributesCodeActionProvider;

[Export(typeof(ICSharpCodeActionProvider)), Shared]
internal sealed class OOPTypeAccessibilityCodeActionProvider : TypeAccessibilityCodeActionProvider;

[Export(typeof(ICSharpCodeActionProvider)), Shared]
[method: ImportingConstructor]
internal sealed class OOPDefaultCSharpCodeActionProvider(LanguageServerFeatureOptions languageServerFeatureOptions) : CSharpCodeActionProvider(languageServerFeatureOptions);

[Export(typeof(IHtmlCodeActionProvider)), Shared]
[method: ImportingConstructor]
internal sealed class OOPDefaultHtmlCodeActionProvider(IEditMappingService editMappingService) : HtmlCodeActionProvider(editMappingService);

// Code Action Resolvers

[Export(typeof(IRazorCodeActionResolver)), Shared]
[method: ImportingConstructor]
internal sealed class OOPExtractToCodeBehindCodeActionResolver(
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IRoslynCodeActionHelpers roslynCodeActionHelpers)
    : ExtractToCodeBehindCodeActionResolver(languageServerFeatureOptions, roslynCodeActionHelpers);

[Export(typeof(IRazorCodeActionResolver)), Shared]
[method: ImportingConstructor]
internal sealed class OOPExtractToComponentCodeActionResolver(LanguageServerFeatureOptions languageServerFeatureOptions) : ExtractToComponentCodeActionResolver(languageServerFeatureOptions);

[Export(typeof(IRazorCodeActionResolver)), Shared]
[method: ImportingConstructor]
internal sealed class OOPCreateComponentCodeActionResolver(LanguageServerFeatureOptions languageServerFeatureOptions) : CreateComponentCodeActionResolver(languageServerFeatureOptions);

[Export(typeof(IRazorCodeActionResolver)), Shared]
internal sealed class OOPAddUsingsCodeActionResolver : AddUsingsCodeActionResolver;

[Export(typeof(IRazorCodeActionResolver)), Shared]
[method: ImportingConstructor]
internal sealed class OOPGenerateMethodCodeActionResolver(
    IRoslynCodeActionHelpers roslynCodeActionHelpers,
    IDocumentMappingService documentMappingService,
    IRazorFormattingService razorFormattingService,
    IFileSystem fileSystem)
    : GenerateMethodCodeActionResolver(roslynCodeActionHelpers, documentMappingService, razorFormattingService, fileSystem);

[Export(typeof(IRazorCodeActionResolver)), Shared]
[method: ImportingConstructor]
internal sealed class OOPPromoteUsingDirectiveCodeActionResolver(IFileSystem fileSystem) : PromoteUsingCodeActionResolver(fileSystem);

[Export(typeof(IRazorCodeActionResolver)), Shared]
internal sealed class OOPWrapAttributesCodeActionResolver : WrapAttributesCodeActionResolver;

[Export(typeof(ICSharpCodeActionResolver)), Shared]
[method: ImportingConstructor]
internal sealed class OOPCSharpCodeActionResolver(IRazorFormattingService razorFormattingService) : CSharpCodeActionResolver(razorFormattingService);

[Export(typeof(ICSharpCodeActionResolver)), Shared]
[method: ImportingConstructor]
internal sealed class OOPUnformattedRemappingCSharpCodeActionResolver(IDocumentMappingService documentMappingService) : UnformattedRemappingCSharpCodeActionResolver(documentMappingService);

[Export(typeof(IHtmlCodeActionResolver)), Shared]
[method: ImportingConstructor]
internal sealed class OOPHtmlCodeActionResolver(IEditMappingService editMappingService) : HtmlCodeActionResolver(editMappingService);
