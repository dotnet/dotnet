// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    public enum SourceValidity
    {
        /// <summary>
        /// No specific source was selected.
        /// </summary>
        None,

        /// <summary>
        /// The provided source string is valid.
        /// </summary>
        Valid,

        /// <summary>
        /// The source could not be found.
        /// </summary>
        UnknownSource,

        /// <summary>
        /// The source is an invalid type.
        /// </summary>
        UnknownSourceType
    }
}
