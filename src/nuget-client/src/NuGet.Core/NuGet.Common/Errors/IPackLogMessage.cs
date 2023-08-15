// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Frameworks;

namespace NuGet.Common
{
    public interface IPackLogMessage : INuGetLogMessage
    {
        /// <summary>
        /// Project or Package Id.
        /// </summary>
        string? LibraryId { get; set; }

        /// <summary>
        /// NuGet Framework
        /// </summary>
        public NuGetFramework? Framework { get; set; }
    }
}
