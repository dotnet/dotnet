// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Feed specific state of current search operation to retrieve search results.
    /// Used for polling results of prolonged search. Opaque to external consumer.
    /// </summary>
    public class RefreshToken
    {
        public TimeSpan RetryAfter { get; set; }
    }
}
