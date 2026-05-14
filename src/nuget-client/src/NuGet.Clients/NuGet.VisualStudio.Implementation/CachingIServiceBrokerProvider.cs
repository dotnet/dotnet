// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio.Common;

namespace NuGet.VisualStudio.Implementation
{
    [Export(typeof(IServiceBrokerProvider))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal sealed class CachingIServiceBrokerProvider : IServiceBrokerProvider
    {
        private readonly AsyncLazy<IServiceBroker> _serviceBroker;

        internal CachingIServiceBrokerProvider()
        {
            _serviceBroker = new AsyncLazy<IServiceBroker>(
                () => BrokeredServicesUtilities.GetRemoteServiceBrokerAsync().AsTask(),
                NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public async ValueTask<IServiceBroker> GetAsync()
        {
            return await _serviceBroker.GetValueAsync();
        }
    }
}
