// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.Configuration
{
    /// <summary>
    /// Provides a common facility for locating extensions
    /// </summary>
    public interface IExtensionLocator
    {
        /// <summary>
        /// Find paths to all extensions
        /// </summary>
        IEnumerable<string> FindExtensions();

        /// <summary>
        /// Find paths to all credential providers
        /// </summary>
        IEnumerable<string> FindCredentialProviders();
    }
}
