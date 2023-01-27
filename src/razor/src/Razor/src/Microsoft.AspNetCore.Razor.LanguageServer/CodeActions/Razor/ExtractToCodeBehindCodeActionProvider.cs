// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal class ExtractToCodeBehindCodeActionProvider : RazorCodeActionProvider
{
    private static readonly Task<IReadOnlyList<RazorVSInternalCodeAction>?> s_emptyResult = Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(null);
    private readonly ILogger<ExtractToCodeBehindCodeActionProvider> _logger;

    public ExtractToCodeBehindCodeActionProvider(ILoggerFactory loggerFactory)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _logger = loggerFactory.CreateLogger<ExtractToCodeBehindCodeActionProvider>();
    }

    public override Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(RazorCodeActionContext context, CancellationToken cancellationToken)
    {
        if (context is null)
        {
            return s_emptyResult;
        }

        if (!context.SupportsFileCreation)
        {
            return s_emptyResult;
        }

        if (!FileKinds.IsComponent(context.CodeDocument.GetFileKind()))
        {
            return s_emptyResult;
        }

        var change = new SourceChange(context.Location.AbsoluteIndex, length: 0, newText: string.Empty);
        var syntaxTree = context.CodeDocument.GetSyntaxTree();
        if (syntaxTree?.Root is null)
        {
            return s_emptyResult;
        }

        var owner = syntaxTree.Root.LocateOwner(change);
        if (owner is null)
        {
            _logger.LogWarning("Owner should never be null.");
            return s_emptyResult;
        }

        var directiveNode = owner?.Parent switch
        {
            // When the caret is '@code$$ {' or '@code$${' then tree is:
            // RazorDirective -> RazorDirectiveBody -> CSharpCodeBlock -> (MetaCode or TextLiteral)
            CSharpCodeBlockSyntax { Parent: { Parent: RazorDirectiveSyntax d } } => d,
            // When the caret is '@$$code' or '@c$$ode' or '@co$$de' or '@cod$$e' then tree is:
            // RazorDirective -> RazorDirectiveBody -> MetaCode
            RazorDirectiveBodySyntax { Parent: RazorDirectiveSyntax d } => d,
            _ => null
        };
        if (directiveNode is null)
        {
            return s_emptyResult;
        }

        // Make sure we've found a @code or @functions
        if (directiveNode.DirectiveDescriptor != ComponentCodeDirective.Directive &&
            directiveNode.DirectiveDescriptor != FunctionsDirective.Directive)
        {
            return s_emptyResult;
        }

        // No code action if malformed
        if (directiveNode.GetDiagnostics().Any(d => d.Severity == RazorDiagnosticSeverity.Error))
        {
            return s_emptyResult;
        }

        var csharpCodeBlockNode = (directiveNode.Body as RazorDirectiveBodySyntax)?.CSharpCode;
        if (csharpCodeBlockNode is null)
        {
            return s_emptyResult;
        }

        // Do not provide code action if the cursor is inside the code block
        if (context.Location.AbsoluteIndex > csharpCodeBlockNode.SpanStart)
        {
            return s_emptyResult;
        }

        if (HasUnsupportedChildren(csharpCodeBlockNode))
        {
            return s_emptyResult;
        }

        if (!TryGetNamespace(context.CodeDocument, out var @namespace))
        {
            return s_emptyResult;
        }

        var actionParams = new ExtractToCodeBehindCodeActionParams()
        {
            Uri = context.Request.TextDocument.Uri,
            ExtractStart = csharpCodeBlockNode.Span.Start,
            ExtractEnd = csharpCodeBlockNode.Span.End,
            RemoveStart = directiveNode.Span.Start,
            RemoveEnd = directiveNode.Span.End,
            Namespace = @namespace
        };

        var resolutionParams = new RazorCodeActionResolutionParams()
        {
            Action = LanguageServerConstants.CodeActions.ExtractToCodeBehindAction,
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = actionParams,
        };

        var codeAction = RazorCodeActionFactory.CreateExtractToCodeBehind(resolutionParams);
        var codeActions = new List<RazorVSInternalCodeAction> { codeAction };

        return Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(codeActions);
    }

    private bool TryGetNamespace(RazorCodeDocument codeDocument, [NotNullWhen(returnValue: true)] out string? @namespace)
        // If the compiler can't provide a computed namespace it will fallback to "__GeneratedComponent" or
        // similar for the NamespaceNode. This would end up with extracting to a wrong namespace
        // and causing compiler errors. Avoid offering this refactoring if we can't accurately get a
        // good namespace to extract to
        => codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out @namespace);

    private static bool HasUnsupportedChildren(Language.Syntax.SyntaxNode node)
    {
        return node.DescendantNodes().Any(n =>
            n is MarkupBlockSyntax ||
            n is CSharpTransitionSyntax ||
            n is RazorCommentBlockSyntax);
    }
}
