// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.ComponentModelHost;

namespace Microsoft.VisualStudio.Editor.Razor;

[Export(typeof(RazorStartupInitializer))]
[method: ImportingConstructor]
internal sealed class RazorStartupInitializer([ImportMany] IEnumerable<IRazorStartupService> services)
{
    private static RazorStartupInitializer? s_initializer;

    public IEnumerable<IRazorStartupService> Services { get; } = services;

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
