// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using NuGet.Common;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    public class PackageRestoreContext
    {
        public NuGetPackageManager PackageManager { get; }
        public IEnumerable<PackageRestoreData> Packages { get; }
        public CancellationToken Token { get; }
        public EventHandler<PackageRestoredEventArgs> PackageRestoredEvent { get; }
        public EventHandler<PackageRestoreFailedEventArgs> PackageRestoreFailedEvent { get; }
        public IEnumerable<SourceRepository> SourceRepositories { get; }
        public int MaxNumberOfParallelTasks { get; }
        public ILogger Logger { get; }
        public bool EnableNuGetAudit { get; }
        public Dictionary<string, RestoreAuditProperties> RestoreAuditProperties { get; }

        private static Dictionary<string, RestoreAuditProperties> EmptyDictionary = new Dictionary<string, RestoreAuditProperties>();
        public PackageRestoreContext(NuGetPackageManager nuGetPackageManager,
            IEnumerable<PackageRestoreData> packages,
            CancellationToken token,
            EventHandler<PackageRestoredEventArgs> packageRestoredEvent,
            EventHandler<PackageRestoreFailedEventArgs> packageRestoreFailedEvent,
            IEnumerable<SourceRepository> sourceRepositories,
            int maxNumberOfParallelTasks,
            ILogger logger) : this(nuGetPackageManager, packages, token, packageRestoredEvent, packageRestoreFailedEvent, sourceRepositories, maxNumberOfParallelTasks, false, EmptyDictionary, logger)
        {
        }

        public PackageRestoreContext(NuGetPackageManager nuGetPackageManager,
            IEnumerable<PackageRestoreData> packages,
            CancellationToken token,
            EventHandler<PackageRestoredEventArgs> packageRestoredEvent,
            EventHandler<PackageRestoreFailedEventArgs> packageRestoreFailedEvent,
            IEnumerable<SourceRepository> sourceRepositories,
            int maxNumberOfParallelTasks,
            bool enableNuGetAudit,
            Dictionary<string, RestoreAuditProperties> restoreAuditProperties,
            ILogger logger)
        {
            if (maxNumberOfParallelTasks <= 0)
            {
                throw new ArgumentException(Strings.ParameterCannotBeZeroOrNegative, nameof(maxNumberOfParallelTasks));
            }

            PackageManager = nuGetPackageManager ?? throw new ArgumentNullException(nameof(nuGetPackageManager));
            Packages = packages ?? throw new ArgumentNullException(nameof(packages));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Token = token;
            PackageRestoredEvent = packageRestoredEvent;
            PackageRestoreFailedEvent = packageRestoreFailedEvent;
            SourceRepositories = sourceRepositories;
            MaxNumberOfParallelTasks = maxNumberOfParallelTasks;
            EnableNuGetAudit = enableNuGetAudit;
            RestoreAuditProperties = restoreAuditProperties ?? throw new ArgumentNullException(nameof(restoreAuditProperties));
        }
    }
}
