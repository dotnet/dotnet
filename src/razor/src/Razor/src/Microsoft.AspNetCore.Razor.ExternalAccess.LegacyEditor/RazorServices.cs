// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

[Export]
internal sealed class RazorServices
{
    public IRazorEditorFactoryService EditorFactoryService { get; }
    public IRazorEditorSettingsManager EditorSettingsManager { get; }
    public IRazorTagHelperCompletionService TagHelperCompletionService { get; }
    public IRazorTagHelperFactsService TagHelperFactsService { get; }

    [ImportingConstructor]
    public RazorServices(SVsServiceProvider serviceProvider)
    {
        var componentModel = serviceProvider.GetService(typeof(SComponentModel)) as IComponentModel;
        Assumes.Present(componentModel);

        var editorFactoryService = componentModel.GetService<VisualStudio.LegacyEditor.Razor.IRazorEditorFactoryService>();
        EditorFactoryService = RazorWrapperFactory.WrapEditorFactoryService(editorFactoryService);

        var clientSettingsManager = componentModel.GetService<IClientSettingsManager>();
        EditorSettingsManager = RazorWrapperFactory.WrapClientSettingsManager(clientSettingsManager);

        var tagHelperCompletionService = componentModel.GetService<ITagHelperCompletionService>();
        TagHelperCompletionService = RazorWrapperFactory.WrapTagHelperCompletionService(tagHelperCompletionService);

        TagHelperFactsService = RazorWrapperFactory.GetWrappedTagHelperFactsService();
    }
}
