// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class JobStatus
    {
        public bool IsCompleted => string.Equals(Status, "finished", StringComparison.OrdinalIgnoreCase)
                                    || string.Equals(Status, "failed", StringComparison.OrdinalIgnoreCase);
    }
}
