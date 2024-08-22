// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.Packaging
{
    public class PackageExtractionTelemetryEvent : TelemetryEvent
    {
        public const string EventName = "PackageExtractionInformation";

        public PackageSaveMode PackageSaveMode => (PackageSaveMode)base[nameof(PackageSaveMode)];

        public NuGetOperationStatus Status => (NuGetOperationStatus)base[nameof(Status)];

        public ExtractionSource ExtractionSource => (ExtractionSource)base[nameof(ExtractionSource)];

        public string PackageId => (string)base[nameof(PackageId)];
        public string PackageVersion => (string)base[nameof(PackageVersion)];

        public PackageExtractionTelemetryEvent(
            PackageSaveMode packageSaveMode,
            NuGetOperationStatus status,
            ExtractionSource extractionSource,
            PackageIdentity packageId = null) :
            base(EventName, new Dictionary<string, object>
                {
                    { nameof(Status), status },
                    { nameof(ExtractionSource), extractionSource },
                    { nameof(PackageSaveMode), packageSaveMode }
                })
        {
            if (packageId != null)
            {
                LogPackageIdentity(packageId);
            }
        }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Matching existing code and existing logged telemetry.")]
        public void LogPackageIdentity(PackageIdentity packageId)
        {
            AddPiiData(nameof(PackageId), packageId.Id.ToLowerInvariant());
            AddPiiData(nameof(PackageVersion), packageId.Version.ToNormalizedString().ToLowerInvariant());
        }

        public void SetResult(NuGetOperationStatus status)
        {
            base[nameof(Status)] = status;
        }
    }
}
