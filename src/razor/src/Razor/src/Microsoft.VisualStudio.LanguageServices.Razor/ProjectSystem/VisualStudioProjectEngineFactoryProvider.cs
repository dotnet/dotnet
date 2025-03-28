// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

[Export(typeof(IProjectEngineFactoryProvider))]
internal sealed class VisualStudioProjectEngineFactoryProvider : IProjectEngineFactoryProvider
{
    public IProjectEngineFactory GetFactory(RazorConfiguration configuration)
        => ProjectEngineFactories.DefaultProvider.GetFactory(configuration);
}
