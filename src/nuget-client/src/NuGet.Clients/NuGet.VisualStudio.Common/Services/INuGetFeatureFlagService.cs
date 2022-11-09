// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// A service that can check whether a feature enabled through the VS Feature Flag service.
    /// The results from the VS Feature Flag service are cached and only calculated the first time the check is performed.
    /// This service does not listen to the updates of the VS Feature Flag service.
    /// </summary>
    public interface INuGetFeatureFlagService
    {
        /// <summary>
        /// Determines whether a feature has been enabled.
        /// </summary>
        /// <param name="featureFlag">The feature flag info.</param>
        /// <returns>Whether the feature is enabled.</returns>
        Task<bool> IsFeatureEnabledAsync(NuGetFeatureFlagConstants featureFlag);
    }
}
