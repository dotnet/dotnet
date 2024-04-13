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
        private readonly IPackageSourceProvider _packageSourceProvider;

        public event EventHandler<IReadOnlyList<PackageSourceContextInfo>>? PackageSourcesChanged;

        public NuGetSourcesService(
            ServiceActivationOptions options,
            IServiceBroker serviceBroker,
            AuthorizationServiceClient authorizationServiceClient,
            IPackageSourceProvider packageSourceProvider)
        {
            Assumes.NotNull(serviceBroker);
            Assumes.NotNull(authorizationServiceClient);
            Assumes.NotNull(packageSourceProvider);

            _options = options;
            _serviceBroker = serviceBroker;
            _authorizationServiceClient = authorizationServiceClient;
            _packageSourceProvider = packageSourceProvider;
            _packageSourceProvider.PackageSourcesChanged += PackageSourceProvider_PackageSourcesChanged;
        }

        public ValueTask<IReadOnlyList<PackageSourceContextInfo>> GetPackageSourcesAsync(CancellationToken cancellationToken)
        {
            return new ValueTask<IReadOnlyList<PackageSourceContextInfo>>(
                _packageSourceProvider
                .LoadPackageSources()
                .Select(PackageSourceContextInfo.Create)
                .ToList());
        }

        public ValueTask SavePackageSourceContextInfosAsync(IReadOnlyList<PackageSourceContextInfo> sources, CancellationToken cancellationToken)
        {
            IEnumerable<PackageSource> packageSources = GetPackageSourcesToUpdate(sources);
            _packageSourceProvider.SavePackageSources(packageSources);

            return new ValueTask();
        }

        public ValueTask<string?> GetActivePackageSourceNameAsync(CancellationToken cancellationToken)
        {
            return new ValueTask<string?>(_packageSourceProvider.ActivePackageSourceName);
        }

        public void Dispose()
        {
            _packageSourceProvider.PackageSourcesChanged -= PackageSourceProvider_PackageSourcesChanged;
            _authorizationServiceClient.Dispose();
            GC.SuppressFinalize(this);
        }

        private void PackageSourceProvider_PackageSourcesChanged(object sender, EventArgs e)
        {
            List<PackageSourceContextInfo> packageSources = _packageSourceProvider.LoadPackageSources().Select(PackageSourceContextInfo.Create).ToList();
            PackageSourcesChanged?.Invoke(this, packageSources);
        }

        private IReadOnlyList<PackageSource> GetPackageSourcesToUpdate(IReadOnlyList<PackageSourceContextInfo> packageSourceContextInfos)
        {
            Dictionary<string, PackageSource>? packageSources = _packageSourceProvider.LoadPackageSources()
                 .ToDictionary(packageSource => packageSource.Name, StringComparer.OrdinalIgnoreCase);

            var newPackageSources = new List<PackageSource>(capacity: packageSourceContextInfos.Count);

            foreach (PackageSourceContextInfo packageSourceContextInfo in packageSourceContextInfos)
            {
                // If package source is pre-existing, retrieve it so that we can keep pre-existing values
                if (packageSources.TryGetValue(packageSourceContextInfo.Name, out PackageSource packageSource))
                {
                    // If Name/Source/IsEnabled/ProtocolVersion has not changed, we don't need to do anything
                    if (packageSource.Name.Equals(packageSourceContextInfo.Name, StringComparison.InvariantCulture)
                        && packageSource.Source.Equals(packageSourceContextInfo.Source, StringComparison.InvariantCulture)
                        && packageSource.ProtocolVersion == packageSourceContextInfo.ProtocolVersion
                        && packageSource.AllowInsecureConnections == packageSourceContextInfo.AllowInsecureConnections
                        && packageSource.DisableTLSCertificateValidation == packageSourceContextInfo.DisableTLSCertificateValidation
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
                            ProtocolVersion = packageSourceContextInfo.ProtocolVersion,
                            AllowInsecureConnections = packageSourceContextInfo.AllowInsecureConnections,
                            DisableTLSCertificateValidation = packageSourceContextInfo.DisableTLSCertificateValidation,
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
                        ProtocolVersion = packageSourceContextInfo.ProtocolVersion,
                    };

                    newPackageSources.Add(newSource);
                }
            }

            return newPackageSources;
        }
    }
}
