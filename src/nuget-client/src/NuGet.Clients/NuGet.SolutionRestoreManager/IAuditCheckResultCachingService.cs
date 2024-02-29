// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using NuGet.PackageManagement;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// A service that helps us cache the result of the last audit check.
    /// This is only used for packages.config scenarios. Packages.config restores are package based, and as such there's 1 restore for all projects.
    /// This service helps us avoid the performance cost of running the audit check on the same package within the same Visual Studio session.
    /// It helps us keep the no-op cost for packages.config restore negligible.
    /// </summary>
    public interface IAuditCheckResultCachingService
    {
        /// <summary>
        /// The result of the last audit check if any.
        /// </summary>
        AuditCheckResult? LastAuditCheckResult { get; set; }

        /// <summary>
        /// Whether the audit check has been cached at least once.
        /// </summary>
        public bool HasAuditBeenCachedAtLeastOnce { get; }
    }
}
