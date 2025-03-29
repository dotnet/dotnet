// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal static partial class ProjectEngineFactories
{
    public static IProjectEngineFactory Empty { get; } = new EmptyProjectEngineFactory();

    public static IProjectEngineFactory Default { get; } = new DefaultProjectEngineFactory();

    public static IProjectEngineFactory MVC_1_0 { get; } = new ProjectEngineFactory("MVC-1.0");
    public static IProjectEngineFactory MVC_1_1 { get; } = new ProjectEngineFactory("MVC-1.1");
    public static IProjectEngineFactory MVC_2_0 { get; } = new ProjectEngineFactory("MVC-2.0");
    public static IProjectEngineFactory MVC_2_1 { get; } = new ProjectEngineFactory("MVC-2.1");
    public static IProjectEngineFactory MVC_3_0 { get; } = new ProjectEngineFactory("MVC-3.0");

    public static IProjectEngineFactory Unsupported { get; } = new ProjectEngineFactory_Unsupported();

    public static ImmutableArray<IProjectEngineFactory> All { get; } =
    [
        // Razor based configurations
        Default,
        MVC_1_0,
        MVC_1_1,
        MVC_2_0,
        MVC_2_1,
        MVC_3_0,

        // Unsupported (Legacy/System.Web.Razor)
        Unsupported
    ];

    public static IProjectEngineFactoryProvider DefaultProvider { get; } = new ProjectEngineFactoryProvider(All);
}
