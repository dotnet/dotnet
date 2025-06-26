// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectEngineHost;

internal sealed class EmptyProjectEngineFactory : IProjectEngineFactory
{
    public string ConfigurationName => "Empty";

    public RazorProjectEngine Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder>? configure)
    {
        ArgHelper.ThrowIfNull(configuration);
        ArgHelper.ThrowIfNull(fileSystem);

        // This is a very basic implementation that will provide reasonable support without crashing.
        // If the user falls into this situation, ideally they can realize that something is wrong and take
        // action.
        //
        // This has no support for:
        // - Tag Helpers
        // - Imports
        // - Default Imports
        // - and will have a very limited set of directives
        return RazorProjectEngine.Create(configuration, fileSystem, configure);
    }
}
