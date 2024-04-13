// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(IRazorCohostLanguageClientActivationService))]
[method: ImportingConstructor]
internal class RazorCohostLanguageClientActivationService(LanguageServerFeatureOptions languageServerFeatureOptions) : IRazorCohostLanguageClientActivationService
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;

    public bool ShouldActivateCohostServer()
    {
        return _languageServerFeatureOptions.UseRazorCohostServer;
    }
}
