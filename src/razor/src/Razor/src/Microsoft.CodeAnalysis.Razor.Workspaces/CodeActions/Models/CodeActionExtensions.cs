// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions.Models;

internal static class CodeActionExtensions
{
    public static SumType<Command, CodeAction> AsVSCodeCommandOrCodeAction(this VSInternalCodeAction razorCodeAction, VSTextDocumentIdentifier textDocument)
    {
        if (razorCodeAction.Data is null)
        {
            // Only code action edit, we must convert this to a resolvable command

            var resolutionParams = new RazorCodeActionResolutionParams
            {
                TextDocument = textDocument,
                Action = LanguageServerConstants.CodeActions.EditBasedCodeActionCommand,
                Language = RazorLanguageKind.Razor,
                Data = razorCodeAction.Edit ?? new WorkspaceEdit(),
            };

            razorCodeAction = new VSInternalCodeAction()
            {
                Title = razorCodeAction.Title,
                Data = JsonSerializer.SerializeToElement(resolutionParams),
                TelemetryId = razorCodeAction.TelemetryId,
            };
        }

        var serializedParams = JsonSerializer.SerializeToNode(razorCodeAction.Data).AssumeNotNull();
        var arguments = new JsonArray(serializedParams);

        return new Command
        {
            Title = razorCodeAction.Title ?? string.Empty,
            CommandIdentifier = LanguageServerConstants.RazorCodeActionRunnerCommand,
            Arguments = arguments.ToArray()!
        };
    }

    public static RazorVSInternalCodeAction WrapResolvableCodeAction(
        this RazorVSInternalCodeAction razorCodeAction,
        RazorCodeActionContext context,
        string action = LanguageServerConstants.CodeActions.Default,
        RazorLanguageKind language = RazorLanguageKind.CSharp,
        bool isOnAllowList = true)
    {
        var resolutionParams = new RazorCodeActionResolutionParams()
        {
            TextDocument = context.Request.TextDocument,
            Action = action,
            Language = language,
            Data = razorCodeAction.Data
        };
        razorCodeAction.Data = JsonSerializer.SerializeToElement(resolutionParams);

        if (!isOnAllowList)
        {
            razorCodeAction.Title = $"(Exp) {razorCodeAction.Title} ({razorCodeAction.Name})";
        }

        if (razorCodeAction.Children != null)
        {
            for (var i = 0; i < razorCodeAction.Children.Length; i++)
            {
                razorCodeAction.Children[i] = razorCodeAction.Children[i].WrapResolvableCodeAction(context, action, language, isOnAllowList);
            }
        }

        return razorCodeAction;
    }

    private static VSInternalCodeAction WrapResolvableCodeAction(
        this VSInternalCodeAction razorCodeAction,
        RazorCodeActionContext context,
        string action,
        RazorLanguageKind language,
        bool isOnAllowList)
    {
        var resolutionParams = new RazorCodeActionResolutionParams()
        {
            TextDocument = context.Request.TextDocument,
            Action = action,
            Language = language,
            Data = razorCodeAction.Data
        };
        razorCodeAction.Data = JsonSerializer.SerializeToElement(resolutionParams);

        if (!isOnAllowList)
        {
            razorCodeAction.Title = "(Exp) " + razorCodeAction.Title;
        }

        if (razorCodeAction.Children != null)
        {
            for (var i = 0; i < razorCodeAction.Children.Length; i++)
            {
                razorCodeAction.Children[i] = razorCodeAction.Children[i].WrapResolvableCodeAction(context, action, language, isOnAllowList);
            }
        }

        return razorCodeAction;
    }
}
