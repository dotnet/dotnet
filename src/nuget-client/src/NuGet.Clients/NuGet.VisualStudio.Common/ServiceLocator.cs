// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using VsServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// This class unifies all the different ways of getting services within visual studio.
    /// </summary>
    public static class ServiceLocator
    {
        public static void InitializePackageServiceProvider(IAsyncServiceProvider provider)
        {
            PackageServiceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        private static IAsyncServiceProvider PackageServiceProvider;

        /// <summary>
        /// Fetches a service from the service provider through the <see cref="ServiceExtensions.GetServiceAsync{TService, TInterface}(IAsyncServiceProvider, bool)"/> extension method.
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <typeparam name="TInterface">Service interface</typeparam>
        /// <returns>The service if available, null otherwise.</returns>
        public static async Task<TInterface> GetGlobalServiceAsync<TService, TInterface>() where TInterface : class
        {
            if (PackageServiceProvider != null)
            {
                var service = await PackageServiceProvider.GetServiceAsync<TService, TInterface>(throwOnFailure: false);
                if (service != null)
                {
                    return service;
                }
            }

            // VS Threading Rule #1
            // Access to ServiceProvider and a lot of casts are performed in this method,
            // and so this method can RPC into main thread. Switch to main thread explictly, since method has STA requirement
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // This is a fallback, primarily hit in tests.
            return Package.GetGlobalService(typeof(TService)) as TInterface;
        }

        /// <summary>
        /// Fetches a service from the service provider in a free threaded fashion (ie. no UI thread transitions).
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <typeparam name="TInterface">Service interface</typeparam>
        /// <returns>The service if available, null otherwise.</returns>
        public static async Task<TInterface> GetGlobalServiceFreeThreadedAsync<TService, TInterface>() where TInterface : class
        {
            if (PackageServiceProvider != null)
            {
                TInterface service = await PackageServiceProvider.GetFreeThreadedServiceAsync<TService, TInterface>();

                if (service != null)
                {
                    return service;
                }
            }

            return await AsyncServiceProvider.GlobalProvider.GetServiceAsync<TService, TInterface>();
        }

        /// <summary>
        /// Fetches a MEF registered service if available.
        /// This method should be called from a background thread only. 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns>The instance of the service request, <see langword="null"/> otherwise. </returns>
        /// <remarks>
        /// This method should only be preferred when using MEF imports is not easily achievable.
        /// This method can be called from the UI thread, but that's unnecessary and a bad practice. Never do things that don't need the UI thread, on the UI thread.
        /// </remarks>
        public static async Task<TService> GetComponentModelServiceAsync<TService>() where TService : class
        {
            IComponentModel componentModel = await GetComponentModelAsync();
            return componentModel?.GetService<TService>();
        }

        /// <inheritdoc cref="GetComponentModelServiceAsync{TService}"/>
        public static TService GetComponentModelService<TService>() where TService : class
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(GetComponentModelServiceAsync<TService>);
        }

        public static async Task<IComponentModel> GetComponentModelAsync()
        {
            return await GetGlobalServiceFreeThreadedAsync<SComponentModel, IComponentModel>();
        }

        public static async Task<IServiceProvider> GetServiceProviderAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await GetGlobalServiceAsync<SDTE, DTE>();
            return GetServiceProviderFromDTE(dte);
        }

        private static IServiceProvider GetServiceProviderFromDTE(DTE dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IServiceProvider serviceProvider = new ServiceProvider(dte as VsServiceProvider);
            Debug.Assert(serviceProvider != null, "Service provider is null");
            return serviceProvider;
        }
    }
}
