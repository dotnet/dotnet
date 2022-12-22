// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class ExperimentUtility
    {
        public static AsyncLazy<bool> IsTransitiveOriginExpEnabled { get; private set; }

        static ExperimentUtility()
        {
            ResetAsyncValues();
        }

        // for testing purposes
        internal static void ResetAsyncValues()
        {
            IsTransitiveOriginExpEnabled = new(() => IsExperimentEnabledAsync(ExperimentationConstants.TransitiveDependenciesInPMUI), NuGetUIThreadHelper.JoinableTaskFactory);
        }

        internal static async Task<bool> IsExperimentEnabledAsync(ExperimentationConstants exp)
        {
            bool isExpEnabled;
            try
            {
                var svc = await ServiceLocator.GetComponentModelServiceAsync<INuGetExperimentationService>();
                isExpEnabled = svc?.IsExperimentEnabled(exp) ?? false;
            }
            catch (ServiceUnavailableException)
            {
                isExpEnabled = false;
            }

            return isExpEnabled;
        }
    }
}
