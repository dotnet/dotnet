// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using NuGet.Configuration;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class NuGetSourcesService : INuGetSourcesService
    {
        private readonly ServiceActivationOptions _options;
        private readonly IServiceBroker _serviceBroker;
        private readonly AuthorizationServiceClient _authorizationServiceClient;
        private readonly ISharedServiceState _sharedServiceState;

        public event EventHandler<IReadOnlyList<PackageSourceContextInfo>>? PackageSourcesChanged;

        public NuGetSourcesService(
            ServiceActivationOptions options,
            IServiceBroker serviceBroker,
            AuthorizationServiceClient authorizationServiceClient,
            ISharedServiceState state)
        {
            Assumes.NotNull(serviceBroker);
            Assumes.NotNull(authorizationServiceClient);
            Assumes.NotNull(state);

            _options = options;
            _serviceBroker = serviceBroker;
            _authorizationServiceClient = authorizationServiceClient;
            _sharedServiceState = state;
            _sharedServiceState.SourceRepositoryProvider.PackageSourceProvider.PackageSourcesChanged += PackageSourceProvider_PackageSourcesChanged;
        }

        public ValueTask<IReadOnlyList<PackageSourceContextInfo>> GetPackageSourcesAsync(CancellationToken cancellationToken)
        {
            return new ValueTask<IReadOnlyList<PackageSourceContextInfo>>(
                _sharedServiceState.SourceRepositoryProvider.PackageSourceProvider
                .LoadPackageSources()
                .Select(packageSource => PackageSourceContextInfo.Create(packageSource))
                .ToList());
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public ValueTask SavePackageSourcesAsync(IReadOnlyList<PackageSource> sources, PackageSourceUpdateOptions packageSourceUpdateOptions, CancellationToken cancellationToken)
        {
            var packageSources2 = _sharedServiceState.SourceRepositoryProvider.PackageSourceProvider as IPackageSourceProvider2;
#pragma warning restore CS0618 // Type or member is obsolete

            if (packageSources2 != null)
            {
                packageSources2.SavePackageSources(sources, packageSourceUpdateOptions);
            }
            else
            {
                _sharedServiceState.SourceRepositoryProvider.PackageSourceProvider.SavePackageSources(sources);
            }

            return new ValueTask();
        }

        public ValueTask SavePackageSourceContextInfosAsync(IReadOnlyList<PackageSourceContextInfo> sources, CancellationToken cancellationToken)
        {
            IEnumerable<PackageSource> packageSources = GetPackageSourcesToUpdate(sources);
            _sharedServiceState.SourceRepositoryProvider.PackageSourceProvider.SavePackageSources(packageSources);

            return new ValueTask();
        }

        public ValueTask<string?> GetActivePackageSourceNameAsync(CancellationToken cancellationToken)
        {
            return new ValueTask<string?>(_sharedServiceState.SourceRepositoryProvider.PackageSourceProvider.ActivePackageSourceName);
        }

        public void Dispose()
        {
            _sharedServiceState.SourceRepositoryProvider.PackageSourceProvider.PackageSourcesChanged -= PackageSourceProvider_PackageSourcesChanged;
            _authorizationServiceClient.Dispose();
            GC.SuppressFinalize(this);
        }

        private void PackageSourceProvider_PackageSourcesChanged(object sender, EventArgs e)
        {
            List<PackageSourceContextInfo> packageSources = _sharedServiceState.SourceRepositoryProvider.PackageSourceProvider.LoadPackageSources().Select(packageSource => PackageSourceContextInfo.Create(packageSource)).ToList();
            PackageSourcesChanged?.Invoke(this, packageSources);
        }

        private IReadOnlyList<PackageSource> GetPackageSourcesToUpdate(IReadOnlyList<PackageSourceContextInfo> packageSourceContextInfos)
        {
            Dictionary<int, PackageSource>? packageSources = _sharedServiceState.SourceRepositoryProvider.PackageSourceProvider.LoadPackageSources()
                 .ToDictionary(packageSource => packageSource.GetHashCode(), _ => _);

            var newPackageSources = new List<PackageSource>(capacity: packageSourceContextInfos.Count);

            foreach (PackageSourceContextInfo packageSourceContextInfo in packageSourceContextInfos)
            {
                // If package source is pre-existing, retrieve it so that we can keep pre-existing values
                if (packageSources.TryGetValue(packageSourceContextInfo.OriginalHashCode, out PackageSource packageSource))
                {
                    // If Name/Source/IsEnabled has not changed, we don't need to do anything
                    if (packageSource.Name.Equals(packageSourceContextInfo.Name, StringComparison.InvariantCulture)
                        && packageSource.Source.Equals(packageSourceContextInfo.Source, StringComparison.InvariantCulture)
                        && packageSource.IsEnabled == packageSourceContextInfo.IsEnabled)
                    {
                        newPackageSources.Add(packageSource);
                    }
                    else
                    {
                        var newSource = new PackageSource(
                           packageSourceContextInfo.Source,
                           packageSourceContextInfo.Name,
                           packageSourceContextInfo.IsEnabled,
                           packageSource.IsOfficial,
                           packageSource.IsPersistable)
                        {
                            IsMachineWide = packageSourceContextInfo.IsMachineWide,
                            Credentials = packageSource.Credentials,
                            ClientCertificates = packageSource.ClientCertificates,
                            Description = packageSource.Description,
                            ProtocolVersion = packageSource.ProtocolVersion,
                            MaxHttpRequestsPerSource = packageSource.MaxHttpRequestsPerSource,
                        };

                        newPackageSources.Add(newSource);
                    }
                }
                else
                {
                    // New package source
                    var newSource = new PackageSource(
                           packageSourceContextInfo.Source,
                           packageSourceContextInfo.Name,
                           packageSourceContextInfo.IsEnabled)
                    {
                        IsMachineWide = packageSourceContextInfo.IsMachineWide,
                    };

                    newPackageSources.Add(newSource);
                }
            }

            return newPackageSources;
        }
    }
}
