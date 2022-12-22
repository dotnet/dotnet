// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.PackageManagement.Telemetry
{
    /// <summary>
    /// Exception used only for telemetry.
    /// </summary>
    [Serializable]
    public class CacheMissException : Exception
    {
    }
}
