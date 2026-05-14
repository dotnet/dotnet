// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Define different sources to trigger nuget action.
    /// </summary>
    public enum OperationSource
    {
        /// <summary>
        /// When nuget action is trigger from Package Management Console.
        /// </summary>
        PMC = 0,

        /// <summary>
        /// When nuget action is trigger from Nuget Manager UI.
        /// </summary>
        UI = 1,

        /// <summary>
        /// When nuget action is trigger from nuget public api.
        /// </summary>
        API = 2,
    }
}
