// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions.Razor;

internal static class RazorCodeActionFactory
{
    private readonly static Guid s_addComponentUsingTelemetryId = new("6c5416b7-7be7-49ee-aa60-904385be676f");
    private readonly static Guid s_fullyQualifyComponentTelemetryId = new("3d9abe36-7d10-4e08-8c18-ad88baa9a923");
    private readonly static Guid s_createComponentFromTagTelemetryId = new("a28e0baa-a4d5-4953-a817-1db586035841");
    private readonly static Guid s_createExtractToCodeBehindTelemetryId = new("f63167f7-fdc6-450f-8b7b-b240892f4a27");
    private readonly static Guid s_createExtractToComponentTelemetryId = new("af67b0a3-f84b-4808-97a7-b53e85b22c64");
    private readonly static Guid s_generateMethodTelemetryId = new("c14fa003-c752-45fc-bb29-3a123ae5ecef");
    private readonly static Guid s_generateAsyncMethodTelemetryId = new("9058ca47-98e2-4f11-bf7c-a16a444dd939");
    private readonly static Guid s_promoteUsingDirectiveTelemetryId = new("751f9012-e37b-444a-9211-b4ebce91d96e");
    private readonly static Guid s_wrapAttributesTelemetryId = new("1df50ba6-4ed1-40d8-8fe2-1c4c1b08e8b5");

    public static RazorVSInternalCodeAction CreateWrapAttributes(RazorCodeActionResolutionParams resolutionParams)
        => new RazorVSInternalCodeAction
        {
            Title = SR.Wrap_attributes,
            Data = JsonSerializer.SerializeToElement(resolutionParams),
            TelemetryId = s_wrapAttributesTelemetryId,
            Name = LanguageServerConstants.CodeActions.WrapAttributes,
        };

    public static RazorVSInternalCodeAction CreatePromoteUsingDirective(string importsFileName, RazorCodeActionResolutionParams resolutionParams)
        => new RazorVSInternalCodeAction
        {
            Title = SR.FormatPromote_using_directive_to(importsFileName),
            Data = JsonSerializer.SerializeToElement(resolutionParams),
            TelemetryId = s_promoteUsingDirectiveTelemetryId,
            Name = LanguageServerConstants.CodeActions.PromoteUsingDirective,
        };

    public static RazorVSInternalCodeAction CreateAddComponentUsing(string @namespace, string? newTagName, RazorCodeActionResolutionParams resolutionParams)
    {
        var title = $"@using {@namespace}";
        var data = JsonSerializer.SerializeToElement(resolutionParams);
        var codeAction = new RazorVSInternalCodeAction
        {
            Title = newTagName is null ? title : $"{newTagName} - {title}",
            Data = data,
            TelemetryId = s_addComponentUsingTelemetryId,
            Priority = VSInternalPriorityLevel.High,
            Name = LanguageServerConstants.CodeActions.AddUsing,
            // Adding a using for an existing component should be first
            Order = -1000,
        };
        return codeAction;
    }

    public static RazorVSInternalCodeAction CreateFullyQualifyComponent(string fullyQualifiedName, WorkspaceEdit workspaceEdit)
    {
        var codeAction = new RazorVSInternalCodeAction()
        {
            Title = fullyQualifiedName,
            Edit = workspaceEdit,
            TelemetryId = s_fullyQualifyComponentTelemetryId,
            Priority = VSInternalPriorityLevel.High,
            Name = LanguageServerConstants.CodeActions.FullyQualify,
            // Fully qualifying an existing component should be very high, but not quite as high as Add Using
            Order = -900,
        };
        return codeAction;
    }

    public static RazorVSInternalCodeAction CreateComponentFromTag(RazorCodeActionResolutionParams resolutionParams)
    {
        var data = JsonSerializer.SerializeToElement(resolutionParams);
        var codeAction = new RazorVSInternalCodeAction()
        {
            Title = SR.Create_Component_FromTag_Title,
            Data = data,
            TelemetryId = s_createComponentFromTagTelemetryId,
            Name = LanguageServerConstants.CodeActions.CreateComponentFromTag,
        };
        return codeAction;
    }

    public static RazorVSInternalCodeAction CreateExtractToCodeBehind(RazorCodeActionResolutionParams resolutionParams)
    {
        var data = JsonSerializer.SerializeToElement(resolutionParams);
        var codeAction = new RazorVSInternalCodeAction()
        {
            Title = SR.ExtractTo_CodeBehind_Title,
            Data = data,
            TelemetryId = s_createExtractToCodeBehindTelemetryId,
            Name = LanguageServerConstants.CodeActions.ExtractToCodeBehindAction,
        };
        return codeAction;
    }

    public static RazorVSInternalCodeAction CreateExtractToComponent(RazorCodeActionResolutionParams resolutionParams)
    {
        var data = JsonSerializer.SerializeToElement(resolutionParams);
        var codeAction = new RazorVSInternalCodeAction()
        {
            Title = SR.ExtractTo_Component_Title,
            Data = data,
            TelemetryId = s_createExtractToComponentTelemetryId,
            Name = LanguageServerConstants.CodeActions.ExtractToNewComponentAction,
            // Since Extract to Component is offered basically everywhere, always offer it last
            Order = 9999
        };
        return codeAction;
    }

    public static RazorVSInternalCodeAction CreateGenerateMethod(VSTextDocumentIdentifier textDocument, Uri? delegatedDocumentUri, string methodName, string? eventParameterType)
    {
        var @params = new GenerateMethodCodeActionParams
        {
            MethodName = methodName,
            EventParameterType = eventParameterType,
            IsAsync = false
        };
        var resolutionParams = new RazorCodeActionResolutionParams()
        {
            TextDocument = textDocument,
            Action = LanguageServerConstants.CodeActions.GenerateEventHandler,
            Language = RazorLanguageKind.Razor,
            DelegatedDocumentUri = delegatedDocumentUri,
            Data = @params,
        };

        var title = SR.FormatGenerate_Event_Handler_Title(methodName);
        var data = JsonSerializer.SerializeToElement(resolutionParams);
        var codeAction = new RazorVSInternalCodeAction()
        {
            Title = title,
            Data = data,
            TelemetryId = s_generateMethodTelemetryId,
            Name = LanguageServerConstants.CodeActions.GenerateEventHandler,
        };
        return codeAction;
    }

    public static RazorVSInternalCodeAction CreateAsyncGenerateMethod(VSTextDocumentIdentifier textDocument, Uri? delegatedDocumentUri, string methodName, string? eventParameterType)
    {
        var @params = new GenerateMethodCodeActionParams
        {
            MethodName = methodName,
            EventParameterType = eventParameterType,
            IsAsync = true
        };
        var resolutionParams = new RazorCodeActionResolutionParams()
        {
            TextDocument = textDocument,
            Action = LanguageServerConstants.CodeActions.GenerateEventHandler,
            Language = RazorLanguageKind.Razor,
            DelegatedDocumentUri = delegatedDocumentUri,
            Data = @params,
        };

        var title = SR.FormatGenerate_Async_Event_Handler_Title(methodName);
        var data = JsonSerializer.SerializeToElement(resolutionParams);
        var codeAction = new RazorVSInternalCodeAction()
        {
            Title = title,
            Data = data,
            TelemetryId = s_generateAsyncMethodTelemetryId,
            Name = LanguageServerConstants.CodeActions.GenerateAsyncEventHandler,
        };
        return codeAction;
    }
}
