// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost;

internal class EmptyProjectEngineFactory : IProjectEngineFactory
{
    public RazorProjectEngine Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder> configure)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (fileSystem is null)
        {
            throw new ArgumentNullException(nameof(fileSystem));
        }

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
