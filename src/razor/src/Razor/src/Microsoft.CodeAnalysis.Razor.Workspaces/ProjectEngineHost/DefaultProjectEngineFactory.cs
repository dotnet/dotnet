// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectEngineHost;

internal sealed class DefaultProjectEngineFactory : IProjectEngineFactory
{
    public string ConfigurationName => "Default";

    public RazorProjectEngine Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder>? configure)
    {
        return RazorProjectEngine.Create(configuration, fileSystem, b =>
        {
            CompilerFeatures.Register(b);

            configure?.Invoke(b);
        });
    }
}
