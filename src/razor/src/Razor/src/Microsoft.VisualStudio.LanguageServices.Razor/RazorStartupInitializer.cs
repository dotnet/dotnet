// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.ComponentModelHost;

namespace Microsoft.VisualStudio.Razor;

[Export(typeof(RazorStartupInitializer))]
internal sealed class RazorStartupInitializer
{
    private static RazorStartupInitializer? s_initializer;

    [ImportingConstructor]
    public RazorStartupInitializer(
        LanguageServerFeatureOptions options,
        [ImportMany] IEnumerable<IRazorStartupService> services)
    {
        Debug.Assert(!options.UseRazorCohostServer, "If cohosting is on we should never initialize Razor startup services.");

        Services = services;
    }

    public IEnumerable<IRazorStartupService> Services { get; }

    public static void Initialize(IServiceProvider serviceProvider)
    {
        if (s_initializer is null)
        {
            Interlocked.CompareExchange(ref s_initializer, GetInitializer(serviceProvider), null);
        }

        static RazorStartupInitializer GetInitializer(IServiceProvider serviceProvider)
        {
            var componentModel = serviceProvider.GetService(typeof(SComponentModel)) as IComponentModel;
            Assumes.Present(componentModel);

            return componentModel.DefaultExportProvider.GetExportedValue<RazorStartupInitializer>();
        }
    }
}
