// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Razor.Settings;
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
