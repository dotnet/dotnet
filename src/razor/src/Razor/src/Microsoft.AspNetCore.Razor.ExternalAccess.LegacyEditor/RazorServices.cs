// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

[Export]
[System.Composition.Shared]
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

        var editorFactoryService = componentModel.GetService<RazorEditorFactoryService>();
        EditorFactoryService = RazorWrapperFactory.WrapEditorFactoryService(editorFactoryService);

        var editorSettingsManager = componentModel.GetService<EditorSettingsManager>();
        EditorSettingsManager = RazorWrapperFactory.WrapEditorSettingsManager(editorSettingsManager);

        var tagHelperCompletionService = componentModel.GetService<TagHelperCompletionService>();
        TagHelperCompletionService = RazorWrapperFactory.WrapTagHelperCompletionService(tagHelperCompletionService);

        var tagHelperFactsService = componentModel.GetService<ITagHelperFactsService>();
        TagHelperFactsService = RazorWrapperFactory.WrapTagHelperFactsService(tagHelperFactsService);
    }
}
