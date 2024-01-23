// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost;

internal static class IProjectEngineFactoryExtensions
{
    public static RazorProjectEngine Create(
        this IProjectEngineFactory factory,
        IProjectSnapshot projectSnapshot,
        Action<RazorProjectEngineBuilder>? configure = null)
        => factory.Create(
            projectSnapshot.Configuration,
            RazorProjectFileSystem.Create(Path.GetDirectoryName(projectSnapshot.FilePath)),
            configure);

    public static RazorProjectEngine Create(
        this IProjectEngineFactoryProvider factoryProvider,
        IProjectSnapshot projectSnapshot,
        Action<RazorProjectEngineBuilder>? configure = null)
        => factoryProvider.Create(
            projectSnapshot.Configuration,
            rootDirectoryPath: Path.GetDirectoryName(projectSnapshot.FilePath).AssumeNotNull(),
            configure);

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
