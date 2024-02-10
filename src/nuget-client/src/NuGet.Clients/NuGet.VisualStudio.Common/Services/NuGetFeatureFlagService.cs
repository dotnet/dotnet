// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using NuGet.Common;

namespace NuGet.VisualStudio
{
    [Export(typeof(INuGetFeatureFlagService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NuGetFeatureFlagService : INuGetFeatureFlagService
    {
        private readonly IEnvironmentVariableReader _environmentVariableReader;
        private readonly IAsyncServiceProvider _asyncServiceProvider;
        private readonly Microsoft.VisualStudio.Threading.AsyncLazy<IVsFeatureFlags> _ivsFeatureFlags;
        private readonly Dictionary<string, bool> _featureFlagCache;

        [ImportingConstructor]
        public NuGetFeatureFlagService()
            : this(EnvironmentVariableWrapper.Instance, AsyncServiceProvider.GlobalProvider)
        {
            // ensure uniqueness.
        }

        internal NuGetFeatureFlagService(IEnvironmentVariableReader environmentVariableReader, IAsyncServiceProvider asyncServiceProvider)
        {
            _environmentVariableReader = environmentVariableReader ?? throw new ArgumentNullException(nameof(environmentVariableReader));
            _asyncServiceProvider = asyncServiceProvider ?? throw new ArgumentNullException(nameof(asyncServiceProvider));
            _ivsFeatureFlags = new(() => _asyncServiceProvider.GetServiceAsync<SVsFeatureFlags, IVsFeatureFlags>(), NuGetUIThreadHelper.JoinableTaskFactory);
            _featureFlagCache = new();
        }

        public async Task<bool> IsFeatureEnabledAsync(NuGetFeatureFlagConstants featureFlag)
        {
            GetEnvironmentVariablesForFeature(featureFlag, out bool isFeatureForcedEnabled, out bool isFeatureForcedDisabled);
            // Perform the check from the feature flag service only once.
            // There are events sent for targetted notification changes, but we don't listen to those at this point.
            if (!_featureFlagCache.TryGetValue(featureFlag.Name, out bool featureEnabled))
            {
                var featureFlagService = await _ivsFeatureFlags.GetValueAsync();
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                if (!_featureFlagCache.TryGetValue(featureFlag.Name, out featureEnabled))
                {
                    featureEnabled = featureFlagService.IsFeatureEnabled(featureFlag.Name, defaultValue: featureFlag.DefaultState);
                    _featureFlagCache[featureFlag.Name] = featureEnabled;
                }
            }
            return !isFeatureForcedDisabled && (isFeatureForcedEnabled || featureEnabled);
        }

        private void GetEnvironmentVariablesForFeature(NuGetFeatureFlagConstants featureFlag, out bool isFeatureForcedEnabled, out bool isFeatureForcedDisabled)
        {
            isFeatureForcedEnabled = false;
            isFeatureForcedDisabled = false;
            if (!string.IsNullOrEmpty(featureFlag.EnvironmentVariable))
            {
                string envVarOverride = _environmentVariableReader.GetEnvironmentVariable(featureFlag.EnvironmentVariable);

                isFeatureForcedDisabled = envVarOverride == "0";
                isFeatureForcedEnabled = envVarOverride == "1";
            }
        }
    }
}
