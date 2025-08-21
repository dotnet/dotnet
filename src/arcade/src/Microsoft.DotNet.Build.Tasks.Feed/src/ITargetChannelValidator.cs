// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.DotNet.Build.Tasks.Feed.Model;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public enum TargetChannelValidationResult
    {
        Success,
        AuditOnlyFailure,
        Fail
    }

    public enum ValidationMode
    {
        /// <summary>
        /// In audit mode, validation failures are reported as AuditOnlyFailure instead of Fail.
        /// This allows builds to be published but with warnings about validation issues.
        /// </summary>
        Audit,
        
        /// <summary>
        /// In enforce mode, validation failures are reported as Fail, preventing builds from being published.
        /// </summary>
        Enforce
    }

    /// <summary>
    /// Interface for validating whether a build can be published to production channels.
    /// </summary>
    public interface ITargetChannelValidator
    {
        /// <summary>
        /// Validates whether the build can be published to the specified target channel.
        /// </summary>
        /// <param name="build">The build information from BAR</param>
        /// <param name="targetChannel">The target channel the build will be published to</param>
        /// <returns>ValidationResult indicating the outcome of the validation</returns>
        Task<TargetChannelValidationResult> ValidateAsync(ProductConstructionService.Client.Models.Build build, TargetChannelConfig targetChannel);
    }
}
