// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// Positions to base providers on
    /// </summary>
    public sealed class NuGetResourceProviderPositions
    {
        /// <summary>
        /// The first provider called
        /// </summary>
        public const string First = "First";

        /// <summary>
        /// The last provider called
        /// </summary>
        public const string Last = "Last";
    }
}
