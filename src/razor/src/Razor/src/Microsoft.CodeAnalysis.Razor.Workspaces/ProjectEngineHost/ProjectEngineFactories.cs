// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Microsoft.CodeAnalysis.Razor.ProjectEngineHost;

internal static partial class ProjectEngineFactories
{
    public static IProjectEngineFactory Empty { get; } = new EmptyProjectEngineFactory();

    public static IProjectEngineFactory Default { get; } = new DefaultProjectEngineFactory();

    public static IProjectEngineFactory MVC_1_0 { get; } = new SimpleFactory("MVC-1.0");
    public static IProjectEngineFactory MVC_1_1 { get; } = new SimpleFactory("MVC-1.1");
    public static IProjectEngineFactory MVC_2_0 { get; } = new SimpleFactory("MVC-2.0");
    public static IProjectEngineFactory MVC_2_1 { get; } = new SimpleFactory("MVC-2.1");
    public static IProjectEngineFactory MVC_3_0 { get; } = new SimpleFactory("MVC-3.0");

    public static ImmutableArray<IProjectEngineFactory> All { get; } =
    [
        // Razor based configurations
        Default,
        MVC_1_0,
        MVC_1_1,
        MVC_2_0,
        MVC_2_1,
        MVC_3_0
    ];

    public static IProjectEngineFactoryProvider DefaultProvider { get; } = new ProjectEngineFactoryProvider(All);
}
