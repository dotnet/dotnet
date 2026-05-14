// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Enum for the type of NuGetAction
    /// </summary>
    public enum NuGetActionType
    {
        /// <summary>
        /// Install
        /// </summary>
        Install,

        /// <summary>
        /// Uninstall
        /// </summary>
        Uninstall,

        /// <summary>
        /// Reinstall
        /// </summary>
        Reinstall,

        /// <summary>
        /// Update
        /// </summary>
        Update,

        /// <summary>
        /// UpdateAll
        /// </summary>
        UpdateAll
    }
}
