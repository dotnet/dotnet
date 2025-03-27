// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal static class IProjectEngineFactoryExtensions
{
    public static RazorProjectEngine Create(
        this IProjectEngineFactoryProvider factoryProvider,
        RazorConfiguration configuration,
        string rootDirectoryPath,
        Action<RazorProjectEngineBuilder>? configure = null)
    {
        var factory = factoryProvider.GetFactory(configuration);

        return factory.Create(configuration, RazorProjectFileSystem.Create(rootDirectoryPath), configure);
    }
}
