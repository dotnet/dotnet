// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Packaging
{
    public class PackageSigningTelemetryEvent : TelemetryEvent
    {
        public PackageSignType PackageSignType => (PackageSignType)(base[nameof(PackageSignType)]!);

        public NuGetOperationStatus Status => (NuGetOperationStatus)(base[nameof(Status)]!);

        [Obsolete("This seems to be unused and will be removed in a future version.")]
        public string? ExtractionId => base[nameof(ExtractionId)] as string;

        public const string EventName = "SigningInformation";

        public PackageSigningTelemetryEvent() :
            base(EventName)
        {
        }

        public void SetResult(PackageSignType packageSignType, NuGetOperationStatus status)
        {
            base[nameof(PackageSignType)] = packageSignType;
            base[nameof(Status)] = status;
        }
    }
}
