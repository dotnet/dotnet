// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Helper methods to acquire services via <see cref="IServiceProvider"/>.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        public static Task<EnvDTE.DTE> GetDTEAsync(
            this IAsyncServiceProvider site)
        {
            return site.GetServiceAsync<SDTE, EnvDTE.DTE>();
        }

        public static Task<IComponentModel> GetComponentModelAsync(
            this IAsyncServiceProvider site)
        {
            return site.GetServiceAsync<SComponentModel, IComponentModel>();
        }
    }
}

