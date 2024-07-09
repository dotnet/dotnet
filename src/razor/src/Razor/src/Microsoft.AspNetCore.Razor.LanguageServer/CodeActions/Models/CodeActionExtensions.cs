// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

internal static class CodeActionExtensions
{
    public static SumType<Command, CodeAction> AsVSCodeCommandOrCodeAction(this VSInternalCodeAction razorCodeAction)
    {
        if (razorCodeAction.Data is null)
        {
            // Only code action edit, we must convert this to a resolvable command

            var resolutionParams = new RazorCodeActionResolutionParams
            {
                Action = LanguageServerConstants.CodeActions.EditBasedCodeActionCommand,
                Language = LanguageServerConstants.CodeActions.Languages.Razor,
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
        string language = LanguageServerConstants.CodeActions.Languages.CSharp,
        bool isOnAllowList = true)
    {
        var resolveParams = new CodeActionResolveParams()
        {
            Data = razorCodeAction.Data,
            RazorFileIdentifier = context.Request.TextDocument
        };

        var resolutionParams = new RazorCodeActionResolutionParams()
        {
            Action = action,
            Language = language,
            Data = resolveParams
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
        string language,
        bool isOnAllowList)
    {
        var resolveParams = new CodeActionResolveParams()
        {
            Data = razorCodeAction.Data,
            RazorFileIdentifier = context.Request.TextDocument
        };

        var resolutionParams = new RazorCodeActionResolutionParams()
        {
            Action = action,
            Language = language,
            Data = resolveParams
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
