// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging
{
    public class PackageExtractionResult
    {
        public bool Cached { get; }

        public TimeSpan SignVerifyDelay { get; }

        public PackageSignType PackageType { get; }

        public bool Success { get; }

        public TimeSpan Duration { get; set; }

        public DateTimeOffset SignVerifyStartTime { get; }

        public DateTimeOffset SignVerifyEndTime { get; }


        public PackageExtractionResult(
            bool cached,
            TimeSpan signVerifyDelay,
            PackageSignType packageType,
            bool success,
            DateTimeOffset signVerifyStartTime,
            DateTimeOffset signVerifyEndTime) :
            this(cached, signVerifyDelay, packageType, success, TimeSpan.Zero)
        {
        }


        public PackageExtractionResult(bool cached, TimeSpan signVerifyDelay, PackageSignType packageType, bool success, TimeSpan duration)
        {
            Cached = cached;
            SignVerifyDelay = signVerifyDelay;
            PackageType = packageType;
            Success = success;
            Duration = duration;
        }
    }
}
