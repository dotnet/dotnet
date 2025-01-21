// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal interface ICodeActionsService
{
    Task<SumType<Command, CodeAction>[]?> GetCodeActionsAsync(
        VSCodeActionParams request,
        IDocumentSnapshot documentSnapshot,
        RazorVSInternalCodeAction[] delegatedCodeActions,
        Uri? delegatedDocumentUri,
        bool supportsCodeActionResolve,
        CancellationToken cancellationToken);

    Task<VSCodeActionParams?> GetCSharpCodeActionsRequestAsync(IDocumentSnapshot documentSnapshot, VSCodeActionParams request, CancellationToken cancellationToken);
}
