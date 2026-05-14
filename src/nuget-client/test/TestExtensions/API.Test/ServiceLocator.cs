// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace API.Test
{
    internal static class ServiceLocator
    {
        private static readonly AsyncLazy<IComponentModel> _componentModel;

        public static EnvDTE.DTE GetDTE() => GetService<SDTE, EnvDTE.DTE>();

        public static IComponentModel ComponentModel => ThreadHelper.JoinableTaskFactory.Run(_componentModel.GetValueAsync);

        static ServiceLocator()
        {
            _componentModel = new AsyncLazy<IComponentModel>(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    return GetService<SComponentModel, IComponentModel>();
                },
                ThreadHelper.JoinableTaskFactory);
        }

        public static TInterface GetService<TService, TInterface>()
            where TInterface : class
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var instance = Package.GetGlobalService(typeof(TService)) as TInterface;
            Assumes.Present(instance);

            return instance;
        }

        public static TService GetComponent<TService>() where TService : class
        {
            var service = ComponentModel.GetService<TService>();
            Assumes.Present(service);

            return service;
        }
    }
}
