// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectEngineHost;

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
