// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.UI
{
    // IconBitmapStatusUtility has some helper methods that will need to be modified if we add new enum values.
    public enum IconBitmapStatus
    {
        None = 0,
        NeedToFetch,
        Fetching,
        MemoryCachedIcon,
        FetchedIcon,
        DefaultIcon,
        DefaultIconDueToDecodingError,
        DefaultIconDueToNullStream,
        DefaultIconDueToRelativeUri
    }
}
