// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Common
{
    /// <summary> Define different states for NuGet operation status. </summary>
    public enum NuGetOperationStatus
    {
        /// <summary> No operation performed. </summary>
        NoOp = 0,

        /// <summary> Operation was successful. </summary>
        Succeeded = 1,

        /// <summary> Operation failed. </summary>
        Failed = 2,

        /// <summary> Operation was cancelled. </summary>
        Cancelled = 3
    }
}
