// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A unique identifier generator.
    /// </summary>
    public sealed class RequestIdGenerator : IIdGenerator
    {
        /// <summary>
        /// Generates a new unique identifier.
        /// </summary>
        /// <returns>A unique identifier.</returns>
        public string GenerateUniqueId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
