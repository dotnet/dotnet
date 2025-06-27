// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.Composition;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

[Export(typeof(IProjectEngineFactoryProvider))]
internal sealed class VisualStudioProjectEngineFactoryProvider : IProjectEngineFactoryProvider
{
    public IProjectEngineFactory GetFactory(RazorConfiguration configuration)
        => ProjectEngineFactories.DefaultProvider.GetFactory(configuration);
}
