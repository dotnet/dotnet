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

#pragma warning disable RS0030 // Do not used banned APIs
        /// <summary>
        /// Use this to acquire services that *do not* have UI thread dependencies.
        /// Under the hood, this method simply justs <see cref="IAsyncServiceProvider.GetServiceAsync(Type)"/>
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <typeparam name="TInterface">Interface type</typeparam>
        /// <param name="site">Service Provider</param>
        /// <returns>Service from the given ServiceProvider.</returns>
        public static async Task<TInterface> GetFreeThreadedServiceAsync<TService, TInterface>(this IAsyncServiceProvider site) where TInterface : class
        {
            // Note that using Microsoft.VisualStudio.Shell.ServiceExtensions.GetServiceAsync<TService, TInterface>()
            // is not appropriate because that method always switches to the UI thread to cast to the Interface.
            object service = await site.GetServiceAsync(typeof(TService));
            return service as TInterface;
        }
#pragma warning restore RS0030 // Do not used banned APIs
    }
}

