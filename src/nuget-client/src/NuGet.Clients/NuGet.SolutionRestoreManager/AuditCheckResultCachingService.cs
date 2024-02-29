// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.ComponentModel.Composition;
using NuGet.PackageManagement;

namespace NuGet.SolutionRestoreManager
{

    /// <inheritdoc cref="IAuditCheckResultCachingService" />
    [Export(typeof(IAuditCheckResultCachingService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AuditCheckResultCachingService : IAuditCheckResultCachingService
    {
        /// <inheritdoc cref="IAuditCheckResultCachingService.HasAuditBeenCachedAtLeastOnce" />
        public bool HasAuditBeenCachedAtLeastOnce { get; private set; }
        private AuditCheckResult? _lastAuditCheckResult = null;

        /// <inheritdoc cref="IAuditCheckResultCachingService.LastAuditCheckResult" />
        public AuditCheckResult? LastAuditCheckResult
        {
            get => _lastAuditCheckResult;
            set
            {
                _lastAuditCheckResult = value;
                HasAuditBeenCachedAtLeastOnce = true;
            }
        }
    }
}
